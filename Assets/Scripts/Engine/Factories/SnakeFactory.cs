using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class SnakeFactory : MonoBehaviour {

	[Header("SnakeFactory - Settable fields")]
	[SerializeField] private GameObject _prefab;

	[Inject]
	private IWorld _world;
	
	public ISnake Create(int initialLength, Vector3Int position, Vector3 direction) {
		var instance = Instantiate(_prefab);
		var snake    = instance.GetComponent<ISnake>();
		(snake as Snake).Construct(initialLength, position, direction, _world);
		OnCreated?.Invoke(snake);
		_world.AddItem(snake, false);
		return snake;
	}

	public ISnake CreateWithRandomPosition(int initialLength) {
		var snake = Create(initialLength, GetRandomFreePosition().ConvertXzToXyz(), GetRandomDirection());
		return snake;
	}
	
	private Vector2Int GetRandomFreePosition() {
		var size = _world.Size;
		var x        = UnityEngine.Random.Range(0, size.x);
		var y        = UnityEngine.Random.Range(0, size.y);
		var position = new Vector2Int(x, y);
		return position;
	}
	private Vector3 GetRandomDirection() {
		var rand = UnityEngine.Random.Range(0, 4);
		int x    = 0;
		int z    = 0;
		switch (rand) {
			case 0:
				x = -1;
				break;
			case 1:
				x = 1;
				break;
			case 2:
				z = -1;
				break;
			case 3:
				z = 1;
				break;
		}
		var direction = new Vector3(x, 0, z);
		return direction;
	}

	public event Action<ISnake> OnCreated;

}

}