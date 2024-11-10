using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class SettingsView : View
	{
		public Settings Settings { get; }

		public SettingsView(Settings settings)
		{
			Settings = settings;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			base.Build(buildPanel);
			Kenedia.Modules.Core.Controls.FlowPanel p = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = buildPanel,
				Location = new Point(50, 0),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill,
				ContentPadding = new RectangleDimensions(10, 10),
				ControlPadding = new Vector2(10f, 5f),
				FlowDirection = ControlFlowDirection.TopToBottom
			};
			Kenedia.Modules.Core.Controls.FlowPanel fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Title = "General",
				Parent = p,
				Location = new Point(50, 0),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(0, 20),
				ContentPadding = new RectangleDimensions(10, 10),
				ControlPadding = new Vector2(10f, 5f),
				FlowDirection = ControlFlowDirection.TopToBottom
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.AutoSetFilterProfession.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.AutoSetFilterProfession.Value = b;
				},
				SetLocalizedText = () => strings.AutoSetProfession_Name,
				SetLocalizedTooltip = () => strings.AutoSetProfession_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.AutoSetFilterSpecialization.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.AutoSetFilterSpecialization.Value = b;
				},
				SetLocalizedText = () => strings.AutoSetFilterSpecialization_Name,
				SetLocalizedTooltip = () => strings.AutoSetFilterSpecialization_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.ShowCornerIcon.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.ShowCornerIcon.Value = b;
				},
				SetLocalizedText = () => string.Format(strings_common.ShowCornerIcon, BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.ModuleName),
				SetLocalizedTooltip = () => strings_common.ShowCornerIcon_ttp
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.RequireVisibleTemplate.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.RequireVisibleTemplate.Value = b;
				},
				SetLocalizedText = () => string.Format(strings.RequireVisibleTemplate, BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.ModuleName),
				SetLocalizedTooltip = () => strings.RequireVisibleTemplate_Tooltip
			};
			Kenedia.Modules.Core.Controls.Checkbox setFilterOnTemplateCreate = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.SetFilterOnTemplateCreate.Value,
				SetLocalizedText = () => string.Format(strings.SetFilterOnTemplateCreate, BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.ModuleName),
				SetLocalizedTooltip = () => strings.SetFilterOnTemplateCreate_Tooltip
			};
			Kenedia.Modules.Core.Controls.Checkbox resetFilterOnTemplateCreate = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.ResetFilterOnTemplateCreate.Value,
				SetLocalizedText = () => string.Format(strings.ResetFilterOnTemplateCreate, BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.ModuleName),
				SetLocalizedTooltip = () => strings.ResetFilterOnTemplateCreate_Tooltip
			};
			setFilterOnTemplateCreate.CheckedChangedAction = delegate(bool b)
			{
				Settings.SetFilterOnTemplateCreate.Value = b;
				if (Settings.ResetFilterOnTemplateCreate.Value && b)
				{
					resetFilterOnTemplateCreate.Checked = false;
				}
			};
			resetFilterOnTemplateCreate.CheckedChangedAction = delegate(bool b)
			{
				Settings.ResetFilterOnTemplateCreate.Value = b;
				if (Settings.SetFilterOnTemplateCreate.Value && b)
				{
					setFilterOnTemplateCreate.Checked = false;
				}
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = fp,
				Width = 300,
				KeyBinding = Settings.ToggleWindowKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					Settings.ToggleWindowKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => string.Format(strings_common.ToggleItem, BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.ModuleName)
			};
			fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Title = "Quick Filter Panel",
				Parent = p,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize,
				AutoSizePadding = new Point(0, 20),
				ContentPadding = new RectangleDimensions(10, 10),
				ControlPadding = new Vector2(10f, 5f),
				FlowDirection = ControlFlowDirection.TopToBottom
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.QuickFiltersPanelFade.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.QuickFiltersPanelFade.Value = b;
				},
				SetLocalizedText = () => strings.FadeQuickFiltersPanel_Name,
				SetLocalizedTooltip = () => strings.FadeQuickFiltersPanel_Tooltip
			};
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = fp,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label fadeDelayLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.QuickFiltersPanelFadeDelay_Name, Settings.QuickFiltersPanelFadeDelay.Value),
				SetLocalizedTooltip = () => strings.QuickFiltersPanelFadeDelay_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 0f,
				MaxValue = 240f,
				SmallStep = false,
				Value = (float)(Settings.QuickFiltersPanelFadeDelay.Value / 0.25),
				Width = 300,
				ValueChangedAction = delegate(int num)
				{
					Settings.QuickFiltersPanelFadeDelay.Value = (double)num * 0.25;
					fadeDelayLabel.UserLocale_SettingChanged(null, null);
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = fp,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label fadeDurationLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.QuickFiltersPanelFadeDuration_Name, Settings.QuickFiltersPanelFadeDuration.Value),
				SetLocalizedTooltip = () => strings.QuickFiltersPanelFadeDuration_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 0f,
				MaxValue = 100f,
				Value = (float)(Settings.QuickFiltersPanelFadeDuration.Value / 25.0),
				Width = 300,
				ValueChangedAction = delegate(int num)
				{
					Settings.QuickFiltersPanelFadeDuration.Value = num * 25;
					fadeDurationLabel.UserLocale_SettingChanged(null, null);
				}
			};
			new Dummy
			{
				Height = 10,
				Parent = fp
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.ShowQuickFilterPanelOnWindowOpen.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.ShowQuickFilterPanelOnWindowOpen.Value = b;
				},
				SetLocalizedText = () => strings.ShowQuickFilterPanelOnWindowOpen_Name,
				SetLocalizedTooltip = () => strings.ShowQuickFilterPanelOnWindowOpen_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = fp,
				Checked = Settings.ShowQuickFilterPanelOnTabOpen.Value,
				CheckedChangedAction = delegate(bool b)
				{
					Settings.ShowQuickFilterPanelOnTabOpen.Value = b;
				},
				SetLocalizedText = () => strings.ShowQuickFilterPanelOnTabOpen_Name,
				SetLocalizedTooltip = () => strings.ShowQuickFilterPanelOnTabOpen_Tooltip
			};
		}
	}
}
