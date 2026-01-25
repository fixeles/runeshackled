using Database;
using ECS.Components;
using ECS.FSM;
using Leopotam.EcsLite;

namespace ECS.Systems.Common
{
	public class CleanupSystem : IEcsRunSystem
	{
		private readonly EcsWorld _world;
		private readonly CMS _cms;
		private EcsFilter _filter;
		public AppState TargetState => AppState.Hub;

		public CleanupSystem(EcsWorld world, CMS cms)
		{
			_world = world;
			_cms = cms;
			_filter = world.Filter<CleanRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var entity in _filter)
			{
				
			}
		}
	}
}