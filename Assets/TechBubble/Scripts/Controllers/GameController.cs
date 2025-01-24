using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechBubble.Behaviors;
using TechBubble.Models;
using UnityEngine;

namespace TechBubble.Controllers
{
    public class GameController
    {
        private readonly IGameRules _gameRules;
        private readonly InvestmentPool _investmentPool;
        private readonly PlayerBehaviour _playerBehaviour;
        private readonly IList<InvestmentBehavior> _spawnedInvestmentBehaviors;

        public GameController(IGameRules gameRules, InvestmentPool investmentPool, PlayerBehaviour playerBehaviour)
        {
            _spawnedInvestmentBehaviors = new List<InvestmentBehavior>();
            _gameRules = gameRules;
            _investmentPool = investmentPool;
            _playerBehaviour = playerBehaviour;
            _ = GameLoop(CancellationToken.None);
        }

        private async Task GameLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                await Awaitable.WaitForSecondsAsync(3f, cancellationToken);
                float despawnDistSq = _gameRules.DespawnRadius * _gameRules.DespawnRadius;
                Vector2 playerPos = _playerBehaviour.transform.position;
                for (int i = _spawnedInvestmentBehaviors.Count - 1; i >= 0; i--)
                {
                    var investmentBehavior = _spawnedInvestmentBehaviors[i];
                    if ((playerPos - (Vector2)investmentBehavior.transform.position).sqrMagnitude < despawnDistSq)
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
                    var pos = playerPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
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
    }
}