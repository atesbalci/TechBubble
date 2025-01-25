using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechBubble.Behaviors;
using TechBubble.Models;
using UnityEngine;

namespace TechBubble.Controllers
{
    public class GameController : IGameState, IDeadlinesProvider
    {
        public event IDeadlinesProvider.DeadlineEvent OnDeadlineCreated;
        public event IDeadlinesProvider.DeadlineEvent OnDeadlineReached;
        public event IDeadlinesProvider.DeadlineEvent OnDeadlineFailed;
        
        public long Money { get; private set; }
        
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
            playerBehaviour.Money = _gameRules.StartMoney;
            _ = InvestmentSpawnLoop(CancellationToken.None);
            _ = SpendingLoop(CancellationToken.None);
            _ = DeadlinesLoop(CancellationToken.None);
        }

        private void OnPickupableCollision(PickupableBehaviour pickupable)
        {
            switch (pickupable)
            {
                case InvestmentBehavior investmentBehavior:
                    if (_playerBehaviour.Money >= investmentBehavior.Money)
                    {
                        pickupable.Consume(_playerBehaviour.transform, () =>
                        {
                            _investmentPool.Despawn(investmentBehavior);
                            _spawnedInvestmentBehaviors.Remove(investmentBehavior);
                        });
                        SpawnDeadlineBehavior(investmentBehavior.InvestmentData);
                        _playerBehaviour.Money += investmentBehavior.Money;
                    }

                    break;
                case DeadlineBehavior deadlineBehavior:
                    _spawnedDeadlineBehaviors.Remove(deadlineBehavior);
                    OnDeadlineReached?.Invoke(deadlineBehavior);
                    pickupable.Consume(_playerBehaviour.transform, () =>
                    {
                        _deadlinePool.Despawn(deadlineBehavior);
                    });
                    break;
            }
        }

        private async Task SpendingLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                var newMoney = _playerBehaviour.Money - Mathf.RoundToInt(_gameRules.MoneyLossPerDistance * Time.deltaTime *
                                                           _playerBehaviour.MovementDirection.magnitude *
                                                           _playerBehaviour.Speed);
                _playerBehaviour.Money = newMoney;
                await Awaitable.NextFrameAsync(cancellationToken);
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
            investment.InvestmentData =
                _gameRules.InvestorPossibilities[Random.Range(0, _gameRules.InvestorPossibilities.Length)];
            investment.transform.position = spawnPos;
            _spawnedInvestmentBehaviors.Add(investment);
        }

        private void SpawnDeadlineBehavior(InvestmentData investmentData)
        {
            var deadline = _deadlinePool.Spawn();
            _spawnedDeadlineBehaviors.Add(deadline);
            Vector2 playerPos = _playerBehaviour.transform.position;
            var pos = playerPos + Random.Range(_gameRules.DeadlineMinDist, _gameRules.DeadlineMaxDist) * Random.insideUnitCircle.normalized;
            deadline.Initialize(investmentData, pos);
            OnDeadlineCreated?.Invoke(deadline);
        }

        private async Task DeadlinesLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                for (int i = _spawnedDeadlineBehaviors.Count - 1; i >= 0; i--)
                {
                    var deadlineBehavior = _spawnedDeadlineBehaviors[i];
                    if (deadlineBehavior.DeadlineTime < Time.time)
                    {
                        _spawnedDeadlineBehaviors.RemoveAt(i);
                        _deadlinePool.Despawn(deadlineBehavior);
                        _playerBehaviour.Money -= deadlineBehavior.Money;
                        OnDeadlineFailed?.Invoke(deadlineBehavior);
                    }
                }
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
    }
}