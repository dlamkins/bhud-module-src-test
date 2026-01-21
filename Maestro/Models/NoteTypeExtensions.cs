namespace Maestro.Models
{
	public static class NoteTypeExtensions
	{
		public static int GetDurationMs(this NoteType noteType, int bpm)
		{
			double quarterNoteMs = 60000.0 / (double)bpm;
			return noteType switch
			{
				NoteType.Whole => (int)(quarterNoteMs * 4.0), 
				NoteType.Half => (int)(quarterNoteMs * 2.0), 
				NoteType.Quarter => (int)quarterNoteMs, 
				NoteType.Eighth => (int)(quarterNoteMs / 2.0), 
				NoteType.Sixteenth => (int)(quarterNoteMs / 4.0), 
				_ => (int)quarterNoteMs, 
			};
		}

		public static string GetDisplayName(this NoteType noteType)
		{
			return noteType switch
			{
				NoteType.Whole => "Whole", 
				NoteType.Half => "Half", 
				NoteType.Quarter => "Quarter", 
				NoteType.Eighth => "Eighth", 
				NoteType.Sixteenth => "16th", 
				_ => "Quarter", 
			};
		}
	}
}
