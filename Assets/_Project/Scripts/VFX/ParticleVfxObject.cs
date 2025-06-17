using System;
using UnityEngine;

namespace MergeBoard.VFX
{
    public class ParticleVfxObject : VfxObjectBase
    {
        [SerializeField] private ParticleSystem particle;

        private void Awake()
        {
            particle ??= GetComponent<ParticleSystem>();
            if (!particle)
                throw new ArgumentNullException(gameObject.name, "ParticleSystem is null...");
        }

        public override async Awaitable Play(Vector3 position, bool autoRelease = true)
        {
            transform.position = position;
            particle.Play();
            Awaitable endPlayAwaitable = null;
            if (autoRelease)
                endPlayAwaitable = ReleaseAutoAsync();
            if (endPlayAwaitable != null)
                await endPlayAwaitable;    
        }

        protected async Awaitable ReleaseAutoAsync()
        {
            // If the particle system is set to loop, we don't need to wait for it to finish.
            if (particle.main.loop)
                return;
            
            await Awaitable.WaitForSecondsAsync(particle.main.duration);
            AwaitableHelper.Run(() => ReleaseAsync());
        }
    }
}