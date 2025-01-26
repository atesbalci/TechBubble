using System;
using System.Threading;
using System.Threading.Tasks;
using TechBubble.Behaviors;
using UnityEngine;

namespace TechBubble.Views
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerView : MonoBehaviour, IDeathAnimator
    {
        [SerializeField] private ParticleSystem trailParticles;
        [SerializeField] private Animator deathAnimator;
        
        private PlayerBehaviour _playerBehaviour;

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
            deathAnimator.enabled = false;
        }

        private void Update()
        {
            if (_playerBehaviour.MovementDirection.sqrMagnitude > 0.01f)
            {
                if (!trailParticles.isPlaying) trailParticles.Play();
                
                trailParticles.transform.forward = -_playerBehaviour.MovementDirection;
            }
            else if (trailParticles.isPlaying) trailParticles.Stop();
        }

        public async Task AnimateDeath(CancellationToken cancellationToken)
        {
            deathAnimator.enabled = true;
            while (deathAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.999f)
            {
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
    }
}