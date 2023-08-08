using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Nekres.ProofLogix.Core.UI
{
	internal class ContextMenuStripItemWithColor : ContextMenuStripItem
	{
		private Color _textColor = StandardColors.get_Default();

		private readonly AsyncTexture2D _textureBullet = AsyncTexture2D.FromAssetId(155038);

		private static readonly Texture2D _textureArrow = Control.get_Content().GetTexture("context-menu-strip-submenu");

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor");
			}
		}

		public ContextMenuStripItemWithColor(string text)
			: this(text)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			Color color = ((!((Control)this).get_Enabled()) ? StandardColors.get_DisabledText() : (((Control)this).get_MouseOver() ? StandardColors.get_Tinted() : StandardColors.get_Default()));
			if (((ContextMenuStripItem)this).get_CanCheck())
			{
				string state = (((ContextMenuStripItem)this).get_Checked() ? "-checked" : "-unchecked");
				string extension = "";
				extension = (((Control)this).get_MouseOver() ? "-active" : extension);
				extension = ((!((Control)this).get_Enabled()) ? "-disabled" : extension);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Checkable.TextureRegionsCheckbox.First((TextureRegion2D cb) => cb.get_Name() == "checkbox/cb" + state + extension), new Rectangle(-1, ((Control)this)._size.Y / 2 - 16, 32, 32), StandardColors.get_Default());
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_textureBullet), new Rectangle(6, ((Control)this)._size.Y / 2 - 9, 18, 18), color);
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), new Rectangle(31, 1, ((Control)this)._size.X - 30 - 6, ((Control)this)._size.Y), StandardColors.get_Shadow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), new Rectangle(30, 0, ((Control)this)._size.X - 30 - 6, ((Control)this)._size.Y), ((Control)this)._enabled ? TextColor : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (((ContextMenuStripItem)this).get_Submenu() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureArrow, new Rectangle(((Control)this)._size.X - 6 - _textureArrow.get_Width(), ((Control)this)._size.Y / 2 - _textureArrow.get_Height() / 2, _textureArrow.get_Width(), _textureArrow.get_Height()), color);
			}
		}
	}
}
