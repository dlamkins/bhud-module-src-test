using System;

namespace DanceDanceRotationModule.Model
{
	public struct Note
	{
		public NoteType NoteType { get; set; }

		public TimeSpan TimeInRotation { get; set; }

		public TimeSpan Duration { get; set; }

		public AbilityId AbilityId { get; set; }

		public bool OverrideAuto { get; set; }

		public Note(NoteType noteType, TimeSpan timeInRotation, TimeSpan duration, AbilityId abilityId, bool overrideAuto)
		{
			NoteType = noteType;
			TimeInRotation = timeInRotation;
			Duration = duration;
			AbilityId = abilityId;
			OverrideAuto = overrideAuto;
		}
	}
}
