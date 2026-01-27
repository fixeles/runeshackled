using Common;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems
{
	public class SaveSystem : IEcsPostDestroySystem, IEcsRunSystem
	{
		private readonly GameProgress _gameProgress;

		
		[Inject]
		public SaveSystem(GameProgress gameProgress)
		{
			_gameProgress = gameProgress;
		}

		public void PostDestroy(IEcsSystems systems)
		{
		}

		public void Run(IEcsSystems systems)
		{
			_gameProgress.Playtime += Time.deltaTime;
		}
	}
}