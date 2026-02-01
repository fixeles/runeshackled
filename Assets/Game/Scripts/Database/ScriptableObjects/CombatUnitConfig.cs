using Enum;
using UnityEngine;

namespace Database
{
	[CreateAssetMenu(fileName = "CombatUnitConfig", menuName = "FPS/Enemy", order = 0)]
	public class CombatUnitConfig : ScriptableObject
	{
		[field: SerializeField] public int Health { get; private set; } = 100;
		[field: SerializeField] public float AggroRadius { get; private set; } = 5;
		[field: SerializeField] public float RotationSpeed { get; private set; } = 5;
		[field: SerializeField] public SkillId[] Skills { get; private set; }
		
	}
}