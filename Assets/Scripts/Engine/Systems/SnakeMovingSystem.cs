using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoSnake.Engine {

public class SnakeMovingSystem : MovingSystem<ISnake, IWorldItem>, ISnakeMovingSystem {
	
#region Public methods
	
	public override bool CanWalkOnBy(IWorldItem itemCollision, ISnake item, Vector3 position) {
		if (itemCollision == null) {
			return true;
		}
		var res = true;
		switch (itemCollision.WorldItemType) {
			case WorldItemType.Barrier:
			case WorldItemType.Snake:
				res = false;
				break;
		}
		return res;
	}

	private bool CheckTarget(ISnake item) {
		var food = item.TargetFood;
		var collisionData =
			_world.GetCollisionByRayFromPointToPoint(item.Position, _world.GetPointInCycleLine(item.Face));
		if (food != null && collisionData.ItemType == WorldItemType.Food) {
			return true;
		}
		return false;
	}
	
	private void ChanceTurn(ISnake item) {
		var chance = UnityEngine.Random.Range(0f, 100f);
		if (chance <= 20f) {
			item.TurnRandom();
		}
	}

	public override bool TryMove(ISnake item) {
		if (!CanMoveForward(item)) {
			if (CanMoveLeft(item)) {
				if (!CheckTarget(item)) {
					item.SetDirection(-item.Face.right);
				}
			} else if (CanMoveRight(item)) {
				if (!CheckTarget(item)) {
					item.SetDirection(item.Face.right);
				}
			} else {
				return false;
			}
		}

		return true;
	}

	public override void Move(ISnake item) {
		var nextPosition = item.GetNextPosition();
		_world.ChangePositionInWorldBorders(ref nextPosition);
		item.MoveTo(nextPosition);

		if (item.TargetFood == null) {
			var collisionItem = _world.GetCollisionByRayFromPointToPoint(item.Position, _world.GetPointInCycleLine(item.Face));
			if (collisionItem.Item != null && collisionItem.Item.WorldItemType == WorldItemType.Food) {
				item.SetTargetFood(collisionItem.Item);
			}
		}

		if (!CheckTarget(item)) {
			ChanceTurn(item);
		}
	}

#endregion
	
}

}