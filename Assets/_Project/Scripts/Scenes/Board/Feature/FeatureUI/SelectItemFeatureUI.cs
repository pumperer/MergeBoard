using MergeBoard.UI.Scenes;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature.UI
{
    public interface ISelectItemInfo
    {
        void OnSelectItem(Item item);
    }
    
    public class SelectItemFeatureUI : MergeFeatureUIBase<SelectItemFeature>
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
    }
}