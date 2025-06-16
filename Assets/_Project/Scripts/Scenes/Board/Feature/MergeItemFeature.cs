using alpoLib.Res;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class MergeItemFeature : InGameFeatureBase
    {
        private readonly IItemTableMapper _itemTableMapper;
        private readonly IUserItemMapper _userItemMapper;
        
        public MergeItemFeature(MergeBoard board, IItemTableMapper itemTableMapper, IUserItemMapper userItemMapper) : base(board)
        {
            _itemTableMapper = itemTableMapper;
            _userItemMapper = userItemMapper;
        }
        
        public Item Merge(Item item1, Item item2)
        {
            if (item1 == null || item2 == null)
                return null;

            if (!CanMerge(item1.ItemData, item2.ItemData))
                return null;
            
            var nextSeq = item1.ItemData.MergeToSeq;
            var list = _itemTableMapper.GetItemBaseList(item1.ItemData.Category);
            var next = list.Find(x => x.Sequence == nextSeq);
            if (next == null)
                return null;
            
            var itemData = _userItemMapper.AddItem(next.Id);
            if (itemData != null)
            {
                var itemComp = GenericPrefab.InstantiatePrefab<Item>();
                itemComp.Init(itemData);
                        
                SoundManager.Instance.PlaySFX(SFXKey.sfx_board_item_merge);
                _userItemMapper.RemoveItem(item1.ItemData.UserDataId);
                _userItemMapper.RemoveItem(item2.ItemData.UserDataId);
                return itemComp;
            }

            return null;
        }
        
        private static bool CanMerge(ItemData data1, ItemData data2)
        {
            if (data1 == null || data2 == null)
                return false;

            if (data1.Category != data2.Category)
                return false;

            if (data1.Sequence != data2.Sequence)
                return false;

            if (data1.MergeToSeq == 0 || data2.MergeToSeq == 0)
                return false;

            return true;
        }
    }
}