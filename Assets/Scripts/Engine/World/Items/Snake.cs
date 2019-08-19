using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class Snake : WorldItem, ISnake {
	
#region Factory

	public void Construct(int initialLength, Vector3Int position, Vector3 direction, IWorld world) {
		_length = initialLength;
		_world = world;

		AddPartFromPrefab(_facePrefab);
		
		SetPosition(position);
		SetDirection(direction);

		for (var i=1; i<_length; i++) {
			AddPartFromPrefab(_bodyPrefab);
		}
	}

#endregion
	
#region IWorldSnakesItem
	
	public override Transform Face => _parts[0];

	public override Vector3 Position => Face.localPosition;
	
	public override bool IsPointInBodyItem(Vector3 position) {
		var positionsOfBodyParts = GetPositionsOfBodyParts();
		positionsOfBodyParts.Add(Position);
		foreach (var positionPart in positionsOfBodyParts) {
			if (position.ConvertToInt() == positionPart.ConvertToInt()) {
				return true;
			}
		}
		return false;
	}

	public override void SetDirection(Vector3 direction) {
		Face.transform.rotation = Quaternion.LookRotation(direction);
	}

	public override void SetPosition(Vector3 position) {
		_world.ChangePositionInWorldBorders(ref position);

		var        offset   = position - Face.transform.localPosition;
		Face.transform.localPosition = position;

		var len = _parts.Count;
		for (var i=1; i<len; i++) {
			var part = _parts[i];
			
			part.transform.localPosition = part.transform.localPosition + offset;
		}
	}
	
	public override void MoveTo(Vector3 position) {
		_world.ChangePositionInWorldBorders(ref position);

		var face = Face;

		_prevPositionNextPart = face.localPosition;

		face.localPosition = position;
		
		var len = _parts.Count;
		for (var i=1; i<len; i++) {
			var part = _parts[i];

			var nextPosition = _prevPositionNextPart;
			_prevPositionNextPart = part.localPosition;
			part.localPosition    = nextPosition;
		}

		Moved();
	}

	public override Vector3 GetNextPosition() {
		var face         = Face;
		var npFloat      = (face.localPosition + face.forward);
		var nextPosition = npFloat.ConvertToInt();
		return nextPosition;
	}
	
#endregion

#region Private fields

	[Header("Snake - Settable fields")]
	[SerializeField] private GameObject _facePrefab;
	[SerializeField] private GameObject _bodyPrefab;
	
	[Header("Snake - Debug fields")]
	[SerializeField] private int _length;
	[SerializeField] private List<Transform> _parts;

	private IWorld _world;

	private IWorldItem _targetFood;

	private Vector3 _prevPositionNextPart;

#endregion
	
#region Public fields

	public IWorldItem TargetFood => _targetFood;

#endregion

#region Private methods

	protected virtual void DestroyPart(Transform part) {
		Destroy(part.gameObject);
	}

	private void RemovePart(Transform part) {
		if (_parts.Contains(part)) {
			_parts.Remove(part);
			DestroyPart(part);
		}
	}

	private void AddPartFromPrefab(GameObject prefab) {
		var part = Instantiate(prefab, transform).transform;

		Vector3 position = Vector3.zero;

		if (_parts.Count == 1) {
			var face = Face;
			position = face.localPosition - face.forward;
			_world.ChangePositionInWorldBorders(ref position);
		} else if (_parts.Count >= 2) {
			var lastPart = _parts[_parts.Count - 1];
			var preLastPart = _parts[_parts.Count - 2];

			if (Vector3.Distance(_prevPositionNextPart, lastPart.localPosition) <= 1) {
				position = _prevPositionNextPart.ConvertToInt();
			} else {
				position = lastPart.localPosition - (preLastPart.localPosition - lastPart.localPosition);
			}

			_world.ChangePositionInWorldBorders(ref position);
		}
		
		part.localPosition = position;
		
		_parts.Add(part);
	}

#endregion

#region ISnake
	
	public int Length => _length;
	
	public void AddLength() {
		_length++;

		AddPartFromPrefab(_bodyPrefab);
	}
	
	public Vector3 GetPositionHead() {
		return Face.transform.localPosition.ConvertToInt();
	}

	public void TurnRandom() {
		var face = Face;
		var faceTr = face.transform;
		var rand = UnityEngine.Random.Range(0, 2);

		Vector3 direction = Vector3.forward;

		if (rand == 0) {
			direction = -faceTr.right;
		} else {
			direction = faceTr.right;
		}
		
		if (_world.GetCollisionItemInPosition((face.localPosition + direction).ConvertToInt()).Item == null) {
			SetDirection(direction);
		}
	}

	public void SetTargetFood(IWorldItem value) {
		_targetFood = value;
		_targetFood.OnDestroyed += (itemDestroyed) => {
			_targetFood = null;
		};
	}

	public Transform[] GetParts() {
		return _parts.GetRange(0, _parts.Count).ToArray();
	}

	public List<Vector3> GetPositionsOfBodyParts() {
		List<Vector3> positionsOfBodyParts = new List<Vector3>();

		var len = _parts.Count;
		for (var i=1; i<len; i++) {
			var part = _parts[i];
			positionsOfBodyParts.Add(part.localPosition.ConvertToInt());
		}

		return positionsOfBodyParts;
	}

	public virtual void DestroyTailFromPosition(Vector3 position) {
		var started = false;
		var parts = GetParts();
		foreach (var part in parts) {
			if (position.ConvertToInt() == part.localPosition.ConvertToInt()) {
				started = true;
			}

			if (started) {
				RemovePart(part);
			}
		}
	}

#endregion
	
}

}