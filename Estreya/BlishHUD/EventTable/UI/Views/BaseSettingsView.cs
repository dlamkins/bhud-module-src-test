using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public abstract class BaseSettingsView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		protected ModuleSettings ModuleSettings { get; set; }

		private static IEnumerable<Color> Colors { get; set; }

		private static Panel ColorPickerPanel { get; set; }

		private static string SelectedColorSetting { get; set; }

		private static ColorPicker ColorPicker { get; set; }

		public BaseSettingsView(ModuleSettings settings)
			: this()
		{
			ModuleSettings = settings;
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			if (Colors == null)
			{
				progress.Report("Loading Colors...");
				try
				{
					Colors = (IEnumerable<Color>)(await ((IAllExpandableClient<Color>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).AllAsync(default(CancellationToken)));
				}
				catch (Exception ex)
				{
					Logger.Warn("Could not load gw2 colors: " + ex.Message);
				}
			}
			if (ColorPicker == null)
			{
				progress.Report("Loading ColorPicker...");
				Panel val = new Panel();
				((Control)val).set_Location(new Point(10, 10));
				((Container)val).set_WidthSizingMode((SizingMode)1);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Control)val).set_Visible(false);
				((Control)val).set_ZIndex(int.MaxValue);
				((Control)val).set_BackgroundColor(Color.get_Black());
				val.set_ShowBorder(false);
				ColorPickerPanel = val;
				ColorPicker val2 = new ColorPicker();
				((Control)val2).set_Location(new Point(10, 10));
				((Control)val2).set_Parent((Container)(object)ColorPickerPanel);
				((Control)val2).set_Visible(true);
				ColorPicker = val2;
				progress.Report("Adding Colors to ColorPicker...");
				if (Colors != null)
				{
					foreach (Color color2 in Colors.OrderBy((Color color) => color.get_Categories().FirstOrDefault()))
					{
						ColorPicker.get_Colors().Add(color2);
					}
				}
			}
			progress.Report("");
			return await InternalLoad(progress);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			Rectangle bounds = buildPanel.get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(10f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			FlowPanel parentPanel = val;
			InternalBuild(parentPanel);
		}

		protected abstract Task<bool> InternalLoad(IProgress<string> progress);

		protected abstract void InternalBuild(FlowPanel parent);

		protected void RenderEmptyLine(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)parent);
			val.Show((IView)(object)new EmptySettingsLineView(25));
		}

		protected void RenderSetting(Panel parent, SettingEntry setting)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			IView settingView = SettingView.FromType(setting, ((Control)parent).get_Width());
			if (settingView != null)
			{
				ViewContainer val = new ViewContainer();
				((Container)val).set_WidthSizingMode((SizingMode)2);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Control)val).set_Parent((Container)(object)parent);
				val.Show(settingView);
				SettingsView subSettingsView = (SettingsView)(object)((settingView is SettingsView) ? settingView : null);
				if (subSettingsView != null)
				{
					subSettingsView.set_LockBounds(false);
				}
			}
		}

		protected void RenderButton(Panel parent, string text, Action action, Func<bool> disabledCallback = null)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			ViewContainer val = new ViewContainer();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)parent);
			ViewContainer settingContainer = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)settingContainer);
			val2.set_Text(text);
			((Control)val2).set_Width((int)EventTableModule.ModuleInstance.Font.MeasureString(text).Width);
			((Control)val2).set_Enabled(disabledCallback == null || !disabledCallback());
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				action();
			});
		}

		protected void RenderColorSetting(Panel parent, SettingEntry<Color> setting)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			ViewContainer val = new ViewContainer();
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)parent);
			ViewContainer settingContainer = val;
			Label val2 = new Label();
			((Control)val2).set_Location(new Point(5, 0));
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)settingContainer);
			val2.set_Text(((SettingEntry)setting).get_DisplayName());
			Label label = val2;
			ColorBox val3 = new ColorBox();
			((Control)val3).set_Location(new Point(Math.Max(185, ((Control)label).get_Left() + 10), 0));
			((Control)val3).set_Parent((Container)(object)settingContainer);
			val3.set_Color(setting.get_Value());
			ColorBox colorBox = val3;
			((Control)colorBox).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Expected O, but got Unknown
				((Control)ColorPickerPanel).set_Parent(((Control)parent).get_Parent());
				((Control)ColorPickerPanel).set_Size(new Point(((Control)parent).get_Width() - 30, 850));
				((Control)ColorPicker).set_Size(new Point(((Control)ColorPickerPanel).get_Size().X - 20, ((Control)ColorPickerPanel).get_Size().Y - 20));
				Color val4 = new Color();
				val4.set_Id(int.MaxValue);
				val4.set_Name("temp");
				Color item = val4;
				((Control)ColorPicker).RecalculateLayout();
				ColorPicker.get_Colors().Add(item);
				ColorPicker.get_Colors().Remove(item);
				((Control)ColorPickerPanel).set_Visible(!((Control)ColorPickerPanel).get_Visible());
				SelectedColorSetting = ((SettingEntry)setting).get_EntryKey();
			});
			ColorPicker.add_SelectedColorChanged((EventHandler<EventArgs>)delegate
			{
				if (!(SelectedColorSetting != ((SettingEntry)setting).get_EntryKey()))
				{
					setting.set_Value(ColorPicker.get_SelectedColor());
					((Control)ColorPickerPanel).set_Visible(false);
					colorBox.set_Color(ColorPicker.get_SelectedColor());
				}
			});
		}
	}
}
