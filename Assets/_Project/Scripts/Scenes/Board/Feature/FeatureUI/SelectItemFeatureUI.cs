using System;
using MergeBoard.UI.Scenes;

namespace MergeBoard.Scenes.Board.Feature.UI
{
    public interface ISelectItemInfo
    {
        void OnSelectItem(Item item);
        void UpdateSelectItem(Item item);
        void SetSellItemEvent(Action<Item> onSellItem);
    }
    
    public class SelectItemFeatureUI : InGameFeatureUIBase<SelectItemFeature>
    {
        private readonly ISelectItemInfo _itemInfo;
        
        public SelectItemFeatureUI(BoardSceneUI sceneUI, ISelectItemInfo itemInfo) : base(sceneUI)
        {
            _itemInfo = itemInfo;
        }

        public void OnSelectItem(Item item)
        {
            _itemInfo?.OnSelectItem(item);
        }
        
        public void UpdateSelectItem(Item item)
        {
            _itemInfo?.UpdateSelectItem(item);
        }

        public void SetSellItemEvent(Action<Item> onSellItem)
        {
            _itemInfo?.SetSellItemEvent(onSellItem);
        }
    }
}