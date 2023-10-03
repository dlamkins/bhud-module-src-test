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
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
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
				_settings.HotbarExpandDirection.set_Value(Enum.TryParse<ExpandType>(b.RemoveSpaces(), out var result3) ? result3 : _settings.HotbarExpandDirection.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width, (Control)(object)dropdown);
			Func<string> localizedLabelContent2 = () => strings.HotbarButtonSorting_Name;
			Func<string> localizedTooltip2 = () => strings.HotbarButtonSorting_Tooltip;
			int width2 = ((Container)this).get_ContentRegion().Width - 20 - 16;
			Dropdown dropdown2 = new Dropdown();
			((Control)dropdown2).set_Location(new Point(250, 0));
			((Control)dropdown2).set_Parent((Container)(object)contentFlowPanel);
			dropdown2.SetLocalizedItems = () => new List<string>
			{
				$"{SortType.ActivesFirst}".SplitStringOnUppercase(),
				$"{SortType.ByModuleName}".SplitStringOnUppercase()
			};
			((Dropdown)dropdown2).set_SelectedItem($"{_settings.HotbarButtonSorting.get_Value()}".SplitStringOnUppercase());
			dropdown2.ValueChangedAction = delegate(string b)
			{
				_settings.HotbarButtonSorting.set_Value(Enum.TryParse<SortType>(b.RemoveSpaces(), out var result2) ? result2 : _settings.HotbarButtonSorting.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent2, localizedTooltip2, (Container)(object)contentFlowPanel, width2, (Control)(object)dropdown2);
			Func<string> localizedLabelContent3 = () => strings.KeyboardLayout_Name;
			Func<string> localizedTooltip3 = () => strings.KeyboardLayout_Tooltip;
			int width3 = ((Container)this).get_ContentRegion().Width - 20 - 16;
			Dropdown dropdown3 = new Dropdown();
			((Control)dropdown3).set_Location(new Point(250, 0));
			((Control)dropdown3).set_Parent((Container)(object)contentFlowPanel);
			dropdown3.SetLocalizedItems = () => new List<string>
			{
				$"{KeyboardLayoutType.QWERTY}".SplitStringOnUppercase(),
				$"{KeyboardLayoutType.AZERTY}".SplitStringOnUppercase(),
				$"{KeyboardLayoutType.QWERTZ}".SplitStringOnUppercase()
			};
			((Dropdown)dropdown3).set_SelectedItem($"{_settings.KeyboardLayout.get_Value()}".SplitStringOnUppercase());
			dropdown3.ValueChangedAction = delegate(string b)
			{
				_settings.KeyboardLayout.set_Value(Enum.TryParse<KeyboardLayoutType>(b.RemoveSpaces(), out var result) ? result : _settings.KeyboardLayout.get_Value());
			};
			UI.WrapWithLabel(localizedLabelContent3, localizedTooltip3, (Container)(object)contentFlowPanel, width3, (Control)(object)dropdown3);
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
