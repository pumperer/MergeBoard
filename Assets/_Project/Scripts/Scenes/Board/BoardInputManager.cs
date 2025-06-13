using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MergeBoard.Scenes.Board
{
    public class BoardInputManager
    {
        private const float moveThresholdSize = 100f;
        private const float selectThresholdTime = 0.5f;

        private readonly MergeBoard board = null;

        private bool shouldReset = false;
        private bool keyDown = false;
        private Vector3 lastPressedPos = Vector3.zero;
        private float lastPressedTime = 0f;
        private ISelectable currentSelectable = null;
        private IMovable currentMovable = null;
        
        public BoardInputManager(MergeBoard board)
		{
			this.board = board;
		}

		public void OnUpdate()
		{
			if (shouldReset)
				Reset();

			MouseUpdate();
		}

		private void Reset()
		{
			shouldReset = false;
			keyDown = false;
			lastPressedPos = Vector3.zero;
			lastPressedTime = 0f;
			currentSelectable = null;
			currentMovable = null;
		}

		private void MouseUpdate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (IsOverUI())
				{
					shouldReset = true;
					return;
				}

				keyDown = true;
				var pos = Input.mousePosition;
				lastPressedTime = Time.unscaledTime;
				lastPressedPos = pos;

				currentSelectable = FindBoardInterface<ISelectable>(pos);
			}
			else if (Input.GetMouseButton(0))
			{
				if (IsOverUI())
				{
					shouldReset = true;
					return;
				}

				if (keyDown)
				{
					var pos = Input.mousePosition;

					if (currentMovable != null)
					{
						var worldPos = Camera.main.ScreenToWorldPoint(pos);
						currentMovable.OnMove(worldPos);
						return;
					}

					var sqrMag = (pos - lastPressedPos).sqrMagnitude;
					//Debug.Log($"sqrMag : {sqrMag}");
					if (sqrMag >= moveThresholdSize)
					{
						if (currentSelectable != null)
						{
							// movable!
							currentMovable = currentSelectable.GetMovable();
						}
					}
					else
					{
						// nothing?
					}
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (IsOverUI())
				{
					shouldReset = true;
					return;
				}

				var pos = Input.mousePosition;
				if (currentMovable != null)
				{
					var receiver = FindBoardInterface<IMovableReceiver>(pos);
					if (receiver != null && receiver != currentSelectable)
					{
						if (!receiver.OnMovableReceive(currentMovable))
							currentMovable.OnCancelMove();
					}
					else
						currentMovable.OnCancelMove();
				}
				else
				{
					var pressTime = Time.unscaledTime - lastPressedTime;
					if (pressTime <= selectThresholdTime)
					{
						var selectable = FindBoardInterface<ISelectable>(pos);
						if (selectable != null && currentSelectable == selectable)
						{
							if (selectable.IsSelected)
								selectable.OnExecute();
							else
								board.Select(selectable);
						}
					}
				}

				Reset();
			}
		}

		private static bool IsOverUI()
		{
			var eventData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, results);

			return results.Count > 0;
		}

		private T FindBoardInterface<T>(Vector3 screenPos) where T : IBoardInterfaceBase
		{
			var hit = Ray(screenPos);

			if (hit.collider == null)
				return default;

			var selectable = hit.transform.GetComponent<T>();
			if (selectable == null)
				return default;

			return selectable;
		}

		private RaycastHit2D Ray(Vector3 screenPos)
		{
			var layer = LayerMask.GetMask("Board");
			var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			var hit = Physics2D.Raycast(worldPos, Vector2.zero, float.PositiveInfinity, layer);
			return hit;
		}
    }
}