using System;
using alpoLib.Data.Serialization;
using MergeBoard.Utility;
using UnityEngine;

namespace MergeBoard.Data
{
    public interface IRewardReceiver
    {
        void Receive(Reward reward);
    }
    
    [Serializable]
    public record Reward
    {
        [DataColumn("RewardType")]
        public RewardType Type { get; set; }
        
        [DataColumn("RewardId")]
        public int Id { get; set; }
        
        [DataColumn("RewardValue")]
        public int Value { get; set; }

        public string GetName()
        {
            return LocalizationManager.Instance.GetString("UI_Label_Reward_{Type}");
        }
    }
}