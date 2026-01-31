using UnityEngine;

namespace Database
{
	[CreateAssetMenu(fileName = "SkillConfig", menuName = "FPS/Skill Config")]
	public class SkillConfig : ScriptableObject
	{
		[field: SerializeField, Min(0)] public int Damage { get; private set; }
		[field: SerializeField, Min(0)] public float UseRange { get; private set; }
		[field: SerializeField, Min(0)] public float Cooldown { get; private set; }
		
	}
}