using System;
using Blish_HUD.Controls;

namespace GW2app
{
	internal class ListWindowEntry
	{
		public string ListId;

		public GW2appWindow Window;

		public Panel Panel;

		public Panel FooterPanel;

		public int FooterReserve;

		public Action RerenderCopyChunks;
	}
}
