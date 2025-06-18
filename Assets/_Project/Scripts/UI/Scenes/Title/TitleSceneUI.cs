using System.Collections;
using System.Collections.Generic;
using alpoLib.Res;
using alpoLib.UI;
using alpoLib.UI.Scene;
using MergeBoard.Data.Table;
using MergeBoard.Scenes;
using MergeBoard.VFX;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Scenes
{
    public class TitleSceneUI : SceneUIBase
    {
        [SerializeField] RectTransform logoTransform;
        [SerializeField] private UIProgressComp loadingProgressComp;
        [SerializeField] private LayoutGroup menuObject;

        public override void OnClose()
        {
            base.OnClose();
            StopAllCoroutines();
        }

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
            StartCoroutine(PlayEffect());
        }

        private void OnClickBoardButton(BoardDefineBase boardDefine)
        {
            SceneManager.Instance.OpenSceneAsync<BoardScene>(param: new BoardSceneParam
            {
                BoardId = boardDefine.Id
            });
        }

        private IEnumerator PlayEffect()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                var p = VfxResourceHolder.Instance.Get<ParticleVfxUIObject>("fanfare-vfx");
                if (p)
                {
                    var parent = UIRoot.Instance.ForwardCanvas as RectTransform;
                    p.transform.SetParentEx(parent);
                    AwaitableHelper.Run(() => p.Play(logoTransform));
                    yield return new WaitForSeconds(Random.Range(1.5f, 3f));
                }
            }
        }
    }
}