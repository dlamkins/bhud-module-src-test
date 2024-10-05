using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagEditView : View
	{
		private Kenedia.Modules.Core.Controls.FlowPanel _tagsPanel;

		private TagEditPanel _tagEditPanel;

		private FilterBox _tagFilterBox;

		private ImageButton _tagAddButton;

		private List<TagSelectable> _tagSelectables = new List<TagSelectable>();

		private Kenedia.Modules.Core.Controls.FlowPanel _groupsPanel;

		private GroupEditPanel _groupEditPanel;

		private FilterBox _groupFilterBox;

		private ImageButton _groupAddButton;

		private List<GroupSelectable> _groupSelectables = new List<GroupSelectable>();

		private Kenedia.Modules.Core.Controls.Panel _tagsParent;

		private Kenedia.Modules.Core.Controls.Panel _groupParent;

		public TemplateTags TemplateTags { get; }

		public TagGroups TagGroups { get; }

		public TagSelectable SelectedTag { get; set; }

		public GroupSelectable SelectedGroup { get; set; }

		public TagEditView(TemplateTags templateTags, TagGroups tagGroups)
		{
			TemplateTags = templateTags;
			TagGroups = tagGroups;
		}

		protected override void Build(Blish_HUD.Controls.Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			base.Build(buildPanel);
			int width = buildPanel.Size.X;
			_tagsParent = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = buildPanel,
				HeightSizingMode = SizingMode.Fill,
				Width = width / 3 * 2
			};
			_groupParent = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = buildPanel,
				HeightSizingMode = SizingMode.Fill,
				Width = width / 3,
				Location = Kenedia.Modules.Core.Extensions.PointExtensions.Add(new Point(_tagsParent.Right, _tagsParent.Top), new Point(0, 0))
			};
			BuildTagView(_tagsParent);
			BuildGroupView(_groupParent);
			buildPanel.Resized += BuildPanel_Resized;
		}

		private void BuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			int width = e.CurrentSize.X;
		}

		private void BuildGroupView(Blish_HUD.Controls.Container buildPanel)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Label lbl = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = buildPanel,
				Location = new Point(0, 0),
				SetLocalizedText = () => strings.Groups,
				Font = GameService.Content.DefaultFont18,
				Height = GameService.Content.DefaultFont18.get_LineHeight() + 2,
				AutoSizeWidth = true
			};
			Separator obj = new Separator
			{
				Parent = buildPanel
			};
			int x = lbl.LocalBounds.X;
			Rectangle localBounds = lbl.LocalBounds;
			obj.Location = Kenedia.Modules.Core.Extensions.PointExtensions.Add(new Point(x, ((Rectangle)(ref localBounds)).get_Bottom()), new Point(0, 2));
			obj.Width = buildPanel.Width - 25;
			obj.Height = 2;
			obj.Color = Color.get_White() * 0.8f;
			Separator sep = obj;
			FilterBox obj2 = new FilterBox
			{
				Parent = buildPanel
			};
			int x2 = sep.LocalBounds.X;
			localBounds = sep.LocalBounds;
			obj2.Location = Kenedia.Modules.Core.Extensions.PointExtensions.Add(new Point(x2, ((Rectangle)(ref localBounds)).get_Bottom()), new Point(0, 10));
			obj2.Width = buildPanel.Width - 50;
			obj2.SetLocalizedPlaceholder = () => strings_common.Search;
			obj2.FilteringOnTextChange = true;
			obj2.PerformFiltering = new Action<string>(FilterGroups);
			_groupFilterBox = obj2;
			_groupAddButton = new ImageButton
			{
				Parent = buildPanel,
				Location = new Point(_groupFilterBox.Right + 2, _groupFilterBox.Top),
				Texture = AsyncTexture2D.FromAssetId(155902),
				DisabledTexture = AsyncTexture2D.FromAssetId(155903),
				HoveredTexture = AsyncTexture2D.FromAssetId(155904),
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				Size = new Point(_groupFilterBox.Height),
				ClickAction = delegate
				{
					TagGroups.Add(new TagGroup(_groupFilterBox.Text));
				},
				SetLocalizedTooltip = () => strings.AddGroup
			};
			_groupsPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = buildPanel,
				Location = new Point(_groupFilterBox.Left, _groupFilterBox.Bottom + 5),
				ContentPadding = new RectangleDimensions(5, 5, 0, 0),
				Width = buildPanel.Width / 2,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				CanScroll = true,
				HeightSizingMode = SizingMode.Fill,
				ControlPadding = new Vector2(0f, 2f)
			};
			_groupEditPanel = new GroupEditPanel(TagGroups)
			{
				Location = new Point(_groupsPanel.Right + 5, _groupFilterBox.Bottom + 5),
				CanScroll = true,
				Parent = buildPanel,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				Width = buildPanel.Width - (_groupsPanel.Right + 27),
				HeightSizingMode = SizingMode.Fill
			};
			foreach (TagGroup g in TagGroups)
			{
				AddGroup(g);
			}
			TagGroups.GroupAdded += new EventHandler<TagGroup>(TagGroups_TagAdded);
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_TagRemoved);
			TagGroups.GroupChanged += new PropertyChangedEventHandler(TagGroups_TagChanged);
			buildPanel.Resized += GroupBuildPanel_Resized;
		}

		private void FilterGroups(string? obj = null)
		{
			if (obj == null)
			{
				obj = _groupFilterBox.Text ?? string.Empty;
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

		private void BuildTagView(Blish_HUD.Controls.Container buildPanel)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			Kenedia.Modules.Core.Controls.Label lbl = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = buildPanel,
				Location = new Point(50, 0),
				SetLocalizedText = () => strings.Tags,
				Font = GameService.Content.DefaultFont18,
				Height = GameService.Content.DefaultFont18.get_LineHeight() + 2,
				AutoSizeWidth = true
			};
			Separator obj = new Separator
			{
				Parent = buildPanel
			};
			int x = lbl.LocalBounds.X;
			Rectangle localBounds = lbl.LocalBounds;
			obj.Location = Kenedia.Modules.Core.Extensions.PointExtensions.Add(new Point(x, ((Rectangle)(ref localBounds)).get_Bottom()), new Point(0, 2));
			obj.Width = buildPanel.Width - 75;
			obj.Height = 2;
			obj.Color = Color.get_White() * 0.8f;
			Separator sep = obj;
			FilterBox obj2 = new FilterBox
			{
				Parent = buildPanel
			};
			int x2 = sep.LocalBounds.X;
			localBounds = sep.LocalBounds;
			obj2.Location = Kenedia.Modules.Core.Extensions.PointExtensions.Add(new Point(x2, ((Rectangle)(ref localBounds)).get_Bottom()), new Point(0, 10));
			obj2.Width = buildPanel.Width - 75 - 27;
			obj2.SetLocalizedPlaceholder = () => strings_common.Search;
			obj2.FilteringOnTextChange = true;
			obj2.PerformFiltering = new Action<string>(FilterTags);
			_tagFilterBox = obj2;
			_tagAddButton = new ImageButton
			{
				Parent = buildPanel,
				Location = new Point(_tagFilterBox.Right + 2, _tagFilterBox.Top),
				Texture = AsyncTexture2D.FromAssetId(155902),
				DisabledTexture = AsyncTexture2D.FromAssetId(155903),
				HoveredTexture = AsyncTexture2D.FromAssetId(155904),
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				Size = new Point(_tagFilterBox.Height),
				ClickAction = delegate
				{
					TemplateTags.Add(new TemplateTag(_tagFilterBox.Text));
				},
				SetLocalizedTooltip = () => strings.AddTag
			};
			_tagsPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = buildPanel,
				Location = new Point(50, _tagFilterBox.Bottom + 5),
				ContentPadding = new RectangleDimensions(5, 5, 0, 0),
				Width = (buildPanel.Width - 75) / 3,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				CanScroll = true,
				HeightSizingMode = SizingMode.Fill,
				ControlPadding = new Vector2(0f, 2f)
			};
			_tagEditPanel = new TagEditPanel(TagGroups)
			{
				Location = new Point(_tagsPanel.Right + 5, _tagFilterBox.Bottom + 5),
				CanScroll = true,
				Parent = buildPanel,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.5f,
				Width = buildPanel.Width - 75 - (_tagsPanel.Width + 5),
				HeightSizingMode = SizingMode.Fill
			};
			foreach (TemplateTag g in TemplateTags)
			{
				AddTag(g);
			}
			TemplateTags.TagAdded += new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved += new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged += new PropertyChangedEventHandler(TemplateTags_TagChanged);
			buildPanel.Resized += TagBuildPanel_Resized;
		}

		private void GroupBuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Point b = e.CurrentSize;
		}

		private void TagBuildPanel_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Point b = e.CurrentSize;
		}

		private void FilterTags(string? obj = null)
		{
			if (obj == null)
			{
				obj = _tagFilterBox.Text ?? string.Empty;
			}
			_tagsPanel.SuspendLayout();
			obj = obj!.ToLowerInvariant();
			foreach (TagSelectable tagSelectable in _tagSelectables)
			{
				tagSelectable.Visible = tagSelectable.Tag.Name.ToLowerInvariant().Contains(obj);
			}
			_tagsPanel.ResumeLayout();
			_tagsPanel.Invalidate();
			SortSelectables();
		}

		private void SortSelectables()
		{
			TemplateTagComparer comparerer = new TemplateTagComparer(TagGroups);
			_tagsPanel.SortChildren((TagSelectable a, TagSelectable b) => comparerer.Compare(a.Tag, b.Tag));
		}

		private void AddTag(TemplateTag g)
		{
			_tagSelectables.Add(new TagSelectable(g, _tagsPanel, TemplateTags)
			{
				OnClickAction = new Action<TemplateTag>(SetTagToEdit)
			});
		}

		public void SetTagToEdit(TemplateTag tag)
		{
			TemplateTag tag2 = tag;
			SelectedTag = _tagSelectables.FirstOrDefault((TagSelectable x) => x.Tag == tag2);
			if (_tagEditPanel != null)
			{
				_tagEditPanel.Tag = tag2;
			}
			foreach (TagSelectable tagSelectable in _tagSelectables)
			{
				tagSelectable.Selected = tagSelectable == SelectedTag;
			}
		}

		private void TemplateTags_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			FilterTags();
		}

		private void TemplateTags_TagRemoved(object sender, TemplateTag e)
		{
			TemplateTag e2 = e;
			TagSelectable group = _tagSelectables.FirstOrDefault((TagSelectable x) => x.Tag == e2);
			if (group != null)
			{
				_tagSelectables.Remove(group);
				group.Dispose();
				FilterTags();
			}
		}

		private void TemplateTags_TagAdded(object sender, TemplateTag e)
		{
			AddTag(e);
			FilterTags();
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
			_groupEditPanel.Group = group2;
			foreach (GroupSelectable groupSelectable in _groupSelectables)
			{
				groupSelectable.Selected = groupSelectable == SelectedGroup;
			}
		}

		protected override void Unload()
		{
			base.Unload();
			TemplateTags.TagAdded -= new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved -= new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged -= new PropertyChangedEventHandler(TemplateTags_TagChanged);
			_tagsParent.Resized -= TagBuildPanel_Resized;
			_groupParent.Resized -= GroupBuildPanel_Resized;
		}
	}
}
