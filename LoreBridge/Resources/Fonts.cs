using System.IO;
using Blish_HUD.Modules.Managers;
using FontStashSharp;

namespace LoreBridge.Resources
{
	public static class Fonts
	{
		private const string FontPath = "fonts/FiraSansCondensed-Medium.ttf";

		private const string FontTcPath = "fonts/NotoSansTC-Medium.ttf";

		private const string FontScPath = "fonts/NotoSansTC-Medium.ttf";

		private const string FontJpPath = "fonts/NotoSansJP-Medium.ttf";

		private const string FontKrPath = "fonts/NotoSansKR-Medium.ttf";

		public static FontSystem FontSystem;

		public static void Initialize(ContentsManager contentsManager)
		{
			Stream[] obj = new Stream[5]
			{
				contentsManager.GetFileStream("fonts/FiraSansCondensed-Medium.ttf"),
				contentsManager.GetFileStream("fonts/NotoSansKR-Medium.ttf"),
				contentsManager.GetFileStream("fonts/NotoSansTC-Medium.ttf"),
				contentsManager.GetFileStream("fonts/NotoSansTC-Medium.ttf"),
				contentsManager.GetFileStream("fonts/NotoSansJP-Medium.ttf")
			};
			FontSystem = new FontSystem();
			Stream[] array = obj;
			foreach (Stream font in array)
			{
				FontSystem.AddFont(font);
			}
		}

		public static void Dispose()
		{
			FontSystem.Dispose();
		}
	}
}
