using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager
{
	internal class Container_TabbedPanel : Container
	{
		public Tab SelectedTab;

		public List<Tab> Tabs = new List<Tab>();

		private Texture2D _TabBarTexture;

		public TextBox TemplateBox;

		public TextBox GearBox;

		private Texture2D _Copy;

		private Texture2D _CopyHovered;

		private int TabSize;

		public Container_TabbedPanel()
			: this()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Expected O, but got Unknown
			_TabBarTexture = BuildsManager.TextureManager.getControlTexture(_Controls.TabBar_FadeIn);
			_Copy = BuildsManager.TextureManager.getControlTexture(_Controls.Copy);
			_CopyHovered = BuildsManager.TextureManager.getControlTexture(_Controls.Copy_Hovered);
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Control)this).get_Width());
			((Control)val).set_Location(new Point(5, 45));
			TemplateBox = val;
			((TextInputBase)TemplateBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)TemplateBox_InputFocusChanged);
			TextBox val2 = new TextBox();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Width(((Control)this).get_Width());
			((Control)val2).set_Location(new Point(5, 50 + ((Control)TemplateBox).get_Height()));
			GearBox = val2;
			((TextInputBase)GearBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)GearBox_InputFocusChanged);
		}

		private void GearBox_InputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				((TextInputBase)GearBox).set_SelectionStart(0);
				((TextInputBase)GearBox).set_SelectionEnd(((TextInputBase)GearBox).get_Text().Length);
			}
			else
			{
				((TextInputBase)GearBox).set_SelectionStart(0);
				((TextInputBase)GearBox).set_SelectionEnd(0);
			}
		}

		private void TemplateBox_InputFocusChanged(object sender, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				((TextInputBase)TemplateBox).set_SelectionStart(0);
				((TextInputBase)TemplateBox).set_SelectionEnd(((TextInputBase)TemplateBox).get_Text().Length);
			}
			else
			{
				((TextInputBase)TemplateBox).set_SelectionStart(0);
				((TextInputBase)TemplateBox).set_SelectionEnd(0);
			}
		}

		protected override void DisposeControl()
		{
			foreach (Tab tab in Tabs)
			{
				if (tab.Panel != null)
				{
					((Control)tab.Panel).Dispose();
				}
			}
			((Container)this).DisposeControl();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnClick(e);
			Rectangle rect = default(Rectangle);
			for (int i = 0; i < 2; i++)
			{
				((Rectangle)(ref rect))._002Ector(((Control)this).get_LocalBounds().Width - ((Control)TemplateBox).get_Height() - 6, ((Control)TemplateBox).get_LocalBounds().Y + i * (((Control)TemplateBox).get_Height() + 5), ((Control)TemplateBox).get_Height(), ((Control)TemplateBox).get_Height());
				if (((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					string text = ((i == 0) ? ((TextInputBase)TemplateBox).get_Text() : ((TextInputBase)GearBox).get_Text());
					ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
					return;
				}
			}
			foreach (Tab tab in Tabs)
			{
				if (!tab.Hovered)
				{
					continue;
				}
				Tab selectedTab = SelectedTab;
				if (selectedTab != null)
				{
					Panel panel = selectedTab.Panel;
					if (panel != null)
					{
						((Control)panel).Hide();
					}
				}
				SelectedTab = tab;
				Panel panel2 = SelectedTab.Panel;
				if (panel2 != null)
				{
					((Control)panel2).Show();
				}
				break;
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			((Control)TemplateBox).set_Width(((Control)this).get_Width() - 10 - ((Control)TemplateBox).get_Height() - 3);
			((Control)GearBox).set_Width(((Control)this).get_Width() - 10 - ((Control)GearBox).get_Height() - 3);
			foreach (Tab tab in Tabs)
			{
				if (tab.Panel != null)
				{
					((Control)tab.Panel).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 50));
				}
			}
		}

		private void UpdateLayout()
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			int i = 0;
			TabSize = ((Control)this).get_Width() / Math.Max(1, Tabs.Count);
			foreach (Tab tab in Tabs)
			{
				if (tab.Panel != null)
				{
					((Control)tab.Panel).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 60));
				}
				Panel panel = tab.Panel;
				int? num = ((panel != null) ? new int?(((Control)panel).get_Location().Y) : null);
				Rectangle localBounds = ((Control)GearBox).get_LocalBounds();
				if (num < ((Rectangle)(ref localBounds)).get_Bottom() + 5)
				{
					Panel panel2 = tab.Panel;
					int x = ((Control)tab.Panel).get_Location().X;
					localBounds = ((Control)GearBox).get_LocalBounds();
					((Control)panel2).set_Location(new Point(x, ((Rectangle)(ref localBounds)).get_Bottom() + 5));
				}
				Panel panel3 = tab.Panel;
				if (panel3 != null && ((Control)panel3).get_Location().X < 5)
				{
					((Control)tab.Panel).set_Location(new Point(5, ((Control)tab.Panel).get_Location().Y));
				}
				tab.Bounds = new Rectangle(i * TabSize, 0, TabSize, 40);
				if (tab.Icon != null)
				{
					tab.Icon_Bounds = new Rectangle(i * TabSize + 5, 5, 30, 30);
					tab.Text_Bounds = new Rectangle(i * TabSize + 45, 0, TabSize - 50, 40);
				}
				else
				{
					tab.Text_Bounds = new Rectangle(i * TabSize, 0, TabSize, 40);
				}
				tab.Hovered = ((Rectangle)(ref tab.Bounds)).Contains(((Control)this).get_RelativeMousePosition());
				i++;
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0492: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_050b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0520: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Unknown result type (might be due to invalid IL or missing references)
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0561: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05db: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			UpdateLayout();
			BitmapFont font = new ContentService().GetFont((FontFace)0, (FontSize)16, (FontStyle)0);
			Rectangle rect = default(Rectangle);
			foreach (Tab tab in Tabs)
			{
				Color color2 = (Color)((SelectedTab == tab) ? new Color(30, 30, 30, 10) : (tab.Hovered ? new Color(0, 0, 0, 50) : Color.get_Transparent()));
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), tab.Bounds, (Rectangle?)tab.Bounds, color2);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _TabBarTexture, tab.Bounds, (Rectangle?)_TabBarTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				if (tab.Icon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, tab.Icon, tab.Icon_Bounds, (Rectangle?)tab.Icon.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, tab.Name, font, tab.Text_Bounds, (SelectedTab == tab) ? Color.get_White() : Color.get_LightGray(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				Color color = Color.get_Black();
				rect = tab.Bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			}
			for (int i = 0; i < 2; i++)
			{
				((Rectangle)(ref rect))._002Ector(bounds.Width - ((Control)TemplateBox).get_Height() - 6, ((Control)TemplateBox).get_LocalBounds().Y + i * (((Control)TemplateBox).get_Height() + 5), ((Control)TemplateBox).get_Height(), ((Control)TemplateBox).get_Height());
				bool hovered = ((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, hovered ? _CopyHovered : _Copy, rect, (Rectangle?)_Copy.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				((Control)this).set_BasicTooltipText(hovered ? "Copy Template" : null);
				Color color = Color.get_Black();
				rect = bounds;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			}
		}
	}
}
