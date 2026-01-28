using ECS.Systems.Common;
using ECS.Systems.Timer;
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
				.Add(CreateSystem<PositionUpdateSystem>())
				.Add(CreateSystem<TimerUpdateSystem>())
				.Add(CreateSystem<UsePreparationSystem>())
				.Add(CreateSystem<CooldownSystem>());
		}
	}
}