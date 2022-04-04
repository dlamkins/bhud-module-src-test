using System;
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
				IEnumerable<Guid> selected = from entity in PathingModule.Instance.PackInitiator.PackState.EditorStates.SelectedPathingEntities
					where entity is StandardMarker
					select (entity as StandardMarker).Guid;
				await Respond(selected, context);
			}
			else
			{
				await base.HandleResponse(context);
			}
		}
	}
}
