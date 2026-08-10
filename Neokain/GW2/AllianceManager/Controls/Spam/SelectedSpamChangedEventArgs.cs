using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class SelectedSpamChangedEventArgs : EventArgs
	{
		public SpamDto SelectedSpam { get; set; }
	}
}
