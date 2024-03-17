using System;
using Blish_HUD;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Estreya.BlishHUD.Shared.Controls.Map
{
	public abstract class MapEntity : IDisposable
	{
		public string TooltipText { get; set; }

		public event EventHandler Disposed;

		public void Dispose()
		{
			this.Disposed?.Invoke(this, EventArgs.Empty);
			InternalDispose();
		}

		public abstract RectangleF? RenderToMiniMap(SpriteBatch spriteBatch, Rectangle bounds, double offsetX, double offsetY, double scale, float opacity);

		protected virtual void InternalDispose()
		{
		}

		protected Vector2 GetScaledLocation(double x, double y, double scale, double offsetX, double offsetY)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
			float num = (float)((x - ((Coordinates2)(ref mapCenter)).get_X()) / scale);
			mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
			Vector2 scaledLocation = default(Vector2);
			((Vector2)(ref scaledLocation))._002Ector(num, (float)((y - ((Coordinates2)(ref mapCenter)).get_Y()) / scale));
			if (!GameService.Gw2Mumble.get_UI().get_IsMapOpen() && GameService.Gw2Mumble.get_UI().get_IsCompassRotationEnabled())
			{
				scaledLocation = Vector2.Transform(scaledLocation, Matrix.CreateRotationZ((float)GameService.Gw2Mumble.get_UI().get_CompassRotation()));
			}
			scaledLocation += new Vector2((float)offsetX, (float)offsetY);
			return scaledLocation;
		}
	}
}
