using System.Collections.Generic;

namespace SongbookOfTyria.Services
{
	public class PracticeModeState
	{
		public float MasterVolume { get; set; } = 1f;


		public bool MasterMuted { get; set; }

		public int SelectedTrackIndex { get; set; }

		public float PlaybackSpeed { get; set; } = 1f;


		public Dictionary<int, TrackVolumeState> TrackStates { get; set; } = new Dictionary<int, TrackVolumeState>();


		public List<SavedMarker> Markers { get; set; } = new List<SavedMarker>();

	}
}
