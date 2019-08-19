using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class CollisionData {
	public IWorldItem Item;
	public WorldItemType ItemType;

	public CollisionData() {}
	public CollisionData(IWorldItem item, WorldItemType itemType) {
		Item     = item;
		ItemType = itemType;
	}
}

public class World : MonoBehaviour, IWorld {

#region Private fields

	[Header("WorldSnakes - Settable fields")]
	[SerializeField] private Vector2Int _size = new Vector2Int(1, 1);

	[SerializeField] private Transform _ground;

	private List<IWorldItem> _items = new List<IWorldItem>();

	[Inject]
	private ICollisionHandler<IWorldItem> _collisionHandler;

#endregion

#region Private methods

	private void ChangeValueByMax(ref float value, float length) {
		if (length < float.Epsilon) {
			return;
		}
		if (value < 0) {
			while (value < 0) {
				value += length;
			}
		} else while (value >= length) {
			value -= length;
		}
	}

#endregion

#region IWorldSnakes

	public Vector2Int Size => _size;

	protected virtual Vector3 GetRandomPosition() {
		Vector3 position = Vector3.zero;
		position.x = UnityEngine.Random.Range(0, _size.x);
		position.z = UnityEngine.Random.Range(0, _size.y);
		return position;
	}

	public Vector3 GetRandomFreePosition() {
		Vector3 position = Vector3Int.zero;
		CollisionData collisionData = null;
		do {
			position = GetRandomPosition();
			collisionData = GetCollisionItemInPosition(position);
		} while (collisionData.Item != null);

		return position;
	}

	public void AddItem(IWorldItem item, bool randomPosition = true) {
		_collisionHandler.AddItem(item);
		_items.Add(item);

		if (randomPosition) {
			item.SetPosition(GetRandomFreePosition());
		}

		item.SetParent(transform);
		item.OnDestroyed += (IWorldItem itemDestroyed) => {
			if (_items.Contains(itemDestroyed)) {
				_items.Remove(itemDestroyed);
			}
		};
	}

	public void ChangePositionInWorldBorders(ref Vector3 position) {
		var x = position.x;
		ChangeValueByMax(ref x, _size.x);
		position.x = x;
		
		var z = position.z;
		ChangeValueByMax(ref z, _size.y);
		position.z = z;
	}
	
	public CollisionData GetCollisionItemInPosition(Vector3 position) {
		ChangePositionInWorldBorders(ref position);
		foreach (var item in _items) {
			if (item.IsPointInBodyItem(position)) {
				return new CollisionData(item, item.WorldItemType);
			}
		}
		return new CollisionData(null, WorldItemType.None);
	}

	public IWorldItem[] GetCollisionItemsInPosition(Vector3 position) {
		var res = new List<IWorldItem>();
		ChangePositionInWorldBorders(ref position);
		foreach (var item in _items) {
			if (item.IsPointInBodyItem(position)) {
				res.Add(item);
			}
		}
		return res.ToArray();
	}

	public Vector3 GetPointInCycleLine(Transform transform) {
		var size = Size;
		size.x--;
		size.y--;
		var forward     = transform.forward;
		var forward2Int = forward.ConvertXyzToXz().ConvertToInt();
		var diffLookPosition = (forward *
								((int)((Vector2Int.Scale(forward2Int,
														size)).magnitude))).ConvertToInt();
		var res = transform.localPosition.ConvertToInt() + diffLookPosition;
		return res;
	}
	
	public CollisionData GetCollisionByRayFromPointToPoint(Vector3 from, Vector3 to) {
		var from2Int = from.ConvertXyzToXz().ConvertToInt();
		var to2Int = to.ConvertXyzToXz().ConvertToInt();

		float lengthX = 0;
		float lengthY = 0;
		if (from2Int.x != 0 || to2Int.x != 0) {
			lengthX = to2Int.x - from2Int.x;
		}
		if (from2Int.y != 0 || to2Int.y != 0) {
			lengthY = to2Int.y - from2Int.y;
		}
		
		if (Mathf.Abs(lengthX) > float.Epsilon) {
			var step = 1;
			if (lengthX < 0) {
				step = -1;
			}
			var y = from2Int.y;
			var x = (int)Mathf.Round(from2Int.x);
			var maxX = (int)Mathf.Round(from2Int.x + lengthX);

			while (x != maxX) {
				var pos = new Vector3(x, 0, y);
				ChangePositionInWorldBorders(ref pos);
				var collisionData = GetCollisionItemInPosition(pos);
				if (collisionData.Item != null && (pos != to && pos != from)) {
					return collisionData;
				}
				x = x + step;
			}
		} else if (Mathf.Abs(lengthY) > float.Epsilon) {
			var step = 1;
			if (lengthY < 0) {
				step = -1;
			}
			var x = from2Int.x;
			var y    = (int)Mathf.Round(from2Int.y);
			var maxY = (int)Mathf.Round(from2Int.y + lengthY);
			while (y != maxY) {
				var pos = new Vector3(x, 0, y);
				ChangePositionInWorldBorders(ref pos);
				var collisionData = GetCollisionItemInPosition(pos);
				if (collisionData.Item != null && (pos != to && pos != from)) {
					return collisionData;
				}
				y = y + step;
			}
		}
		return new CollisionData();
	}

#endregion

#region Unity Events

	private void Awake() {
		_ground.localScale = new Vector3(Size.x, 1, Size.y);
		//_ground.localPosition = Size.ConvertXzToXyz().ConvertToFloat()/2;
	}

#endregion

}

}