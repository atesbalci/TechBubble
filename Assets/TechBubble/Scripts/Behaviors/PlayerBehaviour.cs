using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TechBubble.Behaviors
{
    public class PlayerBehaviour : MonoBehaviour, IMoneyActor
    {
        public event Action<PickupableBehaviour> OnPickupableBehaviourCollision;
        
        public Vector2 MovementDirection { get; private set; }
        public float Speed { get; set; }
        public long Money { get; set; }
        
        private PlayerInput _playerInput;

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public void SetMovementDirection(Vector2 movementDirection)
        {
            MovementDirection = movementDirection;
        }

        private void Update()
        {
            transform.position += (Vector3) MovementDirection * Time.deltaTime * Speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<PickupableBehaviour>(out var pickupable))
            {
                OnPickupableBehaviourCollision?.Invoke(pickupable);
            }
        }

        public void LockInput()
        {
            _playerInput.enabled = false;
        }
    }
}