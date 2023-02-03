using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public abstract class RaceDrawerWorld : RaceDrawer, IMapEntity
	{
		public RaceDrawerWorld()
		{
			((Control)this).set_ClipsBounds(false);
			RacingModule.ScreenMap.AddEntity(this);
		}

		protected sealed override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && !GameService.Gw2Mumble.get_UI().get_IsMapOpen())
			{
				Race race = Race;
				if (race != null && race.MapId == GameService.Gw2Mumble.get_CurrentMap().get_Id())
				{
					DrawRaceToWorld(spriteBatch);
				}
			}
		}

		protected abstract void DrawRaceToWorld(SpriteBatch spriteBatch);

		public void DrawToMap(SpriteBatch spriteBatch, IMapBounds map)
		{
			if (Race != null && ((Control)(object)this).IsVisible())
			{
				DrawRaceToMap(spriteBatch, map);
			}
		}

		protected abstract void DrawRaceToMap(SpriteBatch spriteBatch, IMapBounds map);

		protected override void DisposeControl()
		{
			RacingModule.ScreenMap.RemoveEntity(this);
			((Control)this).DisposeControl();
		}
	}
}
