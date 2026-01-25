using FPS.UI;

namespace ECS.Components
{
	public struct OpenWindowRequest<T> where T : IWindow { }

	public struct CloseWindowRequest { }

	public struct ClickRequest { }

	public struct CreateRequest { }

	public struct CleanRequest { }
}