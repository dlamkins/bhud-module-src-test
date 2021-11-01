using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Tooltips;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.UI.Controls
{
	public class CategoryContextMenuStripItem : ContextMenuStripItem
	{
		private const int BULLET_SIZE = 18;

		private const int HORIZONTAL_PADDING = 6;

		private const int TEXT_LEFTPADDING = 30;

		private readonly IPackState _packState;

		private readonly PathingCategory _pathingCategory;

		private readonly List<(Texture2D, string, Action)> _contexts;

		public CategoryContextMenuStripItem(IPackState packState, PathingCategory pathingCategory)
			: this()
		{
			_packState = packState;
			_pathingCategory = pathingCategory;
			_contexts = new List<(Texture2D, string, Action)>();
			BuildCategoryMenu();
			DetectAndBuildContexts();
		}

		private void BuildCategoryMenu()
		{
			((ContextMenuStripItem)this).set_Text(_pathingCategory.DisplayName);
			if (_packState.CategoryStates != null)
			{
				if (_pathingCategory.Any((PathingCategory c) => CategoryUtil.UiCategoryIsNotFiltered(c, _packState)))
				{
					((ContextMenuStripItem)this).set_Submenu((ContextMenuStrip)(object)new CategoryContextMenuStrip(_packState, _pathingCategory));
				}
				if (!_pathingCategory.IsSeparator)
				{
					((ContextMenuStripItem)this).set_CanCheck(true);
					((ContextMenuStripItem)this).set_Checked(!_packState.CategoryStates.GetCategoryInactive(_pathingCategory));
				}
			}
		}

		private void AddAchievementContext(int achievementId)
		{
			if (achievementId >= 0)
			{
				((IBulkExpandableClient<Achievement, int>)(object)PathingModule.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Achievements()).GetAsync(achievementId, default(CancellationToken)).ContinueWith(delegate(Task<Achievement> achievementTask)
				{
					((Control)this).set_BasicTooltipText("[Achievement]\r\n\r\n " + DrawUtil.WrapText(GameService.Content.get_DefaultFont14(), achievementTask.Result.get_Description(), 300f) + "\r\n\r\n" + DrawUtil.WrapText(GameService.Content.get_DefaultFont14(), achievementTask.Result.get_Requirement(), 300f));
				}, TaskContinuationOptions.NotOnFaulted);
				_contexts.Add((PathingModule.Instance.ContentsManager.GetTexture("png/context/155062.png"), "", delegate
				{
				}));
				_contexts.Add((PathingModule.Instance.ContentsManager.GetTexture("png/context/102365.png"), "", delegate
				{
				}));
			}
		}

		private void DetectAndBuildContexts()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			IAttribute descriptionAttr;
			if (_pathingCategory.TryGetAggregatedAttributeValue("achievementid", out var achievementAttr))
			{
				int achievementId = default(int);
				if (!InvariantUtil.TryParseInt(achievementAttr, ref achievementId) || achievementId < 0)
				{
					return;
				}
				((Control)this).set_Tooltip(new Tooltip((ITooltipView)(object)new AchievementTooltipView(achievementId)));
				if (_packState.UserConfiguration.PackAllowMarkersToAutomaticallyHide.get_Value())
				{
					((Control)this).set_Enabled(!_packState.AchievementStates.IsAchievementHidden(achievementId, -1));
					if (!((Control)this).get_Enabled())
					{
						((ContextMenuStripItem)this).set_Checked(false);
					}
				}
			}
			else if (_pathingCategory.ExplicitAttributes.TryGetAttribute("tip-description", out descriptionAttr))
			{
				((Control)this).set_Tooltip(new Tooltip((ITooltipView)(object)new DescriptionTooltipView(null, descriptionAttr.Value)));
			}
		}

		protected override void OnCheckedChanged(CheckChangedEvent e)
		{
			if (((Control)this).get_Enabled())
			{
				_packState.CategoryStates.SetInactive(_pathingCategory, !e.get_Checked());
			}
			((ContextMenuStripItem)this).OnCheckedChanged(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			if (((ContextMenuStripItem)this).get_CanCheck())
			{
				if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
				{
					Container parent = ((Control)this).get_Parent();
					foreach (CategoryContextMenuStripItem item in ((parent != null) ? ((IEnumerable<Control>)parent.get_Children()).Where((Control child) => child is CategoryContextMenuStripItem).Cast<CategoryContextMenuStripItem>() : null) ?? Enumerable.Empty<CategoryContextMenuStripItem>())
					{
						((ContextMenuStripItem)item).set_Checked(item == this);
					}
					return;
				}
				if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)4))
				{
					((ContextMenuStripItem)this).set_Checked(true);
				}
			}
			((ContextMenuStripItem)this).OnClick(e);
		}

		public override void RecalculateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			int nWidth = (int)GameService.Content.get_DefaultFont14().MeasureString(((ContextMenuStripItem)this).get_Text()).Width + 30 + 30 + 30 * _contexts.Count;
			((Control)this).set_Width((((Control)this).get_Parent() != null) ? Math.Max(((Control)((Control)this).get_Parent()).get_Width() - 4, nWidth) : nWidth);
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			((ContextMenuStripItem)this).Paint(spriteBatch, bounds);
			int rightOffset = 18;
			for (int i = 0; i < _contexts.Count; i++)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _contexts[i].Item1, new Rectangle(((Rectangle)(ref bounds)).get_Right() - rightOffset - 18 * (_contexts.Count - i) + 1, ((Control)this).get_Height() / 2 - 8, 16, 16));
			}
		}
	}
}
