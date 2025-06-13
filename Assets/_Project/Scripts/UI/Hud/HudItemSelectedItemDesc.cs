using alpoLib.UI.Hud;
using MergeBoard.Data.Composition;
using MergeBoard.Scenes.Board;
using MergeBoard.Scenes.Board.Feature.UI;
using MergeBoard.Utility;
using TMPro;
using UnityEngine;

namespace MergeBoard.UI.Hud
{
    public class HudItemSelectedItemDesc : HudItemBase, ISelectItemInfo
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemDescText;
        
        [SerializeField] private GameObject sellButton;
        [SerializeField] private TMP_Text sellPriceText;
        
        public Item SelectedItem = null;
        
        public void OnClickSellButton()
        {
            Debug.Log("Sell button clicked");
        }

        public void OnSelectItem(Item item)
        {
            SelectedItem = item;
            
            if (!SelectedItem || SelectedItem.ItemData == null)
            {
                if (itemNameText)
                    itemNameText.text = string.Empty;
                if (itemDescText)
                    itemDescText.text = LocalizationManager.Instance.GetString("UI_Item_NotSelect");
                if (sellButton)
                    sellButton.gameObject.SetActive(false);
            }
            else
            {
                if (itemNameText)
                    itemNameText.text = SelectedItem.ItemData.GetName(true);
                if (itemDescText)
                    itemDescText.text = SelectedItem.ItemData.GetDescription();
                if (sellPriceText)
                    sellPriceText.text = $"{SelectedItem.ItemData.SellValue}";
                if (sellButton)
                    sellButton.gameObject.SetActive(SelectedItem.ItemData.SellValue > 0);
            }
        }
    }
}