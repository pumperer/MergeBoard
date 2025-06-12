using alpoLib.UI.Scene;
using alpoLib.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MergeBoard.Scenes
{
    public class StartupScene : SceneBase
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            Addressables.InitializeAsync(true);
            alpoLib.Core.Module.Initialize();
            alpoLib.Data.Module.Initialize(new alpoLib.Data.DataModuleInitParam());
            CoroutineTaskManager.Init(true);
            TaskScheduler.Init(true);
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            _ = SceneManager.Initialize(this);
        }

        public override void OnOpen()
        {
            
        }
    }
}