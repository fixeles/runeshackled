using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems
{
	public class MoveSystem : IEcsRunSystem, IEcsInitSystem
	{
		private readonly EcsWorld _world;
		private EcsFilter _filter;

		[Inject]
		public MoveSystem(EcsWorld world)
		{
			_world = world;
		}

		public void Init(IEcsSystems systems)
		{
			_filter = _world.Filter<MoveRequest>().Inc<MonoReference<UnitView>>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				var requestPosition = _world.GetPool<MoveRequest>().Get(entity).Position;
				var agent = _world.GetPool<MonoReference<UnitView>>().Get(entity).Reference.Agent;
				agent.SetDestination(requestPosition);
			}
		}
	}
}