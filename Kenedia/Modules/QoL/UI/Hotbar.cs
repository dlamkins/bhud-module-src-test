using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.UI
{
	public class Hotbar : Container
	{
		private List<Hotbar_Button> SubControls = new List<Hotbar_Button>();

		private FlowPanel FlowPanel;

		private AsyncTexture2D Background;

		private AsyncTexture2D Expand;

		private AsyncTexture2D Expand_Hovered;

		private bool Expanded;

		public Point ButtonSize = new Point(24, 24);

		private Point ExpanderSize = new Point(32, 32);

		private Rectangle ExpanderBounds = new Rectangle(0, 0, 32, 32);

		private Rectangle TotalBounds = new Rectangle(0, 0, 32, 32);

		private Point _PreSize = Point.get_Zero();

		public Hotbar()
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			Texture2D texture = QoL.ModuleInstance.TextureManager.getBackground(_Backgrounds.No3);
			Background = AsyncTexture2D.op_Implicit(Texture2DExtension.GetRegion(texture, 25, 25, texture.get_Width() - 25, texture.get_Height() - 25));
			Expand = AsyncTexture2D.op_Implicit(QoL.ModuleInstance.TextureManager.getIcon(_Icons.Expand));
			Expand_Hovered = AsyncTexture2D.op_Implicit(QoL.ModuleInstance.TextureManager.getIcon(_Icons.Expand_Hovered));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_ControlPadding(new Vector2(2f, 2f));
			val.set_OuterControlPadding(new Vector2(4f, 4f));
			val.set_FlowDirection((ControlFlowDirection)0);
			((Control)val).set_Location(new Point(0, 0));
			FlowPanel = val;
		}

		public void AddButton(Hotbar_Button btn)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			((Control)btn).set_Parent((Container)(object)FlowPanel);
			((Control)btn).set_Visible(btn.SubModule.Active);
			((Control)btn).set_Size(ButtonSize);
			btn.Index = SubControls.Count;
			btn.SubModule.Toggled += SubModule_Toggled;
			SubControls.Add(btn);
			((Control)FlowPanel).set_Size(new Point((int)(FlowPanel.get_OuterControlPadding().X * 2f) + SubControls.Count * (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), ((Control)this).get_Height()));
			TotalBounds = new Rectangle(Point.get_Zero(), new Point(((Control)FlowPanel).get_Size().X + ExpanderSize.X, ((Control)this).get_Height()));
		}

		private void SubModule_Toggled(object sender, EventArgs e)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			int active = SubControls.Where((Hotbar_Button btn) => btn.SubModule != null && btn.SubModule.Active).Count();
			FlowPanel.SortChildren<Hotbar_Button>((Comparison<Hotbar_Button>)((Hotbar_Button a, Hotbar_Button b) => ((Control)b).get_Visible().CompareTo(((Control)a).get_Visible())));
			((Control)this).set_Size(new Point(ExpanderSize.X + active * ((int)FlowPanel.get_ControlPadding().X + ButtonSize.X), ((Control)this).get_Height()));
			_PreSize = ((Control)this).get_Size();
		}

		public void RemoveButton(Hotbar_Button btn)
		{
			if (SubControls.Contains(btn))
			{
				SubControls.Remove(btn);
			}
			btn.SubModule.Toggled -= SubModule_Toggled;
			((Control)btn).Dispose();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			Expanded = ((Control)this).get_MouseOver() || ((Control)FlowPanel).get_MouseOver();
			if (Expanded)
			{
				if (!(((Rectangle)(ref TotalBounds)).get_Size() != ((Control)this).get_Size()))
				{
					return;
				}
				_PreSize = ((_PreSize == Point.get_Zero()) ? ((Control)this).get_Size() : _PreSize);
				((Control)this).set_Size(((Rectangle)(ref TotalBounds)).get_Size());
				foreach (Hotbar_Button subControl in SubControls)
				{
					((Control)subControl).set_Visible(true);
				}
				((Control)FlowPanel).Invalidate();
			}
			else
			{
				if (!(_PreSize != Point.get_Zero()))
				{
					return;
				}
				((Control)this).set_Size(_PreSize);
				_PreSize = Point.get_Zero();
				foreach (Hotbar_Button subControl2 in SubControls)
				{
					((Control)subControl2).set_Visible(subControl2.SubModule.Active);
				}
				FlowPanel.SortChildren<Hotbar_Button>((Comparison<Hotbar_Button>)((Hotbar_Button a, Hotbar_Button b) => ((Control)b).get_Visible().CompareTo(((Control)a).get_Visible())));
				((Control)FlowPanel).Invalidate();
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			int pad = 8;
			ExpanderBounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() - bounds.Height + pad, bounds.Y + pad / 2, bounds.Height - pad, bounds.Height - pad);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Background), bounds, (Rectangle?)bounds, new Color(96, 96, 96, 105), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Expand), ExpanderBounds, (Rectangle?)Expand.get_Texture().get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			Rectangle rect = bounds;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		protected override void DisposeControl()
		{
			((Container)this).DisposeControl();
			foreach (Hotbar_Button btn in SubControls)
			{
				if (btn.SubModule != null)
				{
					btn.SubModule.Toggled -= SubModule_Toggled;
				}
			}
			SubControls?.Clear();
			FlowPanel flowPanel = FlowPanel;
			if (flowPanel != null)
			{
				((Control)flowPanel).Dispose();
			}
			Background = null;
		}
	}
}
