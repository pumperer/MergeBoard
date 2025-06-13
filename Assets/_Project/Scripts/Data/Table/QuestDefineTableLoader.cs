using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record QuestDefineBase : TableDataBase
    {
        [DataColumn("Key")]
        public string Key { get; set; }

        [DataColumn("BoardId")]
        public int BoardId { get; set; }
    }
    
    public interface IQuestDefineTableMapper : ITableDataMapperBase
    {
        QuestDefineBase GetQuestDefineBase(int boardId, int questId);
        QuestDefineBase GetRandomQuestDefine(int boardId);
    }
    
    [TableDataSheetName("QuestTable_QuestDefine")]
    public class QuestDefineTableLoader : ThreadedTableDataLoader<QuestDefineBase>, IQuestDefineTableMapper
    {
        private Dictionary<int, List<QuestDefineBase>> _questDefinesByBoardId;
        
        protected override void PostProcess(IEnumerable<QuestDefineBase> loadedElementList)
        {
            _questDefinesByBoardId = loadedElementList
                .GroupBy(b => b.BoardId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public QuestDefineBase GetQuestDefineBase(int boardId, int questId)
        {
            if (!_questDefinesByBoardId.TryGetValue(boardId, out var list))
                return null;
            return list.FirstOrDefault(q => q.Id == questId);
        }
        
        public QuestDefineBase GetRandomQuestDefine(int boardId)
        {
            if (!_questDefinesByBoardId.TryGetValue(boardId, out var list))
                return null;
            return list.GetRandom();
        }
    }
}