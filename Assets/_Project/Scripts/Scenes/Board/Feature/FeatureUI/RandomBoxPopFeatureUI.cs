using System;
using MergeBoard.UI.Scenes;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature.UI
{
    public interface IRandomBoxPop
    {
        void SetRandomBoxPopEvent(Action<Vector3> onRandomBoxPop);
    }
    
    public class RandomBoxPopFeatureUI : InGameFeatureUIBase<RandomBoxPopFeature>
    {
        private IRandomBoxPop _randomBoxPop;
        
        public RandomBoxPopFeatureUI(BoardSceneUI sceneUI, IRandomBoxPop randomBoxPop) : base(sceneUI)
        {
            _randomBoxPop = randomBoxPop;
        }

        public void SetRandomBoxPopEvent(Action<Vector3> onRandomBoxPop)
        {
            _randomBoxPop?.SetRandomBoxPopEvent(onRandomBoxPop);
        }
    }
}