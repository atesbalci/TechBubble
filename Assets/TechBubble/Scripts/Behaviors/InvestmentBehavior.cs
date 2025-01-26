using TechBubble.Models;
using TechBubble.Views;
using UnityEngine;
using Zenject;

namespace TechBubble.Behaviors
{
    public class InvestmentBehavior : PickupableBehaviour, IMoneyActor
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public long Money => InvestmentData.Money;
        public InvestmentData InvestmentData { get; private set; }

        private IViewData _viewData;

        [Inject]
        public void Initialize(IViewData viewData)
        {
            _viewData = viewData;
        }

        public void Bind(InvestmentData investmentData)
        {
            InvestmentData = investmentData;
            spriteRenderer.sprite = _viewData.GetInvestorIcon(investmentData.Id);
        }
    }

    public class InvestmentPool : MonoMemoryPool<InvestmentBehavior> { }
}