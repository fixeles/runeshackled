using System.Collections.Generic;
using ECS.FSM;
using FPS.UI;
using UI;
using UnityEngine;
using Utils;
using VContainer;

namespace ECS.Systems.UI
{
	public class HubUISystem : BaseUIWindowSystem<UIHubWindow>
	{
		private readonly IObjectResolver _resolver;
		private readonly IAppStateMachine _appStateMachine;

		[Inject]
		public HubUISystem(IObjectResolver resolver, IUIService uiService, IAppStateMachine appStateMachine) : base(uiService)
		{
			_resolver = resolver;
			_appStateMachine = appStateMachine;
		}

		protected override void OnShow(UIHubWindow window, int entity)
		{
			window.ButtonsProvider.Subscribe("Start", () => _appStateMachine.SetState(AppState.Battle));

			// CreateObserver(window.ArmyParent, categories.Army);
			// CreateObserver(window.CurrencyParent, categories.Currency);
			return;

			void CreateObserver(Transform parent, HashSet<string> category)
			{
				var inventoryObserver = _resolver.Resolve<InventoryObserver>();
				inventoryObserver.Init(window.gameObject.GetLifetime(), parent, category);
			}
		}
	}
}