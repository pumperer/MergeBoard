using System.Collections.Generic;
using alpoLib.Res;
using alpoLib.UI;
using alpoLib.UI.Scene;
using MergeBoard.Data.Table;
using MergeBoard.Scenes;
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

        public void CreateMenu(List<BoardDefineBase> boardDefines)
        {
            loadingProgressComp.gameObject.SetActive(false);
            foreach (var boardDefine in boardDefines)
            {
                var b = GenericPrefab.InstantiatePrefab<BoardButton>();
                b.transform.SetParentEx(menuObject.transform);
                b.Initialize(boardDefine, OnClickBoardButton);
            }
            
            menuObject.gameObject.SetActive(true);
        }

        private void OnClickBoardButton(BoardDefineBase boardDefine)
        {
            SceneManager.Instance.OpenSceneAsync<BoardScene>(param: new BoardSceneParam
            {
                BoardId = boardDefine.Id
            });
        }
    }
}