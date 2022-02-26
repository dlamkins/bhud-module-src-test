using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class CategoryContextMenuStrip : ContextMenuStrip
	{
		private readonly IPackState _packState;

		private readonly PathingCategory _pathingCategory;

		private bool _forceShowAll;

		public CategoryContextMenuStrip(IPackState packState, PathingCategory pathingCategory, bool forceShowAll)
			: this()
		{
			_packState = packState;
			_pathingCategory = pathingCategory;
			_forceShowAll = forceShowAll;
		}

		private (IEnumerable<PathingCategory> SubCategories, int Skipped) GetSubCategories(bool forceShowAll = false)
		{
			IEnumerable<PathingCategory> subCategories = _pathingCategory.Where((PathingCategory cat) => cat.LoadedFromPack);
			if (_packState.UserConfiguration.PackShowCategoriesFromAllMaps.get_Value() || forceShowAll)
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
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			var (subCategories, skipped) = GetSubCategories(_forceShowAll);
			foreach (PathingCategory subCategory in subCategories)
			{
				((ContextMenuStrip)this).AddMenuItem((ContextMenuStripItem)(object)new CategoryContextMenuStripItem(_packState, subCategory, _forceShowAll));
			}
			if (skipped > 0 && _packState.UserConfiguration.PackShowWhenCategoriesAreFiltered.get_Value())
			{
				ContextMenuStripItem val = new ContextMenuStripItem();
				val.set_Text($"{skipped} Categories Are Hidden");
				((Control)val).set_Enabled(false);
				val.set_CanCheck(true);
				((Control)val).set_BasicTooltipText(Strings.Info_HiddenCategories);
				ContextMenuStripItem showAllSkippedCategories = val;
				((ContextMenuStrip)this).AddMenuItem(showAllSkippedCategories);
				((Control)showAllSkippedCategories).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ShowAllSkippedCategories_LeftMouseButtonReleased);
			}
			((ContextMenuStrip)this).OnShown(e);
		}

		private void ShowAllSkippedCategories_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			((Container)this).ClearChildren();
			IEnumerable<PathingCategory> subCategories = GetSubCategories(forceShowAll: true).SubCategories;
			foreach (PathingCategory subCategory in subCategories)
			{
				((ContextMenuStrip)this).AddMenuItem((ContextMenuStripItem)(object)new CategoryContextMenuStripItem(_packState, subCategory, forceShowAll: true));
			}
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
