using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using Nekres.Regions_Of_Tyria.UI.Models;
using Nekres.Regions_Of_Tyria.UI.Presenters;

namespace Nekres.Regions_Of_Tyria.UI.Views
{
	public class CustomSettingsView : View<CustomSettingsPresenter>
	{
		private FlowPanel _settingFlowPanel;

		private bool _lockBounds = true;

		private ViewContainer _lastSettingContainer;

		private FlowPanel _socialFlowPanel;

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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(new Point(((Control)buildPanel).get_Width(), 78));
			((Control)val).set_Location(new Point(0, 0));
			val.set_FlowDirection((ControlFlowDirection)5);
			val.set_ControlPadding(new Vector2(27f, 2f));
			val.set_OuterControlPadding(new Vector2(27f, 2f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_Parent(buildPanel);
			_socialFlowPanel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)_socialFlowPanel);
			((Control)val2).set_Size(new Point(160, 46));
			val2.set_Text("Support Me on Ko-Fi");
			val2.set_Icon(AsyncTexture2D.op_Implicit(((Presenter<CustomSettingsView, CustomSettingsModel>)base.get_Presenter()).get_Model().GetSocialLogo(CustomSettingsModel.Social.KoFi)));
			val2.set_ResizeIcon(true);
			((Control)val2).set_BasicTooltipText(((Presenter<CustomSettingsView, CustomSettingsModel>)base.get_Presenter()).get_Model().GetSocialUrl(CustomSettingsModel.Social.KoFi));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)OnBrowserButtonClick);
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Size(new Point(((Control)buildPanel).get_Width(), ((Control)buildPanel).get_Height() - ((Control)_socialFlowPanel).get_Height()));
			((Control)val3).set_Location(new Point(0, ((Control)_socialFlowPanel).get_Height()));
			val3.set_FlowDirection((ControlFlowDirection)3);
			val3.set_ControlPadding(new Vector2(5f, 2f));
			val3.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Container)val3).set_AutoSizePadding(new Point(0, 15));
			((Control)val3).set_Parent(buildPanel);
			_settingFlowPanel = val3;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)((Presenter<CustomSettingsView, CustomSettingsModel>)base.get_Presenter()).get_Model().Settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView settingView;
				if ((settingView = SettingView.FromType(item, ((Control)_settingFlowPanel).get_Width())) != null)
				{
					ViewContainer val4 = new ViewContainer();
					((Container)val4).set_WidthSizingMode((SizingMode)2);
					((Container)val4).set_HeightSizingMode((SizingMode)1);
					((Control)val4).set_Parent((Container)(object)_settingFlowPanel);
					_lastSettingContainer = val4;
					_lastSettingContainer.Show(settingView);
					SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
					if (subSettingsView != null)
					{
						subSettingsView.set_LockBounds(false);
					}
				}
			}
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
