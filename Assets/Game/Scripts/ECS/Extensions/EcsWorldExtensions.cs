using ECS.Components;
using JetBrains.Lifetimes;
using Leopotam.EcsLite;
using UnityEngine;

namespace ECS.Extensions
{
	public static class EcsWorldExtensions
	{
		public static int CreateLifetimedEntity(this EcsWorld world, Lifetime lifetime)
		{
			var newEntity = world.NewEntity();
			var lifetimePool = world.GetPool<LifetimeComponent>();
			ref var lifetimeComponent = ref lifetimePool.Add(newEntity);

			lifetimeComponent.CreateNested(lifetime);
			lifetimeComponent.Lifetime.OnTermination(() => world.DelEntity(newEntity));

			return newEntity;
		}

		public static void DestroyLifetimedEntity(this EcsWorld world, int entity)
		{
			var lifetimePool = world.GetPool<LifetimeComponent>();
			lifetimePool.Get(entity).Terminate();
		}

		public static void CreateClearTimer(this EcsWorld world, int targetEntity, float clearDelay)
		{
			var lifetime = world.GetPool<LifetimeComponent>().Get(targetEntity).Lifetime;
			var timerEntity = world.CreateLifetimedEntity(lifetime);
			ref var timerComponent = ref world.GetPool<TimerComponent>().Add(timerEntity);
			timerComponent.TimeLeft = clearDelay;

			timerComponent.Callback += () => world.DestroyLifetimedEntity(targetEntity);
		}

		// private static void CreateTimer(this EcsWorld world, int targetEntity)
	}
}