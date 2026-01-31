using UnityEngine;

namespace Database
{
	public class GameConfig : ScriptableObject
	{
		[field: SerializeField] public float EnemySpawnFrequency { get; private set; } = 1;
		[field: SerializeField] public CombatUnits CombatUnits { get; private set; }
		[field: SerializeField] public Skills Skills { get; private set; }
	}
}