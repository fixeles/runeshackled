using ECS.Components;
using ECS.Extensions;
using ECS.Mono;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ECS.Systems.Common
{
	public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
	{
		private readonly EcsFilter _playerFilter;
		private readonly EcsFilter _activeSkillFilter;
		private readonly GameInputs _inputs;
		private readonly EcsWorld _world;

		[Inject]
		public PlayerInputSystem(GameInputs inputs, EcsWorld world)
		{
			_inputs = inputs;
			_world = world;
			_playerFilter = world.Filter<PlayerTag>().End();
			_activeSkillFilter = world.Filter<SelectedSkill>().Exc<PreparationComponent>().Exc<CooldownComponent>().End();
		}

		public void Init(IEcsSystems systems)
		{
			_inputs.Gameplay.Attack.performed += TryUseSkill;
		}

		public void Run(IEcsSystems systems)
		{
			TryMove();
		}

		private void TryUseSkill(InputAction.CallbackContext callbackContext)
		{
			foreach (var skillEntity in _activeSkillFilter)
			{
				_world.GetPool<PreparationComponent>().Add(skillEntity).TimeLeft += 0.5f; //todo: from config
			}
		}

		private void TryMove()
		{
			var inputAction = _inputs.Gameplay.Move;
			if (inputAction.phase is not (InputActionPhase.Performed or InputActionPhase.Started))
				return;

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

		public void Destroy(IEcsSystems systems)
		{
			_inputs.Gameplay.Attack.performed -= TryUseSkill;
		}
	}
}