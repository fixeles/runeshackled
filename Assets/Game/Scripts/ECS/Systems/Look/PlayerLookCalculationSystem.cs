using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Systems.Look
{
	public class PlayerLookCalculationSystem : IEcsRunSystem, IEcsInitSystem
	{
		private EcsFilter _playerFilter;
		private Camera _camera;


		public void Init(IEcsSystems systems)
		{
			_camera = Camera.main;
			_playerFilter = systems.GetWorld().Filter<PlayerTeam>().Inc<LookDirection>().End();
		}

		public void Run(IEcsSystems systems)
		{
			var ray = _camera.ScreenPointToRay(Input.mousePosition);
			var groundPlane = new Plane(Vector3.up, Vector3.zero);
			if (!groundPlane.Raycast(ray, out float distance))
				return;

			foreach (var entity in _playerFilter)
			{
				ref var lookComponent = ref systems.GetWorld().GetPool<LookDirection>().Get(entity);

				var worldPosition = ray.GetPoint(distance);
				worldPosition.y = lookComponent.Tracker.LookTransform.position.y;

				var lookVector = worldPosition - lookComponent.Tracker.LookTransform.position;
				lookComponent.TargetRotation = Quaternion.LookRotation(lookVector);
			}
		}
	}
}