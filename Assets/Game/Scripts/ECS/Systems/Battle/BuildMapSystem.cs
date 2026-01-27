using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.Systems.Battle
{
	public class BuildMapSystem : IStateEnter, IEcsSystem
	{
		private readonly CMS _cms;
		private readonly EcsWorld _world;

		[Inject]
		public BuildMapSystem(CMS cms, EcsWorld world)
		{
			_cms = cms;
			_world = world;
		}

		public AppState TargetState => AppState.Battle;

		public void Enter(Lifetime lifetime)
		{
			var mapEntity = _world.CreateLifetimedEntity(lifetime);
			var monoPool = _world.GetPool<MonoReference<LevelView>>();
			ref var component = ref monoPool.Add(mapEntity);
			var mapInstance = Object.Instantiate(_cms.LevelViews[0]);
			component.Reference = mapInstance;

			lifetime.OnTermination(() => Object.Destroy(mapInstance));
		}
	}
}