using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace BhModule.WebPeeper
{
	internal class WebPeeperSettingsView : View
	{
		public static Action UpdateWebWindowOpacityTitle;

		public static Action UpdateIsAutoPauseWebState;

		private FlowPanel _rootflowPanel;

		private readonly SettingCollection _settings;

		private readonly string[] _hiddenSettings;

		public WebPeeperSettingsView(SettingCollection settings)
		{
			_settings = settings;
			_hiddenSettings = new string[1] { "CefErrorVersion" };
			((View)this)._002Ector();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_rootflowPanel = val;
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)_settings).Where((SettingEntry s) => s.get_SessionDefined() && !_hiddenSettings.Contains(s.get_EntryKey())))
			{
				IView val2 = null;
				if (item.get_EntryKey() == "WebBgColor")
				{
					SettingEntry<string> val3 = item as SettingEntry<string>;
					if (val3 != null)
					{
						val2 = (IView)(object)new HexColorSettingView(val3, ((Control)_rootflowPanel).get_Width());
						goto IL_00f7;
					}
				}
				if (item.get_EntryKey() == "CefVersion")
				{
					SettingEntry<CefAvailableVersion> val4 = item as SettingEntry<CefAvailableVersion>;
					if (val4 != null)
					{
						val2 = (IView)(object)new CefVersionSettingView(val4, ((Control)_rootflowPanel).get_Width());
					}
				}
				goto IL_00f7;
				IL_01de:
				_003F val5;
				((ViewContainer)val5).Show(val2);
				continue;
				IL_00f7:
				if (val2 == null && (val2 = SettingView.FromType(item, ((Control)_rootflowPanel).get_Width())) == null)
				{
					continue;
				}
				val5 = new ViewContainer();
				((Container)val5).set_WidthSizingMode((SizingMode)2);
				((Container)val5).set_HeightSizingMode((SizingMode)1);
				((Control)val5).set_Parent((Container)(object)_rootflowPanel);
				SettingEntry<float> settingFloat = item as SettingEntry<float>;
				if (settingFloat != null)
				{
					FloatSettingView settingViewFloat = (FloatSettingView)(object)((val2 is FloatSettingView) ? val2 : null);
					if (settingViewFloat != null && ((SettingEntry)settingFloat).get_EntryKey() == "WebWindowOpacity")
					{
						UpdateWebWindowOpacityTitle = delegate
						{
							((SettingView<float>)(object)settingViewFloat).set_DisplayName(((SettingEntry)settingFloat).get_GetDisplayNameFunc()());
						};
						goto IL_01de;
					}
				}
				SettingEntry<bool> val6 = item as SettingEntry<bool>;
				if (val6 != null)
				{
					BoolSettingView settingViewBool = (BoolSettingView)(object)((val2 is BoolSettingView) ? val2 : null);
					if (settingViewBool != null && ((SettingEntry)val6).get_EntryKey() == "IsAutoPauseWeb")
					{
						UpdateIsAutoPauseWebState = delegate
						{
							((View<IPresenter>)(object)settingViewBool).get_Presenter().DoUpdateView();
						};
					}
				}
				goto IL_01de;
			}
		}
	}
}
