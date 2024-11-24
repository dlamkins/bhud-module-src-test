using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
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
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
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
				TemplateCollection templates3 = Templates;
				List<Template> list = new List<Template>(templates3.Count);
				list.AddRange(templates3);
				AddTemplateSelectable(firstLoad: true, list);
			}
		}

		private void Templates_Loaded(object sender, EventArgs e)
		{
			TemplateCollection templates = Templates;
			List<Template> list = new List<Template>(templates.Count);
			list.AddRange(templates);
			AddTemplateSelectable(firstLoad: true, list);
		}

		private void AddNewTemplate()
		{
			Task.Run(async delegate
			{
				string code = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
				code = code.Trim();
				GameService.Graphics.QueueMainThreadRender(delegate
				{
					string name = (string.IsNullOrEmpty(Search.Text) ? strings.NewTemplate : Search.Text);
					Template t = CreateTemplate(Templates.GetNewName(name));
					if (!string.IsNullOrEmpty(code))
					{
						try
						{
							BaseModule<BuildsManager, MainWindow, Kenedia.Modules.BuildsManager.Services.Settings, Paths>.Logger.Debug("Load template from clipboard code: " + code);
							t.LoadFromCode(code);
						}
						catch (Exception)
						{
						}
					}
					TemplateSelectable templateSelectable = null;
					SelectionPanel?.SetTemplateAnchor(templateSelectable = TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == t));
					templateSelectable?.ToggleEditMode(enable: true);
					if (Settings.SetFilterOnTemplateCreate.Value)
					{
						Search.Text = t.Name;
					}
					else if (Settings.ResetFilterOnTemplateCreate.Value)
					{
						Search.Text = null;
					}
				});
			});
		}

		private void Templates_TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			FilterTemplates();
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
			TemplateSelectable current = TemplateSelectables.FirstOrDefault((TemplateSelectable x) => x.Template == TemplatePresenter.Template);
			if ((((!(current?.Visible)) ?? true) && Settings.RequireVisibleTemplate.Value) || current?.Template == Template.Empty)
			{
				TemplateSelectable t = SelectionContent.OfType<TemplateSelectable>().FirstOrDefault((TemplateSelectable x) => x.Visible);
				TemplatePresenter.SetTemplate(((t != null) ? t : null)?.Template);
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
					int num2 = a.Template.Name.CompareTo(b.Template.Name);
					return (num != 0) ? num : num2;
				});
				break;
			case TemplateSortBehavior.ByName:
				SelectionContent.SortChildren((TemplateSelectable a, TemplateSelectable b) => a.Template.Name.CompareTo(b.Template.Name));
				break;
			case TemplateSortBehavior.ByModified:
				SelectionContent.SortChildren(delegate(TemplateSelectable a, TemplateSelectable b)
				{
					int num3 = a.Template.LastModified.CompareTo(b.Template.LastModified);
					int num4 = a.Template.Name.CompareTo(b.Template.Name);
					return (num3 != 0) ? num3 : num4;
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
					Width = SelectionContent.Width - 35,
					OnNameChangedAction = new Action(FilterTemplates)
				};
				template.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(ProfessionChanged);
				t.OnClickAction = delegate
				{
					SelectionPanel?.SetTemplateAnchor(t);
				};
				if (!firstLoad)
				{
					SelectionPanel?.SetTemplateAnchor(t);
					TemplatePresenter.SetTemplate(t.Template);
					t.ToggleEditMode(enable: true);
				}
				TemplateSelectables.Add(t);
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
		}

		public Template CreateTemplate(string name)
		{
			for (int i = 0; i < int.MaxValue; i++)
			{
				string newName = ((i == 0) ? name : $"{name} #{i}");
				if (Templates.Where((Template e) => e.Name == newName)?.FirstOrDefault() != null)
				{
					continue;
				}
				TemplateSelectable ts = null;
				Template t;
				Templates.Add(t = TemplateFactory.CreateTemplate(name));
				SelectionPanel?.SetTemplateAnchor(ts = TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == t));
				ts?.ToggleEditMode(enable: false);
				TemplatePresenter.SetTemplate(t);
				t.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(ProfessionChanged);
				if (ts != null)
				{
					ts.DisposeAction = delegate
					{
						t.ProfessionChanged -= new ValueChangedEventHandler<ProfessionType>(ProfessionChanged);
					};
				}
				return t;
			}
			return null;
		}

		private void SpecializationChanged(object sender, DictionaryItemChangedEventArgs<SpecializationSlotType, Specialization> e)
		{
			FilterTemplates();
		}

		private void ProfessionChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<ProfessionType> e)
		{
			FilterTemplates();
		}

		public TemplateSelectable? GetFirstTemplateSelectable()
		{
			FilterTemplates();
			return SelectionContent.GetChildrenOfType<TemplateSelectable>().FirstOrDefault((TemplateSelectable e) => e.Visible);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			Search?.SetSize(base.Width - Search.Left - Search.Height - 2, null);
			_addBuildsButton?.SetLocation(Search.Right, Search.Top);
			_addBuildsButton?.SetSize(Search.Height, Search.Height);
			_sortBehavior?.SetLocation(Search.Left, null);
			_sortBehavior?.SetSize((_addBuildsButton?.Right ?? 0) - Search.Left, null);
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
