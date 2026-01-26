using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Look
{
	public class LookRotationSystem : IEcsRunSystem
	{
		private readonly EcsFilter _lookDirectionFilter;
		private readonly EcsWorld _world;

		[Inject]
		public LookRotationSystem(EcsWorld world)
		{
			_world = world;
			_lookDirectionFilter = _world.Filter<LookDirection>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _lookDirectionFilter)
			{
				ref var lookComponent = ref _world.GetPool<LookDirection>().Get(entity);
				var softRotation = Quaternion.Lerp(
					lookComponent.Tracker.LookTransform.rotation,
					lookComponent.TargetLocalRotation,
					lookComponent.RotationSpeed * Time.deltaTime);
				lookComponent.Tracker.LookTransform.rotation = softRotation;
			}
		}
	}
}