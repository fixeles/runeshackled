using ECS.Components;
using ECS.Extensions;
using Enum;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle.Skills
{
	public class AiSkillUseSystem : IEcsRunSystem
	{
		private readonly EcsFilter _filter;
		private readonly EcsWorld _world;

		[Inject]
		public AiSkillUseSystem(EcsWorld world)
		{
			_world = world;
			_filter = _world.Filter<SkillId>().Inc<AiSkillUse>().Exc<CooldownComponent>()
				.Exc<PreparationComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var skillEntity in _filter)
			{
				var ownerEntity = _world.GetPool<ChildComponent>().Get(skillEntity).OwnerEntity;
				var targetPool = _world.GetPool<HasTargetComponent>();
				if (!targetPool.Has(ownerEntity)) //todo: sleep unit if have not target
					continue;

				ref var targetComponent = ref targetPool.Get(ownerEntity);

				ref var ai = ref _world.GetPool<AiSkillUse>().Get(skillEntity);

				if (targetComponent.SqrDistanceToTarget < ai.SqrDistanceToUse)
				{
					_world.GetPool<PreparationRequest>().TryAdd(skillEntity);

					Debug.Log("prep");
				}
			}
		}
	}
}