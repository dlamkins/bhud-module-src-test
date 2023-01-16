using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class HorizontalCompass : Control
	{
		private readonly IRootPackState _packState;

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)22;
		}

		public HorizontalCompass(IRootPackState packState)
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_ZIndex(-1073741824);
			_packState = packState;
			((Control)this).set_Location(new Point(0, 50));
			((Control)this).set_Height(50);
		}

		public override void DoUpdate(GameTime gameTime)
		{
			((Control)this).DoUpdate(gameTime);
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).set_Width(((Control)((Control)this).get_Parent()).get_Width());
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, Color.get_LightBlue());
			List<StandardMarker> list = (from marker in _packState.Entities.OfType<StandardMarker>()
				orderby marker.DistanceToPlayer
				select marker).ToList();
			((Rectangle)(ref bounds))._002Ector(((Control)this).get_Location().X, ((Control)this).get_Location().Y, bounds.Width, bounds.Height);
			foreach (StandardMarker item in list)
			{
				item.RenderToHorizontalCompass(spriteBatch, bounds);
			}
		}
	}
}
