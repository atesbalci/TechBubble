using TechBubble.Behaviors;
using TMPro;
using UnityEngine;
using Zenject;

namespace TechBubble.Views
{
    [RequireComponent(typeof(IMoneyActor))]
    public class MoneySizeView : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [SerializeField] private TMP_Text moneyText;
        
        private IMoneyActor _moneyActor;
        private IViewData _viewData;

        [Inject]
        public void Initialize(IViewData viewData)
        {
            _viewData = viewData;
            _moneyActor = GetComponent<IMoneyActor>();
        }

        private void Update()
        {
            moneyText.text = _moneyActor.Money.ToMoneyString();
            var scale = Mathf.Max(_moneyActor.Money * _viewData.MoneySizeMultiplier, _viewData.MinimumMoneySizeScale);
            body.localScale = new Vector3(scale, scale, scale);
        }
    }
}