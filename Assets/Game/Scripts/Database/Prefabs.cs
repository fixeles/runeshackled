using System;
using ECS.Mono;
using UnityEngine;

namespace Database
{
	[Serializable]
	public struct Prefabs
	{
		[field: SerializeField] public NavigationFollower PlayerCharacter { get; private set; }
	}
}