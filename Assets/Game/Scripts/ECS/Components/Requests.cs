using FPS.UI;
using UnityEngine;

namespace ECS.Components
{
	public struct OpenWindowRequest<T> where T : IWindow { }

	public struct CloseWindowRequest { }

	public struct ClickRequest { }

	public struct CreateRequest { }

	public struct CleanRequest { }

	public struct MoveRequest
	{
		public Vector3 Position;
	}

	public struct UseRequest { }
	public struct InitRequest { }
}