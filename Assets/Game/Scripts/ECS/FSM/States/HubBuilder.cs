using Database;
using ECS.Components;
using ECS.Extensions;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.FSM
{
	public class HubBuilder : IEcsSystem, IStateEnter
	{
		private readonly EcsWorld _world;
		private readonly CMS _cms;
		public AppState TargetState => AppState.Hub;


		[Inject]
		public HubBuilder(EcsWorld world, CMS cms)
		{
			_world = world;
			_cms = cms;
		}

		public void Enter(Lifetime lifetime)
		{
			var hubEntity = _world.CreateLifetimedEntity(lifetime);
			var hubView = Object.Instantiate(_cms.Prefabs.HubView);
			_world.GetPool<MonoReference<HubView>>().Add(hubEntity).Reference = hubView;

			lifetime.OnTermination(() => Object.Destroy(hubView));
		}
	}
}