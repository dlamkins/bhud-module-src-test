using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace rp.spark.UI.Controls
{
	internal sealed class ContextMenuColours : ContextMenuStripItem
	{
		private const int BulletSize = 18;

		private const int HorizontalPadding = 6;

		private const int TextLeftPadding = 30;

		private readonly AsyncTexture2D _bulletTexture = AsyncTexture2D.FromAssetId(155038);

		private static readonly Texture2D SubmenuArrowTexture = Control.get_Content().GetTexture("context-menu-strip-submenu");

		public Color TextColor { get; set; } = StandardColors.get_Default();


		public ContextMenuColours(string text, Color textColor)
			: this()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			((ContextMenuStripItem)this).set_Text(text);
			TextColor = textColor;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			Color iconTint = ((!((Control)this).get_Enabled()) ? StandardColors.get_DisabledText() : (((Control)this).get_MouseOver() ? StandardColors.get_Tinted() : StandardColors.get_Default()));
			if (((ContextMenuStripItem)this).get_CanCheck())
			{
				string checkState = (((ContextMenuStripItem)this).get_Checked() ? "-checked" : "-unchecked");
				string checkStyle = ((!((Control)this).get_Enabled()) ? "-disabled" : (((Control)this).get_MouseOver() ? "-active" : string.Empty));
				TextureRegion2D checkbox = Checkable.TextureRegionsCheckbox.First((TextureRegion2D region) => region.get_Name() == "checkbox/cb" + checkState + checkStyle);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, checkbox, new Rectangle(-1, ((Control)this).get_Height() / 2 - 16, 32, 32), StandardColors.get_Default());
			}
			else
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_bulletTexture), new Rectangle(6, ((Control)this).get_Height() / 2 - 9, 18, 18), iconTint);
			}
			Rectangle textBounds = default(Rectangle);
			((Rectangle)(ref textBounds))._002Ector(30, 0, ((Control)this).get_Width() - 30 - 6, ((Control)this).get_Height());
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), new Rectangle(textBounds.X + 1, textBounds.Y + 1, textBounds.Width, textBounds.Height), StandardColors.get_Shadow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ((ContextMenuStripItem)this).get_Text(), Control.get_Content().get_DefaultFont14(), textBounds, ((Control)this).get_Enabled() ? TextColor : StandardColors.get_DisabledText(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			if (((ContextMenuStripItem)this).get_Submenu() != null)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, SubmenuArrowTexture, new Rectangle(((Control)this).get_Width() - 6 - SubmenuArrowTexture.get_Width(), ((Control)this).get_Height() / 2 - SubmenuArrowTexture.get_Height() / 2, SubmenuArrowTexture.get_Width(), SubmenuArrowTexture.get_Height()), iconTint);
			}
		}
	}
}
