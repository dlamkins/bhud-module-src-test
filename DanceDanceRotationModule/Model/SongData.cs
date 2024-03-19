using System;

namespace DanceDanceRotationModule.Model
{
	public struct SongData
	{
		public enum UtilitySkillMapping
		{
			One,
			Two,
			Three
		}

		public const int MinimumNotePositionChangePerSecond = 75;

		public const int MaximumNotePositionChangePerSecond = 600;

		public const int DefaultNotePositionChangePerSecond = 300;

		public Song.ID Id { get; set; }

		public UtilitySkillMapping Utility1Mapping { get; set; }

		public UtilitySkillMapping Utility2Mapping { get; set; }

		public UtilitySkillMapping Utility3Mapping { get; set; }

		public float PlaybackRate { get; set; }

		public int StartAtSecond { get; set; }

		public int NotePositionChangePerSecond { get; set; }

		public SongData(Song.ID id)
		{
			Id = id;
			Utility1Mapping = UtilitySkillMapping.One;
			Utility2Mapping = UtilitySkillMapping.Two;
			Utility3Mapping = UtilitySkillMapping.Three;
			PlaybackRate = 1f;
			StartAtSecond = 0;
			NotePositionChangePerSecond = 300;
		}

		public NoteType RemapNoteType(NoteType noteType)
		{
			return noteType switch
			{
				NoteType.Utility1 => RemapNoteType(Utility1Mapping), 
				NoteType.Utility2 => RemapNoteType(Utility2Mapping), 
				NoteType.Utility3 => RemapNoteType(Utility3Mapping), 
				_ => noteType, 
			};
		}

		public static NoteType RemapNoteType(UtilitySkillMapping skillMapping)
		{
			return skillMapping switch
			{
				UtilitySkillMapping.One => NoteType.Utility1, 
				UtilitySkillMapping.Two => NoteType.Utility2, 
				UtilitySkillMapping.Three => NoteType.Utility3, 
				_ => throw new ArgumentOutOfRangeException("skillMapping", skillMapping, null), 
			};
		}

		public static SongData DefaultSettings(Song.ID id)
		{
			return new SongData(id);
		}
	}
}
