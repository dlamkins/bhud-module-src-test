using System;
using System.Collections.Generic;

namespace CinemaModule.Services.YouTube
{
	public class YouTubeQualitiesEventArgs : EventArgs
	{
		public IReadOnlyList<string> QualityNames { get; }

		public int SelectedIndex { get; }

		public YouTubeQualitiesEventArgs(IReadOnlyList<string> qualityNames, int selectedIndex)
		{
			QualityNames = qualityNames;
			SelectedIndex = selectedIndex;
		}
	}
}
