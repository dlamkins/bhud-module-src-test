using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

		private readonly Kenedia.Modules.Core.Controls.LoadingSpinner _spinner;

		private readonly Kenedia.Modules.Core.Controls.Dropdown _sortBehavior;

		private double _lastShown;

		public List<TemplateSelectable> TemplateSelectables { get; } = new List<TemplateSelectable>();


		public SelectionPanel SelectionPanel { get; set; }

		public TemplateCollection Templates { get; }

		public TemplateTags TemplateTags { get; }

		public Data Data { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public TemplateFactory TemplateFactory { get; }

		public List<KeyValuePair<string, List<Func<Template, bool>>>> FilterQueries { get; } = new List<KeyValuePair<string, List<Func<Template, bool>>>>();


		public List<Func<Template, bool>> SpecializationFilterQueries { get; } = new List<Func<Template, bool>>();


		public BuildSelection(TemplateCollection templates, TemplateTags templateTags, Data data, TemplatePresenter templatePresenter, TemplateFactory templateFactory)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			Data = data;
			Templates = templates;
			TemplateTags = templateTags;
			TemplateFactory = templateFactory;
			TemplatePresenter = templatePresenter;
			Kenedia.Modules.Core.Controls.LoadingSpinner obj = new Kenedia.Modules.Core.Controls.LoadingSpinner
			{
				Parent = this
			};
			Rectangle localBounds = SelectionContent.LocalBounds;
			obj.Location = ((Rectangle)(ref localBounds)).get_Center();
			obj.Size = new Point(64);
			_spinner = obj;
			_sortBehavior = new Kenedia.Modules.Core.Controls.Dropdown
			{
				Parent = this,
				Location = new Point(0, 0),
				ValueChangedAction = delegate(string s)
				{
					BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Settings.SortBehavior.Value = GetSortBehaviorFromString(s);
					FilterTemplates();
				},
				SetLocalizedItems = delegate
				{
					if (_sortBehavior != null)
					{
						_sortBehavior.SelectedItem = GetSortBehaviorString(BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Settings.SortBehavior.Value);
					}
					return new List<string>(2)
					{
						GetSortBehaviorString(TemplateSortBehavior.ByProfession),
						GetSortBehaviorString(TemplateSortBehavior.ByName)
					};
				},
				SelectedItem = GetSortBehaviorString(BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Settings.SortBehavior.Value)
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
					Task.Run(async delegate
					{
						string name = (string.IsNullOrEmpty(Search.Text) ? strings.NewTemplate : Search.Text);
						Template t = CreateTemplate(name);
						if (t != null)
						{
							Search.Text = null;
							try
							{
								string code = await ClipboardUtil.WindowsClipboardService.GetTextAsync();
								BaseModule<BuildsManager, MainWindow, Settings, Paths>.Logger.Debug("Load template from clipboard code: " + code);
								t.LoadFromCode(code);
								TemplatePresenter.SetTemplate(t);
							}
							catch (Exception)
							{
							}
							TemplateSelectable ts = null;
							SelectionPanel?.SetTemplateAnchor(ts = TemplateSelectables.FirstOrDefault((TemplateSelectable e) => e.Template == t));
							ts?.ToggleEditMode(enable: true);
							FilterTemplates();
						}
					});
				}
			};
			Search.TextChangedAction = delegate(string txt)
			{
				_addBuildsButton.BasicTooltipText = (string.IsNullOrEmpty(txt) ? strings.CreateNewTemplate : string.Format(strings.CreateNewTemplateName, txt));
			};
			BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.TemplatesLoadedDone += new ValueChangedEventHandler<bool>(ModuleInstance_TemplatesLoadedDone);
			templates.CollectionChanged += Templates_CollectionChanged;
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
			Templates_CollectionChanged(this, null);
		}

		private TemplateSortBehavior GetSortBehaviorFromString(string s)
		{
			if (!(s == strings.SortyByProfession))
			{
				if (!(s == strings.SortByName))
				{
					return TemplateSortBehavior.ByProfession;
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

		private void ModuleInstance_TemplatesLoadedDone(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			Templates_CollectionChanged(sender, null);
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
				template.Visible = filterQueriesMatches && specMatches && nameMatches;
			}
			if (BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Settings.SortBehavior.Value == TemplateSortBehavior.ByProfession)
			{
				SelectionContent.SortChildren(delegate(TemplateSelectable a, TemplateSelectable b)
				{
					int num = a.Template.Profession.CompareTo(b.Template.Profession);
					int num2 = a.Template.Name.CompareTo(b.Template.Name);
					return (num != 0) ? num : num2;
				});
			}
			if (BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.Settings.SortBehavior.Value == TemplateSortBehavior.ByName)
			{
				SelectionContent.SortChildren((TemplateSelectable a, TemplateSelectable b) => a.Template.Name.CompareTo(b.Template.Name));
			}
			if ((!(TemplateSelectables.FirstOrDefault((TemplateSelectable x) => x.Template == TemplatePresenter.Template)?.Visible)) ?? true)
			{
				TemplateSelectable t = SelectionContent.OfType<TemplateSelectable>().FirstOrDefault((TemplateSelectable x) => x.Visible);
				if (t != null)
				{
					TemplatePresenter.SetTemplate(t.Template);
				}
			}
		}

		private void Templates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			if (!BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.TemplatesLoaded)
			{
				if (!_spinner.Visible)
				{
					_spinner.Show();
					Kenedia.Modules.Core.Controls.LoadingSpinner spinner = _spinner;
					Rectangle localBounds = SelectionPanel.LocalBounds;
					spinner.Location = ((Rectangle)(ref localBounds)).get_Center().Add(_spinner.Size.Scale(-0.5));
					TemplateSelectables.DisposeAll();
					TemplateSelectables.Clear();
				}
				return;
			}
			_spinner.Hide();
			bool firstLoad = TemplateSelectables.Count == 0 && (Templates?.Count ?? 0) != 0;
			IEnumerable<Template> templates = TemplateSelectables.Select((TemplateSelectable e) => e.Template);
			IEnumerable<Template> removedTemplates = templates.Except(Templates ?? new TemplateCollection());
			IEnumerable<Template> addedTemplates = Templates?.Except(templates);
			if (addedTemplates == null)
			{
				return;
			}
			foreach (Template template2 in addedTemplates)
			{
				TemplateSelectable t = new TemplateSelectable(TemplatePresenter, Templates, Data, TemplateTags, TemplateFactory)
				{
					Parent = SelectionContent,
					Template = template2,
					Width = SelectionContent.Width - 35,
					OnNameChangedAction = new Action(FilterTemplates)
				};
				template2.ProfessionChanged += new ValueChangedEventHandler<ProfessionType>(ProfessionChanged);
				t.OnClickAction = delegate
				{
					SelectionPanel?.SetTemplateAnchor(t);
				};
				if (!firstLoad)
				{
					SelectionPanel?.SetTemplateAnchor(t);
					t.ToggleEditMode(enable: true);
					_ = t;
				}
				TemplateSelectables.Add(t);
			}
			for (int i = TemplateSelectables.Count - 1; i >= 0; i--)
			{
				TemplateSelectable template = TemplateSelectables[i];
				if (removedTemplates.Contains<Template>(template.Template))
				{
					TemplateSelectables.Remove(template);
					template.Dispose();
				}
			}
			FilterTemplates();
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

		public TemplateSelectable GetFirstTemplateSelectable()
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
			BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.TemplatesLoadedDone -= new ValueChangedEventHandler<bool>(ModuleInstance_TemplatesLoadedDone);
			Templates.CollectionChanged -= Templates_CollectionChanged;
			Templates_CollectionChanged(this, null);
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(LocalizingService_LocaleChanged);
			TemplateSelectables.Clear();
		}
	}
}
