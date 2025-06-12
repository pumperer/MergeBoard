using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;
using UnityEngine;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record PopProbabilityBase : TableDataBase
    {
        [DataColumn("PopType")]
        public PopType PopType { get; set; }
        
        [DataColumn("ItemId")]
        public int ItemId { get; set; }

        [DataColumn("PopItemId")]
        public int PopItemId { get; set; }

        [DataColumn("Probability")]
        public int Probability { get; set; }
    }

    public interface IPopProbabilityTableMapper : ITableDataMapperBase
    {
        int GetPopItemIdByRandom(PopType popType, int itemId);
    }
    
    [TableDataSheetName("PopProbabilityTable_PopProbability")]
    public class PopProbabilityTableLoader : ThreadedTableDataLoader<PopProbabilityBase>, IPopProbabilityTableMapper
    {
        private Dictionary<PopType, Dictionary<int, List<PopProbabilityBase>>> _popProbabilityMap;
        
        protected override void PostProcess(IEnumerable<PopProbabilityBase> loadedElementList)
        {
            _popProbabilityMap = loadedElementList.GroupBy(b => b.PopType)
                .ToDictionary(
                    g => g.Key,
                    g => g.GroupBy(b => b.ItemId)
                        .ToDictionary(
                            gg => gg.Key,
                            gg => gg.ToList()));
        }

        public int GetPopItemIdByRandom(PopType popType, int itemId)
        {
            if (!_popProbabilityMap.TryGetValue(popType, out var itemMap))
                return 0;
            if (!itemMap.TryGetValue(itemId, out var itemList))
                return 0;
            itemList.Sort((l, r) => l.Probability.CompareTo(r.Probability));
            var sumProb = itemList.Sum(l => l.Probability);
            var random = new System.Random();
            var result = random.Next(sumProb);
            
            var currentProb = 0;
            var popItemId = 0;
            foreach (var item in itemList)
            {
                currentProb += item.Probability;
                if (result >= currentProb)
                    continue;
                
                popItemId = item.PopItemId;
                break;
            }
            return popItemId;
        }
    }
}