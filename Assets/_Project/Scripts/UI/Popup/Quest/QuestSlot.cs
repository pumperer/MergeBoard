using System;
using System.Collections.Generic;
using alpoLib.Res;
using alpoLib.UI;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using MergeBoard.Data.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Popup
{
    [PrefabPath("Addr/UI/Prefabs/Popup/Quest/QuestSlot.prefab")]
    public class QuestSlot : CachedUIBehaviour
    {
        [SerializeField] protected TMP_Text questNameText;
        [SerializeField] protected Transform questConditionItemSlotParent;
        [SerializeField] protected Button submitButton;

        private QuestData _questData;
        private Action<QuestData> _submitCallback;
        private IUserItemMapper _userItemMapper;
        
        private ListSlotController<ItemSlotData, ItemSlot> _itemSlotController;
        private List<ItemSlotData> _itemSlotDataList = new();

        protected override void Awake()
        {
            base.Awake();
            _itemSlotController = new ListSlotController<ItemSlotData, ItemSlot>(CreateSlotFunc, DeleteSlotFunc);
        }

        private void DeleteSlotFunc(ItemSlot slot)
        {
            Destroy(slot.gameObject);
        }

        private ItemSlot CreateSlotFunc(ItemSlotData data)
        {
            var slot = GenericPrefab.InstantiatePrefab<ItemSlot>("QuestSlot", questConditionItemSlotParent);
            slot.SetData(data);
            return slot;
        }

        public void SetData(QuestData questData, IUserItemMapper userItemMapper, Action<QuestData> submitCallback)
        {
            _questData = questData;
            _submitCallback = submitCallback;
            _userItemMapper = userItemMapper;
            UpdateUI();
            if (submitButton)
                submitButton.gameObject.SetActive(_questData.CheckIsDone());
        }

        private void UpdateUI()
        {
            if (questNameText)
                questNameText.text = _questData.GetQuestName();
            RefreshItemList();
        }

        private void RefreshItemList()
        {
            _itemSlotDataList.Clear();
            for (var index = 0; index < _questData.Conditions.Count; index++)
            {
                var condition = _questData.Conditions[index];
                var itemBase = _questData.ConditionItemBaseList[index];
                _itemSlotDataList.Add(new ItemSlotData
                {
                    UserItemMapper = _userItemMapper,
                    ItemBase = itemBase,
                    RequireCount = condition.Condition.ConditionValue,
                });
            }
            _itemSlotController.ApplyData(_itemSlotDataList);

            // for (var index = 0; index < questConditionItemSlots.Length; index++)
            // {
            //     var slot = questConditionItemSlots[index];
            //     if (index < _questData.Conditions.Count)
            //     {
            //         var condition = _questData.Conditions[index];
            //         slot.UpdateUI(new ItemSlotData
            //         {
            //             UserItemMapper = _userItemMapper,
            //             ItemBase = _questData.ConditionItemBaseList[index],
            //             RequireCount = condition.Condition.ConditionValue,
            //         });
            //         slot.gameObject.SetActive(true);
            //     }
            //     else
            //     {
            //         slot.gameObject.SetActive(false);
            //     }
            // }
        }

        public void OnClickSubmit()
        {
            _submitCallback?.Invoke(_questData);
        }
    }
}