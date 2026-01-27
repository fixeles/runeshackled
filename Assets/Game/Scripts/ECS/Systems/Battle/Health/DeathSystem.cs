using ECS.Components;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle.Health
{
	public class DeathSystem : IEcsRunSystem
	{
		private const float ClearDelay = 2f;
		private readonly EcsFilter _deathFilter;
		private readonly EcsWorld _world;

		[Inject]
		public DeathSystem(EcsWorld world)
		{
			_world = world;
			_deathFilter = _world.Filter<DeathRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _deathFilter)
			{
				ref var request = ref _world.GetPool<DeathRequest>().Get(entity);
				//remove "alive behaviour" components

				_world.CreateClearTimer(entity, ClearDelay);
			}
		}
	}
}