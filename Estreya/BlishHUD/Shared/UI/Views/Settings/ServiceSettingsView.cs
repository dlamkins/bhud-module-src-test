using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Services;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.Shared.UI.Views.Settings
{
	public class ServiceSettingsView : BaseSettingsView
	{
		private readonly Func<Task> _reloadCalledAction;

		private readonly IEnumerable<ManagedService> _stateList;

		public ServiceSettingsView(IEnumerable<ManagedService> stateList, Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, SettingEventService settingEventService, Func<Task> reloadCalledAction = null)
			: base(apiManager, iconService, translationService, settingEventService)
		{
			_stateList = stateList;
			_reloadCalledAction = reloadCalledAction;
		}

		protected override void BuildView(FlowPanel parent)
		{
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			foreach (ManagedService state in _stateList)
			{
				List<Type> baseTypes = new List<Type>();
				Type baseType = state.GetType().BaseType;
				while (baseType != null)
				{
					if (baseType.IsGenericType)
					{
						baseTypes.Add(baseType.GetGenericTypeDefinition());
					}
					else
					{
						baseTypes.Add(baseType);
					}
					baseType = baseType.BaseType;
				}
				if (state.GetType().BaseType.IsGenericType && baseTypes.Contains(typeof(APIService<>)))
				{
					bool loading = (bool)state.GetType().GetProperty("Loading").GetValue(state);
					bool finished = state.Running && !loading;
					string title = state.GetType().Name + " running & loaded:";
					string value = finished.ToString();
					Color? textColorValue = (finished ? Color.get_Green() : Color.get_Red());
					RenderLabel((Panel)(object)parent, title, value, null, textColorValue);
				}
				else
				{
					string title2 = state.GetType().Name + " running:";
					string value2 = state.Running.ToString();
					Color? textColorValue = (state.Running ? Color.get_Green() : Color.get_Red());
					RenderLabel((Panel)(object)parent, title2, value2, null, textColorValue);
				}
			}
			if (_reloadCalledAction != null)
			{
				RenderEmptyLine((Panel)(object)parent);
				RenderEmptyLine((Panel)(object)parent);
				RenderButtonAsync((Panel)(object)parent, "Reload", _reloadCalledAction);
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
