using System;
using alpoLib.Data;
using alpoLib.Res;
using alpoLib.UI.Scene;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Sound;
using MergeBoard.UI.Scenes;

namespace MergeBoard.Scenes
{
    [SceneDefine("Addr/Scenes/BoardScene.unity", SceneResourceType.Addressable)]
    [LoadingBlockDefinition(typeof(BoardSceneLoadingBlock))]
    public class BoardScene : SceneBaseWithUI<BoardSceneUI>
    {
        public BoardSceneInitData InitData => sceneInitData as BoardSceneInitData;

        private Board.MergeBoard _mergeBoard;
        
        public override void OnOpen()
        {
            base.OnOpen();

            try
            {
                _mergeBoard = GenericPrefab.InstantiatePrefab<Board.MergeBoard>();
                _mergeBoard.Initialize(new Board.MergeBoard.Context
                {
                    BoardDefine = InitData.BoardDefine,
                    ItemTableMapper = InitData.ItemTableMapper,
                    UserInfoMapper = InitData.UserInfoMapper,
                    UserBoardMapper = InitData.UserBoardMapper,
                    UserItemMapper = InitData.UserItemMapper,
                    UserQuestMapper = InitData.UserQuestMapper
                });
                InitData.UserBoardMapper.SetBoardSerializer(_mergeBoard);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
            
            SoundManager.Instance.PlayBGM(InitData.BoardDefine.BGMKey, true);
        }

        public override void OnClose()
        {
            base.OnClose();
            _mergeBoard.OnClose();
            InitData.UserBoardMapper.SetBoardSerializer(null);
            SoundManager.Instance.StopBGM();
        }
    }

    public class BoardSceneParam : SceneParam
    {
        public int BoardId;
    }
    
    public class BoardSceneInitData : SceneInitData
    {
        public BoardDefineBase BoardDefine;
        public IItemTableLoader ItemTableMapper;
        public IUserInfoMapper UserInfoMapper;
        public IUserBoardMapper UserBoardMapper;
        public IUserItemMapper UserItemMapper;
        public IUserQuestMapper UserQuestMapper;
    }

    public class BoardSceneLoadingBlock : SceneLoadingBlock<BoardSceneParam, BoardSceneInitData>
    {
        public override BoardSceneInitData MakeInitData(BoardSceneParam param)
        {
            return new BoardSceneInitData
            {
                BoardDefine = TableDataManager.GetLoader<IBoardDefineTableMapper>().GetBoardDefineBase(param.BoardId),
                ItemTableMapper = TableDataManager.GetLoader<IItemTableLoader>(),
                UserInfoMapper = UserDataManager.GetLoader<IUserInfoMapper>(),
                UserBoardMapper = UserDataManager.GetLoader<IUserBoardMapper>(),
                UserItemMapper = UserDataManager.GetLoader<IUserItemMapper>(),
                UserQuestMapper = UserDataManager.GetLoader<IUserQuestMapper>(),
            };
        }
    }
}