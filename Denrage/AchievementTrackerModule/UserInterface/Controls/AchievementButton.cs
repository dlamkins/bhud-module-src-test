using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Controls
{
	public class AchievementButton : DetailsButton
	{
		public string Description { get; set; }

		public AsyncTexture2D AchievementIcon { get; set; }

		public string CompleteText { get; set; }

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (Description != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Description, Control.get_Content().get_DefaultFont14(), new Rectangle(((Control)this)._size.Y + 20, 5, ((Control)this)._size.X - ((Control)this)._size.Y - 35, ((Control)this).get_Height()), Color.get_LightGreen(), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
			((DetailsButton)this).PaintBeforeChildren(spriteBatch, bounds);
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (CompleteText != null)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, CompleteText, Control.get_Content().get_DefaultFont14(), new Rectangle(((Control)this)._size.Y + 20, -20, ((Control)this)._size.X - ((Control)this)._size.Y - 35, ((Control)this).get_Height()), Color.get_White(), true, true, 1, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}

		public AchievementButton()
			: this()
		{
		}
	}
}
