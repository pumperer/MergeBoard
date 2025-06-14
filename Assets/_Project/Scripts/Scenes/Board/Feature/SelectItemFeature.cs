using MergeBoard.Scenes.Board.Feature.UI;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public class SelectItemFeature : MergeFeatureBase
    {
        private SelectItemFeatureUI UI => FeatureUI as SelectItemFeatureUI;
        
        public SelectItemFeature(MergeBoard board) : base(board)
        {
        }

        public override void OnOpen()
        {
            base.OnOpen();
            UI?.SetSellItemEvent(OnSellItem);
        }

        public override void OnSelect(Item item)
        {
            UI?.OnSelectItem(item);
        }

        private void OnSellItem(Item item)
        {
            SoundManager.Instance.PlaySFX(SFXKey.sfx_sell);
            Board.SellItem(item);
        }
    }
}