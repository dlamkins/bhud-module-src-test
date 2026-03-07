using System;
using System.Collections.Generic;

namespace CinemaModule.Services.Twitch
{
	public class TwitchQualitiesEventArgs : EventArgs
	{
		public IReadOnlyList<string> QualityNames { get; }

		public int SelectedIndex { get; }

		public TwitchQualitiesEventArgs(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			QualityNames = qualityNames;
			SelectedIndex = selectedIndex;
		}
	}
}
