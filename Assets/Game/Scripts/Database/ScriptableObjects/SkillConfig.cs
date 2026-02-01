using UnityEngine;

namespace Database
{
	[CreateAssetMenu(fileName = "SkillConfig", menuName = "FPS/Skill Config")]
	public class SkillConfig : ScriptableObject
	{
		[field: SerializeField, Min(0)] public int Damage { get; private set; }
		[field: SerializeField, Min(0)] public float Range { get; private set; }
		[field: SerializeField, Min(0)] public float Cooldown { get; private set; }
		[field: SerializeField, Min(0)] public float CastTime { get; private set; }
	}
}