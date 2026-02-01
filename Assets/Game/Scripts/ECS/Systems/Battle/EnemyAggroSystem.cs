using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle
{
	public class EnemyAggroSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter _aggroFilter;
		private readonly EcsFilter _playerFilter;

		[Inject]
		public EnemyAggroSystem(EcsWorld world)
		{
			_world = world;
			_aggroFilter = _world.Filter<EnemyTeam>().Inc<AggroComponent>().Exc<HasTargetComponent>().End();
			_playerFilter = _world.Filter<PlayerTeam>().Inc<MonoReference<HitableMono>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var enemyEntity in _aggroFilter)
			{
				var enemyPosition = _world.GetPool<PositionComponent>().Get(enemyEntity).Value;
				var sqrAggroRadius = _world.GetPool<AggroComponent>().Get(enemyEntity).AggroRadius;
				sqrAggroRadius *= sqrAggroRadius;

				foreach (var playerEntity in _playerFilter)
				{
					var playerPosition = _world.GetPool<PositionComponent>().Get(playerEntity).Value;
					var sqrDistance = (playerPosition - enemyPosition).sqrMagnitude;
					var isInAggroRadius = sqrDistance < sqrAggroRadius;

					if (isInAggroRadius)
						_world.GetPool<HasTargetComponent>().Add(enemyEntity).TargetEntity = playerEntity;
				}
			}
		}
	}
}