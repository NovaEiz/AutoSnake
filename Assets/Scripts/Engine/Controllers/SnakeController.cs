using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class SnakeController : MonoBehaviour {

#region Private fields
	
	[Inject]
	private IWorld _world;
	[Inject]
	private ISnakeMovingSystem _movingSystem;

	private ISnake _snake;

#endregion
	
#region Private methods

	private void MoveByDirection(Vector3 direction) {
		var position      = (_snake.Position + direction).ConvertToInt();
		var collisionItem = _world.GetCollisionItemInPosition(position);
		if (_movingSystem.CanWalkOnBy(collisionItem.Item, _snake, position)) {
			_snake.MoveTo(position);
		}
	}

#endregion
	
#region Public methods

	public void SetSnake(ISnake value) {
		_snake = value;
	}

#endregion

#region Unity Event methods

	private void Update() {
		if (_snake == null) {
			return;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			MoveByDirection(Vector3.left);
		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			MoveByDirection(Vector3.right);
		} else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveByDirection(Vector3.forward);
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			MoveByDirection(Vector3.back);
		}
	}

#endregion

}

}