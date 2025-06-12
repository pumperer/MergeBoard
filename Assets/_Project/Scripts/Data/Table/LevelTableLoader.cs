using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Data.Serialization;
using UnityEngine;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record LevelBase : TableDataBase
    {
        [DataColumn("Level")]
        public int Level { get; set; }
        
        [DataColumn("MaxExp")]
        public int MaxExp { get; set; }
    }

    public interface ILevelTableMapper : ITableDataMapperBase
    {
        LevelBase GetLevelBase(int level);
    }
    
    [TableDataSheetName("LevelTable_Level")]
    public class LevelTableLoader : ThreadedTableDataLoader<LevelBase>, ILevelTableMapper
    {
        private Dictionary<int, LevelBase> _levelBaseMap;
        
        protected override void PostProcess(IEnumerable<LevelBase> loadedElementList)
        {
            _levelBaseMap = loadedElementList.ToDictionary(b => b.Level, b => b);
        }

        public LevelBase GetLevelBase(int level)
        {
            return _levelBaseMap.TryGetValue(level, out var result) ? result : null;
        }
    }
}