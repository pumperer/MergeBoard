using alpoLib.UI.Scene;
using MergeBoard.Sound;
using UnityEngine;

namespace MergeBoard.Scenes
{
    [SceneDefine("Addr/Scenes/SplashScene.unity", SceneResourceType.Addressable)]
    public class SplashScene : SceneBase
    {
        [SerializeField] private GameObject logoObject;

        public override void OnOpen()
        {
            AwaitableHelper.Run(SequenceAsync);
        }

        private async Awaitable SequenceAsync()
        {
            await SoundManager.Instance.PreloadAsync();
            SoundManager.Instance.PlaySFX(SFXKey.sfx_splash);
            logoObject.SetActive(true);
            await Awaitable.WaitForSecondsAsync(2f);

            AwaitableHelper.Run(() => SceneManager.Instance.OpenSceneAsync<TitleScene>());
        }
    }
}