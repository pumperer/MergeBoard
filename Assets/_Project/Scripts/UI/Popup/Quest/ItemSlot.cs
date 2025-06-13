using alpoLib.Data;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Popup
{
    public class ItemSlotData : DataManagerHolder
    {
        public ItemBase ItemBase;
        public int RequireCount;

        private int GetCurrentValue()
        {
            return UserDataManager.GetLoader<IUserItemMapper>().GetItemCount(ItemBase.Id);
        }
        
        public bool IsComplete => GetCurrentValue() >= RequireCount;
        public string ValueString => $"{GetCurrentValue()}/{RequireCount}";
        public string LevelString => ItemData.GetLevelString(ItemBase);
        public string NameString => ItemData.GetName(ItemBase, false);
    }
    
    [MultiPrefabPath("QuestSlot", "Addr/UI/Prefabs/Popup/Quest/ItemSlot.prefab")]
    public class ItemSlot : CachedUIBehaviour
    {
        [SerializeField]
        private Image bgImage = null;

        [SerializeField]
        private Image itemImage = null;

        [SerializeField]
        private TMP_Text itemCountText = null;

        [SerializeField]
        private TMP_Text itemSequenceText = null;

        [SerializeField]
        private TMP_Text itemNameText = null;

        [SerializeField]
        private Image checkImage = null;
        
        [SerializeField]
        private Color defaultColor = Color.white;

        [SerializeField]
        private Color completeColor = Color.green;

        private int _requestKey = 0;
        
        public void UpdateUI(ItemSlotData data)
        {
            if (bgImage)
                bgImage.color = data.IsComplete ? completeColor : defaultColor;
            
            if (itemImage)
            {
                OnDemandAtlasManager.Instance.GetSprite(new SpriteAtlasRequest
                {
                    AtlasKey = $"Addr/Atlas/BoardItem_{data.ItemBase.AtlasName}_Atlas.spriteatlasv2",
                    SpriteName = data.ItemBase.SpriteName,
                    RequestKey = ++_requestKey
                }, result =>
                {
                    if (result.RequestKey != _requestKey)
                        return;
                    if (itemImage)
                        itemImage.sprite = result.Sprite;
                });
            }

            if (itemCountText)
                itemCountText.text = data.ValueString;

            if (itemSequenceText)
                itemSequenceText.text = data.LevelString;

            if (itemNameText)
                itemNameText.text = data.NameString;

            if (checkImage)
                checkImage.gameObject.SetActive(data.IsComplete);
        }
    }
}