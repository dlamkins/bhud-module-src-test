using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD.Controls;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class CategoryContextMenuStrip : ContextMenuStrip
	{
		private readonly IPackState _packState;

		private readonly PathingCategory _pathingCategory;

		public CategoryContextMenuStrip(IPackState packState, PathingCategory pathingCategory)
			: this()
		{
			_packState = packState;
			_pathingCategory = pathingCategory;
		}

		private (IEnumerable<PathingCategory> SubCategories, int Skipped) GetSubCategories()
		{
			IEnumerable<PathingCategory> subCategories = _pathingCategory.Where((PathingCategory cat) => cat.LoadedFromPack);
			if (_packState.UserConfiguration.PackShowCategoriesFromAllMaps.get_Value())
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
					if (!CategoryUtil.UiCategoryIsNotFiltered(subCategory, _packState))
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

		protected override void OnShown(EventArgs e)
		{
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Expected O, but got Unknown
			var (subCategories, skipped) = GetSubCategories();
			foreach (PathingCategory subCategory in subCategories)
			{
				((ContextMenuStrip)this).AddMenuItem((ContextMenuStripItem)(object)new CategoryContextMenuStripItem(_packState, subCategory));
			}
			if (skipped > 0 && _packState.UserConfiguration.PackShowWhenCategoriesAreFiltered.get_Value())
			{
				ContextMenuStripItem val = new ContextMenuStripItem();
				val.set_Text($"{skipped} Categories Are Hidden");
				((Control)val).set_Enabled(false);
				((Control)val).set_BasicTooltipText("Categories hidden because they are for markers on a different map.\n\nYou can disable this filter by toggling\nPathing Module Settings > Show Categories From All Maps.");
				((ContextMenuStrip)this).AddMenuItem(val);
			}
			((ContextMenuStrip)this).OnShown(e);
		}

		protected override void OnHidden(EventArgs e)
		{
			foreach (ContextMenuStripItem item in ((IEnumerable<Control>)((Container)this).get_Children()).Select((Control otherChild) => (ContextMenuStripItem)(object)((otherChild is ContextMenuStripItem) ? otherChild : null)))
			{
				if (item != null)
				{
					ContextMenuStrip submenu = item.get_Submenu();
					if (submenu != null)
					{
						((Control)submenu).Hide();
					}
				}
			}
			((Container)this).ClearChildren();
			((ContextMenuStrip)this).OnHidden(e);
		}
	}
}
