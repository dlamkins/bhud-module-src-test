using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.PortalDistance.Controls
{
	public class DistanceMessageControl : Control
	{
		private float _distance;

		private Color _color;

		public DistanceMessageControl()
			: this()
		{
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, $"Distance: {Math.Round(_distance, 2)}", GameService.Content.get_DefaultFont32(), bounds, _color, false, (HorizontalAlignment)1, (VerticalAlignment)1);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(((Control)((Control)this).get_Parent()).get_Width(), ((Control)((Control)this).get_Parent()).get_Height() / 2));
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}

		public void UpdateDistance(float distance)
		{
			_distance = distance;
		}

		public void UpdateColor(Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_color = color;
		}
	}
}
