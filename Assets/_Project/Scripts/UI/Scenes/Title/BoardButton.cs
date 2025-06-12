using System;
using alpoLib.Res;
using alpoLib.Util;
using MergeBoard.Data.Table;
using TMPro;
using UnityEngine;

namespace MergeBoard.UI
{
    [PrefabPath("Addr/UI/Prefabs/Title/BoardButton.prefab")]
    public class BoardButton : CachedUIBehaviour
    {
        [SerializeField] private TMP_Text boardNameText;
        
        private BoardDefineBase _boardDefine;
        private Action<BoardDefineBase> _clickAction;
        
        public void Initialize(BoardDefineBase boardDefine, Action<BoardDefineBase> clickAction)
        {
            _boardDefine = boardDefine;
            _clickAction = clickAction;

            if (boardNameText)
            {
            }
        }
        
        public void OnClickButton()
        {
            _clickAction?.Invoke(_boardDefine);
        }
    }
}