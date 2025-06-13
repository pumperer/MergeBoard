using MergeBoard.Scenes.Board.Feature.UI;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class SelectItemFeature : MergeFeatureBase
    {
        private SelectItemFeatureUI UI => FeatureUI as SelectItemFeatureUI;
        
        public SelectItemFeature(MergeBoard board) : base(board)
        {
        }

        public override void OnSelect(Item item)
        {
            UI?.OnSelectItem(item);
        }
    }
}