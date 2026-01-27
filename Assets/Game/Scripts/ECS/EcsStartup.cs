using System;
using Common;
using Database;
using ECS.FSM;
using ECS.Systems;
using ECS.Systems.Battle;
using ECS.Systems.Battle.Health;
using ECS.Systems.Battle.Skills;
using ECS.Systems.Common;
using ECS.Systems.Look;
using ECS.Systems.Move;
using ECS.Systems.Timer;
using ECS.Systems.UI;
using FPS;
using JetBrains.Lifetimes;
using Leopotam.EcsLite;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Lifetime = VContainer.Lifetime;

namespace ECS
{
	public class EcsStartup : MonoBehaviour
	{
		[SerializeField, Get] private LifetimeScope _scope;
		[SerializeField] private CMS _cms;
		[SerializeField] private CinemachineCamera _mainCamera;


		private readonly LifetimeDefinition _appDefinition = new();
		private EcsSystems _systems;

		public void Start()
		{
			var inputs = new GameInputs();
			inputs.Enable();
			_scope.CreateChild(builder =>
			{
				builder.RegisterInstance(_appDefinition.Lifetime);
				builder.RegisterInstance(_mainCamera);
				builder.RegisterInstance(_cms);
				builder.RegisterInstance(inputs);
				builder.Register<RuntimeData>(Lifetime.Singleton);
				builder.Register<TimerService>(Lifetime.Singleton);
				builder.Register<GameProgress>(Lifetime.Singleton);
				builder.Register<AppStateMachine>(Lifetime.Singleton).As<IAppStateMachine>();
				builder.RegisterInstance<EcsWorld>(new());
				builder.RegisterBuildCallback(InitSystems);
			});
		}

		private void InitSystems(IObjectResolver resolver)
		{
			var world = resolver.Resolve<EcsWorld>();

			_systems = new EcsSystems(world);
			_systems

				#region Debug

#if UNITY_EDITOR
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
				.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif

				#endregion

				.Add(CreateSystem<LinkPositionSystem>())

				#region States

				.Add(CreateSystem<AppInitState>())
				.Add(CreateSystem<HubState>())
				.Add(CreateSystem<HubBuilder>())
				.Add(CreateSystem<IAppStateMachine>())

				#endregion

				#region UI

				.Add(CreateSystem<CloseWindowSystem>())
				.Add(CreateSystem<HubUISystem>())

				#endregion

				#region Hub

				#endregion

				#region Battle

				.Add(CreateSystem<BuildMapSystem>())
				.Add(CreateSystem<PlayerSpawnSystem>())
				.Add(CreateSystem<PlayerInputSystem>())
				.Add(CreateSystem<MoveSystem>())
				.Add(CreateSystem<EnemySpawnSystem>())
				.Add(CreateSystem<DamageSystem>())

				#endregion

				#region Skills

				.Add(CreateSystem<RaycastAttackSystem>())

				#endregion

				.Add(CreateSystem<PlayerLookCalculationSystem>())
				.Add(CreateSystem<LookRotationSystem>())
				.Add(CreateSystem<TimerUpdateSystem>())
				.Add(CreateSystem<SaveSystem>())
				.Add(CreateSystem<RemoveRequestsSystem>())
				.Init();

			return;

			T CreateSystem<T>() where T : IEcsSystem
			{
				return resolver.TryResolve<T>(out var resolved)
					? resolved
					: Activator.CreateInstance<T>();
			}
		}


		private void Update()
		{
			_systems?.Run();
		}

		private void OnDestroy()
		{
			_appDefinition.Terminate();
			_systems?.Destroy();
			_systems?.GetWorld()?.Destroy();
			_systems = null;
		}
	}
}