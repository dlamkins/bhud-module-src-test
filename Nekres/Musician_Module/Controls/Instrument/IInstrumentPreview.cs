using System;
using Blish_HUD.Controls.Intern;

namespace Nekres.Musician_Module.Controls.Instrument
{
	public interface IInstrumentPreview : IDisposable
	{
		void PlaySoundByKey(GuildWarsControls key);
	}
}
