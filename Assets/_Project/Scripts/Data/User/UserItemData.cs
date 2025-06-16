using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using UnityEngine;

namespace MergeBoard.Data.User
{
    public record UserItem : UserDataBase
    {
        public int ItemId { get; set; }

        public int RemainPopCount { get; set; }

        public DateTimeOffset NextCooltimeEnd { get; set; }
		
        public bool Active { get; set; }

        public string TimerKey()
        {
            return $"{GetType().Name}_{Id}";
        }
    }
    
    public interface IUserItemMapper : IUserDataMapperBase
    {
        ItemData GetItemData(int id);
        ItemData AddItem(int itemId);
        void RemoveItem(int userItemId);
        void RemoveItemByItemId(int itemId, int count);
        int GetItemCount(int itemId);
    }
    
    public class UserItemData : UserDataManagerBase, IUserItemMapper
    {
        public delegate void OnAddUserItemEvent(ItemData data);
        public static event OnAddUserItemEvent OnAddUserItem;

        public delegate void OnDeleteUserItemEvent(ItemData data);
        public static event OnDeleteUserItemEvent OnDeleteUserItem;

        private IItemTableMapper _itemTableMapper = null;

        [SerializeField]
        private Dictionary<int, UserItem> _itemDic = new();

        public override void OnCreateInstance()
        {
            _itemTableMapper = TableDataManager.GetLoader<IItemTableMapper>();
        }

        public ItemData GetItemData(int id)
        {
            if (_itemDic.TryGetValue(id, out var userItem))
            {
                if (!userItem.Active)
                    return null;

                var itemBase = _itemTableMapper.GetItemBase(userItem.ItemId);
                return new ItemData(itemBase, userItem);
            }
            return null;
        }

        public ItemData AddItem(int itemId)
        {
            var itemBase = _itemTableMapper.GetItemBase(itemId);
            if (itemBase != null)
            {
                var userItem = new UserItem
                {
                    Id = GetNextUserItemId(),
                    ItemId = itemId,
                    RemainPopCount = itemBase.PopCount,
                    NextCooltimeEnd = DateTimeOffset.MinValue,
                    Active = true,
                };
                _itemDic.Add(userItem.Id, userItem);

                var data = new ItemData(itemBase, userItem);
                OnAddUserItem?.Invoke(data);
                return data;
            }
            return null;
        }

        public void RemoveItem(int userItemId)
        {
            if (_itemDic.TryGetValue(userItemId, out var item))
            {
                SetDeactiveUserItem(item);
            }
        }

        public void RemoveItemByItemId(int itemId, int count)
        {
            var items = _itemDic.Where(p => p.Value.ItemId == itemId && p.Value.Active).Select(p => p.Value).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                if (i < count)
                {
                    SetDeactiveUserItem(items[i]);
                }
            }
        }

        public int GetItemCount(int itemId)
        {
            return (from item in _itemDic
                where item.Value.ItemId == itemId && item.Value.Active
                select item.Value).Count();
        }

        private int GetNextUserItemId()
        {
            if (_itemDic.Count == 0)
                return 1;
            else
                return _itemDic.Keys.Max() + 1;
        }
        
        private ItemData MakeItemData(int itemId, UserItem userItem)
        {
            return new ItemData(_itemTableMapper.GetItemBase(itemId), userItem);
        }
        
        private void SetDeactiveUserItem(UserItem userItem)
        {
            if (userItem == null)
                return;

            if (!userItem.Active)
                return;

            userItem.Active = false;
            var itemData = MakeItemData(userItem.ItemId, userItem);
            TaskScheduler.Cancel(userItem.TimerKey());
            OnDeleteUserItem?.Invoke(itemData);
        }
    }
}