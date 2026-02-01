using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.Mono;
using Enum;
using FPS.Pool;
using JetBrains.Collections.Viewable;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle
{
	public class CombatUnitInitSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;
		private readonly IObjectPool _objectPool;
		private readonly CMS _cms;

		[Inject]
		public CombatUnitInitSystem(EcsWorld world, IObjectPool objectPool, CMS cms)
		{
			_world = world;
			_objectPool = objectPool;
			_cms = cms;
			_filter = _world.Filter<UnitId>().Inc<InitRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var id = _world.GetPool<UnitId>().Get(entity);
				var config = _cms.GameConfig.CombatUnits.Get(id);

				var navigationAgent = _objectPool.Get<NavigationAgent>();
				_world.GetPool<MonoReference<NavigationAgent>>().Add(entity).Reference = navigationAgent;

				var follower = _objectPool.Get<NavigationFollower>(id.ToString());
				_world.GetPool<MonoReference<NavigationFollower>>().Add(entity).Reference = follower;

				_world.GetPool<PositionComponent>().Add(entity).Value = navigationAgent.CachedTransform.position;

				AddHitable(entity, config, follower);
				AddAggro(entity, config);
				TryAddAttack(entity, follower);
				AddLookTracker(entity, config, follower);
				AddSkills(entity, config);
			}
		}


		private void TryAddAttack(int enemyEntity, NavigationFollower follower)
		{
			_world.GetPool<MonoReference<AttackableMono>>().Add(enemyEntity).Reference
				= follower.GetComponent<AttackableMono>();
		}

		private void AddHitable(int entity, CombatUnitConfig config, NavigationFollower follower)
		{
			if (config.Health <= 0)
				return;

			ref var health = ref _world.GetPool<HealthComponent>().Add(entity);
			health.MaxHealth = config.Health;
			health.CurrentHealth = config.Health;

			var hitable = _world.GetPool<MonoReference<HitableMono>>().Add(entity).Reference =
				follower.GetComponentInChildren<HitableMono>();
			hitable.Entity = entity;
			hitable.SetActive(true);
		}

		private void AddLookTracker(int entity, CombatUnitConfig config, NavigationFollower navigationFollower)
		{
			ref var lookComponent = ref _world.GetPool<LookDirection>().Add(entity);
			lookComponent.Tracker = navigationFollower.GetComponentInChildren<LookTracker>();
			lookComponent.TargetRotation = Quaternion.identity;
			lookComponent.RotationSpeed = config.RotationSpeed;
		}

		private void AddAggro(int enemyEntity, CombatUnitConfig config)
		{
			if (config.AggroRadius <= 0)
				return;

			_world.GetPool<AggroComponent>().Add(enemyEntity).AggroRadius = config.AggroRadius;
		}


		private void AddSkills(int playerEntity, CombatUnitConfig config)
		{
			foreach (var skillId in config.Skills)
			{
				Debug.Log(skillId);
				var skillEntity = _world.CreateLifetimedEntity(playerEntity);
				_world.GetPool<SkillId>().Add(skillEntity) = skillId;
				_world.GetPool<InitRequest>().Add(skillEntity);
				_world.GetPool<ChildComponent>().Add(skillEntity).OwnerEntity = playerEntity;
			}
		}
	}
}