using System;
using UnityEngine;

namespace TechBubble.Views
{
    [RequireComponent(typeof(Renderer))]
    public class TileView : MonoBehaviour
    {
        private Transform _camera;
        private Material _material;
        private Vector2 _tileMovementMultiplier;
        
        private void Start()
        {
            _camera = transform.parent;
            _material = GetComponent<Renderer>().material;
            _tileMovementMultiplier = _material.mainTextureScale / transform.localScale.x;
        }

        private void LateUpdate()
        {
            _material.mainTextureOffset = _camera.position * _tileMovementMultiplier;
        }
    }
}