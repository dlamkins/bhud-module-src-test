using System;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class CategoryEventArgs : EventArgs
	{
		public SpamCategoryDto Category { get; set; }
	}
}
