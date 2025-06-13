using System.Collections.Generic;
using alpoLib.Data;
using MergeBoard.Data.Table;
using UnityEngine;

namespace MergeBoard.Data.User
{
    public record UserBoardSlot
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int UserItemId { get; set; }
    }
    
    public record UserBoard : UserDataBase
    {
        public int BoardId { get; set; }

        public List<UserBoardSlot> UserBoardSlotList { get; set; }
    }
    
    public interface IUserBoardMapper : IUserDataMapperBase
    {
        UserBoard GetUserBoardData(int boardId);
        void SetBoardSerializer(IUserBoardSerializer boardSerializer);
    }

    public interface IUserBoardSerializer
    {
        void SerializeBoard();
    }
    
    public class UserBoardData : UserDataManagerBase, IUserBoardMapper
    {
        private IBoardInitialDataTableMapper _boardInitialDataTableMapper;
        private IUserItemMapper _userItemMapper;
        
        [SerializeField]
        private List<UserBoard> _userBoardList = new();
        
        private IUserBoardSerializer _boardSerializer;

        public override void OnSerialize()
        {
            _boardSerializer?.SerializeBoard();
        }

        public override void OnCreateInstance()
        {
            _boardInitialDataTableMapper = TableDataManager.GetLoader<IBoardInitialDataTableMapper>();
            _userItemMapper = UserDataManager.GetLoader<IUserItemMapper>();
        }

        private UserBoard CreateNewUserBoard(int boardId)
        {
            var initSlotList = _boardInitialDataTableMapper.GetBoardInitialDataBaseList(boardId);
            var initUserSlotList = new List<UserBoardSlot>();

            foreach (var initSlot in initSlotList)
            {
                var itemData = _userItemMapper.AddItem(initSlot.ItemId);
                var userSlot = new UserBoardSlot
                {
                    X = initSlot.X,
                    Y = initSlot.Y,
                    UserItemId = itemData.UserDataId,
                };
                initUserSlotList.Add(userSlot);
            }

            var board = new UserBoard
            {
                BoardId = boardId,
                UserBoardSlotList = initUserSlotList
            };
            _userBoardList.Add(board);
            return board;
        }
        
        public UserBoard GetUserBoardData(int boardId)
        {
            var user = _userBoardList.Find(u => u.BoardId == boardId) ?? CreateNewUserBoard(boardId);
            return user;
        }

        public void SetBoardSerializer(IUserBoardSerializer boardSerializer)
        {
            _boardSerializer = boardSerializer;
        }
    }
}