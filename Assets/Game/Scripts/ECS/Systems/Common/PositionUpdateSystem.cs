using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Common
{
	public class PositionUpdateSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter _navigationFilter;

		[Inject]
		public PositionUpdateSystem(EcsWorld world)
		{
			_world = world;
			_navigationFilter = _world.Filter<PositionComponent>().Inc<MonoReference<NavigationAgent>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _navigationFilter)
			{
				ref var positionComponent = ref _world.GetPool<PositionComponent>().Get(entity);
				positionComponent.Value = _world.GetPool<MonoReference<NavigationAgent>>()
					.Get(entity).Reference.CachedTransform.position;
			}
		}
	}
}