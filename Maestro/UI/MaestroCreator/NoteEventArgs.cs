using System;

namespace Maestro.UI.MaestroCreator
{
	public class NoteEventArgs : EventArgs
	{
		public string Note { get; }

		public bool IsSharp { get; }

		public bool IsHighC { get; }

		public bool IsRest { get; }

		public NoteEventArgs(string note, bool isSharp = false, bool isHighC = false, bool isRest = false)
		{
			Note = note;
			IsSharp = isSharp;
			IsHighC = isHighC;
			IsRest = isRest;
		}
	}
}
