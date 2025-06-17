using System;
using alpoLib.Data;
using alpoLib.Res;
using alpoLib.UI.Scene;
using MergeBoard.Data.Table;
using MergeBoard.Data.User;
using MergeBoard.Sound;
using MergeBoard.UI.Scenes;
using UnityEngine;

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

            AwaitableHelper.Run(InitializeMergeBoardAsync);
            
            SoundManager.Instance.PlayBGM(InitData.BoardDefine.BGMKey, true);
        }
        
        private async Awaitable InitializeMergeBoardAsync()
        {
            try
            {
                _mergeBoard = GenericPrefab.InstantiatePrefab<Board.MergeBoard>();
                await _mergeBoard.InitializeAsync(new Board.MergeBoard.Context
                {
                    BoardDefine = InitData.BoardDefine,
                    ItemTableMapper = InitData.ItemTableMapper,
                    PopProbabilityTableMapper = InitData.PopProbabilityTableMapper,
                    UserInfoMapper = InitData.UserInfoMapper,
                    UserBoardMapper = InitData.UserBoardMapper,
                    UserItemMapper = InitData.UserItemMapper,
                    UserQuestMapper = InitData.UserQuestMapper
                });
                InitData.UserBoardMapper.SetBoardSerializer(_mergeBoard);
                SceneUI.InitFeatures();
                _mergeBoard.OnOpen();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        public override void OnClose()
        {
            base.OnClose();
            _mergeBoard.OnClose();
            InitData.UserBoardMapper.SetBoardSerializer(null);
            SoundManager.Instance.StopBGM();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                InitData.UserInfoMapper.AddGold(100);
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                InitData.UserInfoMapper.AddEnergy(10);
                return;
            }
        }
    }

    public class BoardSceneParam : SceneParam
    {
        public int BoardId;
    }
    
    public class BoardSceneInitData : SceneInitData
    {
        public BoardDefineBase BoardDefine;
        public IItemTableMapper ItemTableMapper;
        public IPopProbabilityTableMapper PopProbabilityTableMapper;
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
                ItemTableMapper = TableDataManager.GetLoader<IItemTableMapper>(),
                PopProbabilityTableMapper = TableDataManager.GetLoader<IPopProbabilityTableMapper>(),
                UserInfoMapper = UserDataManager.GetLoader<IUserInfoMapper>(),
                UserBoardMapper = UserDataManager.GetLoader<IUserBoardMapper>(),
                UserItemMapper = UserDataManager.GetLoader<IUserItemMapper>(),
                UserQuestMapper = UserDataManager.GetLoader<IUserQuestMapper>(),
            };
        }
    }
}