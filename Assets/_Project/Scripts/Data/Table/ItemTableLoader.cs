using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Core.Foundation;
using alpoLib.Data;
using alpoLib.Data.Serialization;

namespace MergeBoard.Data.Table
{
    [Serializable]
    public record ItemBase : TableDataBase
    {
        [DataColumn("Category")]
        public string Category { get; set; }

        [DataColumn("Sequence")]
        public int Sequence { get; set; }

        [DataColumn("Energy")]
        public int Energy { get; set; }

        [DataColumn("MergeToSeq")]
        public int MergeToSeq { get; set; }

        [DataColumn("CanPop")]
        public CustomBoolean CanPop { get; set; }

        [DataColumn("PopCount")]
        public int PopCount { get; set; }

        [DataColumn("PopCooltime")]
        public int PopCooltime { get; set; }
		
        [DataColumn("SellValue")]
        public int SellValue { get; set; }

        [DataColumn("AtlasName")]
        public string AtlasName { get; set; }

        [DataColumn("SpriteName")]
        public string SpriteName { get; set; }
    }
    
    public interface IItemTableMapper : ITableDataMapperBase
    {
        List<ItemBase> GetItemBaseList(string category);
        ItemBase GetItemBase(int id);
    }
    
    [TableDataSheetName("ItemTable_Item")]
    public class ItemTableLoader : ThreadedTableDataLoader<ItemBase>, IItemTableMapper
    {
        private Dictionary<int, ItemBase> _itemBaseById;
        private Dictionary<string, List<ItemBase>> _itemBaseListByCategory;
        
        protected override void PostProcess(IEnumerable<ItemBase> loadedElementList)
        {
            _itemBaseById = loadedElementList.ToDictionary(b => b.Id, b => b);
            _itemBaseListByCategory = _itemBaseById.Values
                .GroupBy(b => b.Category)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<ItemBase> GetItemBaseList(string category)
        {
            return _itemBaseListByCategory.TryGetValue(category, out var itemList) ? itemList : null;
        }

        public ItemBase GetItemBase(int id)
        {
            return _itemBaseById.TryGetValue(id, out var itemBase) ? itemBase : null;
        }
    }
}