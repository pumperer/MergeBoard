using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Core.Foundation;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record BoardDefineBase : TableDataBase
    {
        [DataColumn("Name")]
        public string Name { get; set; }
        
        [DataColumn("BoardType")]
        public BoardType BoardType { get; set; }
        
        [DataColumn("NameStringKey")]
        public string NameStringKey { get; set; }
        
        [DataColumn("BGMKey")]
        public Sound.BGMKey BGMKey { get; set; }
        
        [DataColumn("IsDefault")]
        public CustomBoolean IsDefault { get; set; }
    }

    public interface IBoardDefineTableMapper : ITableDataMapperBase
    {
        BoardDefineBase GetBoardDefineBase(int boardId);
        List<BoardDefineBase> GetBoardDefineBaseList();
    }
    
    [TableDataSheetName("BoardTable_BoardDefine")]
    public class BoardDefineTableLoader : ThreadedTableDataLoader<BoardDefineBase>, IBoardDefineTableMapper
    {
        private List<BoardDefineBase> boardDefineBaseList;
        
        protected override void PostProcess(IEnumerable<BoardDefineBase> loadedElementList)
        {
            boardDefineBaseList = new List<BoardDefineBase>(loadedElementList);
        }

        public BoardDefineBase GetBoardDefineBase(int boardId)
        {
            return boardDefineBaseList.FirstOrDefault(board => board.Id == boardId);
        }
        
        public List<BoardDefineBase> GetBoardDefineBaseList()
        {
            return boardDefineBaseList;
        }
    }
}