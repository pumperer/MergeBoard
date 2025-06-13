using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    public interface IBoardInterfaceBase
    {
    }

    public interface ISelectable : IBoardInterfaceBase
    {
        bool IsSelected { get; }

        bool OnSelect(bool select);

        bool OnExecute();

        IMovable GetMovable();
    }

    public interface IMovable : IBoardInterfaceBase
    {
        bool OnMove(Vector3 worldPos);
        void OnCancelMove();
    }

    public interface IMovableReceiver : IBoardInterfaceBase
    {
        bool OnMovableReceive(IMovable movable);
    }
}