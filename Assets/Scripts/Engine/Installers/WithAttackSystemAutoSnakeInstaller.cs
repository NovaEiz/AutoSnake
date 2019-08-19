using System;
using UnityEngine;
using Zenject;

namespace AutoSnake.Engine {

public class WithAttackSystemAutoSnakeInstaller : MonoInstaller {

#region Private fields

	[Header("WithAttackAutoSnakeInstaller - Settable fields")]
	[SerializeField] private BaseAutoSnakeInstaller _baseInstaller;
	
	[Space]
	[SerializeField] private SnakeAttackSystem _snakeAttackSystem;

#endregion

#region Private methods

	public override void InstallBindings() {
		_baseInstaller.Container.Bind<SnakeAttackSystem>().FromInstance(_snakeAttackSystem);
	}

#endregion
	
}

}