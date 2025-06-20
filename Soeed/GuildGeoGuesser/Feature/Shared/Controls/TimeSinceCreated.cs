using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class TimeSinceCreated : Control, IDisposable
	{
		protected AsyncTexture2D _timer = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(496252);

		protected int UpdateInverval = 30;

		protected int IntervalCount;

		protected Rectangle ICON_BOUNDS = new Rectangle(0, 4, 24, 24);

		protected Rectangle TEXT_BOUNDS = new Rectangle(30, 0, 210, 30);

		protected string Text = "Created on...";

		protected DateTime CreatedTime;

		public TimeSinceCreated(DateTime created)
			: this()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			CreatedTime = created;
			((Control)this).set_Width(240);
			((Control)this).set_Height(30);
			((Control)this).set_BasicTooltipText("Created: " + created.ToString());
		}

		protected override void DisposeControl()
		{
		}

		protected void UpdateStrings()
		{
			TimeSpan timeSinceCreation = DateTime.UtcNow - CreatedTime;
			if (timeSinceCreation.TotalSeconds < 0.0)
			{
				Text = "Just created";
			}
			else if (timeSinceCreation.Days > 0)
			{
				Text = string.Format("{0} {1} ago", timeSinceCreation.Days, (timeSinceCreation.Days == 1) ? "day" : "days");
				UpdateInverval = 3600;
			}
			else if (timeSinceCreation.Hours > 0)
			{
				Text = $"{timeSinceCreation.Hours}h {timeSinceCreation.Minutes}m ago";
			}
			else if (timeSinceCreation.Minutes > 0)
			{
				Text = $"{timeSinceCreation.Minutes}m {timeSinceCreation.Seconds}s ago";
			}
			else
			{
				Text = $"{timeSinceCreation.Seconds}s ago";
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			IntervalCount++;
			if (IntervalCount >= UpdateInverval)
			{
				UpdateStrings();
				IntervalCount = 0;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_timer), ICON_BOUNDS);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, Text, Control.get_Content().get_DefaultFont16(), TEXT_BOUNDS, Color.get_LightGoldenrodYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
