using System.Diagnostics;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Nekres.Musician.Core.Domain;
using Nekres.Musician.Core.Instrument;

namespace Nekres.Musician.Core.Player.Algorithms
{
	public abstract class PlayAlgorithmBase
	{
		protected bool _abort;

		protected readonly Stopwatch _stopwatch;

		public readonly InstrumentBase Instrument;

		public readonly Vector3 CharacterPosition;

		protected PlayAlgorithmBase(InstrumentBase instrument)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			Instrument = instrument;
			_stopwatch = new Stopwatch();
			CharacterPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position();
		}

		public abstract void Play(Metronome metronomeMark, ChordOffset[] melody);

		public virtual void Dispose()
		{
			_abort = true;
			_stopwatch.Stop();
		}

		protected bool CharacterMoved()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Vector3 characterPosition = CharacterPosition;
			if (!((Vector3)(ref characterPosition)).Equals(GameService.Gw2Mumble.get_PlayerCharacter().get_Position()))
			{
				return !Instrument.Walkable;
			}
			return false;
		}

		protected bool CanContinue()
		{
			if (GameService.Gw2Mumble.get_IsAvailable() && !GameService.Gw2Mumble.get_UI().get_IsTextInputFocused())
			{
				return !CharacterMoved();
			}
			return false;
		}
	}
}
