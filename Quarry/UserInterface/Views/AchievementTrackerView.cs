using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarry.Interfaces;
using Quarry.WikiData.Achievement;

namespace Quarry.UserInterface.Views
{
	public class AchievementTrackerView : View
	{
		private readonly SemaphoreSlim searchSemaphore = new SemaphoreSlim(1, 1);

		private readonly IAchievementItemOverviewFactory achievementItemOverviewFactory;

		private readonly IAchievementService achievementService;

		private readonly ITextureService textureService;

		private readonly Action openTrackedWindow;

		private readonly IDictionary<MenuItem, AchievementCategory> menuItemCategories;

		private Menu menu;

		private ViewContainer selectedMenuItemView;

		private Task delayTask;

		private CancellationTokenSource delayCancellationToken;

		private TextBox searchBar;

		private Action apiAchievementsLoadedHandler;

		private Dictionary<AchievementCategory, IEnumerable<AchievementTableEntry>> achievementCache;

		private Dictionary<int, AchievementCategory> categories;

		public AchievementTrackerView(IAchievementItemOverviewFactory achievementItemOverviewFactory, IAchievementService achievementService, ITextureService textureService, Action openTrackedWindow)
			: this()
		{
			this.achievementItemOverviewFactory = achievementItemOverviewFactory;
			this.achievementService = achievementService;
			this.textureService = textureService;
			this.openTrackedWindow = openTrackedWindow;
			menuItemCategories = new Dictionary<MenuItem, AchievementCategory>();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Expected O, but got Unknown
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search...");
			((Control)val).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X - 110 - 5);
			((Control)val).set_Parent(buildPanel);
			searchBar = val;
			((TextInputBase)searchBar).add_TextChanged((EventHandler<EventArgs>)SearchBar_TextChanged);
			StandardButton val2 = new StandardButton();
			val2.set_Text("Target List");
			((Control)val2).set_Width(110);
			((Control)val2).set_Height(((Control)searchBar).get_Height());
			((Control)val2).set_Location(new Point(((Control)searchBar).get_Width() + 5, 0));
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				openTrackedWindow();
			});
			Panel val3 = new Panel();
			val3.set_Title("Achievements");
			val3.set_ShowBorder(true);
			((Control)val3).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val3).set_Height(buildPanel.get_ContentRegion().Height - ((Control)searchBar).get_Height() - 10);
			((Control)val3).set_Location(new Point(0, ((Control)searchBar).get_Height() + 10));
			((Control)val3).set_Parent(buildPanel);
			val3.set_CanScroll(true);
			Panel menuPanel = val3;
			Menu val4 = new Menu();
			Rectangle contentRegion = ((Container)menuPanel).get_ContentRegion();
			((Control)val4).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val4.set_MenuItemHeight(40);
			val4.set_CanSelect(true);
			((Control)val4).set_Parent((Container)(object)menuPanel);
			menu = val4;
			ViewContainer val5 = new ViewContainer();
			val5.set_FadeView(true);
			((Control)val5).set_Parent(buildPanel);
			((Control)val5).set_Size(new Point(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width(), ((Control)menuPanel).get_Height()));
			((Control)val5).set_Location(new Point(((Control)menuPanel).get_Width(), 0));
			selectedMenuItemView = val5;
			Label val6 = new Label();
			val6.set_Text("Weren't able to gather needed information from the API or it is still ongoing. Consult the log for details. Retrying every 5 minutes");
			((Control)val6).set_Parent(buildPanel);
			val6.set_Font(GameService.Content.get_DefaultFont18());
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Width(250);
			val6.set_WrapText(true);
			val6.set_TextColor(Color.get_Red());
			Label apiErrorLabel = val6;
			((Control)apiErrorLabel).set_Visible(false);
			((Control)apiErrorLabel).set_Location(new Point((((Control)selectedMenuItemView).get_Width() - ((Control)apiErrorLabel).get_Width()) / 2 + ((Control)selectedMenuItemView).get_Location().X, (((Control)selectedMenuItemView).get_Height() - ((Control)apiErrorLabel).get_Height()) / 2 + ((Control)selectedMenuItemView).get_Location().Y));
			if (achievementService.AchievementGroups == null || achievementService.AchievementCategories == null)
			{
				apiAchievementsLoadedHandler = delegate
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						((Control)apiErrorLabel).set_Visible(false);
						((Control)searchBar).set_Enabled(true);
						InitializeAchievementElements();
					});
				};
				achievementService.ApiAchievementsLoaded += apiAchievementsLoadedHandler;
				((Control)searchBar).set_Enabled(false);
				((Control)apiErrorLabel).set_Visible(true);
			}
			else
			{
				InitializeAchievementElements();
			}
		}

		protected override void Unload()
		{
			if (apiAchievementsLoadedHandler != null)
			{
				achievementService.ApiAchievementsLoaded -= apiAchievementsLoadedHandler;
			}
			delayCancellationToken?.Cancel();
			delayCancellationToken?.Dispose();
			searchSemaphore.Dispose();
			((View<IPresenter>)this).Unload();
		}

		private void InitializeAchievementElements()
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			categories = achievementService.AchievementCategories.ToDictionary((AchievementCategory x) => x.get_Id(), (AchievementCategory y) => y);
			foreach (AchievementGroup group in achievementService.AchievementGroups.OrderBy((AchievementGroup x) => x.get_Order()))
			{
				MenuItem menuItem = menu.AddMenuItem(group.get_Name(), (Texture2D)null);
				foreach (AchievementCategory category in from x in @group.get_Categories()
					select categories[x] into x
					orderby x.get_Order()
					select x)
				{
					MenuItem val = new MenuItem(category.get_Name(), textureService.GetTexture(RenderUrl.op_Implicit(category.get_Icon())));
					((Control)val).set_Parent((Container)(object)menuItem);
					MenuItem innerMenuItem = val;
					innerMenuItem.add_ItemSelected((EventHandler<ControlActivatedEventArgs>)delegate(object sender, ControlActivatedEventArgs e)
					{
						//IL_000e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0018: Expected O, but got Unknown
						AchievementCategory menuItemCategory = menuItemCategories[(MenuItem)sender];
						IEnumerable<AchievementTableEntry> source = achievementService.Achievements.Where((AchievementTableEntry x) => menuItemCategory.get_Achievements().Contains(x.Id));
						selectedMenuItemView.Clear();
						selectedMenuItemView.Show((IView)(object)achievementItemOverviewFactory.Create(source.Select((AchievementTableEntry x) => (menuItemCategory, x)), menuItemCategory.get_Name()));
					});
					menuItemCategories.Add(innerMenuItem, category);
				}
			}
		}

		private void SearchBar_TextChanged(object sender, EventArgs e)
		{
			if (((Control)searchBar).get_Enabled())
			{
				if (delayTask != null)
				{
					delayCancellationToken.Cancel();
					delayCancellationToken.Dispose();
					delayTask = null;
					delayCancellationToken = null;
				}
				string searchText = ((TextInputBase)searchBar).get_Text();
				delayCancellationToken = new CancellationTokenSource();
				delayTask = Task.Run(() => DelaySeach(searchText, delayCancellationToken.Token), delayCancellationToken.Token);
			}
		}

		private async Task DelaySeach(string searchText, CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				await Task.Delay(300, cancellationToken);
				await SearchAsync(searchText, cancellationToken);
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async Task SearchAsync(string searchText, CancellationToken cancellationToken = default(CancellationToken))
		{
			await searchSemaphore.WaitAsync(cancellationToken);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (string.IsNullOrWhiteSpace(searchText))
				{
					GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
					{
						if (!cancellationToken.IsCancellationRequested)
						{
							selectedMenuItemView.Clear();
						}
					});
					return;
				}
				if (achievementCache == null)
				{
					Dictionary<AchievementCategory, IEnumerable<AchievementTableEntry>> achievements = new Dictionary<AchievementCategory, IEnumerable<AchievementTableEntry>>();
					foreach (AchievementCategory item in categories.Values)
					{
						if (item.get_Achievements().Count > 0)
						{
							achievements[item] = achievementService.Achievements.Where((AchievementTableEntry x) => item.get_Achievements().Contains(x.Id));
						}
					}
					achievementCache = achievements;
				}
				List<(AchievementCategory, AchievementTableEntry)> searchedAchievements = new List<(AchievementCategory, AchievementTableEntry)>();
				foreach (KeyValuePair<AchievementCategory, IEnumerable<AchievementTableEntry>> item2 in achievementCache)
				{
					cancellationToken.ThrowIfCancellationRequested();
					foreach (AchievementTableEntry categoryAchievement in item2.Value.Where((AchievementTableEntry x) => x.Name.ToUpper().Contains(searchText.ToUpper())))
					{
						searchedAchievements.Add((item2.Key, categoryAchievement));
					}
				}
				GameService.Overlay.QueueMainThreadUpdate((Action<GameTime>)delegate
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						selectedMenuItemView.Clear();
						selectedMenuItemView.Show((IView)(object)achievementItemOverviewFactory.Create(searchedAchievements, searchText));
					}
				});
			}
			finally
			{
				searchSemaphore.Release();
			}
		}
	}
}
