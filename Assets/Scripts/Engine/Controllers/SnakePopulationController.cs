using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class SnakePopulationController : MonoBehaviour, ISnakePopulationController {

#region Private fields

	[Header("SnakePopulationController - Settable fields")]
	[SerializeField] private int _amountSnakes = 1;
	[SerializeField] private int _initialLengthSnake = 1;
	
	[Header("SnakePopulationController - Debug fields")]
	[SerializeField] private int _currentAmountSnakes;

	[Inject]
	private SnakeFactory _snakeFactory;
	[Inject]
	private ISnakeMovingSystem _snakeMovingSystem;

#endregion
	
#region Private methods

	private void CreateSnake() {
		_currentAmountSnakes++;
		var snake = _snakeFactory.CreateWithRandomPosition(_initialLengthSnake);
		_snakeMovingSystem.AddItem(snake);
		snake.OnDestroyed += (snakeDestroyed) => {
			_currentAmountSnakes--;
		};
	}

	private bool NeedCreateSnake() {
		if (_currentAmountSnakes < _amountSnakes) {
			return true;
		}
		return false;
	}
	
#endregion

#region Unity Event methods

	private void Update() {
		if (NeedCreateSnake()) {
			CreateSnake();
		}
	}

#endregion

}

}