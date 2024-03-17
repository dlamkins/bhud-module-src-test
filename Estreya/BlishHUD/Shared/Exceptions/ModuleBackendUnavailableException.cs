using System;

namespace Estreya.BlishHUD.Shared.Exceptions
{
	public class ModuleBackendUnavailableException : Exception
	{
		public ModuleBackendUnavailableException()
			: this(null)
		{
		}

		public ModuleBackendUnavailableException(string message)
			: base((!string.IsNullOrWhiteSpace(message)) ? message : "The backend of this module is currently unavailable. The module can't be used without.")
		{
		}
	}
}
