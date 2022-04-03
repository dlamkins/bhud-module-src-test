using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Content;
using TmfLib;

namespace BhModule.Community.Pathing.LocalHttp.Routes.API.Pack
{
	[Route("/api/pack/register")]
	public class Register : Route
	{
		private const string QS_TYPE = "type";

		private const string QS_URI = "uri";

		public override async Task HandleResponse(HttpListenerContext context)
		{
			bool handled = false;
			if (context.Request.QueryString.AllKeys.Contains("type") && context.Request.QueryString.AllKeys.Contains("uri") && context.Request.QueryString["type"].ToLowerInvariant() == "web")
			{
				WebReader webReader = new WebReader(context.Request.QueryString["uri"]);
				await webReader.InitWebReader();
				TmfLib.Pack webPack = TmfLib.Pack.FromIDataReader(webReader);
				await PathingModule.Instance.PackInitiator.LoadPack(webPack);
				handled = true;
			}
			if (handled)
			{
				await RespondOk(context);
			}
			else
			{
				await RespondStatus(context, 500);
			}
		}
	}
}
