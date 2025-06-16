using MergeBoard.Data;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Scenes.Board.Feature.UI;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class RandomBoxPopFeature : InGameFeatureBase
    {
        private RandomBoxPopFeatureUI UI => FeatureUI as RandomBoxPopFeatureUI;
        
        private readonly IUserInfoMapper _userInfoMapper;
        private readonly IUserItemMapper _userItemMapper;
        private readonly IPopProbabilityTableMapper _popProbabilityMapper;
        
        private PopItemFeature _popItemFeature;
        
        public RandomBoxPopFeature(MergeBoard board,
            IUserInfoMapper userInfoMapper,
            IUserItemMapper userItemMapper,
            IPopProbabilityTableMapper popProbabilityMapper) : base(board)
        {
            _userInfoMapper = userInfoMapper;
            _userItemMapper = userItemMapper;
            _popProbabilityMapper = popProbabilityMapper;
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _popItemFeature = Board.GetFeature<PopItemFeature>();
            UI?.SetRandomBoxPopEvent(OnPopFromRandomBox);
        }

        private void OnPopFromRandomBox(Vector3 worldPosition)
        {
            if (_userInfoMapper.GetGold() < 100)
            {
                SoundManager.Instance.PlaySFX(SFXKey.sfx_cancel);
                return;
            }

            worldPosition.z = 0f;
            if (PopItemFromRandomBox(worldPosition))
                _userInfoMapper.AddGold(-100);
        }
        
        private bool PopItemFromRandomBox(Vector3 pos)
        {
            var emptySlot = Board.FindNearestEmptySlot(null);
            if (emptySlot == null)
                return false;

            var poppedItemId = _popProbabilityMapper.GetPopItemIdByRandom(PopType.FromRandomBox, Board.CurrentBoardId);
            if (poppedItemId == 0)
                return false;

            var newItemData = _userItemMapper.AddItem(poppedItemId);
            var itemComp = PopItemFeature.CreateItemComp(newItemData, emptySlot, pos);
            Board.OnRandomBoxPop(itemComp);
            return true;
        }
    }
}