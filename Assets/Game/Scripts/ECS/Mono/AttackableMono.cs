using UnityEngine;

namespace ECS.Mono
{
	public class AttackableMono : MonoBehaviour
	{
		[field: SerializeField] public Transform AttackPoint { get; private set; }
	}
}