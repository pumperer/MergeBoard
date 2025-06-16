using System;
using MergeBoard.UI.Scenes;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature.UI
{
    public interface IInGameFeatureUI
    {
        Type GetPairFeatureType();
        void OnAttachFeature(IInGameFeature feature);
        void OnOpen();
        void OnClose();
    }
    
    public abstract class InGameFeatureUIBase<T> : IInGameFeatureUI where T : IInGameFeature
    {
        protected InGameFeatureUIBase(BoardSceneUI sceneUI)
        {
        }
        
        public Type GetPairFeatureType()
        {
            return typeof(T);
        }

        public virtual void OnAttachFeature(IInGameFeature feature)
        {
        }

        public virtual void OnOpen()
        {
        }

        public virtual void OnClose()
        {
        }
    }
}