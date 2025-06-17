using System;
using System.Collections;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using MergeBoard.Utility;
using MergeBoard.VFX;
using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    [PrefabPath("Addr/Board/Item.prefab")]
    public class Item : MonoBehaviour, IMovable
    {
        [SerializeField]
        private SpriteRenderer itemIcon = null;
        [SerializeField]
        private SpriteRenderer energyIcon = null;
        [SerializeField]
        private ItemRechargeTimerComp timerIcon = null;
        [SerializeField]
        private SpriteRenderer checkIcon = null;

        [SerializeField]
        private Animation rootAnimation = null;
        [SerializeField]
        private Animation itemAnimation = null;

        [SerializeField]
        private BoardSlot currentSlot = null;
        private ItemData itemData = null;

        public ItemData ItemData => itemData;

        public BoardSlot CurrentSlot => currentSlot;

        private int _requestKey = 0;
        
        public void Init(ItemData data)
        {
            itemData = data;
            if (itemData == null)
                throw new Exception("ItemData is null");

            OnDemandAtlasManager.Instance.GetSprite(new SpriteAtlasRequest
            {
                AtlasKey = $"Addr/Atlas/BoardItem_{itemData.AtlasName}_Atlas.spriteatlasv2",
                SpriteName = itemData.SpriteName,
                RequestKey = ++_requestKey,
            }, result =>
            {
                if (result.RequestKey != _requestKey)
                    return;
                if (result.Sprite && itemIcon)
                    itemIcon.sprite = result.Sprite;
            });

            ItemData_PopCountChangeEvent(0);

            itemData.PopCountChangeEvent += ItemData_PopCountChangeEvent;

            // transform.localScale = Vector3.one * 0.4f;
            // TweenScale.Begin(gameObject, 0.1f, Vector3.one);

            StartCoroutine(CR_PlayPopIdleAnimation(1f));
        }
        
        private void Update()
        {
            if (itemData == null)
                return;
			
            if (!itemData.IsPopItem())
                return;

            if (itemData.CanPop())
                return;

            var remainSpan = itemData.GetRemainSpan();
            var coolTimeSpan = TimeSpan.FromMinutes(itemData.PopCooltime);
            var remainRate = 1 - (remainSpan.TotalMilliseconds / coolTimeSpan.TotalMilliseconds);
            timerIcon.SetAmount((float)remainRate);
        }
        
        private void OnDestroy()
        {
            if (itemData != null)
                itemData.PopCountChangeEvent -= ItemData_PopCountChangeEvent;
        }
        
        private void ItemData_PopCountChangeEvent(int count)
        {
            var canPop = itemData.CanPop();
            energyIcon.gameObject.SetActive(canPop);
            timerIcon.gameObject.SetActive(itemData.IsPopItem() && !canPop);
        }
        
        private IEnumerator CR_PlayPopIdleAnimation(float delaySec = 0f)
        {
            if (!itemData.IsPopItem())
                yield break;

            yield return new WaitForSeconds(delaySec);

            if (rootAnimation != null)
                rootAnimation.Play("PopItemIdle");
        }
        
        public void PlaySelectAnimation()
        {
            if (itemAnimation != null)
                itemAnimation.Play("ItemSelect");
        }
        
        public void SetSlot(BoardSlot slot)
        {
            if (currentSlot != null)
                currentSlot.SetItem(null);
            currentSlot = slot;
        }

        public void SetActiveCheckObject(bool active)
        {
            if (checkIcon != null)
                checkIcon.gameObject.SetActive(active);
        }
        
        public bool OnMove(Vector3 worldPos)
        {
            worldPos.z = 0;
            transform.position = worldPos;
            return true;
        }

        public void OnCancelMove(Action onComplete = null)
        {
            var spring = SpringPosition.Begin(gameObject, Vector3.zero, 20f);
            spring.OnFinished = onComplete != null ? new SpringPosition.OnFinishedDelegate(onComplete) : null;
        }
    }
}