using ECS.Components;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Timer
{
	public class TimerService
	{
		private readonly EcsWorld _world;

		public bool HasTimer(int entity) => Pool.Has(entity);
		private EcsPool<TimerComponent> Pool => _world.GetPool<TimerComponent>();

		[Inject]
		public TimerService(EcsWorld world)
		{
			_world = world;
		}
	}
}