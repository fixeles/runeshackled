using System;
using UnityEngine;

namespace ECS.Mono
{
	public class HitableMono : MonoBehaviour
	{
		[field: SerializeField] public Transform AimPoint { get; private set; }
		[SerializeField] private Collider _collider;


		[NonSerialized] public int Entity;

		public void SetActive(bool active) => _collider.enabled = active;
	}
}