using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class FoodGenerator : MonoBehaviour {

#region Private fields

	[Header("FoodGenerator - Settable fields")]
	[SerializeField] private int _amountFood = 1;
	[SerializeField] private GameObject _foodPrefab;

	[Header("FoodGenerator - Debug fields")]
	[SerializeField] private int _currentAmountFood;

	[Inject]
	private IWorld _world;

#endregion

#region Public fields



#endregion

#region Private methods

	private void OnDestroyedItem(IWorldItem item) {
		_currentAmountFood--;
	}

	private void CreateItem() {
		_currentAmountFood++;

		var item = Instantiate(_foodPrefab, _world.transform).GetComponent<IWorldItem>();

		item.OnDestroyed += OnDestroyedItem;
		
		_world.AddItem(item);
	}

#endregion

#region Unity Event methods

	private void Update() {
		if (_currentAmountFood < _amountFood) {
			CreateItem();
		}
	}

#endregion

}

}