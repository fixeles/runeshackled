using ECS.FSM;
using ECS.Systems;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public class StateSystems : SystemsBuilder
	{
		public StateSystems(IObjectResolver resolver) : base(resolver) { }

		public override void Build(IEcsSystems systems)
		{
			systems
				.Add(CreateSystem<AppInitState>())
				.Add(CreateSystem<HubState>())
				.Add(CreateSystem<HubBuilder>())
				.Add(CreateSystem<IAppStateMachine>());
		}
	}
}