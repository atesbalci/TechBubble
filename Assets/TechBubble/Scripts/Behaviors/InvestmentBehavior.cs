using TechBubble.Models;
using Zenject;

namespace TechBubble.Behaviors
{
    public class InvestmentBehavior : PickupableBehaviour, IMoneyActor
    {
        public long Money => InvestmentData.Money;

        public InvestmentData InvestmentData;
    }

    public class InvestmentPool : MonoMemoryPool<InvestmentBehavior> { }
}