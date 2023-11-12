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
using MonoGame.Extended;
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

		private readonly bool _forceShowAll;

		private readonly List<(Texture2D, string, Action)> _contexts;

		public CategoryContextMenuStripItem(IPackState packState, PathingCategory pathingCategory, bool forceShowAll)
			: this()
		{
			_packState = packState;
			_pathingCategory = pathingCategory;
			_contexts = new List<(Texture2D, string, Action)>();
			_forceShowAll = forceShowAll;
			BuildCategoryMenu();
			DetectAndBuildContexts();
		}

		private void BuildCategoryMenu()
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			if (!_packState.UserConfiguration.PackTruncateLongCategoryNames.get_Value())
			{
				((ContextMenuStripItem)this).set_Text(_pathingCategory.DisplayName);
			}
			else
			{
				string text = _pathingCategory.DisplayName;
				Size2 textSize = GameService.Content.get_DefaultFont14().MeasureString(text);
				while (textSize.Width > (float)_packState.UserResourceStates.Advanced.CategoryNameTruncateWidth)
				{
					if (text.Length <= 1)
					{
						text = _pathingCategory.DisplayName;
						break;
					}
					text = text.Substring(0, text.Length - 2);
					textSize = GameService.Content.get_DefaultFont14().MeasureString(text + "...");
				}
				((ContextMenuStripItem)this).set_Text((text == _pathingCategory.DisplayName) ? text : (text.Trim() + "..."));
			}
			if (_packState.CategoryStates != null)
			{
				if ((_forceShowAll && _pathingCategory.Any()) || _pathingCategory.Any((PathingCategory c) => CategoryUtil.UiCategoryIsNotFiltered(c, _packState)))
				{
					((ContextMenuStripItem)this).set_Submenu((ContextMenuStrip)(object)new CategoryContextMenuStrip(_packState, _pathingCategory, _forceShowAll));
				}
				((ContextMenuStripItem)this).set_CanCheck(true);
				((ContextMenuStripItem)this).set_Checked(!_packState.CategoryStates.GetCategoryInactive(_pathingCategory));
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((ContextMenuStripItem)this).OnMouseEntered(e);
			if (!((Control)this).get_Enabled())
			{
				ContextMenuStrip submenu = ((ContextMenuStripItem)this).get_Submenu();
				if (submenu != null)
				{
					submenu.Show((Control)(object)this);
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
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			IAttribute descriptionAttr;
			if (_pathingCategory.TryGetAggregatedAttributeValue("achievementid", out var achievementAttr))
			{
				int achievementBit = -1;
				int achievementBitParsed = default(int);
				if (_pathingCategory.TryGetAggregatedAttributeValue("achievementbit", out var achievementBitAttr) && InvariantUtil.TryParseInt(achievementBitAttr, ref achievementBitParsed))
				{
					achievementBit = achievementBitParsed;
				}
				int achievementId = default(int);
				if (!InvariantUtil.TryParseInt(achievementAttr, ref achievementId) || achievementId < 0)
				{
					return;
				}
				((Control)this).set_Tooltip(new Tooltip((ITooltipView)(object)new AchievementTooltipView(achievementId, achievementBit)));
				if (_packState.UserConfiguration.PackAllowMarkersToAutomaticallyHide.get_Value())
				{
					((Control)this).set_Enabled(!_packState.AchievementStates.IsAchievementHidden(achievementId, achievementBit));
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
			if (((Control)this).get_Enabled() && !_pathingCategory.IsSeparator)
			{
				_packState.CategoryStates.SetInactive(_pathingCategory, !e.get_Checked());
			}
			((ContextMenuStripItem)this).OnCheckedChanged(e);
		}

		private bool TryGetCopyDetails(out string copyValue, out string copyMessage)
		{
			copyValue = string.Empty;
			copyMessage = "'{0}' copied to clipboard.";
			if (_pathingCategory.ExplicitAttributes.TryGetAttribute("copy", out var copyValueAttr))
			{
				copyValue = copyValueAttr.GetValueAsString();
				if (_packState.UserConfiguration.PackMarkerConsentToClipboard.get_Value() == MarkerClipboardConsentLevel.Never)
				{
					return false;
				}
				if (_pathingCategory.ExplicitAttributes.TryGetAttribute("copy-message", out var copyMessageAttr))
				{
					copyMessage = copyMessageAttr.GetValueAsString();
				}
				copyMessage = string.Format(copyMessage, copyValue);
				return true;
			}
			return false;
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			if (_pathingCategory.IsSeparator && TryGetCopyDetails(out var copyValue, out var copyMessage))
			{
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(copyValue).ContinueWith(delegate(Task<bool> t)
				{
					if (t.IsCompleted && t.Result)
					{
						ScreenNotification.ShowNotification(string.Format(copyMessage, copyValue), (NotificationType)0, (Texture2D)null, 2);
					}
				});
			}
			else if (!_pathingCategory.IsSeparator)
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
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			((ContextMenuStripItem)this).set_CanCheck(!_pathingCategory.IsSeparator);
			((ContextMenuStripItem)this).Paint(spriteBatch, bounds);
			((ContextMenuStripItem)this).set_CanCheck(true);
			int rightOffset = 18;
			for (int i = 0; i < _contexts.Count; i++)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, _contexts[i].Item1, new Rectangle(((Rectangle)(ref bounds)).get_Right() - rightOffset - 18 * (_contexts.Count - i) + 1, ((Control)this).get_Height() / 2 - 8, 16, 16));
			}
		}
	}
}
