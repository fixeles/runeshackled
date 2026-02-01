using Database;
using ECS.Components;
using ECS.Extensions;
using Enum;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Timer
{
	public class UsePreparationSystem : IEcsRunSystem
	{
		private readonly EcsFilter _preparationFilter;
		private readonly EcsFilter _requestsFilter;
		private readonly EcsWorld _world;
		private readonly CMS _cms;

		[Inject]
		public UsePreparationSystem(EcsWorld world, CMS cms)
		{
			_world = world;
			_cms = cms;
			_preparationFilter = _world.Filter<PreparationComponent>().End();
			_requestsFilter = _world.Filter<PreparationRequest>().Exc<PreparationComponent>().Exc<CooldownComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			HandleRequests();

			foreach (var entity in _preparationFilter)
			{
				var pool = _world.GetPool<PreparationComponent>();
				ref var preparationComponent = ref pool.Get(entity);
				preparationComponent.TimeLeft -= Time.deltaTime;

				if (preparationComponent.TimeLeft > 0)
					continue;

				pool.Del(entity);
				_world.GetPool<UseRequest>().Add(entity);
			}
		}

		private void HandleRequests()
		{
			foreach (var entity in _requestsFilter)
			{
				var skillId = _world.GetPool<SkillId>().Get(entity);
				var skillConfig = _cms.GameConfig.Skills.Get(skillId);
				var castTime = skillConfig.CastTime;
				_world.GetPool<PreparationComponent>().Add(entity).TimeLeft = castTime;

				if (!skillConfig.CanMoveWhileCast)
				{
					var owner = _world.GetPool<ChildComponent>().Get(entity).OwnerEntity;
					_world.GetPool<ImmobilizedComponent>().GetOrAdd(owner).TimeLeft = castTime;
				}

				//todo: start animation? or another system?
			}
		}
	}
}