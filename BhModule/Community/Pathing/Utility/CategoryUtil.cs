using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Utility
{
	public static class CategoryUtil
	{
		public static bool ParentIsActive(this PathingCategory category, IPackState packState)
		{
			bool active = !packState.CategoryStates.GetCategoryInactive(category);
			if (category.Parent != null)
			{
				if (active)
				{
					return category.Parent.ParentIsActive(packState);
				}
				return false;
			}
			return active;
		}

		[IteratorStateMachine(typeof(_003CFlattenCategories_003Ed__1))]
		public static IEnumerable<PathingCategory> FlattenCategories(PathingCategory category)
		{
			return new _003CFlattenCategories_003Ed__1(-2)
			{
				_003C_003E3__category = category
			};
		}

		public static string GetPath(this PathingCategory category)
		{
			return "." + category.Namespace;
		}

		public static (IEnumerable<PathingCategory>, int skipped) FilterCategories(this IEnumerable<PathingCategory> categories, IPackState packState, bool forceShowAll = false)
		{
			if (categories == null || packState == null)
			{
				return (null, 0);
			}
			IEnumerable<PathingCategory> subCategories = categories.Where((PathingCategory cat) => cat.LoadedFromPack && cat.DisplayName != "" && !cat.IsHidden);
			if (!packState.UserConfiguration.PackEnableSmartCategoryFilter.get_Value() || forceShowAll)
			{
				return (subCategories, 0);
			}
			List<PathingCategory> filteredSubCategories = new List<PathingCategory>();
			PathingCategory lastCategory = null;
			bool lastIsSeparator = false;
			int skipped = 0;
			foreach (PathingCategory subCategory in subCategories.Reverse())
			{
				if (subCategory.IsSeparator && ((lastCategory != null && !lastCategory.IsSeparator) || lastIsSeparator))
				{
					filteredSubCategories.Add(subCategory);
					lastIsSeparator = true;
				}
				else
				{
					if (!subCategory.HasVisibleChildren(packState, recursively: true))
					{
						lastIsSeparator = false;
						if (!subCategory.IsSeparator)
						{
							skipped++;
						}
						continue;
					}
					filteredSubCategories.Add(subCategory);
					lastIsSeparator = false;
				}
				lastCategory = subCategory;
			}
			return (Enumerable.Reverse(filteredSubCategories), skipped);
		}

		public static bool HasVisibleChildren(this PathingCategory category, IPackState packState, bool recursively = false)
		{
			if (packState == null || string.IsNullOrWhiteSpace(category.DisplayName) || !category.LoadedFromPack)
			{
				return false;
			}
			int mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			if (packState.Entities.ToArray().Any((IPathingEntity e) => e.MapId == mapId && e.Category == category))
			{
				return true;
			}
			if (recursively)
			{
				foreach (PathingCategory item in category)
				{
					if (item.HasVisibleChildren(packState, recursively: true))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool UiCategoryIsNotFiltered(PathingCategory category, IPackState packState, IPathingEntity[] pathingEntities = null)
		{
			if (pathingEntities == null)
			{
				pathingEntities = packState.Entities.ToArray();
			}
			if (string.IsNullOrWhiteSpace(category.DisplayName) || packState.UserConfiguration.PackEnableSmartCategoryFilter.get_Value() || !GetCategoryIsNotFiltered(category, Array.Empty<IPathingEntity>(), LoadedCategoryFilter))
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
