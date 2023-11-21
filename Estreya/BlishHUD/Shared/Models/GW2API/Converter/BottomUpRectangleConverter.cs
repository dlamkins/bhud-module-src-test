using Gw2Sharp.WebApi.V2.Models;

namespace Estreya.BlishHUD.Shared.Models.GW2API.Converter
{
	public class BottomUpRectangleConverter : RectangleConverter
	{
		public BottomUpRectangleConverter()
			: base((RectangleDirectionType)1)
		{
		}
	}
}
