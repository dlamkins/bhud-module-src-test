using System.Collections;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MysticCrafting.Module.Discovery.Menu
{
	public class MenuPanel : FlowPanel
	{
		private Rectangle _layoutHeaderBounds;

		private Rectangle _layoutHeaderStatusTextBounds;

		public Color HighlightColor = Color.get_Yellow();

		public int CurrentSelected;

		public int MaxSelected;

		public Scrollbar AssociatedScrollbar
		{
			get
			{
				Container parent = ((Control)this).get_Parent();
				if (parent == null)
				{
					return null;
				}
				return ((IEnumerable)parent.get_Children()).OfType<Scrollbar>().FirstOrDefault((Scrollbar s) => s.get_AssociatedContainer() == this);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			int num1 = ((!string.IsNullOrEmpty(((Panel)this)._title)) ? 36 : 0);
			((Container)this).set_ContentRegion(new Rectangle(0, num1, ((Control)this)._size.X, ((Control)this)._size.Y - num1));
			Rectangle contentRegion = ((Container)this).get_ContentRegion();
			_layoutHeaderBounds = new Rectangle(((Rectangle)(ref contentRegion)).get_Left(), 0, ((Container)this).get_ContentRegion().Width, 36);
			_layoutHeaderStatusTextBounds = new Rectangle(((Rectangle)(ref _layoutHeaderBounds)).get_Right() - 60, 0, _layoutHeaderBounds.Width - 10, 36);
			((FlowPanel)this).RecalculateLayout();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (e.get_CurrentSize().Y == 0)
			{
				((Control)this).set_Size(new Point(e.get_CurrentSize().X, 36));
			}
			((Container)this).OnResized(e);
			if (AssociatedScrollbar != null)
			{
				((Control)AssociatedScrollbar).set_Visible(!((Panel)this).get_Collapsed());
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if (MaxSelected != 0 && CurrentSelected != MaxSelected)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(_layoutHeaderBounds.X, _layoutHeaderBounds.Y, 5, _layoutHeaderBounds.Height), HighlightColor * 0.3f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _layoutHeaderBounds, HighlightColor * 0.3f);
			}
			((Panel)this).PaintBeforeChildren(spriteBatch, bounds);
			if (MaxSelected != 0)
			{
				string text = $"{CurrentSelected}/{MaxSelected}";
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, GameService.Content.get_DefaultFont16(), RectangleExtension.OffsetBy(_layoutHeaderStatusTextBounds, 1, 1), Color.get_Black(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Control.get_Content().get_DefaultFont16(), _layoutHeaderStatusTextBounds, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		public MenuPanel()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
