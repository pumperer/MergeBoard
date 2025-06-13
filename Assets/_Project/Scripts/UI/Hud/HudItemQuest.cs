using alpoLib.UI;
using alpoLib.UI.Hud;
using MergeBoard.UI.Popup;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public class HudItemQuest : HudItemBase
    {
        protected override void OnClickHudItemEvent()
        {
            var p = PopupBase.CreatePopup<PopupQuest>();
            p.Initialize(new PopupQuestParam
            {
                QuestSubmitCallback = MergeBoard.Scenes.Board.MergeBoard.Instance.RefreshQuestStatus,
                CurrentBoardId = MergeBoard.Scenes.Board.MergeBoard.Instance.CurrentBoardId,
            });
            p.Open();
        }
    }
}