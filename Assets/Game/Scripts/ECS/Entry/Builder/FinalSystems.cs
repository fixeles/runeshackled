using ECS.Systems;
using ECS.Systems.Common;
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
				.Add(CreateSystem<SaveSystem>())
				.Add(CreateSystem<RemoveRequestsSystem>());
		}
	}
}