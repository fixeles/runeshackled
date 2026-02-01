using ECS.Components;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle
{
	public class TargetDistanceUpdateSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public TargetDistanceUpdateSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<HasTargetComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				ref var hasTargetComponent = ref _world.GetPool<HasTargetComponent>().Get(entity);
				var ownerPosition = _world.GetPool<PositionComponent>().Get(entity).Value;
				var targetPosition = _world.GetPool<PositionComponent>().Get(hasTargetComponent.TargetEntity).Value;
				hasTargetComponent.SqrDistanceToTarget = (targetPosition - ownerPosition).sqrMagnitude;
			}
		}
	}
}