using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Strings;
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
				if (!((Rectangle)(ref rect)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					continue;
				}
				string text = ((i == 0) ? ((TextInputBase)TemplateBox).get_Text() : ((TextInputBase)GearBox).get_Text());
				if (text != "" && text != null)
				{
					try
					{
						ClipboardUtil.get_WindowsClipboardService().SetTextAsync(text);
					}
					catch (ArgumentException)
					{
						ScreenNotification.ShowNotification("Failed to set the clipboard text!", (NotificationType)2, (Texture2D)null, 4);
					}
					catch
					{
					}
				}
				return;
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
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_0551: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			//IL_056c: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			UpdateLayout();
			BitmapFont font = GameService.Content.get_DefaultFont16();
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
				((Control)this).set_BasicTooltipText(hovered ? (common.Copy + " " + common.Template) : null);
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
