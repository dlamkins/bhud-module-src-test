using System;
using Blish_HUD.Modules.Managers;
using Neokain.GW2.AllianceManager.Extensions;
using Neokain.GW2.AllianceManager.Utils;

namespace Neokain.GW2.AllianceManager.Services
{
	public class FontService : IFontService, IDisposable
	{
		public BitmapFont DejaVuSans14 { get; }

		public BitmapFont DejaVuSans16 { get; }

		public BitmapFont DejaVuSans18 { get; }

		public BitmapFont DejaVuSans20 { get; }

		public BitmapFont DejaVuSans22 { get; }

		public BitmapFont DejaVuSans24 { get; }

		public BitmapFont DejaVuSans26 { get; }

		public BitmapFont DejaVuSans28 { get; }

		public BitmapFont DejaVuSans30 { get; }

		public BitmapFont DejaVuSans32 { get; }

		public BitmapFont DejaVuSansDefault => DejaVuSans20;

		public BitmapFont DejaVuSansHeader => DejaVuSans32;

		public FontService(ContentsManager contentsManager)
		{
			if (contentsManager == null)
			{
				throw new ArgumentNullException("contentsManager");
			}
			DejaVuSans14 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 14);
			DejaVuSans16 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 16);
			DejaVuSans18 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 18);
			DejaVuSans20 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 20);
			DejaVuSans22 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 22);
			DejaVuSans24 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 24);
			DejaVuSans26 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 26);
			DejaVuSans28 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 28);
			DejaVuSans30 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 30);
			DejaVuSans32 = contentsManager.GetBitmapFont("DejaVuSans.ttf", 32);
		}

		public void Dispose()
		{
			DejaVuSans14?.Dispose();
			DejaVuSans16?.Dispose();
			DejaVuSans18?.Dispose();
			DejaVuSans20?.Dispose();
			DejaVuSans22?.Dispose();
			DejaVuSans24?.Dispose();
			DejaVuSans26?.Dispose();
			DejaVuSans28?.Dispose();
			DejaVuSans30?.Dispose();
			DejaVuSans32?.Dispose();
		}
	}
}
