using ECS.Components;
using Enum;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Systems.Battle.Skills
{
	public class SkillsInitSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public SkillsInitSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<SkillId>().Inc<InitRequest>().End();
		}

		public void Run(IEcsSystems systems) { }
	}
}

