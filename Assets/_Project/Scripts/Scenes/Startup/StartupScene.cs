using alpoLib.UI.Scene;
using alpoLib.Util;
using MergeBoard.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MergeBoard.Scenes
{
    public class StartupScene : SceneBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            Addressables.InitializeAsync(true).WaitForCompletion();
            LocalizationManager.Init(true);
            LocalizationManager.Instance.Initialize("Default");
            OnDemandAtlasManager.Init(true);
            OnDemandAtlasManager.Instance.LoadPreloadAtlases();
            alpoLib.Core.Module.Initialize();
            alpoLib.Data.Module.Initialize(new alpoLib.Data.DataModuleInitParam());
            CoroutineTaskManager.Init(true);
            TaskScheduler.Init(true);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            AwaitableHelper.Run(() => SceneManager.Initialize(this));
        }

        public override void OnOpen()
        {
            AwaitableHelper.Run(() => SceneManager.Instance.OpenSceneAsync<SplashScene>());
        }
    }
}