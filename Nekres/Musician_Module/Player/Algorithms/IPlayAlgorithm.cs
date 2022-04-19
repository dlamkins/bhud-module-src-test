using Nekres.Musician_Module.Controls.Instrument;
using Nekres.Musician_Module.Domain.Values;

namespace Nekres.Musician_Module.Player.Algorithms
{
	public interface IPlayAlgorithm
	{
		void Play(Instrument instrument, MetronomeMark metronomeMark, ChordOffset[] melody);

		void Dispose();
	}
}
