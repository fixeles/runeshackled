using ECS.Systems.UI;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public class UISystems : SystemsBuilder
	{
		public UISystems(IObjectResolver resolver) : base(resolver) { }

		public override void Build(IEcsSystems systems)
		{
			systems
				.Add(CreateSystem<CloseWindowSystem>())
				.Add(CreateSystem<HubUISystem>());
		}
	}
}