using System;
using alpoLib.Res;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class PopItemFeature : MergeFeatureBase
    {
        private readonly IUserInfoMapper _userInfoMapper;
        private readonly IUserItemMapper _userItemMapper;
        private readonly IPopProbabilityTableMapper _popProbabilityMapper;

        public PopItemFeature(
            MergeBoard board,
            IUserInfoMapper userInfoMapper,
            IUserItemMapper userItemMapper,
            IPopProbabilityTableMapper popProbabilityMapper) : base(board)
        {
            _userInfoMapper = userInfoMapper;
            _userItemMapper = userItemMapper;
            _popProbabilityMapper = popProbabilityMapper;
        }

        public bool PopItemFromRandomBox(Vector3 pos)
        {
            var emptySlot = Board.FindNearestEmptySlot(null);
            if (emptySlot == null)
                return false;

            var poppedItemId = _popProbabilityMapper.GetPopItemIdByRandom(PopType.FromRandomBox, Board.CurrentBoardId);
            if (poppedItemId == 0)
                return false;

            var newItemData = _userItemMapper.AddItem(poppedItemId);
            Pop(newItemData, emptySlot, pos);
            return true;
        }

        public Item Pop(Item item)
        {
            if (item == null || item.ItemData == null)
                return null;

            var targetSlot = Board.FindNearestEmptySlot(item.CurrentSlot);
            if (targetSlot == null)
                return null;

            if (_userInfoMapper.GetEnergyCount() == 0)
                return null;

            if (!item.ItemData.CanPop())
                return null;

            _userInfoMapper.AddEnergy(-item.ItemData.Energy);
            var poppedItemData = item.ItemData.Pop();
            return Pop(poppedItemData, targetSlot, item.CurrentSlot.transform.position);
        }

        public Item Pop(ItemData newItemData, BoardSlot targetSlot, Vector3 fromPos)
        {
            if (newItemData == null)
                return null;

            var newItem = CreateItemComponent(newItemData);
            if (targetSlot != null && newItem != null)
            {
                newItem.SetSlot(targetSlot);
                newItem.transform.position = fromPos;
                targetSlot.SetItem(newItem);
                targetSlot.RepositionItem(true);
            }
            SoundManager.Instance.PlaySFX(SFXKey.sfx_board_item_pop);
            return newItem;
        }

        private static Item CreateItemComponent(ItemData itemData)
        {
            var item = GenericPrefab.InstantiatePrefab<Item>();
            item.Init(itemData);
            return item;
        }
    }
}
