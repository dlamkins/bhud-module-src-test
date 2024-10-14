using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Tabs
{
	public class AboutTab : Blish_HUD.Controls.Container
	{
		private readonly Kenedia.Modules.Core.Controls.TextBox _modifiedField;

		private readonly MultilineTextBox _noteField;

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _tagPanel;

		private readonly Kenedia.Modules.Core.Controls.Label _modifiedLabel;

		private readonly Kenedia.Modules.Core.Controls.Label _notesLabel;

		private readonly Kenedia.Modules.Core.Controls.Label _tagsLabel;

		private readonly ButtonImage _addTag;

		private readonly FilterBox _tagFilter;

		private readonly List<(TemplateFlag tag, Kenedia.Modules.Core.Controls.Image texture, Kenedia.Modules.Core.Controls.Checkbox checkbox)> _tags = new List<(TemplateFlag, Kenedia.Modules.Core.Controls.Image, Kenedia.Modules.Core.Controls.Checkbox)>();

		private readonly List<(EncounterFlag tag, Kenedia.Modules.Core.Controls.Image texture, Kenedia.Modules.Core.Controls.Checkbox checkbox)> _encounters = new List<(EncounterFlag, Kenedia.Modules.Core.Controls.Image, Kenedia.Modules.Core.Controls.Checkbox)>();

		private readonly TemplateTagComparer _comparer;

		private readonly bool _created;

		private int _tagSectionWidth;

		private bool _changeBuild = true;

		private Color _disabledColor = Color.get_Gray();

		private Dictionary<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> _tagControls = new Dictionary<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>>();

		private Kenedia.Modules.Core.Controls.FlowPanel _ungroupedPanel;

		public TemplateTags TemplateTags { get; }

		public TagGroups TagGroups { get; }

		public TemplatePresenter TemplatePresenter { get; private set; }

		public MainWindow MainWindow { get; internal set; }

		public AboutTab(TemplatePresenter templatePresenter, TemplateTags templateTags, TagGroups tagGroups)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_045e: Unknown result type (might be due to invalid IL or missing references)
			TemplatePresenter = templatePresenter;
			TemplateTags = templateTags;
			TagGroups = tagGroups;
			_comparer = new TemplateTagComparer(TagGroups);
			HeightSizingMode = SizingMode.Fill;
			WidthSizingMode = SizingMode.Fill;
			_tagSectionWidth = 300;
			_tagsLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Tags,
				Font = Control.Content.DefaultFont32,
				Height = 35,
				Width = _tagSectionWidth - 35 - 5,
				Location = new Point(0, 10)
			};
			_tagFilter = new FilterBox
			{
				Parent = this,
				Location = new Point(0, _tagsLabel.Bottom + 10),
				Width = _tagSectionWidth - 30,
				SetLocalizedPlaceholder = () => strings_common.Search,
				FilteringOnTextChange = true,
				FilteringOnEnter = true,
				EnterPressedAction = delegate(string txt)
				{
					string txt3 = txt;
					if (!string.IsNullOrEmpty(txt3.Trim()))
					{
						TemplateTag templateTag = TemplateTags.FirstOrDefault((TemplateTag e) => e.Name.ToLower() == txt3.ToLower());
						if (templateTag == null)
						{
							TemplateTags.Add(new TemplateTag
							{
								Name = txt3
							});
						}
						else
						{
							TagControl tagControl = _tagPanel.GetChildrenOfType<TagControl>().FirstOrDefault((TagControl e) => e.Tag == templateTag);
							tagControl?.SetSelected(!tagControl.Selected);
							_tagFilter.Focused = true;
						}
					}
				},
				TextChangedAction = delegate(string txt)
				{
					string txt2 = txt;
					_addTag.Enabled = !string.IsNullOrEmpty(txt2.Trim()) && TemplateTags.FirstOrDefault((TemplateTag e) => e.Name.ToLower() == txt2.ToLower()) == null;
				},
				PerformFiltering = new Action<string>(FilterTags)
			};
			_addTag = new ButtonImage
			{
				Parent = this,
				Size = new Point(_tagFilter.Height),
				Location = new Point(_tagFilter.Right + 2, _tagFilter.Top),
				Texture = AsyncTexture2D.FromAssetId(255443),
				HoveredTexture = AsyncTexture2D.FromAssetId(255297),
				DisabledTexture = AsyncTexture2D.FromAssetId(255296),
				SetLocalizedTooltip = () => strings.AddTag,
				Enabled = false,
				ClickAction = delegate
				{
					TemplateTags.Add(new TemplateTag
					{
						Name = (string.IsNullOrEmpty(_tagFilter.Text) ? TemplateTag.DefaultName : _tagFilter.Text)
					});
				}
			};
			_tagPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, _tagFilter.Bottom + 2),
				Width = _tagSectionWidth,
				HeightSizingMode = SizingMode.Fill,
				ShowBorder = false,
				BorderColor = Color.get_Black(),
				BorderWidth = new RectangleDimensions(2),
				BackgroundColor = Color.get_Black() * 0.4f,
				ShowRightBorder = true,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5),
				CanScroll = true
			};
			_notesLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => strings.Notes,
				Font = Control.Content.DefaultFont32,
				Location = new Point(_tagPanel.Right + 18, _tagsLabel.Top),
				Size = _tagsLabel.Size
			};
			_noteField = new MultilineTextBox
			{
				Parent = this,
				Location = new Point(_tagPanel.Right + 10, _notesLabel.Bottom + 10),
				HideBackground = false
			};
			_noteField.TextChanged += NoteField_TextChanged;
			_modifiedLabel = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				SetLocalizedText = () => string.Format(strings.LastModified, string.Empty).Substring(0, strings.LastModified.Length - 5),
				Font = Control.Content.DefaultFont16,
				Location = new Point(_tagPanel.Right + 18, _noteField.Bottom + 5),
				Size = _notesLabel.Size,
				HorizontalAlignment = HorizontalAlignment.Right
			};
			_modifiedField = new Kenedia.Modules.Core.Controls.TextBox
			{
				Parent = this,
				Location = new Point(_modifiedLabel.Right + 10, _modifiedLabel.Top),
				HideBackground = false,
				TextChangedAction = delegate(string s)
				{
					if (TemplatePresenter.Template != null)
					{
						TemplatePresenter.Template.LastModified = s;
					}
				}
			};
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TagGroups.GroupChanged += new PropertyAndValueChangedEventHandler(TagGroups_GroupChanged);
			TagGroups.GroupAdded += new EventHandler<TagGroup>(TagGroups_GroupAdded);
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_GroupRemoved);
			TemplateTags.TagAdded += new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved += new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged += new PropertyChangedEventHandler(TemplateTags_TagChanged);
			_tagPanel.ChildAdded += TagPanel_ChildsChanged;
			_tagPanel.ChildRemoved += TagPanel_ChildsChanged;
			TemplateTags.Loaded += new EventHandler(TemplateTags_Loaded);
			if (TemplateTags.IsLoaded)
			{
				CreateTagControls();
			}
			ApplyTemplate();
			_created = true;
		}

		private void TemplateTags_Loaded(object sender, EventArgs e)
		{
			CreateTagControls();
		}

		private void FilterTags(string txt)
		{
			string t = txt.ToLower();
			bool any = string.IsNullOrEmpty(t);
			_tagControls.SelectMany<KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>>, TagControl>((KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> x) => x.Value);
			foreach (KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> p in _tagControls)
			{
				foreach (TagControl tag in p.Value)
				{
					tag.Tag.Name.ToLower();
					tag.Tag.Group.ToLower();
					tag.Visible = any || tag.Tag.Name.ToLower().Contains(t) || tag.Tag.Group.ToLower().Contains(t);
				}
				p.Key.Visible = p.Value.Any((TagControl x) => x.Visible);
				p.Key.Invalidate();
			}
			_tagPanel.Invalidate();
		}

		private void TagGroups_GroupAdded(object sender, TagGroup e)
		{
			SortPanels();
		}

		private void TagGroups_GroupRemoved(object sender, TagGroup e)
		{
			SortPanels();
		}

		private void TagGroups_GroupChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			SortPanels();
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			base.Draw(spriteBatch, drawBounds, scissor);
		}

		private void TagPanel_ChildsChanged(object sender, ChildChangedEventArgs e)
		{
			SortPanels();
		}

		private void SortPanels()
		{
			_tagPanel.SortChildren(delegate(Kenedia.Modules.Core.Controls.FlowPanel x, Kenedia.Modules.Core.Controls.FlowPanel y)
			{
				Kenedia.Modules.Core.Controls.FlowPanel x2 = x;
				Kenedia.Modules.Core.Controls.FlowPanel y2 = y;
				TagGroup a = TagGroups.FirstOrDefault((TagGroup group) => group.Name == x2.Title);
				TagGroup b = TagGroups.FirstOrDefault((TagGroup group) => group.Name == y2.Title);
				return TemplateTagComparer.CompareGroups(a, b);
			});
		}

		private void TemplateTags_TagChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag tag = sender as TemplateTag;
			if (tag == null)
			{
				return;
			}
			switch (e.PropertyName)
			{
			case "Priority":
			case "Name":
			{
				Kenedia.Modules.Core.Controls.FlowPanel panel2 = GetPanel(tag.Group);
				new TemplateTagComparer(TagGroups);
				panel2.SortChildren<TagControl>(SortTagControls);
				break;
			}
			case "Group":
			{
				List<Kenedia.Modules.Core.Controls.FlowPanel> flowPanelsToDelete = new List<Kenedia.Modules.Core.Controls.FlowPanel>();
				foreach (KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> t2 in _tagControls)
				{
					TagControl control = t2.Value.FirstOrDefault((TagControl x) => x.Tag == tag);
					if (control == null)
					{
						continue;
					}
					Kenedia.Modules.Core.Controls.FlowPanel panel = t2.Key;
					Kenedia.Modules.Core.Controls.FlowPanel p = GetPanel(tag.Group);
					if (panel == p)
					{
						SortPanels();
						return;
					}
					control.Parent = p;
					p.Children.Add(control);
					p.SortChildren<TagControl>(SortTagControls);
					_tagControls[p].Add(control);
					_tagControls[panel].Remove(control);
					panel.Children.Remove(control);
					if (panel != _ungroupedPanel && panel.Children.Where((Control x) => x != control).Count() <= 0)
					{
						flowPanelsToDelete.Add(panel);
						panel.Dispose();
					}
					break;
				}
				if (flowPanelsToDelete.Count <= 0)
				{
					break;
				}
				foreach (Kenedia.Modules.Core.Controls.FlowPanel t in flowPanelsToDelete)
				{
					_tagControls.Remove(t);
				}
				break;
			}
			}
		}

		public Kenedia.Modules.Core.Controls.FlowPanel GetPanel(string title)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			string title2 = title;
			Kenedia.Modules.Core.Controls.FlowPanel panel = null;
			if (!string.IsNullOrEmpty(title2))
			{
				Kenedia.Modules.Core.Controls.FlowPanel p = _tagControls.Keys.FirstOrDefault((Kenedia.Modules.Core.Controls.FlowPanel x) => x.Title == title2);
				if (p != null)
				{
					panel = p;
				}
				if (panel == null)
				{
					panel = new Kenedia.Modules.Core.Controls.FlowPanel
					{
						Title = title2,
						Parent = _tagPanel,
						Width = _tagPanel.Width - 25,
						WidthSizingMode = SizingMode.Standard,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2),
						OuterControlPadding = new Vector2(25f, 0f),
						CanCollapse = true
					};
				}
			}
			if (panel == null)
			{
				Kenedia.Modules.Core.Controls.FlowPanel flowPanel = _ungroupedPanel;
				if (flowPanel == null)
				{
					Kenedia.Modules.Core.Controls.FlowPanel obj = new Kenedia.Modules.Core.Controls.FlowPanel
					{
						Title = TagGroup.DefaultName,
						Parent = _tagPanel,
						Width = _tagPanel.Width - 25,
						WidthSizingMode = SizingMode.Standard,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2),
						OuterControlPadding = new Vector2(25f, 0f),
						CanCollapse = true
					};
					Kenedia.Modules.Core.Controls.FlowPanel flowPanel2 = obj;
					_ungroupedPanel = obj;
					flowPanel = flowPanel2;
				}
				panel = flowPanel;
			}
			if (!_tagControls.ContainsKey(panel))
			{
				_tagControls.Add(panel, new List<TagControl>());
				SortPanels();
			}
			return panel;
		}

		private void RemoveEmptyPanels()
		{
			foreach (Kenedia.Modules.Core.Controls.FlowPanel p in _tagControls.Keys.ToList())
			{
				if (p != _ungroupedPanel && !p.Children.Any())
				{
					_tagControls.Remove(p);
					p.Dispose();
				}
			}
		}

		private void TemplateTags_TagRemoved(object sender, TemplateTag e)
		{
			RemoveTemplateTag(e);
		}

		private void TemplateTags_TagAdded(object sender, TemplateTag e)
		{
			AddTemplateTag(e);
		}

		private void RemoveTemplateTag(TemplateTag e)
		{
			TemplateTag e2 = e;
			TagControl tagControl = null;
			KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> p = _tagControls.FirstOrDefault<KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>>>((KeyValuePair<Kenedia.Modules.Core.Controls.FlowPanel, List<TagControl>> x) => x.Value.Any((TagControl x) => x.Tag == e2));
			Kenedia.Modules.Core.Controls.FlowPanel panel = p.Key;
			tagControl = p.Value.FirstOrDefault((TagControl x) => x.Tag == e2);
			tagControl?.Dispose();
			_tagControls[panel].Remove(tagControl);
			if (panel.Children.Any())
			{
				panel.SortChildren<TagControl>(SortTagControls);
				panel.Visible = panel.Children.OfType<TagControl>().Any((TagControl x) => x.Visible);
			}
			else
			{
				panel.Visible = panel.Children.OfType<TagControl>().Any((TagControl x) => x.Visible);
			}
			RemoveEmptyPanels();
		}

		private void AddTemplateTag(TemplateTag e)
		{
			TemplateTag e2 = e;
			Kenedia.Modules.Core.Controls.FlowPanel panel = GetPanel(e2.Group);
			_tagControls[panel].Add(new TagControl
			{
				Parent = panel,
				Tag = e2,
				Width = panel.Width - 25,
				OnEditClicked = delegate
				{
					if (MainWindow != null)
					{
						MainWindow.SelectedTab = MainWindow.TagEditViewTab;
						MainWindow.TagEditView.SetTagToEdit(e2);
					}
				},
				OnClicked = delegate(bool selected)
				{
					Template template = TemplatePresenter.Template;
					if (template != null)
					{
						if (selected)
						{
							template.Tags.Add(e2.Name);
						}
						else
						{
							template.Tags.Remove(e2.Name);
						}
					}
				}
			});
			SortPanels();
			panel.SortChildren<TagControl>(SortTagControls);
		}

		private int SortTagControls(TagControl x, TagControl y)
		{
			return _comparer.Compare(x.Tag, y.Tag);
		}

		private void CreateTagControls()
		{
			List<TemplateTag> list = TemplateTags.ToList();
			list.Sort(new TemplateTagComparer(TagGroups));
			List<string> added = new List<string>();
			foreach (TemplateTag tag in list)
			{
				GetPanel(tag.Group);
				AddTemplateTag(tag);
				added.Add(tag.Name);
			}
			SortPanels();
		}

		private void TemplatePresenter_TemplateChanged(object sender, ValueChangedEventArgs<Template> e)
		{
			ApplyTemplate();
		}

		private void NoteField_TextChanged(object sender, EventArgs e)
		{
			if (_changeBuild && TemplatePresenter.Template != null)
			{
				TemplatePresenter.Template.Description = _noteField.Text;
			}
		}

		private void ApplyTemplate()
		{
			_changeBuild = false;
			_modifiedField.Text = TemplatePresenter?.Template?.LastModified;
			_noteField.Text = TemplatePresenter?.Template?.Description;
			List<TagControl> list = new List<TagControl>(_tagPanel.GetChildrenOfType<TagControl>());
			list.AddRange(_tagPanel.GetChildrenOfType<Kenedia.Modules.Core.Controls.FlowPanel>().SelectMany((Kenedia.Modules.Core.Controls.FlowPanel x) => x.GetChildrenOfType<TagControl>()));
			foreach (TagControl tag in list)
			{
				tag.Selected = (TemplatePresenter?.Template?.Tags.Contains(tag.Tag.Name)).GetValueOrDefault();
			}
			_changeBuild = true;
		}

		private void TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			ApplyTemplate();
		}

		public override void RecalculateLayout()
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_created && _noteField != null)
			{
				_noteField.Size = new Point(base.Width - _tagPanel.Right - 15, base.Height - _noteField.Top - _modifiedField.Height - 5);
				_modifiedLabel.Location = new Point(_tagPanel.Right + 18, _noteField.Bottom);
				_modifiedField.Location = new Point(_modifiedLabel.Right + 10, _modifiedLabel.Top + 5);
				_modifiedField.Size = new Point(base.Width - _modifiedField.Left - 5, _modifiedField.Font.get_LineHeight() + 5);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			TemplatePresenter.TemplateChanged -= new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			_tagPanel.ChildAdded -= TagPanel_ChildsChanged;
			_tagPanel.ChildRemoved -= TagPanel_ChildsChanged;
			TagGroups.GroupChanged -= new PropertyAndValueChangedEventHandler(TagGroups_GroupChanged);
			TagGroups.GroupAdded -= new EventHandler<TagGroup>(TagGroups_GroupAdded);
			TagGroups.GroupRemoved -= new EventHandler<TagGroup>(TagGroups_GroupRemoved);
			TemplateTags.TagAdded -= new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved -= new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged -= new PropertyChangedEventHandler(TemplateTags_TagChanged);
			foreach (Control child in base.Children)
			{
				child?.Dispose();
			}
		}
	}
}
