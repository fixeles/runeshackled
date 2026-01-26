using ECS.Systems.Battle;
using UnityEngine;

namespace Database
{
	public class CMS : ScriptableObject
	{
		[field: SerializeField] public Sprites Sprites { get; private set; }
		[field: SerializeField] public Prefabs Prefabs { get; private set; }
		[field: SerializeField] public LevelView[] LevelViews { get; private set; }
		[field: SerializeField] public GameConfig GameConfig { get; private set; }
	}
}