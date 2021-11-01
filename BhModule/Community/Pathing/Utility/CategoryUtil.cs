using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Utility
{
	public static class CategoryUtil
	{
		public static bool UiCategoryIsNotFiltered(PathingCategory category, IPackState packState, IPathingEntity[] pathingEntities = null)
		{
			if (pathingEntities == null)
			{
				pathingEntities = packState.Entities.ToArray();
			}
			if (string.IsNullOrWhiteSpace(category.DisplayName) || !packState.UserConfiguration.PackShowCategoriesFromAllMaps.get_Value() || !GetCategoryIsNotFiltered(category, Array.Empty<IPathingEntity>(), LoadedCategoryFilter))
			{
				return GetCategoryIsNotFiltered(category, pathingEntities, CurrentMapCategoryFilter);
			}
			return true;
		}

		public static bool CurrentMapCategoryFilter(PathingCategory category, IEnumerable<IPathingEntity> pathingEntities)
		{
			IPathingEntity[] searchedEntities = (pathingEntities as IPathingEntity[]) ?? pathingEntities.ToArray();
			if (!GetAssociatedPathingEntities(category, searchedEntities).Any((IPathingEntity poi) => poi.MapId == GameService.Gw2Mumble.get_CurrentMap().get_Id()))
			{
				return category.Any((PathingCategory c) => GetCategoryIsNotFiltered(c, searchedEntities, CurrentMapCategoryFilter));
			}
			return true;
		}

		public static bool LoadedCategoryFilter(PathingCategory category, IEnumerable<IPathingEntity> pathingEntities)
		{
			if (!category.LoadedFromPack)
			{
				return category.Any((PathingCategory c) => GetCategoryIsNotFiltered(c, pathingEntities, LoadedCategoryFilter));
			}
			return true;
		}

		public static bool GetCategoryIsNotFiltered(PathingCategory category, IEnumerable<IPathingEntity> pathingEntities, Func<PathingCategory, IEnumerable<IPathingEntity>, bool> categoryFilterFunc)
		{
			return categoryFilterFunc(category, pathingEntities);
		}

		public static IEnumerable<IPathingEntity> GetAssociatedPathingEntities(PathingCategory category, IEnumerable<IPathingEntity> pathingEntities)
		{
			return pathingEntities.Where((IPathingEntity entity) => entity.Category == category);
		}
	}
}
