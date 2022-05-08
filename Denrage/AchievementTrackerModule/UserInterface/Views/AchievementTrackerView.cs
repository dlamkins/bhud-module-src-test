using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Denrage.AchievementTrackerModule.Interfaces;
using Denrage.AchievementTrackerModule.Models.Achievement;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Denrage.AchievementTrackerModule.UserInterface.Views
{
	public class AchievementTrackerView : View
	{
		private readonly SemaphoreSlim searchSemaphore = new SemaphoreSlim(1, 1);

		private readonly IAchievementItemOverviewFactory achievementItemOverviewFactory;

		private readonly IAchievementService achievementService;

		private readonly IDictionary<MenuItem, AchievementCategory> menuItemCategories;

		private Menu menu;

		private ViewContainer selectedMenuItemView;

		private Task delayTask;

		private CancellationTokenSource delayCancellationToken;

		private TextBox searchBar;

		private Dictionary<AchievementCategory, IEnumerable<AchievementTableEntry>> achievementCache;

		private Dictionary<int, AchievementCategory> categories;

		public AchievementTrackerView(IAchievementItemOverviewFactory achievementItemOverviewFactory, IAchievementService achievementService)
			: this()
		{
			this.achievementItemOverviewFactory = achievementItemOverviewFactory;
			this.achievementService = achievementService;
			menuItemCategories = new Dictionary<MenuItem, AchievementCategory>();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			TextBox val = new TextBox();
			((TextInputBase)val).set_PlaceholderText("Search...");
			((Control)val).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val).set_Parent(buildPanel);
			searchBar = val;
			((TextInputBase)searchBar).add_TextChanged((EventHandler<EventArgs>)SearchBar_TextChanged);
			Panel val2 = new Panel();
			val2.set_Title("Achievements");
			val2.set_ShowBorder(true);
			((Control)val2).set_Width(((DesignStandard)(ref Panel.MenuStandard)).get_Size().X);
			((Control)val2).set_Height(((DesignStandard)(ref Panel.MenuStandard)).get_Size().Y - ((Control)searchBar).get_Height() - 10);
			((Control)val2).set_Location(new Point(0, ((Control)searchBar).get_Height() + 10));
			((Control)val2).set_Parent(buildPanel);
			val2.set_CanScroll(true);
			Panel menuPanel = val2;
			Menu val3 = new Menu();
			Rectangle contentRegion = ((Container)menuPanel).get_ContentRegion();
			((Control)val3).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			val3.set_MenuItemHeight(40);
			val3.set_CanSelect(true);
			((Control)val3).set_Parent((Container)(object)menuPanel);
			menu = val3;
			ViewContainer val4 = new ViewContainer();
			val4.set_FadeView(true);
			((Control)val4).set_Parent(buildPanel);
			((Control)val4).set_Size(new Point(buildPanel.get_ContentRegion().Width - ((Control)menuPanel).get_Width(), ((Control)menuPanel).get_Height()));
			((Control)val4).set_Location(new Point(((Control)menuPanel).get_Width(), 0));
			selectedMenuItemView = val4;
			Label val5 = new Label();
			val5.set_Text("Weren't able to gather needed information from the API or it is still ongoing. Consult the log for details. Retrying every 5 minutes");
			((Control)val5).set_Parent(buildPanel);
			val5.set_Font(GameService.Content.get_DefaultFont18());
			val5.set_AutoSizeHeight(true);
			((Control)val5).set_Width(250);
			val5.set_WrapText(true);
			val5.set_TextColor(Color.get_Red());
			Label apiErrorLabel = val5;
			((Control)apiErrorLabel).set_Visible(false);
			((Control)apiErrorLabel).set_Location(new Point((((Control)selectedMenuItemView).get_Width() - ((Control)apiErrorLabel).get_Width()) / 2 + ((Control)selectedMenuItemView).get_Location().X, (((Control)selectedMenuItemView).get_Height() - ((Control)apiErrorLabel).get_Height()) / 2 + ((Control)selectedMenuItemView).get_Location().Y));
			if (achievementService.AchievementGroups == null || achievementService.AchievementCategories == null)
			{
				achievementService.ApiAchievementsLoaded += delegate
				{
					((Control)apiErrorLabel).set_Visible(false);
					((Control)searchBar).set_Enabled(true);
					InitializeAchievementElements();
				};
				((Control)searchBar).set_Enabled(false);
				((Control)apiErrorLabel).set_Visible(true);
			}
			else
			{
				InitializeAchievementElements();
			}
		}

		private void InitializeAchievementElements()
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			categories = achievementService.AchievementCategories.ToDictionary((AchievementCategory x) => x.get_Id(), (AchievementCategory y) => y);
			foreach (AchievementGroup group in achievementService.AchievementGroups.OrderBy((AchievementGroup x) => x.get_Order()))
			{
				MenuItem menuItem = menu.AddMenuItem(group.get_Name(), (Texture2D)null);
				foreach (AchievementCategory category in from x in @group.get_Categories()
					select categories[x] into x
					orderby x.get_Order()
					select x)
				{
					MenuItem val = new MenuItem(category.get_Name());
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
			if (!((Control)searchBar).get_Enabled())
			{
				return;
			}
			try
			{
				if (delayTask != null)
				{
					delayCancellationToken.Cancel();
					delayTask = null;
					delayCancellationToken = null;
				}
				delayCancellationToken = new CancellationTokenSource();
				delayTask = new Task(async delegate
				{
					await DelaySeach(delayCancellationToken.Token);
				}, delayCancellationToken.Token);
				delayTask.Start();
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async Task DelaySeach(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				await Task.Delay(300, cancellationToken);
				await SearchAsync(cancellationToken);
			}
			catch (OperationCanceledException)
			{
			}
		}

		private async Task SearchAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await searchSemaphore.WaitAsync(cancellationToken);
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				string searchText = ((TextInputBase)searchBar).get_Text();
				if (string.IsNullOrWhiteSpace(searchText))
				{
					selectedMenuItemView.Clear();
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
				selectedMenuItemView.Clear();
				selectedMenuItemView.Show((IView)(object)achievementItemOverviewFactory.Create(searchedAchievements, searchText));
			}
			finally
			{
				searchSemaphore.Release();
			}
		}
	}
}
