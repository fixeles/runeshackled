using System;
using ECS.Mono;
using UnityEngine;

namespace Database
{
	[Serializable]
	public struct Prefabs
	{
		[field: SerializeField] public UnitView PlayerCharacter { get; private set; }
	}
}