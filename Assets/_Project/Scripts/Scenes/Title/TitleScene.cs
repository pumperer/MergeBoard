using alpoLib.UI.Scene;
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
        }

        private void LoadSequence()
        {
            _loadingTaskMachine = new LoadingTaskMachine(this);
            _loadingTaskMachine.ClearState();
            _loadingTaskMachine.AddState(new LoadingTaskHello());
            _loadingTaskMachine.AddState(new LoadingTaskLoadTableData());
            _loadingTaskMachine.AddState(new LoadingTaskLoadUserData());
            _loadingTaskMachine.AddState(new LoadingTaskShowMenu(SceneUI.CreateMenu));
            _loadingTaskMachine.DoNextState();
        }

        public void OnLoadingProgressChanged(LoadingTaskBase task)
        {
            SceneUI.OnLoadingProgressChanged(task.Progress, task.ProgressMessage);
        }
    }
}