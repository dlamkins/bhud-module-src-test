using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class MumbleDistance : Control, IDisposable
	{
		protected bool _onCorrectMap;

		protected int UpdateInverval = 30;

		protected int IntervalCount;

		protected Rectangle LINE_1_BOUNDS = new Rectangle(0, 0, 500, 20);

		protected Rectangle LINE_2_BOUNDS = new Rectangle(0, 20, 500, 40);

		protected string DistanceString = "Distance";

		protected string ScoreString = "Score";

		protected Location TargetLocation { get; set; } = new Location();


		public MumbleDistance(Location location)
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			TargetLocation = location;
			((Control)this).set_Width(500);
			((Control)this).set_Height(60);
			_onCorrectMap = TargetLocation.MapId == GameService.Gw2Mumble.get_CurrentMap().get_Id();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		private void CurrentMap_MapChanged(object sender, ValueEventArgs<int> e)
		{
			_onCorrectMap = e.get_Value() == TargetLocation.MapId;
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)CurrentMap_MapChanged);
		}

		protected void UpdateStrings()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Location CurrentLocation = Location.SetFromMumble();
			float distance = Vector3.Distance(CurrentLocation.AvatarPosition, TargetLocation.AvatarPosition);
			DistanceString = $"Current distance from the puzzle location: {Math.Round(distance, 4)}";
			ScoreString = "Score at this location: " + TargetLocation.Score(CurrentLocation);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (!_onCorrectMap)
			{
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Not on the correct map to show real-time distance to puzzle location", Control.get_Content().get_DefaultFont16(), LINE_1_BOUNDS, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
				return;
			}
			IntervalCount++;
			if (IntervalCount >= UpdateInverval)
			{
				UpdateStrings();
				IntervalCount = 0;
			}
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, DistanceString, Control.get_Content().get_DefaultFont16(), LINE_1_BOUNDS, Color.get_White(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, ScoreString, Control.get_Content().get_DefaultFont16(), LINE_2_BOUNDS, Color.get_LightGoldenrodYellow(), false, (HorizontalAlignment)0, (VerticalAlignment)1);
		}
	}
}
