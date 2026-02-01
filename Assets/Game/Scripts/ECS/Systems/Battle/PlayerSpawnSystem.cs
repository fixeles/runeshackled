using ECS.Components;
using ECS.Extensions;
using ECS.FSM;
using Enum;
using Leopotam.EcsLite;
using VContainer;
using Lifetime = JetBrains.Lifetimes.Lifetime;

namespace ECS.Systems.Battle
{
	public class PlayerSpawnSystem : IStateEnter, IEcsSystem
	{
		private readonly EcsWorld _world;

		public AppState TargetState => AppState.Battle;


		[Inject]
		public PlayerSpawnSystem(EcsWorld world)
		{
			_world = world;
		}

		public void Enter(Lifetime lifetime)
		{
			CreatePlayer(lifetime);
		}

		private void CreatePlayer(Lifetime lifetime)
		{
			var playerEntity = _world.CreateLifetimedEntity(lifetime);
			_world.GetPool<PlayerTeam>().Add(playerEntity);
			_world.GetPool<UnitId>().Add(playerEntity) = UnitId.player;
			_world.GetPool<InitRequest>().Add(playerEntity);
		}
	}
}