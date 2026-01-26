using System;
using UnityEngine;

namespace ECS.Mono
{
	public class HitableMono : MonoBehaviour
	{
		[field: SerializeField] public Transform AimPoint { get; private set; }
		[NonSerialized] public int Entity;
	}
}