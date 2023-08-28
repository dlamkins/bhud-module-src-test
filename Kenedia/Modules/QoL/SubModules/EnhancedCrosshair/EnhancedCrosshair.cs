using System;
using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.SubModules.EnhancedCrosshair
{
	public class EnhancedCrosshair : SubModule
	{
		private SettingEntry<Point> _crosshairSize;

		private Image _crosshair;

		public override SubModuleType SubModuleType => SubModuleType.EnhancedCrosshair;

		public EnhancedCrosshair(SettingCollection settings)
			: base(settings)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			EnhancedCrosshair enhancedCrosshair = this;
			SubModuleUI uI_Elements = UI_Elements;
			Image image = new Image();
			((Control)image).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)image).set_Size(_crosshairSize.get_Value());
			((Image)image).set_Texture(AsyncTexture2D.FromAssetId(1677342));
			((Control)image).set_Enabled(false);
			Image item = image;
			_crosshair = image;
			uI_Elements.Add((Control)(object)item);
			string path = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Paths.ModulePath + "crosshair.png";
			if (File.Exists(path))
			{
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice graphicsDevice)
				{
					((Image)enhancedCrosshair._crosshair).set_Texture(AsyncTexture2D.op_Implicit(TextureUtil.FromStreamPremultiplied(graphicsDevice, (Stream)new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))));
				});
			}
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			if (base.Enabled)
			{
				_crosshairSize.set_Value(new Point(84));
				((Image)_crosshair).set_Texture(AsyncTexture2D.FromAssetId(1058519));
				((Control)_crosshair).set_Opacity(0.5f);
				_ = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Services.ClientWindowService.WindowBounds;
				Rectangle p = ((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds();
				((Control)_crosshair).set_Size(_crosshairSize.get_Value());
				((Control)_crosshair).set_Location(((Rectangle)(ref p)).get_Center().Add(new Point(-_crosshairSize.get_Value().X / 2, -_crosshairSize.get_Value().Y / 2)));
				((Control)_crosshair).set_Visible(base.Enabled && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen());
			}
		}

		protected override void Enable()
		{
			base.Enable();
			((Control)_crosshair).set_Visible(base.Enabled);
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_crosshair).set_Visible(base.Enabled);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			base.DefineSettings(settings);
			_crosshairSize = settings.DefineSetting<Point>("_crosshairSize", new Point(48), (Func<string>)(() => strings.DisableOnSearch_Name), (Func<string>)(() => strings.DisableOnSearch_Tooltip));
		}

		public override void CreateSettingsPanel(FlowPanel flowPanel, int width)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)flowPanel);
			((Control)panel).set_Width(width);
			((Container)panel).set_HeightSizingMode((SizingMode)1);
			((Panel)panel).set_ShowBorder(true);
			((Panel)panel).set_CanCollapse(true);
			panel.TitleIcon = base.Icon.Texture;
			((Panel)panel).set_Title(SubModuleType.ToString());
			Panel headerPanel = panel;
			FlowPanel flowPanel2 = new FlowPanel();
			((Control)flowPanel2).set_Parent((Container)(object)headerPanel);
			((Container)flowPanel2).set_HeightSizingMode((SizingMode)1);
			((Container)flowPanel2).set_WidthSizingMode((SizingMode)2);
			((FlowPanel)flowPanel2).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(10f));
			FlowPanel contentFlowPanel = flowPanel2;
			KeybindingAssigner keybindingAssigner = new KeybindingAssigner();
			((Control)keybindingAssigner).set_Parent((Container)(object)contentFlowPanel);
			((Control)keybindingAssigner).set_Width(width);
			((KeybindingAssigner)keybindingAssigner).set_KeyBinding(base.HotKey.get_Value());
			keybindingAssigner.KeybindChangedAction = delegate(KeyBinding kb)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Expected O, but got Unknown
				SettingEntry<KeyBinding> hotKey = base.HotKey;
				KeyBinding val = new KeyBinding();
				val.set_ModifierKeys(kb.get_ModifierKeys());
				val.set_PrimaryKey(kb.get_PrimaryKey());
				val.set_Enabled(kb.get_Enabled());
				val.set_IgnoreWhenInTextField(true);
				hotKey.set_Value(val);
			};
			keybindingAssigner.SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}");
			keybindingAssigner.SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}");
		}
	}
}
