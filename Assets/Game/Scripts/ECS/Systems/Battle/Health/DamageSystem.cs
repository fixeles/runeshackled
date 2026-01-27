using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle.Health
{
	public class DamageSystem : IEcsRunSystem
	{
		private readonly EcsFilter _damageFilter;
		private readonly EcsWorld _world;

		[Inject]
		public DamageSystem(EcsWorld world)
		{
			_world = world;
			_damageFilter = _world.Filter<DamageRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _damageFilter)
			{
				ref var request = ref _world.GetPool<DamageRequest>().Get(entity);
				ref var targetHealth = ref _world.GetPool<HealthComponent>().Get(request.TargetEntity);
				targetHealth.CurrentHealth.Value -= request.DamageValue;
				Debug.LogError(targetHealth.CurrentHealth.Value);
			}
		}
	}
}