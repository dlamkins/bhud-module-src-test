using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Estreya.BlishHUD.EventTable.Resources;
using Estreya.BlishHUD.EventTable.UI.Views.Controls;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.EventTable.UI.Views
{
	public abstract class BaseView : View
	{
		private const int LEFT_PADDING = 20;

		private const int CONTROL_X_SPACING = 20;

		private const int LABEL_WIDTH = 250;

		private const int BINDING_WIDTH = 170;

		private static readonly Logger Logger = Logger.GetLogger<BaseView>();

		private CancellationTokenSource ErrorCancellationTokenSource = new CancellationTokenSource();

		private static IEnumerable<Color> Colors { get; set; }

		private static Panel ColorPickerPanel { get; set; }

		private static ColorPicker ColorPicker { get; set; }

		private Container BuildPanel { get; set; }

		private Panel ErrorPanel { get; set; }

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
					if (EventTableModule.ModuleInstance.ModuleSettings.DefaultGW2Color != null)
					{
						Logger.Debug("Adding default color: " + EventTableModule.ModuleInstance.ModuleSettings.DefaultGW2Color.get_Name());
						Colors = new List<Color> { EventTableModule.ModuleInstance.ModuleSettings.DefaultGW2Color };
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

		protected sealed override void Build(Container buildPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			Rectangle bounds = buildPanel.get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Size(((Rectangle)(ref bounds)).get_Size());
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)2);
			((Container)val).set_AutoSizePadding(new Point(15, 15));
			((Control)val).set_Parent(buildPanel);
			Panel parentPanel = (Panel)(object)(BuildPanel = (Container)val);
			RegisterErrorPanel(buildPanel);
			InternalBuild(parentPanel);
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

		protected Panel RenderProperty<TObject, TProperty>(Panel parent, TObject obj, Expression<Func<TObject, TProperty>> expression, Func<TObject, bool> isEnabled, (float Min, float Max)? range = null, string title = null, int width = -1)
		{
			return RenderPropertyWithValidation(parent, obj, expression, isEnabled, null, range, title, width);
		}

		protected Panel RenderPropertyWithValidation<TObject, TProperty>(Panel parent, TObject obj, Expression<Func<TObject, TProperty>> expression, Func<TObject, bool> isEnabled, Func<TProperty, (bool Valid, string Message)> validationFunction, (float Min, float Max)? range = null, string title = null, int width = -1)
		{
			return RenderPropertyWithChangedTypeValidation(parent, obj, expression, isEnabled, validationFunction, range, title, width);
		}

		protected Panel RenderPropertyWithChangedTypeValidation<TObject, TProperty, TOverrideType>(Panel parent, TObject obj, Expression<Func<TObject, TProperty>> expression, Func<TObject, bool> isEnabled, Func<TOverrideType, (bool Valid, string Message)> validationFunction, (float Min, float Max)? range = null, string title = null, int width = -1)
		{
			Panel panel = GetPanel((Container)(object)parent);
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression != null)
			{
				PropertyInfo property = memberExpression.Member as PropertyInfo;
				if ((object)property != null && title == null)
				{
					title = property.Name;
				}
			}
			Label label = GetLabel(panel, title ?? string.Empty);
			try
			{
				ControlHandler.CreateFromPropertyWithChangedTypeValidation(obj, expression, isEnabled, delegate(TOverrideType val)
				{
					(bool, string) tuple = ((validationFunction != null) ? validationFunction(val) : (true, null));
					if (!tuple.Item1)
					{
						ShowError(tuple.Item2);
					}
					return tuple.Item1;
				}, range, (width == -1) ? 170 : width, -1, ((Control)label).get_Right() + 20, 0).set_Parent((Container)(object)panel);
				return panel;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Type \"" + typeof(TProperty).FullName + "\" with override \"" + typeof(TOverrideType).FullName + "\" could not be found in internal type lookup:");
				return panel;
			}
		}

		protected Panel RenderTextbox(Panel parent, string description, string placeholder, Action<string> onEnterAction)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Panel panel = GetPanel((Container)(object)parent);
			Label label = GetLabel(panel, description);
			try
			{
				TextBox textBox = (TextBox)ControlHandler.Create<string>(170, -1, ((Control)label).get_Right() + 20, 0);
				((Control)textBox).set_Parent((Container)(object)panel);
				((Control)textBox).set_BasicTooltipText(description);
				((TextInputBase)textBox).set_PlaceholderText(placeholder);
				textBox.add_EnterPressed((EventHandler<EventArgs>)delegate
				{
					onEnterAction?.Invoke(((TextInputBase)textBox).get_Text());
					((TextInputBase)textBox).set_Text(string.Empty);
				});
				return panel;
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Type \"" + typeof(string).FullName + "\" could not be found in internal type lookup:");
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

		protected StandardButton RenderButton(Panel parent, string text, Action action, Func<bool> disabledCallback = null)
		{
			return RenderButton(parent, text, delegate
			{
				action();
				return Task.CompletedTask;
			}, disabledCallback);
		}

		protected StandardButton RenderButton(Panel parent, string text, Func<Task> action, Func<bool> disabledCallback = null)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)panel);
			val.set_Text(text);
			((Control)val).set_Enabled(disabledCallback == null || !disabledCallback());
			StandardButton button = val;
			int measuredWidth = (int)EventTableModule.ModuleInstance.Font.MeasureString(text).Width + 10;
			if (((Control)button).get_Width() < measuredWidth)
			{
				((Control)button).set_Width(measuredWidth);
			}
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
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
			return button;
		}

		protected void RenderLabel(Panel parent, string title, string value = null, Color? textColorTitle = null, Color? textColorValue = null)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = GetPanel((Container)(object)parent);
			Label titleLabel = GetLabel(panel, title);
			titleLabel.set_TextColor((Color)(((_003F?)textColorTitle) ?? titleLabel.get_TextColor()));
			if (value != null)
			{
				Label valueLabel = GetLabel(panel, value);
				((Control)valueLabel).set_Left(((Control)titleLabel).get_Right() + 20);
				valueLabel.set_TextColor((Color)(((_003F?)textColorValue) ?? valueLabel.get_TextColor()));
			}
			else
			{
				titleLabel.set_AutoSizeWidth(true);
			}
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

		protected override void Unload()
		{
			((View<IPresenter>)this).Unload();
			((Control)ErrorPanel).Dispose();
		}

		protected BaseView()
			: this()
		{
		}
	}
}
