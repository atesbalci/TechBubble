using DG.Tweening;
using TechBubble.Behaviors;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TechBubble.Views
{
    public class InvestorCollectionView : MonoBehaviour
    {
        private Tween _tween;
        private IViewData _viewData;

        [Inject]
        public void Initialize(IDeadlinesProvider deadlinesProvider, IViewData viewData)
        {
            _viewData = viewData;
            deadlinesProvider.OnDeadlineReached += OnDeadlineReached;
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }

        private void OnDeadlineReached(DeadlineBehavior deadlineBehavior)
        {
            var investor = new GameObject().AddComponent<Image>();
            investor.transform.SetParent(transform);
            investor.rectTransform.sizeDelta = new Vector2(150f, 150f);
            investor.transform.localScale = 2f * Vector3.one;
            investor.transform.rotation = Quaternion.identity;
            investor.sprite = _viewData.GetInvestorIcon(deadlineBehavior.InvestmentData.Id);
            investor.color = new Color(1f, 1f, 1f, 0f);
            _tween.Complete();
            _tween = DOTween.Sequence()
                .Append(investor.DOFade(1f, 1f))
                .Join(investor.transform.DOScale(1f, 1f));
        }
    }
}