using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.UI.Views.Controls;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public abstract class BaseSettingsView : BaseView
	{
		private const int LEFT_PADDING = 20;

		private const int CONTROL_X_SPACING = 20;

		private const int LABEL_WIDTH = 250;

		private const int BINDING_WIDTH = 170;

		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		protected ModuleSettings ModuleSettings { get; set; }

		private static IEnumerable<Color> Colors { get; set; }

		private static Panel ColorPickerPanel { get; set; }

		private static string SelectedColorSetting { get; set; }

		private static ColorPicker ColorPicker { get; set; }

		public BaseSettingsView(ModuleSettings settings)
		{
			ModuleSettings = settings;
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			if (Colors == null)
			{
				progress.Report(Strings.BaseSettingsView_LoadingColors);
				try
				{
					Colors = (IEnumerable<Color>)(await ((IAllExpandableClient<Color>)(object)EventTableModule.ModuleInstance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).AllAsync(default(CancellationToken)));
				}
				catch (Exception ex)
				{
					Logger.Warn("Could not load gw2 colors: " + ex.Message);
					if (ModuleSettings.DefaultGW2Color != null)
					{
						Logger.Debug("Adding default color: " + ModuleSettings.DefaultGW2Color.get_Name());
						Colors = new List<Color> { ModuleSettings.DefaultGW2Color };
					}
				}
			}
			if (ColorPicker == null)
			{
				progress.Report(Strings.BaseSettingsView_LoadingColorPicker);
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
				progress.Report(Strings.BaseSettingsView_AddingColorsToColorPicker);
				if (Colors != null)
				{
					foreach (Color color2 in Colors.OrderBy((Color color) => color.get_Categories().FirstOrDefault()))
					{
						ColorPicker.get_Colors().Add(color2);
					}
				}
			}
			progress.Report(string.Empty);
			return await InternalLoad(progress);
		}

		protected sealed override void InternalBuild(Panel parent)
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
			Rectangle bounds = ((Container)parent).get_ContentRegion();
			FlowPanel val = new FlowPanel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(5f, 2f));
			val.set_OuterControlPadding(new Vector2(20f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent((Container)(object)parent);
			FlowPanel parentPanel = val;
			BuildView((Panel)(object)parentPanel);
		}

		protected abstract void BuildView(Panel parent);

		protected Panel RenderChangedTypeSetting<T, TOverride>(Panel parent, SettingEntry<T> setting, Func<TOverride, T> convertFunction)
		{
			Panel panel = GetPanel((Container)(object)parent);
			Label label = GetLabel(panel, ((SettingEntry)setting).get_DisplayName());
			try
			{
				Control obj = ControlHandler.CreateFromChangedTypeSetting(setting, delegate(SettingEntry<T> settingEntry, TOverride val)
				{
					T value = convertFunction(val);
					return HandleValidation(settingEntry, value);
				}, 170, -1, ((Control)label).get_Right() + 20, 0);
				obj.set_Parent((Container)(object)panel);
				obj.set_BasicTooltipText(((SettingEntry)setting).get_Description());
				return panel;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Type \"" + ((SettingEntry)setting).get_SettingType().FullName + "\" could not be found in internal type lookup:");
				return panel;
			}
		}

		protected Panel RenderSetting<T>(Panel parent, SettingEntry<T> setting)
		{
			return RenderChangedTypeSetting(parent, setting, (T val) => val);
		}

		protected Panel RenderSetting<T>(Panel parent, SettingEntry<T> setting, Action<T> onChangeAction)
		{
			Panel result = RenderSetting<T>(parent, setting);
			setting.add_SettingChanged((EventHandler<ValueChangedEventArgs<T>>)delegate(object s, ValueChangedEventArgs<T> e)
			{
				onChangeAction?.Invoke(e.get_NewValue());
			});
			return result;
		}

		protected void RenderColorSetting(Panel parent, SettingEntry<Color> setting)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			Panel panel = GetPanel((Container)(object)parent);
			Label label = GetLabel(panel, ((SettingEntry)setting).get_DisplayName());
			ColorBox val = new ColorBox();
			((Control)val).set_Location(new Point(((Control)label).get_Right() + 20, 0));
			((Control)val).set_Parent((Container)(object)panel);
			val.set_Color(setting.get_Value());
			ColorBox colorBox = val;
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
				Color val3 = new Color();
				val3.set_Id(int.MaxValue);
				val3.set_Name("temp");
				Color item = val3;
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
					Color val2 = ColorPicker.get_SelectedColor();
					if (!HandleValidation<Color>(setting, val2))
					{
						val2 = setting.get_Value();
					}
					setting.set_Value(val2);
					((Control)ColorPickerPanel).set_Visible(false);
					colorBox.set_Color(val2);
				}
			});
		}

		private bool HandleValidation<T>(SettingEntry<T> settingEntry, T value)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			SettingValidationResult result = settingEntry.CheckValidation(value);
			if (!((SettingValidationResult)(ref result)).get_Valid())
			{
				ShowError(((SettingValidationResult)(ref result)).get_InvalidMessage());
				return false;
			}
			return true;
		}

		protected override void Unload()
		{
			base.Unload();
		}
	}
}
