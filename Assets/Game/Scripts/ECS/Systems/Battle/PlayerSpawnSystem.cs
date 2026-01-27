using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using FPS.Pool;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle
{
	public class PlayerSpawnSystem : IStateEnter, IEcsSystem
	{
		private readonly CMS _cms;
		private readonly EcsWorld _world;
		private readonly CinemachineCamera _mainCamera;
		private readonly IObjectPool _pool;

		public AppState TargetState => AppState.Battle;


		[Inject]
		public PlayerSpawnSystem(CMS cms, EcsWorld world,
			CinemachineCamera mainCamera, IObjectPool pool)
		{
			_cms = cms;
			_world = world;
			_mainCamera = mainCamera;
			_pool = pool;
		}

		public void Enter()
		{
			var playerEntity = CreatePlayer();
			AddAttackSkill(playerEntity);
		}

		private int CreatePlayer()
		{
			var playerEntity = _world.NewEntity();
			_world.GetPool<PlayerTag>().Add(playerEntity);
			var navigationFollower = Object.Instantiate(_cms.Prefabs.PlayerCharacter);
			_world.GetPool<MonoReference<NavigationFollower>>().Add(playerEntity).Reference = navigationFollower;
			_world.GetPool<MonoReference<NavigationAgent>>().Add(playerEntity).Reference = _pool.Get<NavigationAgent>();
			_world.GetPool<MonoReference<AttackableMono>>().Add(playerEntity).Reference = navigationFollower.GetComponent<AttackableMono>();

			_mainCamera.Follow = navigationFollower.transform;

			ref var lookComponent = ref _world.GetPool<LookDirection>().Add(playerEntity);
			lookComponent.Tracker = navigationFollower.GetComponentInChildren<LookTracker>();
			lookComponent.TargetLocalRotation = Quaternion.identity;
			lookComponent.RotationSpeed = 5f;

			return playerEntity;
		}

		private void AddAttackSkill(int playerEntity)
		{
			var skillEntity = _world.NewEntity();
			_world.GetPool<ChildComponent>().Add(skillEntity).OwnerEntity = playerEntity;
			_world.GetPool<MeleeAttack>().Add(skillEntity);
			_world.GetPool<Damage>().Add(skillEntity).Value = 50;
			_world.GetPool<Range>().Add(skillEntity).Value = 3;
			_world.GetPool<PhysicInfluence>().Add(skillEntity).LayerMask = LayerMask.GetMask("Enemy");
			_world.GetPool<SelectedSkill>().Add(skillEntity);
		}
	}
}