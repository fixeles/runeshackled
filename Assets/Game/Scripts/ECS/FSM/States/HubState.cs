using Leopotam.EcsLite;
using UI;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.FSM
{
	public class HubState : IEcsSystem, IStateEnter, IStateExit
	{
		private readonly EcsWorld _world;

		[Inject]
		public HubState(EcsWorld world)
		{
			_world = world;
		}

		public AppState TargetState => AppState.Hub;

		public void Enter(Lifetime lifetime)
		{
			UIHelper.ShowWindow<UIHubWindow>(_world);
		}

		public void Exit()
		{
			UIHelper.HideWindow<UIHubWindow>(_world);
		}
	}
}