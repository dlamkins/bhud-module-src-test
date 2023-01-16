using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
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

		private static readonly Texture2D _textureContinueMenu = Control.get_Content().GetTexture("156057");

		private readonly Color _backColor = Color.FromNonPremultiplied(37, 36, 37, 255);

		private bool _forceShowAll;

		private const int SCROLLHINT_HEIGHT = 20;

		public CategoryContextMenuStrip(IPackState packState, PathingCategory pathingCategory, bool forceShowAll)
			: this()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_packState = packState;
			_pathingCategory = pathingCategory;
			_forceShowAll = forceShowAll;
		}

		private (IEnumerable<PathingCategory> SubCategories, int Skipped) GetSubCategories(bool forceShowAll = false)
		{
			IEnumerable<PathingCategory> subCategories = _pathingCategory.Where((PathingCategory cat) => cat.LoadedFromPack && cat.DisplayName != "");
			if (!_packState.UserConfiguration.PackEnableSmartCategoryFilter.get_Value() || forceShowAll)
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
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
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
				((Control)val).set_BasicTooltipText(string.Format(Strings.Info_HiddenCategories, ((SettingEntry)_packState.UserConfiguration.PackEnableSmartCategoryFilter).get_DisplayName()));
				ContextMenuStripItem showAllSkippedCategories = val;
				((ContextMenuStrip)this).AddMenuItem(showAllSkippedCategories);
				((Control)showAllSkippedCategories).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ShowAllSkippedCategories_LeftMouseButtonReleased);
			}
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
