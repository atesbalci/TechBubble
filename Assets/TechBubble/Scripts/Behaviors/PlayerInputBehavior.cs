using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TechBubble.Behaviors
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerInputBehavior : MonoBehaviour
    {
        private PlayerBehaviour _player;

        private void Awake()
        {
            _player = GetComponent<PlayerBehaviour>();
        }

        public void Move(InputAction.CallbackContext context)
        {
            _player.SetMovementDirection(context.ReadValue<Vector2>());
        }
    }
}