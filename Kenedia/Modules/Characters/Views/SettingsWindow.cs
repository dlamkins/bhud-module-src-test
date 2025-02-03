using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Characters.Extensions;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Views
{
	public class SettingsWindow : BaseSettingsWindow
	{
		private Kenedia.Modules.Core.Controls.Label _customFontSizeLabel;

		private Kenedia.Modules.Core.Controls.Dropdown _customFontSize;

		private Kenedia.Modules.Core.Controls.Label _customNameFontSizeLabel;

		private Kenedia.Modules.Core.Controls.Dropdown _customNameFontSize;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

		private readonly SharedSettingsView _sharedSettingsView;

		private readonly OCR _ocr;

		private readonly Settings _settings;

		private double _tick;

		public SettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, SharedSettingsView sharedSettingsView, OCR ocr, Settings settings)
			: base(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			_sharedSettingsView = sharedSettingsView;
			_ocr = ocr;
			_settings = settings;
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Width = base.ContentRegion.Width,
				Height = base.ContentRegion.Height,
				ControlPadding = new Vector2(0f, 10f),
				CanScroll = true
			};
			base.SubWindowEmblem = AsyncTexture2D.FromAssetId(156027);
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156015);
			base.Name = string.Format(strings.ItemSettings, BaseModule<Characters, MainWindow, Settings, PathCollection>.ModuleName ?? "");
			CreateOCR();
			CreateAppearance();
			CreateBehavior();
			CreateRadial();
			CreateDelays();
			CreateGeneral();
			CreateKeybinds();
			GameService.Overlay.UserLocale.SettingChanged += OnLanguageChanged;
			OnLanguageChanged();
		}

		private void CreateRadial()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_051b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0528: Unknown result type (might be due to invalid IL or missing references)
			//IL_053e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_057c: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0638: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Unknown result type (might be due to invalid IL or missing references)
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Unknown result type (might be due to invalid IL or missing references)
			//IL_0778: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0872: Unknown result type (might be due to invalid IL or missing references)
			//IL_087f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0895: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = AsyncTexture2D.FromAssetId(157122),
				SetLocalizedTitle = () => strings.RadialMenuSettings,
				SetLocalizedTitleTooltip = () => strings.RadialMenuSettings_Tooltip
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(10f)
			};
			Kenedia.Modules.Core.Controls.FlowPanel settingsFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				HeightSizingMode = SizingMode.AutoSize,
				Width = base.ContentRegion.Width - 20,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f),
				OuterControlPadding = new Vector2(5f)
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				Checked = _settings.EnableRadialMenu.Value,
				SetLocalizedText = () => strings.EnableRadialMenu,
				SetLocalizedTooltip = () => strings.EnableRadialMenu_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.EnableRadialMenu.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				SetLocalizedText = () => strings.Radial_ShowAdvancedTooltip,
				SetLocalizedTooltip = () => strings.Radial_ShowAdvancedTooltip_Tooltip,
				Checked = _settings.Radial_ShowAdvancedTooltip.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.Radial_ShowAdvancedTooltip.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				SetLocalizedText = () => strings.Radial_UseProfessionColor,
				SetLocalizedTooltip = () => strings.Radial_UseProfessionColor_Tooltip,
				Checked = _settings.Radial_UseProfessionColor.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.Radial_UseProfessionColor.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				SetLocalizedText = () => strings.Radial_UseProfessionIcons,
				SetLocalizedTooltip = () => strings.Radial_UseProfessionIcons_Tooltip,
				Checked = _settings.Radial_UseProfessionIcons.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.Radial_UseProfessionIcons.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				SetLocalizedText = () => strings.Radial_UseProfessionIconsColor,
				SetLocalizedTooltip = () => strings.Radial_UseProfessionIconsColor_Tooltip,
				Checked = _settings.Radial_UseProfessionIconsColor.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.Radial_UseProfessionIconsColor.Value = b;
				}
			};
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = settingsFlowPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label scaleLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				SetLocalizedText = () => string.Format(strings.Radial_Scale + " {0}%", _settings.Radial_Scale.Value * 100f),
				SetLocalizedTooltip = () => strings.Radial_Scale_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Parent = subP,
				SetLocalizedTooltip = () => strings.Radial_Scale_Tooltip,
				Value = _settings.Radial_Scale.Value * 100f,
				ValueChangedAction = delegate(int v)
				{
					_settings.Radial_Scale.Value = (float)v / 100f;
					scaleLabel.UserLocale_SettingChanged(_settings.Radial_Scale.Value, null);
				},
				MinValue = 0f,
				MaxValue = 100f,
				Location = new Point(250, 0)
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = settingsFlowPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Location = new Point(30, 0),
				SetLocalizedText = () => strings.Radial_IdleBackgroundColor
			};
			Kenedia.Modules.Core.Controls.Panel idleBackgroundPreview = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = subP,
				Location = new Point(0, 0),
				Size = new Point(20),
				BackgroundColor = _settings.Radial_IdleColor.Value
			};
			new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = subP,
				Location = new Point(250, 0),
				Text = _settings.Radial_IdleColor.Value.ToHex(),
				Width = base.ContentRegion.Width - 20 - 250,
				TextChangedAction = delegate(string t)
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					if (t.ColorFromHex(out var outColor4))
					{
						_settings.Radial_IdleColor.Value = outColor4;
						idleBackgroundPreview.BackgroundColor = outColor4;
					}
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = settingsFlowPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Location = new Point(30, 0),
				SetLocalizedText = () => strings.Radial_IdleBorderColor
			};
			Kenedia.Modules.Core.Controls.Panel idleBorderPreview = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = subP,
				Location = new Point(0, 0),
				Size = new Point(20),
				BackgroundColor = _settings.Radial_IdleBorderColor.Value
			};
			new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = subP,
				Location = new Point(250, 0),
				Text = _settings.Radial_IdleBorderColor.Value.ToHex(),
				Width = base.ContentRegion.Width - 20 - 250,
				TextChangedAction = delegate(string t)
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					if (t.ColorFromHex(out var outColor3))
					{
						_settings.Radial_IdleBorderColor.Value = outColor3;
						idleBorderPreview.BackgroundColor = outColor3;
					}
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = settingsFlowPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Location = new Point(30, 0),
				SetLocalizedText = () => strings.Radial_HoveredBackgroundColor
			};
			Kenedia.Modules.Core.Controls.Panel activeBackgroundPreview = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = subP,
				Location = new Point(0, 0),
				Size = new Point(20),
				BackgroundColor = _settings.Radial_HoveredColor.Value
			};
			new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = subP,
				Location = new Point(250, 0),
				Text = _settings.Radial_HoveredColor.Value.ToHex(),
				Width = base.ContentRegion.Width - 20 - 250,
				TextChangedAction = delegate(string t)
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					if (t.ColorFromHex(out var outColor2))
					{
						_settings.Radial_HoveredColor.Value = outColor2;
						activeBackgroundPreview.BackgroundColor = outColor2;
					}
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = settingsFlowPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Location = new Point(30, 0),
				SetLocalizedText = () => strings.Radial_HoveredBorderColor
			};
			Kenedia.Modules.Core.Controls.Panel activeBorderPreview = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = subP,
				Location = new Point(0, 0),
				Size = new Point(20),
				BackgroundColor = _settings.Radial_HoveredBorderColor.Value
			};
			new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = subP,
				Location = new Point(250, 0),
				Text = _settings.Radial_HoveredBorderColor.Value.ToHex(),
				Width = base.ContentRegion.Width - 20 - 250,
				TextChangedAction = delegate(string t)
				{
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					if (t.ColorFromHex(out var outColor))
					{
						_settings.Radial_HoveredBorderColor.Value = outColor;
						activeBorderPreview.BackgroundColor = outColor;
					}
				}
			};
		}

		private void CreateOCR()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = AsyncTexture2D.FromAssetId(759447),
				SetLocalizedTitle = () => strings.OCRAndImageRecognition,
				SetLocalizedTitleTooltip = () => strings.OCRAndImageRecognition_Tooltip
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(10f)
			};
			Kenedia.Modules.Core.Controls.FlowPanel settingsFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = contentFlowPanel,
				HeightSizingMode = SizingMode.AutoSize,
				Width = (base.ContentRegion.Width - 20) / 2,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f),
				OuterControlPadding = new Vector2(5f)
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				Checked = _settings.UseOCR.Value,
				SetLocalizedText = () => strings.UseOCR,
				SetLocalizedTooltip = () => strings.UseOCR_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.UseOCR.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = settingsFlowPanel,
				Checked = _settings.UseBetaGamestate.Value,
				SetLocalizedText = () => strings.UseBetaGameState,
				SetLocalizedTooltip = () => strings.UseBetaGameState_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.UseBetaGamestate.Value = b;
				}
			};
			Kenedia.Modules.Core.Controls.FlowPanel buttonFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				Location = new Point(settingsFlowPanel.Right, 0),
				HeightSizingMode = SizingMode.AutoSize,
				Width = (base.ContentRegion.Width - 20) / 2,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f),
				OuterControlPadding = new Vector2(5f)
			};
			new Button
			{
				Parent = buttonFlowPanel,
				SetLocalizedText = () => strings.EditOCR,
				SetLocalizedTooltip = () => strings.EditOCR_Tooltip,
				Width = buttonFlowPanel.Width - 15,
				Height = 40,
				ClickAction = new Action(_ocr.ToggleContainer)
			};
			_sharedSettingsView.CreateLayout(contentFlowPanel, base.ContentRegion.Width - 20);
		}

		private void CreateKeybinds()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				SetLocalizedTitle = () => strings.Keybinds,
				TitleIcon = AsyncTexture2D.FromAssetId(156734)
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				Location = new Point(5),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = cP,
				Width = base.ContentRegion.Width - 35,
				KeyBinding = _settings.LogoutKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					_settings.LogoutKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.Logout,
				SetLocalizedTooltip = () => strings.LogoutDescription
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = cP,
				Width = base.ContentRegion.Width - 35,
				KeyBinding = _settings.ShortcutKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					_settings.ShortcutKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.ShortcutToggle,
				SetLocalizedTooltip = () => strings.ShortcutToggle_Tooltip
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = cP,
				Width = base.ContentRegion.Width - 35,
				KeyBinding = _settings.RadialKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					_settings.RadialKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.RadialMenuKey,
				SetLocalizedTooltip = () => strings.RadialMenuKey_Tooltip
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = cP,
				Width = base.ContentRegion.Width - 35,
				KeyBinding = _settings.InventoryKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					_settings.InventoryKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.InventoryKey,
				SetLocalizedTooltip = () => strings.InventoryKey_Tooltip
			};
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = cP,
				Width = base.ContentRegion.Width - 35,
				KeyBinding = _settings.MailKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					_settings.MailKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => strings.MailKey,
				SetLocalizedTooltip = () => strings.MailKey_Tooltip
			};
		}

		private void CreateBehavior()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0704: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				SetLocalizedTitle = () => strings.ModuleBehavior,
				SetLocalizedTitleTooltip = () => strings.ModuleBehavior_Tooltip,
				TitleIcon = AsyncTexture2D.FromAssetId(60968)
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				Location = new Point(5, 5),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.OnlyEnterOnExact.Value,
				SetLocalizedText = () => strings.OnlyEnterOnExact,
				SetLocalizedTooltip = () => strings.OnlyEnterOnExact_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.OnlyEnterOnExact.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.EnterOnSwap.Value,
				SetLocalizedText = () => strings.EnterOnSwap,
				SetLocalizedTooltip = () => strings.EnterOnSwap_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.EnterOnSwap.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.OpenInventoryOnEnter.Value,
				SetLocalizedText = () => strings.OpenInventoryOnEnter,
				SetLocalizedTooltip = () => strings.OpenInventoryOnEnter_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.OpenInventoryOnEnter.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.CloseWindowOnSwap.Value,
				SetLocalizedText = () => strings.CloseWindowOnSwap,
				SetLocalizedTooltip = () => strings.CloseWindowOnSwap_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.CloseWindowOnSwap.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.DoubleClickToEnter.Value,
				SetLocalizedText = () => strings.DoubleClickToEnter,
				SetLocalizedTooltip = () => strings.DoubleClickToEnter_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.DoubleClickToEnter.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.EnterToLogin.Value,
				SetLocalizedText = () => strings.EnterToLogin,
				SetLocalizedTooltip = () => strings.EnterToLogin_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.EnterToLogin.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.AutoSortCharacters.Value,
				SetLocalizedText = () => strings.AutoFix,
				SetLocalizedTooltip = () => strings.AutoFix_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.AutoSortCharacters.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.FilterDiacriticsInsensitive.Value,
				SetLocalizedText = () => strings.FilterDiacriticsInsensitive,
				SetLocalizedTooltip = () => strings.FilterDiacriticsInsensitive_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.FilterDiacriticsInsensitive.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.AutomaticCharacterDelete.Value,
				SetLocalizedText = () => strings.AutomaticCharacterDelete,
				SetLocalizedTooltip = () => strings.AutomaticCharacterDelete_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.AutomaticCharacterDelete.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.CancelOnlyOnESC.Value,
				SetLocalizedText = () => strings.CancelOnlyOnESC,
				SetLocalizedTooltip = () => strings.CancelOnlyOnESC_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.CancelOnlyOnESC.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.FilterAsOne.Value,
				SetLocalizedText = () => strings.FilterAsOne,
				SetLocalizedTooltip = () => strings.FilterAsOne_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.FilterAsOne.Value = b;
				}
			};
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label checkDistanceLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.CheckDistance, _settings.CheckDistance.Value),
				SetLocalizedTooltip = () => strings.CheckDistance_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 0f,
				MaxValue = 72f,
				Value = _settings.CheckDistance.Value,
				Width = base.ContentRegion.Width - 35 - 225,
				SetLocalizedTooltip = () => strings.CheckDistance_Tooltip,
				ValueChangedAction = delegate(int num)
				{
					_settings.CheckDistance.Value = num;
					checkDistanceLabel.UserLocale_SettingChanged(null, null);
				}
			};
		}

		private void CreateAppearance()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0664: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_0753: Unknown result type (might be due to invalid IL or missing references)
			//IL_0775: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = AsyncTexture2D.FromAssetId(156740),
				SetLocalizedTitle = () => strings.Appearance
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				Location = new Point(5, 5),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowCornerIcon.Value,
				SetLocalizedText = () => strings.ShowCorner_Name,
				SetLocalizedTooltip = () => string.Format(strings.ShowCorner_Tooltip, BaseModule<Characters, MainWindow, Settings, PathCollection>.ModuleName),
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowCornerIcon.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowRandomButton.Value,
				SetLocalizedText = () => strings.ShowRandomButton_Name,
				SetLocalizedTooltip = () => strings.ShowRandomButton_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowRandomButton.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowLastButton.Value,
				SetLocalizedText = () => strings.ShowLastButton,
				SetLocalizedTooltip = () => strings.ShowLastButton_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowLastButton.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowDetailedTooltip.Value,
				SetLocalizedText = () => string.Format(strings.ShowItem, strings.DetailedTooltip),
				SetLocalizedTooltip = () => strings.DetailedTooltip_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowDetailedTooltip.Value = b;
				}
			};
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 30,
				SetLocalizedText = () => strings.CharacterDisplaySize
			};
			new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = subP,
				SetLocalizedItems = () => new List<string>
				{
					strings.Small,
					strings.Normal,
					strings.Large,
					strings.Custom
				},
				SelectedItem = _settings.PanelSize.Value.GetPanelSize(),
				ValueChangedAction = delegate(string b)
				{
					_settings.PanelSize.Value = b.GetPanelSize();
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 30,
				SetLocalizedText = () => strings.CharacterDisplayOption
			};
			new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = subP,
				Location = new Point(250, 0),
				SetLocalizedItems = () => new List<string>
				{
					strings.OnlyText,
					strings.OnlyIcons,
					strings.TextAndIcon
				},
				SelectedItem = _settings.PanelLayout.Value.GetPanelLayout(),
				ValueChangedAction = delegate(string b)
				{
					_settings.PanelLayout.Value = b.GetPanelLayout();
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			_customFontSizeLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				SetLocalizedText = () => string.Format(strings.FontSize, _settings.CustomCharacterFontSize.Value)
			};
			_customFontSize = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = subP,
				Location = new Point(250, 0),
				SelectedItem = _settings.CustomCharacterFontSize.Value.ToString(),
				ValueChangedAction = delegate(string str)
				{
					GetFontSize(_settings.CustomCharacterFontSize, str);
					_customFontSizeLabel.UserLocale_SettingChanged(null, null);
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			_customNameFontSizeLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				SetLocalizedText = () => string.Format(strings.NameFontSize, _settings.CustomCharacterNameFontSize.Value)
			};
			_customNameFontSize = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = subP,
				Location = new Point(250, 0),
				SelectedItem = _settings.CustomCharacterNameFontSize.Value.ToString(),
				ValueChangedAction = delegate(string str)
				{
					GetFontSize(_settings.CustomCharacterNameFontSize, str);
					_customNameFontSizeLabel.UserLocale_SettingChanged(null, null);
				}
			};
			foreach (object i in Enum.GetValues(typeof(ContentService.FontSize)))
			{
				_customFontSize.Items.Add($"{(int)i}");
				_customNameFontSize.Items.Add($"{(int)i}");
			}
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			Kenedia.Modules.Core.Controls.Label iconSizeLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				SetLocalizedText = () => string.Format(strings.IconSize, _settings.CustomCharacterIconSize.Value)
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Parent = subP,
				Location = new Point(250, 0),
				MinValue = 8f,
				MaxValue = 256f,
				Width = base.ContentRegion.Width - 35 - 250,
				Value = _settings.CustomCharacterIconSize.Value,
				ValueChangedAction = delegate(int num)
				{
					_settings.CustomCharacterIconSize.Value = num;
					iconSizeLabel.UserLocale_SettingChanged(num, null);
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				Height = 30
			};
			Kenedia.Modules.Core.Controls.Checkbox cardWidthCheckbox = new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = subP,
				Checked = _settings.CharacterPanelFixedWidth.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.CharacterPanelFixedWidth.Value = b;
				},
				SetLocalizedText = () => string.Format(strings.CardWith, _settings.CharacterPanelWidth.Value),
				SetLocalizedTooltip = () => string.Format(strings.CardWidth_Tooltip, _settings.CharacterPanelWidth.Value)
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Parent = subP,
				Location = new Point(250, 0),
				MinValue = 25f,
				MaxValue = 750f,
				Width = base.ContentRegion.Width - 35 - 250,
				Value = _settings.CharacterPanelWidth.Value,
				ValueChangedAction = delegate(int num)
				{
					_settings.CharacterPanelWidth.Value = num;
					cardWidthCheckbox.UserLocale_SettingChanged(null, null);
				}
			};
		}

		private void CreateDelays()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				SetLocalizedTitle = () => strings.Delays,
				TitleIcon = AsyncTexture2D.FromAssetId(155035)
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				Location = new Point(5),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			Kenedia.Modules.Core.Controls.Panel subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label keyDelayLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.KeyDelay, _settings.KeyDelay.Value),
				SetLocalizedTooltip = () => strings.KeyDelay_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 0f,
				MaxValue = 500f,
				Value = _settings.KeyDelay.Value,
				Width = base.ContentRegion.Width - 35 - 225,
				ValueChangedAction = delegate(int num)
				{
					_settings.KeyDelay.Value = num;
					keyDelayLabel.UserLocale_SettingChanged(null, null);
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label filterDelayLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.FilterDelay, _settings.FilterDelay.Value),
				SetLocalizedTooltip = () => strings.FilterDelay_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 0f,
				MaxValue = 500f,
				Value = _settings.FilterDelay.Value,
				Width = base.ContentRegion.Width - 35 - 225,
				ValueChangedAction = delegate(int num)
				{
					_settings.FilterDelay.Value = num;
					filterDelayLabel.UserLocale_SettingChanged(num, null);
				}
			};
			subP = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = cP,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Kenedia.Modules.Core.Controls.Label swapDelayLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = subP,
				AutoSizeWidth = true,
				Height = 20,
				SetLocalizedText = () => string.Format(strings.SwapDelay, _settings.SwapDelay.Value),
				SetLocalizedTooltip = () => strings.SwapDelay_Tooltip
			};
			new Kenedia.Modules.Core.Controls.TrackBar
			{
				Location = new Point(225, 2),
				Parent = subP,
				MinValue = 500f,
				MaxValue = 30000f,
				Value = _settings.SwapDelay.Value,
				Width = base.ContentRegion.Width - 35 - 225,
				ValueChangedAction = delegate(int num)
				{
					_settings.SwapDelay.Value = num;
					swapDelayLabel.UserLocale_SettingChanged(null, null);
				}
			};
		}

		private void CreateGeneral()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel p = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				SetLocalizedTitle = () => strings.GeneralAndWindows,
				TitleIcon = AsyncTexture2D.FromAssetId(157109)
			};
			Kenedia.Modules.Core.Controls.FlowPanel cP = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = p,
				Location = new Point(5),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(3f, 3f)
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.LoadCachedAccounts.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.LoadCachedAccounts.Value = b;
				},
				SetLocalizedText = () => strings.LoadCachedAccounts,
				SetLocalizedTooltip = () => strings.LoadCachedAccounts_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowStatusWindow.Value,
				SetLocalizedText = () => strings.ShowStatusWindow_Name,
				SetLocalizedTooltip = () => strings.ShowStatusWindow_Tooltip,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowStatusWindow.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowChoyaSpinner.Value,
				SetLocalizedText = () => strings.ShowChoyaSpinner,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowChoyaSpinner.Value = b;
				}
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.OpenSidemenuOnSearch.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.OpenSidemenuOnSearch.Value = b;
				},
				SetLocalizedText = () => strings.OpenSidemenuOnSearch,
				SetLocalizedTooltip = () => strings.OpenSidemenuOnSearch_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.FocusSearchOnShow.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.FocusSearchOnShow.Value = b;
				},
				SetLocalizedText = () => strings.FocusSearchOnShow,
				SetLocalizedTooltip = () => strings.FocusSearchOnShow_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.ShowNotifications.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.ShowNotifications.Value = b;
				},
				SetLocalizedText = () => strings.ShowNotifications,
				SetLocalizedTooltip = () => strings.ShowNotifications_Tooltip
			};
			new Kenedia.Modules.Core.Controls.Checkbox
			{
				Parent = cP,
				Checked = _settings.DebugMode.Value,
				CheckedChangedAction = delegate(bool b)
				{
					_settings.DebugMode.Value = b;
				},
				SetLocalizedText = () => strings.DebugMode_Name,
				SetLocalizedTooltip = () => strings.DebugMode_Tooltip
			};
		}

		private void GetFontSize(SettingEntry<int> setting, string item)
		{
			if (int.TryParse(item, out var fontSize))
			{
				setting.Value = fontSize;
			}
		}

		public void OnLanguageChanged(object s = null, EventArgs e = null)
		{
			base.Name = string.Format(strings.ItemSettings, BaseModule<Characters, MainWindow, Settings, PathCollection>.ModuleName ?? "");
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick >= 1000.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus)
				{
					_sharedSettingsView?.UpdateOffset();
				}
			}
		}
	}
}
