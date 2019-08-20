using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class SnakeAttackSystem : MonoBehaviour {

#region Private fields

	[Inject]
	private ICollisionHandler<IWorldItem> _collisionHandler;

#endregion

#region Private methods

	protected virtual void OnCollision(IWorldItem item1, IWorldItem item2) {
		var snake1 = (item1 as ISnake);
		var snake2 = (item2 as ISnake);
		if (snake2 == null) {
			return;
		}
		if (item1.Position.ConvertToInt() == item2.Position.ConvertToInt()) {
			snake1?.Destroy();
			snake2?.Destroy();
		} else {
			snake2?.Destroy();
		}
	}

#endregion

#region Unity Event methods

	private void Start() {
		_collisionHandler.OnCollision += OnCollision;
	}

#endregion

}

}