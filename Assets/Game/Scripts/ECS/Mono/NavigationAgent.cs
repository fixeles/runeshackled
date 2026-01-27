using FPS;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Mono
{
	public class NavigationAgent : MonoBehaviour
	{
		[field: SerializeField, Get] public NavMeshAgent Agent { get; private set; }
		[field: SerializeField, Get] public Transform CachedTransform { get; private set; }
	}
}