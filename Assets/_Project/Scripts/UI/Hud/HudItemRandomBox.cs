using System;
using alpoLib.UI;
using alpoLib.UI.Hud;
using MergeBoard.Scenes.Board.Feature.UI;
using MergeBoard.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Hud
{
    public class HudItemRandomBox : HudItemBase, IRandomBoxPop
    {
        private Action<Vector3> _onRandomBoxPopEvent;

        protected override void OnClickHudItemEvent()
        {
            var sp = CachedRectTransform.TransformPoint(CachedRectTransform.localPosition);
            var canvasRect = UIRoot.Instance.Canvas.GetComponent<RectTransform>();
            var scale = canvasRect.rect.size;
            var centerBasedCanvasPosition = Vector3.Scale(sp, new Vector3(1 / scale.x, 1 / scale.y, 1));
            var wp = Camera.main.ViewportToWorldPoint(centerBasedCanvasPosition);
            _onRandomBoxPopEvent?.Invoke(wp);
        }

        public void SetRandomBoxPopEvent(Action<Vector3> onRandomBoxPop)
        {
            _onRandomBoxPopEvent = onRandomBoxPop;
        }
    }
}