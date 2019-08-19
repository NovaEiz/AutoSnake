using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class EatingSystem : MonoBehaviour {

#region Private fields
	
	[Inject]
	private ICollisionHandler<IWorldItem> _collisionHandler;

#endregion

#region Private methods

	private void OnCollision(IWorldItem snakeItem, IWorldItem food) {
		if (food.WorldItemType != WorldItemType.Food) {
			return;
		}
		var snake = snakeItem as ISnake;
		snake.AddLength();
		food.Destroy();
	}

#endregion

#region Unity Event methods

	private void Start() {
		_collisionHandler.OnCollision += OnCollision;
	}

#endregion

}

}