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
		public static Action UpdateSoundVolumeTitle;

		public static Action UpdateWebWindowOpacityTitle;

		public static Action UpdateIsAutoPauseWebState;

		public static Action DisposeRootFlowPanel;

		private FlowPanel _rootFlowPanel;

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
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			DisposeRootFlowPanel?.Invoke();
			ModuleSettings settings = WebPeeperModule.Instance.Settings;
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			_rootFlowPanel = val;
			DisposeRootFlowPanel = delegate
			{
				DisposeRootFlowPanel = null;
				((Control)_rootFlowPanel).Dispose();
			};
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)_settings).Where((SettingEntry s) => s.get_SessionDefined() && !_hiddenSettings.Contains(s.get_EntryKey())))
			{
				IView val2 = null;
				if (((object)item).Equals((object)settings.WebBgColor))
				{
					val2 = (IView)(object)new HexColorSettingView(settings.WebBgColor, ((Control)_rootFlowPanel).get_Width());
				}
				else if (((object)item).Equals((object)settings.CefVersion))
				{
					val2 = (IView)(object)new CefVersionSettingView(settings.CefVersion, ((Control)_rootFlowPanel).get_Width());
				}
				if (val2 == null && (val2 = SettingView.FromType(item, ((Control)_rootFlowPanel).get_Width())) == null)
				{
					continue;
				}
				ViewContainer val3 = new ViewContainer();
				((Container)val3).set_WidthSizingMode((SizingMode)2);
				((Container)val3).set_HeightSizingMode((SizingMode)1);
				((Control)val3).set_Parent((Container)(object)_rootFlowPanel);
				if (((object)item).Equals((object)settings.WebWindowOpacity))
				{
					FloatSettingView settingViewFloat = (FloatSettingView)(object)((val2 is FloatSettingView) ? val2 : null);
					if (settingViewFloat != null)
					{
						UpdateWebWindowOpacityTitle = delegate
						{
							((SettingView<float>)(object)settingViewFloat).set_DisplayName(((SettingEntry)settings.WebWindowOpacity).get_GetDisplayNameFunc()());
						};
						goto IL_0283;
					}
				}
				if (((object)item).Equals((object)settings.SoundVolume))
				{
					FloatSettingView settingViewFloat2 = (FloatSettingView)(object)((val2 is FloatSettingView) ? val2 : null);
					if (settingViewFloat2 != null)
					{
						UpdateSoundVolumeTitle = delegate
						{
							((SettingView<float>)(object)settingViewFloat2).set_DisplayName(((SettingEntry)settings.SoundVolume).get_GetDisplayNameFunc()());
						};
						goto IL_0283;
					}
				}
				if (((object)item).Equals((object)settings.IsAutoPauseWeb))
				{
					BoolSettingView settingViewBool = (BoolSettingView)(object)((val2 is BoolSettingView) ? val2 : null);
					if (settingViewBool != null)
					{
						UpdateIsAutoPauseWebState = delegate
						{
							((View<IPresenter>)(object)settingViewBool).get_Presenter().DoUpdateView();
						};
					}
				}
				goto IL_0283;
				IL_0283:
				val3.Show(val2);
			}
		}
	}
}
