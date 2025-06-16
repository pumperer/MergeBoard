using MergeBoard.Data.Composition;
using MergeBoard.Scenes.Board.Feature.UI;

namespace MergeBoard.Scenes.Board.Feature
{
    public interface IInGameFeature
    {
        void AttachFeatureUI(IInGameFeatureUI featureUI);

        void OnOpen();
        void OnClose();
        
        void OnUpdate(float deltaTime, float timeScale);
        void OnSelect(Item item);
        void OnMerge(Item item1, Item item2, Item newItem);
        void OnPop(Item fromItem, Item newItem);
        void OnPopFromRandomBox(Item newItem);
        void OnMove(Item moveItem, BoardSlot fromSlot, BoardSlot toSlot);
        void OnSell(Item item);
        void OnQuestComplete(QuestData questData);
        void OnRandomBoxPop(Item item);
    }
    
    public abstract class InGameFeatureBase : IInGameFeature
    {
        protected MergeBoard Board { get; private set; }
        protected IInGameFeatureUI FeatureUI { get; private set; }

        protected InGameFeatureBase(MergeBoard board)
        {
            Board = board;
        }
        
        public void AttachFeatureUI(IInGameFeatureUI featureUI)
        {
            FeatureUI = featureUI;
        }

        public virtual void OnOpen()
        {
        }

        public virtual void OnClose()
        {
        }

        public virtual void OnUpdate(float deltaTime, float timeScale)
        {
        }

        public virtual void OnSelect(Item item)
        {
        }

        public virtual void OnMerge(Item item1, Item item2, Item newItem)
        {
        }

        public virtual void OnPop(Item fromItem, Item newItem)
        {
        }

        public void OnPopFromRandomBox(Item newItem)
        {
        }

        public virtual void OnMove(Item moveItem, BoardSlot fromSlot, BoardSlot toSlot)
        {
        }

        public virtual void OnSell(Item item)
        {
        }

        public virtual void OnQuestComplete(QuestData questData)
        {
        }

        public virtual void OnRandomBoxPop(Item item)
        {
        }
    }
}