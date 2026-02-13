using CinemaModule.Models;
using Microsoft.Xna.Framework;

namespace CinemaModule.Settings
{
	internal class CinemaUserSettingsData
	{
		public string StreamUrl { get; set; } = "";


		public string CurrentTwitchChannel { get; set; } = "";


		public StreamSourceType CurrentStreamSourceType { get; set; }

		public int Volume { get; set; } = 50;


		public CinemaDisplayMode DisplayMode { get; set; }

		public string SelectedPresetLocationId { get; set; } = "";


		public string SelectedSavedLocationId { get; set; } = "";


		public string SelectedSavedStreamId { get; set; } = "";


		public WorldPosition3D WorldPosition { get; set; } = new WorldPosition3D(0f, 0f, 0f, 0);


		public float WorldScreenWidth { get; set; } = 10f;


		public Point WindowPosition { get; set; } = new Point(100, 50);


		public Point WindowSize { get; set; } = new Point(640, 360);


		public SavedLocationCollection SavedLocations { get; set; } = new SavedLocationCollection();


		public SavedStreamCollection SavedStreams { get; set; } = new SavedStreamCollection();

	}
}
