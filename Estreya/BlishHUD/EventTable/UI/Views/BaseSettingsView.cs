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
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public abstract class BaseSettingsView : View
	{
		private const int LEFT_PADDING = 20;

		private const int CONTROL_X_SPACING = 20;

		private const int LABEL_WIDTH = 250;

		private const int BINDING_WIDTH = 170;

		private static readonly Logger Logger = Logger.GetLogger<BaseSettingsView>();

		private CancellationTokenSource ErrorCancellationTokenSource = new CancellationTokenSource();

		private Dictionary<Type, Func<SettingEntry, int, int, Control>> _typeLookup;

		protected ModuleSettings ModuleSettings { get; set; }

		private static IEnumerable<Color> Colors { get; set; }

		private static Panel ColorPickerPanel { get; set; }

		private static string SelectedColorSetting { get; set; }

		private static ColorPicker ColorPicker { get; set; }

		private Container BuildPanel { get; set; }

		private Panel ErrorPanel { get; set; }

		public BaseSettingsView(ModuleSettings settings)
			: this()
		{
			ModuleSettings = settings;
			LoadTypeLookup();
		}

		private void LoadTypeLookup()
		{
			_typeLookup = new Dictionary<Type, Func<SettingEntry, int, int, Control>>
			{
				{
					typeof(bool),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						//IL_001f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0026: Unknown result type (might be due to invalid IL or missing references)
						//IL_0029: Unknown result type (might be due to invalid IL or missing references)
						//IL_0033: Unknown result type (might be due to invalid IL or missing references)
						//IL_004b: Unknown result type (might be due to invalid IL or missing references)
						//IL_005f: Expected O, but got Unknown
						SettingEntry<bool> setting6 = settingEntry as SettingEntry<bool>;
						Checkbox val6 = new Checkbox();
						((Control)val6).set_Width(definedWidth);
						((Control)val6).set_Location(new Point(xPos, 0));
						val6.set_Checked(setting6?.get_Value() ?? false);
						((Control)val6).set_Enabled(!settingEntry.IsDisabled());
						Checkbox checkbox = val6;
						if (setting6 != null)
						{
							checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
							{
								if (HandleValidation(setting6, e.get_Checked()))
								{
									setting6.set_Value(e.get_Checked());
								}
								else
								{
									checkbox.set_Checked(!e.get_Checked());
								}
							});
						}
						return (Control)(object)checkbox;
					}
				},
				{
					typeof(string),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_001a: Unknown result type (might be due to invalid IL or missing references)
						//IL_001f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0026: Unknown result type (might be due to invalid IL or missing references)
						//IL_0029: Unknown result type (might be due to invalid IL or missing references)
						//IL_0033: Unknown result type (might be due to invalid IL or missing references)
						//IL_0054: Unknown result type (might be due to invalid IL or missing references)
						//IL_0068: Expected O, but got Unknown
						SettingEntry<string> setting5 = settingEntry as SettingEntry<string>;
						TextBox val4 = new TextBox();
						((Control)val4).set_Width(definedWidth);
						((Control)val4).set_Location(new Point(xPos, 0));
						((TextInputBase)val4).set_Text(setting5?.get_Value() ?? string.Empty);
						((Control)val4).set_Enabled(!settingEntry.IsDisabled());
						TextBox textBox = val4;
						if (setting5 != null)
						{
							((TextInputBase)textBox).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
							{
								ValueChangedEventArgs<string> val5 = (ValueChangedEventArgs<string>)(object)e;
								if (HandleValidation(setting5, val5.get_NewValue()))
								{
									setting5.set_Value(val5.get_NewValue());
								}
								else
								{
									((TextInputBase)textBox).set_Text(val5.get_PreviousValue());
								}
							});
						}
						return (Control)(object)textBox;
					}
				},
				{
					typeof(float),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_004b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0050: Unknown result type (might be due to invalid IL or missing references)
						//IL_0057: Unknown result type (might be due to invalid IL or missing references)
						//IL_005a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0064: Unknown result type (might be due to invalid IL or missing references)
						//IL_0073: Unknown result type (might be due to invalid IL or missing references)
						//IL_0095: Unknown result type (might be due to invalid IL or missing references)
						//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
						//IL_00be: Unknown result type (might be due to invalid IL or missing references)
						//IL_00df: Expected O, but got Unknown
						SettingEntry<float> setting4 = settingEntry as SettingEntry<float>;
						(float, float)? tuple2 = setting4?.GetRange() ?? null;
						TrackBar val3 = new TrackBar();
						((Control)val3).set_Width(definedWidth);
						((Control)val3).set_Location(new Point(xPos, 0));
						((Control)val3).set_Enabled(!settingEntry.IsDisabled());
						val3.set_MinValue(tuple2.HasValue ? tuple2.Value.Item1 : 0f);
						val3.set_MaxValue(tuple2.HasValue ? tuple2.Value.Item2 : 100f);
						val3.set_SmallStep(true);
						val3.set_Value(setting4?.GetValue() ?? 50f);
						TrackBar trackBar2 = val3;
						if (setting4 != null)
						{
							trackBar2.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
							{
								if (HandleValidation(setting4, e.get_Value()))
								{
									setting4.set_Value(e.get_Value());
								}
								else
								{
									trackBar2.set_Value(setting4.get_Value());
								}
							});
						}
						return (Control)(object)trackBar2;
					}
				},
				{
					typeof(int),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_004b: Unknown result type (might be due to invalid IL or missing references)
						//IL_0050: Unknown result type (might be due to invalid IL or missing references)
						//IL_0057: Unknown result type (might be due to invalid IL or missing references)
						//IL_005a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0064: Unknown result type (might be due to invalid IL or missing references)
						//IL_0073: Unknown result type (might be due to invalid IL or missing references)
						//IL_0092: Unknown result type (might be due to invalid IL or missing references)
						//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d1: Expected O, but got Unknown
						SettingEntry<int> setting3 = settingEntry as SettingEntry<int>;
						(int, int)? tuple = setting3?.GetRange() ?? null;
						TrackBar val2 = new TrackBar();
						((Control)val2).set_Width(definedWidth);
						((Control)val2).set_Location(new Point(xPos, 0));
						((Control)val2).set_Enabled(!settingEntry.IsDisabled());
						val2.set_MinValue((float)(tuple.HasValue ? tuple.Value.Item1 : 0));
						val2.set_MaxValue((float)(tuple.HasValue ? tuple.Value.Item2 : 100));
						val2.set_Value((float)(setting3?.GetValue() ?? 50));
						TrackBar trackBar = val2;
						if (setting3 != null)
						{
							trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
							{
								if (HandleValidation(setting3, (int)e.get_Value()))
								{
									setting3.set_Value((int)e.get_Value());
								}
								else
								{
									trackBar.set_Value((float)setting3.get_Value());
								}
							});
						}
						return (Control)(object)trackBar;
					}
				},
				{
					typeof(KeyBinding),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
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
								if (HandleValidation<KeyBinding>(setting2, keybindingAssigner.KeyBinding))
								{
									setting2.set_Value(keybindingAssigner.KeyBinding);
								}
								else
								{
									keybindingAssigner.KeyBinding = setting2.get_Value();
								}
							};
						}
						return (Control)(object)keybindingAssigner;
					}
				},
				{
					typeof(Enum),
					delegate(SettingEntry settingEntry, int definedWidth, int xPos)
					{
						//IL_0021: Unknown result type (might be due to invalid IL or missing references)
						//IL_0027: Expected O, but got Unknown
						//IL_0031: Unknown result type (might be due to invalid IL or missing references)
						dynamic setting = settingEntry;
						Dropdown val = new Dropdown();
						((Control)val).set_Width(definedWidth);
						((Control)val).set_Location(new Point(xPos, 0));
						val.set_SelectedItem((string)setting?.Value.ToString());
						((Control)val).set_Enabled(!settingEntry.IsDisabled());
						Dropdown dropdown = val;
						string[] names = Enum.GetNames(settingEntry.get_SettingType());
						foreach (string item in names)
						{
							dropdown.get_Items().Add(item);
						}
						if (setting != null)
						{
							bool resetingValue = false;
							dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
							{
								if (!resetingValue)
								{
									object obj = Enum.Parse(settingEntry.get_SettingType(), e.get_CurrentValue());
									if (HandleValidation(setting, (dynamic)obj))
									{
										setting.Value = obj;
									}
									else
									{
										resetingValue = true;
										dropdown.set_SelectedItem(e.get_PreviousValue());
										resetingValue = false;
									}
								}
							});
						}
						return (Control)(object)dropdown;
					}
				}
			};
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
			FlowPanel parentPanel = (FlowPanel)(object)(BuildPanel = (Container)val);
			RegisterErrorPanel(buildPanel);
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
			Panel panel = GetPanel((Container)(object)parent);
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

		private Panel GetPanel(Container parent)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			Panel val = new Panel();
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Control)val).set_Parent(parent);
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
			Panel panel = GetPanel((Container)(object)parent);
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

		private void RegisterErrorPanel(Container parent)
		{
			Panel panel = GetPanel(parent);
			((Control)panel).set_ZIndex(1000);
			((Container)panel).set_WidthSizingMode((SizingMode)2);
			((Control)panel).set_Visible(false);
			ErrorPanel = panel;
		}

		public async void ShowError(string message)
		{
			lock (ErrorPanel)
			{
				if (((Control)ErrorPanel).get_Visible())
				{
					ErrorCancellationTokenSource.Cancel();
					ErrorCancellationTokenSource = new CancellationTokenSource();
				}
			}
			((Container)ErrorPanel).ClearChildren();
			BitmapFont font = GameService.Content.get_DefaultFont32();
			message = DrawUtil.WrapText(font, message, (float)((Control)ErrorPanel).get_Width() * 0.75f);
			Label label = GetLabel(ErrorPanel, message);
			((Control)label).set_Width(((Control)ErrorPanel).get_Width());
			label.set_Font(font);
			label.set_HorizontalAlignment((HorizontalAlignment)1);
			label.set_TextColor(Color.get_Red());
			((Control)ErrorPanel).set_Height(((Control)label).get_Height());
			Panel errorPanel = ErrorPanel;
			Rectangle contentRegion = BuildPanel.get_ContentRegion();
			((Control)errorPanel).set_Bottom(((Rectangle)(ref contentRegion)).get_Bottom());
			lock (ErrorPanel)
			{
				((Control)ErrorPanel).Show();
			}
			try
			{
				await Task.Delay(5000, ErrorCancellationTokenSource.Token);
			}
			catch (TaskCanceledException)
			{
				Logger.Debug("Task was canceled to show new error:");
			}
			lock (ErrorPanel)
			{
				((Control)ErrorPanel).Hide();
			}
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
			((View<IPresenter>)this).Unload();
			((Control)ErrorPanel).Dispose();
		}
	}
}
