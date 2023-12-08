using System;

namespace Estreya.BlishHUD.Shared.Exceptions
{
	public class ModuleInvalidException : Exception
	{
		public ModuleInvalidException(string message)
			: base((!string.IsNullOrWhiteSpace(message)) ? message : "This module is invalid due to unknown reasons. (This is a custom module exception)")
		{
		}
	}
}
