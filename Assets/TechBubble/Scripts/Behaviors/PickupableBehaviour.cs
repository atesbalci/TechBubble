using System;
using DG.Tweening;
using UnityEngine;

namespace TechBubble.Behaviors
{
    public class PickupableBehaviour : MonoBehaviour
    {
        private Tween _tween;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _tween.Kill();
            _collider.enabled = true;
            transform.localScale = Vector3.one;
        }
        

        public void Consume(Transform target, Action despawnAction)
        {
            _tween.Kill();
            _tween = DOTween.Sequence()
                .Append(transform.DOMove(target.position, 0.5f))
                .Join(transform.DOScale(0f, 0.5f))
                .AppendCallback(() => despawnAction?.Invoke());
        }
    }
}