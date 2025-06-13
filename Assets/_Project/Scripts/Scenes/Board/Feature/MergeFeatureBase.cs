using MergeBoard.Data.Composition;
using MergeBoard.Scenes.Board.Feature.UI;
using UnityEngine;

namespace MergeBoard.Scenes.Board.Feature
{
    public interface IMergeFeature
    {
        void AttachFeatureUI(IMergeFeatureUI featureUI);

        void OnOpen();
        void OnClose();
        
        void OnUpdate(float deltaTime, float timeScale);
        void OnSelect(Item item);
        void OnMerge(Item item1, Item item2);
        void OnPop(Item fromItem);
        void OnMove(Item moveItem, BoardSlot fromSlot, BoardSlot toSlot);
        void OnSell(Item item);
        void OnQuestComplete(QuestData questData);
        void OnRandomBoxPop(Item item);
    }
    
    public abstract class MergeFeatureBase : IMergeFeature
    {
        protected MergeBoard Board { get; private set; }
        protected IMergeFeatureUI FeatureUI { get; private set; }

        protected MergeFeatureBase(MergeBoard board)
        {
            Board = board;
        }
        
        public void AttachFeatureUI(IMergeFeatureUI featureUI)
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

        public virtual void OnMerge(Item item1, Item item2)
        {
        }

        public virtual void OnPop(Item fromItem)
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