using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using Enum;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.Systems.Battle
{
	public class PlayerSpawnSystem : IStateEnter, IEcsSystem
	{
		private readonly EcsWorld _world;

		public AppState TargetState => AppState.Battle;


		[Inject]
		public PlayerSpawnSystem(EcsWorld world)
		{
			_world = world;
		}

		public void Enter(Lifetime lifetime)
		{
			var playerEntity = CreatePlayer(lifetime);
			AddAttackSkill(playerEntity);
		}

		private int CreatePlayer(Lifetime lifetime)
		{
			var playerEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<PlayerTag>().Add(playerEntity);
			_world.GetPool<UnitId>().Add(playerEntity) = UnitId.player;
			_world.GetPool<InitRequest>().Add(playerEntity);
			

			return playerEntity;
		}

		private void AddAttackSkill(int playerEntity)
		{
			var skillEntity = _world.CreateLifetimedEntity(playerEntity);

			_world.GetPool<ChildComponent>().Add(skillEntity).OwnerEntity = playerEntity;
			_world.GetPool<MeleeAttack>().Add(skillEntity);
			_world.GetPool<Damage>().Add(skillEntity).Value = 50;
			_world.GetPool<Range>().Add(skillEntity).Value = 3;
			_world.GetPool<Maskable>().Add(skillEntity).LayerMask = LayerMask.GetMask("Enemy");
			_world.GetPool<SelectedSkill>().Add(skillEntity);
		}
	}
}