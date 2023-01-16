using BhModule.Community.Pathing.Entity;
using Neo.IronLua;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Scripting.Extensions
{
	internal static class PathingCategoryScriptExtensions
	{
		public static bool IsVisible(this PathingCategory category)
		{
			return !PathingModule.Instance.PackInitiator.PackState.CategoryStates.GetCategoryInactive(category);
		}

		public static void Show(this PathingCategory category)
		{
			PathingModule.Instance.PackInitiator.PackState.CategoryStates.SetInactive(category, isInactive: false);
		}

		public static void Hide(this PathingCategory category)
		{
			PathingModule.Instance.PackInitiator.PackState.CategoryStates.SetInactive(category, isInactive: true);
		}

		public static LuaTable GetMarkers(this PathingCategory category)
		{
			return category.GetMarkers(getAll: false);
		}

		public static LuaTable GetMarkers(this PathingCategory category, bool getAll)
		{
			LuaTable markers = new LuaTable();
			foreach (IPathingEntity pathable in PathingModule.Instance.PackInitiator.PackState.Entities)
			{
				StandardMarker marker = pathable as StandardMarker;
				if (marker == null)
				{
					continue;
				}
				if (pathable.Category == category)
				{
					markers.Add(marker);
				}
				else
				{
					if (!getAll)
					{
						continue;
					}
					if (category.Root)
					{
						markers.Add(marker);
						continue;
					}
					foreach (PathingCategory parent in pathable.Category.GetParents())
					{
						if (parent == category)
						{
							markers.Add(marker);
							break;
						}
					}
				}
			}
			return markers;
		}
	}
}
