using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Frtal.Wayfinder.UI
{
	public class QuickActionsPanel : Panel
	{
		private const int RowH = 24;

		private const int PadX = 10;

		private const int Width_ = 260;

		private readonly ModuleSettings _s;

		private readonly Checkbox _compass;

		private readonly Checkbox _radial;

		private readonly Checkbox _guide;

		private readonly Checkbox _clickDone;

		public Func<bool> GetEnabled { get; set; }

		public Action<bool> SetEnabled { get; set; }

		public Action OpenDiscovery { get; set; }

		public QuickActionsPanel(ModuleSettings settings)
			: this()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			_s = settings;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Panel)this).set_Title("Wayfinder");
			((Panel)this).set_ShowBorder(true);
			((Panel)this).set_CanScroll(false);
			((Control)this).set_ZIndex(25);
			((Control)this).set_Visible(false);
			((Control)this).set_Size(new Point(260, 200));
			int y = 8;
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Show compass");
			((Control)val).set_Location(new Point(10, y));
			((Control)val).set_Width(240);
			((Control)val).set_Height(24);
			_compass = val;
			_compass.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				SetEnabled?.Invoke(e.get_Checked());
			});
			y += 26;
			Checkbox val2 = new Checkbox();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Radial mode (ring at your feet)");
			((Control)val2).set_Location(new Point(10, y));
			((Control)val2).set_Width(240);
			((Control)val2).set_Height(24);
			_radial = val2;
			_radial.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_s.RadialMode.set_Value(e.get_Checked());
			});
			y += 26;
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Content guide - lite");
			((Control)val3).set_Location(new Point(10, y));
			((Control)val3).set_Width(240);
			((Control)val3).set_Height(24);
			((Control)val3).set_BasicTooltipText("Shows only the single nearest objective.");
			_guide = val3;
			_guide.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_s.ContentGuideLite.set_Value(e.get_Checked());
			});
			y += 26;
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Click an icon to mark it found");
			((Control)val4).set_Location(new Point(10, y));
			((Control)val4).set_Width(240);
			((Control)val4).set_Height(24);
			((Control)val4).set_BasicTooltipText("Useful for hearts that complete before you reach the NPC.\nWhile enabled the bar captures mouse clicks.");
			_clickDone = val4;
			_clickDone.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_s.ClickToComplete.set_Value(e.get_Checked());
			});
			y += 32;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Discovered objectives...");
			((Control)val5).set_Location(new Point(10, y));
			((Control)val5).set_Width(240);
			((Control)val5).set_Height(28);
			((Control)val5).set_BasicTooltipText("Opens the map where you tick off what you have already found.");
			((Control)val5).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenDiscovery?.Invoke();
				((Control)this).set_Visible(false);
			});
			y += 32;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text("All settings...");
			((Control)val6).set_Location(new Point(10, y));
			((Control)val6).set_Width(240);
			((Control)val6).set_Height(28);
			((Control)val6).set_BasicTooltipText("Opens Blish HUD, where the full options live under Modules > Wayfinder.");
			((Control)val6).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				try
				{
					((Control)GameService.Overlay.get_BlishHudWindow()).Show();
				}
				catch
				{
				}
				((Control)this).set_Visible(false);
			});
			y += 36;
			int chrome = Math.Max(0, ((Control)this).get_Height() - ((Container)this).get_ContentRegion().Height);
			((Control)this).set_Height(y + chrome + 8);
		}

		public void ToggleAt(int anchorX, int anchorY)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Visible())
			{
				((Control)this).set_Visible(false);
				return;
			}
			Screen screen = GameService.Graphics.get_SpriteScreen();
			int x = MathHelper.Clamp(anchorX, 0, Math.Max(0, ((Control)screen).get_Width() - ((Control)this).get_Width()));
			int y = MathHelper.Clamp(anchorY, 0, Math.Max(0, ((Control)screen).get_Height() - ((Control)this).get_Height()));
			((Control)this).set_Location(new Point(x, y));
			Sync();
			((Control)this).set_Visible(true);
		}

		public void Sync()
		{
			if (GetEnabled != null)
			{
				_compass.set_Checked(GetEnabled());
			}
			_radial.set_Checked(_s.RadialMode.get_Value());
			_guide.set_Checked(_s.ContentGuideLite.get_Value());
			_clickDone.set_Checked(_s.ClickToComplete.get_Value());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(0, 0, bounds.Width, bounds.Height), new Color(24, 25, 29));
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
		}
	}
}
