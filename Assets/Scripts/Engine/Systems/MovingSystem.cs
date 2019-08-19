using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public abstract class MovingSystem<Obj, Item> : MonoBehaviour, IMovingSystem<Obj, Item> where Item : IWorldItem where  Obj : Item {

#region Private fields

	[Header("SnakeMovingSystem - Settable fields")]
	[SerializeField]
	private float _moveInterval;
	
	protected List<Obj> _items = new List<Obj>();
	
	[Inject]
	protected ICollisionHandler<Item> _collisionHandler;
	
	[Inject]
	protected IWorld _world;
	
	[Inject]
	private SnakeFactory _snakeFactory;

#endregion

#region IMovingSystem
	
	public void AddItem(Obj item) {
		_items.Add(item);
		item.OnDestroyed += (itemDestroyed) => {
			if (_items.Contains(item)) {
				_items.Remove((Obj)itemDestroyed);
			}
		};
	}

	public event Action<Obj> OnMovedItem;

#endregion

#region Private methods

	public abstract bool CanWalkOnBy(Item itemCollision, Obj item, Vector3 position);
	
	private bool CanStandInPosition(Obj item, Vector3 position) {
		var itemCollision = _collisionHandler.GetCollisionInPosition(position);
		if (itemCollision != null) {
			return CanWalkOnBy((Item)itemCollision.Item, item, position);
		}
	
		return true;
	}
	
	private Vector3 GetNextPositionByDirection(Obj item, Vector3 direction) {
		var nextPosition = item.Face.localPosition + direction;
		return nextPosition.ConvertToInt();
	}
	
	private bool CanMoveByDirection(Obj item, Vector3 offset) {
		var forward = offset.ConvertToInt();
		return CanStandInPosition(item, GetNextPositionByDirection(item, forward));
	}
	
	protected bool CanMoveForward(Obj item) {
		return CanMoveByDirection(item, item.Face.forward);
	}
	
	protected bool CanMoveLeft(Obj item) {
		return CanMoveByDirection(item, -item.Face.right);
	}
	
	protected bool CanMoveRight(Obj item) {
		return CanMoveByDirection(item, item.Face.right);
	}
	
	private IEnumerator UpdateIe() {
		while (true) {
			for (var i = 0; i < _items.Count; i++) {
				var item = _items[i];
				if (TryMove(item)) {
					Move(item);
				} else {
					if (!CanMoveForward(item) && !CanMoveLeft(item) && !CanMoveRight(item)) {
						item.Destroy();
					}
				}
			}
	
			yield return new WaitForSeconds(_moveInterval);
		}
	}

#endregion

#region Public methods

	public abstract void Move(Obj    snake);
	public abstract bool TryMove(Obj snake);
	
	public void MovedItem(Obj item) {
		OnMovedItem?.Invoke(item);
	}

#endregion

#region Unity Events

	private void Start() {
		StartCoroutine(UpdateIe());
	}

#endregion

}

}