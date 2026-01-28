using ECS.Systems;
using ECS.Systems.Common;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public class BaseSystems : SystemsBuilder
	{
		public BaseSystems(IObjectResolver resolver) : base(resolver) { }

		public override void Build(IEcsSystems systems)
		{
			systems
				.Add(CreateSystem<PlayerInputSystem>())
				.Add(CreateSystem<PositionUpdateSystem>());
		}
	}
}