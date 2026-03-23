using System.Collections.Generic;
using SongbookOfTyria.Models;

namespace SongbookOfTyria.Services
{
	public class UserSettingsData
	{
		public FilterState FilterState { get; set; } = new FilterState();


		public Dictionary<int, TabWindowState> TabWindowStates { get; set; } = new Dictionary<int, TabWindowState>();


		public bool GlobalDetailsCollapsed { get; set; }

		public bool GlobalViewOptionsCollapsed { get; set; }

		public bool GlobalAudioPlayerCollapsed { get; set; }

		public bool GlobalPianoKeybindsCollapsed { get; set; }

		public PianoKeybinds PianoKeybinds { get; set; } = new PianoKeybinds();


		public HashSet<int> Favorites { get; set; } = new HashSet<int>();

	}
}
