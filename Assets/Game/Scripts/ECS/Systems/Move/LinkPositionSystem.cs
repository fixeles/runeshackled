using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Move
{
	public class LinkPositionSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter _ecsFilter;

		[Inject]
		public LinkPositionSystem(EcsWorld world)
		{
			_world = world;
			_ecsFilter = world
				.Filter<MonoReference<NavigationFollower>>()
				.Inc<MonoReference<NavigationAgent>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _ecsFilter)
			{
				var follower = _world.GetPool<MonoReference<NavigationFollower>>().Get(entity).Reference;
				var navigation = _world.GetPool<MonoReference<NavigationAgent>>().Get(entity).Reference;

				follower.CachedTransform.position = navigation.CachedTransform.position;
			}
		}
	}
}