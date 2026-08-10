using System;
using Neokain.GW2.AllianceManager.Utils;

namespace Neokain.GW2.AllianceManager.Services
{
	public interface IFontService : IDisposable
	{
		BitmapFont DejaVuSans14 { get; }

		BitmapFont DejaVuSans16 { get; }

		BitmapFont DejaVuSans18 { get; }

		BitmapFont DejaVuSans20 { get; }

		BitmapFont DejaVuSans22 { get; }

		BitmapFont DejaVuSans24 { get; }

		BitmapFont DejaVuSans26 { get; }

		BitmapFont DejaVuSans28 { get; }

		BitmapFont DejaVuSans30 { get; }

		BitmapFont DejaVuSans32 { get; }

		BitmapFont DejaVuSansDefault { get; }

		BitmapFont DejaVuSansHeader { get; }
	}
}
