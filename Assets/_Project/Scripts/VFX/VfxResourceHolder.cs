using System;
using System.Collections.Generic;
using alpoLib.UI.Scene;
using alpoLib.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MergeBoard.VFX
{
    public class VfxResourceHolder : SingletonMonoBehaviour<VfxResourceHolder>, IDisposable
    {
        [SerializeField]
        private VfxResDefineDictionary vfxResDefines = new();
        
        public async Awaitable LoadAsync()
        {
            var values = vfxResDefines.Values;
            var list = new List<AsyncOperationHandle<GameObject>>(values.Count);
            foreach (var vfxResDefine in values)
            {
                var handle = vfxResDefine.Load();
                list.Add(handle);
            }

            await AddressableLoaderHelper.WhenAll(list.ToArray());
        }
        
        public VfxObjectBase Get(string key)
        {
            var obj = vfxResDefines.TryGetValue(key, out var vfxResDefine) ? vfxResDefine.Get() : null;
            obj?.SetKey(key);
            return obj;
        }
        
        public T Get<T>(string key) where T : VfxObjectBase
        {
            return Get(key) as T;
        }
        
        public void Release(string key, VfxObjectBase obj)
        {
            if (vfxResDefines.TryGetValue(key, out var vfxResDefine))
            {
                vfxResDefine.Release(obj);
            }
        }

        public void Dispose()
        {
            foreach (var vfxResDefine in vfxResDefines.Values)
            {
                vfxResDefine.Dispose();
            }
        }
        
        [Serializable]
        public class VfxResDefine : IObjectPool<VfxObjectBase>, IDisposable
        {
            [SerializeField] private AssetReferenceGameObject vfxPrefab;
            [SerializeField] private int preloadCount;
        
            private DefaultObjectPool<VfxObjectBase> _vfxObjectPool;

            public AsyncOperationHandle<GameObject> Load()
            {
                var handle = vfxPrefab.LoadAssetAsync();
                handle.Completed += operationHandle =>
                {
                    var prefab = operationHandle.Result;
                    _vfxObjectPool = new DefaultObjectPool<VfxObjectBase>(prefab);
                    _vfxObjectPool.Preload(preloadCount);
                };
                return handle;
            }

            public VfxObjectBase Get()
            {
                return _vfxObjectPool?.Get();
            }

            public void Release(VfxObjectBase obj)
            {
                obj.transform.parent = SceneManager.CurrentScene.transform;
                obj.transform.parent = null;
                _vfxObjectPool?.Release(obj);
            }

            public void Dispose()
            {
                _vfxObjectPool?.Dispose();
                vfxPrefab.ReleaseAsset();
            }
        }
        
        [Serializable]
        public class VfxResDefineDictionary : SerializableDictionary<string, VfxResDefine>
        {
        }
    }
}