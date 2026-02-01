using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Move
{
	public class ImmobilizationSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public ImmobilizationSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<ImmobilizedComponent>().Inc<MonoReference<NavigationAgent>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var agent = _world.GetPool<MonoReference<NavigationAgent>>().Get(entity).Reference;
				agent.Agent.isStopped = true;
				
				var pool = _world.GetPool<ImmobilizedComponent>();
				ref var immobilized = ref pool.Get(entity);
				immobilized.TimeLeft -= Time.deltaTime;
				if (immobilized.TimeLeft > 0)
					continue;
				
				pool.Del(entity);
			}
		}
	}
}