using System.Collections.Generic;
using System.Linq;
using alpoLib.Data;
using MergeBoard.Data.Composition;
using MergeBoard.Data.Table;
using UnityEngine;

namespace MergeBoard.Data.User
{
    public record UserQuest : UserDataBase
    {
        public int QuestId { get; set; }
    }
    
    public interface IUserQuestMapper : IUserDataMapperBase
    {
        QuestData AddRandomQuest(int currentBoard);
        IEnumerable<QuestData> GetAllQuestDatas(int currentBoard);
        void CompleteQuest(QuestData data);
    }
    
    public class UserQuestData : UserDataManagerBase, IUserQuestMapper
    {
        public delegate void OnQuestCompleteEvent(QuestData data);
        public static event OnQuestCompleteEvent OnComplete;

        private IQuestDefineTableMapper _questMapper = null;
        private IUserItemMapper _userItemMapper = null;

        [SerializeField]
        private Dictionary<int, UserQuest> _questDic = new();
        
        public override void OnCreateInstance()
        {
            _questMapper = TableDataManager.GetLoader<IQuestDefineTableMapper>();
            _userItemMapper = UserDataManager.GetLoader<IUserItemMapper>();
        }

        public QuestData AddRandomQuest(int currentBoard)
        {
            var newQuestDefine = _questMapper.GetRandomQuestDefine(currentBoard);

            var maxId = _questDic.Count == 0 ? 1 : (_questDic.Keys.Max() + 1);
            var newUserQuest = new UserQuest
            {
                Id = maxId,
                QuestId = newQuestDefine.Id,
            };
            _questDic.Add(maxId, newUserQuest);

            return new QuestData(newQuestDefine, newUserQuest);
        }

        public IEnumerable<QuestData> GetAllQuestDatas(int currentBoard)
        {
            var list = from userQuest in _questDic.Values
                let defineBase = _questMapper.GetQuestDefineBase(currentBoard, userQuest.QuestId)
                where defineBase.BoardId == currentBoard
                select new QuestData(defineBase, userQuest);
            return list;
        }

        public void CompleteQuest(QuestData data)
        {
            data.CompleteQuest();
            OnComplete?.Invoke(data);
            _questDic.Remove(data.QuestId);
        }
    }
}