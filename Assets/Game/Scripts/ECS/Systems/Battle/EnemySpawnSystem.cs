using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using FPS.Pool;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle
{
	public class EnemySpawnSystem : IStateUpdate, IEcsSystem, IStateEnter, IStateExit
	{
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

		public void Enter()
		{
			var spawnerEntity = _world.NewEntity();
			ref var timerComponent = ref _world.GetPool<TimerComponent>().Add(spawnerEntity);
			timerComponent.LoopTime = _cms.GameConfig.EnemySpawnFrequency;
			timerComponent.Callback += SpawnEnemies;
		}

		private void SpawnEnemies()
		{
			var enemyEntity = _world.NewEntity();
			var id = _cms.GameConfig.EnemyConfig.ViewId;
			_world.GetPool<MonoReference<NavigationAgent>>().Add(enemyEntity).Reference = _pool.Get<NavigationAgent>();
			
			var follower = _pool.Get<NavigationFollower>(id);
			_world.GetPool<MonoReference<NavigationFollower>>().Add(enemyEntity).Reference = follower;
			_world.GetPool<EnemyTag>().Add(enemyEntity);
			_world.GetPool<MonoReference<HitableMono>>().Add(enemyEntity).Reference = follower.GetComponentInChildren<HitableMono>();
		}

		public void Update() { }

		public void Exit()
		{
			var filter = _world.Filter<EnemySpawner>().End();
			foreach (var entity in filter)
				_world.DelEntity(entity);
		}
	}
}