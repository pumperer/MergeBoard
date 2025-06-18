using UnityEngine;

namespace MergeBoard.VFX
{
    public class ParticleVfxUIObject : ParticleVfxObject
    {
        public override async Awaitable Play(RectTransform target, bool autoRelease = true)
        {
            var p = transform.TransformPoint(target.localPosition);
            await Play(p, autoRelease);
        }
    }
}