using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Services;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Humanizer;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.UI.Views
{
	public abstract class BaseView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<BaseView>();

		private CancellationTokenSource _messageCancellationTokenSource;

		protected int CONTROL_X_SPACING { get; set; } = 20;


		protected int LABEL_WIDTH { get; set; } = 250;


		protected static List<Color> Colors { get; set; }

		protected Gw2ApiManager APIManager { get; }

		protected BitmapFont Font { get; }

		public Color DefaultColor { get; set; }

		protected IconService IconService { get; }

		protected TranslationService TranslationService { get; }

		protected Panel MainPanel { get; private set; }

		public BaseView(Gw2ApiManager apiManager, IconService iconService, TranslationService translationService, BitmapFont font = null)
			: this()
		{
			APIManager = apiManager;
			IconService = iconService;
			TranslationService = translationService;
			Font = font ?? GameService.Content.get_DefaultFont16();
		}

		protected sealed override async Task<bool> Load(IProgress<string> progress)
		{
			if (Colors == null)
			{
				progress.Report(TranslationService.GetTranslation("baseView-loadingColors", "Loading Colors..."));
				try
				{
					if (APIManager != null)
					{
						Colors = ((IEnumerable<Color>)(await ((IAllExpandableClient<Color>)(object)APIManager.get_Gw2ApiClient().get_V2().get_Colors()).AllAsync(default(CancellationToken)))).ToList();
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("Could not load gw2 colors: " + ex.Message);
					if (DefaultColor != null)
					{
						Logger.Debug("Adding default color: " + DefaultColor.get_Name());
						Colors = new List<Color> { DefaultColor };
					}
				}
			}
			progress.Report(string.Empty);
			return await InternalLoad(progress);
		}

		protected abstract Task<bool> InternalLoad(IProgress<string> progress);

		protected sealed override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			Rectangle bounds = buildPanel.get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			((Control)val).set_Parent(buildPanel);
			Panel parentPanel = (MainPanel = val);
			try
			{
				InternalBuild(parentPanel);
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed building view " + ((object)this).GetType().FullName);
			}
		}

		protected abstract void InternalBuild(Panel parent);

		protected void RenderEmptyLine(Panel parent, int height = 25)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Height(height);
			((Container)val).set_WidthSizingMode((SizingMode)2);
		}

		protected TextBox RenderTextbox(Panel parent, Point location, int width, string value, string placeholder, Action<string> onChangeAction = null, Action<string> onEnterAction = null, bool clearOnEnter = false, Func<string, string, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (string _, string _) => Task.FromResult(result: true);
			}
			bool changing = false;
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)parent);
			((TextInputBase)val).set_PlaceholderText(placeholder);
			((TextInputBase)val).set_Text(value);
			((Control)val).set_Location(location);
			((Control)val).set_Width(width);
			((TextInputBase)val).set_Font(Font);
			TextBox textBox = val;
			if (onChangeAction != null)
			{
				((TextInputBase)textBox).add_TextChanged((EventHandler<EventArgs>)delegate(object s, EventArgs e)
				{
					if (!changing)
					{
						changing = true;
						TextBox scopeTextBox = (TextBox)((s is TextBox) ? s : null);
						ValueChangedEventArgs<string> ea = e as ValueChangedEventArgs<string>;
						onBeforeChangeAction(ea?.get_PreviousValue(), ((TextInputBase)scopeTextBox).get_Text()).ContinueWith(delegate(Task<bool> resultTask)
						{
							if (resultTask.Result)
							{
								onChangeAction?.Invoke(((TextInputBase)scopeTextBox).get_Text());
							}
							else
							{
								((TextInputBase)scopeTextBox).set_Text(ea.get_PreviousValue());
							}
							changing = false;
						});
						onChangeAction?.Invoke(((TextInputBase)scopeTextBox).get_Text());
					}
				});
			}
			if (onEnterAction != null)
			{
				textBox.add_EnterPressed((EventHandler<EventArgs>)delegate(object s, EventArgs e)
				{
					TextBox val2 = (TextBox)((s is TextBox) ? s : null);
					onEnterAction?.Invoke(((TextInputBase)val2).get_Text());
					if (clearOnEnter)
					{
						((TextInputBase)textBox).set_Text(string.Empty);
					}
				});
			}
			return textBox;
		}

		protected TrackBar RenderTrackBar(Panel parent, Point location, int width, int value, (int Min, int Max)? range = null, Action<int> onChangeAction = null, Func<int, int, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (int _, int _) => Task.FromResult(result: true);
			}
			TrackBar val = new TrackBar();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Location(location);
			((Control)val).set_Width(width);
			TrackBar trackBar = val;
			trackBar.set_MinValue((float)(range?.Min ?? 0));
			trackBar.set_MaxValue((float)(range?.Max ?? 100));
			trackBar.set_Value((float)value);
			if (onChangeAction != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					TrackBar val2 = (TrackBar)((s is TrackBar) ? s : null);
					onChangeAction?.Invoke((int)val2.get_Value());
				});
			}
			return trackBar;
		}

		protected TrackBar RenderTrackBar(Panel parent, Point location, int width, float value, (float Min, float Max)? range = null, Action<float> onChangeAction = null, Func<float, float, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (float _, float _) => Task.FromResult(result: true);
			}
			TrackBar val = new TrackBar();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_SmallStep(true);
			((Control)val).set_Location(location);
			((Control)val).set_Width(width);
			TrackBar trackBar = val;
			trackBar.set_MinValue(range?.Min ?? 0f);
			trackBar.set_MaxValue(range?.Max ?? 1f);
			trackBar.set_Value(value);
			if (onChangeAction != null)
			{
				trackBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate(object s, ValueEventArgs<float> e)
				{
					TrackBar val2 = (TrackBar)((s is TrackBar) ? s : null);
					onChangeAction?.Invoke(val2.get_Value());
				});
			}
			return trackBar;
		}

		protected Checkbox RenderCheckbox(Panel parent, Point location, bool value, Action<bool> onChangeAction = null, Func<bool, bool, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (bool _, bool _) => Task.FromResult(result: true);
			}
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Checked(value);
			((Control)val).set_Location(location);
			Checkbox checkBox = val;
			if (onChangeAction != null)
			{
				checkBox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					onBeforeChangeAction(!e.get_Checked(), e.get_Checked()).ContinueWith(delegate(Task<bool> resultTask)
					{
						object obj = s;
						Checkbox val2 = (Checkbox)((obj is Checkbox) ? obj : null);
						if (resultTask.Result)
						{
							onChangeAction?.Invoke(val2.get_Checked());
						}
						else
						{
							val2.set_Checked(!e.get_Checked());
						}
					});
				});
			}
			return checkBox;
		}

		protected Dropdown RenderDropdown<T>(Panel parent, Point location, int width, T? value, T[] values = null, Action<T> onChangeAction = null, Func<string, string, Task<bool>> onBeforeChangeAction = null) where T : struct, Enum
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (string _, string _) => Task.FromResult(result: true);
			}
			LetterCasing casing = LetterCasing.Title;
			Dropdown dropdown2 = new Dropdown();
			((Control)dropdown2).set_Parent((Container)(object)parent);
			((Control)dropdown2).set_Width(width);
			((Control)dropdown2).set_Location(location);
			Dropdown dropdown = dropdown2;
			if (values == null)
			{
				values = (T[])Enum.GetValues(typeof(T));
			}
			string[] formattedValues = values.Select((T value) => value.GetTranslatedValue(TranslationService, casing)).ToArray();
			string selectedValue = null;
			if (value.HasValue)
			{
				selectedValue = value.GetTranslatedValue(TranslationService, casing);
			}
			string[] array = formattedValues;
			foreach (string valueToAdd in array)
			{
				dropdown.Items.Add(valueToAdd);
			}
			dropdown.SelectedItem = selectedValue;
			if (onChangeAction != null)
			{
				dropdown.ValueChanged += delegate(object s, ValueChangedEventArgs e)
				{
					Dropdown dropdown3 = s as Dropdown;
					onChangeAction?.Invoke(values[formattedValues.ToList().IndexOf(dropdown3.SelectedItem)]);
				};
			}
			return dropdown;
		}

		protected Dropdown RenderDropdown(Panel parent, Point location, int width, string[] values, string value, Action<string> onChangeAction = null, Func<string, string, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (string _, string _) => Task.FromResult(result: true);
			}
			Dropdown dropdown2 = new Dropdown();
			((Control)dropdown2).set_Parent((Container)(object)parent);
			((Control)dropdown2).set_Width(width);
			((Control)dropdown2).set_Location(location);
			Dropdown dropdown = dropdown2;
			if (values != null)
			{
				foreach (string valueToAdd in values)
				{
					dropdown.Items.Add(valueToAdd);
				}
				dropdown.SelectedItem = value;
			}
			if (onChangeAction != null)
			{
				dropdown.ValueChanged += delegate(object s, ValueChangedEventArgs e)
				{
					Dropdown dropdown3 = s as Dropdown;
					onChangeAction?.Invoke(dropdown3.SelectedItem);
				};
			}
			return dropdown;
		}

		protected KeybindingAssigner RenderKeybinding(Panel parent, Point location, int width, KeyBinding value, Action<KeyBinding> onChangeAction = null, Func<KeyBinding, KeyBinding, Task<bool>> onBeforeChangeAction = null)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (onBeforeChangeAction == null)
			{
				onBeforeChangeAction = (KeyBinding _, KeyBinding _) => Task.FromResult(result: true);
			}
			KeybindingAssigner keybindingAssigner2 = new KeybindingAssigner(withName: false);
			((Control)keybindingAssigner2).set_Parent((Container)(object)parent);
			((Control)keybindingAssigner2).set_Width(width);
			((Control)keybindingAssigner2).set_Location(location);
			keybindingAssigner2.KeyBinding = value;
			KeybindingAssigner keybindingAssigner = keybindingAssigner2;
			if (onChangeAction != null)
			{
				keybindingAssigner.BindingChanged += delegate(object s, EventArgs e)
				{
					KeybindingAssigner keybindingAssigner3 = s as KeybindingAssigner;
					onChangeAction?.Invoke(keybindingAssigner3.KeyBinding);
				};
			}
			return keybindingAssigner;
		}

		protected Panel GetPanel(Container parent)
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

		protected Label GetLabel(Panel parent, string text, Color? color = null, BitmapFont font = null)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			if (font == null)
			{
				font = Font;
			}
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text(text);
			val.set_Font(font);
			val.set_TextColor((Color)(((_003F?)color) ?? Color.get_White()));
			val.set_AutoSizeHeight(!string.IsNullOrWhiteSpace(text));
			((Control)val).set_Width((int)font.MeasureString(text).Width + 20);
			return val;
		}

		private Button BuildButton(Panel parent, string text, Func<bool> disabledCallback = null)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Button button2 = new Button();
			((Control)button2).set_Parent((Container)(object)parent);
			button2.Text = text;
			((Control)button2).set_Enabled(disabledCallback == null || !disabledCallback());
			Button button = button2;
			int measuredWidth = (int)Font.MeasureString(text).Width + 10;
			if (((Control)button).get_Width() < measuredWidth)
			{
				((Control)button).set_Width(measuredWidth);
			}
			return button;
		}

		protected Button RenderButton(Panel parent, string text, Action action, Func<bool> disabledCallback = null)
		{
			Button button = BuildButton(parent, text, disabledCallback);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					action?.Invoke();
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed executing action:");
					ShowError(ex.Message);
				}
			});
			return button;
		}

		protected Button RenderButtonAsync(Panel parent, string text, Func<Task> action, Func<bool> disabledCallback = null)
		{
			Button button = BuildButton(parent, text, disabledCallback);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(async delegate
				{
					try
					{
						((Control)button).set_Enabled(false);
						await (action?.Invoke());
					}
					catch (Exception ex)
					{
						Logger.Warn(ex, "Failed executing action:");
						ShowError(ex.Message);
					}
					finally
					{
						((Control)button).set_Enabled(true);
					}
				});
			});
			return button;
		}

		protected (Label TitleLabel, Label ValueLabel) RenderLabel(Panel parent, string title, string value = null, Color? textColorTitle = null, Color? textColorValue = null, int? valueXLocation = null)
		{
			Panel panel = GetPanel((Container)(object)parent);
			Label titleLabel = GetLabel(panel, title, textColorTitle);
			Label valueLabel = null;
			if (value != null)
			{
				valueLabel = GetLabel(panel, value, textColorValue);
				((Control)valueLabel).set_Left(valueXLocation ?? (((Control)titleLabel).get_Right() + CONTROL_X_SPACING));
			}
			else
			{
				titleLabel.set_AutoSizeWidth(true);
			}
			return (titleLabel, valueLabel);
		}

		protected ColorBox RenderColorBox(Panel parent, Point location, Color initialColor, Action<Color> onChange, Panel selectorPanel = null, Thickness? innerSelectorPanelPadding = null)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Expected O, but got Unknown
			Panel panel = GetPanel((Container)(object)parent);
			ColorBox val = new ColorBox();
			((Control)val).set_Location(location);
			((Control)val).set_Parent((Container)(object)panel);
			val.set_Color(initialColor);
			ColorBox colorBox = val;
			bool selectorPanelCreated = selectorPanel == null;
			if (selectorPanel == null)
			{
				selectorPanel = GetPanel((Container)(object)parent);
				((Control)selectorPanel).set_Visible(false);
			}
			ColorPicker val2 = new ColorPicker();
			((Control)val2).set_Parent((Container)(object)selectorPanel);
			((Control)val2).set_ZIndex(int.MaxValue);
			((Control)val2).set_Visible(false);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)2);
			((Control)val2).set_Padding((Thickness)(((_003F?)innerSelectorPanelPadding) ?? Thickness.Zero));
			val2.set_AssociatedColorBox(colorBox);
			ColorPicker colorPicker = val2;
			if (Colors != null)
			{
				foreach (Color color2 in Colors.OrderBy((Color color) => color.get_Categories().FirstOrDefault()))
				{
					colorPicker.get_Colors().Add(color2);
				}
			}
			((Control)colorBox).add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)delegate
			{
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Expected O, but got Unknown
				if (selectorPanelCreated)
				{
					((Control)selectorPanel).set_Visible(true);
				}
				((Control)colorPicker).set_Visible(true);
				try
				{
					((Control)colorPicker).DoUpdate((GameTime)null);
					Color val3 = new Color();
					val3.set_Id(int.MaxValue);
					val3.set_Name("temp");
					Color item = val3;
					((Control)colorPicker).RecalculateLayout();
					colorPicker.get_Colors().Add(item);
					colorPicker.get_Colors().Remove(item);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Hacky colorpicker resize failed.. Nothing to prevent this..");
				}
			});
			colorPicker.add_SelectedColorChanged((EventHandler<EventArgs>)delegate(object sender, EventArgs eArgs)
			{
				object obj = ((sender is ColorPicker) ? sender : null);
				Color selectedColor = ((ColorPicker)obj).get_SelectedColor();
				onChange?.Invoke(selectedColor);
				if (selectorPanelCreated)
				{
					((Control)selectorPanel).set_Visible(false);
				}
				((Control)obj).set_Visible(false);
			});
			return colorBox;
		}

		private void ShowMessage(string message, Color color, int durationMS, BitmapFont font = null)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			_messageCancellationTokenSource?.Cancel();
			_messageCancellationTokenSource = new CancellationTokenSource();
			if (font == null)
			{
				font = Font;
			}
			Size2 textSize = font.MeasureString(message);
			Panel messagePanel = new Panel();
			((Container)messagePanel).set_HeightSizingMode((SizingMode)0);
			((Control)messagePanel).set_Height((int)textSize.Height);
			((Container)messagePanel).set_WidthSizingMode((SizingMode)0);
			((Control)messagePanel).set_Width((int)textSize.Width + 10);
			((Control)messagePanel).set_Location(new Point(((Control)MainPanel).get_Width() / 2 - ((Control)messagePanel).get_Width() / 2, ((Control)MainPanel).get_Bottom() - ((Control)messagePanel).get_Height()));
			GetLabel(messagePanel, message, color, font);
			((Control)messagePanel).set_Parent((Container)(object)MainPanel);
			Task.Run(async delegate
			{
				try
				{
					await Task.Delay(durationMS, _messageCancellationTokenSource.Token);
				}
				catch (Exception)
				{
				}
				((Control)messagePanel).Dispose();
			});
		}

		protected void ShowError(string message)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			ShowMessage(message, Color.get_Red(), 5000, GameService.Content.get_DefaultFont18());
		}

		protected void ShowInfo(string message)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			ShowMessage(message, Color.get_White(), 2500, GameService.Content.get_DefaultFont18());
		}

		protected override void Unload()
		{
			Panel mainPanel = MainPanel;
			if (mainPanel != null)
			{
				((Container)mainPanel).get_Children()?.ToList().ForEach(delegate(Control c)
				{
					if (c != null)
					{
						c.Dispose();
					}
				});
			}
			Panel mainPanel2 = MainPanel;
			if (mainPanel2 != null)
			{
				((Container)mainPanel2).get_Children()?.Clear();
			}
			Panel mainPanel3 = MainPanel;
			if (mainPanel3 != null)
			{
				((Control)mainPanel3).Dispose();
			}
			MainPanel = null;
		}
	}
}
