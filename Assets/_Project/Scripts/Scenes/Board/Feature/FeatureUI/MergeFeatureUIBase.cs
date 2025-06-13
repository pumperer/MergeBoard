using System;
using MergeBoard.UI.Scenes;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature.UI
{
    public interface IMergeFeatureUI
    {
        Type GetPairFeatureType();
        void OnAttachFeature(IMergeFeature feature);
        void OnOpen();
        void OnClose();
    }
    
    public abstract class MergeFeatureUIBase<T> : IMergeFeatureUI where T : IMergeFeature
    {
        protected MergeFeatureUIBase(BoardSceneUI sceneUI)
        {
        }
        
        public Type GetPairFeatureType()
        {
            return typeof(T);
        }

        public virtual void OnAttachFeature(IMergeFeature feature)
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