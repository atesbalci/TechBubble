using System;
using TechBubble.Behaviors;
using TMPro;
using UnityEngine;

namespace TechBubble.Views
{
    public class DeadlineScreenIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private TMP_Text text;
        
        private RectTransform RectTransform => (RectTransform) transform;
        
        private DeadlineBehavior _deadlineBehavior;
        private Vector2 _canvasSize;
        private Camera _camera;

        public void Bind(DeadlineBehavior deadlineBehavior)
        {
            _deadlineBehavior = deadlineBehavior;
        }

        private void Start()
        {
            _canvasSize = ((RectTransform)transform.parent).rect.size;
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector2 viewportPoint = _camera.WorldToViewportPoint(_deadlineBehavior.transform.position);
            viewportPoint.x = Mathf.Clamp01(viewportPoint.x);
            viewportPoint.y = Mathf.Clamp01(viewportPoint.y);
            var pos = viewportPoint * _canvasSize;
            var indicatorHalfSize = RectTransform.sizeDelta * 0.5f;
            pos.x = Mathf.Clamp(pos.x, indicatorHalfSize.x, _canvasSize.x - indicatorHalfSize.x);
            pos.y = Mathf.Clamp(pos.y, indicatorHalfSize.y, _canvasSize.y - indicatorHalfSize.y);
            RectTransform.anchoredPosition = pos;
            RefreshArrow(viewportPoint);

            text.text = $"${_deadlineBehavior.Money:N0}\n" +
                        $"{_deadlineBehavior.DeadlineTime - Time.time:F1}";
        }

        private void RefreshArrow(Vector2 viewportPosition)
        {
            float rotation;
            if (viewportPosition.x < 0.001f)
            {
                rotation = 90f;
            }
            else if (viewportPosition.x > 0.999f)
            {
                rotation = 270f;
            }
            else if (viewportPosition.y < 0.001f)
            {
                rotation = 180f;
            }
            else if (viewportPosition.y > 0.999f)
            {
                rotation = 0f;
            }
            else
            {
                arrow.gameObject.SetActive(false);
                return;
            }
            
            arrow.gameObject.SetActive(true);
            arrow.localEulerAngles = new Vector3(0f, 0f, rotation);
        }
    }
}