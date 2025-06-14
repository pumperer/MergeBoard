using System;
using System.Collections.Generic;
using alpoLib.Data;
using alpoLib.Res;
using alpoLib.UI;
using MergeBoard.Data.Composition;
using MergeBoard.Data.User;
using TMPro;
using UnityEngine;

namespace MergeBoard.UI.Popup
{
    [PrefabPath("Addr/UI/Prefabs/Popup/QuestSubmit/PopupQuestSubmit.prefab")]
    [LoadingBlockDefinition(typeof(PopupQuestSubmitLoadingBlock))]
    public class PopupQuestSubmit : DataPopup<PopupQuestSubmitParam, PopupQuestSubmitInitData>
    {
        [SerializeField] protected TMP_Text questNameText;
        [SerializeField] protected Transform questConditionItemSlotParent;
        [SerializeField] protected Transform questRewardItemSlotParent;
        
        private List<ItemSlotData> _conditionItemSlotDataList = new();
        private List<RewardSlotData> _rewardSlotDataList = new();
        
        private ListSlotController<ItemSlotData, ItemSlot> _conditionItemSlotController;
        private ListSlotController<RewardSlotData, RewardSlot> _rewardSlotController;
        
        protected override void Awake()
        {
            base.Awake();
            _conditionItemSlotController = new ListSlotController<ItemSlotData, ItemSlot>(CreateConditionSlotFunc, DeleteConditionSlotFunc);
            _rewardSlotController = new ListSlotController<RewardSlotData, RewardSlot>(CreateRewardSlotFunc, DeleteRewardSlotFunc);
        }

        private ItemSlot CreateConditionSlotFunc(ItemSlotData data)
        {
            var slot = GenericPrefab.InstantiatePrefab<ItemSlot>("QuestSubmit", questConditionItemSlotParent);
            slot.SetData(data);
            return slot;
        }

        private void DeleteConditionSlotFunc(ItemSlot slot)
        {
            Destroy(slot.gameObject);
        }

        private RewardSlot CreateRewardSlotFunc(RewardSlotData data)
        {
            var slot = GenericPrefab.InstantiatePrefab<RewardSlot>(parent: questRewardItemSlotParent);
            slot.SetData(data);
            return slot;
        }

        private void DeleteRewardSlotFunc(RewardSlot slot)
        {
            Destroy(slot.gameObject);
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (questNameText)
                questNameText.text = InitData.QuestData.GetQuestName();

            _conditionItemSlotDataList.Clear();
            for (var index = 0; index < InitData.QuestData.Conditions.Count; index++)
            {
                var condition = InitData.QuestData.Conditions[index];
                var itemBase = InitData.QuestData.ConditionItemBaseList[index];
                var itemSlotData = new ItemSlotData
                {
                    UserItemMapper = InitData.UserItemMapper,
                    ItemBase = itemBase,
                    RequireCount = condition.Condition.ConditionValue,
                };
                _conditionItemSlotDataList.Add(itemSlotData);
            }
            _conditionItemSlotController.ApplyData(_conditionItemSlotDataList);

            _rewardSlotDataList.Clear();
            foreach (var reward in InitData.QuestData.Rewards)
            {
                var rewardSlotData = new RewardSlotData { Reward = reward.Reward };
                _rewardSlotDataList.Add(rewardSlotData);
            }
            _rewardSlotController.ApplyData(_rewardSlotDataList);
        }
        
        public void OnClickSubmit()
        {
            if (InitData.QuestData.CheckIsDone())
            {
                InitData.QuestData.CompleteQuest();
                InitData.QuestSubmitCallback?.Invoke(InitData.QuestData);
                Close();
            }
            else
            {
                Debug.LogWarning("Quest is not completed yet.");
            }
        }
    }

    public class PopupQuestSubmitParam : PopupParam
    {
        public QuestData QuestData;
        public Action<QuestData> QuestSubmitCallback;
    }

    public class PopupQuestSubmitInitData : PopupInitData
    {
        public QuestData QuestData;
        public Action<QuestData> QuestSubmitCallback;
        public IUserItemMapper UserItemMapper;
    }

    public class PopupQuestSubmitLoadingBlock : PopupLoadingBlock<PopupQuestSubmitParam, PopupQuestSubmitInitData>
    {
        public override PopupQuestSubmitInitData MakeInitData(PopupQuestSubmitParam param)
        {
            return new PopupQuestSubmitInitData
            {
                QuestData = param.QuestData,
                QuestSubmitCallback = param.QuestSubmitCallback,
                UserItemMapper = UserDataManager.GetLoader<IUserItemMapper>()
            };
        }
    }
}