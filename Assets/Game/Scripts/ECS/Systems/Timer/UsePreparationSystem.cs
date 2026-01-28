using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Timer
{
	public class UsePreparationSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public UsePreparationSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<PreparationComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var pool = _world.GetPool<PreparationComponent>();
				ref var preparationComponent = ref pool.Get(entity);
				preparationComponent.TimeLeft -= Time.deltaTime;
				
				if (preparationComponent.TimeLeft > 0)
					continue;

				pool.Del(entity);
				_world.GetPool<UseRequest>().Add(entity);
			}
		}
	}
}