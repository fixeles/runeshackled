using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Look
{
	public class EnemyLookSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public EnemyLookSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<EnemyTeam>().Inc<MonoReference<NavigationAgent>>().Inc<LookDirection>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				ref var lookDirection = ref _world.GetPool<LookDirection>().Get(entity);
				var agent = _world.GetPool<MonoReference<NavigationAgent>>().Get(entity).Reference;
				lookDirection.TargetRotation = agent.CachedTransform.rotation;
			}
		}
	}
}