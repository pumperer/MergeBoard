using System;
using System.Collections;
using System.Collections.Generic;
using alpoLib.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
#if USE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace MergeBoard.Utility
{
    public struct SpriteAtlasRequest
    {
        public string AtlasKey;
        public string SpriteName;
        public int RequestKey;
    }

    public struct SpriteAtlasResult
    {
        public SpriteAtlas Atlas;
        public Sprite Sprite;
        public int RequestKey;
    }

    public class OnDemandAtlasManager : SingletonMonoBehaviour<OnDemandAtlasManager>
    {
        private class SpriteAtlasHolder
        {
            public string AtlasKey;
            public SpriteAtlas Atlas;
            public bool IsLoadingComplete;
        }

        private Dictionary<int, SpriteAtlasRequest> requestMap = new();
        private Dictionary<string, SpriteAtlasHolder> holderMap = new();
        
        public void LoadPreloadAtlases()
        {
            var preloadAtlases = new List<string>
            {
                "Addr/Atlas/UIAtlas.spriteatlasv2",
            };
            foreach (var atlasKey in preloadAtlases)
            {
                StartCoroutine(LoadAtlas(new SpriteAtlasRequest
                {
                    AtlasKey = atlasKey,
                    SpriteName = string.Empty,
                    RequestKey = 0,
                }, result => { }));
            }
            // LoadAtlas(new SpriteAtlasRequest
            // {
            //     AtlasKey = "Addressable/Atlases/Atlas1.spriteatlasv2",
            //     SpriteName = string.Empty,
            //     RequestKey = 0,
            // });
            // LoadAtlas(new SpriteAtlasRequest
            // {
            //     AtlasKey = "Addressable/Atlases/Atlas2.spriteatlasv2",
            //     SpriteName = string.Empty,
            //     RequestKey = 0,
            // });
        }
        
        private void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += OnAtlasRequested;
        }

        private void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= OnAtlasRequested;
        }

#if USE_UNITASK
        private async void OnAtlasRequested(string key, Action<SpriteAtlas> callback)
        {
            var addr = ZString.Format("Addressable/Atlases/{0}.spriteatlasv2", key);
            var sa = await LoadAtlas(new SpriteAtlasRequest
            {
                AtlasKey = addr,
                SpriteName = string.Empty,
                RequestKey = 0,
            });
            callback.Invoke(sa.Atlas);
        }

        public async UniTask<SpriteAtlasResult> GetSpriteAsync(SpriteAtlasRequest request)
        {
            var result = await LoadAtlas(request);
            result.Sprite = result.Atlas.GetSprite(request.SpriteName);
            return result;
        }

        private async UniTask<SpriteAtlasResult> LoadAtlas(SpriteAtlasRequest request)
        {
            if (holderMap.TryGetValue(request.AtlasKey, out var context))
            {
                await UniTask.WaitUntil(() => context.IsLoadingComplete);
                return new SpriteAtlasResult
                {
                    Atlas = context.Atlas,
                    RequestKey = request.RequestKey,
                };
            }

            context = new SpriteAtlasHolder
            {
                AtlasKey = request.AtlasKey,
                IsLoadingComplete = false,
            };
            holderMap.Add(request.AtlasKey, context);

            var atlas = await Addressables.LoadAssetAsync<SpriteAtlas>(request.AtlasKey);
            context.Atlas = atlas;
            context.IsLoadingComplete = true;

            return new SpriteAtlasResult
            {
                Atlas = atlas,
                RequestKey = request.RequestKey,
            };
        }
#else
        public void GetSprite(SpriteAtlasRequest request, Action<SpriteAtlasResult> callback)
        {
            StartCoroutine(LoadAtlas(request, result =>
            {
                result.Sprite = result.Atlas.GetSprite(request.SpriteName);
                callback?.Invoke(result);
            }));
        }

        public void GetAtlas(string key, int requestKey, Action<SpriteAtlas> callback)
        {
            OnAtlasRequested(key, requestKey, atlas => { callback?.Invoke(atlas); });
        }

        private void OnAtlasRequested(string key, Action<SpriteAtlas> callback)
        {
            OnAtlasRequested(key, 0, callback);
        }

        private void OnAtlasRequested(string key, int requestKey, Action<SpriteAtlas> callback)
        {
            var addr = $"Addrs/Atlas/{key}.spriteatlasv2";
            var request = new SpriteAtlasRequest
            {
                AtlasKey = addr,
                SpriteName = string.Empty,
                RequestKey = requestKey,
            };

            StartCoroutine(LoadAtlas(request, result => callback?.Invoke(result.Atlas)));
        }

        private IEnumerator LoadAtlas(SpriteAtlasRequest request, Action<SpriteAtlasResult> callback)
        {
            if (holderMap.TryGetValue(request.AtlasKey, out SpriteAtlasHolder context))
            {
                yield return new WaitUntil(() => context.IsLoadingComplete);
                callback?.Invoke(new SpriteAtlasResult
                {
                    Atlas = context.Atlas,
                    RequestKey = request.RequestKey,
                });
                yield break;
            }

            context = new SpriteAtlasHolder
            {
                AtlasKey = request.AtlasKey,
                IsLoadingComplete = false,
            };
            holderMap.Add(request.AtlasKey, context);

            AsyncOperationHandle<SpriteAtlas> h = Addressables.LoadAssetAsync<SpriteAtlas>(request.AtlasKey);
            while (!h.IsDone)
            {
                yield return null;
            }

            SpriteAtlas atlas = h.Result;
            context.Atlas = atlas;
            context.IsLoadingComplete = true;

            callback?.Invoke(new SpriteAtlasResult
            {
                Atlas = atlas,
                RequestKey = request.RequestKey,
            });

            Addressables.Release(h);
        }
#endif
    }
}
