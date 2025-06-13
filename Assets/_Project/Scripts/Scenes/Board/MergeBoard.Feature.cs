using System;
using System.Collections.Generic;
using MergeBoard.Scenes.Board.Feature;
using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    public partial class MergeBoard
    {
        private HashSet<IMergeFeature> _features = new();
        private Dictionary<Type, IMergeFeature> _featureDic = new();
        
        #region Feature Management
        
        private void AddFeature(MergeFeatureBase feature)
        {
            _features.Add(feature);
            _featureDic.Add(feature.GetType(), feature);
        }

        public IMergeFeature GetFeature(Type featureType)
        {
            if (_featureDic.TryGetValue(featureType, out var feature))
                return feature;
            return null;
        }

        public T GetFeature<T>() where T : IMergeFeature
        {
            return (T)GetFeature(typeof(T));
        }
        
        #endregion

        #region Feature Broadcast
        
        private void OnSelectItem(Item item)
        {
            foreach (var feature in _features)
            {
                feature.OnSelect(item);
            }
        }
        
        #endregion
    }
}