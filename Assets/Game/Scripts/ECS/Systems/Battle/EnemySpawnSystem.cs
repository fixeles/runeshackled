using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using Enum;
using FPS.Pool;
using Leopotam.EcsLite;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.Systems.Battle
{
	public class EnemySpawnSystem : IStateEnter, IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly CMS _cms;
		private readonly IObjectPool _pool;
		private readonly EcsFilter _spawnerFilter;
		public AppState TargetState => AppState.Battle;

		[Inject]
		public EnemySpawnSystem(EcsWorld world, CMS cms, IObjectPool pool)
		{
			_world = world;
			_cms = cms;
			_pool = pool;
			_spawnerFilter = _world.Filter<SpawnerComponent>().Exc<CooldownComponent>().End();
		}

		public void Enter(Lifetime lifetime)
		{
			var spawnerEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<SpawnerComponent>().Add(spawnerEntity);
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _spawnerFilter)
			{
				var spawnerLifetime = _world.GetPool<LifetimeComponent>().Get(entity).Lifetime;
				SpawnEnemy(spawnerLifetime);
				_world.GetPool<CooldownComponent>().Add(entity).TimeLeft = _cms.GameConfig.EnemySpawnFrequency;
			}
		}
		
		private void SpawnEnemy(Lifetime lifetime)
		{
			var enemyEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<EnemyTag>().Add(enemyEntity);
			_world.GetPool<UnitId>().Add(enemyEntity) = UnitId.base_enemy;
			_world.GetPool<InitRequest>().Add(enemyEntity);
		}
	}
}