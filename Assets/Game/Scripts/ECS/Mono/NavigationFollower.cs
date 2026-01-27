using UnityEngine;

namespace ECS.Mono
{
	public class NavigationFollower : MonoBehaviour
	{
		[field: SerializeField] public Transform CachedTransform { get; private set; }
	}
}