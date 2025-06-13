using MergeBoard.Data.User;
using MergeBoard.Scenes.Board.Feature.UI;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class RandomBoxPopFeature : MergeFeatureBase
    {
        private RandomBoxPopFeatureUI UI => FeatureUI as RandomBoxPopFeatureUI;
        
        private readonly IUserInfoMapper _userInfoMapper;
        
        public RandomBoxPopFeature(MergeBoard board, IUserInfoMapper userInfoMapper) : base(board)
        {
            _userInfoMapper = userInfoMapper;
        }

        public override void OnOpen()
        {
            base.OnOpen();
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
            if (Board.PopItemFromRandomBox(worldPosition))
                _userInfoMapper.AddGold(-100);
        }
    }
}