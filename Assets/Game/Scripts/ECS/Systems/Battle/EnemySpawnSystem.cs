using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using ECS.Mono;
using FPS.Pool;
using JetBrains.Collections.Viewable;
using Leopotam.EcsLite;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.Systems.Battle
{
	public class EnemySpawnSystem : IEcsSystem, IStateEnter
	{
		private readonly Lifetime _appLifetime;
		private readonly EcsWorld _world;
		private readonly CMS _cms;
		private readonly IObjectPool _pool;
		public AppState TargetState => AppState.Battle;

		[Inject]
		public EnemySpawnSystem(EcsWorld world, CMS cms, IObjectPool pool)
		{
			_world = world;
			_cms = cms;
			_pool = pool;
		}

		public void Enter(Lifetime lifetime)
		{
			var spawnerEntity = _world.CreateLifetimedEntity(lifetime);
			ref var timerComponent = ref _world.GetPool<TimerComponent>().Add(spawnerEntity);
			timerComponent.TimeLeft = _cms.GameConfig.EnemySpawnFrequency;
			timerComponent.Callback += () => SpawnEnemies(lifetime);
		}

		private void SpawnEnemies(Lifetime lifetime)
		{
			var enemyEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<EnemyTag>().Add(enemyEntity);

			var id = _cms.GameConfig.EnemyConfig.ViewId;
			_world.GetPool<MonoReference<NavigationAgent>>().Add(enemyEntity).Reference = _pool.Get<NavigationAgent>();

			var follower = _pool.Get<NavigationFollower>(id);
			_world.GetPool<MonoReference<NavigationFollower>>().Add(enemyEntity).Reference = follower;

			AddHitable(enemyEntity, follower);
			AddHealth(enemyEntity);
			AddAggro(enemyEntity);
			AddAttack(enemyEntity, follower);
		}

		private void AddAttack(int enemyEntity, NavigationFollower follower)
		{
			_world.GetPool<MonoReference<AttackableMono>>().Add(enemyEntity).Reference
				= follower.GetComponent<AttackableMono>();
		}

		private void AddHealth(int enemyEntity)
		{
			ref var health = ref _world.GetPool<HealthComponent>().Add(enemyEntity);
			health.MaxHealth = new ViewableProperty<float>(100);
			health.CurrentHealth = new ViewableProperty<float>(100);
		}

		private void AddHitable(int entity, NavigationFollower follower)
		{
			var hitable = _world.GetPool<MonoReference<HitableMono>>().Add(entity).Reference =
				follower.GetComponentInChildren<HitableMono>();
			hitable.Entity = entity;
			hitable.SetActive(true);
		}

		private void AddAggro(int enemyEntity)
		{
			_world.GetPool<AggroComponent>().Add(enemyEntity).AggroRadius = 5;
		}
	}
}