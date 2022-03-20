using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Utility
{
	public static class Gw2SharpModelExtensions
	{
		public static Rectangle ToXnaRectangle(this Rectangle rectangle)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Coordinates2 topLeft = ((Rectangle)(ref rectangle)).get_TopLeft();
			int num = (int)((Coordinates2)(ref topLeft)).get_X();
			topLeft = ((Rectangle)(ref rectangle)).get_TopLeft();
			return new Rectangle(num, (int)((Coordinates2)(ref topLeft)).get_Y(), (int)((Rectangle)(ref rectangle)).get_Width(), (int)((Rectangle)(ref rectangle)).get_Height());
		}
	}
}
