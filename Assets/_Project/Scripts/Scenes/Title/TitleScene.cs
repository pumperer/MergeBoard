using System.Collections.Generic;
using alpoLib.Data;
using alpoLib.UI.Scene;
using alpoLib.Util;
using MergeBoard.Data.Table;
using MergeBoard.Sound;
using MergeBoard.UI;
using MergeBoard.UI.Scenes;
using UnityEngine;

namespace MergeBoard.Scenes
{
    [SceneDefine("Addr/Scenes/TitleScene.unity", SceneResourceType.Addressable)]
    public class TitleScene : SceneBaseWithUI<TitleSceneUI>, ILoadingProgressChangeListener
    {
        private LoadingTaskMachine _loadingTaskMachine;
        
        public override void OnOpen()
        {
            LoadSequence();
            SoundManager.Instance.PlayBGM(BGMKey.bgm_main, true);
        }

        private void LoadSequence()
        {
            _loadingTaskMachine = new LoadingTaskMachine(this);
            _loadingTaskMachine.ClearState();
            _loadingTaskMachine.AddState(new LoadingTaskHello());
            
            var holder = GameStateManager.Instance.GetState<DataLoadCompleteHolder>(false);
            if (holder == null)
            {
                _loadingTaskMachine.AddState(new LoadingTaskLoadTableData());
                _loadingTaskMachine.AddState(new LoadingTaskLoadUserData());
            }
            
            _loadingTaskMachine.AddState(new LoadingTaskShowMenu(SceneUI.CreateMenu));
            _loadingTaskMachine.DoNextState();
        }

        public void OnLoadingProgressChanged(LoadingTaskBase task)
        {
            SceneUI.OnLoadingProgressChanged(task.Progress, task.ProgressMessage);
        }
    }
}