using System;
using FPS.UI;
using UnityEngine;

namespace ECS
{
	public struct MonoReference<T> where T : MonoBehaviour
	{
		public T View;
	}

	public struct WindowComponent
	{
		public Type WindowType;
		public Action WindowCloseCallback;
	}

	public struct OpenWindowRequest<T> where T : IWindow { }

	public struct CloseWindowRequest { }

	public struct ClickRequest { }

	public struct CreateRequest { }

	public struct CleanRequest { }

	public struct TimerComponent
	{
		public Action Callback;
		public float LoopTime;
		public float TimeLeft;

		public bool Loop => LoopTime > 0;
	}
}