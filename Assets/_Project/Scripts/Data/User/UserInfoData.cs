using System;
using System.Data;
using alpoLib.Data;
using alpoLib.Util;
using MergeBoard.Data.Table;
using UnityEngine;

namespace MergeBoard.Data.User
{
    public record UserInfo
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public int Level { get; set; } = 1;

        public int Exp { get; set; } = 0;

        public int Energy { get; set; } = 100;

        public int Gold { get; set; } = 500;

        public DateTimeOffset EnergyNextChargeTime { get; set; }
    }
    
    public interface IUserInfoMapper : IUserDataMapperBase, IRewardReceiver
    {
        (int level, int exp, int maxExp) GetCurrentLevelExpInfo();
        int AddExp(int exp);
        
        int AddEnergy(int energy);
        int GetEnergyCapacity();
        int GetEnergyCount();
        DateTimeOffset GetEnergyNextChargeTime();
        
        int GetGold();
        int AddGold(int gold);
    }
    
    public class UserInfoData : UserDataManagerBase, IUserInfoMapper
    {
        [SerializeField]
        private UserInfo _userInfo;
        
        public delegate void OnChangeExpLevel(int beforeLevel, int afterLevel, int afterExp, int afterMaxExp);
        public static event OnChangeExpLevel OnChangeExpLevelEvent;

        public delegate void OnChangeEnergyDelegate(int totalEnergy);
        public static event OnChangeEnergyDelegate OnChangeEnergyEvent;

        public delegate void OnChangeGoldDelegate(int beforeGold, int afterGold);
        public static event OnChangeGoldDelegate OnChangeGoldEvent;
        
        private RechargeTimer _energyRechargeTimer;
        private TaskScheduler.ITask _energyRechargeTask;

        private ILevelTableMapper _levelMapper;
        
        public override void OnCreateInstance()
        {
            _levelMapper = TableDataManager.GetLoader<ILevelTableMapper>();
        }

        public override void CreateNewUser()
        {
            _userInfo = new();
        }

        public override void OnInitialize()
        {
            InitEnergyRechargeTimer();
        }

        #region IRewardReceiver
        
        public void Receive(Reward reward)
        {
            switch (reward.Type)
            {
                case RewardType.Exp:
                    AddExp(reward.Value);
                    break;

                case RewardType.Energy:
                    AddEnergy(reward.Value);
                    break;

                case RewardType.Gold:
                    AddGold(reward.Value);
                    break;
            }
        }
        
        #endregion

        #region Level & Exp
        
        public (int level, int exp, int maxExp) GetCurrentLevelExpInfo()
        {
            var currentLevelData = _levelMapper.GetLevelBase(_userInfo.Level);
            return (_userInfo.Level, _userInfo.Exp, currentLevelData.MaxExp);
        }

        public int AddExp(int exp)
        {
            var beforeLevel = _userInfo.Level;
            var	currentLevel = beforeLevel;
            var beforeExp = _userInfo.Exp;
            var currentExp = beforeExp;

            var currentLevelData = _levelMapper.GetLevelBase(currentLevel);
            var currentMaxExp = currentLevelData.MaxExp;
            currentExp += exp;
			
            while (currentLevelData.MaxExp <= currentExp)
            {
                currentExp -= currentMaxExp;
                currentLevel++;
                currentLevelData = _levelMapper.GetLevelBase(currentLevel);

                if (currentLevelData == null)
                {
                    currentLevel--;
                    currentExp = currentMaxExp;
                    break;
                }
                else
                    currentMaxExp = currentLevelData.MaxExp;
            }
			
            _userInfo.Exp = currentExp;
            _userInfo.Level = currentLevel;

            if (beforeExp != currentExp || beforeLevel != currentLevel)
                OnChangeExpLevelEvent?.Invoke(beforeLevel, currentLevel, currentExp, currentMaxExp);

            return currentLevel;
        }
        
        #endregion

        #region Energy
        
        private void InitEnergyRechargeTimer()
        {
            if (_energyRechargeTask != null)
                TaskScheduler.Cancel(_energyRechargeTask.Name);
            
            _energyRechargeTimer = new RechargeTimer();
            _energyRechargeTimer.Initialize(new RechargeTimerCreateContext
            {
                ValueGetter = () => _userInfo?.Energy ?? 0,
                ValueSetter = (v, fromCharge) =>
                {
                    if (_userInfo == null)
                        return;
                    _userInfo.Energy = v;
                    OnChangeEnergyEvent?.Invoke(_userInfo.Energy);
                },
                NextChargeTickGetter = () => _userInfo?.EnergyNextChargeTime ?? DateTimeOffset.MinValue,
                NextChargeTickSetter = v =>
                {
                    if (_userInfo == null)
                        return;
                    _userInfo.EnergyNextChargeTime = v;
                },
                NowGetter = () => DateTimeOffset.Now,

                MaxValue = 100,
                ChargeIntervalSeconds = 2 * 60,
                ChargeAmount = 1,
                CooltimeSeconds = 0
            });
            
            _energyRechargeTask = TaskScheduler.CreateTask("EnergyRechargeTimer", UpdateRechargeTimer, 0.1f);
        }
        
        private void UpdateRechargeTimer()
        {
            _energyRechargeTimer?.Calc();
        }
        
        public int AddEnergy(int energy)
        {
            if (_energyRechargeTimer == null)
                return 0;
            
            if (energy > 0)
            {
                _energyRechargeTimer.Add(energy);
            }
            else if (energy < 0)
            {
                _energyRechargeTimer.Use(-energy);
            }

            return _energyRechargeTimer.CurrentValue;
        }

        public int GetEnergyCapacity()
        {
            return _energyRechargeTimer?.GetCapacity() ?? 0;
        }

        public int GetEnergyCount()
        {
            return _energyRechargeTimer?.CurrentValue ?? 0;
        }

        public DateTimeOffset GetEnergyNextChargeTime()
        {
            return _energyRechargeTimer.GetDateTimeNextChargeTime();
        }
        
        #endregion

        #region Gold
        
        public int GetGold()
        {
            return _userInfo?.Gold ?? 0;
        }

        public int AddGold(int gold)
        {
            if (_userInfo == null)
                return 0;
            
            var beforeGold = _userInfo.Gold;
            if (gold > 0)
            {
                _userInfo.Gold += gold;
            }
            else if (gold < 0)
            {
                _userInfo.Gold += gold;
                _userInfo.Gold = Math.Max(_userInfo.Gold, 0);
            }

            if (beforeGold != _userInfo.Gold)
                OnChangeGoldEvent?.Invoke(beforeGold, _userInfo.Gold);

            return _userInfo.Gold;
        }
        
        #endregion
    }
}