using System;
using TechBubble.Models;
using UnityEngine;
using Zenject;

namespace TechBubble.Behaviors
{
    public class DeadlineBehavior : PickupableBehaviour
    {
        public float DeadlineTime { get; private set; }
        public long Money { get; set; }
        public InvestmentData InvestmentData { get; private set; }

        public void Initialize(InvestmentData investmentData, Vector3 position)
        {
            InvestmentData = investmentData;
            transform.position = position;
            DeadlineTime = Time.time + investmentData.Deadline;
            Money = investmentData.Money;
        }
    }
    
    public class DeadlinePool : MonoMemoryPool<DeadlineBehavior> { }
}