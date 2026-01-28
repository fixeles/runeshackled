using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Timer
{
	public class CooldownSystem : IEcsRunSystem
	{
		private readonly EcsFilter _cooldownFilter;
		private readonly EcsWorld _world;

		[Inject]
		public CooldownSystem(EcsWorld world)
		{
			_world = world;
			_cooldownFilter = _world.Filter<CooldownComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _cooldownFilter)
			{
				var pool = _world.GetPool<CooldownComponent>();
				ref var cooldownComponent = ref pool.Get(entity);
				cooldownComponent.TimeLeft -= Time.deltaTime;

				if (cooldownComponent.TimeLeft <= 0)
					pool.Del(entity);
			}
		}
	}
}