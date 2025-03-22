using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD.Content;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Achievements.Categories;
using GuildWars2.Hero.Achievements.Groups;
using Microsoft.Extensions.Localization;
using SL.ChatLinks.Storage;
using SL.ChatLinks.UI.Tabs.Achievements.Tooltips;
using SL.Common;
using SL.Common.ModelBinding;

namespace SL.ChatLinks.UI.Tabs.Achievements
{
	public sealed class AchievementTileViewModel : ViewModel, IDisposable
	{
		public delegate AchievementTileViewModel Factory(Achievement achievement, AchievementCategory? category, AchievementGroup? group, IReadOnlyList<AccountAchievement>? progression);

		private readonly IEventAggregator _eventAggregator;

		private readonly IStringLocalizer<AchievementTile> _localizer;

		private readonly IClipBoard _clipboard;

		private readonly IDbContextFactory _contextFactory;

		private readonly IconsService _icons;

		private readonly AchievementTooltipViewModel.Factory _achievementTooltipViewModelFactory;

		private Achievement _achievement;

		private AchievementCategory? _category;

		private AccountAchievement? _progress;

		private IReadOnlyList<AccountAchievement>? _progression;

		private AchievementGroup? _group;

		public string Name => Achievement.Name;

		public string CompletedLabel => (string)_localizer["Completed"];

		public string ChatLink => Achievement.GetChatLink().ToString();

		public string CopyNameLabel => (string)_localizer["Copy Name"];

		public RelayCommand CopyNameCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(Achievement.Name);
		});

		public string CopyChatLinkLabel => (string)_localizer["Copy Chat Link"];

		public RelayCommand CopyChatLinkCommand => new RelayCommand(delegate
		{
			_clipboard.SetText(ChatLink);
		});

		public string OpenWikiLabel => (string)_localizer["Open Wiki"];

		public RelayCommand OpenWikiCommand => new RelayCommand(delegate
		{
			Process.Start(_localizer["Wiki search", new object[1] { WebUtility.UrlEncode(Name) }]);
		});

		public string OpenApiLabel => (string)_localizer["Open API"];

		public RelayCommand OpenApiCommand => new RelayCommand(delegate
		{
			Process.Start(_localizer["Achievement API", new object[1] { Achievement.Id }]);
		});

		public Achievement Achievement
		{
			get
			{
				return _achievement;
			}
			set
			{
				SetField(ref _achievement, value, "Achievement");
			}
		}

		public AchievementCategory? Category
		{
			get
			{
				return _category;
			}
			set
			{
				SetField(ref _category, value, "Category");
			}
		}

		public AchievementGroup? Group
		{
			get
			{
				return _group;
			}
			set
			{
				SetField(ref _group, value, "Group");
			}
		}

		public IReadOnlyList<AccountAchievement>? Progression
		{
			get
			{
				return _progression;
			}
			set
			{
				if (SetField(ref _progression, value, "Progression"))
				{
					OnPropertyChanged("Progress");
				}
			}
		}

		public AccountAchievement? Progress => Progression.SingleOrDefault((AccountAchievement accountAchievement) => accountAchievement.Id == Achievement.Id);

		public bool Locked => Achievement.IsLocked(Group, Progression);

		public string AchievementProgressUnavailable => (string)_localizer["Achievement progress unavailable"];

		public string DailyAchievementProgressUnavailable => (string)_localizer["Daily achievement progress unavailable"];

		public string WeeklyAchievementProgressUnavailable => (string)_localizer["Weekly achievement progress unavailable"];

		public string PerCharacterAchievementProgressUnavailable => (string)_localizer["Per-character achievement progress unavailable"];

		public bool IsPerCharacter => Group?.IsPerCharacter() ?? false;

		public bool IsDaily => Achievement.Flags.Daily;

		public bool IsWeekly => Achievement.Flags.Weekly;

		public string HiddenLabel => (string)_localizer["Hidden achievement"];

		public AchievementTileViewModel(IEventAggregator eventAggregator, IStringLocalizer<AchievementTile> localizer, IClipBoard clipboard, IDbContextFactory contextFactory, IconsService icons, AchievementTooltipViewModel.Factory achievementTooltipViewModelFactory, Achievement achievement, AchievementCategory? category, AchievementGroup? group, IReadOnlyList<AccountAchievement>? progression)
		{
			ThrowHelper.ThrowIfNull(eventAggregator, "eventAggregator");
			_eventAggregator = eventAggregator;
			_localizer = localizer;
			_clipboard = clipboard;
			_contextFactory = contextFactory;
			_icons = icons;
			_achievementTooltipViewModelFactory = achievementTooltipViewModelFactory;
			_achievement = achievement;
			_category = category;
			_group = group;
			_progression = progression;
			eventAggregator.Subscribe(new Func<LocaleChanged, Task>(OnLocaleChanged));
		}

		private async Task OnLocaleChanged(LocaleChanged args)
		{
			OnPropertyChanged("CopyNameLabel");
			OnPropertyChanged("CopyChatLinkLabel");
			OnPropertyChanged("OpenWikiLabel");
			OnPropertyChanged("OpenApiLabel");
			ChatLinksContext context = _contextFactory.CreateDbContext(args.Language);
			ConfiguredAsyncDisposable configuredAsyncDisposable = context.ConfigureAwait(continueOnCapturedContext: false);
			try
			{
				Achievement = context.Achievements.SingleOrDefault((Achievement achievement) => achievement.Id == Achievement.Id);
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

		public AsyncTexture2D? GetIcon()
		{
			if (!string.IsNullOrEmpty(Achievement.IconHref))
			{
				return _icons.GetIcon(Achievement.IconHref);
			}
			if (!string.IsNullOrEmpty(_category?.IconHref))
			{
				return _icons.GetIcon(_category!.IconHref);
			}
			return null;
		}

		public AchievementTooltipViewModel CreateAchievementTooltipViewModel()
		{
			return _achievementTooltipViewModelFactory(Achievement);
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<LocaleChanged>(new Func<LocaleChanged, Task>(OnLocaleChanged));
		}
	}
}
