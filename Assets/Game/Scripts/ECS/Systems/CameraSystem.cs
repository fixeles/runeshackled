using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using VContainer;

namespace ECS.Systems
{
	public class CameraSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;
		private readonly CinemachineCamera _mainCamera;

		[Inject]
		public CameraSystem(EcsWorld world, CinemachineCamera mainCamera)
		{
			_world = world;
			_mainCamera = mainCamera;
			_filter = _world.Filter<PlayerTeam>().Inc<InitRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				_mainCamera.Follow = _world.GetPool<MonoReference<NavigationFollower>>()
					.Get(entity).Reference.transform;
			}
		}
	}
}