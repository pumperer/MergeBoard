using UnityEngine;

namespace MergeBoard.VFX
{
    public class ParticleVfxUIObject : ParticleVfxObject
    {
        public override async Awaitable Play(RectTransform rectTransform, bool autoRelease = true)
        {
            var p = rectTransform.TransformPoint(rectTransform.anchoredPosition);
            await Play(p, autoRelease);
        }
    }
}