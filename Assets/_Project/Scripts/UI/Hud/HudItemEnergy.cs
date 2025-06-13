using alpoLib.UI.Hud;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Hud
{
    public class HudItemEnergy : HudItemBase
    {
        [SerializeField] private TMP_Text energyCountText;
        [SerializeField] private TMP_Text chargeRemainTimeText;
        [SerializeField] private Slider energyCountSlider;
    }
}