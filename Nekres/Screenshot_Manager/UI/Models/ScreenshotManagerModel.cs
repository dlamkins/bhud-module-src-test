using System;
using Nekres.Screenshot_Manager.Core;

namespace Nekres.Screenshot_Manager.UI.Models
{
	public class ScreenshotManagerModel : IDisposable
	{
		public FileWatcherFactory FileWatcherFactory;

		public ScreenshotManagerModel(FileWatcherFactory fileWatcherFactory)
		{
			FileWatcherFactory = fileWatcherFactory;
		}

		public void Dispose()
		{
		}
	}
}
