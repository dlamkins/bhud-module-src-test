using System;

namespace Kenedia.Modules.Core.Contexts
{
	public class ContextEventArgs : EventArgs
	{
		public Type Caller { get; private set; }

		public ContextEventArgs(Type caller)
		{
			Caller = caller;
		}
	}
}
