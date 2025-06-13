using alpoLib.Res;
using UnityEngine;

namespace MergeBoard.Scenes.Board
{
    [PrefabPath("Addr/Board/BoardSlot.prefab")]
    public class BoardSlot : MonoBehaviour, ISelectable, IMovableReceiver
    {
        [SerializeField]
        private SpriteRenderer bgSprite = null;
        [SerializeField]
        private SpriteRenderer selectedSprite = null;
        
        [SerializeField]
        private Color defaultColor = Color.white;
        [SerializeField]
        private Color questCompleteColor = Color.white;
        
        [SerializeField]
        private Item currentItem = null;
        
        public MergeBoard Board;
        public int X = -1;
        public int Y = -1;

        private readonly float _width = 1f;
        private readonly float _height = 1f;
        private bool _isSelected;

        public Item CurrentItem => currentItem;
        
        public void Init(MergeBoard mb, int x, int y, Item item)
        {
            name = $"SLOT {x} - {y}";
            Board = mb;
            X = x;
            Y = y;
            RefreshPosition();

            SetItem(item);
            RepositionItem(false);
            if (item != null)
                item.SetSlot(this);
            OnSelect(false);
        }
        
        public void SetItem(Item item)
        {
            currentItem = item;
            if (item != null)
            {
                item.transform.SetParent(transform);
                Board.RefreshQuestStatus();
            }
            else
                SetBGColor(false);
        }
        
        public void RepositionItem(bool animated)
        {
            if (currentItem != null)
            {
                if (animated)
                    currentItem.OnCancelMove();
                else
                    currentItem.transform.localPosition = Vector3.zero;
            }
        }
        
        public bool IsEmpty()
        {
            return currentItem == null;
        }
        
        public void RefreshPosition()
        {
            transform.position = new Vector3(X * _width, Y * _height, 0f);
        }
        
        public void SetBGColor(bool questAcquired)
        {
            if (bgSprite != null)
                bgSprite.color = questAcquired ? questCompleteColor : defaultColor;
        }
        
        #region ISelectable
        
        public bool IsSelected => _isSelected;
        
        public bool OnSelect(bool select)
        {
            if (select)
            {
                var haveItem = currentItem != null;
                _isSelected = haveItem;
                selectedSprite.gameObject.SetActive(haveItem);
                if (haveItem)
                    currentItem.PlaySelectAnimation();
                return haveItem;
            }
            else
            {
                _isSelected = false;
                selectedSprite.gameObject.SetActive(false);
            }
            return false;
        }

        public bool OnExecute()
        {
            if (IsEmpty())
                return false;

            if (currentItem != null)
                currentItem.PlaySelectAnimation();

            Board.Execute(this, currentItem);

            return true;
        }

        public IMovable GetMovable()
        {
            return currentItem;
        }
        
        #endregion

        #region IMovableReceiver
        
        public bool OnMovableReceive(IMovable movable)
        {
            if (movable == null)
                return false;

            if (movable is Item item)
            {
                if (IsEmpty())
                {
                    item.SetSlot(this);
                    SetItem(item);
                    RepositionItem(true);
                    Board.Select(this);
                    return true;
                }
                else
                {
                    // merge!
                    var mergedItem = Board.Merge(currentItem, item);
                    if (mergedItem != null)
                    {
                        mergedItem.SetSlot(this);
                        SetItem(mergedItem);
                        RepositionItem(false);
                        Board.Select(this);
                        return true;
                    }
                    else
                    {
                        var emptySlot = Board.FindNearestEmptySlot(this);
                        if (emptySlot == null)
                            return false;

                        var tempItem = currentItem;

                        currentItem.SetSlot(emptySlot);
                        emptySlot.SetItem(tempItem);

                        item.SetSlot(this);
                        SetItem(item);

                        tempItem.OnCancelMove();
                        item.OnCancelMove();

                        Board.Select(this);
                        return true;
                    }
                }
            }

            return false;
        }
        
        #endregion
    }
}