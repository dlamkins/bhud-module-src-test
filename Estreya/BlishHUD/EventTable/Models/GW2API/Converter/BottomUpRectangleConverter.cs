using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.EventTable.Models.GW2API.Converter
{
	public class BottomUpRectangleConverter : RectangleConverter
	{
		public BottomUpRectangleConverter()
			: base((RectangleDirectionType)1)
		{
		}
	}
}
