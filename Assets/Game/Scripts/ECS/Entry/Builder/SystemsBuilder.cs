using System;
using Leopotam.EcsLite;
using VContainer;

namespace ECS.Entry.Builder
{
	public abstract class SystemsBuilder
	{
		private readonly IObjectResolver _resolver;
		public abstract void Build(IEcsSystems systems);

		public SystemsBuilder(IObjectResolver resolver)
		{
			_resolver = resolver;
		}

		protected T CreateSystem<T>() where T : IEcsSystem
		{
			return (T)(_resolver.TryResolve(typeof(T), out var resolved)
				? resolved
				: Activator.CreateInstance(typeof(T)));
		}
	}
}