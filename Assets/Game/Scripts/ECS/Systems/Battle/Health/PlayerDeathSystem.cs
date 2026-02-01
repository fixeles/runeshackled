using ECS.Components;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle.Health
{
	public class PlayerDeathSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public PlayerDeathSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<DeathRequest>().Inc<PlayerTeam>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				_world.GetPool<PlayerTeam>().Del(entity);
			}
		}
	}
}

