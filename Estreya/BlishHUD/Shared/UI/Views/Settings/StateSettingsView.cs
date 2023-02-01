using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.State;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views.Settings
{
	public class StateSettingsView : BaseSettingsView
	{
		private readonly Collection<ManagedState> _stateList;

		private readonly Func<Task> _reloadCalledAction;

		public StateSettingsView(Collection<ManagedState> stateList, Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, BitmapFont font = null, Func<Task> reloadCalledAction = null)
			: base(apiManager, iconState, translationState, font)
		{
			_stateList = stateList;
			_reloadCalledAction = reloadCalledAction;
		}

		protected override void BuildView(Panel parent)
		{
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			foreach (ManagedState state in _stateList)
			{
				if (state.GetType().BaseType.IsGenericType && state.GetType().BaseType.GetGenericTypeDefinition() == typeof(APIState<>))
				{
					bool loading = (bool)state.GetType().GetProperty("Loading").GetValue(state);
					bool finished = state.Running && !loading;
					string title = state.GetType().Name + " running & loaded:";
					string value = finished.ToString();
					Color? textColorValue = (finished ? Color.get_Green() : Color.get_Red());
					RenderLabel(parent, title, value, null, textColorValue);
				}
				else
				{
					string title2 = state.GetType().Name + " running:";
					string value2 = state.Running.ToString();
					Color? textColorValue = (state.Running ? Color.get_Green() : Color.get_Red());
					RenderLabel(parent, title2, value2, null, textColorValue);
				}
			}
			if (_reloadCalledAction != null)
			{
				RenderEmptyLine(parent);
				RenderEmptyLine(parent);
				RenderButtonAsync(parent, "Reload", _reloadCalledAction);
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
