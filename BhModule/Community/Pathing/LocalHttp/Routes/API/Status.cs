using System.Net;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules;

namespace BhModule.Community.Pathing.LocalHttp.Routes.API
{
	[Route("/api/status")]
	public class Status : Route
	{
		public override async Task HandleResponse(HttpListenerContext context)
		{
			if (context.Request.HttpMethod == "GET")
			{
				await Respond(new
				{
					OverlayVersion = ((object)Program.get_OverlayVersion()).ToString(),
					PathingVersion = ((object)((Module)PathingModule.Instance).get_Version()).ToString()
				}, context);
			}
			else
			{
				await base.HandleResponse(context);
			}
		}
	}
}
