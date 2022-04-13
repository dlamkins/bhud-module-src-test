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
using Estreya.BlishHUD.EventTable.Extensions;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.UI.Views.Settings.Controls;
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

		protected Panel RenderSetting<T>(Panel parent, SettingEntry<T> setting)
		{
			Panel panel = GetPanel((Container)(object)parent);
			Label label = GetLabel(panel, ((SettingEntry)setting).get_DisplayName());
			((SettingEntry)setting).get_SettingType();
			try
			{
				Control obj = ControlProvider.Create<T>(setting, HandleValidation, 170, -1, ((Control)label).get_Right() + 20, 0);
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

		protected Label GetLabel(Panel parent, string text)
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
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)panel);
			val.set_Text(text);
			((Control)val).set_Width((int)EventTableModule.ModuleInstance.Font.MeasureString(text).Width);
			((Control)val).set_Enabled(disabledCallback == null || !disabledCallback());
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Task.Run(async delegate
				{
					try
					{
						await action();
					}
					catch (Exception ex)
					{
						ShowError(ex.Message);
					}
				});
			});
		}

		protected void RenderLabel(Panel parent, string title, string value)
		{
			Panel panel = GetPanel((Container)(object)parent);
			Label titleLabel = GetLabel(panel, title);
			((Control)GetLabel(panel, value)).set_Left(((Control)titleLabel).get_Right() + 20);
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
