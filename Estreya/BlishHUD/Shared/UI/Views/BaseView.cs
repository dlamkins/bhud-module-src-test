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
using Estreya.BlishHUD.Shared.State;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
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

		protected IconState IconState { get; }

		protected TranslationState TranslationState { get; }

		protected Panel MainPanel { get; private set; }

		public BaseView(Gw2ApiManager apiManager, IconState iconState, TranslationState translationState, BitmapFont font = null)
			: this()
		{
			APIManager = apiManager;
			IconState = iconState;
			TranslationState = translationState;
			Font = font ?? GameService.Content.get_DefaultFont16();
		}

		protected sealed override async Task<bool> Load(IProgress<string> progress)
		{
			if (Colors == null)
			{
				progress.Report(TranslationState.GetTranslation("baseView-loadingColors", "Loading Colors..."));
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
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			Rectangle bounds = buildPanel.get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			((Container)val).set_AutoSizePadding(new Point(15, 15));
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

		protected TextBox RenderTextbox(Panel parent, Point location, int width, string value, string placeholder, Action<string> onChangeAction = null, Action<string> onEnterAction = null, bool clearOnEnter = false)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
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
					TextBox val3 = (TextBox)((s is TextBox) ? s : null);
					onChangeAction?.Invoke(((TextInputBase)val3).get_Text());
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

		protected TrackBar RenderTrackBar(Panel parent, Point location, int width, int value, (int Min, int Max)? range = null, Action<int> onChangeAction = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
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

		protected TrackBar RenderTrackBar(Panel parent, Point location, int width, float value, (float Min, float Max)? range = null, Action<float> onChangeAction = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
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

		protected Checkbox RenderCheckbox(Panel parent, Point location, bool value, Action<bool> onChangeAction = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Checked(value);
			((Control)val).set_Location(location);
			Checkbox checkBox = val;
			if (onChangeAction != null)
			{
				checkBox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
				{
					Checkbox val2 = (Checkbox)((s is Checkbox) ? s : null);
					onChangeAction?.Invoke(val2.get_Checked());
				});
			}
			return checkBox;
		}

		protected Dropdown RenderDropdown(Panel parent, Point location, int width, string[] values, string value, Action<string> onChangeAction = null)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Parent((Container)(object)parent);
			((Control)val).set_Width(width);
			((Control)val).set_Location(location);
			Dropdown dropdown = val;
			if (values != null)
			{
				foreach (string valueToAdd in values)
				{
					dropdown.get_Items().Add(valueToAdd);
				}
				dropdown.set_SelectedItem(value);
			}
			if (onChangeAction != null)
			{
				dropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
				{
					Dropdown val2 = (Dropdown)((s is Dropdown) ? s : null);
					onChangeAction?.Invoke(val2.get_SelectedItem());
				});
			}
			return dropdown;
		}

		protected KeybindingAssigner RenderKeybinding(Panel parent, Point location, int width, KeyBinding value, Action<KeyBinding> onChangeAction = null)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text(text);
			val.set_Font(font ?? Font);
			val.set_TextColor((Color)(((_003F?)color) ?? Color.get_White()));
			val.set_AutoSizeHeight(!string.IsNullOrWhiteSpace(text));
			((Control)val).set_Width((int)Font.MeasureString(text).Width + 10);
			return val;
		}

		private StandardButton BuildButton(Panel parent, string text, Func<bool> disabledCallback = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)parent);
			val.set_Text(text);
			((Control)val).set_Enabled(disabledCallback == null || !disabledCallback());
			StandardButton button = val;
			int measuredWidth = (int)Font.MeasureString(text).Width + 10;
			if (((Control)button).get_Width() < measuredWidth)
			{
				((Control)button).set_Width(measuredWidth);
			}
			return button;
		}

		protected StandardButton RenderButton(Panel parent, string text, Action action, Func<bool> disabledCallback = null)
		{
			StandardButton obj = BuildButton(parent, text, disabledCallback);
			((Control)obj).add_Click((EventHandler<MouseEventArgs>)delegate
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
			return obj;
		}

		protected StandardButton RenderButtonAsync(Panel parent, string text, Func<Task> action, Func<bool> disabledCallback = null)
		{
			StandardButton button = BuildButton(parent, text, disabledCallback);
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

		protected (Label TitleLabel, Label ValueLabel) RenderLabel(Panel parent, string title, string value = null, Color? textColorTitle = null, Color? textColorValue = null)
		{
			Panel panel = GetPanel((Container)(object)parent);
			Label titleLabel = GetLabel(panel, title, textColorTitle);
			Label valueLabel = null;
			if (value != null)
			{
				valueLabel = GetLabel(panel, value, textColorValue);
				((Control)valueLabel).set_Left(((Control)titleLabel).get_Right() + CONTROL_X_SPACING);
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
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			_messageCancellationTokenSource?.Cancel();
			_messageCancellationTokenSource = new CancellationTokenSource();
			if (font == null)
			{
				font = Font;
			}
			Size2 textSize = font.MeasureString(message);
			Panel messagePanel = GetPanel((Container)(object)MainPanel);
			((Container)messagePanel).set_HeightSizingMode((SizingMode)0);
			((Control)messagePanel).set_Height((int)textSize.Height);
			((Container)messagePanel).set_WidthSizingMode((SizingMode)0);
			((Control)messagePanel).set_Width((int)textSize.Width + 10);
			((Control)messagePanel).set_Location(new Point(((Control)MainPanel).get_Width() / 2 - ((Control)messagePanel).get_Width() / 2, ((Control)MainPanel).get_Bottom() - ((Control)messagePanel).get_Height()));
			GetLabel(messagePanel, message, color, font);
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
			ShowMessage(message, Color.get_White(), 2500);
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
