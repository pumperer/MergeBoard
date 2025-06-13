using System;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data.Table;
using MergeBoard.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace MergeBoard.UI
{
    [PrefabPath("Addr/UI/Prefabs/Title/BoardButton.prefab")]
    public class BoardButton : CachedUIBehaviour
    {
        [SerializeField] private LocalizeStringEvent boardNameLocalize;
        
        private BoardDefineBase _boardDefine;
        private Action<BoardDefineBase> _clickAction;
        
        public void Initialize(BoardDefineBase boardDefine, Action<BoardDefineBase> clickAction)
        {
            _boardDefine = boardDefine;
            _clickAction = clickAction;

            LocalizationManager.Instance.ChangeLocalizeStringEvent(boardNameLocalize, boardDefine.NameStringKey);
        }
        
        public void OnClickButton()
        {
            _clickAction?.Invoke(_boardDefine);
        }
    }
}