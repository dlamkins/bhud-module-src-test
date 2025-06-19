using System;

namespace Kenedia.Modules.Core.Controls
{
	public class CornerElements : IDisposable
	{
		public CornerIcon CornerIcon { get; } = new CornerIcon();


		public CornerNotificationBadge CornerNotificationBadge { get; } = new CornerNotificationBadge();


		public CornerLoadingSpinner CornerLoadingSpinner { get; } = new CornerLoadingSpinner();


		public void Dispose()
		{
			CornerIcon?.Dispose();
			CornerNotificationBadge?.Dispose();
			CornerLoadingSpinner?.Dispose();
		}
	}
}
