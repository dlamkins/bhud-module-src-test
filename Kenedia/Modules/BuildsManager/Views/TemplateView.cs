using System;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Kenedia.Modules.BuildsManager.Controls;
using Kenedia.Modules.BuildsManager.Controls.Selection;
using Kenedia.Modules.BuildsManager.Controls.Tabs;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TemplateView : View
	{
		private TabbedRegion _tabbedRegion;

		public MainWindow MainWindow { get; }

		public SelectionPanel SelectionPanel { get; }

		public AboutTab AboutTab { get; }

		public BuildTab BuildTab { get; }

		public GearTab GearTab { get; }

		public QuickFiltersPanel QuickFiltersPanel { get; }

		public MainWindowPresenter MainWindowPresenter { get; }

		public TemplateView(MainWindow mainWindow, SelectionPanel selectionPanel, AboutTab aboutTab, BuildTab buildTab, GearTab gearTab, QuickFiltersPanel quickFiltersPanel, MainWindowPresenter mainWindowPresenter)
		{
			AboutTab = aboutTab;
			BuildTab = buildTab;
			GearTab = gearTab;
			QuickFiltersPanel = quickFiltersPanel;
			MainWindowPresenter = mainWindowPresenter;
			MainWindow = mainWindow;
			SelectionPanel = selectionPanel;
			QuickFiltersPanel.Anchor = mainWindow;
			QuickFiltersPanel.AnchorPosition = AnchoredContainer.AnchorPos.Left;
			QuickFiltersPanel.RelativePosition = new RectangleDimensions(0, 50, 0, 0);
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			base.Build(buildPanel);
			SelectionPanel.Parent = buildPanel;
			SelectionPanel.Location = new Point(35, 0);
			_tabbedRegion = new TabbedRegion
			{
				Parent = buildPanel,
				Location = new Point(SelectionPanel.Right + 15, 0),
				Width = buildPanel.ContentRegion.Width - 144,
				HeightSizingMode = SizingMode.Fill,
				OnTabSwitched = delegate
				{
					SelectionPanel?.ResetAnchor();
				}
			};
			_tabbedRegion.Width = buildPanel.ContentRegion.Width - (SelectionPanel.Right + 15);
			TabbedRegion tabbedRegion = _tabbedRegion;
			TabbedRegionTab obj = new TabbedRegionTab(AboutTab)
			{
				Header = () => strings.About,
				Icon = AsyncTexture2D.FromAssetId(440023)
			};
			TabbedRegionTab tab = obj;
			tabbedRegion.AddTab(obj);
			_tabbedRegion.AddTab(new TabbedRegionTab(BuildTab)
			{
				Header = () => strings.Build,
				Icon = AsyncTexture2D.FromAssetId(156720)
			});
			_tabbedRegion.AddTab(new TabbedRegionTab(GearTab)
			{
				Header = () => strings.Equipment,
				Icon = AsyncTexture2D.FromAssetId(156714)
			});
			Container presenterTab = ((MainWindowPresenter.SelectedTemplateTabType == typeof(AboutTab)) ? AboutTab : ((MainWindowPresenter.SelectedTemplateTabType == typeof(BuildTab)) ? ((Container)BuildTab) : ((Container)((MainWindowPresenter.SelectedTemplateTabType == typeof(GearTab)) ? GearTab : null))));
			tab = _tabbedRegion.Tabs.FirstOrDefault((TabbedRegionTab x) => x.Container == presenterTab) ?? tab;
			_tabbedRegion.SwitchTab(tab);
			TabbedRegion tabbedRegion2 = _tabbedRegion;
			tabbedRegion2.OnTabSwitched = (Action)Delegate.Combine(tabbedRegion2.OnTabSwitched, new Action(OnTabSwitched));
			BuildTab buildTab = BuildTab;
			GearTab gearTab = GearTab;
			int num2 = (AboutTab.Width = buildPanel.ContentRegion.Width - 144);
			int num5 = (buildTab.Width = (gearTab.Width = num2));
		}

		private void OnTabSwitched()
		{
			MainWindowPresenter mainWindowPresenter = MainWindowPresenter;
			TabbedRegionTab tab = _tabbedRegion.ActiveTab;
			if (tab == null)
			{
				goto IL_0066;
			}
			Type selectedTemplateTabType;
			if (tab.Container == AboutTab)
			{
				selectedTemplateTabType = typeof(AboutTab);
			}
			else if (tab.Container == BuildTab)
			{
				selectedTemplateTabType = typeof(BuildTab);
			}
			else
			{
				if (tab.Container != GearTab)
				{
					goto IL_0066;
				}
				selectedTemplateTabType = typeof(GearTab);
			}
			goto IL_0068;
			IL_0066:
			selectedTemplateTabType = null;
			goto IL_0068;
			IL_0068:
			mainWindowPresenter.SelectedTemplateTabType = selectedTemplateTabType;
		}
	}
}
