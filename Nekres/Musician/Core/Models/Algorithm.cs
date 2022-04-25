using System.Runtime.Serialization;

namespace Nekres.Musician.Core.Models
{
	public enum Algorithm
	{
		[EnumMember(Value = "favor-notes")]
		FavorNotes,
		[EnumMember(Value = "favor-chords")]
		FavorChords
	}
}
