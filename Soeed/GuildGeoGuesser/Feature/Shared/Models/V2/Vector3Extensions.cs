using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Models.V2
{
	public static class Vector3Extensions
	{
		public static JArray ToJArray(this Coordinates2 coords)
		{
			return new JArray(((Coordinates2)(ref coords)).get_X(), ((Coordinates2)(ref coords)).get_Y());
		}

		public static Coordinates2 ToCoords2(this JToken token)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			return new Coordinates2(token[0].Value<double>(), token[1].Value<double>());
		}

		public static JArray ToJArray(this Vector3 vector)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new JArray(vector.X, vector.Y, vector.Z);
		}

		public static Vector3 ToVector3(this JToken token)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(token[0].Value<float>(), token[1].Value<float>(), token[2].Value<float>());
		}
	}
}
