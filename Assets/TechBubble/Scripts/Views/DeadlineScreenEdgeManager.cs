using System;
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
        private IDeadlinesProvider _deadlinesProvider;
        private Camera _camera;
        private Vector2 _canvasSize;

        [Inject]
        public void Initialize(IDeadlinesProvider deadlinesProvider)
        {
            _spawnedIndicators = new Dictionary<DeadlineBehavior, DeadlineScreenIndicator>();
            _deadlinesProvider = deadlinesProvider;
            _camera = Camera.main;
            templateIndicator.gameObject.SetActive(false);
        }

        private void Start()
        {
            _canvasSize = ((RectTransform)transform).rect.size;
        }

        private void Update()
        {
            foreach (var deadline in _deadlinesProvider.Deadlines)
            {
                Vector2 viewportPoint = _camera.WorldToViewportPoint(deadline.transform.position);
                if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
                {
                    if (!_spawnedIndicators.TryGetValue(deadline, out var indicator))
                    {
                        indicator = Instantiate(templateIndicator, transform, false);
                        _spawnedIndicators.Add(deadline, indicator);
                        indicator.gameObject.SetActive(true);
                    }
                    
                    viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
                    viewportPoint.y = Mathf.Clamp01(viewportPoint.y);
                    indicator.RectTransform.anchoredPosition = viewportPoint * _canvasSize;
                    indicator.RectTransform.pivot = viewportPoint;
                    indicator.RefreshViewportPosition(viewportPoint);
                }
                else
                {
                    if (_spawnedIndicators.TryGetValue(deadline, out var indicator))
                    {
                        Destroy(indicator.gameObject);
                        _spawnedIndicators.Remove(deadline);
                    }
                }
            }
        }
    }
}