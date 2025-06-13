using alpoLib.UI.Hud;
using TMPro;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public class HudItemSelectedItemDesc : HudItemBase
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemDescText;
        
        [SerializeField] private GameObject sellButton;
        [SerializeField] private TMP_Text sellPriceText;
        
        public void OnClickSellButton()
        {
            Debug.Log("Sell button clicked");
        }
    }
}