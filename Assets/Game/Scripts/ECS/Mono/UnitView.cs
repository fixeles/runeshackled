using FPS;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Mono
{
	public class UnitView : MonoBehaviour
	{
		[field: SerializeField, Get] public NavMeshAgent Agent { get; private set; }
	}
}