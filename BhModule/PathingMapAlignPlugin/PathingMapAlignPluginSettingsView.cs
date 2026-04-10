using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;

namespace BhModule.PathingMapAlignPlugin
{
	public class PathingMapAlignPluginSettingsView : View
	{
		public static Action DisposeRootFlowPanel;

		public static Action UpdateTitles;

		private FlowPanel rootFlowPanel;

		private readonly SettingCollection settings;

		public PathingMapAlignPluginSettingsView(SettingCollection settings)
		{
			this.settings = settings;
			((View)this)._002Ector();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			DisposeRootFlowPanel?.Invoke();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Control)buildPanel).get_Size());
			val.set_FlowDirection((ControlFlowDirection)0);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			rootFlowPanel = val;
			DisposeRootFlowPanel = delegate
			{
				UpdateTitles = null;
				DisposeRootFlowPanel = null;
				((Control)rootFlowPanel).Dispose();
			};
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)rootFlowPanel);
			val2.set_Text("Reset");
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				PathingMapAlignPluginModule.Instance.Settings.Reset();
			});
			foreach (SettingEntry item in ((IEnumerable<SettingEntry>)settings).Where((SettingEntry s) => s.get_SessionDefined()))
			{
				IView val3;
				if ((val3 = SettingView.FromType(item, ((Control)rootFlowPanel).get_Width())) == null)
				{
					continue;
				}
				ViewContainer val4 = new ViewContainer();
				((Container)val4).set_WidthSizingMode((SizingMode)2);
				((Container)val4).set_HeightSizingMode((SizingMode)1);
				((Control)val4).set_Parent((Container)(object)rootFlowPanel);
				SettingEntry<float> settingFloat = item as SettingEntry<float>;
				if (settingFloat != null)
				{
					FloatSettingView settingViewFloat = (FloatSettingView)(object)((val3 is FloatSettingView) ? val3 : null);
					if (settingViewFloat != null)
					{
						UpdateTitles = (Action)Delegate.Combine(UpdateTitles, (Action)delegate
						{
							((SettingView<float>)(object)settingViewFloat).set_DisplayName(((SettingEntry)settingFloat).get_GetDisplayNameFunc()());
						});
						goto IL_01e3;
					}
				}
				SettingEntry<int> settingInt = item as SettingEntry<int>;
				if (settingInt != null)
				{
					IntSettingView settingViewInt = (IntSettingView)(object)((val3 is IntSettingView) ? val3 : null);
					if (settingViewInt != null)
					{
						UpdateTitles = (Action)Delegate.Combine(UpdateTitles, (Action)delegate
						{
							((SettingView<int>)(object)settingViewInt).set_DisplayName(((SettingEntry)settingInt).get_GetDisplayNameFunc()());
						});
					}
				}
				goto IL_01e3;
				IL_01e3:
				val4.Show(val3);
			}
		}
	}
}
