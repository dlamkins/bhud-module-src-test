using System.Net;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;
using Gw2Sharp.Models;

namespace BhModule.Community.Pathing.LocalHttp.Routes.API
{
	[Route("/api/status")]
	public class Status : Route
	{
		public override async Task HandleResponse(HttpListenerContext context)
		{
			if (context.Request.HttpMethod == "GET")
			{
				Status status = this;
				string overlayVersion = ((object)Program.get_OverlayVersion()).ToString();
				string pathingVersion = ((object)((Module)PathingModule.Instance).get_Version()).ToString();
				Coordinates2 mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
				double x = ((Coordinates2)(ref mapCenter)).get_X();
				mapCenter = GameService.Gw2Mumble.get_UI().get_MapCenter();
				await status.Respond(new
				{
					OverlayVersion = overlayVersion,
					PathingVersion = pathingVersion,
					PlayerMapX = x,
					PlayerMapY = ((Coordinates2)(ref mapCenter)).get_Y()
				}, context);
			}
			else
			{
				await base.HandleResponse(context);
			}
		}
	}
}
