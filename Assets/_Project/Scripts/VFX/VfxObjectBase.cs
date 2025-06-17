using UnityEngine;

namespace MergeBoard.VFX
{
    public abstract class VfxObjectBase : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
        }
        
        protected virtual void OnDisable()
        {
        }

        public void SetKey(string key)
        {
            gameObject.name = key;
        }
        
        public virtual Awaitable Play(Transform target, bool toLocal = true)
        {
            return null;
        }
        
        public virtual Awaitable Play(Vector3 position, bool autoRelease = true)
        {
            return null;
        }
        
        protected async Awaitable ReleaseAsync(float delayedSec = 0f)
        {
            if (delayedSec > 0f)
                await Awaitable.WaitForSecondsAsync(delayedSec);
            VfxResourceHolder.Instance.Release(gameObject.name, this);
        }
    }
}