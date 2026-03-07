using System;

namespace CinemaModule.Services.Twitch
{
	public class DeviceCodeEventArgs : EventArgs
	{
		public string UserCode { get; }

		public string VerificationUri { get; }

		public DeviceCodeEventArgs(string userCode, string verificationUri)
		{
			UserCode = userCode;
			VerificationUri = verificationUri;
		}
	}
}
