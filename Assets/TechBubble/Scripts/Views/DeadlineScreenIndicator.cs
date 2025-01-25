using System;
using DG.Tweening;
using TechBubble.Behaviors;
using TMPro;
using UnityEngine;

namespace TechBubble.Views
{
    public class DeadlineScreenIndicator : MonoBehaviour
    {
        [SerializeField] private RectTransform arrow;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text timerText;
        
        private RectTransform RectTransform => (RectTransform) transform;
        
        private DeadlineBehavior _deadlineBehavior;
        private Vector2 _canvasSize;
        private Camera _camera;
        private Tween _suspenseTween;

        public void Bind(DeadlineBehavior deadlineBehavior)
        {
            _deadlineBehavior = deadlineBehavior;
            moneyText.text = $"${_deadlineBehavior.Money:N0}";
            _suspenseTween.Kill();
            _suspenseTween = null;
            timerText.transform.localScale = Vector3.one;
            timerText.color = Color.white;
        }

        private void Start()
        {
            _canvasSize = ((RectTransform)transform.parent).rect.size;
            _camera = Camera.main;
        }

        private void OnDisable()
        {
            _suspenseTween.Kill();
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

            var remainingTime = _deadlineBehavior.DeadlineTime - Time.time;
            timerText.text = $"{remainingTime:F1}";
            if (remainingTime < 5f && _suspenseTween == null)
            {
                _suspenseTween = DOTween.Sequence()
                    .Append(timerText.transform.DOScale(1.5f, 0.5f))
                    .Join(timerText.DOColor(new Color(1f, 0.4f, 0.38f), 0.5f))
                    .Append(timerText.transform.DOScale(1, 0.5f))
                    .Join(timerText.DOColor(Color.white, 0.5f))
                    .SetLoops(-1);
            }
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