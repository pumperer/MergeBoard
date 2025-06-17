using alpoLib.Res;
using MergeBoard.Data;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Sound;
using MergeBoard.VFX;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class PopItemFeature : InGameFeatureBase
    {
        private readonly IUserInfoMapper _userInfoMapper;

        public PopItemFeature(
            MergeBoard board,
            IUserInfoMapper userInfoMapper) : base(board)
        {
            _userInfoMapper = userInfoMapper;
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
            return CreateItemComp(poppedItemData, targetSlot, item.CurrentSlot.transform.position);
        }

        public static Item CreateItemComp(ItemData newItemData, BoardSlot targetSlot, Vector3 fromPos)
        {
            if (newItemData == null)
                return null;

            var newItem = GenericPrefab.InstantiatePrefab<Item>();
            newItem.Init(newItemData);
            if (targetSlot != null && newItem != null)
            {
                newItem.SetSlot(targetSlot);
                newItem.transform.position = fromPos;
                targetSlot.SetItem(newItem);
                targetSlot.RepositionItem(true, () =>
                {
                    VfxResourceHolder.Instance.Get("pop-item-vfx").Play(targetSlot.transform.position);
                });
            }
            SoundManager.Instance.PlaySFX(SFXKey.sfx_board_item_pop);
            return newItem;
        }

        public override void OnPop(Item fromItem, Item newItem)
        {
            Debug.Log("PopItemFeature.OnPop called.. Implement your logic here.");
        }
    }
}
