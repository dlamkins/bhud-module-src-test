using System;
using BhModule.Community.Pathing.Utility;

namespace BhModule.Community.Pathing.Scripting.Extensions
{
	internal static class GuidExtensions
	{
		public static string ToBase64(this Guid guid)
		{
			return guid.ToBase64String();
		}
	}
}
