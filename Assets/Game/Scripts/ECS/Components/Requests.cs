using Enum;
using FPS.UI;
using UnityEngine;

namespace ECS.Components
{
	public struct OpenWindowRequest<T> where T : IWindow { }

	public struct CloseWindowRequest { }

	public struct MoveRequest
	{
		public Vector3 Position;
	}

	public struct UseRequest { }
	public struct PreparationRequest { }
	public struct InitRequest { }
	public struct DeathRequest { }

	public struct DamageRequest
	{
		public int SourceEntity;
		public int TargetEntity;
		public float DamageValue;
		public DamageType DamageType;
	}
}