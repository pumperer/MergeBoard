using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record BoardInitialDataBase : TableDataBase
    {
        [DataColumn("BoardId")]
        public int BoardId { get; set; }
        
        [DataColumn("X")]
        public int X { get; set; }

        [DataColumn("Y")]
        public int Y { get; set; }

        [DataColumn("ItemId")]
        public int ItemId { get; set; }
    }

    public interface IBoardInitialDataTableMapper : ITableDataMapperBase
    {
        List<BoardInitialDataBase> GetBoardInitialDataBaseList(int boardId);
    }
    
    [TableDataSheetName("BoardTable_BoardInitialData")]
    public class BoardInitialDataTableLoader : ThreadedTableDataLoader<BoardInitialDataBase>, IBoardInitialDataTableMapper
    {
        private Dictionary<int, List<BoardInitialDataBase>> _boardInitialDataMap;
        
        protected override void PostProcess(IEnumerable<BoardInitialDataBase> loadedElementList)
        {
            _boardInitialDataMap = loadedElementList
                .GroupBy(b => b.BoardId)
                .ToDictionary(b => b.Key, g => g.ToList());
        }

        public List<BoardInitialDataBase> GetBoardInitialDataBaseList(int boardId)
        {
            _boardInitialDataMap.TryGetValue(boardId, out var boardInitialDataList);
            return boardInitialDataList;
        }
    }
}