using System;
using System.Collections.Generic;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public class CategoriesLoadedEventArgs : EventArgs
	{
		public List<SpamCategoryDto> Categories { get; set; }
	}
}
