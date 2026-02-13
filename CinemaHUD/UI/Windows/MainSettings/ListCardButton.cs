using System;

namespace CinemaHUD.UI.Windows.MainSettings
{
	public class ListCardButton
	{
		public string Text { get; set; }

		public int Width { get; set; } = 50;


		public Action OnClick { get; set; }
	}
}
