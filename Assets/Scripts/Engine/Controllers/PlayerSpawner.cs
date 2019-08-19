using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class PlayerSpawner : MonoBehaviour {

#region Private fields

	[Inject]
	private SnakeController _snakeController;
	[Inject]
	private SnakeFactory _snakeFactory;

#endregion

#region Unity Event methods
	
	private ISnake CreateSnake() {
		var snake = _snakeFactory.CreateWithRandomPosition(4);
		snake.OnDestroyed += (snakeDestroyed) => {
			CreateSnake();
		};
		_snakeController.SetSnake(snake);
		return snake;
	}

	private void Start() {
		CreateSnake();
	}

#endregion

}

}