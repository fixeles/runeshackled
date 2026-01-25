using System;
using Common;
using ECS.FSM;
using FPS;
using UnityEngine;
using VContainer;

namespace Commands
{
	public class StartGameLoopCommand : SyncCommand
	{
		private readonly IAppStateMachine _appStateMachine;
		private readonly GameProgress _gameProgress;

		[Inject]
		public StartGameLoopCommand(IAppStateMachine appStateMachine, GameProgress gameProgress)
		{
			_appStateMachine = appStateMachine;
			_gameProgress = gameProgress;
		}

		public override void Do()
		{
			try
			{
				_appStateMachine.SetState(_gameProgress.IsTutorialComplete ? AppState.Hub : AppState.Tutorial);
				Status = CommandStatus.Success;
			}
			catch (Exception e)
			{
				Status = CommandStatus.Error;
				Debug.LogException(e);
			}
		}
	}
}