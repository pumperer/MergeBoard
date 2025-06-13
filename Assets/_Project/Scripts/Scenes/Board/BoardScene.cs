using alpoLib.Data;
using alpoLib.UI.Scene;
using MergeBoard.Data.Table;
using MergeBoard.UI.Scenes;

namespace MergeBoard.Scenes
{
    [SceneDefine("Addr/Scenes/BoardScene.unity", SceneResourceType.Addressable)]
    [LoadingBlockDefinition(typeof(BoardSceneLoadingBlock))]
    public class BoardScene : SceneBaseWithUI<BoardSceneUI>
    {
        public BoardSceneInitData InitData => sceneInitData as BoardSceneInitData;
    }

    public class BoardSceneParam : SceneParam
    {
        public int BoardId;
    }
    
    public class BoardSceneInitData : SceneInitData
    {
        public BoardDefineBase BoardDefine;
    }

    public class BoardSceneLoadingBlock : SceneLoadingBlock<BoardSceneParam, BoardSceneInitData>
    {
        public override BoardSceneInitData MakeInitData(BoardSceneParam param)
        {
            return new BoardSceneInitData
            {
                BoardDefine = TableDataManager.GetLoader<IBoardDefineTableMapper>().GetBoardDefineBase(param.BoardId)
            };
        }
    }
}