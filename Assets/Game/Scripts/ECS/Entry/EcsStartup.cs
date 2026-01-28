using Common;
using Database;
using ECS.Entry.Builder;
using ECS.FSM;
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
				builder.RegisterInstance(_mainCamera);
				builder.RegisterInstance(_cms);
				builder.RegisterInstance(inputs);
				builder.Register<RuntimeData>(Lifetime.Singleton);
				builder.Register<GameProgress>(Lifetime.Singleton);
				builder.RegisterInstance<EcsWorld>(new());

				var stateMachine = new AppStateMachine(_appDefinition.Lifetime);
				builder.RegisterInstance(stateMachine).As<IAppStateMachine>();
				builder.RegisterBuildCallback(InitSystems);
			});
		}

		private void InitSystems(IObjectResolver resolver)
		{
			var world = resolver.Resolve<EcsWorld>();

			_systems = new EcsSystems(world);
#if UNITY_EDITOR
			_systems
				.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
				.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif

			SystemsBuilder[] builders =
			{
				new BaseSystems(resolver),
				new StateSystems(resolver),
				new BattleSystems(resolver),
				new UISystems(resolver),
				new FinalSystems(resolver)
			};
			foreach (var builder in builders)
				builder.Build(_systems);
			_systems.Init();
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