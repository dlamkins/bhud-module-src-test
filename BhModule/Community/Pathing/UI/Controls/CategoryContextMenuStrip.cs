using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class CategoryContextMenuStrip : ContextMenuStrip
	{
		private readonly IPackState _packState;

		private readonly PathingCategory _pathingCategory;

		private HashSet<PathingCategory> _activeCategories;

		private static readonly Texture2D _textureContinueMenu = Control.get_Content().GetTexture("156057");

		private readonly Color _backColor = Color.FromNonPremultiplied(37, 36, 37, 255);

		private bool _forceShowAll;

		private const int SCROLLHINT_HEIGHT = 20;

		public CategoryContextMenuStrip(IPackState packState, PathingCategory pathingCategory, bool forceShowAll, HashSet<PathingCategory> activeCategories = null)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_packState = packState;
			_pathingCategory = pathingCategory;
			_forceShowAll = forceShowAll;
			_activeCategories = activeCategories;
		}

		private void EnsureActiveCategoriesPopulated()
		{
			if (_activeCategories != null)
			{
				return;
			}
			_activeCategories = new HashSet<PathingCategory>();
			int currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			IPathingEntity[] array = _packState.Entities.ToArray();
			foreach (IPathingEntity entity in array)
			{
				if (entity.MapId == currentMapId && entity.Category != null)
				{
					PathingCategory cat = entity.Category;
					while (cat != null && _activeCategories.Add(cat))
					{
						cat = cat.Parent;
					}
				}
			}
		}

		private (IEnumerable<PathingCategory> SubCategories, int Skipped) GetSubCategories(bool forceShowAll = false)
		{
			IEnumerable<PathingCategory> subCategories = _pathingCategory.Where((PathingCategory cat) => cat.LoadedFromPack && cat.DisplayName != "" && !cat.IsHidden);
			if (!_packState.UserConfiguration.PackEnableSmartCategoryFilter.get_Value() || forceShowAll)
			{
				return (subCategories, 0);
			}
			EnsureActiveCategoriesPopulated();
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
					if (string.IsNullOrWhiteSpace(subCategory.DisplayName) || !_activeCategories.Contains(subCategory))
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

		private void ShowSearch()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			ContextMenuStripItem val = new ContextMenuStripItem();
			val.set_Text("Search");
			((Control)val).set_BackgroundColor(Color.get_LightBlue() * 0.1f);
			ContextMenuStripItem searchMarker = val;
			((Control)searchMarker).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				PathingModule.Instance.SettingsWindow.set_SelectedTab(PathingModule.Instance.CategoryTreeTab);
				((Control)PathingModule.Instance.SettingsWindow).Show();
			});
			((ContextMenuStrip)this).AddMenuItem(searchMarker);
		}

		protected override void OnShown(EventArgs e)
		{
			PopulateMenuItems(_forceShowAll);
			((ContextMenuStrip)this).OnShown(e);
			if (((Control)this).get_Bottom() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom())
			{
				((Control)this).set_Bottom(((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom());
			}
			if (((Control)this).get_Top() < 0)
			{
				((Control)this).set_Top(0);
			}
			if (((Control)this).get_Right() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Right())
			{
				((Control)this).set_Right(((Control)GameService.Graphics.get_SpriteScreen()).get_Right());
			}
			if (((Control)this).get_Left() < 0)
			{
				((Control)this).set_Left(0);
			}
		}

		private void PopulateMenuItems(bool showAll)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Expected O, but got Unknown
			((Container)this).ClearChildren();
			var (subCategories, skipped) = GetSubCategories(showAll);
			if (_pathingCategory != null && _pathingCategory.Root)
			{
				ShowSearch();
				if (skipped == 0 && !subCategories.Any())
				{
					ContextMenuStripItem val = new ContextMenuStripItem();
					val.set_Text("No marker packs loaded...");
					((Control)val).set_Enabled(false);
					((ContextMenuStrip)this).AddMenuItem(val);
				}
			}
			foreach (PathingCategory subCategory in subCategories)
			{
				((ContextMenuStrip)this).AddMenuItem((ContextMenuStripItem)(object)new CategoryContextMenuStripItem(_packState, subCategory, showAll, _activeCategories));
			}
			if (skipped > 0 && _packState.UserConfiguration.PackShowWhenCategoriesAreFiltered.get_Value())
			{
				ContextMenuStripItem val2 = new ContextMenuStripItem();
				val2.set_Text($"{skipped} Categories Are Hidden");
				((Control)val2).set_Enabled(false);
				val2.set_CanCheck(true);
				((Control)val2).set_BasicTooltipText(string.Format(Strings.Info_HiddenCategories, ((SettingEntry)_packState.UserConfiguration.PackEnableSmartCategoryFilter).get_DisplayName()));
				ContextMenuStripItem showAllSkippedCategories = val2;
				((ContextMenuStrip)this).AddMenuItem(showAllSkippedCategories);
				((Control)showAllSkippedCategories).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ShowAllSkippedCategories_LeftMouseButtonReleased);
			}
		}

		private void ShowAllSkippedCategories_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			PopulateMenuItems(showAll: true);
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

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMoved(e);
			if (((Control)this).get_Bottom() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() && ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() - e.get_MousePosition().Y < 20)
			{
				((Control)this).set_Bottom(((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom());
			}
			if (((Control)this).get_Top() < 0 && e.get_MousePosition().Y < 20)
			{
				((Control)this).set_Top(0);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Bottom() > ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom())
			{
				Rectangle scrollBounds = default(Rectangle);
				((Rectangle)(ref scrollBounds))._002Ector(4, ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() - 20, ((Control)this).get_Width() - 8, 20);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), scrollBounds, _backColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() - 20, ((Control)this).get_Width() - 6, 1), Color.get_DarkGray());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, ((Control)GameService.Graphics.get_SpriteScreen()).get_Bottom() - 20 + 1, ((Control)this).get_Width() - 6, 1), Color.get_LightGray());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureContinueMenu, new Rectangle(((Control)this).get_Width() / 2 - _textureContinueMenu.get_Width() / 2, ((Rectangle)(ref scrollBounds)).get_Bottom() - scrollBounds.Height / 2 - _textureContinueMenu.get_Height() / 2, _textureContinueMenu.get_Width(), _textureContinueMenu.get_Height()));
			}
			if (((Control)this).get_Top() < 0)
			{
				Rectangle scrollBounds2 = default(Rectangle);
				((Rectangle)(ref scrollBounds2))._002Ector(4, -((Control)this).get_Top(), ((Control)this).get_Width() - 8, 20);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), scrollBounds2, _backColor);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, -((Control)this).get_Top() + 20 - 1, ((Control)this).get_Width() - 6, 1), Color.get_DarkGray());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(2, -((Control)this).get_Top() + 20, ((Control)this).get_Width() - 6, 1), Color.get_LightGray());
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _textureContinueMenu, new Rectangle(((Control)this).get_Width() / 2 - _textureContinueMenu.get_Width() / 2, ((Rectangle)(ref scrollBounds2)).get_Bottom() - scrollBounds2.Height / 2 - _textureContinueMenu.get_Height() / 2, _textureContinueMenu.get_Width(), _textureContinueMenu.get_Height()), (Rectangle?)null, Color.get_White(), 0f, Vector2.get_Zero(), (SpriteEffects)2);
			}
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
		}
	}
}
