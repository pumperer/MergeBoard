using System.Collections.Generic;
using alpoLib.UI;
using alpoLib.UI.Hud;
using MergeBoard.Scenes.Board.Feature.UI;
using UnityEngine;
using MB = MergeBoard.Scenes.Board.MergeBoard;

namespace MergeBoard.UI.Scenes
{
    public class BoardSceneUI : SceneUIBase
    {
        private readonly HashSet<IMergeFeatureUI> _uiFeatures = new();
        private HudItemController _hudItemController;

        public override void OnInitialize()
        {
            base.OnInitialize();
            _hudItemController = new HudItemController(GetComponentsInChildren<HudItemBase>(true));
        }
        
        private void AddFeatureUI(IMergeFeatureUI featureUI)
        {
            _uiFeatures.Add(featureUI);
        }
        
        public void InitFeatures()
        {
            AddFeatureUI(new SelectItemFeatureUI(this, _hudItemController.FindFirstHudItem<ISelectItemInfo>()));
            AddFeatureUI(new RandomBoxPopFeatureUI(this, _hudItemController.FindFirstHudItem<IRandomBoxPop>()));
            
            foreach (var featureUI in _uiFeatures)
                InitOneFeature(featureUI);
        }

        private void InitOneFeature(IMergeFeatureUI featureUI)
        {
            var feature = MB.Instance.GetFeature(featureUI.GetPairFeatureType());
            if (feature == null)
            {
                featureUI.OnClose();
                return;
            }

            feature.AttachFeatureUI(featureUI);
            featureUI.OnAttachFeature(feature);
        }
    }
}