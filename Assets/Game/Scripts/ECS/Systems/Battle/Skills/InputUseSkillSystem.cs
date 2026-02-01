using ECS.Components;
using Leopotam.EcsLite;
using UnityEngine.InputSystem;
using VContainer;

namespace ECS.Systems.Battle.Skills
{
	public class InputUseSkillSystem : IEcsInitSystem, IEcsDestroySystem
	{
		private readonly EcsFilter _filter;
		private readonly GameInputs _inputs;
		private readonly EcsWorld _world;

		[Inject]
		public InputUseSkillSystem(GameInputs inputs, EcsWorld world)
		{
			_inputs = inputs;
			_world = world;
			_filter = _world.Filter<SelectedSkill>().End();
		}


		public void Init(IEcsSystems systems)
		{
			_inputs.Gameplay.Attack.performed += TryUseSkill;
		}

		public void Destroy(IEcsSystems systems)
		{
			_inputs.Gameplay.Attack.performed -= TryUseSkill;
		}

		private void TryUseSkill(InputAction.CallbackContext callbackContext)
		{
			foreach (var skillEntity in _filter)
			{
				_world.GetPool<PreparationRequest>().Add(skillEntity);
			}
		}
	}
}