using Leopotam.EcsLite;

namespace ECS.Extensions
{
	public static class PoolExtension
	{
		public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if (pool.Has(entity))
				return ref pool.Get(entity);

			return ref pool.Add(entity);
		}

		public static bool TryGet<T>(this EcsPool<T> pool, int entity, out T component) where T : struct
		{
			var hasComponent = pool.Has(entity);
			component = hasComponent ? pool.Get(entity) : default;
			return hasComponent;
		}

		public static bool TryAdd<T>(this EcsPool<T> pool, int entity) where T : struct
		{
			if (pool.Has(entity))
				return false;

			pool.Add(entity);
			return true;
		}

		public static bool TryAdd<T>(this EcsPool<T> pool, int entity, out T component) where T : struct
		{
			var hasComponent = pool.Has(entity);
			component = !hasComponent ? pool.Add(entity) : default;
			return !hasComponent;
		}
	}
}