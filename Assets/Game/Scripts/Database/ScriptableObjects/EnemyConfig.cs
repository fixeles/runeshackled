using UnityEngine;

namespace Database
{
	[CreateAssetMenu(fileName = "EnemyConfig", menuName = "FPS/Enemy", order = 0)]
	public class EnemyConfig : ScriptableObject
	{
		[field: SerializeField] public int Health { get; private set; }
		[field: SerializeField] public string ViewId { get; private set; }
		
	}
}