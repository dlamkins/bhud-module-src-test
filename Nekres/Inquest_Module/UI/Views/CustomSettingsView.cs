using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Nekres.Inquest_Module.UI.Models;
using Nekres.Inquest_Module.UI.Presenters;

namespace Nekres.Inquest_Module.UI.Views
{
	public class CustomSettingsView : View<CustomSettingsPresenter>
	{
		private FlowPanel _settingFlowPanel;

		private bool _lockBounds = true;

		private ViewContainer _lastSettingContainer;

		public bool LockBounds
		{
			get
			{
				return _lockBounds;
			}
			set
			{
				if (_lockBounds != value)
				{
					_lockBounds = value;
					UpdateBoundsLocking(_lockBounds);
				}
			}
		}

		public event EventHandler<EventArgs> BrowserButtonClick;

		private void UpdateBoundsLocking(bool locked)
		{
			if (_settingFlowPanel != null)
			{
				((Panel)_settingFlowPanel).set_ShowBorder(!locked);
				((Panel)_settingFlowPanel).set_CanCollapse(!locked);
			}
		}

		public CustomSettingsView()
		{
		}

		public CustomSettingsView(CustomSettingsModel model)
		{
			base.WithPresenter(new CustomSettingsPresenter(this, model));
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height()));
			((Control)val).set_Location(new Point(0, 0));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_settingFlowPanel = val;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)((Presenter<CustomSettingsView, CustomSettingsModel>)base.get_Presenter()).get_Model().Settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val2 = new ViewContainer();
					((Container)val2).set_WidthSizingMode((SizingMode)2);
					((Container)val2).set_HeightSizingMode((SizingMode)1);
					((Control)val2).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val2;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_settingFlowPanel);
			((Control)val3).set_Size(new Point(250, 50));
			val3.set_Text("Policy: Macros and Macro Use");
			((Control)val3).set_BasicTooltipText(((Presenter<CustomSettingsView, CustomSettingsModel>)base.get_Presenter()).get_Model().PolicyMacrosAndMacroUse);
			val3.set_Icon(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("common/1441452")));
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)OnBrowserButtonClick);
		}

		private void OnBrowserButtonClick(object sender, MouseEventArgs e)
		{
			this.BrowserButtonClick?.Invoke(sender, (EventArgs)(object)e);
		}

		protected override void Unload()
		{
		}
	}
}
