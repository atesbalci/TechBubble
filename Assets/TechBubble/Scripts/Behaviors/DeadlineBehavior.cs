using System;
using UnityEngine;
using Zenject;

namespace TechBubble.Behaviors
{
    public class DeadlineBehavior : PickupableBehaviour
    {
        public float DeadlineTime { get; private set; }
        public Renderer Renderer => renderer;
        
        [SerializeField] private Renderer renderer;

        public void Initialize(float deadlineTime, Vector3 position)
        {
            transform.position = position;
            DeadlineTime = deadlineTime;
        }
    }
    
    public class DeadlinePool : MonoMemoryPool<DeadlineBehavior> { }
}