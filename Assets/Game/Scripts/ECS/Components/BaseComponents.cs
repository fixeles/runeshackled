using System;
using ECS.Mono;
using Enum;
using JetBrains.Collections.Viewable;
using UnityEngine;

namespace ECS.Components
{
	public struct MonoReference<T> where T : MonoBehaviour
	{
		public T Reference;
	}

	public struct WindowComponent
	{
		public Type WindowType;
		public Action WindowCloseCallback;
	}

	public struct TimerComponent
	{
		public Action Callback;
		public float LoopTime;
		public float TimeLeft;

		public bool Loop => LoopTime > 0;
	}

	public struct Movable { }

	public struct HealthComponent
	{
		public IViewableProperty<float> MaxHealth;
		public IViewableProperty<float> CurrentHealth;
	}

	public struct ChildComponent
	{
		public int OwnerEntity;
	}

	public struct LookDirection
	{
		public float RotationSpeed;
		public Quaternion TargetLocalRotation;
		public LookTracker Tracker;
	}
}