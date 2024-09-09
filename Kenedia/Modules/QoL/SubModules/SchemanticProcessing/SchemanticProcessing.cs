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

		private readonly FlowPanel _slotGrid;

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
			GameService.Gw2Mumble.get_UI().add_UISizeChanged((EventHandler<ValueEventArgs<UiSize>>)UI_UISizeChanged);
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)flowPanel).set_Location(_clickContainerLocation.get_Value());
			flowPanel.BorderColor = Color.get_Black();
			flowPanel.CanDrag = true;
			flowPanel.CaptureInput = false;
			((Control)flowPanel).set_Visible(base.Enabled);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)0);
			((Container)flowPanel).set_HeightSizingMode((SizingMode)1);
			((Control)flowPanel).set_Width(_sizes[GameService.Gw2Mumble.get_UI().get_UISize()] * 4);
			_slotGrid = flowPanel;
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					List<ClickContainer> slots = _slots;
					ClickContainer clickContainer = new ClickContainer();
					((Control)clickContainer).set_Parent((Container)(object)_slotGrid);
					clickContainer.CaptureInput = false;
					slots.Add(clickContainer);
				}
			}
			AdjustSizes();
			((Control)_slotGrid).add_Moved((EventHandler<MovedEventArgs>)ResizeableContainer_Moved);
			((Control)_slotGrid).add_Resized((EventHandler<ResizedEventArgs>)ResizeableContainer_Resized);
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
			((FlowPanel)_slotGrid).set_ControlPadding(new Vector2(8f));
			((Control)_slotGrid).set_Width((_sizes[GameService.Gw2Mumble.get_UI().get_UISize()] + (int)((FlowPanel)_slotGrid).get_ControlPadding().X) * 4);
			for (int row = 0; row < 9; row++)
			{
				for (int col = 0; col < 4; col++)
				{
					((Control)_slots[row * 4 + col]).set_Size(new Point(_sizes[GameService.Gw2Mumble.get_UI().get_UISize()]));
					_slots[row * 4 + col].BorderWidth = new RectangleDimensions(2);
				}
			}
		}

		private void ResizeableContainer_Resized(object sender, ResizedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_clickContainerSize.set_Value(((Control)_slotGrid).get_Size());
		}

		private void ResizeableContainer_Moved(object sender, MovedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_clickContainerLocation.set_Value(((Control)_slotGrid).get_Location());
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			if (base.Enabled)
			{
				_slotGrid.CaptureInput = (int)GameService.Input.get_Keyboard().get_ActiveModifiers() == 6;
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
					Rectangle absoluteBounds = ((Control)slot).get_AbsoluteBounds();
					Point p = PointExtensions.ScaleToUi(((Rectangle)(ref absoluteBounds)).get_Center());
					absoluteBounds = ((Control)GameService.Graphics.get_SpriteScreen()).get_AbsoluteBounds();
					Point okPos = PointExtensions.ScaleToUi(((Rectangle)(ref absoluteBounds)).get_Center().Add(new Point(0, 35)));
					bool num = CanInteract(slot, i);
					GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus();
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
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2HasFocus())
			{
				Rectangle val = c.MaskedRegion;
				Point size = PointExtensions.ScaleToUi(((Rectangle)(ref val)).get_Size().Add(new Point(-c.BorderWidth.Horizontal, -c.BorderWidth.Vertical)));
				using Bitmap bitmap = new Bitmap(size.X, size.Y);
				using (Graphics g = Graphics.FromImage(bitmap))
				{
					val = ((Control)c).get_AbsoluteBounds();
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
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			base.DefineSettings(settings);
			_clickContainerLocation = settings.DefineSetting<Point>("_clickContainerLocation", new Point(50, 50), (Func<string>)null, (Func<string>)null);
			_clickContainerSize = settings.DefineSetting<Point>("_clickContainerSize", new Point(64, 64), (Func<string>)null, (Func<string>)null);
			_triggerClick = settings.DefineSetting<KeyBinding>("_triggerClick", new KeyBinding((ModifierKeys)2, (Keys)49), (Func<string>)null, (Func<string>)null);
			_triggerClick.get_Value().set_Enabled(true);
			_triggerClick.get_Value().add_Activated((EventHandler<EventArgs>)TriggerClick_Activated);
		}

		private void TriggerClick_Activated(object sender, EventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = ((Control)_slotGrid).get_AbsoluteBounds();
			((Rectangle)(ref absoluteBounds)).get_Center().ClientToScreenPos(scaleToUi: true);
		}

		protected override void Enable()
		{
			base.Enable();
			((Control)_slotGrid).set_Visible(true);
		}

		protected override void Disable()
		{
			base.Disable();
			((Control)_slotGrid).set_Visible(false);
		}

		public override void CreateSettingsPanel(FlowPanel flowPanel, int width)
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
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
			flowPanel2.ContentPadding = new RectangleDimensions(5, 2);
			((FlowPanel)flowPanel2).set_ControlPadding(new Vector2(0f, 2f));
			FlowPanel contentFlowPanel = flowPanel2;
			Func<string> localizedLabelContent = () => string.Format(strings.ShowInHotbar_Name, base.Name);
			Func<string> localizedTooltip = () => string.Format(strings.ShowInHotbar_Description, base.Name);
			int width2 = width - 16;
			Checkbox checkbox = new Checkbox();
			((Control)checkbox).set_Height(20);
			((Checkbox)checkbox).set_Checked(base.ShowInHotbar.get_Value());
			checkbox.CheckedChangedAction = delegate(bool b)
			{
				base.ShowInHotbar.set_Value(b);
			};
			UI.WrapWithLabel(localizedLabelContent, localizedTooltip, (Container)(object)contentFlowPanel, width2, (Control)(object)checkbox);
		}
	}
}
