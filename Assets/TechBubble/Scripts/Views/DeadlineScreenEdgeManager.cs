using System.Collections.Generic;
using TechBubble.Behaviors;
using UnityEngine;
using Zenject;

namespace TechBubble.Views
{
    public class DeadlineScreenEdgeManager : MonoBehaviour
    {
        [SerializeField] private DeadlineScreenIndicator templateIndicator;
        
        private IDictionary<DeadlineBehavior, DeadlineScreenIndicator> _spawnedIndicators;

        [Inject]
        public void Initialize(IDeadlinesProvider deadlinesProvider)
        {
            _spawnedIndicators = new Dictionary<DeadlineBehavior, DeadlineScreenIndicator>();
            templateIndicator.gameObject.SetActive(false);
            deadlinesProvider.OnDeadlineCreated += OnDeadlineCreated;
            deadlinesProvider.OnDeadlineReached += OnDeadlineDestroyed;
            deadlinesProvider.OnDeadlineFailed += OnDeadlineDestroyed;
        }

        private void OnDeadlineCreated(DeadlineBehavior deadlineBehavior)
        {
            if (!_spawnedIndicators.ContainsKey(deadlineBehavior))
            {
                var indicator = Instantiate(templateIndicator, transform, false);
                _spawnedIndicators.Add(deadlineBehavior, indicator);
                indicator.gameObject.SetActive(true);
                indicator.Bind(deadlineBehavior);
            }
        }

        private void OnDeadlineDestroyed(DeadlineBehavior deadlineBehavior)
        {
            if (_spawnedIndicators.Remove(deadlineBehavior, out var indicator))
            {
                Destroy(indicator.gameObject);
            }
        }
    }
}