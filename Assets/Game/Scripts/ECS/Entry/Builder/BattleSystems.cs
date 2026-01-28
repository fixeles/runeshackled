using ECS.Systems;
using ECS.Systems.Battle;
using ECS.Systems.Battle.Health;
using ECS.Systems.Battle.Skills;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public class BattleSystems : SystemsBuilder
	{
		public BattleSystems(IObjectResolver resolver) : base(resolver) { }

		public override void Build(IEcsSystems systems)
		{
			systems
				.Add(CreateSystem<BuildMapSystem>())
				.Add(CreateSystem<PlayerSpawnSystem>())
				.Add(CreateSystem<EnemySpawnSystem>())
				.Add(CreateSystem<DamageSystem>())
				.Add(CreateSystem<DeathSystem>())
				.Add(CreateSystem<RaycastAttackSystem>());
		}
	}
}