using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;

namespace BhModule.Community.Pathing.LocalHttp.Routes.API.Edit
{
	[Route("/api/edit/selected")]
	public class Selected : Route
	{
		public override async Task HandleResponse(HttpListenerContext context)
		{
			if (context.Request.HttpMethod == "GET")
			{
				IEnumerable<int?> selected = PathingModule.Instance.PackInitiator.PackState.EditorStates.SelectedPathingEntities.Select((IPathingEntity entity) => entity.EditTag);
				await Respond(selected, context);
			}
			else
			{
				await base.HandleResponse(context);
			}
		}
	}
}
