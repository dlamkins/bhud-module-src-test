using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2BuildTemplates;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selection
{
	public class BuildSelection : BaseSelection
	{
		private readonly ImageButton _addBuildsButton;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _sortBehavior;

		private double _lastShown;

		private Template? _pendingFocusedTemplate;

		private bool _pendingRename;

		private int _pendingFocusDelayFrames;

		private int _pendingFocusFramesRemaining;

		private static readonly BindingFlags s_instanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		public List<TemplateSelectable> TemplateSelectables { get; } = new List<TemplateSelectable>();


		public SelectionPanel SelectionPanel { get; set; }

		public TemplateCollection Templates { get; }

		public TemplateTags TemplateTags { get; }

		public Data Data { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public TemplateFactory TemplateFactory { get; }

		public Settings Settings { get; }

		public List<KeyValuePair<string, List<Func<Template, bool>>>> FilterQueries { get; } = new List<KeyValuePair<string, List<Func<Template, bool>>>>();


		public List<Func<Template, bool>> SpecializationFilterQueries { get; } = new List<Func<Template, bool>>();


		public BuildSelection(TemplateCollection templates, TemplateTags templateTags, Data data, TemplatePresenter templatePresenter, TemplateFactory templateFactory, Settings settings)
		{
			Data = data;
			Templates = templates;
			TemplateTags = templateTags;
			TemplateFactory = templateFactory;
			Settings = settings;
			TemplatePresenter = templatePresenter;
			_sortBehavior = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this,
				Location = new Point(0, 0),
				ValueChangedAction = delegate(string s)
				{
					if (_sortBehavior != null)
					{
						Settings.SortBehavior.Value = GetSortBehaviorFromString(s);
						FilterTemplates();
					}
				},
				SetLocalizedItems = delegate
				{
					if (_sortBehavior != null)
					{
						_sortBehavior.SelectedItem = GetSortBehaviorString(Settings.SortBehavior.Value);
					}
					return new List<string>(3)
					{
						GetSortBehaviorString(TemplateSortBehavior.ByProfession),
						GetSortBehaviorString(TemplateSortBehavior.ByName),
						GetSortBehaviorString(TemplateSortBehavior.ByModified)
					};
				},
				SelectedItem = GetSortBehaviorString(Settings.SortBehavior.Value)
			};
			Search.Location = new Point(2, _sortBehavior.Bottom + 5);
			SelectionContent.Location = new Point(0, Search.Bottom + 5);
			Search.PerformFiltering = delegate
			{
				FilterTemplates();
			};
			new Point(0, 0);
			_ = GameService.Gw2Mumble.PlayerCharacter;
			_addBuildsButton = new ImageButton
			{
				Parent = this,
				Location = new Point(0, 30),
				Texture = AsyncTexture2D.FromAssetId(155902),
				DisabledTexture = AsyncTexture2D.FromAssetId(155903),
				HoveredTexture = AsyncTexture2D.FromAssetId(155904),
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				SetLocalizedTooltip = () => strings.AddNewTemplateWithClipboard,
				ClickAction = delegate
				{
					AddNewTemplate();
				}
			};
			Search.TextChangedAction = delegate(string txt)
			{
				_addBuildsButton.BasicTooltipText = (string.IsNullOrEmpty(txt) ? strings.CreateNewTemplate : string.Format(strings.CreateNewTemplateName, txt));
			};
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
			TemplateCollection templates2 = Templates;
			templates2.CollectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(templates2.CollectionChanged, new NotifyCollectionChangedEventHandler(Templates_CollectionChanged));
			Templates.TemplateChanged += new PropertyChangedEventHandler(Templates_TemplateChanged);
			Templates.Loaded += new EventHandler(Templates_Loaded);
			if (Templates.IsLoaded)
			{
				AddTemplateSelectable(firstLoad: true, Templates.ToList());
			}
		}

		private void Templates_Loaded(object sender, EventArgs e)
		{
			TemplateSelectables?.DisposeAll();
			TemplateSelectables?.Clear();
			AddTemplateSelectable(firstLoad: true, Templates.ToList());
		}

		private void AddNewTemplate()
		{
			Task.Run(async delegate
			{
				string code = null;
				try
				{
					code = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
				}
				catch (Exception ex)
				{
					BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Warn(ex, "Failed to read clipboard while creating a template.");
				}
				string trimmedCode = (string.IsNullOrWhiteSpace(code) ? null : code.Trim());
				bool hasClipboardCode = !string.IsNullOrEmpty(trimmedCode);
				BuildTemplate build;
				bool hasValidBuildCode = hasClipboardCode && Gw2BuildCodec.TryDecode(trimmedCode, out build);
				GameService.Graphics.QueueMainThreadRender(delegate
				{
					string name = (string.IsNullOrEmpty(Search.Text) ? strings.NewTemplate : Search.Text);
					Template template = CreateTemplate(name);
					if (hasValidBuildCode)
					{
						try
						{
							BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Debug("Load template from clipboard code: " + trimmedCode);
							template.LoadFromCode(trimmedCode);
						}
						catch (Exception exception)
						{
							BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Warn(exception, "Failed to load clipboard build code '" + trimmedCode + "'.");
							ScreenNotification.ShowNotification("Clipboard build code could not be loaded. Created blank template instead.");
						}
					}
					if (Settings.SetFilterOnTemplateCreate.Value)
					{
						Search.Text = template.Name;
						Search.ForceFilter();
					}
					else if (Settings.ResetFilterOnTemplateCreate.Value)
					{
						Search.Text = null;
						Search.ForceFilter();
					}
					QueueFocusTemplate(template, rename: true);
				});
			});
		}

		private void Templates_TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			RefreshTemplateSelection(sender as Template);
		}

		private TemplateSortBehavior GetSortBehaviorFromString(string s)
		{
			if (!(s == strings.SortyByProfession))
			{
				if (!(s == strings.SortByName))
				{
					if (!(s == strings.SortByModified))
					{
						return TemplateSortBehavior.ByProfession;
					}
					return TemplateSortBehavior.ByModified;
				}
				return TemplateSortBehavior.ByName;
			}
			return TemplateSortBehavior.ByProfession;
		}

		private string GetSortBehaviorString(TemplateSortBehavior templateSortBehavior)
		{
			return templateSortBehavior switch
			{
				TemplateSortBehavior.ByProfession => strings.SortyByProfession, 
				TemplateSortBehavior.ByName => strings.SortByName, 
				TemplateSortBehavior.ByModified => strings.SortByModified, 
				_ => string.Empty, 
			};
		}

		private void SortBehavior_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			FilterTemplates();
		}

		private void LocalizingService_LocaleChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			_sortBehavior.Items[0] = strings.SortyByProfession;
			_sortBehavior.Items[1] = strings.SortByName;
		}

		public void FilterTemplates()
		{
			try
			{
				string lowerTxt = Search.Text?.Trim().ToLower();
				bool anyName = string.IsNullOrEmpty(lowerTxt);
				foreach (TemplateSelectable template in TemplateSelectables)
				{
					bool filterQueriesMatches = FilterQueries.Count == 0 || FilterQueries.All<KeyValuePair<string, List<Func<Template, bool>>>>((KeyValuePair<string, List<Func<Template, bool>>> x) => x.Value.Count == 0 || x.Value.Any((Func<Template, bool> x) => x(template.Template)));
					bool specMatches = SpecializationFilterQueries.Count == 0 || SpecializationFilterQueries.Any((Func<Template, bool> x) => x(template.Template));
					bool nameMatches = anyName || template.Template.Name.ToLower().Contains(lowerTxt);
					bool lastModifiedMatch = template.Template.LastModified.ToLower().Contains(lowerTxt);
					template.Visible = filterQueriesMatches && specMatches && (nameMatches || lastModifiedMatch);
				}
				SortTemplates();
				SelectionContent.Invalidate();
				TemplateSelectable current = TemplateSelectables.FirstOrDefault((TemplateSelectable x) => x.Template == TemplatePresenter.Template);
				if ((((!(current?.Visible)) ?? true) && Settings.RequireVisibleTemplate.Value) || current?.Template == Template.Empty)
				{
					TemplateSelectable t = SelectionContent.OfType<TemplateSelectable>().FirstOrDefault((TemplateSelectable x) => x.Visible);
					TemplatePresenter.SetTemplate(((t != null) ? t : null)?.Template);
				}
			}
			catch (Exception ex)
			{
				BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths, Kenedia.Modules.BuildsManager.Services.StaticHosting>.Logger.Debug(ex, "Error while filtering templates");
			}
		}

		private void SortTemplates()
		{
			switch (Settings.SortBehavior.Value)
			{
			case TemplateSortBehavior.ByProfession:
				SelectionContent.SortChildren(delegate(TemplateSelectable a, TemplateSelectable b)
				{
					int num = a.Template.Profession.CompareTo(b.Template.Profession);
					int num2 = a.Template.EliteSpecializationId.CompareTo(b.Template.EliteSpecializationId);
					int num3 = a.Template.Name.CompareTo(b.Template.Name);
					if (num != 0)
					{
						return num;
					}
					return (num2 == 0) ? num3 : num2;
				});
				break;
			case TemplateSortBehavior.ByName:
				SelectionContent.SortChildren((TemplateSelectable a, TemplateSelectable b) => a.Template.Name.CompareTo(b.Template.Name));
				break;
			case TemplateSortBehavior.ByModified:
				SelectionContent.SortChildren(delegate(TemplateSelectable a, TemplateSelectable b)
				{
					int num4 = a.Template.LastModified.CompareTo(b.Template.LastModified);
					int num5 = a.Template.Name.CompareTo(b.Template.Name);
					return (num4 != 0) ? num4 : num5;
				});
				break;
			}
		}

		private void Templates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			TemplateSelectables.Select((TemplateSelectable e) => e.Template);
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				List<Template> removedTemplates = e.OldItems?.OfType<Template>()?.ToList();
				RemoveTemplateSelectable(removedTemplates);
				if (Templates.Count > 0)
				{
					FilterTemplates();
				}
				else
				{
					CreateTemplate(strings.NewTemplate);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				bool firstLoad = TemplateSelectables.Count == 0 && (Templates?.Count ?? 0) != 0;
				List<Template> addedTemplates = e.NewItems?.OfType<Template>()?.ToList();
				AddTemplateSelectable(firstLoad, addedTemplates);
				FilterTemplates();
			}
		}

		private void RemoveTemplateSelectable(List<Template> removedTemplates)
		{
			if (removedTemplates == null || !removedTemplates.Any())
			{
				return;
			}
			for (int i = TemplateSelectables.Count - 1; i >= 0; i--)
			{
				TemplateSelectable template = TemplateSelectables[i];
				if (removedTemplates.Contains(template.Template))
				{
					TemplateSelectables.Remove(template);
					template.Dispose();
				}
			}
		}

		private void AddTemplateSelectable(bool firstLoad, List<Template> addedTemplates)
		{
			if (addedTemplates == null || !addedTemplates.Any())
			{
				return;
			}
			foreach (Template template in addedTemplates)
			{
				TemplateSelectable t = new TemplateSelectable(TemplatePresenter, Templates, Data, TemplateTags, TemplateFactory)
				{
					Parent = SelectionContent,
					Template = template,
					Width = SelectionContent.Width - 35
				};
				t.OnNameChangedAction = delegate
				{
					RefreshTemplateSelection(t.Template);
				};
				template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(ProfessionChanged);
				t.OnClickAction = delegate
				{
					SelectionPanel?.SetTemplateAnchor(t);
				};
				TemplateSelectables.Add(t);
				if (!firstLoad)
				{
					QueueFocusTemplate(t.Template, rename: true);
				}
			}
			if (firstLoad)
			{
				FilterTemplates();
				TemplateSelectable tt = GetFirstTemplateSelectable();
				TemplatePresenter.SetTemplate(tt?.Template);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		public Template CreateTemplate(string name)
		{
			string uniqueName = Templates.GetNewName(name);
			Template template = TemplateFactory.CreateTemplate(uniqueName);
			Templates.Add(template);
			return template;
		}

		private void QueueFocusTemplate(Template template, bool rename)
		{
			if (template != null)
			{
				_pendingFocusedTemplate = template;
				_pendingRename = rename;
				_pendingFocusDelayFrames = 2;
				_pendingFocusFramesRemaining = 30;
			}
		}

		private void RefreshTemplateSelection(Template? template)
		{
			FilterTemplates();
			if (template != null && template == TemplatePresenter.Template)
			{
				QueueFocusTemplate(template, rename: false);
			}
		}

		private void FocusTemplate(Template template, bool rename)
		{
			Template template2 = template;
			if (template2 == null)
			{
				return;
			}
			TemplateSelectable selectable = TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == template2);
			if (selectable != null)
			{
				TemplatePresenter.SetTemplate(template2);
				SelectionPanel?.SetTemplateAnchor(selectable);
				BringTemplateIntoView(selectable);
				if (rename)
				{
					selectable.ToggleEditMode(enable: true);
				}
			}
		}

		private Kenedia.Modules.Core.Controls.Scrollbar? GetSelectionScrollbar()
		{
			object obj = base.Parent?.Children?.OfType<Kenedia.Modules.Core.Controls.Scrollbar>().FirstOrDefault((Kenedia.Modules.Core.Controls.Scrollbar s) => s.AssociatedContainer == SelectionContent);
			if (obj == null)
			{
				Blish_HUD.Controls.Container parent = SelectionContent.Parent;
				if (parent == null)
				{
					return null;
				}
				ControlCollection<Control> children = parent.Children;
				if (children == null)
				{
					return null;
				}
				obj = children.OfType<Kenedia.Modules.Core.Controls.Scrollbar>().FirstOrDefault((Kenedia.Modules.Core.Controls.Scrollbar s) => s.AssociatedContainer == SelectionContent);
			}
			return (Kenedia.Modules.Core.Controls.Scrollbar?)obj;
		}

		private object? GetNativeSelectionScrollbar()
		{
			Type type = SelectionContent.GetType();
			while ((object)type != null)
			{
				object scrollbar = type.GetField("_panelScrollbar", s_instanceFlags)?.GetValue(SelectionContent);
				if (scrollbar != null)
				{
					return scrollbar;
				}
				type = type.BaseType;
			}
			return null;
		}

		private void SetSelectionScrollState(int targetOffset, int maxOffset)
		{
			targetOffset = Math.Max(0, Math.Min(targetOffset, maxOffset));
			SelectionContent.VerticalScrollOffset = targetOffset;
			float scrollDistance = ((maxOffset == 0) ? 0f : Math.Max(0f, Math.Min((float)targetOffset / (float)maxOffset, 1f)));
			Kenedia.Modules.Core.Controls.Scrollbar customScrollbar = GetSelectionScrollbar();
			if (customScrollbar != null)
			{
				customScrollbar.ScrollDistance = scrollDistance;
			}
			object nativeScrollbar = GetNativeSelectionScrollbar();
			if (nativeScrollbar != null)
			{
				Type type = nativeScrollbar.GetType();
				type.GetProperty("ScrollDistance", s_instanceFlags)?.SetValue(nativeScrollbar, scrollDistance);
				type.GetProperty("TargetScrollDistance", s_instanceFlags)?.SetValue(nativeScrollbar, scrollDistance);
			}
		}

		private bool TryGetTemplateBounds(TemplateSelectable selectable, out int top, out int bottom, out int contentHeight)
		{
			top = 0;
			bottom = 0;
			contentHeight = SelectionContent.ContentRegion.Height;
			if (selectable == null || !selectable.Visible)
			{
				return false;
			}
			int y = SelectionContent.ContentPadding.Top;
			int spacing = (int)SelectionContent.ControlPadding.Y;
			foreach (TemplateSelectable child in SelectionContent.Children.OfType<TemplateSelectable>())
			{
				if (child.Visible)
				{
					if (child == selectable)
					{
						top = y;
						bottom = y + child.Height;
					}
					y += child.Height + spacing;
				}
			}
			contentHeight = Math.Max(y + SelectionContent.ContentPadding.Bottom - spacing, SelectionContent.ContentRegion.Height);
			return bottom > top;
		}

		private void BringTemplateIntoView(TemplateSelectable selectable)
		{
			if (!TryGetTemplateBounds(selectable, out var childTop, out var childBottom, out var contentHeight))
			{
				return;
			}
			int maxOffset = Math.Max(contentHeight - SelectionContent.ContentRegion.Height, 0);
			if (maxOffset == 0)
			{
				SetSelectionScrollState(0, 0);
				return;
			}
			int margin = 10;
			int viewportTop;
			int num = (viewportTop = SelectionContent.VerticalScrollOffset);
			int viewportBottom = num + SelectionContent.ContentRegion.Height;
			int targetOffset = num;
			if (childTop < viewportTop + margin)
			{
				targetOffset = Math.Max(childTop - margin, 0);
			}
			else if (childBottom > viewportBottom - margin)
			{
				targetOffset = Math.Max(childBottom - SelectionContent.ContentRegion.Height + margin, 0);
			}
			targetOffset = Math.Max(0, Math.Min(targetOffset, maxOffset));
			SetSelectionScrollState(targetOffset, maxOffset);
		}

		private bool IsTemplateInView(TemplateSelectable selectable)
		{
			if (!TryGetTemplateBounds(selectable, out var childTop, out var childBottom, out var _))
			{
				return false;
			}
			int margin = 10;
			int viewportTop = SelectionContent.VerticalScrollOffset;
			int viewportBottom = viewportTop + SelectionContent.ContentRegion.Height;
			if (childTop >= viewportTop + margin)
			{
				return childBottom <= viewportBottom - margin;
			}
			return false;
		}

		private void SpecializationChanged(object sender, DictionaryItemChangedEventArgs<SpecializationSlotType, Specialization> e)
		{
			RefreshTemplateSelection(sender as Template);
		}

		private void ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			RefreshTemplateSelection(sender as Template);
		}

		public TemplateSelectable? GetFirstTemplateSelectable()
		{
			FilterTemplates();
			return SelectionContent.GetChildrenOfType<TemplateSelectable>().FirstOrDefault((TemplateSelectable e) => e.Visible);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			Search?.SetSize(base.Width - Search.Left - Search.Height - 2);
			_addBuildsButton?.SetLocation(Search.Right, Search.Top);
			_addBuildsButton?.SetSize(Search.Height, Search.Height);
			_sortBehavior?.SetLocation(Search.Left);
			_sortBehavior?.SetSize((_addBuildsButton?.Right ?? 0) - Search.Left);
		}

		protected override void OnSelectionContent_Resized(object sender, ResizedEventArgs e)
		{
			base.OnSelectionContent_Resized(sender, e);
			foreach (TemplateSelectable templateSelectable in TemplateSelectables)
			{
				templateSelectable.Width = SelectionContent.Width - 35;
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (Common.Now - _lastShown >= 250.0)
			{
				base.OnClick(e);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			_lastShown = Common.Now;
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_sortBehavior.Enabled = false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!_sortBehavior.Enabled)
			{
				_sortBehavior.Enabled = _sortBehavior.Enabled || Common.Now - _lastShown >= 5.0;
			}
			if (_pendingFocusedTemplate == null)
			{
				return;
			}
			Template pendingTemplate = _pendingFocusedTemplate;
			bool pendingRename = _pendingRename;
			if (_pendingFocusDelayFrames > 0)
			{
				_pendingFocusDelayFrames--;
				return;
			}
			TemplateSelectable selectable = TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == pendingTemplate);
			if (selectable != null && selectable.Visible && selectable.Parent == SelectionContent && selectable.Height > 0)
			{
				FocusTemplate(pendingTemplate, pendingRename);
				_pendingRename = false;
				_pendingFocusFramesRemaining--;
				if (IsTemplateInView(selectable) || _pendingFocusFramesRemaining <= 0)
				{
					_pendingFocusedTemplate = null;
					_pendingRename = false;
				}
			}
			else if (_pendingFocusFramesRemaining <= 0)
			{
				_pendingFocusedTemplate = null;
				_pendingRename = false;
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_sortBehavior?.Dispose();
			TemplateCollection templates = Templates;
			templates.CollectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(templates.CollectionChanged, new NotifyCollectionChangedEventHandler(Templates_CollectionChanged));
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
			TemplateSelectables.Clear();
		}
	}
}
