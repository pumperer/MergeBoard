using alpoLib.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Scenes
{
    public class TitleSceneUI : SceneUIBase
    {
        [SerializeField] private UIProgressComp loadingProgressComp;
        [SerializeField] private LayoutGroup menuObject;
        
        public void OnLoadingProgressChanged(int progress, string message)
        {
            if (!loadingProgressComp)
                return;
            loadingProgressComp.gameObject.SetActive(true);
            loadingProgressComp.SetProgress(progress);
            loadingProgressComp.SetText(message);
        }

        public void CreateMenu()
        {
            loadingProgressComp.gameObject.SetActive(false);
            menuObject.gameObject.SetActive(true);
        }
    }
}