using ECS.Components;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ECS.Systems
{
	public class PlayerInputSystem : IEcsRunSystem
	{
		private EcsFilter _playerFilter;
		private readonly GameInputs _inputs;
		private readonly EcsWorld _world;

		[Inject]
		public PlayerInputSystem(GameInputs inputs, EcsWorld world)
		{
			_inputs = inputs;
			_world = world;
			_playerFilter = world.Filter<Player>().End();
		}

		public void Run(IEcsSystems systems)
		{
			var inputAction = _inputs.Gameplay.Move;
			Debug.LogError(inputAction.phase);
			if (inputAction.phase is not (InputActionPhase.Performed or InputActionPhase.Started))
				return;

			var input = inputAction.ReadValue<Vector2>();
			AddMoveRequest(input);
		}

		private void AddMoveRequest(Vector2 input)
		{
			foreach (var entity in _playerFilter)
			{
				var unitView = _world.GetPool<MonoReference<UnitView>>().Get(entity).View;
				ref var request = ref _world.GetPool<MoveRequest>().Add(entity);
				var moveDirection = new Vector3(input.x, 0, input.y).normalized;
				request.Position = unitView.transform.position + moveDirection;
			}
		}
	}
}