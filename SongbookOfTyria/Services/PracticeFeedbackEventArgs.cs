using System;
using System.Collections.Generic;

namespace SongbookOfTyria.Services
{
	public class PracticeFeedbackEventArgs : EventArgs
	{
		public Dictionary<int, NoteFeedbackState> NoteFeedback { get; }

		public PracticeFeedbackEventArgs(Dictionary<int, NoteFeedbackState> noteFeedback)
		{
			NoteFeedback = noteFeedback;
		}
	}
}
