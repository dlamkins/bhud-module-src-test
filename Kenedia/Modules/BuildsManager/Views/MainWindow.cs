using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.Controls.Tabs;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Res;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class MainWindow : Kenedia.Modules.Core.Views.TabbedWindow
	{
		private DetailedTexture _quickFilterToggle = new DetailedTexture(440021)
		{
			HoverDrawColor = Color.get_White(),
			DrawColor = Color.get_White() * 0.5f
		};

		private readonly TabbedRegion _tabbedRegion;

		public Tab TemplateViewTab { get; }

		public TemplateView TemplateView { get; private set; }

		public Tab TagEditViewTab { get; }

		public TagEditView TagEditView { get; private set; }

		public SelectionPanel SelectionPanel { get; }

		public AboutTab AboutTab { get; }

		public BuildTab BuildTab { get; }

		public GearTab GearTab { get; }

		public QuickFiltersPanel QuickFiltersPanel { get; }

		public Settings Settings { get; }

		public TemplatePresenter TemplatePresenter { get; }

		public Tab SettingsViewTab { get; private set; }

		public SettingsView SettingsView { get; private set; }

		public MainWindow(Module module, TemplatePresenter templatePresenter, TemplateTags templateTags, TagGroups tagGroups, SelectionPanel selectionPanel, AboutTab aboutTab, BuildTab buildTab, GearTab gearTab, QuickFiltersPanel quickFiltersPanel, Settings settings)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			SelectionPanel selectionPanel2 = selectionPanel;
			AboutTab aboutTab2 = aboutTab;
			BuildTab buildTab2 = buildTab;
			GearTab gearTab2 = gearTab;
			QuickFiltersPanel quickFiltersPanel2 = quickFiltersPanel;
			TemplateTags templateTags2 = templateTags;
			TagGroups tagGroups2 = tagGroups;
			Settings settings2 = settings;
			base._002Ector((AsyncTexture2D)TexturesService.GetTextureFromRef("textures\\mainwindow_background.png", "mainwindow_background"), new Rectangle(30, 30, 915, 665), new Rectangle(40, 20, 895, 665));
			MainWindow mainWindow = this;
			TemplatePresenter = templatePresenter;
			base.Parent = Control.Graphics.SpriteScreen;
			AboutTab = aboutTab2;
			BuildTab = buildTab2;
			GearTab = gearTab2;
			QuickFiltersPanel = quickFiltersPanel2;
			Settings = settings2;
			SelectionPanel = selectionPanel2;
			AboutTab.MainWindow = (SelectionPanel.MainWindow = this);
			base.Title = "❤";
			base.Subtitle = "❤";
			base.SavesPosition = true;
			base.Id = module.Name + " MainWindow";
			base.MainWindowEmblem = AsyncTexture2D.FromAssetId(156020);
			base.Name = module.Name;
			base.Version = module.Version;
			base.Width = 1250;
			base.Height = 900;
			TemplatePresenter.TemplateChanged += new ValueChangedEventHandler<Template>(TemplatePresenter_TemplateChanged);
			TemplatePresenter.NameChanged += new ValueChangedEventHandler<string>(TemplatePresenter_NameChanged);
			base.Tabs.Add(TemplateViewTab = new Tab(AsyncTexture2D.FromAssetId(156720), () => mainWindow.TemplateView = new TemplateView(mainWindow, selectionPanel2, aboutTab2, buildTab2, gearTab2, quickFiltersPanel2), strings.Templates));
			base.Tabs.Add(TagEditViewTab = new Tab(TexturesService.GetTextureFromRef(textures_common.Tag, "Tag"), () => mainWindow.TagEditView = new TagEditView(templateTags2, tagGroups2), strings.Tags));
			base.Tabs.Add(SettingsViewTab = new Tab(AsyncTexture2D.FromAssetId(157109), () => mainWindow.SettingsView = new SettingsView(settings2), strings_common.Settings));
		}

		private void TemplatePresenter_TemplateChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Template> e)
		{
			base.SubName = e.NewValue?.Name;
		}

		protected override void OnTabChanged(Blish_HUD.ValueChangedEventArgs<Tab> e)
		{
			base.OnTabChanged(e);
			if (e.NewValue == TemplateViewTab && Settings.ShowQuickFilterPanelOnTabOpen.Value)
			{
				QuickFiltersPanel.Show();
			}
			base.SubName = ((e.NewValue != TemplateViewTab) ? ((e.NewValue == SettingsViewTab) ? strings_common.Settings : ((e.NewValue == TagEditViewTab) ? strings.Tags : string.Empty)) : TemplatePresenter?.Template?.Name);
		}

		private void TemplatePresenter_NameChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<string> e)
		{
			base.SubName = e.NewValue;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (Settings.ShowQuickFilterPanelOnWindowOpen.Value)
			{
				QuickFiltersPanel.Show();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			int index = 1;
			int TAB_VERTICALOFFSET = 80;
			int TAB_HEIGHT = 50;
			int TAB_Width = 84;
			new Rectangle(0, TAB_VERTICALOFFSET + TAB_HEIGHT * index, TAB_Width, TAB_HEIGHT);
			Rectangle bounds = _quickFilterToggle.Bounds;
			if (((Rectangle)(ref bounds)).Contains(base.RelativeMousePosition))
			{
				QuickFiltersPanel.ToggleVisibility();
			}
			else
			{
				base.OnClick(e);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (SelectionPanel != null)
			{
				SelectionPanel.Pointer.ZIndex = ZIndex + ((WindowBase2.ActiveWindow == this) ? 1 : 0);
			}
			if (QuickFiltersPanel != null)
			{
				QuickFiltersPanel.ZIndex = ZIndex;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_quickFilterToggle.Bounds = new Rectangle(8, 45, 32, 32);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			_quickFilterToggle.Draw(this, spriteBatch, base.RelativeMousePosition, null, null, QuickFiltersPanel.Visible);
			if (_quickFilterToggle.Hovered)
			{
				base.BasicTooltipText = strings.ToggleQuickFilters;
			}
		}

		public override void Hide()
		{
			base.Hide();
			QuickFiltersPanel?.Hide();
		}

		protected override void DisposeControl()
		{
			Hide();
			base.DisposeControl();
			_tabbedRegion?.Dispose();
			BuildTab?.Dispose();
			GearTab?.Dispose();
			AboutTab?.Dispose();
			SelectionPanel?.Dispose();
			QuickFiltersPanel?.Dispose();
		}
	}
}
