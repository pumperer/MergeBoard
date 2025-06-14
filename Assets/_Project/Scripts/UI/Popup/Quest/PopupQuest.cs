using System;
using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using alpoLib.Res;
using alpoLib.UI;
using alpoLib.Util;
using MergeBoard.Data.Composition;
using MergeBoard.Data.User;
using UnityEngine;

namespace MergeBoard.UI.Popup
{
    [PrefabPath("Addr/UI/Prefabs/Popup/Quest/PopupQuest.prefab")]
    [LoadingBlockDefinition(typeof(PopupQuestLoadingBlock))]
    public class PopupQuest : DataPopup<PopupQuestParam, PopupQuestInitData>
    {
        [SerializeField] private TableView tableView;
        
        [SerializeField]
        private GameObject emptyObject = null;

        private List<QuestData> _questDataList = new();
        private DefaultObjectPool<QuestSlot> _slotPool;

        protected override void OnOpen()
        {
            base.OnOpen();
            tableView.OnGetTotalCellItemCount += TableViewOnGetTotalCellItemCount;
            tableView.OnGetCellItemSize += TableViewOnGetCellItemSize;
            tableView.OnGetCellItem += TableViewOnGetCellItem;
            tableView.OnReleaseCellItem += TableViewOnReleaseCellItem;

            var prefab = GenericPrefab.LoadPrefab<QuestSlot>();
            _slotPool = new DefaultObjectPool<QuestSlot>(prefab.gameObject);

            RefreshQuestList();
        }

        protected override void OnClose()
        {
            base.OnClose();
            tableView.OnGetTotalCellItemCount -= TableViewOnGetTotalCellItemCount;
            tableView.OnGetCellItemSize -= TableViewOnGetCellItemSize;
            tableView.OnGetCellItem -= TableViewOnGetCellItem;
            tableView.OnReleaseCellItem -= TableViewOnReleaseCellItem;
        }

        private void TableViewOnReleaseCellItem(int arg1, Transform arg2)
        {
            _slotPool.Release(arg2.GetComponent<QuestSlot>());
        }

        private GameObject TableViewOnGetCellItem(int arg)
        {
            var slot = _slotPool.Get();
            slot.SetData(_questDataList[arg], InitData.UserItemMapper, OnClickQuestSubmit);
            slot.transform.SetParent(tableView.content);
            return slot.gameObject;
        }

        private Vector2 TableViewOnGetCellItemSize(int arg)
        {
            return new Vector2(960, 300);
        }

        private int TableViewOnGetTotalCellItemCount()
        {
            return _questDataList.Count;
        }

        private void RefreshQuestList()
        {
            _questDataList = InitData.UserQuestMapper.GetAllQuestDatas(InitData.CurrentBoardId).ToList();
            _questDataList.Sort((a, b) => b.CheckIsDone().CompareTo(a.CheckIsDone()));
            tableView.RefreshAll(true);
            emptyObject.SetActive(_questDataList.Count == 0);
        }
        
        public void OnClickAddQuest()
        {
            var newQuestData = InitData.UserQuestMapper.AddRandomQuest(InitData.CurrentBoardId);
            _questDataList.Insert(0, newQuestData);
            tableView.RefreshAll();
            emptyObject.SetActive(false);
        }

        public void OnClickQuestSubmit(QuestData data)
        {
            // submit popup
            var p = PopupBase.CreatePopup<PopupQuestSubmit>();
            p.Initialize(new PopupQuestSubmitParam
            {
                QuestData = data,
                QuestSubmitCallback = OnQuestSubmit,
            });
            p.Open();
        }

        private void OnQuestSubmit(QuestData data)
        {
            _questDataList.Remove(data);
            tableView.RefreshAll();
        }
    }

    public class PopupQuestParam : PopupParam
    {
        public Action QuestSubmitCallback;
        public int CurrentBoardId;
    }
    
    public class PopupQuestInitData : PopupInitData
    {
        public Action QuestSubmitCallback;
        public int CurrentBoardId;
        public IUserQuestMapper UserQuestMapper;
        public IUserItemMapper UserItemMapper;
    }

    public class PopupQuestLoadingBlock : PopupLoadingBlock<PopupQuestParam, PopupQuestInitData>
    {
        public override PopupQuestInitData MakeInitData(PopupQuestParam param)
        {
            return new PopupQuestInitData
            {
                QuestSubmitCallback = param.QuestSubmitCallback,
                CurrentBoardId = param.CurrentBoardId,
                UserQuestMapper = UserDataManager.GetLoader<IUserQuestMapper>(),
                UserItemMapper = UserDataManager.GetLoader<IUserItemMapper>()
            };
        }
    }
}