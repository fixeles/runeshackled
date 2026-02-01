using ECS.Components;
using ECS.Extensions;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ECS.Systems.Common
{
	public class InputMoveSystem : IEcsRunSystem
	{
		private readonly EcsFilter _playerFilter;
		private readonly GameInputs _inputs;
		private readonly EcsWorld _world;

		[Inject]
		public InputMoveSystem(GameInputs inputs, EcsWorld world)
		{
			_inputs = inputs;
			_world = world;
			_playerFilter = world.Filter<PlayerTeam>().Exc<AiMovable>().Exc<ImmobilizedComponent>().End();
		}

		public void Run(IEcsSystems systems)
		{
			TryMove();
		}

		private void TryMove()
		{
			var inputAction = _inputs.Gameplay.Move;
			if (inputAction.phase is not (InputActionPhase.Performed or InputActionPhase.Started))
			{
				foreach (var playerEntity in _playerFilter)
					_world.GetPool<ImmobilizedComponent>().Add(playerEntity);

				return;
			}

			var input = inputAction.ReadValue<Vector2>();
			AddMoveRequest(input);
		}

		private void AddMoveRequest(Vector2 input)
		{
			foreach (var entity in _playerFilter)
			{
				var unitView = _world.GetPool<MonoReference<NavigationAgent>>().Get(entity).Reference;
				var requestPool = _world.GetPool<MoveRequest>();
				ref var request = ref requestPool.GetOrAdd(entity);
				var moveDirection = new Vector3(input.x, 0, input.y).normalized;
				request.Position = unitView.transform.position + moveDirection;
			}
		}
	}
}