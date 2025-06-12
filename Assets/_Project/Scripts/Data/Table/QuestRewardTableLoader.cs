using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record QuestRewardBase : TableDataBase
    {
        [DataColumn("QuestId")]
        public int QuestId { get; set; }
        
        [DataCompoundType]
        public Reward Reward { get; set; }
    }

    public interface IQuestRewardTableMapper : ITableDataMapperBase
    {
        List<QuestRewardBase> GetQuestRewards(int questId);
    }
    
    [TableDataSheetName("QuestTable_QuestReward")]
    public class QuestRewardTableLoader : ThreadedTableDataLoader<QuestRewardBase>, IQuestRewardTableMapper
    {
        private Dictionary<int, List<QuestRewardBase>> _questRewardMapByQuestId;
        
        protected override void PostProcess(IEnumerable<QuestRewardBase> loadedElementList)
        {
            _questRewardMapByQuestId = loadedElementList.GroupBy(b => b.QuestId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<QuestRewardBase> GetQuestRewards(int questId)
        {
            _questRewardMapByQuestId.TryGetValue(questId, out var result);
            return result;
        }
    }
}