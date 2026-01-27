using ECS.Components;
using ECS.Extensions;
using ECS.Mono;
using FPS.Pool;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace ECS.Systems.Battle.Health
{
	public class DeathSystem : IEcsRunSystem
	{
		private const float ClearDelay = 2f;
		private readonly EcsFilter _navigationFilter;
		private readonly EcsFilter _deathFilter;
		private readonly EcsFilter _hitableFilter;
		// private readonly EcsFilter _attackFilter;
		private readonly EcsWorld _world;
		private readonly IObjectPool _pool;

		[Inject]
		public DeathSystem(EcsWorld world, IObjectPool pool)
		{
			_world = world;
			_pool = pool;
			_navigationFilter = _world.Filter<DeathRequest>().Inc<MonoReference<NavigationAgent>>().End();
			_hitableFilter = _world.Filter<DeathRequest>().Inc<MonoReference<HitableMono>>().End();
			// _attackFilter = _world.Filter<DeathRequest>().Inc<AttackComponent>().End();
			_deathFilter = _world.Filter<DeathRequest>().End();
		}

		public void Run(IEcsSystems systems)
		{
			ClearNavigation();
			ClearHitable();
			// ClearAttack();
			StartClearTimer();
		}

		// private void ClearAttack()
		// {
		// 	foreach (var entity in _attackFilter)
		// 	{
		// 		var hitablePool = _world.GetPool<HealthComponent>();
		// 		hitablePool.Get(entity).Reference.SetActive(false);
		// 		hitablePool.Del(entity);
		// 	}
		// }

		private void ClearHitable()
		{
			foreach (var entity in _hitableFilter)
			{
				var hitablePool = _world.GetPool<MonoReference<HitableMono>>();
				hitablePool.Get(entity).Reference.SetActive(false);
				hitablePool.Del(entity);
			}
		}

		private void StartClearTimer()
		{
			foreach (var entity in _deathFilter)
				_world.CreateClearTimer(entity, ClearDelay);
		}

		private void ClearNavigation()
		{
			foreach (var entity in _navigationFilter)
			{
				var navigationAgentPool = _world.GetPool<MonoReference<NavigationAgent>>();
				var navigationAgent = navigationAgentPool.Get(entity).Reference;
				navigationAgent.Agent.isStopped = true;
				_pool.Return(navigationAgent);
				navigationAgentPool.Del(entity);

				var navigationFollowerPool = _world.GetPool<MonoReference<NavigationFollower>>();
				var navigationFollower = navigationFollowerPool.Get(entity).Reference;
				_pool.Return(navigationFollower);
				navigationFollowerPool.Del(entity);
			}
		}
	}
}