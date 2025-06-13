using System;
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
        
        private Item _selectedItem;
        private Action<Item> _onSellItem;
        
        public void OnClickSellButton()
        {
            _onSellItem?.Invoke(_selectedItem);
        }

        public void OnSelectItem(Item item)
        {
            _selectedItem = item;
            
            if (!_selectedItem || _selectedItem.ItemData == null)
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
                    itemNameText.text = _selectedItem.ItemData.GetName(true);
                if (itemDescText)
                    itemDescText.text = _selectedItem.ItemData.GetDescription();
                if (sellPriceText)
                    sellPriceText.text = $"{_selectedItem.ItemData.SellValue}";
                if (sellButton)
                    sellButton.gameObject.SetActive(_selectedItem.ItemData.SellValue > 0);
            }
        }

        public void SetSellItemEvent(Action<Item> onSellItem)
        {
            _onSellItem = onSellItem;
        }
    }
}