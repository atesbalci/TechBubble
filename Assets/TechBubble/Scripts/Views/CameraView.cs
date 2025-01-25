using System;
using TechBubble.Behaviors;
using UnityEngine;
using Zenject;

namespace TechBubble.Views
{
    public class CameraView : MonoBehaviour
    {
        private PlayerBehaviour _playerBehaviour;
        
        [Inject]
        public void Initialize(PlayerBehaviour playerBehaviour)
        {
            _playerBehaviour = playerBehaviour;
        }

        private void Update()
        {
            var newPos = Vector2.Lerp(transform.position, _playerBehaviour.transform.position, 5f * Time.deltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }
}