using ECS.Systems;
using ECS.Systems.Battle;
using ECS.Systems.Battle.Health;
using ECS.Systems.Battle.Skills;
using ECS.Systems.Look;
using ECS.Systems.Move;
using ECS.Systems.Timer;
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

				.Add(CreateSystem<TargetResetSystem>()) //before aggro
				.Add(CreateSystem<EnemyAggroSystem>())
				.Add(CreateSystem<TargetFollowSystem>())
				.Add(CreateSystem<TargetDistanceUpdateSystem>())
				
				.Add(CreateSystem<InputUseSkillSystem>())
				.Add(CreateSystem<AiSkillUseSystem>())
				.Add(CreateSystem<UsePreparationSystem>())
				.Add(CreateSystem<RaycastAttackSystem>())
				
				.Add(CreateSystem<ImmobilizationSystem>())
				.Add(CreateSystem<MoveSystem>())
				.Add(CreateSystem<LinkPositionSystem>())
				
				.Add(CreateSystem<PlayerLookCalculationSystem>())
				.Add(CreateSystem<EnemyLookSystem>())
				.Add(CreateSystem<LookRotationSystem>())

				.Add(CreateSystem<DamageSystem>())
				.Add(CreateSystem<DeathSystem>())
				.Add(CreateSystem<PlayerDeathSystem>());
		}
	}
}