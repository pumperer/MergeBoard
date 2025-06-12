using System;
using alpoLib.Data.Serialization;
using UnityEngine;

namespace MergeBoard.Data
{
    [Serializable]
    public record Reward
    {
        [DataColumn("RewardType")]
        public RewardType Type { get; set; }
        
        [DataColumn("RewardId")]
        public int Id { get; set; }
        
        [DataColumn("RewardValue")]
        public int Value { get; set; }
    }
}