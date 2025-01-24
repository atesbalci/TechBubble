using System;
using UnityEngine;

namespace TechBubble.Behaviors
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public Vector2 MovementDirection { get; private set; }

        public void SetMovementDirection(Vector2 movementDirection)
        {
            MovementDirection = movementDirection;
        }

        private void Update()
        {
            transform.position += (Vector3) MovementDirection * Time.deltaTime * 5f;
        }
    }
}