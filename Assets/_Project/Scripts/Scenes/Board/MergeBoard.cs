using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Scenes.Board.Feature;
using MergeBoard.Sound;
using MergeBoard.VFX;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MergeBoard.Scenes.Board
{
    [PrefabPath("Addr/Board/Board.prefab")]
    public partial class MergeBoard : SingletonMonoBehaviour<MergeBoard>, IUserBoardSerializer
    {
        public class Context
        {
            public BoardDefineBase BoardDefine;
            public IItemTableMapper ItemTableMapper;
            public IPopProbabilityTableMapper PopProbabilityTableMapper;
            public IUserInfoMapper UserInfoMapper;
            public IUserBoardMapper UserBoardMapper;
            public IUserItemMapper UserItemMapper;
            public IUserQuestMapper UserQuestMapper;
        }
        
        private Context _context;
        
        protected int WidthCount = 7;
        protected int HeightCount = 8;

        protected List<BoardSlot> slots = new();

        private UserBoard userBoard = null;
        private BoardInputManager _inputManager;

        public int CurrentBoardId => _context.BoardDefine.Id;

        protected override void OnAwakeEvent()
        {
            base.OnAwakeEvent();
            
            _inputManager = new BoardInputManager(this);
            UserItemData.OnDeleteUserItem += OnUserItemDelete;
        }

        private void Update()
        {
            _inputManager?.OnUpdate();
        }

        protected override void OnDestroyEvent()
        {
            base.OnDestroyEvent();
            UserItemData.OnDeleteUserItem -= OnUserItemDelete;
        }

        public async Awaitable InitializeAsync(Context context)
        {
            _context = context;
            CreateBoard();
            ResizeCameraOrthoSize(new Resolution { width = Screen.width, height = Screen.height });
            RefreshQuestStatus();
            InitFeatures();

            await InitializeVfxAsync();
        }

        public void OnOpen()
        {
            foreach (var feature in _features)
                feature.OnOpen();
            
            OnSelectItem(null);
        }
        
        public void OnClose()
        {
            foreach (var feature in _features)
                feature.OnClose();
            
            VfxResourceHolder.Instance.Dispose();
        }
        
        private async Awaitable InitializeVfxAsync()
        {
            await Addressables.InstantiateAsync("Addr/VFX/VfxResourceHolderInGame.prefab").Wait();
            await VfxResourceHolder.Instance.LoadAsync();
        }
        
        private void CreateBoard()
        {
            Dictionary<Vector2Int, ItemData> itemBySlot = new();
            userBoard = _context.UserBoardMapper.GetUserBoardData(CurrentBoardId);
            foreach (var slot in userBoard.UserBoardSlotList)
            {
                var data = _context.UserItemMapper.GetItemData(slot.UserItemId);
                if (data == null)
                    throw new Exception($"ItemData not found for UserItemId: {slot.UserItemId}");
                itemBySlot.Add(new Vector2Int(slot.X, slot.Y), data);
            }
            CreateSlots(itemBySlot);
        }
        
        private void CreateSlots(IDictionary<Vector2Int, ItemData> itemBySlot)
        {
            itemBySlot ??= new Dictionary<Vector2Int, ItemData>();
            for (int i = 0; i < WidthCount; i++)
            {
                for (int j = 0; j < HeightCount; j++)
                {
                    var bs = GenericPrefab.InstantiatePrefab<BoardSlot>();
                    if (bs != null)
                    {
                        bs.transform.SetParent(transform);
                        Item item = null;
                        if (itemBySlot.TryGetValue(new Vector2Int(i, j), out var itemData))
                        {
                            item = GenericPrefab.InstantiatePrefab<Item>();
                            item.Init(itemData);
                        }
                        bs.Init(this, i, j, item);
                        slots.Add(bs);
                    }
                }
            }
        }
        
        private void DestroyItemComp(Item item)
        {
            if (item != null)
            {
                item.SetSlot(null);
                Destroy(item.gameObject);
            }
        }

        public void Select(ISelectable slot)
        {
            if (slot is BoardSlot comp)
            {
                slots.ForEach(bs =>
                {
                    if (bs != null)
                        bs.OnSelect(bs == comp);
                });
                OnSelectItem(comp.CurrentItem);
            }
            else
                OnSelectItem(null);
        }
        
        public void Execute(BoardSlot slot, Item item)
        {
            if (slot == null || item == null)
                return;

            if (item.ItemData == null)
                return;

            var result = Pop(item);
            if (result == null)
                SoundManager.Instance.PlaySFX(SFXKey.sfx_cancel);
        }
        
        public BoardSlot FindNearestEmptySlot(BoardSlot source)
        {
            if (!HasEmptySlot())
                return null;

            if (source == null)
            {
                var emptySlots = slots.FindAll(bs => bs.IsEmpty()).OrderBy(a => Guid.NewGuid());
                return emptySlots.ElementAt(0);
            }

            var sourcePos = new Vector2Int(source.X, source.Y);
            var minDist = float.MaxValue;
            BoardSlot targetSlot = null;
            slots.ForEach(bs =>
            {
                if (!bs.IsEmpty())
                    return;
                if (bs == source)
                    return;

                var targetPos = new Vector2Int(bs.X, bs.Y);
                var dist = Vector2Int.Distance(sourcePos, targetPos);
                if (dist < minDist)
                {
                    minDist = dist;
                    targetSlot = bs;
                }
            });
            return targetSlot;
        }
        
        private bool HasEmptySlot()
        {
            return !slots.TrueForAll(bs => !bs.IsEmpty());
        }
        
        public void SellItem(Item item)
        {
            if (item == null)
                return;

            _context.UserInfoMapper.AddGold(item.ItemData.SellValue);
            _context.UserItemMapper.RemoveItem(item.ItemData.UserDataId);
            RefreshQuestStatus();
        }
        
        #region Events
        
        private void OnUserItemDelete(ItemData data)
        {
            var targetSlot = slots.Find(slot => slot.CurrentItem != null && slot.CurrentItem.ItemData.Equals(data));
            if (targetSlot != null)
            {
                DestroyItemComp(targetSlot.CurrentItem);
                targetSlot.OnSelect(false);
                Select(null);
            }
        }
        
        private void ResizeCameraOrthoSize(Resolution res)
        {
            var camera = Camera.main;

            var padding = 1;
            var orthoByWidth = (WidthCount + padding) * 1 * 0.5f * res.height / res.width;
            var orthoByHeight = (HeightCount+ padding) * 1 * 0.5f;
            camera.orthographicSize = Mathf.Max(orthoByWidth, orthoByHeight);
        }
        
        #endregion
        
        #region Pop

        public Item Pop(Item item)
        {
            var newItem = GetFeature<PopItemFeature>()?.Pop(item);
            OnPop(item, newItem);
            return newItem;
        }

        #endregion
        
        #region Quest

        public void RefreshQuestStatus()
        {
            var questList = _context.UserQuestMapper.GetAllQuestDatas(CurrentBoardId);
            var checkedQuestId = new List<int>();
            var acquiredQuestConditionItemIds = new List<int>();
            var questItemIds = new List<int>();
            foreach (QuestData quest in questList)
            {
                var qid = quest.QuestId;
                if (checkedQuestId.Contains(qid))
                    continue;

                var itemIdList = quest.Conditions.Select(c => c.Condition.ConditionId).ToList();
                // var itemIdList = from condition in quest.Conditions
                //     select condition.Condition.ConditionId;

                questItemIds.AddRange(itemIdList);
                if (quest.CheckIsDone())
                    acquiredQuestConditionItemIds.AddRange(itemIdList);

                checkedQuestId.Add(qid);
            }
            var distinctedItemIds = questItemIds.Distinct().ToList();
            var distinctedAcquiredItemIds = acquiredQuestConditionItemIds.Distinct().ToList();

            foreach (var slot in slots)
            {
                var item = slot.CurrentItem;
                if (item == null)
                    continue;

                item.SetActiveCheckObject(distinctedItemIds.Contains(item.ItemData.Id));
                slot.SetBGColor(distinctedAcquiredItemIds.Contains(item.ItemData.Id));
            }
        }
        
        #endregion

        #region IUserBoardSerializer
        
        public void SerializeBoard()
        {
            userBoard.UserBoardSlotList.Clear();
            foreach (var slot in slots)
            {
                if (slot.CurrentItem == null)
                    continue;

                var x = slot.X;
                var y = slot.Y;
                var userItemId = slot.CurrentItem.ItemData.UserDataId;
                var userBoardSlot = new UserBoardSlot
                {
                    X = x,
                    Y = y,
                    UserItemId = userItemId,
                };
                userBoard.UserBoardSlotList.Add(userBoardSlot);
            }
        }
        
        #endregion
    }
}