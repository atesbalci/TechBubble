using UnityEngine;
using Zenject;

namespace TechBubble.Behaviors
{
    public class InvestmentBehavior : PickupableBehaviour
    {
    }

    public class InvestmentPool : MonoMemoryPool<InvestmentBehavior> { }
}