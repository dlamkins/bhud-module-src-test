using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class QuickFiltersPanel : AnchoredContainer
	{
		private int _specializationHeight = 270;

		private Rectangle _textBounds = Rectangle.get_Empty();

		private readonly DetailedTexture _headerSeparator = new DetailedTexture(605022);

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _tagPanel;

		private readonly Button _resetButton;

		private Dictionary<TagGroupPanel, List<TagToggle>> _tagControls = new Dictionary<TagGroupPanel, List<TagToggle>>();

		private TagGroupPanel _ungroupedPanel;

		private TagGroupPanel _specPanel;

		private List<TagToggle> _specToggles = new List<TagToggle>();

		public TemplateCollection Templates { get; }

		public TemplateTags TemplateTags { get; }

		public TagGroups TagGroups { get; }

		public SelectionPanel SelectionPanel { get; }

		public Settings Settings { get; }

		public Data Data { get; }

		public QuickFiltersPanel(TemplateCollection templates, TemplateTags templateTags, TagGroups tagGroups, SelectionPanel selectionPanel, Settings settings, Data data)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			Templates = templates;
			TemplateTags = templateTags;
			TagGroups = tagGroups;
			SelectionPanel = selectionPanel;
			Settings = settings;
			Data = data;
			base.Parent = Control.Graphics.SpriteScreen;
			base.Width = 205;
			base.Height = 640;
			base.ContentPadding = new RectangleDimensions(5);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(155985);
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(430, 30, 250, 600);
			}
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.Visible = false;
			Kenedia.Modules.Core.Controls.FlowPanel fp = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				Location = new Point(0, 30),
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Fill
			};
			_tagPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = fp,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Standard,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				ContentPadding = new RectangleDimensions(5),
				ControlPadding = new Vector2(5f),
				CanScroll = true
			};
			_resetButton = new Button
			{
				SetLocalizedText = () => string.Format(strings.ResetAll, strings.Filters),
				Width = 192,
				Parent = fp,
				ClickAction = new Action(ResetAllToggles)
			};
			base.FadeSteps = 150;
			GameService.Gw2Mumble.PlayerCharacter.SpecializationChanged += PlayerCharacter_SpecializationChanged;
			Settings.QuickFiltersPanelFade.SettingChanged += QuickFiltersPanelFade_SettingChanged;
			Settings.QuickFiltersPanelFadeDelay.SettingChanged += QuickFiltersPanelFadeDelay_SettingChanged;
			Settings.QuickFiltersPanelFadeDuration.SettingChanged += QuickFiltersPanelFadeDuration_SettingChanged;
			ApplySettings();
			SetAutoFilters(GameService.Gw2Mumble.PlayerCharacter?.Specialization ?? 0);
			TagGroups.GroupAdded += new EventHandler<TagGroup>(TagGroups_GroupAdded);
			TagGroups.GroupRemoved += new EventHandler<TagGroup>(TagGroups_GroupRemoved);
			TagGroups.GroupChanged += new PropertyAndValueChangedEventHandler(TagGroups_GroupChanged);
			TemplateTags.TagAdded += new EventHandler<TemplateTag>(TemplateTags_TagAdded);
			TemplateTags.TagRemoved += new EventHandler<TemplateTag>(TemplateTags_TagRemoved);
			TemplateTags.TagChanged += new PropertyChangedEventHandler(TemplateTags_TagChanged);
			TemplateCollection templates2 = Templates;
			templates2.CollectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(templates2.CollectionChanged, new NotifyCollectionChangedEventHandler(Templates_CollectionChanged));
			GameService.Graphics.QueueMainThreadRender(delegate
			{
				SortPanels();
			});
			SetHeightToTags();
			TemplateTags.Loaded += new EventHandler(TemplateTags_Loaded);
			if (TemplateTags.IsLoaded)
			{
				CreateTagControls();
			}
			Data.Loaded += new EventHandler(Data_Loaded);
			if (Data.IsLoaded)
			{
				CreateSpecToggles();
			}
		}

		private void Data_Loaded(object sender, EventArgs e)
		{
			CreateSpecToggles();
		}

		private void TemplateTags_Loaded(object sender, EventArgs e)
		{
			CreateTagControls();
		}

		private void ResetAllToggles()
		{
			_specToggles.ForEach(delegate(TagToggle x)
			{
				x.Selected = false;
			});
			_tagControls.SelectMany<KeyValuePair<TagGroupPanel, List<TagToggle>>, TagToggle>((KeyValuePair<TagGroupPanel, List<TagToggle>> x) => x.Value).ForEach(delegate(TagToggle x)
			{
				x.Selected = false;
			});
		}

		private void Templates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedAction? notifyCollectionChangedAction = e?.Action;
			if (notifyCollectionChangedAction.HasValue && notifyCollectionChangedAction.GetValueOrDefault() == NotifyCollectionChangedAction.Add && Settings.ResetFilterOnTemplateCreate.Value)
			{
				ResetAllToggles();
			}
		}

		private void TagGroups_GroupChanged(object sender, PropertyAndValueChangedEventArgs e)
		{
			SortPanels();
		}

		private void TagGroups_GroupRemoved(object sender, TagGroup e)
		{
			SortPanels();
		}

		private void TagGroups_GroupAdded(object sender, TagGroup e)
		{
			SortPanels();
		}

		private void QuickFiltersPanelFadeDelay_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<double> e)
		{
			ApplySettings();
		}

		private void QuickFiltersPanelFade_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<bool> e)
		{
			ApplySettings();
		}

		private void QuickFiltersPanelFadeDuration_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<double> e)
		{
			ApplySettings();
		}

		private void ApplySettings()
		{
			base.FadeOut = Settings.QuickFiltersPanelFade.Value;
			base.FadeDelay = Settings.QuickFiltersPanelFadeDelay.Value * 1000.0;
			base.FadeDuration = Settings.QuickFiltersPanelFadeDuration.Value;
		}

		private void PlayerCharacter_SpecializationChanged(object sender, ValueEventArgs<int> e)
		{
			SetAutoFilters(e.Value);
		}

		private void SetAutoFilters(int e)
		{
			ProfessionType professionType = GameService.Gw2Mumble.PlayerCharacter.Profession;
			if (!Data.Professions.TryGetValue(professionType, out var profession))
			{
				return;
			}
			foreach (TagToggle specToggle2 in _specToggles)
			{
				specToggle2.Selected = false;
			}
			if (Settings.AutoSetFilterProfession?.Value ?? false)
			{
				TagToggle toggle = _specToggles.FirstOrDefault((TagToggle x) => x.Tag?.Name == profession.Name);
				if (toggle != null)
				{
					toggle.Selected = true;
				}
			}
			if (!(Settings.AutoSetFilterSpecialization?.Value ?? false))
			{
				return;
			}
			Specialization spec = profession.Specializations.Values.FirstOrDefault((Specialization x) => x.Id == e);
			if (spec != null)
			{
				TagToggle specToggle = _specToggles.FirstOrDefault((TagToggle x) => x.Tag?.Name == spec.Name);
				if (specToggle != null)
				{
					specToggle.Selected = true;
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
				GetPanel(tag.Group).SortChildren<TagToggle>(SortTagControls);
				break;
			case "Group":
			{
				List<TagGroupPanel> flowPanelsToDelete = new List<TagGroupPanel>();
				foreach (KeyValuePair<TagGroupPanel, List<TagToggle>> t2 in _tagControls)
				{
					TagToggle control = t2.Value.FirstOrDefault((TagToggle x) => x.Tag == tag);
					if (control == null)
					{
						continue;
					}
					TagGroupPanel panel = t2.Key;
					TagGroupPanel p = GetPanel(tag.Group);
					if (panel == p)
					{
						SortPanels();
						return;
					}
					control.Parent = p;
					p.Children.Add(control);
					p.SortChildren<TagToggle>(SortTagControls);
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
				if (flowPanelsToDelete.Count > 0)
				{
					foreach (TagGroupPanel t in flowPanelsToDelete)
					{
						_tagControls.Remove(t);
					}
				}
				SortPanels();
				break;
			}
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int width = base.Width - 50;
			int w = width / 2;
			_ = width / 512;
			int padding = (base.Width - width) / 2;
			_headerSeparator.Bounds = new Rectangle(padding + w, -w + 30, 16, width);
			_textBounds = new Rectangle(0, base.ContentPadding.Top, base.Width, Control.Content.DefaultFont18.get_LineHeight());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawStringOnCtrl(this, "Filter Templates", Control.Content.DefaultFont18, _textBounds, Color.get_White(), wrap: false, HorizontalAlignment.Center);
			spriteBatch.DrawCenteredRotationOnCtrl(this, (Texture2D)_headerSeparator.Texture, _headerSeparator.Bounds, _headerSeparator.TextureRegion, Color.get_White(), 1.56f, flipVertically: false, flipHorizontally: false);
		}

		public TagGroupPanel GetPanel(string groupName)
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			string groupName2 = groupName;
			TagGroupPanel panel = null;
			if (!string.IsNullOrEmpty(groupName2))
			{
				TagGroup tagGroup = TagGroups.FirstOrDefault((TagGroup x) => x.Name == groupName2);
				if (tagGroup != null)
				{
					TagGroupPanel p = _tagControls.Keys.FirstOrDefault((TagGroupPanel x) => x.TagGroup == tagGroup);
					if (p != null)
					{
						panel = p;
					}
					if (panel == null)
					{
						panel = new TagGroupPanel(tagGroup, _tagPanel)
						{
							WidthSizingMode = SizingMode.Fill,
							HeightSizingMode = SizingMode.AutoSize,
							AutoSizePadding = new Point(0, 2)
						};
					}
				}
			}
			if (panel == null)
			{
				TagGroupPanel tagGroupPanel = _ungroupedPanel;
				if (tagGroupPanel == null)
				{
					TagGroupPanel obj = new TagGroupPanel(TagGroup.Empty, _tagPanel)
					{
						Parent = _tagPanel,
						WidthSizingMode = SizingMode.Fill,
						HeightSizingMode = SizingMode.AutoSize,
						AutoSizePadding = new Point(0, 2)
					};
					TagGroupPanel tagGroupPanel2 = obj;
					_ungroupedPanel = obj;
					tagGroupPanel = tagGroupPanel2;
				}
				panel = tagGroupPanel;
			}
			if (!_tagControls.ContainsKey(panel))
			{
				_tagControls.Add(panel, new List<TagToggle>());
				SortPanels();
			}
			return panel;
		}

		private TagToggle AddTemplateTag(TemplateTag e, TagGroupPanel? parent = null, Action<TemplateTag>? action = null)
		{
			TemplateTag e2 = e;
			TagGroupPanel panel = parent ?? GetPanel(e2.Group);
			Action<TemplateTag> a = action ?? ((Action<TemplateTag>)delegate
			{
				if (!SelectionPanel.BuildSelection.FilterQueries.Any<KeyValuePair<string, List<Func<Template, bool>>>>((KeyValuePair<string, List<Func<Template, bool>>> x) => x.Key == e2.Group))
				{
					SelectionPanel.BuildSelection.FilterQueries.Add(new KeyValuePair<string, List<Func<Template, bool>>>(e2.Group, new List<Func<Template, bool>>()));
				}
				KeyValuePair<string, List<Func<Template, bool>>> keyValuePair = SelectionPanel.BuildSelection.FilterQueries.FirstOrDefault<KeyValuePair<string, List<Func<Template, bool>>>>((KeyValuePair<string, List<Func<Template, bool>>> x) => x.Key == e2.Group);
				if (keyValuePair.Value != null)
				{
					if (keyValuePair.Value.Contains(hasTag))
					{
						keyValuePair.Value.Remove(hasTag);
					}
					else
					{
						keyValuePair.Value.Add(hasTag);
					}
				}
				SelectionPanel.BuildSelection.FilterTemplates();
			});
			TagToggle t2 = new TagToggle(e2)
			{
				Parent = panel,
				OnSelectedChanged = a
			};
			if (_tagControls.ContainsKey(panel))
			{
				_tagControls[panel].Add(t2);
			}
			else
			{
				_tagControls.Add(panel, new List<TagToggle>(1) { t2 });
			}
			SortPanels();
			TemplateTagComparer comparer = new TemplateTagComparer(TagGroups);
			panel.SortChildren(delegate(TagToggle x, TagToggle y)
			{
				TemplateTag tag = x.Tag;
				TemplateTag tag2 = y.Tag;
				return comparer.Compare(tag, tag2);
			});
			return t2;
			bool hasTag(Template t)
			{
				return t.Tags.Contains(e2.Name);
			}
		}

		private void SortPanels()
		{
			_tagPanel.SortChildren(delegate(TagGroupPanel x, TagGroupPanel y)
			{
				TagGroupPanel x2 = x;
				TagGroupPanel y2 = y;
				if (x2 == _ungroupedPanel)
				{
					if (y2 != _specPanel)
					{
						return 1;
					}
					return -1;
				}
				if (x2 == _specPanel)
				{
					if (y2 != _ungroupedPanel)
					{
						return -1;
					}
					return 1;
				}
				TagGroup a = TagGroups.FirstOrDefault((TagGroup group) => group == x2.TagGroup);
				TagGroup b = TagGroups.FirstOrDefault((TagGroup group) => group == y2.TagGroup);
				return TemplateTagComparer.CompareGroups(a, b);
			});
			SetHeightToTags();
			GameService.Graphics.QueueMainThreadRender(delegate
			{
				SetHeightToTags();
			});
		}

		private void SetHeightToTags()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			int height = _specializationHeight + 15;
			foreach (KeyValuePair<TagGroupPanel, List<TagToggle>> t in _tagControls)
			{
				if (!(t.Key.TagGroup.Name == strings.Specializations))
				{
					int rows = (int)Math.Ceiling((double)t.Value.Count / 6.0);
					int rowHeight = ((t.Key.Height <= 20) ? (rows * TagToggle.TagHeight + (rows - 1) * TagGroupPanel.ControlPaddingY + TagGroupPanel.OuterControlPaddingY + (int)_tagPanel.ControlPadding.Y) : (t.Key.Height + (int)_tagPanel.ControlPadding.Y));
					height += rowHeight;
				}
			}
			_tagPanel.Height = height;
			base.Height = _tagPanel.Height + 30 + 30 + _tagPanel.ContentPadding.Vertical;
		}

		private int SortTagControls(TagToggle x, TagToggle y)
		{
			return new TemplateTagComparer(TagGroups).Compare(x.Tag, y.Tag);
		}

		private void RemoveTemplateTag(TemplateTag e)
		{
			TemplateTag e2 = e;
			TagToggle tagControl = null;
			KeyValuePair<TagGroupPanel, List<TagToggle>> p = _tagControls.FirstOrDefault<KeyValuePair<TagGroupPanel, List<TagToggle>>>((KeyValuePair<TagGroupPanel, List<TagToggle>> x) => x.Value.Any((TagToggle x) => x.Tag == e2));
			TagGroupPanel panel = p.Key;
			tagControl = p.Value.FirstOrDefault((TagToggle x) => x.Tag == e2);
			tagControl?.Dispose();
			_tagControls[panel].Remove(tagControl);
			if (panel.Children.Any())
			{
				panel.SortChildren<TagToggle>(SortTagControls);
			}
			RemoveEmptyPanels();
		}

		private void RemoveEmptyPanels(TagGroupPanel? fp = null)
		{
			foreach (TagGroupPanel p in _tagControls.Keys.ToList())
			{
				if (p != fp && !p.Children.Any())
				{
					_tagControls.Remove(p);
					p.Dispose();
				}
			}
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

		private void CreateSpecToggles()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			_specPanel = new TagGroupPanel(new TagGroup(strings.Specializations), _tagPanel)
			{
				FlowDirection = ControlFlowDirection.LeftToRight,
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.Standard,
				Height = _specializationHeight,
				AutoSizePadding = new Point(0, 2),
				ControlPadding = new Vector2(21f, 2f)
			};
			_specToggles.Clear();
			foreach (Profession value in Data.Professions.Values)
			{
				Profession p = value;
				int prio = (int)p.Id * 100;
				TagToggle toggle;
				_specToggles.Add(toggle = AddTemplateTag(new TemplateTag
				{
					Name = p.Name,
					Group = strings.Specializations,
					AssetId = p.IconAssetId,
					Priority = prio
				}, _specPanel, delegate
				{
					if (SelectionPanel.BuildSelection.SpecializationFilterQueries.Contains(isProfession))
					{
						SelectionPanel.BuildSelection.SpecializationFilterQueries.Remove(isProfession);
					}
					else
					{
						SelectionPanel.BuildSelection.SpecializationFilterQueries.Add(isProfession);
					}
					SelectionPanel.BuildSelection.FilterTemplates();
				}));
				toggle.SetLocalizedTooltip = () => p.Name;
				foreach (Specialization value2 in p.Specializations.Values)
				{
					Specialization s = value2;
					if (!s.Elite)
					{
						continue;
					}
					_specToggles.Add(toggle = AddTemplateTag(new TemplateTag
					{
						Name = s.Name,
						Group = strings.Specializations,
						AssetId = s.ProfessionIconAssetId.GetValueOrDefault(),
						Priority = prio + s.Id
					}, _specPanel, delegate
					{
						if (SelectionPanel.BuildSelection.SpecializationFilterQueries.Contains(isSpecialization))
						{
							SelectionPanel.BuildSelection.SpecializationFilterQueries.Remove(isSpecialization);
						}
						else
						{
							SelectionPanel.BuildSelection.SpecializationFilterQueries.Add(isSpecialization);
						}
						SelectionPanel.BuildSelection.FilterTemplates();
					}));
					toggle.SetLocalizedTooltip = () => s.Name;
					bool isSpecialization(Template t)
					{
						return t.EliteSpecializationId == s.Id;
					}
				}
				bool isProfession(Template t)
				{
					return t.Profession == p.Id;
				}
			}
			SortPanels();
		}

		public override void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			base.UserLocale_SettingChanged(sender, e);
		}
	}
}
