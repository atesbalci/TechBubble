using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using TechBubble.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TechBubble.Views
{
    public class AssholeInvestorView : MonoBehaviour, IAssholeInvestorAnimator
    {
        [SerializeField] private Image background;
        [SerializeField] private Image strip;
        [SerializeField] private Image investorImage;
        [SerializeField] private TMP_Text infoText;
        
        private Tween _tween;
        private IViewData _viewData;

        [Inject]
        public void Initialize(IViewData viewData)
        {
            _viewData = viewData;
            gameObject.SetActive(false);
        }

        public Task ShowAnimation(InvestmentData investmentData, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            _tween.Kill();
            investorImage.sprite = _viewData.GetInvestorIcon(investmentData.Id);
            gameObject.SetActive(true);
            background.color = Color.clear;
            investorImage.transform.localScale = 1.5f * Vector3.one;
            investorImage.color = new Color(1f, 1f, 1f, 0f);
            infoText.color = new Color(1f, 1f, 1f, 0f);
            strip.rectTransform.anchoredPosition = new Vector2(-1000f, 0f);
            _tween = DOTween.Sequence()
                .SetUpdate(true)
                .Append(strip.rectTransform.DOAnchorPos(Vector2.zero, 0.5f))
                .Join(background.DOFade(0.5f, 0.25f))
                .Append(investorImage.DOFade(1f, 0.5f))
                .Join(investorImage.transform.DOScale(1f, 0.5f))
                .Append(infoText.DOFade(1f, 0.25f))
                .AppendInterval(3f)
                .AppendCallback(() =>
                {
                    gameObject.SetActive(false);
                    tcs.SetResult(true);
                });
            return tcs.Task;
        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}