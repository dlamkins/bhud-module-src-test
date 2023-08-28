using System;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Models
{
	public class GameStateChangedEventArgs : EventArgs
	{
		public GameStatusType OldStatus;

		public GameStatusType Status;
	}
}
