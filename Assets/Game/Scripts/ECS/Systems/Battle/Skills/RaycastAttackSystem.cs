using Database;
using ECS.Components;
using ECS.FSM;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle.Skills
{
	public class RaycastAttackSystem : IStateUpdate, IEcsSystem
	{
		private readonly EcsFilter _skillFilter;
		private readonly CMS _cms;
		private readonly EcsWorld _world;
		public AppState TargetState => AppState.Battle;

		[Inject]
		public RaycastAttackSystem(CMS cms, EcsWorld world)
		{
			_cms = cms;
			_world = world;
			_skillFilter = _world.Filter<MeleeAttack>().Inc<UseRequest>().End();
		}

		public void Update()
		{
			foreach (var skillEntity in _skillFilter)
			{
				var ownerEntity = _world.GetPool<ChildComponent>().Get(skillEntity).OwnerEntity;
				var attackableMono = _world.GetPool<MonoReference<AttackableMono>>().Get(ownerEntity);
				var range = _world.GetPool<Range>().Get(skillEntity).Value;
				var attackPoint = attackableMono.Reference.AttackPoint;
				var mask = _world.GetPool<PhysicInfluence>().Get(skillEntity).LayerMask;
				var raycast = Physics.RaycastAll(attackPoint.position, attackPoint.forward, range, mask);

				if (raycast.Length == 0)
					continue;

				if (!raycast[0].collider.TryGetComponent<HitableMono>(out var hitableMono))
					continue;

				//todo: add hit request
				var requestEntity = _world.NewEntity();
				ref var request = ref _world.GetPool<DamageRequest>().Add(requestEntity);
				request.TargetEntity = hitableMono.Entity;
				ref var damage = ref _world.GetPool<Damage>().Get(skillEntity);
				request.DamageValue = damage.Value;
			}
		}
	}
}