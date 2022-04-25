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
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (MusicianModule.ModuleInstance.stopWhenMoving.get_Value())
			{
				Vector3 characterPosition = CharacterPosition;
				return !((Vector3)(ref characterPosition)).Equals(GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
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
