using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoSnake.Engine {

public interface ISnakeMovingSystem : IMovingSystem<ISnake, IWorldItem> {
	
}

public interface ISnakePopulationController {

}

public interface ICollisionHandler<T> {
	event Action<T, T> OnCollision;

	CollisionData GetCollisionInPosition(Vector3 position);

	void AddItem(T item);
}

public interface IMovingSystem<T, T2> {
	void AddItem(T item);
	event Action<T> OnMovedItem;
	bool CanWalkOnBy(T2 itemCollision, T item, Vector3 position);
}

public interface IWorld {
	
	Transform  transform { get; }
	Vector2Int Size      { get; }
	
	CollisionData GetCollisionItemInPosition(Vector3 position);
	IWorldItem[] GetCollisionItemsInPosition(Vector3 position);
	void ChangePositionInWorldBorders(ref Vector3 position);

	void AddItem(IWorldItem item, bool randomPosition = true);
	CollisionData GetCollisionByRayFromPointToPoint(Vector3 from, Vector3 to);

	Vector3 GetPointInCycleLine(Transform transform);
}

public interface IWorldItem {
	
	Vector3 Position { get; }
	WorldItemType WorldItemType { get; }
	float Speed { get; }
	Transform transform { get; }
	Transform Face { get; }

	void SetWorldItemType(WorldItemType type);
	void SetPosition(Vector3 position);
	void SetDirection(Vector3 direction);
	void MoveTo(Vector3 toPosition);
	Vector3 GetNextPosition();
	
	bool IsPointInBodyItem(Vector3 position);

	void SetParent(Transform parent);

	void Destroy();

	event Action<IWorldItem> OnDestroyed;
	event Action<IWorldItem> OnMoved;
}

public interface ISnake : IWorldItem {
	
	Transform Face { get; }
	
	int Length { get; }
	
	IWorldItem TargetFood { get; }

	void AddLength();
	void MoveTo(Vector3 toPosition);

	Vector3 GetNextPosition();

	void SetTargetFood(IWorldItem item);

	void TurnRandom();
	
	Vector3 GetPositionHead();

	Transform[] GetParts();
	List<Vector3> GetPositionsOfBodyParts();
	
	void DestroyTailFromPosition(Vector3 position);

}

}