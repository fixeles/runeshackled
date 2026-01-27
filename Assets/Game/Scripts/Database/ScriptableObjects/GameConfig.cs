using Database.Enemies;
using UnityEngine;

namespace Database
{
	public class GameConfig : ScriptableObject
	{
		[field: SerializeField] public float EnemySpawnFrequency { get; private set; } = 1;
		[field: SerializeField] public EnemyConfig EnemyConfig { get; private set; }
		[field: SerializeField] public NavigationConfig PlayerNavigation { get; private set; }
	}
}