using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle
{
	public class TargetFollowSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly EcsFilter _filter;

		[Inject]
		public TargetFollowSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<HasTargetComponent>().Inc<MonoReference<NavigationAgent>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var targetEntity = _world.GetPool<HasTargetComponent>().Get(entity).TargetEntity;
				var targetPosition = _world.GetPool<PositionComponent>().Get(targetEntity).Value;

				_world.GetPool<MoveRequest>().Add(entity).Position = targetPosition;
			}
		}
	}
}