using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class BaseAutoSnakeInstaller : MonoInstaller {

#region Private fields

	[Header("AutoSnakeInstaller - Settable fields")]
	[SerializeField] protected List<MonoInstaller> _childInstallers;
	
	[Space]
	[SerializeField] private GameObject _iWorld_GameObject;
	[SerializeField] private GameObject _iMovingSystem_GameObject;
	[SerializeField] private GameObject _iCollisionHandler_GameObject;

	[SerializeField] private GameObject _iSnakePopulationController_GameObject;
	
	[Space]
	[SerializeField] private SnakeFactory _snakeFactory;
	[SerializeField] private FoodGenerator _foodGenerator;

	[Space]
	[SerializeField] private EatingSystem _eatingSystem;

#endregion
	
#region Public fields

	public DiContainer Container => base.Container;

#endregion

#region Private methods

	protected T GetAndBind<T>(GameObject gameObject) {
		var instance = gameObject.GetComponent<T>();

		Container.Bind<T>()
				.FromInstance(instance);
		
		return instance;
	}

	public override void InstallBindings() {
		GetAndBind<IWorld>(_iWorld_GameObject);
		var movingSystem = GetAndBind<ISnakeMovingSystem>(_iMovingSystem_GameObject);
		GetAndBind<ICollisionHandler<IWorldItem>>(_iCollisionHandler_GameObject);
		GetAndBind<ISnakePopulationController>(_iSnakePopulationController_GameObject);
		
		Container.Bind<SnakeFactory>().FromInstance(_snakeFactory);
		Container.Bind<FoodGenerator>().FromInstance(_foodGenerator);
		Container.Bind<EatingSystem>().FromInstance(_eatingSystem);

		foreach (var childInstaller in _childInstallers) {
			childInstaller.InstallBindings();
		}
	}

#endregion
	
}

}