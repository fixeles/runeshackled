using FPS;
using UnityEngine;

namespace ECS.Mono
{
	public class LookTracker : MonoBehaviour
	{
		[field: SerializeField, Get] public Transform LookTransform { get; private set; }
	}
}