using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TechBubble.Behaviors;
using TechBubble.Models;
using TechBubble.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TechBubble.Controllers
{
    public class GameController : IGameState, IDeadlinesProvider, IDisposable
    {
        public event DeadlineEvent OnDeadlineCreated;
        public event DeadlineEvent OnDeadlineReached;
        public event DeadlineEvent OnDeadlineFailed;
        public event Action OnGameOver;
        
        public long Money => _playerBehaviour.Money;
        public bool IsGameOver => Money <= 0;
        public int InvestmentsSurvived { get; private set; }

        private readonly IGameRules _gameRules;
        private readonly InvestmentPool _investmentPool;
        private readonly DeadlinePool _deadlinePool;
        private readonly PlayerBehaviour _playerBehaviour;
        private readonly IAssholeInvestorAnimator _assholeInvestorAnimator;
        private readonly IList<InvestmentBehavior> _spawnedInvestmentBehaviors;
        private readonly IList<DeadlineBehavior> _spawnedDeadlineBehaviors;
        private readonly CancellationTokenSource _cancellationToken;

        public GameController(IGameRules gameRules, InvestmentPool investmentPool, PlayerBehaviour playerBehaviour,
            DeadlinePool deadlinePool, IAssholeInvestorAnimator assholeInvestorAnimator)
        {
            _spawnedInvestmentBehaviors = new List<InvestmentBehavior>();
            _spawnedDeadlineBehaviors = new List<DeadlineBehavior>();
            _gameRules = gameRules;
            _investmentPool = investmentPool;
            _deadlinePool = deadlinePool;
            _playerBehaviour = playerBehaviour;
            _assholeInvestorAnimator = assholeInvestorAnimator;
            _playerBehaviour.OnPickupableBehaviourCollision += OnPickupableCollision;
            playerBehaviour.Speed = 5;
            playerBehaviour.Money = _gameRules.StartMoney;
            _cancellationToken = new CancellationTokenSource();
            _ = GameLoop(_cancellationToken.Token);
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
        }

        private async Task GameLoop(CancellationToken cancellationToken)
        {
            await Task.WhenAny(InvestmentSpawnLoop(cancellationToken), SpendingLoop(cancellationToken),
                DeadlinesLoop(cancellationToken));
            _playerBehaviour.LockInput();
            OnGameOver?.Invoke();
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
                        _ = SpawnDeadlineBehavior(investmentBehavior.InvestmentData, _cancellationToken.Token);
                        _playerBehaviour.Money += investmentBehavior.Money;
                    }

                    break;
                case DeadlineBehavior deadlineBehavior:
                    _spawnedDeadlineBehaviors.Remove(deadlineBehavior);
                    OnDeadlineReached?.Invoke(deadlineBehavior);
                    InvestmentsSurvived++;
                    pickupable.Consume(_playerBehaviour.transform, () =>
                    {
                        _deadlinePool.Despawn(deadlineBehavior);
                    });
                    break;
            }
        }

        private async Task SpendingLoop(CancellationToken cancellationToken)
        {
            while (!IsGameOver)
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
            while (!IsGameOver)
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

        private async Task SpawnDeadlineBehavior(InvestmentData investmentData, CancellationToken cancellationToken)
        {
            var deadline = _deadlinePool.Spawn();
            _spawnedDeadlineBehaviors.Add(deadline);
            Vector2 playerPos = _playerBehaviour.transform.position;
            bool isAsshole = Random.value < _gameRules.DeadlineLongDistPossibility;
            float dist = isAsshole
                ? _gameRules.DeadlineLongDist
                : Random.Range(_gameRules.DeadlineMinDist, _gameRules.DeadlineMaxDist);
            var pos = playerPos + dist * Random.insideUnitCircle.normalized;
            deadline.Initialize(investmentData, pos);
            OnDeadlineCreated?.Invoke(deadline);

            if (isAsshole)
            {
                Time.timeScale = 0f;
                await _assholeInvestorAnimator.ShowAnimation(investmentData, cancellationToken);
                Time.timeScale = 1f;
            }
        }

        private async Task DeadlinesLoop(CancellationToken cancellationToken)
        {
            while (!IsGameOver)
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