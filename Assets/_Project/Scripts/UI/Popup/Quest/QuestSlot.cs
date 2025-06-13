using System;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MergeBoard.UI.Popup
{
    [PrefabPath("Addr/UI/Prefabs/Popup/Quest/QuestSlot.prefab")]
    public class QuestSlot : CachedUIBehaviour
    {
        [SerializeField] protected TMP_Text questNameText;
        [SerializeField] protected ItemSlot[] questConditionItemSlots;
        [SerializeField] protected Button submitButton;

        private QuestData _questData;
        private Action _submitCallback;

        public void SetData(QuestData questData, Action submitCallback)
        {
            _questData = questData;
            _submitCallback = submitCallback;
            UpdateUI();
            if (submitButton)
                submitButton.gameObject.SetActive(_questData.CheckIsDone());
        }

        private void UpdateUI()
        {
            if (questNameText)
                questNameText.text = _questData.GetQuestName();
            RefreshItemList();
        }

        private void RefreshItemList()
        {
            for (var index = 0; index < questConditionItemSlots.Length; index++)
            {
                var slot = questConditionItemSlots[index];
                if (index < _questData.Conditions.Count)
                {
                    var condition = _questData.Conditions[index];
                    slot.UpdateUI(new ItemSlotData
                    {
                        ItemBase = _questData.ConditionItemBaseList[index],
                        RequireCount = condition.Condition.ConditionValue,
                    });
                    slot.gameObject.SetActive(true);
                }
                else
                {
                    slot.gameObject.SetActive(false);
                }
            }
        }

        public void OnClickSubmit()
        {
            _submitCallback?.Invoke();
        }
    }
}