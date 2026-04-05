using System;
using System.Collections.Generic;
using System.Linq;

namespace SongbookOfTyria.Services
{
	public class ActiveNoteEventArgs : EventArgs
	{
		public List<ActiveNoteInfo> ActiveNotes { get; }

		public ActiveNoteEventArgs(List<ActiveNoteInfo> activeNotes)
		{
			ActiveNotes = activeNotes.ToList();
		}
	}
}
