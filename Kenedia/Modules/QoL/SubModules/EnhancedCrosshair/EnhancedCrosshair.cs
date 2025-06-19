using System.IO;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Kenedia.Modules.QoL.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.SubModules.EnhancedCrosshair
{
	public class EnhancedCrosshair : SubModule
	{
		private SettingEntry<Point> _crosshairSize;

		private Kenedia.Modules.Core.Controls.Image _crosshair;

		public override SubModuleType SubModuleType => SubModuleType.EnhancedCrosshair;

		public EnhancedCrosshair(SettingCollection settings)
			: base(settings)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			EnhancedCrosshair enhancedCrosshair = this;
			SubModuleUI uI_Elements = UI_Elements;
			Kenedia.Modules.Core.Controls.Image obj = new Kenedia.Modules.Core.Controls.Image
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = _crosshairSize.Value,
				Texture = AsyncTexture2D.FromAssetId(1677342),
				Enabled = false
			};
			Kenedia.Modules.Core.Controls.Image item = obj;
			_crosshair = obj;
			uI_Elements.Add(item);
			string path = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.Paths.ModulePath + "crosshair.png";
			if (File.Exists(path))
			{
				GameService.Graphics.QueueMainThreadRender(delegate(GraphicsDevice graphicsDevice)
				{
					enhancedCrosshair._crosshair.Texture = TextureUtil.FromStreamPremultiplied(graphicsDevice, new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
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
				_crosshairSize.Value = new Point(84);
				_crosshair.Texture = AsyncTexture2D.FromAssetId(1058519);
				_crosshair.Opacity = 0.5f;
				_ = BaseModule<QoL, StandardWindow, Kenedia.Modules.QoL.Services.Settings, PathCollection>.ModuleInstance.CoreServices.ClientWindowService.WindowBounds;
				Rectangle p = GameService.Graphics.SpriteScreen.AbsoluteBounds;
				_crosshair.Size = _crosshairSize.Value;
				_crosshair.Location = ((Rectangle)(ref p)).get_Center().Add(new Point(-_crosshairSize.Value.X / 2, -_crosshairSize.Value.Y / 2));
				_crosshair.Visible = base.Enabled && GameService.GameIntegration.Gw2Instance.IsInGame && !GameService.Gw2Mumble.UI.IsMapOpen;
			}
		}

		protected override void Enable()
		{
			base.Enable();
			_crosshair.Visible = base.Enabled;
		}

		protected override void Disable()
		{
			base.Disable();
			_crosshair.Visible = base.Enabled;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			base.DefineSettings(settings);
			_crosshairSize = settings.DefineSetting<Point>("_crosshairSize", new Point(48), () => strings.DisableOnSearch_Name, () => strings.DisableOnSearch_Tooltip);
		}

		public override void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Panel headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = flowPanel,
				Width = width,
				HeightSizingMode = SizingMode.AutoSize,
				ShowBorder = true,
				CanCollapse = true,
				TitleIcon = base.Icon.Texture,
				Title = SubModuleType.ToString()
			};
			Kenedia.Modules.Core.Controls.FlowPanel contentFlowPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = headerPanel,
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.Fill,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ControlPadding = new Vector2(10f)
			};
			UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, $"{SubModuleType}"), () => string.Format(strings.ShowInHotbar_Description, $"{SubModuleType}"), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = base.ShowInHotbar.Value,
				CheckedChangedAction = delegate(bool b)
				{
					base.ShowInHotbar.Value = b;
				}
			});
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width,
				KeyBinding = base.HotKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					base.HotKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => string.Format(strings.HotkeyEntry_Name, $"{SubModuleType}"),
				SetLocalizedTooltip = () => string.Format(strings.HotkeyEntry_Description, $"{SubModuleType}")
			};
		}
	}
}
