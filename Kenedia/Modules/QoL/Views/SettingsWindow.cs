using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Views;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Kenedia.Modules.QoL.SubModules;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.QoL.Views
{
	public class SettingsWindow : BaseSettingsWindow
	{
		private readonly Settings _settings;

		private readonly SharedSettingsView _sharedSettingsView;

		private readonly Dictionary<SubModuleType, SubModule> _subModules;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

		private double _tick;

		public SettingsWindow(AsyncTexture2D background, Rectangle windowRegion, Rectangle contentRegion, Settings settings, SharedSettingsView sharedSettingsView, Dictionary<SubModuleType, SubModule> subModules)
			: base(background, windowRegion, contentRegion)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			base.SubWindowEmblem = AsyncTexture2D.FromAssetId(156027);
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156035);
			base.Name = string.Format(strings_common.ItemSettings, BaseModule<QoL, Blish_HUD.Controls.StandardWindow, Settings, PathCollection>.ModuleName ?? "");
			_settings = settings;
			_sharedSettingsView = sharedSettingsView;
			_subModules = subModules;
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Width = base.ContentRegion.Width,
				Height = base.ContentRegion.Height,
				ControlPadding = new Vector2(0f, 10f),
				CanScroll = true
			};
			CreateGeneralSettings();
			foreach (KeyValuePair<SubModuleType, SubModule> subModule in _subModules)
			{
				subModule.Value.CreateSettingsPanel(_contentPanel, base.ContentRegion.Width - 20);
			}
			CreateClientSettings();
		}

		private void CreateGeneralSettings()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = AsyncTexture2D.FromAssetId(157109),
				SetLocalizedTitle = () => strings_common.GeneralSettings
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5, 2),
				ControlPadding = new Vector2(0f, 2f)
			};
			UI.WrapWithLabel(() => strings.HotbarExpandDirection_Name, () => strings.HotbarExpandDirection_Tooltip, contentFlowPanel, base.ContentRegion.Width - 20 - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = contentFlowPanel,
				SetLocalizedItems = () => new List<string>
				{
					$"{ExpandType.LeftToRight}".SplitStringOnUppercase(),
					$"{ExpandType.RightToLeft}".SplitStringOnUppercase(),
					$"{ExpandType.TopToBottom}".SplitStringOnUppercase(),
					$"{ExpandType.BottomToTop}".SplitStringOnUppercase()
				},
				SelectedItem = $"{_settings.HotbarExpandDirection.Value}".SplitStringOnUppercase(),
				ValueChangedAction = delegate(string b)
				{
					_settings.HotbarExpandDirection.Value = (Enum.TryParse<ExpandType>(b.RemoveSpaces(), out var result3) ? result3 : _settings.HotbarExpandDirection.Value);
				}
			});
			UI.WrapWithLabel(() => strings.HotbarButtonSorting_Name, () => strings.HotbarButtonSorting_Tooltip, contentFlowPanel, base.ContentRegion.Width - 20 - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = contentFlowPanel,
				SetLocalizedItems = () => new List<string>
				{
					$"{SortType.ActivesFirst}".SplitStringOnUppercase(),
					$"{SortType.ByModuleName}".SplitStringOnUppercase()
				},
				SelectedItem = $"{_settings.HotbarButtonSorting.Value}".SplitStringOnUppercase(),
				ValueChangedAction = delegate(string b)
				{
					_settings.HotbarButtonSorting.Value = (Enum.TryParse<SortType>(b.RemoveSpaces(), out var result2) ? result2 : _settings.HotbarButtonSorting.Value);
				}
			});
			UI.WrapWithLabel(() => strings.KeyboardLayout_Name, () => strings.KeyboardLayout_Tooltip, contentFlowPanel, base.ContentRegion.Width - 20 - 16, new Kenedia.Modules.Core.Controls.Dropdown
			{
				Location = new Point(250, 0),
				Parent = contentFlowPanel,
				SetLocalizedItems = () => new List<string>
				{
					$"{KeyboardLayoutType.QWERTY}".SplitStringOnUppercase(),
					$"{KeyboardLayoutType.AZERTY}".SplitStringOnUppercase(),
					$"{KeyboardLayoutType.QWERTZ}".SplitStringOnUppercase()
				},
				SelectedItem = $"{_settings.KeyboardLayout.Value}".SplitStringOnUppercase(),
				ValueChangedAction = delegate(string b)
				{
					_settings.KeyboardLayout.Value = (Enum.TryParse<KeyboardLayoutType>(b.RemoveSpaces(), out var result) ? result : _settings.KeyboardLayout.Value);
				}
			});
		}

		private void CreateClientSettings()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = _contentPanel,
				Width = base.ContentRegion.Width - 20,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = AsyncTexture2D.FromAssetId(759447),
				SetLocalizedTitle = () => strings_common.SharedSettings
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(10f)
			};
			_sharedSettingsView.CreateLayout(contentFlowPanel, base.ContentRegion.Width - 20);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick >= 1000.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus)
				{
					_sharedSettingsView?.SetWindowOffsetImages();
				}
			}
		}
	}
}
