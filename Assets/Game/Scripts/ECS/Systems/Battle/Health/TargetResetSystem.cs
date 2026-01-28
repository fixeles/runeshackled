using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle.Health
{
	public class TargetResetSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;
		
		[Inject]
		public TargetResetSystem(EcsWorld world)
		{
			_world = world;
			_filter = world.Filter<HasTargetComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var hitablePool = _world.GetPool<MonoReference<HitableMono>>();
				var hasTargetPool = _world.GetPool<HasTargetComponent>();
				var targetEntity = hasTargetPool.Get(entity).TargetEntity;

				if (!hitablePool.Has(targetEntity)) 
					hasTargetPool.Del(entity);
			}
		}
	}
}