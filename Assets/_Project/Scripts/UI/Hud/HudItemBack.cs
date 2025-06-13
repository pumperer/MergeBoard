using alpoLib.Data;
using alpoLib.UI.Hud;
using alpoLib.UI.Scene;
using MergeBoard.Scenes;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public class HudItemBack : HudItemBase
    {
        protected override void OnClickHudItemEvent()
        {
            var p = new DefaultUserDataSaveProcess();
            p.Save();
            SceneManager.Instance.OpenSceneAsync<TitleScene>();
        }
    }
}