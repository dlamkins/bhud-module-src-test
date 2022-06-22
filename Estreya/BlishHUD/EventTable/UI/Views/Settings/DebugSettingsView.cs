using System;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Estreya.BlishHUD.EventTable.State;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views.Settings
{
	public class DebugSettingsView : BaseSettingsView
	{
		public DebugSettingsView(ModuleSettings settings)
			: base(settings)
		{
		}

		protected override void BuildView(Panel parent)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			foreach (ManagedState state in EventTableModule.ModuleInstance.States)
			{
				string title = state.GetType().Name + " running:";
				string value = state.Running.ToString();
				Color? textColorValue = (state.Running ? Color.get_Green() : Color.get_Red());
				RenderLabel(parent, title, value, null, textColorValue);
			}
		}

		protected override Task<bool> InternalLoad(IProgress<string> progress)
		{
			return Task.FromResult(result: true);
		}
	}
}
