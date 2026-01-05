using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Behavior;
using BhModule.Community.Pathing.Entity;
using Blish_HUD;
using Blish_HUD.Content;
using Microsoft.Xna.Framework;
using Neo.IronLua;

namespace BhModule.Community.Pathing.Scripting.Extensions
{
	internal static class StandardTrailScriptExtensions
	{
		private static PackInitiator _packInitiator;

		internal static void SetPackInitiator(PackInitiator packInitiator)
		{
			_packInitiator = packInitiator;
		}

		public static void SetPoints(this StandardTrail trail, LuaTable points)
		{
			Vector3[] vpoints = ((IEnumerable<object>)points.ArrayList).Select((Func<object, Vector3>)((object a) => (Vector3)a)).ToArray();
			List<Vector3[]> trailSections = new List<Vector3[]>(1) { trail.PostProcessing_DouglasPeucker(vpoints, _packInitiator.PackState.UserResourceStates.Advanced.MapTrailDouglasPeuckerError).ToArray() };
			trail._sectionPoints = trailSections.ToArray();
			trail.BuildBuffers(vpoints);
		}

		public static void Remove(this StandardTrail trail)
		{
			trail.Unload();
			_packInitiator.PackState.RemovePathingEntity(trail);
		}

		public static void SetTexture(this StandardTrail trail, string texturePath)
		{
			throw new NotImplementedException("This method has not been implemented yet.");
		}

		public static void SetTexture(this StandardTrail trail, int textureId)
		{
			trail.Texture = AsyncTexture2D.FromAssetId(textureId) ?? AsyncTexture2D.op_Implicit(Textures.get_Error());
		}

		public static IBehavior GetBehavior(this StandardTrail trail, string behaviorName)
		{
			foreach (IBehavior behavior in trail.Behaviors)
			{
				if (string.Equals(behavior.GetType().Name, behaviorName, StringComparison.InvariantCultureIgnoreCase))
				{
					return behavior;
				}
			}
			return null;
		}
	}
}
