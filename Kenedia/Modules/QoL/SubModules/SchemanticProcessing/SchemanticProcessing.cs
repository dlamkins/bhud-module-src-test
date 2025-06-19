using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.Mumble.Models;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Kenedia.Modules.QoL.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.QoL.SubModules.SchemanticProcessing
{
	public class SchemanticProcessing : SubModule
	{
		private double _ticks;

		private SettingEntry<Point> _clickContainerLocation;

		private SettingEntry<Point> _clickContainerSize;

		private SettingEntry<KeyBinding> _triggerClick;

		private readonly Dictionary<UiSize, int> _sizes = new Dictionary<UiSize, int>
		{
			{
				(UiSize)0,
				56
			},
			{
				(UiSize)1,
				56
			},
			{
				(UiSize)2,
				58
			},
			{
				(UiSize)3,
				56
			}
		};

		private readonly List<ClickContainer> _slots = new List<ClickContainer>();

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _slotGrid;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public override SubModuleType SubModuleType => SubModuleType.SchemanticProcessing;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetCursorPos(int x, int y);

		public SchemanticProcessing(SettingCollection settings)
			: base(settings)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			GameService.Gw2Mumble.UI.UISizeChanged += UI_UISizeChanged;
			_slotGrid = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Location = _clickContainerLocation.Value,
				BorderColor = Color.get_Black(),
				CanDrag = true,
				CaptureInput = false,
				Visible = base.Enabled,
				FlowDirection = ControlFlowDirection.LeftToRight,
				HeightSizingMode = SizingMode.AutoSize,
				Width = _sizes[GameService.Gw2Mumble.UI.UISize] * 4
			};
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					_slots.Add(new ClickContainer
					{
						Parent = _slotGrid,
						CaptureInput = false
					});
				}
			}
			AdjustSizes();
			_slotGrid.Moved += ResizeableContainer_Moved;
			_slotGrid.Resized += ResizeableContainer_Resized;
		}

		private void UI_UISizeChanged(object sender, ValueEventArgs<UiSize> e)
		{
			AdjustSizes();
		}

		private void AdjustSizes()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			_sizes[(UiSize)2] = 56;
			_slotGrid.ControlPadding = new Vector2(8f);
			_slotGrid.Width = (_sizes[GameService.Gw2Mumble.UI.UISize] + (int)_slotGrid.ControlPadding.X) * 4;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					_slots[row * 4 + col].Size = new Point(_sizes[GameService.Gw2Mumble.UI.UISize]);
					_slots[row * 4 + col].BorderWidth = new RectangleDimensions(2);
				}
			}
		}

		private void ResizeableContainer_Resized(object sender, ResizedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_clickContainerSize.Value = _slotGrid.Size;
		}

		private void ResizeableContainer_Moved(object sender, MovedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_clickContainerLocation.Value = _slotGrid.Location;
		}

		public override void Update(GameTime gameTime)
		{
			if (base.Enabled)
			{
				_slotGrid.CaptureInput = GameService.Input.Keyboard.ActiveModifiers == (ModifierKeys.Alt | ModifierKeys.Shift);
				if (Common.Now - _ticks > 10000.0 && !_slotGrid.CaptureInput)
				{
					_ticks = Common.Now;
					PerformClicks();
				}
			}
		}

		private async Task PerformClicks()
		{
			for (int i = 0; i < _slots.Count; i++)
			{
				ClickContainer slot = _slots[i];
				if (slot.Selected)
				{
					Rectangle absoluteBounds = slot.AbsoluteBounds;
					Point p = ((Rectangle)(ref absoluteBounds)).get_Center().ScaleToUi();
					absoluteBounds = GameService.Graphics.SpriteScreen.AbsoluteBounds;
					Point okPos = ((Rectangle)(ref absoluteBounds)).get_Center().Add(new Point(0, 35)).ScaleToUi();
					bool num = CanInteract(slot, i);
					_ = GameService.GameIntegration.Gw2Instance.Gw2HasFocus;
					if (num)
					{
						MouseUtil.DoubleClick(MouseUtil.MouseButton.LEFT, p, sendToSystem: false);
						await Task.Delay(350);
						MouseUtil.Click(MouseUtil.MouseButton.LEFT, okPos, sendToSystem: false);
						await Task.Delay(250);
					}
				}
			}
		}

		private bool CanInteract(ClickContainer c, int index = 0)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			if (GameService.GameIntegration.Gw2Instance.Gw2HasFocus)
			{
				Rectangle val = c.MaskedRegion;
				Point size = ((Rectangle)(ref val)).get_Size().Add(new Point(-c.BorderWidth.Horizontal, -c.BorderWidth.Vertical)).ScaleToUi();
				using Bitmap bitmap = new Bitmap(size.X, size.Y);
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					val = c.AbsoluteBounds;
					Point p = ((Rectangle)(ref val)).get_Location().Add(new Point(c.BorderWidth.Horizontal, c.BorderWidth.Vertical)).ClientToScreenPos(scaleToUi: true);
					g.CopyFromScreen(new Point(p.X, p.Y), Point.Empty, new Size(size.X, size.Y));
				}
				return !IsGrayscale(bitmap, 150);
			}
			return true;
			static bool IsGrayscale(Bitmap image, int threshold = 50)
			{
				int count = 0;
				int highestBlue = 0;
				for (int x = 0; x < image.Width; x++)
				{
					for (int y = 0; y < image.Height; y++)
					{
						Color pixelColor = image.GetPixel(x, y);
						if (pixelColor.R != pixelColor.G || pixelColor.G != pixelColor.B)
						{
							count++;
						}
						highestBlue = Math.Max(highestBlue, pixelColor.B);
						if (count > threshold && highestBlue > 100)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			base.DefineSettings(settings);
			_clickContainerLocation = settings.DefineSetting<Point>("_clickContainerLocation", new Point(50, 50));
			_clickContainerSize = settings.DefineSetting<Point>("_clickContainerSize", new Point(64, 64));
			_triggerClick = settings.DefineSetting("_triggerClick", new KeyBinding(ModifierKeys.Alt, (Keys)49));
			_triggerClick.Value.Enabled = true;
			_triggerClick.Value.Activated += TriggerClick_Activated;
		}

		private void TriggerClick_Activated(object sender, EventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = _slotGrid.AbsoluteBounds;
			((Rectangle)(ref absoluteBounds)).get_Center().ClientToScreenPos(scaleToUi: true);
		}

		protected override void Enable()
		{
			base.Enable();
			_slotGrid.Visible = true;
		}

		protected override void Disable()
		{
			base.Disable();
			_slotGrid.Visible = false;
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
			UI.WrapWithLabel(() => string.Format(strings.ShowInHotbar_Name, base.Name), () => string.Format(strings.ShowInHotbar_Description, base.Name), contentFlowPanel, width - 16, new Kenedia.Modules.Core.Controls.Checkbox
			{
				Height = 20,
				Checked = base.ShowInHotbar.Value,
				CheckedChangedAction = delegate(bool b)
				{
					base.ShowInHotbar.Value = b;
				}
			});
		}
	}
}
