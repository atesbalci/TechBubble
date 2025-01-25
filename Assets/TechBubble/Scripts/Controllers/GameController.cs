using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechBubble.Behaviors;
using TechBubble.Models;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace TechBubble.Controllers
{
    public class GameController : IGameState, IDeadlinesProvider
    {
        public long Money { get; private set; }
        public ICollection<DeadlineBehavior> Deadlines => _spawnedDeadlineBehaviors;
        
        private readonly IGameRules _gameRules;
        private readonly InvestmentPool _investmentPool;
        private readonly DeadlinePool _deadlinePool;
        private readonly PlayerBehaviour _playerBehaviour;
        private readonly IList<InvestmentBehavior> _spawnedInvestmentBehaviors;
        private readonly IList<DeadlineBehavior> _spawnedDeadlineBehaviors;

        public GameController(IGameRules gameRules, InvestmentPool investmentPool, PlayerBehaviour playerBehaviour,
            DeadlinePool deadlinePool)
        {
            _spawnedInvestmentBehaviors = new List<InvestmentBehavior>();
            _spawnedDeadlineBehaviors = new List<DeadlineBehavior>();
            _gameRules = gameRules;
            _investmentPool = investmentPool;
            _deadlinePool = deadlinePool;
            _playerBehaviour = playerBehaviour;
            _playerBehaviour.OnPickupableBehaviourCollision += OnPickupableCollision;
            playerBehaviour.Speed = 5;
            _ = InvestmentSpawnLoop(CancellationToken.None);
        }

        private void OnPickupableCollision(PickupableBehaviour pickupable)
        {
            switch (pickupable)
            {
                case InvestmentBehavior investmentBehavior:
                    pickupable.Consume(_playerBehaviour.transform, () =>
                    {
                        _investmentPool.Despawn(investmentBehavior);
                        _spawnedInvestmentBehaviors.Remove(investmentBehavior);
                    });
                    SpawnDeadlineBehavior();
                    break;
                case DeadlineBehavior deadlineBehavior:
                    pickupable.Consume(_playerBehaviour.transform, () =>
                    {
                        _deadlinePool.Despawn(deadlineBehavior);
                        _spawnedDeadlineBehaviors.Remove(deadlineBehavior);
                    });
                    break;
            }
        }

        private async Task InvestmentSpawnLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                await Awaitable.WaitForSecondsAsync(3f, cancellationToken);
                float despawnDistSq = _gameRules.DespawnRadius * _gameRules.DespawnRadius;
                Vector2 playerPos = _playerBehaviour.transform.position;
                for (int i = _spawnedInvestmentBehaviors.Count - 1; i >= 0; i--)
                {
                    var investmentBehavior = _spawnedInvestmentBehaviors[i];
                    if ((playerPos - (Vector2)investmentBehavior.transform.position).sqrMagnitude > despawnDistSq)
                    {
                        _investmentPool.Despawn(investmentBehavior);
                        _spawnedInvestmentBehaviors.RemoveAt(i);
                    }
                }

                if (_spawnedInvestmentBehaviors.Count < _gameRules.MaxSpawnedInvestmentCount &&
                    _playerBehaviour.MovementDirection.sqrMagnitude > 0.001f)
                {
                    var angle = (Vector2.SignedAngle(Vector2.up, _playerBehaviour.MovementDirection) +
                                Random.Range(-45f, 45f)) * Mathf.Deg2Rad;
                    var pos = playerPos + new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle)) * _gameRules.SpawnRadius;
                    SpawnInvestmentBehavior(pos);
                }
            }
        }

        private void SpawnInvestmentBehavior(Vector2 spawnPos)
        {
            var investment = _investmentPool.Spawn();
            investment.transform.position = spawnPos;
            _spawnedInvestmentBehaviors.Add(investment);
        }

        private void SpawnDeadlineBehavior()
        {
            var deadline = _deadlinePool.Spawn();
            _spawnedDeadlineBehaviors.Add(deadline);
            Vector2 playerPos = _playerBehaviour.transform.position;
            var pos = playerPos + Random.Range(_gameRules.DeadlineMinDist, _gameRules.DeadlineMaxDist) * Random.insideUnitCircle.normalized;
            deadline.Initialize(0f, pos);
        }
    }
}