using ECS.Systems;
using ECS.Systems.Battle;
using ECS.Systems.Battle.Health;
using ECS.Systems.Battle.Skills;
using ECS.Systems.Common;
using ECS.Systems.Look;
using ECS.Systems.Move;
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
				.Add(CreateSystem<CombatUnitInitSystem>())
				.Add(CreateSystem<CameraSystem>())
				.Add(CreateSystem<SkillsInitSystem>())
				
				.Add(CreateSystem<TargetResetSystem>())//before aggro
				.Add(CreateSystem<EnemyAggroSystem>())
				.Add(CreateSystem<TargetFollowSystem>())
				.Add(CreateSystem<DamageSystem>())
				.Add(CreateSystem<DeathSystem>())
				.Add(CreateSystem<RaycastAttackSystem>())
				.Add(CreateSystem<MoveSystem>())
				.Add(CreateSystem<LinkPositionSystem>())
				.Add(CreateSystem<PlayerLookCalculationSystem>())
				.Add(CreateSystem<EnemyLookSystem>())
				.Add(CreateSystem<LookRotationSystem>());
		}
	}
}