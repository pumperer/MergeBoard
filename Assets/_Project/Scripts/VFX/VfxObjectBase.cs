using UnityEngine;

namespace MergeBoard.VFX
{
    public abstract class VfxObjectBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            
        }
        
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
            throw new System.NotImplementedException("Play(Transform) method must be overridden in derived class.");
        }
        
        public virtual Awaitable Play(Vector3 position, bool autoRelease = true)
        {
            throw new System.NotImplementedException("Play(Vector3) method must be overridden in derived class.");
        }
        
        public virtual Awaitable Play(RectTransform rectTransform, bool autoRelease = true)
        {
            throw new System.NotImplementedException("Play(RectTransform) method must be overridden in derived class.");
        }
        
        protected async Awaitable ReleaseAsync(float delayedSec = 0f)
        {
            if (delayedSec > 0f)
                await Awaitable.WaitForSecondsAsync(delayedSec);
            if (this == null || gameObject == null)
            {
                Debug.LogWarning("VfxObjectBase::ReleaseAsync called with a null object");
                return;
            }
            VfxResourceHolder.Instance.Release(gameObject.name, this);
        }
    }
}