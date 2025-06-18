using System;
using UnityEngine;

namespace MergeBoard.VFX
{
    public class ParticleVfxObject : VfxObjectBase
    {
        [SerializeField] protected ParticleSystem particle;

        protected override void Awake()
        {
            particle ??= GetComponent<ParticleSystem>();
            if (!particle)
                throw new ArgumentNullException(gameObject.name, "ParticleSystem is null...");
        }

        public override async Awaitable Play(Vector3 position, bool autoRelease = true)
        {
            transform.position = position;
            particle.Play();
            if (particle.main.loop)
                return;
            await ReleaseAsync(particle.main.duration);
        }
    }
}