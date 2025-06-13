using alpoLib.UI.Hud;
using MergeBoard.Data.User;
using TMPro;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public class HudItemGold : HudItemBaseWithData
    {
        [SerializeField] private TMP_Text goldCountText;

        protected override void OnAddEvent()
        {
            var userMapper = Data.GetUserDataLoader<IUserInfoMapper>();
            UpdateUI(userMapper.GetGold());
            UserInfoData.OnChangeGoldEvent += OnChangeGoldEvent;
        }

        protected override void OnRemoveEvent()
        {
            UserInfoData.OnChangeGoldEvent -= OnChangeGoldEvent;
        }

        private void UpdateUI(int gold)
        {
            if(goldCountText)
                goldCountText.text = gold.ToString();
        }
        
        private void OnChangeGoldEvent(int beforeGold, int afterGold)
        {
            UpdateUI(afterGold);
        }
    }
}