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
        [DataCompoundElement("RewardType")]
        public RewardType Type { get; set; }
        
        [DataCompoundElement("RewardId")]
        public int Id { get; set; }
        
        [DataCompoundElement("RewardValue")]
        public int Value { get; set; }

        public (string atlasKey, string spriteKey) GetSpriteAtlasKey()
        {
            const string atlasKey = "Addr/Atlas/UIAtlas.spriteatlasv2";
            return Type switch
            {
                RewardType.Energy => (atlasKey, $"Icon_Energy"),
                RewardType.Gold => (atlasKey, "Icon_Gold"),
                RewardType.Exp => (atlasKey, "Icon_Level"),
                _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, null)
            };
        }
        
        public string GetName()
        {
            return LocalizationManager.Instance.GetString($"UI_Label_Reward_{Type}");
        }
    }
}