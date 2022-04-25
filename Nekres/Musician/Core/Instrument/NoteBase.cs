using Blish_HUD.Controls.Intern;
using Nekres.Musician.Core.Domain;

namespace Nekres.Musician.Core.Instrument
{
	public abstract class NoteBase
	{
		public readonly GuildWarsControls Key;

		public readonly Octave Octave;

		protected NoteBase(GuildWarsControls key, Octave octave)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			Key = key;
			Octave = octave;
		}

		public override bool Equals(object obj)
		{
			return Equals((NoteBase)obj);
		}

		protected bool Equals(NoteBase other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (Key == other.Key)
			{
				return Octave == other.Octave;
			}
			return false;
		}

		public override int GetHashCode()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected I4, but got Unknown
			return (Key * 397) ^ Octave;
		}
	}
}
