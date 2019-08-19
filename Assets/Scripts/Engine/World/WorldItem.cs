using System;
using UnityEngine;

namespace AutoSnake.Engine {

public class WorldItem : MonoBehaviour, IWorldItem {

#region Private fields

	[Header("WorldItem - Settable fields")]
	[SerializeField] private WorldItemType _worldItemType;
	
	[Header("WorldItem - Debug fields")]
	[SerializeField] private float _speed;
	
	public virtual Transform Face => transform;

#endregion

#region IWorldItem

	public virtual Vector3 Position => transform.localPosition;

	public WorldItemType WorldItemType => _worldItemType;
	
	public float Speed => _speed;

	public void SetWorldItemType(WorldItemType value) {
		_worldItemType = value;
	}
	
	public virtual void SetPosition(Vector3 position) {
		transform.localPosition = position;
	}

	public virtual void MoveTo(Vector3 toPosition) {
		transform.localPosition = toPosition;
		
		Moved();
	}

	public virtual void SetDirection(Vector3 direction) {
		transform.rotation = Quaternion.LookRotation(direction);
	}

	public virtual Vector3 GetNextPosition() {
		return (transform.localPosition + transform.forward).ConvertToInt();;
	}

	public virtual bool IsPointInBodyItem(Vector3 position) {
		if (Position == position) {
			return true;
		}
		return false;
	}

	public virtual void Destroy() {
		Destroy(gameObject);
		OnDestroyed?.Invoke(this);
	}

	public void SetParent(Transform parent) {
		transform.SetParent(parent);
	}

	protected void Moved() {
		OnMoved?.Invoke(this);
	}

	public event Action<IWorldItem> OnDestroyed;
	public event Action<IWorldItem> OnMoved;

#endregion

}

}