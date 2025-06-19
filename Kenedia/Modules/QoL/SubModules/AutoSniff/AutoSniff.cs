using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.GameIntegration.GfxSettings;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.Core.Utility.WindowsUtil;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.AutoSniff
{
	public class AutoSniff : SubModule
	{
		private double _ticks;

		private double _threshold = 500.0;

		private SettingEntry<KeyBinding> _sniffSkillKey;

		private Blish_HUD.Controls.Image _capturedImageControl;

		public Texture2D SlotTexture { get; private set; }

		public Texture2D MountSlotTexture { get; private set; }

		public override SubModuleType SubModuleType => SubModuleType.AutoSniff;

		public ClientWindowService ClientWindowService { get; }

		public SharedSettings SharedSettings { get; }

		public bool WasMounted { get; private set; }

		public AutoSniff(SettingCollection settings, ClientWindowService clientWindowService, SharedSettings sharedSettings)
			: base(settings)
		{
			ClientWindowService = clientWindowService;
			SharedSettings = sharedSettings;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (!base.Enabled)
			{
				return;
			}
			Gw2MumbleService Mumble = GameService.Gw2Mumble;
			if (_capturedImageControl != null)
			{
				_capturedImageControl.Location = new Point(1990, 1595);
				_capturedImageControl.Size = new Point(40);
			}
			if (Mumble.Info.IsGameFocused && !Mumble.UI.IsTextInputFocused && GameService.GameIntegration.Gw2Instance.IsInGame && !(gameTime.get_TotalGameTime().TotalMilliseconds - _ticks < _threshold))
			{
				int id = GameService.Gw2Mumble.CurrentMap.Id;
				bool flag = ((id == 1550 || id == 1554) ? true : false);
				if (flag && Sniff())
				{
					_ticks = gameTime.get_TotalGameTime().TotalMilliseconds;
					return;
				}
				double delay = _threshold * 0.5;
				_ticks = gameTime.get_TotalGameTime().TotalMilliseconds + delay;
			}
		}

		private bool IsMounted()
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.UI.IsMapOpen && WasMounted)
			{
				return true;
			}
			Point mountSlotPosition = default(Point);
			((Point)(ref mountSlotPosition))._002Ector(1944, 1600);
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			float factor = GameService.Graphics.UIScaleMultiplier;
			Size size = new Size(40, 40);
			Rectangle bounds = default(Rectangle);
			((Rectangle)(ref bounds))._002Ector(mountSlotPosition.X, mountSlotPosition.Y, size.Width, size.Height);
			using Bitmap bitmap = new Bitmap((int)((float)bounds.Width * factor), (int)((float)bounds.Height * factor));
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				int x = (int)((float)bounds.X * factor);
				int y = (int)((float)bounds.Y * factor);
				g.CopyFromScreen(new Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), Point.Empty, size);
			}
			using MemoryStream s = new MemoryStream();
			bitmap.Save(s, ImageFormat.Bmp);
			Texture2D mountSlotTexture = MountSlotTexture;
			if (mountSlotTexture != null)
			{
				((GraphicsResource)mountSlotTexture).Dispose();
			}
			MountSlotTexture = s.CreateTexture2D();
			_capturedImageControl?.SetTexture(MountSlotTexture);
			Color color = bitmap.GetAverageColor();
			int avg = (color.R + color.B + color.G) / 3;
			WasMounted = avg == 26;
			return WasMounted;
		}

		private bool IsSkillReady()
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.Gw2Mumble.UI.IsMapOpen && WasMounted)
			{
				return true;
			}
			Point slotPosition = default(Point);
			((Point)(ref slotPosition))._002Ector(1292, 1600);
			User32Dll.RECT wndBounds = ClientWindowService.WindowBounds;
			ScreenModeSetting? screenMode = GameService.GameIntegration.GfxSettings.ScreenMode;
			Point p = (Point)(((screenMode.HasValue ? ((string)screenMode.GetValueOrDefault()) : null) == (string)ScreenModeSetting.Windowed) ? new Point(SharedSettings.WindowOffset.Left, SharedSettings.WindowOffset.Top) : Point.get_Zero());
			float factor = GameService.Graphics.UIScaleMultiplier;
			Size size = new Size(40, 40);
			Rectangle bounds = default(Rectangle);
			((Rectangle)(ref bounds))._002Ector(slotPosition.X, slotPosition.Y, size.Width, size.Height);
			using Bitmap bitmap = new Bitmap((int)((float)bounds.Width * factor), (int)((float)bounds.Height * factor));
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				int x = (int)((float)bounds.X * factor);
				int y = (int)((float)bounds.Y * factor);
				g.CopyFromScreen(new Point(wndBounds.Left + p.X + x, wndBounds.Top + p.Y + y), Point.Empty, size);
			}
			using MemoryStream s = new MemoryStream();
			bitmap.Save(s, ImageFormat.Bmp);
			Texture2D slotTexture = SlotTexture;
			if (slotTexture != null)
			{
				((GraphicsResource)slotTexture).Dispose();
			}
			SlotTexture = s.CreateTexture2D();
			_capturedImageControl?.SetTexture(SlotTexture);
			Color color = bitmap.GetAverageColor();
			int avg = (color.R + color.B + color.G) / 3;
			if (avg == 119 || avg == 172)
			{
				return true;
			}
			return false;
		}

		private bool Sniff()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (IsMounted() && IsSkillReady())
			{
				Keyboard.Stroke((VirtualKeyShort)_sniffSkillKey.Value.PrimaryKey);
				return true;
			}
			return false;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			base.DefineSettings(settings);
			_sniffSkillKey = Settings.DefineSetting("_sniffSkillKey", new KeyBinding((Keys)50));
		}

		protected override void Enable()
		{
			base.Enable();
		}

		protected override void Disable()
		{
			base.Disable();
		}

		public override void Load()
		{
			base.Load();
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.CurrentMap.MapChanged += CurrentMap_MapChanged;
			gw2Mumble.PlayerCharacter.NameChanged += PlayerCharacter_NameChanged;
		}

		private void BuildUi()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			SubModuleUI uI_Elements = UI_Elements;
			Blish_HUD.Controls.Image obj = new Blish_HUD.Controls.Image
			{
				Parent = GameService.Graphics.SpriteScreen,
				Texture = GameService.Content.GetTexture("102339"),
				Location = new Point(1990, 1595),
				Size = new Point(40)
			};
			Blish_HUD.Controls.Image item = obj;
			_capturedImageControl = obj;
			uI_Elements.Add(item);
		}

		private void PlayerCharacter_NameChanged(object sender, ValueEventArgs<string> e)
		{
			_ticks = 0.0;
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			_ticks = 0.0;
		}

		public override void Unload()
		{
			base.Unload();
			Gw2MumbleService gw2Mumble = GameService.Gw2Mumble;
			gw2Mumble.CurrentMap.MapChanged -= CurrentMap_MapChanged;
			gw2Mumble.PlayerCharacter.NameChanged -= PlayerCharacter_NameChanged;
		}

		protected override void SwitchLanguage()
		{
			base.SwitchLanguage();
		}

		public override void CreateSettingsPanel(Kenedia.Modules.Core.Controls.FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
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
				ContentPadding = new RectangleDimensions(5, 2),
				ControlPadding = new Vector2(0f, 2f)
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
				Width = width - 16,
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
			new Kenedia.Modules.Core.Controls.KeybindingAssigner
			{
				Parent = contentFlowPanel,
				Width = width - 16,
				KeyBinding = _sniffSkillKey.Value,
				KeybindChangedAction = delegate(KeyBinding kb)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					_sniffSkillKey.Value = new KeyBinding
					{
						ModifierKeys = kb.ModifierKeys,
						PrimaryKey = kb.PrimaryKey,
						Enabled = kb.Enabled,
						IgnoreWhenInTextField = true
					};
				},
				SetLocalizedKeyBindingName = () => "Sniff Skill Keybind",
				SetLocalizedTooltip = () => "Set Sniff Skill Keybind"
			};
		}
	}
}
