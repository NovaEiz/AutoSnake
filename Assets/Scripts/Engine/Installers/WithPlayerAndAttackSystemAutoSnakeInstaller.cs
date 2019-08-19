using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class WithPlayerAndAttackSystemAutoSnakeInstaller : MonoInstaller {

#region Private fields

	[Header("WithPlayerAndAttackSystemAutoSnakeInstaller - Settable fields")]
	[SerializeField] private BaseAutoSnakeInstaller _baseInstaller;
	
	[Space]
	[SerializeField] private SnakeController _snakeController;
	[SerializeField] private PlayerSpawner _playerSpawner;
	
#endregion

#region Private methods

	public override void InstallBindings() {
		_baseInstaller.Container.Bind<SnakeController>().FromInstance(_snakeController);
		_baseInstaller.Container.Bind<PlayerSpawner>().FromInstance(_playerSpawner);
	}

#endregion
	
}

}