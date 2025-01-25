using Zenject;

namespace TechBubble.Behaviors
{
    public class InvestmentBehavior : PickupableBehaviour, IMoneyActor
    {
        public long Money { get; set; }
    }

    public class InvestmentPool : MonoMemoryPool<InvestmentBehavior> { }
}