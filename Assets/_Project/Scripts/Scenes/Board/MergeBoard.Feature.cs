using System;
using System.Collections.Generic;
using MergeBoard.Scenes.Board.Feature;
using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    public partial class MergeBoard
    {
        private HashSet<IInGameFeature> _features = new();
        private Dictionary<Type, IInGameFeature> _featureDic = new();
        
        #region Feature Management
        
        private void AddFeature(InGameFeatureBase feature)
        {
            _features.Add(feature);
            _featureDic.Add(feature.GetType(), feature);
        }

        public IInGameFeature GetFeature(Type featureType)
        {
            if (_featureDic.TryGetValue(featureType, out var feature))
                return feature;
            return null;
        }

        public T GetFeature<T>() where T : IInGameFeature
        {
            return (T)GetFeature(typeof(T));
        }
        
        #endregion

        #region Feature Broadcast
        
        private void OnSelectItem(Item item)
        {
            foreach (var feature in _features)
                feature.OnSelect(item);
        }

        private void OnPop(Item fromItem, Item newItem)
        {
            if (!newItem)
                return;
            
            foreach (var feature in _features)
                feature.OnPop(fromItem, newItem);
        }
        
        public void OnRandomBoxPop(Item newItem)
        {
            foreach (var feature in _features)
                feature.OnRandomBoxPop(newItem);
        }
        
        #endregion
    }
}