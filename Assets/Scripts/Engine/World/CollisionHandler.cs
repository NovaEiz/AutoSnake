using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class CollisionHandler : MonoBehaviour, ICollisionHandler<IWorldItem> {

#region Private fields

	[Inject]
	private ISnakeMovingSystem _movingSystem;
	[Inject]
	private IWorld _world;

#endregion

#region Private methods
	
	public CollisionData GetCollisionInPosition(Vector3 position) {
		var itemCollision = _world.GetCollisionItemInPosition(position);
		return itemCollision;
	}
	
	private void OnMovedItem(IWorldItem item) {
		var collisionItems = _world.GetCollisionItemsInPosition(item.Position);

		var itemId = item.transform.GetInstanceID();

		foreach (var collisionItem in collisionItems) {
			if (collisionItem.transform.GetInstanceID() != itemId) {
				OnCollision?.Invoke(item, collisionItem);
			}
		}
	}
	
#endregion

#region Public methods

	public void AddItem(IWorldItem item) {
		item.OnMoved += OnMovedItem;
	}
	
#endregion

#region Events

	public event Action<IWorldItem, IWorldItem> OnCollision;

#endregion

}

}