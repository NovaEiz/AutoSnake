using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class SliceTailSnakeAttackSystem : SnakeAttackSystem {

#region Private methods

	protected override void OnCollision(IWorldItem item1, IWorldItem item2) {
		var snake1 = (item1 as ISnake);
		var snake2 = (item2 as ISnake);
		if (snake2 == null) {
			return;
		}
		if (item1.Position == item2.Position) {
			snake1?.Destroy();
			snake2?.Destroy();
		} else {
			snake2.DestroyTailFromPosition(item1.Position);
		}
	}

#endregion

}

}