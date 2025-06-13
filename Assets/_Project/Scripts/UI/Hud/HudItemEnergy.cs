using System;
using alpoLib.UI.Hud;
using alpoLib.Util;
using MergeBoard.Data.User;
using MergeBoard.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Hud
{
    public class HudItemEnergy : HudItemBaseWithData
    {
        [SerializeField] private TMP_Text energyCountText;
        [SerializeField] private TMP_Text chargeRemainTimeText;

        private IUserInfoMapper _userInfoMapper;
        private TaskScheduler.ITask _timeUpdateTask;

        protected override void OnAddEvent()
        {
            base.OnAddEvent();
            if (_timeUpdateTask != null)
                TaskScheduler.Cancel(_timeUpdateTask.Name);
            _timeUpdateTask = TaskScheduler.CreateTask("HudItemEnergy.UpdateUI", UpdateUI, 0.1f);
        }

        protected override void OnRemoveEvent()
        {
            base.OnRemoveEvent();
            if (_timeUpdateTask != null)
                TaskScheduler.Cancel(_timeUpdateTask.Name);
            _timeUpdateTask = null;
        }

        private void UpdateUI()
        {
            var nextChargeRemainTime = TimeSpan.Zero;
            _userInfoMapper ??= Data.GetUserDataLoader<IUserInfoMapper>();
            if (_userInfoMapper.GetEnergyCapacity() > 0)
            {
                nextChargeRemainTime = _userInfoMapper.GetEnergyNextChargeTime() - DateTime.Now;
                if (nextChargeRemainTime < TimeSpan.Zero)
                    nextChargeRemainTime = TimeSpan.Zero;
            }

            if (energyCountText)
                energyCountText.text = $"{_userInfoMapper.GetEnergyCount()}";

            if (nextChargeRemainTime <= TimeSpan.Zero)
            {
                if (chargeRemainTimeText)
                    chargeRemainTimeText.text = string.Empty;
            }
            else
            {
                if (chargeRemainTimeText)
                    chargeRemainTimeText.text = LocalizationManager.MakeTimeSpanString(nextChargeRemainTime);
            }
        }
    }
}