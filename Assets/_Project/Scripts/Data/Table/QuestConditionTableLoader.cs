using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record QuestCondition
    {
        [DataColumn("ConditionType")]
        public string ConditionType { get; set; }

        [DataColumn("ConditionId")]
        public int ConditionId { get; set; }

        [DataColumn("ConditionValue")]
        public int ConditionValue { get; set; }
    }
    
    [Serializable]
    public record QuestConditionBase : TableDataBase
    {
        [DataColumn("QuestId")]
        public int QuestId { get; set; }

        [DataCompoundType]
        public QuestCondition Condition { get; set; }
    }

    public interface IQuestConditionTableMapper : ITableDataMapperBase
    {
        List<QuestConditionBase> GetQuestConditions(int questId);
    }
    
    [TableDataSheetName("QuestTable_QuestCondition")]
    public class QuestConditionTableLoader : ThreadedTableDataLoader<QuestConditionBase>, IQuestConditionTableMapper
    {
        private Dictionary<int, List<QuestConditionBase>> _questConditionsByQuestId;
        
        protected override void PostProcess(IEnumerable<QuestConditionBase> loadedElementList)
        {
            _questConditionsByQuestId = loadedElementList.GroupBy(b => b.QuestId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<QuestConditionBase> GetQuestConditions(int questId)
        {
            _questConditionsByQuestId.TryGetValue(questId, out var result);
            return result;
        }
    }
}