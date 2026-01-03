using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using LoreBridge.Models;
using LoreBridge.Views.SettingsView.Controls;
using Microsoft.Xna.Framework;

namespace LoreBridge.Views.SettingsView
{
	public class SettingsView : View
	{
		[CompilerGenerated]
		private Settings _003Csettings_003EP;

		private Panel _settingsPanel;

		private Policy _policyView;

		public SettingsView(Settings settings)
		{
			_003Csettings_003EP = settings;
			((View)this)._002Ector();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(buildPanel);
			((Panel)val).set_CanCollapse(false);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_OuterControlPadding(new Vector2(22f, 6f));
			val.set_ControlPadding(new Vector2(6f, 6f));
			((Container)val).set_HorizontalScrollOffset(16);
			_settingsPanel = (Panel)val;
			if (!_003Csettings_003EP.ContentUsePolicyConfirmation.get_Value())
			{
				_policyView = new Policy(_settingsPanel, _003Csettings_003EP);
				_003Csettings_003EP.ContentUsePolicyConfirmation.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PolicyValueChanged);
			}
			else
			{
				LoadSettings();
			}
			((View<IPresenter>)this).Build(buildPanel);
		}

		protected override void Unload()
		{
			((Control)_settingsPanel).Dispose();
			((View<IPresenter>)this).Unload();
		}

		private void LoadSettings()
		{
			new General(_settingsPanel, _003Csettings_003EP);
			new Area(_settingsPanel, _003Csettings_003EP);
			new Chat(_settingsPanel, _003Csettings_003EP);
		}

		private void PolicyValueChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				_policyView.Dispose();
				LoadSettings();
				_003Csettings_003EP.ContentUsePolicyConfirmation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PolicyValueChanged);
			}
		}
	}
}
