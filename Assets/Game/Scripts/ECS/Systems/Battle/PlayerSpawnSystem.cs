using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle
{
	public class PlayerSpawnSystem : IStateEnter, IEcsSystem
	{
		private readonly CMS _cms;
		private readonly EcsWorld _world;
		private readonly CinemachineCamera _mainCamera;

		public AppState TargetState => AppState.Battle;


		[Inject]
		public PlayerSpawnSystem(CMS cms, EcsWorld world, CinemachineCamera mainCamera)
		{
			_cms = cms;
			_world = world;
			_mainCamera = mainCamera;
		}

		public void Enter()
		{
			var playerEntity = _world.NewEntity();
			_world.GetPool<PlayerTag>().Add(playerEntity);
			var unitView = Object.Instantiate(_cms.Prefabs.PlayerCharacter);
			_world.GetPool<MonoReference<UnitView>>().Add(playerEntity).Reference = unitView;
			_world.GetPool<MonoReference<AttackableMono>>().Add(playerEntity).Reference = unitView.GetComponentInChildren<AttackableMono>();
			_mainCamera.Follow = unitView.transform;

			ref var movable = ref _world.GetPool<Movable>().Add(playerEntity);
			AddAttackSkill(playerEntity);
		}

		private void AddAttackSkill(int playerEntity)
		{
			var skillEntity = _world.NewEntity();
			_world.GetPool<ChildComponent>().Add(skillEntity).OwnerEntity = playerEntity;
			_world.GetPool<MeleeAttack>().Add(skillEntity);
			_world.GetPool<Damage>().Add(skillEntity).Value = 50;
			_world.GetPool<Range>().Add(skillEntity).Value = 3;
			_world.GetPool<PhysicInfluence>().Add(skillEntity).LayerMask = LayerMask.GetMask("Enemy");
			_world.GetPool<SelectedSkill>().Add(skillEntity);
		}
	}
}