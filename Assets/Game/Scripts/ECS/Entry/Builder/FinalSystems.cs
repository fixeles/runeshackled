using ECS.Systems;
using ECS.Systems.Common;
using ECS.Systems.Timer;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public class FinalSystems : SystemsBuilder
	{
		public FinalSystems(IObjectResolver resolver) : base(resolver) { }

		public override void Build(IEcsSystems systems)
		{
			systems
				.Add(CreateSystem<TimerUpdateSystem>())
				.Add(CreateSystem<SaveSystem>())
				.Add(CreateSystem<RemoveRequestsSystem>());
		}
	}
}