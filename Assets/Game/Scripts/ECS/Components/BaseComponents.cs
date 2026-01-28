using System;
using ECS.Mono;
using JetBrains.Collections.Viewable;
using JetBrains.Lifetimes;
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

	public struct PositionComponent
	{
		public Vector3 Value;
	}

	public struct HealthComponent
	{
		public IViewableProperty<float> MaxHealth;
		public IViewableProperty<float> CurrentHealth;
	}

	public struct HasTargetComponent
	{
		public int TargetEntity;
	}
	
	public struct AggroComponent
	{
		public float AggroRadius;
	}
	
	public struct ChildComponent
	{
		public int OwnerEntity;
	}

	public struct LifetimeComponent
	{
		public Lifetime Lifetime => _definition.Lifetime;
		private LifetimeDefinition _definition;

		public void Terminate() => _definition.Terminate();
		public void CreateNested(Lifetime parentLifetime) => _definition = parentLifetime.CreateNested();
	}

	public struct LookDirection
	{
		public float RotationSpeed;
		public Quaternion TargetLocalRotation;
		public LookTracker Tracker;
	}
}