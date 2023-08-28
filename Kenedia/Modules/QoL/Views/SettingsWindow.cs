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

		private readonly FlowPanel _contentPanel;

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
			base.Name = string.Format(strings_common.ItemSettings, BaseModule<QoL, StandardWindow, Settings, PathCollection>.ModuleName ?? "");
			_settings = settings;
			_sharedSettingsView = sharedSettingsView;
			_subModules = subModules;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((Control)flowPanel).set_Width(((Container)this).get_ContentRegion().Width);
			((Control)flowPanel).set_Height(((Container)this).get_ContentRegion().Height);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 10f));
			((Panel)flowPanel).set_CanScroll(true);
			_contentPanel = flowPanel;
			CreateGeneralSettings();
			foreach (KeyValuePair<SubModuleType, SubModule> subModule in _subModules)
			{
				subModule.Value.CreateSettingsPanel(_contentPanel, ((Container)this).get_ContentRegion().Width - 20);
			}
			CreateClientSettings();
		}

		private void CreateGeneralSettings()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = AsyncTexture2D.FromAssetId(157109);
			panel.SetLocalizedTitle = () => strings_common.GeneralSettings;
			Panel headerPanel = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			flowPanel.ContentPadding = new RectangleDimensions(5, 2);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel contentFlowPanel = flowPanel;
			Func<string> localizedLabelContent = () => strings.HotbarExpandDirection_Name;
			Func<string> localizedTooltip = () => strings.HotbarExpandDirection_Tooltip;
			int width = ((Container)this).get_ContentRegion().Width - 20 - 16;
			Dropdown dropdown = new Dropdown();
			((Control)dropdown).set_Location(new Point(250, 0));
			((Control)dropdown).set_Parent((Container)(object)contentFlowPanel);
			dropdown.SetLocalizedItems = () => new List<string>
			{
				$"{ExpandType.LeftToRight}".SplitStringOnUppercase(),
				$"{ExpandType.RightToLeft}".SplitStringOnUppercase(),
				$"{ExpandType.TopToBottom}".SplitStringOnUppercase(),
				$"{ExpandType.BottomToTop}".SplitStringOnUppercase()
			};
			((Dropdown)dropdown).set_SelectedItem($"{_settings.HotbarExpandDirection.get_Value()}".SplitStringOnUppercase());
			dropdown.ValueChangedAction = delegate(string b)
			{
				_settings.HotbarExpandDirection.set_Value(Enum.TryParse<ExpandType>(b.RemoveSpaces(), out var result) ? result : _settings.HotbarExpandDirection.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width, (Control)(object)dropdown);
		}

		private void CreateClientSettings()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)_contentPanel);
			((Control)panel).set_Width(((Container)this).get_ContentRegion().Width - 20);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = AsyncTexture2D.FromAssetId(759447);
			panel.SetLocalizedTitle = () => strings_common.SharedSettings;
			Panel headerPanel = panel;
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(10f));
			FlowPanel contentFlowPanel = flowPanel;
			_sharedSettingsView.CreateLayout((Container)(object)contentFlowPanel, ((Container)this).get_ContentRegion().Width - 20);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (gameTime.get_TotalGameTime().TotalMilliseconds - _tick >= 1000.0)
			{
				_tick = gameTime.get_TotalGameTime().TotalMilliseconds;
				if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
				{
					_sharedSettingsView?.UpdateOffset();
				}
			}
		}
	}
}
