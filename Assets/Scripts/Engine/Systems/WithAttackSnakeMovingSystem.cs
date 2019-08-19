using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoSnake.Engine {

public class WithAttackSnakeMovingSystem : SnakeMovingSystem {
	
#region Public methods
	
	public override bool CanWalkOnBy(IWorldItem itemCollision, ISnake item, Vector3 position) {
		if (itemCollision == null) {
			return true;
		}
		var res = true;
		switch (itemCollision.WorldItemType) {
			case WorldItemType.Barrier:
				res = false;
				break;
			case WorldItemType.Snake:
				if (itemCollision.transform.GetInstanceID() == item.transform.GetInstanceID()) {
					res = false;
				}
				break;
		}
		return res;
	}

#endregion
	
}

}