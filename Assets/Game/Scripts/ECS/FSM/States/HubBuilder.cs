using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.FSM
{
	public class HubBuilder : IEcsSystem, IStateEnter, IStateExit
	{
		private readonly EcsWorld _world;
		private GameObject _hubGo;
		public AppState TargetState => AppState.Hub;
		
		
		[Inject]
		public HubBuilder(EcsWorld world)
		{
			_world = world;
		}

		public void Enter(Lifetime lifetime)
		{
			SpawnHubAsync().Forget();
		}

		public void Exit()
		{
			Object.Destroy(_hubGo);
		}

		private async UniTaskVoid SpawnHubAsync()
		{
			_hubGo = await Addressables.InstantiateAsync("Hub");
			//deserialize buildings
		}
	}
}