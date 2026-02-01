using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using Enum;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle.Skills
{
	public class RaycastAttackSystem : IEcsRunSystem
	{
		private readonly EcsFilter _skillFilter;
		private readonly EcsFilter _initFilter;
		private readonly CMS _cms;
		private readonly EcsWorld _world;
		public AppState TargetState => AppState.Battle;

		[Inject]
		public RaycastAttackSystem(CMS cms, EcsWorld world)
		{
			_cms = cms;
			_world = world;
			_skillFilter = _world.Filter<SkillId>().Inc<UseRequest>().Exc<CooldownComponent>().End();
			_initFilter = _world.Filter<SkillId>().Inc<InitRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			TryInit();
			foreach (var skillEntity in _skillFilter)
			{
				var skillId = _world.GetPool<SkillId>().Get(skillEntity);
				if (skillId is not SkillId.melee_attack)
					continue;

				var ownerEntity = _world.GetPool<ChildComponent>().Get(skillEntity).OwnerEntity;
				var attackableMono = _world.GetPool<MonoReference<AttackableMono>>().Get(ownerEntity);
				var range = _world.GetPool<Range>().Get(skillEntity).Value;
				var attackPoint = attackableMono.Reference.AttackPoint;
				var mask = _world.GetPool<Maskable>().Get(skillEntity).LayerMask;
				var isRaycastHit = Physics.Raycast(attackPoint.position, attackPoint.forward, out var hit, range, mask);

				var cooldown = _cms.GameConfig.Skills.Get(skillId).Cooldown;
				if (cooldown > 0)
					_world.GetPool<CooldownComponent>().Add(skillEntity).TimeLeft = cooldown;

				if (!isRaycastHit)
					continue;

				if (!hit.collider.TryGetComponent<HitableMono>(out var hitableMono))
					continue;

				//todo: add hit request
				var requestEntity = _world.NewEntity();
				ref var request = ref _world.GetPool<DamageRequest>().Add(requestEntity);
				request.TargetEntity = hitableMono.Entity;
				ref var damage = ref _world.GetPool<Damage>().Get(skillEntity);
				request.DamageValue = damage.Value;
			}
		}

		private void TryInit()
		{
			foreach (var skillEntity in _initFilter)
			{
				var skillId = _world.GetPool<SkillId>().Get(skillEntity);
				var config = _cms.GameConfig.Skills.Get(skillId);

				_world.GetPool<Damage>().Add(skillEntity).Value = config.Damage;
				_world.GetPool<Range>().Add(skillEntity).Value = config.Range;
				_world.GetPool<SelectedSkill>().Add(skillEntity);

				ref var maskable = ref _world.GetPool<Maskable>().Add(skillEntity);

				var owner = _world.GetPool<ChildComponent>().Get(skillEntity).OwnerEntity;
				var isEnemy = _world.GetPool<EnemyTeam>().Has(owner);
				maskable.LayerMask = LayerMask.GetMask(isEnemy ? "Player" : "Enemy");


				if (isEnemy)
				{
					var sqrRange = config.Range - 1;
					sqrRange *= sqrRange;
					_world.GetPool<AiSkillUse>().Add(skillEntity).SqrDistanceToUse = sqrRange;
				}
			}
		}
	}
}