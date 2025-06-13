using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Utility;

namespace MergeBoard.Data.Composition
{
    public class QuestData : CompositionDataBase<QuestDefineBase, UserQuest>, IComparable<QuestData>
    {
        private readonly List<QuestConditionBase> _conditions;
        private readonly List<QuestRewardBase> _rewards;
        
        public int QuestId => BaseData.Id;
        public List<QuestConditionBase> Conditions => _conditions;
        
        private List<ItemBase> _conditionItemBaseList;

        public List<ItemBase> ConditionItemBaseList
        {
            get
            {
                _conditionItemBaseList ??= _conditions.Select(b =>
                    TableDataManager.GetLoader<IItemTableLoader>().GetItemBase(b.Condition.ConditionId)).ToList();
                return _conditionItemBaseList;
            }
            
        }

        public QuestData(QuestDefineBase baseData, UserQuest userData) : base(baseData, userData)
        {
            _conditions = TableDataManager.GetLoader<IQuestConditionTableMapper>().GetQuestConditions(baseData.Id);
            _rewards = TableDataManager.GetLoader<IQuestRewardTableMapper>().GetQuestRewards(baseData.Id);
        }

        public void CompleteQuest()
        {
            foreach (var condition in _conditions)
            {
                UserDataManager.GetLoader<IUserItemMapper>().RemoveItemByItemId(condition.Condition.ConditionId, condition.Condition.ConditionValue);
            }

            foreach (var reward in _rewards)
            {
                UserDataManager.GetLoader<IUserInfoMapper>().Receive(reward.Reward);
            }
        }

        public bool CheckIsDone()
        {
            return _conditions.TrueForAll(c =>
            {
                var count = UserDataManager.GetLoader<IUserItemMapper>().GetItemCount(c.Condition.ConditionId);
                var requiredCount = c.Condition.ConditionValue;
                return count >= requiredCount;
            });
        }

        public string GetQuestName()
        {
            return LocalizationManager.Instance.GetString(BaseData.Key);
        }

        public int CompareTo(QuestData other)
        {
            return -CheckIsDone().CompareTo(other.CheckIsDone());
        }
    }
}