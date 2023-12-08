using System;

namespace Estreya.BlishHUD.Shared.Contexts
{
	public class ContextEventArgs : EventArgs
	{
		public Type Caller { get; private set; }

		public ContextEventArgs(Type caller)
		{
			Caller = caller;
		}
	}
	public class ContextEventArgs<T> : EventArgs
	{
		public Type Caller { get; private set; }

		public T Content { get; private set; }

		public ContextEventArgs(Type caller, T content)
		{
			Caller = caller;
			Content = content;
		}
	}
}
