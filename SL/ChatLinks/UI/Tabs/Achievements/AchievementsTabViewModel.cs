using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Blish_HUD;
using Blish_HUD.Content;
using GuildWars2.Authorization;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.Storage;
using SL.Common;
using SL.Common.ModelBinding;
using SL.Common.Progression;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementsTabViewModel : ViewModel, IDisposable
	{
		public delegate AchievementsTabViewModel Factory();

		[CompilerGenerated]
		private IEventAggregator _003CeventAggregator_003EP;

		[CompilerGenerated]
		private IDbContextFactory _003CcontextFactory_003EP;

		[CompilerGenerated]
		private IStringLocalizer<AchievementsTabView> _003Clocalizer_003EP;

		[CompilerGenerated]
		private ILocale _003Clocale_003EP;

		[CompilerGenerated]
		private AchievementTileViewModel.Factory _003CachievementTileViewModelFactory_003EP;

		[CompilerGenerated]
		private CurrentAccount _003Caccount_003EP;

		[CompilerGenerated]
		private IconsService _003Cicons_003EP;

		private ObservableCollection<AchievementGroupMenuItem> _groups;

		private ObservableCollection<AchievementTileViewModel> _achievements;

		private string? _headerText;

		private AsyncTexture2D? _headerIcon;

		private string _searchText;

		private AchievementCategory? _selectedCategory;

		public ObservableCollection<AchievementGroupMenuItem> MenuItems
		{
			get
			{
				return _groups;
			}
			private set
			{
				SetField(ref _groups, value, "MenuItems");
			}
		}

		public ObservableCollection<AchievementTileViewModel> Achievements
		{
			get
			{
				return _achievements;
			}
			private set
			{
				SetField(ref _achievements, value, "Achievements");
			}
		}

		public string? HeaderText
		{
			get
			{
				return _headerText;
			}
			set
			{
				SetField(ref _headerText, value, "HeaderText");
			}
		}

		public AsyncTexture2D? HeaderIcon
		{
			get
			{
				return _headerIcon;
			}
			set
			{
				SetField(ref _headerIcon, value, "HeaderIcon");
			}
		}

		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				SetField(ref _searchText, value, "SearchText");
			}
		}

		public ICommand SearchCommand => new AsyncRelayCommand(async delegate
		{
			await Task.Run((Func<Task>)OnSearch).ConfigureAwait(continueOnCapturedContext: false);
		});

		public string CategoriesTitle => (string)_003Clocalizer_003EP["Categories"];

		public string SearchPlaceholder => (string)_003Clocalizer_003EP["Search"];

		public AchievementCategory? SelectedCategory
		{
			get
			{
				return _selectedCategory;
			}
			set
			{
				SetField(ref _selectedCategory, value, "SelectedCategory");
			}
		}

		public AchievementsTabViewModel(IEventAggregator eventAggregator, IDbContextFactory contextFactory, IStringLocalizer<AchievementsTabView> localizer, ILocale locale, AchievementTileViewModel.Factory achievementTileViewModelFactory, CurrentAccount account, IconsService icons)
		{
			_003CeventAggregator_003EP = eventAggregator;
			_003CcontextFactory_003EP = contextFactory;
			_003Clocalizer_003EP = localizer;
			_003Clocale_003EP = locale;
			_003CachievementTileViewModelFactory_003EP = achievementTileViewModelFactory;
			_003Caccount_003EP = account;
			_003Cicons_003EP = icons;
			_groups = new ObservableCollection<AchievementGroupMenuItem>();
			_achievements = new ObservableCollection<AchievementTileViewModel>();
			_searchText = "";
			base._002Ector();
		}

		public async Task<bool> Load()
		{
			await LoadAchievementCategories().ConfigureAwait(continueOnCapturedContext: false);
			_003CeventAggregator_003EP.Subscribe(new Func<LocaleChanged, Task>(OnLocaleChanged));
			_003CeventAggregator_003EP.Subscribe(new Func<DatabaseSeeded, Task>(OnDatabaseSeeded));
			return true;
		}

		private async Task LoadAchievementCategories()
		{
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<AchievementGroup> groups2 = await context.AchievementGroups.OrderBy((AchievementGroup groups) => groups.Order).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<AchievementCategory> categories = await context.AchievementCategories.OrderBy((AchievementCategory category) => category.Order).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<int> list = new List<int>();
				list.AddRange(categories.SelectMany(delegate(AchievementCategory c)
				{
					List<int> list4 = new List<int>();
					list4.AddRange(c.Achievements.Select((AchievementRef a) => a.Id));
					list4.AddRange(c.Tomorrow?.Select((AchievementRef a) => a.Id) ?? Array.Empty<int>());
					return list4;
				}));
				List<int> categorizedIds = list;
				var achievements = await context.Achievements.Select((Achievement a) => new { a.Id, a.Flags }).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<AchievementCategory> list2 = categories;
				AchievementCategory obj = new AchievementCategory
				{
					Id = -1,
					Name = (string)_003Clocalizer_003EP["Uncategorized"],
					Description = "",
					IconHref = "",
					Order = int.MaxValue
				};
				List<AchievementRef> list3 = new List<AchievementRef>();
				list3.AddRange(from a in achievements
					where !a.Flags.Daily
					where !a.Flags.Weekly
					where !categorizedIds.Contains(a.Id)
					select new AchievementRef
					{
						Id = a.Id,
						Level = new LevelRequirement
						{
							Min = 0,
							Max = 0
						},
						Flags = new GuildWars2.Hero.Achievements.Categories.AchievementFlags
						{
							PvE = !a.Flags.Pvp,
							SpecialEvent = false,
							Other = Array.Empty<string>()
						}
					});
				obj.Achievements = new _003C_003Ez__ReadOnlyList<AchievementRef>(list3);
				obj.Tomorrow = Array.Empty<AchievementRef>();
				list2.Add(obj);
				groups2.Add(new AchievementGroup
				{
					Id = "",
					Name = (string)_003Clocalizer_003EP["Uncategorized"],
					Description = "",
					Order = int.MaxValue,
					Categories = new _003C_003Ez__ReadOnlyArray<int>(new int[1] { -1 })
				});
				ObservableCollection<AchievementGroupMenuItem> observableCollection = new ObservableCollection<AchievementGroupMenuItem>();
				foreach (AchievementGroupMenuItem item in groups2.Select((AchievementGroup group) => new AchievementGroupMenuItem
				{
					Group = group,
					Categories = categories.Where((AchievementCategory category) => group.Categories.Contains(category.Id))
				}))
				{
					observableCollection.Add(item);
				}
				MenuItems = observableCollection;
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		private async Task OnLocaleChanged(LocaleChanged changed)
		{
			OnPropertyChanged("SearchPlaceholder");
			OnPropertyChanged("CategoriesTitle");
			await LoadAchievementCategories().ConfigureAwait(continueOnCapturedContext: false);
			if ((object)SelectedCategory == null)
			{
				await OnSearch().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private async Task OnDatabaseSeeded(DatabaseSeeded seeded)
		{
			if (seeded.Updated["achievement_categories"] > 0)
			{
				await LoadAchievementCategories().ConfigureAwait(continueOnCapturedContext: false);
			}
			if ((object)SelectedCategory == null && seeded.Updated["achievements"] > 0)
			{
				await OnSearch().ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		public async Task OnSearch()
		{
			SelectedCategory = null;
			ObservableCollection<AchievementTileViewModel> results = new ObservableCollection<AchievementTileViewModel>();
			string query = SearchText.Trim();
			if (query.Length >= 3)
			{
				ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
				{
					ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
					try
					{
						List<Achievement> achievements2 = await (from achievement in context.Achievements
							where EF.Functions.Like(achievement.Name, $"%{query}%")
							orderby achievement.Name
							select achievement).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
						if (achievements2.Count > 0)
						{
							List<AchievementCategory> categories = await context.AchievementCategories.ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
							List<AchievementGroup> groups = await context.AchievementGroups.ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
							List<AccountAchievement> progression = null;
							if (_003Caccount_003EP.HasPermission(Permission.Progression))
							{
								IReadOnlyList<AccountAchievement> readOnlyList = await _003Caccount_003EP.GetAchievementProgress(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
								List<AccountAchievement> list = new List<AccountAchievement>(readOnlyList.Count);
								list.AddRange(readOnlyList);
								progression = list;
							}
							achievements2 = SortAchievements(achievements2, categories, groups, progression);
							foreach (Achievement achievement2 in achievements2)
							{
								AchievementCategory category2 = categories.FirstOrDefault((AchievementCategory category) => category.IsParentOf(achievement2.Id) == true);
								AchievementGroup group2 = null;
								if ((object)category2 != null)
								{
									group2 = groups.FirstOrDefault((AchievementGroup group) => group.Categories.Contains(category2.Id));
								}
								AchievementTileViewModel achievementTileViewModel = _003CachievementTileViewModelFactory_003EP(achievement2, category2, group2, progression);
								results.Add(achievementTileViewModel);
							}
						}
					}
					finally
					{
						IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
						if (asyncDisposable != null)
						{
							await asyncDisposable.DisposeAsync();
						}
					}
				}
			}
			HeaderIcon = AsyncTexture2D.FromAssetId(155061);
			HeaderText = SearchText;
			Achievements = results;
		}

		public async Task SelectCategory(AchievementCategory category)
		{
			AchievementCategory category2 = category;
			ThrowHelper.ThrowIfNull(category2, "category");
			SelectedCategory = category2;
			ChatLinksContext context = _003CcontextFactory_003EP.CreateDbContext(_003Clocale_003EP.Current);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				List<AchievementGroup> groups = await context.AchievementGroups.ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<AchievementGroup> list = new List<AchievementGroup>();
				list.AddRange(groups.Where((AchievementGroup group) => group.Categories.Contains(category2.Id)));
				groups = list;
				IEnumerable<int> ids = category2.Achievements.Select((AchievementRef achievement) => achievement.Id);
				List<Achievement> achievements2 = await (from achievement in context.Achievements
					where ids.Contains(achievement.Id)
					orderby achievement.Name
					select achievement).ToListAsync().ConfigureAwait(continueOnCapturedContext: false);
				List<AccountAchievement> progression = null;
				if (_003Caccount_003EP.HasPermission(Permission.Progression))
				{
					IReadOnlyList<AccountAchievement> readOnlyList = await _003Caccount_003EP.GetAchievementProgress(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
					List<AccountAchievement> list2 = new List<AccountAchievement>(readOnlyList.Count);
					list2.AddRange(readOnlyList);
					progression = list2;
				}
				achievements2 = SortAchievements(achievements2, new List<AchievementCategory>(1) { category2 }, groups, progression);
				HeaderText = ((!string.IsNullOrEmpty(category2.Name)) ? category2.Name : null);
				HeaderIcon = ((!string.IsNullOrEmpty(category2.IconHref)) ? GameService.Content.GetRenderServiceTexture(category2.IconHref) : null);
				ObservableCollection<AchievementTileViewModel> observableCollection = new ObservableCollection<AchievementTileViewModel>();
				foreach (AchievementTileViewModel item in achievements2.Select((Achievement achievement) => _003CachievementTileViewModelFactory_003EP(achievement, category2, groups.FirstOrDefault(), progression)))
				{
					observableCollection.Add(item);
				}
				Achievements = observableCollection;
			}
			finally
			{
				IAsyncDisposable asyncDisposable = configuredAsyncDisposable as IAsyncDisposable;
				if (asyncDisposable != null)
				{
					await asyncDisposable.DisposeAsync();
				}
			}
		}

		private static List<Achievement> SortAchievements(List<Achievement> achievements, List<AchievementCategory> categories, List<AchievementGroup> groups, List<AccountAchievement>? progression)
		{
			List<AchievementCategory> categories2 = categories;
			List<AchievementGroup> groups2 = groups;
			List<AccountAchievement> progression2 = progression;
			List<Achievement> list = new List<Achievement>();
			list.AddRange(from achievement in achievements
				let category = categories2.FirstOrDefault((AchievementCategory category) => category.IsParentOf(achievement.Id) == true)
				let @group = groups2.FirstOrDefault((AchievementGroup x) => (object)category != null && x.Categories.Contains(category.Id))
				let locked = achievement.IsLocked(@group, progression2)
				let hidden = achievement.IsHidden(progression2)
				orderby hidden, locked, @group?.Order ?? int.MaxValue, category?.Order ?? int.MaxValue, achievement.Flags.CategoryDisplay descending, achievement.Flags.MoveToTop descending
				select achievement);
			return list;
		}

		public AsyncTexture2D GetIcon(Uri? iconUrl)
		{
			return _003Cicons_003EP.GetIcon(iconUrl) ?? AsyncTexture2D.FromAssetId(155865).Duplicate();
		}

		public void Dispose()
		{
			_003CeventAggregator_003EP.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, Task>(OnLocaleChanged));
		}
	}
}
