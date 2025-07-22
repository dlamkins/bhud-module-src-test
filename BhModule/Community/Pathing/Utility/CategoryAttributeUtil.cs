using Blish_HUD;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Utility
{
	internal static class CategoryAttributeUtil
	{
		internal static bool TryGetAchievementId(this PathingCategory pathingCategory, out int achievementId)
		{
			achievementId = -1;
			if (pathingCategory.ExplicitAttributes.TryGetAttribute("achievementid", out var achievementAttr) && InvariantUtil.TryParseInt(achievementAttr?.Value, ref achievementId))
			{
				return true;
			}
			return false;
		}

		internal static bool TryGetAchievementBit(this PathingCategory pathingCategory, out int achievementBit)
		{
			achievementBit = -1;
			if (pathingCategory.TryGetAggregatedAttributeValue("achievementbit", out var achievementBitAttr) && InvariantUtil.TryParseInt(achievementBitAttr, ref achievementBit))
			{
				return true;
			}
			return false;
		}

		internal static bool TryGetCopy(this PathingCategory pathingCategory, out string copyValue)
		{
			copyValue = string.Empty;
			if (pathingCategory.ExplicitAttributes.TryGetAttribute("copy", out var copyValueAttr))
			{
				copyValue = copyValueAttr.GetValueAsString();
				return true;
			}
			return false;
		}

		internal static bool TryGetCopyMessage(this PathingCategory pathingCategory, out string copyMessage)
		{
			copyMessage = "'{0}' copied to clipboard.";
			if (pathingCategory.ExplicitAttributes.TryGetAttribute("copy-message", out var copyMessageAttr))
			{
				copyMessage = copyMessageAttr.GetValueAsString();
				return true;
			}
			return false;
		}
	}
}
