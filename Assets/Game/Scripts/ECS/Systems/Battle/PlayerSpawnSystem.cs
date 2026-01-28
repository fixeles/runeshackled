using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using ECS.Mono;
using FPS.Pool;
using JetBrains.Collections.Viewable;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

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

		public void Enter(Lifetime lifetime)
		{
			var playerEntity = CreatePlayer(lifetime);
			AddAttackSkill(playerEntity);
		}

		private int CreatePlayer(Lifetime lifetime)
		{
			var playerEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<PlayerTag>().Add(playerEntity);
			var navigationFollower = Object.Instantiate(_cms.Prefabs.PlayerCharacter);
			_world.GetPool<MonoReference<NavigationFollower>>().Add(playerEntity).Reference = navigationFollower;
			_world.GetPool<MonoReference<NavigationAgent>>().Add(playerEntity).Reference = _pool.Get<NavigationAgent>();
			_world.GetPool<MonoReference<AttackableMono>>().Add(playerEntity).Reference = navigationFollower.GetComponent<AttackableMono>();

			_mainCamera.Follow = navigationFollower.transform;

			AddLookTracker(playerEntity, navigationFollower);
			AddHitable(playerEntity, navigationFollower);
			AddHealth(playerEntity);

			return playerEntity;
		}

		private void AddHitable(int entity, NavigationFollower follower)
		{
			var hitable = _world.GetPool<MonoReference<HitableMono>>().Add(entity).Reference =
				follower.GetComponentInChildren<HitableMono>();
			hitable.Entity = entity;
			hitable.SetActive(true);
		}

		private void AddHealth(int enemyEntity)
		{
			ref var health = ref _world.GetPool<HealthComponent>().Add(enemyEntity);
			health.MaxHealth = new ViewableProperty<float>(200);
			health.CurrentHealth = new ViewableProperty<float>(200);
		}

		private void AddLookTracker(int playerEntity, NavigationFollower navigationFollower)
		{
			ref var lookComponent = ref _world.GetPool<LookDirection>().Add(playerEntity);
			lookComponent.Tracker = navigationFollower.GetComponentInChildren<LookTracker>();
			lookComponent.TargetLocalRotation = Quaternion.identity;
			lookComponent.RotationSpeed = 5f;
		}

		private void AddAttackSkill(int playerEntity)
		{
			var playerLifetime = _world.GetPool<LifetimeComponent>().Get(playerEntity).Lifetime;
			var skillEntity = _world.CreateLifetimedEntity(playerLifetime);

			_world.GetPool<ChildComponent>().Add(skillEntity).OwnerEntity = playerEntity;
			_world.GetPool<MeleeAttack>().Add(skillEntity);
			_world.GetPool<Damage>().Add(skillEntity).Value = 50;
			_world.GetPool<Range>().Add(skillEntity).Value = 3;
			_world.GetPool<PhysicInfluence>().Add(skillEntity).LayerMask = LayerMask.GetMask("Enemy");
			_world.GetPool<SelectedSkill>().Add(skillEntity);
		}
	}
}