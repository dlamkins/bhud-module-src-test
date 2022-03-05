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
using Estreya.BlishHUD.EventTable.Controls;
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Helpers;
using Estreya.BlishHUD.EventTable.Resources;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public abstract class BaseSettingsView : View
	{
		private const int LEFT_PADDING = 20;

		private const int CONTROL_X_SPACING = 20;

		private const int LABEL_WIDTH = 250;

		private const int BINDING_WIDTH = 170;

		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		private static readonly Dictionary<Type, Func<SettingEntry, int, int, Control>> _typeLookup = new Dictionary<Type, Func<SettingEntry, int, int, Control>>
		{
			{
				typeof(bool),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Expected O, but got Unknown
					SettingEntry<bool> setting6 = settingEntry as SettingEntry<bool>;
					Checkbox val9 = new Checkbox();
					((Control)val9).set_Width(definedWidth);
					((Control)val9).set_Location(new Point(xPos, 0));
					val9.set_Checked(setting6?.get_Value() ?? false);
					((Control)val9).set_Enabled(!settingEntry.IsDisabled());
					Checkbox val10 = val9;
					if (setting6 != null)
					{
						val10.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
						{
							setting6.set_Value(e.get_Checked());
						});
					}
					return (Control)(object)val10;
				}
			},
			{
				typeof(string),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_005c: Expected O, but got Unknown
					SettingEntry<string> setting5 = settingEntry as SettingEntry<string>;
					TextBox val7 = new TextBox();
					((Control)val7).set_Width(definedWidth);
					((Control)val7).set_Location(new Point(xPos, 0));
					((TextInputBase)val7).set_Text(setting5?.get_Value() ?? string.Empty);
					((Control)val7).set_Enabled(!settingEntry.IsDisabled());
					TextBox val8 = val7;
					if (setting5 != null)
					{
						((TextInputBase)val8).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
						{
							setting5.set_Value(((ValueChangedEventArgs<string>)(object)e).get_NewValue());
						});
					}
					return (Control)(object)val8;
				}
			},
			{
				typeof(float),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_0045: Unknown result type (might be due to invalid IL or missing references)
					//IL_004a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0054: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_006d: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d5: Expected O, but got Unknown
					SettingEntry<float> setting4 = settingEntry as SettingEntry<float>;
					(float, float)? tuple2 = setting4?.GetRange() ?? null;
					TrackBar val5 = new TrackBar();
					((Control)val5).set_Width(definedWidth);
					((Control)val5).set_Location(new Point(xPos, 0));
					((Control)val5).set_Enabled(!settingEntry.IsDisabled());
					val5.set_MinValue(tuple2.HasValue ? tuple2.Value.Item1 : 0f);
					val5.set_MaxValue(tuple2.HasValue ? tuple2.Value.Item2 : 100f);
					val5.set_SmallStep(true);
					val5.set_Value(setting4?.GetValue() ?? 50f);
					TrackBar val6 = val5;
					if (setting4 != null)
					{
						val6.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
						{
							setting4.set_Value(e.get_Value());
						});
					}
					return (Control)(object)val6;
				}
			},
			{
				typeof(int),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_0045: Unknown result type (might be due to invalid IL or missing references)
					//IL_004a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0054: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_006d: Unknown result type (might be due to invalid IL or missing references)
					//IL_008c: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c7: Expected O, but got Unknown
					SettingEntry<int> setting3 = settingEntry as SettingEntry<int>;
					(int, int)? tuple = setting3?.GetRange() ?? null;
					TrackBar val3 = new TrackBar();
					((Control)val3).set_Width(definedWidth);
					((Control)val3).set_Location(new Point(xPos, 0));
					((Control)val3).set_Enabled(!settingEntry.IsDisabled());
					val3.set_MinValue((float)(tuple.HasValue ? tuple.Value.Item1 : 0));
					val3.set_MaxValue((float)(tuple.HasValue ? tuple.Value.Item2 : 100));
					val3.set_Value((float)(setting3?.GetValue() ?? 50));
					TrackBar val4 = val3;
					if (setting3 != null)
					{
						val4.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
						{
							setting3.set_Value((int)e.get_Value());
						});
					}
					return (Control)(object)val4;
				}
			},
			{
				typeof(KeyBinding),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					SettingEntry<KeyBinding> setting2 = settingEntry as SettingEntry<KeyBinding>;
					KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner(setting2.get_Value(), withName: false);
					((Control)keybindingAssigner2).set_Width(definedWidth);
					((Control)keybindingAssigner2).set_Location(new Point(xPos, 0));
					((Control)keybindingAssigner2).set_Enabled(!settingEntry.IsDisabled());
					KeybindingAssigner keybindingAssigner = keybindingAssigner2;
					if (setting2 != null)
					{
						keybindingAssigner.BindingChanged += delegate
						{
							setting2.set_Value(keybindingAssigner.KeyBinding);
						};
					}
					return (Control)(object)keybindingAssigner;
				}
			},
			{
				typeof(Enum),
				delegate(SettingEntry settingEntry, int definedWidth, int xPos)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					//IL_001f: Expected O, but got Unknown
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					dynamic setting = settingEntry;
					Dropdown val = new Dropdown();
					((Control)val).set_Width(definedWidth);
					((Control)val).set_Location(new Point(xPos, 0));
					val.set_SelectedItem((string)setting?.Value.ToString());
					((Control)val).set_Enabled(!settingEntry.IsDisabled());
					Dropdown val2 = val;
					string[] names = Enum.GetNames(settingEntry.get_SettingType());
					foreach (string item in names)
					{
						val2.get_Items().Add(item);
					}
					if (setting != null)
					{
						val2.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
						{
							setting.Value = Enum.Parse(settingEntry.get_SettingType(), e.get_CurrentValue());
						});
					}
					return (Control)(object)val2;
				}
			}
		};

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
			val.set_OuterControlPadding(new Vector2(20f, 15f));
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_AutoSizePadding(new Point(0, 15));
			((Control)val).set_Parent(buildPanel);
			FlowPanel parentPanel = val;
			InternalBuild((Panel)(object)parentPanel);
		}

		protected abstract Task<bool> InternalLoad(IProgress<string> progress);

		protected abstract void InternalBuild(Panel parent);

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

		protected Panel RenderSetting(Panel parent, SettingEntry setting)
		{
			Panel panel = GetPanel(parent);
			Label label = GetLabel(panel, setting.get_DisplayName());
			Type type = setting.get_SettingType();
			if (setting.get_SettingType().IsEnum)
			{
				type = typeof(Enum);
			}
			if (_typeLookup.TryGetValue(type, out var controlBuilder))
			{
				Control obj = controlBuilder(setting, 170, ((Control)label).get_Right() + 20);
				obj.set_Parent((Container)(object)panel);
				obj.set_BasicTooltipText(setting.get_Description());
			}
			else
			{
				Logger.Warn("Type \"" + setting.get_SettingType().FullName + "\" could not be found in internal type lookup.");
			}
			return panel;
		}

		private Panel GetPanel(Panel parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent((Container)(object)parent);
			return val;
		}

		private Label GetLabel(Panel parent, string text)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text(text);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Width(250);
			return val;
		}

		protected void RenderButton(Panel parent, string text, Action action, Func<bool> disabledCallback = null)
		{
			RenderButton(parent, text, delegate
			{
				action();
				return Task.CompletedTask;
			}, disabledCallback);
		}

		protected void RenderButton(Panel parent, string text, Func<Task> action, Func<bool> disabledCallback = null)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel(parent);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)panel);
			val.set_Text(text);
			((Control)val).set_Width((int)EventTableModule.ModuleInstance.Font.MeasureString(text).Width);
			((Control)val).set_Enabled(disabledCallback == null || !disabledCallback());
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AsyncHelper.RunSync(action.Invoke);
			});
		}

		protected void RenderColorSetting(Panel parent, SettingEntry<Color> setting)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			Panel panel = GetPanel(parent);
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
				Color val2 = new Color();
				val2.set_Id(int.MaxValue);
				val2.set_Name("temp");
				Color item = val2;
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
