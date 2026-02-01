using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Controls.TreeView;
using BhModule.Community.Pathing.UI.Models;
using BhModule.Community.Pathing.UI.Tooltips;
using BhModule.Community.Pathing.UI.Views;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.UI.Controls.TreeNodes
{
	public class PathingCategoryNode : PathingNode
	{
		private bool _active = true;

		private readonly IPackState _packState;

		private readonly IList<IPathingEntity> _entities;

		private readonly bool _forceShowAll;

		private int _achievementId;

		private int _achievementBit;

		private bool _achievementHidden;

		protected Label PackNameControl;

		private Tooltip _categoryPathTooltip;

		private ViewContainer _confirmationContainer;

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				if (((Control)this).SetProperty<bool>(ref _active, value, false, "Active"))
				{
					UpdateChildrenActiveState();
					UpdateLabelActiveState();
				}
			}
		}

		public PathingCategory PathingCategory { get; }

		private PathingCategory PackCategory { get; }

		public bool IsSearchResult { get; init; }

		public PathingCategoryNode(IPackState packState, PathingCategory pathingCategory, bool showForceAll)
			: base(pathingCategory.DisplayName)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			_packState = packState;
			PathingCategory = pathingCategory;
			_entities = CategoryUtil.GetAssociatedPathingEntities(pathingCategory, packState.Entities).ToList();
			_forceShowAll = showForceAll;
			if (pathingCategory.IsSeparator)
			{
				base.Checkable = false;
				base.TextColor = Color.get_LightYellow();
				BackgroundOpacity = 0.3f;
				ShowIconTooltip = false;
			}
			else
			{
				base.Checkable = true;
				if (_entities.Count <= 0 && (PathingCategory.IsSeparator || PathingCategory.Count > 0))
				{
					BackgroundOpacity = 0.3f;
				}
				else
				{
					BackgroundOpacity = 0.05f;
					BackgroundOpaqueColor = Color.get_LightYellow();
				}
			}
			DetectAndBuildContexts();
			if (base.Checkable)
			{
				base.Checked = !_packState.CategoryStates.GetCategoryInactive(pathingCategory);
				UpdateActiveState(base.Checked);
			}
			PackCategory = PathingCategory.GetParents().LastOrDefault();
			CheckedChanged = (EventHandler<CheckChangedEvent>)Delegate.Combine(CheckedChanged, new EventHandler<CheckChangedEvent>(CheckboxOnCheckedChanged));
		}

		public override void Build()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			IconPaddingTop = 4;
			List<PathingTexture> iconTextures = new List<PathingTexture>(new List<PathingTexture>
			{
				new PathingTexture
				{
					Icon = AsyncTexture2D.FromAssetId(255302)
				}
			});
			if (PathingCategory.IsSeparator)
			{
				IconTextures = iconTextures;
				IconSize = new Point(25, 25);
				IconPaddingTop = 8;
			}
			else
			{
				IconTextures = GetEntityTextures().ToList();
			}
			BuildTooltip();
			base.Build();
			UpdateLabelActiveState();
			BuildAchievementTexture();
			if (IsSearchResult)
			{
				((Control)LabelControl).add_MouseEntered((EventHandler<MouseEventArgs>)NameControlOnMouseEntered);
				BuildPackName();
			}
			BuildEntityCount();
			if (((Control)this).get_Menu() != null)
			{
				BuildContextMenu();
			}
		}

		private void BuildPackName()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			if (PackCategory != null)
			{
				Label packNameControl = PackNameControl;
				if (packNameControl != null)
				{
					((Control)packNameControl).Dispose();
				}
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_propertiesPanel);
				val.set_Text(PackCategory.DisplayName);
				((Control)val).set_Height(base.PanelHeight);
				val.set_AutoSizeWidth(true);
				val.set_Font(GameService.Content.get_DefaultFont16());
				val.set_TextColor(Color.get_Orange());
				val.set_WrapText(true);
				val.set_StrokeText(true);
				((Control)val).set_BasicTooltipText(((Control)this).get_BasicTooltipText());
				PackNameControl = val;
				((Control)PackNameControl).add_MouseEntered((EventHandler<MouseEventArgs>)NameControlOnMouseEntered);
			}
		}

		private void NameControlOnMouseEntered(object sender, MouseEventArgs e)
		{
			if (_categoryPathTooltip == null)
			{
				BuildCategoryPathTooltip();
			}
		}

		public void InvalidatePath()
		{
			Tooltip categoryPathTooltip = _categoryPathTooltip;
			if (categoryPathTooltip != null)
			{
				((Control)categoryPathTooltip).Dispose();
			}
			_categoryPathTooltip = null;
		}

		private void BuildCategoryPathTooltip()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			Tooltip categoryPathTooltip = _categoryPathTooltip;
			if (categoryPathTooltip != null)
			{
				((Control)categoryPathTooltip).Dispose();
			}
			_categoryPathTooltip = new Tooltip((ITooltipView)(object)new CategoryPathTooltip(PathingCategory, _packState));
			if (PackNameControl != null)
			{
				((Control)PackNameControl).set_Tooltip(_categoryPathTooltip);
			}
			if (LabelControl != null)
			{
				((Control)LabelControl).set_Tooltip(_categoryPathTooltip);
			}
		}

		private void BuildAchievementTexture()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Expected O, but got Unknown
			if (_achievementId > 0)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_propertiesPanel);
				((Control)val).set_Size(new Point(30, ((Control)this).get_Height()));
				Panel achievementIconContainer = val;
				Image val2 = new Image();
				((Control)val2).set_Parent((Container)(object)achievementIconContainer);
				((Control)val2).set_Size(new Point(30, 30));
				((Control)val2).set_Top(4);
				val2.set_Texture(_achievementHidden ? AsyncTexture2D.op_Implicit(PathingModule.Instance.ContentsManager.GetTexture("png\\155061+255218.png")) : AsyncTexture2D.FromAssetId(155061));
				Image tooltipIcon = val2;
				if (_packState.UserConfiguration.PackShowTooltipsOnAchievements.get_Value())
				{
					((Control)tooltipIcon).set_Tooltip(new Tooltip((ITooltipView)(object)new AchievementTooltipView(_achievementId, _achievementBit)));
				}
				else
				{
					((Control)tooltipIcon).set_BasicTooltipText("Achievement tooltips have been disabled in the settings: Pathing Module Settings > Marker Options > Show Tooltips for Achievements");
				}
			}
		}

		private void BuildEntityCount()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			if (_entities.Count <= 0)
			{
				if (!PathingCategory.IsSeparator && PathingCategory.Count <= 0)
				{
					Label val = new Label();
					((Control)val).set_Parent((Container)(object)_propertiesPanel);
					val.set_Text("not loaded");
					((Control)val).set_Height(base.PanelHeight);
					val.set_AutoSizeWidth(true);
					val.set_Font(GameService.Content.get_DefaultFont16());
					val.set_TextColor(StandardColors.get_Yellow());
					val.set_StrokeText(true);
					((Control)val).set_BasicTooltipText("No markers or trails have been loaded for this category, likely because there are none for the current map.");
				}
				return;
			}
			int markerCount = _entities.OfType<StandardMarker>().Count();
			if (markerCount > 0)
			{
				string affix2 = ((markerCount > 1) ? "markers" : "marker");
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_propertiesPanel);
				val2.set_Text($"{markerCount} {affix2}");
				((Control)val2).set_Height(base.PanelHeight);
				val2.set_AutoSizeWidth(true);
				val2.set_Font(GameService.Content.get_DefaultFont16());
				val2.set_TextColor(Color.get_LightBlue());
				val2.set_StrokeText(true);
				((Control)val2).set_BasicTooltipText(((Control)this).get_BasicTooltipText());
			}
			int trailCount = _entities.OfType<StandardTrail>().Count();
			if (trailCount > 0)
			{
				string affix = ((trailCount > 1) ? "trails" : "trail");
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)_propertiesPanel);
				val3.set_Text($"{trailCount} {affix}");
				((Control)val3).set_Height(base.PanelHeight);
				val3.set_AutoSizeWidth(true);
				val3.set_Font(GameService.Content.get_DefaultFont16());
				val3.set_TextColor(Color.get_LightYellow());
				val3.set_StrokeText(true);
				((Control)val3).set_BasicTooltipText(((Control)this).get_BasicTooltipText());
			}
		}

		private void BuildTooltip()
		{
			if (PathingCategory.ExplicitAttributes.TryGetAttribute("tip-description", out var descriptionAttr))
			{
				((Control)this).set_BasicTooltipText(descriptionAttr.Value);
			}
		}

		protected override void BuildContextMenu()
		{
			base.BuildContextMenu();
			BuildCopyItem();
			BuildCopyPath();
			if (IsSearchResult)
			{
				BuildOpenInTree();
			}
		}

		private void BuildCopyItem()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			if (PathingCategory.IsSeparator && PathingCategory.TryGetCopy(out var copyValue) && !string.IsNullOrWhiteSpace(copyValue))
			{
				ContextMenuStripItem val = new ContextMenuStripItem("Copy: " + copyValue);
				((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
				PathingCategory.TryGetCopyMessage(out var copyMessage);
				if (string.IsNullOrWhiteSpace(copyMessage))
				{
					copyMessage = "'{0}' copied to clipboard.";
				}
				((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CopyToClipboard(copyValue, copyMessage);
				});
			}
		}

		private void BuildCopyPath()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStripItem val = new ContextMenuStripItem("Copy Path");
			((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				CopyToClipboard(PathingCategory.GetPath(), "Path copied to clipboard");
			});
		}

		private void BuildOpenInTree()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStripItem val = new ContextMenuStripItem("Open In Explorer");
			((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				BhModule.Community.Pathing.UI.Controls.TreeView.TreeView treeView = base.TreeView;
				treeView.LoadNodes();
				treeView.NavigateToPath(PathingCategory.GetPath());
			});
		}

		private void CheckboxOnCheckedChanged(object sender, CheckChangedEvent e)
		{
			if (((Control)this).get_Enabled() && !PathingCategory.IsSeparator)
			{
				_packState.CategoryStates.SetInactive(PathingCategory, !e.get_Checked());
			}
			InvalidatePath();
			UpdateActiveState(e.get_Checked());
			if (!ParentIsActive() && e.get_Checked())
			{
				base.TreeView.SkipNextStateCheck(this);
				ShowConfirmationWindow();
			}
		}

		private void ShowConfirmationWindow()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			TabbedWindow2 window = PathingModule.Instance.SettingsWindow;
			ViewContainer confirmationContainer = _confirmationContainer;
			if (confirmationContainer != null)
			{
				((Control)confirmationContainer).Dispose();
			}
			ViewContainer val = new ViewContainer();
			((Control)val).set_Size(new Point(400, 400));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Location(new Point(((Control)window).get_Location().X + (((Control)window).get_Size().X / 2 - 200), ((Control)window).get_Location().Y + (((Control)window).get_Size().Y / 2 - 200)));
			((Control)val).set_ZIndex(2147483645);
			val.set_FadeView(true);
			((Control)val).set_Parent((Container)(object)Control.get_Graphics().get_SpriteScreen());
			_confirmationContainer = val;
			_confirmationContainer.Show((IView)(object)new ConfirmationView(PathingCategory, _packState));
		}

		public void UpdateActiveState(bool active)
		{
			Active = active && ParentIsActive();
			if (Active && _achievementHidden && _packState.UserConfiguration.PackAllowMarkersToAutomaticallyHide.get_Value())
			{
				Active = false;
			}
		}

		public bool ParentIsActive()
		{
			PathingCategoryNode parentNode = ((Control)this).get_Parent() as PathingCategoryNode;
			if (parentNode != null)
			{
				if (base.Checked)
				{
					return parentNode.ParentIsActive();
				}
				return false;
			}
			return PathingCategory.ParentIsActive(_packState);
		}

		private void UpdateChildrenActiveState()
		{
			foreach (PathingCategoryNode item in from n in base.ChildBaseNodes.OfType<PathingCategoryNode>()
				where n.Checkable
				select n)
			{
				item.UpdateActiveState(item.Checked);
			}
		}

		protected override void OnParentChanged()
		{
			base.OnParentChanged();
			if (base.Checkable)
			{
				UpdateActiveState(base.Checked);
			}
		}

		public void UpdateLabelActiveState()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (LabelControl != null)
			{
				LabelControl.set_TextColor(Active ? base.TextColor : (Color.get_LightGray() * 0.7f));
				LabelControl.set_StrokeText(Active);
				LabelControl.set_ShowShadow(!Active);
			}
		}

		public int AddSubNodes(bool forceShowAll)
		{
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			if (PathingCategory.Count <= 0)
			{
				return 0;
			}
			var (subCategories, skipped) = PathingCategory.FilterCategories(_packState, forceShowAll);
			foreach (PathingCategory subCategory in subCategories)
			{
				if (subCategory != null)
				{
					if (((Control)this).get_Parent() == null)
					{
						break;
					}
					PathingCategoryNode pathingCategoryNode = new PathingCategoryNode(_packState, subCategory, forceShowAll);
					((Control)pathingCategoryNode).set_Width(((Control)((Control)this).get_Parent()).get_Width() - 14);
					((Control)pathingCategoryNode).set_Parent((Container)(object)this);
					((Control)pathingCategoryNode).set_Visible(base.Expanded);
					pathingCategoryNode.IsSearchResult = IsSearchResult;
				}
			}
			if (skipped > 0 && ((Control)this).get_Parent() != null && _packState.UserConfiguration.PackShowWhenCategoriesAreFiltered.get_Value())
			{
				LabelNode obj = new LabelNode($"{skipped} hidden (click to show)", AsyncTexture2D.FromAssetId(358463))
				{
					Clickable = true
				};
				((Control)obj).set_Width(((Control)((Control)this).get_Parent()).get_Width() - 14);
				obj.TextColor = Color.get_LightYellow();
				((Control)obj).set_BasicTooltipText(string.Format(Strings.Info_HiddenCategories, ((SettingEntry)_packState.UserConfiguration.PackEnableSmartCategoryFilter).get_DisplayName()));
				((Control)obj).set_Parent((Container)(object)this);
				((Control)obj).add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)ShowAllSkippedCategories_LeftMouseButtonReleased);
			}
			return skipped;
		}

		protected override void OnShown(EventArgs e)
		{
			ClearChildNodes();
			AddSubNodes(_forceShowAll);
			base.OnShown(e);
		}

		protected override void OnHidden(EventArgs e)
		{
			HideChildren();
			((Control)this).OnHidden(e);
		}

		private void ShowAllSkippedCategories_LeftMouseButtonReleased(object sender, MouseEventArgs e)
		{
			ClearChildNodes();
			AddSubNodes(forceShowAll: true);
		}

		private IEnumerable<PathingTexture> GetEntityTextures()
		{
			HashSet<Texture2D> uniqueTextures = new HashSet<Texture2D>();
			return _entities.Select(delegate(IPathingEntity e)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Unknown result type (might be due to invalid IL or missing references)
				StandardMarker standardMarker = e as StandardMarker;
				if (standardMarker != null)
				{
					return new PathingTexture
					{
						Icon = standardMarker.Texture,
						Tint = standardMarker.Tint
					};
				}
				StandardTrail standardTrail = e as StandardTrail;
				return (standardTrail != null) ? new PathingTexture
				{
					Icon = standardTrail.Texture,
					Tint = standardTrail.Tint
				} : null;
			}).Where(delegate(PathingTexture pt)
			{
				object obj;
				if (pt == null)
				{
					obj = null;
				}
				else
				{
					AsyncTexture2D icon = pt.Icon;
					obj = ((icon != null) ? icon.get_Texture() : null);
				}
				return obj != null && uniqueTextures.Add(pt.Icon.get_Texture());
			});
		}

		private void DetectAndBuildContexts()
		{
			if (!PathingCategory.TryGetAchievementId(out _achievementId))
			{
				return;
			}
			PathingCategory.TryGetAchievementBit(out _achievementBit);
			if (_packState.UserConfiguration.PackAllowMarkersToAutomaticallyHide.get_Value())
			{
				_achievementHidden = _packState.AchievementStates.IsAchievementHidden(_achievementId, _achievementBit);
				if (_achievementHidden)
				{
					CheckDisabled = true;
				}
			}
		}

		protected override void DisposeControl()
		{
			Tooltip tooltip = ((Control)this).get_Tooltip();
			if (tooltip != null)
			{
				((Control)tooltip).Dispose();
			}
			ContextMenuStrip menu = ((Control)this).get_Menu();
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			base.DisposeControl();
		}
	}
}
