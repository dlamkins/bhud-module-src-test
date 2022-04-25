using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Musician.Core.Models;

namespace Nekres.Musician
{
	internal static class InstrumentExtensions
	{
		private static Dictionary<Instrument, Texture2D> _iconCache = new Dictionary<Instrument, Texture2D>();

		public static Texture2D GetIcon(this Instrument instrument)
		{
			if (_iconCache.TryGetValue(instrument, out var icon))
			{
				return icon;
			}
			icon = MusicianModule.ModuleInstance.ContentsManager.GetTexture("instruments\\" + instrument.ToString().ToLowerInvariant() + ".png");
			_iconCache.Add(instrument, icon);
			return icon;
		}
	}
}
