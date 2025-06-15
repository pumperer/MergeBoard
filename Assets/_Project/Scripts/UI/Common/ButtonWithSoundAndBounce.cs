using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MergeBoard.Sound;

namespace MergeBoard.UI.Common
{
    public class ButtonWithSoundAndBounce : Button, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private SFXKey soundKey = SFXKey.sfx_click;
        [SerializeField] private float pressedScale = 0.95f;
        [SerializeField] private float bounceScale = 1.1f;
        [SerializeField] private float bounceDuration = 0.1f;

        private Coroutine _bounceRoutine;

        private void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        protected override void Awake()
        {
            base.Awake();
            transition = Transition.None;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (_bounceRoutine != null)
            {
                StopCoroutine(_bounceRoutine);
                _bounceRoutine = null;
            }
            SetScale(pressedScale);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            SetScale(1f);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            SoundManager.Instance.PlaySFX(soundKey);
            if (gameObject.activeInHierarchy)
            {
                if (_bounceRoutine != null)
                    StopCoroutine(_bounceRoutine);
                _bounceRoutine = StartCoroutine(Bounce());
            }
        }

        private IEnumerator Bounce()
        {
            var t = transform;
            var origin = t.localScale;
            var half = bounceDuration * 0.5f;

            float time = 0f;
            while (time < half)
            {
                float lerp = time / half;
                float scale = Mathf.Lerp(1f, bounceScale, lerp);
                t.localScale = new Vector3(scale, scale, scale);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            time = 0f;
            while (time < half)
            {
                float lerp = time / half;
                float scale = Mathf.Lerp(bounceScale, 1f, lerp);
                t.localScale = new Vector3(scale, scale, scale);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = origin;
            _bounceRoutine = null;
        }
    }
}
