using Enum;
using FPS;
using UnityEngine;

namespace Database
{
	public class Skills : ScriptableObject
	{
		[SerializeField] private SerializableDictionary<SkillId, SkillConfig> _skills;

		public SkillConfig Get(SkillId id) => _skills[id];
	}
}