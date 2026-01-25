using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle
{
	public class SpawnPlayerSystem : IStateEnter, IEcsSystem
	{
		private readonly CMS _cms;
		private readonly EcsWorld _world;
		private readonly CinemachineCamera _mainCamera;

		[Inject]
		public SpawnPlayerSystem(CMS cms, EcsWorld world, CinemachineCamera mainCamera)
		{
			_cms = cms;
			_world = world;
			_mainCamera = mainCamera;
		}

		public AppState TargetState => AppState.Battle;


		public void Enter()
		{
			var playerEntity = _world.NewEntity();
			_world.GetPool<Player>().Add(playerEntity);
			var unitView = Object.Instantiate(_cms.Prefabs.PlayerCharacter);
			_world.GetPool<MonoReference<UnitView>>().Add(playerEntity).View = unitView;
			_mainCamera.Follow = unitView.transform;
			
			ref var movable = ref _world.GetPool<Movable>().Add(playerEntity);
		}
	}
}