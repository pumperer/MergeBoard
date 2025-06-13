using alpoLib.UI.Hud;
using MergeBoard.Data.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Hud
{
    public class HudItemLevelExp : HudItemBaseWithData
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text expText;
        [SerializeField] private Slider expSlider;

        protected override void OnAddEvent()
        {
            var userMapper = Data.GetUserDataLoader<IUserInfoMapper>();
            var (level, exp, maxExp) = userMapper.GetCurrentLevelExpInfo();
            UpdateUI(level, exp, maxExp);
            UserInfoData.OnChangeExpLevelEvent += OnChangeLevelExp;
        }
        
        protected override void OnRemoveEvent()
        {
            UserInfoData.OnChangeExpLevelEvent -= OnChangeLevelExp;
        }
        
        private void OnChangeLevelExp(int beforeLevel, int afterLevel, int afterExp, int afterMaxExp)
        {
            UpdateUI(afterLevel, afterExp, afterMaxExp);
        }
        
        private void UpdateUI(int level, int exp, int maxExp)
        {
            if (levelText)
                levelText.text = $"{level}";
            if (expSlider)
                expSlider.value = (float)exp / maxExp;
            if (expText)
                expText.text = $"{exp}/{maxExp}";
        }
        
        
    }
}