using System;
using alpoLib.Data.Composition;
using alpoLib.Util;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Utility;

namespace MergeBoard.Data.Composition
{
    public class ItemData : CompositionDataBase<ItemBase, UserItem>
    {
        public delegate void OnChangePopCount(int count);
        public event OnChangePopCount PopCountChangeEvent = null;
        
        public int Id => BaseData.Id;
        public int UserDataId => UserData.Id;
        public int Energy => BaseData.Energy;
        public int MergeToSeq => BaseData.MergeToSeq;
        public int Sequence => BaseData.Sequence;
        public string Category => BaseData.Category;
        public string AtlasName => BaseData.AtlasName;
        public string SpriteName => BaseData.SpriteName;
        public int PopCooltime => BaseData.PopCooltime;
        public int SellValue => BaseData.SellValue;
        
        public ItemData(ItemBase baseData, UserItem userData) : base(baseData, userData)
        {
            CreateTimer();
        }

        protected override bool CustomEqualityComparer(CompositionDataBase<ItemBase, UserItem> other)
        {
            if (other is not ItemData otherData)
                return false;
            
            return BaseData.Id == otherData.BaseData.Id && UserData.Id == otherData.UserData.Id;
        }

        public void CreateTimer()
        {
            if (CanPop())
                return;

            TaskScheduler.CreateAlarm(UserData.TimerKey(), UserData.NextCooltimeEnd, RefillPopCount);
        }

        public ItemData Pop()
        {
            if (!CanPop())
                return null;

            var poppedItemId = TableDataManager.GetLoader<IPopProbabilityTableMapper>()
                .GetPopItemIdByRandom(PopType.FromPopItem, BaseData.Id);

            if (BaseData.PopCount > 0)
            {
                UserData.RemainPopCount--;
                PopCountChangeEvent?.Invoke(UserData.RemainPopCount);
            }

            if (UserData.RemainPopCount == 0)
            {
                UserData.NextCooltimeEnd = DateTimeOffset.Now.AddMinutes(BaseData.PopCooltime);
                CreateTimer();
            }
            return UserDataManager.GetLoader<IUserItemMapper>().AddItem(poppedItemId);
        }

        public bool CanPop()
        {
            if (BaseData == null || UserData == null)
                return false;

            if (!BaseData.CanPop)
                return false;

            if (UserData.NextCooltimeEnd > DateTimeOffset.Now)
                return false;

            if (UserData.RemainPopCount <= 0)
                return false;

            return true;
        }
        
        private void RefillPopCount(string timerKey)
        {
            if (UserData.RemainPopCount == 0)
            {
                UserData.RemainPopCount = BaseData.PopCount;
                UserData.NextCooltimeEnd = DateTimeOffset.MinValue;
            }
            PopCountChangeEvent?.Invoke(UserData.RemainPopCount);
        }

        public bool IsPopItem()
        {
            if (BaseData == null || UserData == null)
                return false;
            return BaseData.CanPop;
        }

        public TimeSpan GetRemainSpan()
        {
            return UserData.NextCooltimeEnd - DateTimeOffset.Now;
        }

        public string GetName(bool withLevel)
        {
            return GetName(BaseData, withLevel);
        }
        
        public static string GetName(ItemBase itemBase, bool withLevel)
        {
            var itemName = LocalizationManager.Instance.GetString($"Item_Name_{itemBase.Id}");
            if (withLevel)
                return $"{itemName} ({GetLevelString(itemBase)})";
            else
                return itemName;
        }

        public static string GetLevelString(ItemBase itemBase)
        {
            string itemLevel;
            if (itemBase.MergeToSeq == 0)
                itemLevel = LocalizationManager.Instance.GetString("UI_Item_LevelMax");
            else
                itemLevel = LocalizationManager.Instance.GetString("UI_Item_Level", itemBase.Sequence);
            return itemLevel;
        }

        public string GetDescription()
        {
            var suffix = string.Empty;
            var lm = LocalizationManager.Instance;
            var totalDesc = lm.GetString($"Item_Desc_{BaseData.Id}");
            if (BaseData.CanPop)
            {
                if (UserData.RemainPopCount > 0)
                {
                    suffix = lm.GetString("UI_Label_RemainItemPopCount", UserData.RemainPopCount);
                }
                else
                {
                    totalDesc = lm.GetString("UI_Item_Charging");
                    var span = UserData.NextCooltimeEnd - DateTimeOffset.Now;
                    suffix = lm.GetString("UI_Label_RemainItemCooltime", LocalizationManager.MakeTimeSpanString(span));
                }
            }

            if (!string.IsNullOrEmpty(suffix))
                totalDesc = $"{totalDesc} {suffix}";
            return totalDesc;
        }
    }
}