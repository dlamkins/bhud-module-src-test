using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagGroupView : View
	{
		private Kenedia.Modules.Core.Controls.FlowPanel _groupsPanel;

		private GroupEditPanel _editPanel;

		private FilterBox _filterBox;

		private ImageButton _addButton;

		private List<GroupSelectable> _groupSelectables = new List<GroupSelectable>();

		public GroupSelectable SelectedGroup { get; set; }

		public Blish_HUD.Controls.Container BuildPanel { get; private set; }

		public TagGroups TagGroups { get; }

		public TagGroupView(TagGroups tagGroups)
		{
			TagGroups = tagGroups;
		}

		protected override void Build(Blish_HUD.Controls.Container buildPanel)
		{
			base.Build(buildPanel);
			BuildGroupView(buildPanel);
		}

		private void BuildGroupView(Blish_HUD.Controls.Container buildPanel)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			BuildPanel = buildPanel;
			_filterBox = new FilterBox
			{
				Parent = BuildPanel,
				Location = new Point(50, 0),
				Width = BuildPanel.Width - 75 - 27,
				SetLocalizedPlaceholder = () => strings_common.Search,
				FilteringOnTextChange = true,
				PerformFiltering = new Action<string>(FilterGroups)
			};
			_addButton = new ImageButton
			{
				Parent = BuildPanel,
				Location = new Point(_filterBox.Right + 2, _filterBox.Top),
				Texture = AsyncTexture2D.FromAssetId(155902),
				DisabledTexture = AsyncTexture2D.FromAssetId(155903),
				HoveredTexture = AsyncTexture2D.FromAssetId(155904),
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				Size = new Point(_filterBox.Height),
				ClickAction = delegate
				{
					TagGroups.Add(new TagGroup(_filterBox.Text));
				},
				SetLocalizedTooltip = () => "Add Group"
			};
			_groupsPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = BuildPanel,
				Location = new Point(50, _filterBox.Bottom + 5),
				ContentPadding = new RectangleDimensions(5, 5, 0, 0),
				Width = (BuildPanel.Width - 75) / 3,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				CanScroll = true,
				HeightSizingMode = SizingMode.Fill,
				ControlPadding = new Vector2(0f, 2f)
			};
			_editPanel = new GroupEditPanel(TagGroups)
			{
				Location = new Point(_groupsPanel.Right + 5, _filterBox.Bottom + 5),
				CanScroll = true,
				Parent = BuildPanel,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				Width = BuildPanel.Width - 75 - (_groupsPanel.Width + 5),
				HeightSizingMode = SizingMode.Fill
			};
			foreach (TagGroup g in TagGroups)
			{
				AddGroup(g);
			}
			TagGroups.GroupAdded += new EventHandler<TagGroup>(TagGroups_TagAdded);
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_TagRemoved);
			TagGroups.GroupChanged += new PropertyChangedEventHandler(TagGroups_TagChanged);
			BuildPanel.Resized += BuildPanel_Resized;
		}

		private void BuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			Point b = e.CurrentSize;
			if (_filterBox != null)
			{
				_filterBox.Width = b.X - 75 - 27;
			}
			if (_editPanel != null)
			{
				_editPanel.Width = b.X - 75 - (_groupsPanel.Width + 5);
			}
			if (_addButton != null)
			{
				_addButton.Location = new Point(_filterBox.Right + 2, _filterBox.Top);
			}
		}

		private void FilterGroups(string? obj = null)
		{
			if (obj == null)
			{
				obj = _filterBox.Text ?? string.Empty;
			}
			_groupsPanel.SuspendLayout();
			obj = obj!.ToLowerInvariant();
			bool isEmpty = string.IsNullOrEmpty(obj);
			foreach (GroupSelectable g in _groupSelectables)
			{
				g.Visible = isEmpty || g.Group.Name.ToLowerInvariant().Contains(obj);
			}
			_groupsPanel.ResumeLayout();
			_groupsPanel.Invalidate();
			SortSelectables();
		}

		private void SortSelectables()
		{
			_groupsPanel.SortChildren((GroupSelectable a, GroupSelectable b) => TemplateTagComparer.CompareGroups(a.Group, b.Group));
		}

		private void AddGroup(TagGroup g)
		{
			_groupSelectables.Add(new GroupSelectable(g, _groupsPanel, TagGroups)
			{
				OnClickAction = new Action<TagGroup>(SetGroupToEdit)
			});
			FilterGroups();
		}

		private void SetGroupToEdit(TagGroup group)
		{
			TagGroup group2 = group;
			SelectedGroup = _groupSelectables.FirstOrDefault((GroupSelectable x) => x.Group == group2);
			_editPanel.Group = group2;
			foreach (GroupSelectable groupSelectable in _groupSelectables)
			{
				groupSelectable.Selected = groupSelectable == SelectedGroup;
			}
		}

		private void TagGroups_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			FilterGroups();
		}

		private void TagGroups_TagRemoved(object sender, TagGroup e)
		{
			TagGroup e2 = e;
			GroupSelectable group = _groupSelectables.FirstOrDefault((GroupSelectable x) => x.Group == e2);
			if (group != null)
			{
				_groupSelectables.Remove(group);
				group.Dispose();
				FilterGroups();
			}
		}

		private void TagGroups_TagAdded(object sender, TagGroup e)
		{
			AddGroup(e);
		}

		protected override void Unload()
		{
			base.Unload();
			TagGroups.GroupAdded -= new EventHandler<TagGroup>(TagGroups_TagAdded);
			TagGroups.GroupRemoved -= new EventHandler<TagGroup>(TagGroups_TagRemoved);
			TagGroups.GroupChanged -= new PropertyChangedEventHandler(TagGroups_TagChanged);
			BuildPanel.Resized -= BuildPanel_Resized;
		}
	}
}
