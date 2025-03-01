using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
using GuildWars2.Authorization;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Equipment.Finishers;
using Microsoft.Extensions.Logging;
using SL.Common;

namespace SL.ChatLinks
{
	public sealed class AccountUnlocks : IDisposable
	{
		private IReadOnlyList<AccountAchievement>? _accountAchievements;

		private readonly ILogger<AccountUnlocks> _logger;

		private readonly Gw2Client _gw2Client;

		private readonly ITokenProvider _tokenProvider;

		private readonly IEventAggregator _eventAggregator;

		private IReadOnlyList<int>? _unlockedDyes;

		private IReadOnlyList<int>? _unlockedFinishers;

		private IReadOnlyList<int>? _unlockedGliderSkins;

		private IReadOnlyList<int>? _unlockedJadeBotSkins;

		private IReadOnlyList<int>? _unlockedMailCarriers;

		private IReadOnlyList<int>? _unlockedMiniatures;

		private IReadOnlyList<int>? _unlockedMistChampionSkins;

		private IReadOnlyList<int>? _unlockedNovelties;

		private IReadOnlyList<int>? _unlockedOutfits;

		private IReadOnlyList<int>? _unlockedRecipes;

		private IReadOnlyList<int>? _unlockedWardrobe;

		public bool IsAuthorized => _tokenProvider.IsAuthorized;

		public async ValueTask<IReadOnlyList<AccountAchievement>> GetAccountAchievements(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<AccountAchievement> readOnlyList = _accountAchievements;
				if (readOnlyList == null)
				{
					readOnlyList = (_accountAchievements = await GetAccountAchievementsInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve account achievements.");
				return Array.Empty<AccountAchievement>();
			}
		}

		private async ValueTask<IReadOnlyList<AccountAchievement>> GetAccountAchievementsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Progression))
			{
				return Array.Empty<AccountAchievement>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<AccountAchievement> obj = await _gw2Client.Hero.Achievements.GetAccountAchievements(token, MissingMemberBehavior.Error, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			AccountAchievement[] array = new AccountAchievement[obj.Count];
			foreach (AccountAchievement item in obj)
			{
				AccountAchievement accountAchievement = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<AccountAchievement>(array);
		}

		public AccountUnlocks(ILogger<AccountUnlocks> logger, Gw2Client gw2Client, ITokenProvider tokenProvider, IEventAggregator eventAggregator)
		{
			ThrowHelper.ThrowIfNull(eventAggregator, "eventAggregator");
			_logger = logger;
			_gw2Client = gw2Client;
			_tokenProvider = tokenProvider;
			_eventAggregator = eventAggregator;
			eventAggregator.Subscribe(new Func<AuthorizationInvalidated, Task>(OnAuthorizationInvalidated));
		}

		public bool HasPermission(Permission permission)
		{
			if (IsAuthorized)
			{
				return _tokenProvider.Grants.Contains(permission);
			}
			return false;
		}

		public bool HasPermissions(params Permission[] permissions)
		{
			if (IsAuthorized)
			{
				return permissions.All((Permission permission) => _tokenProvider.Grants.Contains(permission));
			}
			return false;
		}

		private async Task OnAuthorizationInvalidated(AuthorizationInvalidated _)
		{
			string token = await _tokenProvider.GetTokenAsync(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
			if (token != null && await HasAccountPermission(token).ConfigureAwait(continueOnCapturedContext: false))
			{
				ValueTask<IReadOnlyList<int>> unlockedFinishersTask = GetUnlockedFinishersInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedGliderSkinsTask = GetUnlockedGliderSkinsInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedJadeBotSkinsTask = GetUnlockedJadeBotSkinsInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedMailCarriersTask = GetUnlockedMailCarriersInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedMiniaturesTask = GetUnlockedMiniaturesInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedMistChampionSkinsTask = GetUnlockedMistChampionSkinsInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedNoveltiesTask = GetUnlockedNoveltiesInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedOutfitsTask = GetUnlockedOutfitsInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedWardrobeTask = GetUnlockedWardrobeInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<int>> unlockedRecipesTask = GetUnlockedRecipesInternal(CancellationToken.None);
				ValueTask<IReadOnlyList<AccountAchievement>> accountAchievementsTask = GetAccountAchievementsInternal(CancellationToken.None);
				try
				{
					_unlockedFinishers = await unlockedFinishersTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason11)
				{
					_logger.LogWarning(reason11, "Failed to retrieve unlocked finishers.");
				}
				try
				{
					_unlockedGliderSkins = await unlockedGliderSkinsTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason10)
				{
					_logger.LogWarning(reason10, "Failed to retrieve unlocked gliders.");
				}
				try
				{
					_unlockedJadeBotSkins = await unlockedJadeBotSkinsTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason9)
				{
					_logger.LogWarning(reason9, "Failed to retrieve unlocked jade bots.");
				}
				try
				{
					_unlockedMailCarriers = await unlockedMailCarriersTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason8)
				{
					_logger.LogWarning(reason8, "Failed to retrieve unlocked mail carriers.");
				}
				try
				{
					_unlockedMiniatures = await unlockedMiniaturesTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason7)
				{
					_logger.LogWarning(reason7, "Failed to retrieve unlocked miniatures.");
				}
				try
				{
					_unlockedMistChampionSkins = await unlockedMistChampionSkinsTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason6)
				{
					_logger.LogWarning(reason6, "Failed to retrieve unlocked mist champions.");
				}
				try
				{
					_unlockedNovelties = await unlockedNoveltiesTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason5)
				{
					_logger.LogWarning(reason5, "Failed to retrieve unlocked novelties.");
				}
				try
				{
					_unlockedOutfits = await unlockedOutfitsTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason4)
				{
					_logger.LogWarning(reason4, "Failed to retrieve unlocked outfits.");
				}
				try
				{
					_unlockedWardrobe = await unlockedWardrobeTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason3)
				{
					_logger.LogWarning(reason3, "Failed to retrieve unlocked skins.");
				}
				try
				{
					_unlockedRecipes = await unlockedRecipesTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason2)
				{
					_logger.LogWarning(reason2, "Failed to retrieve unlocked recipes.");
				}
				try
				{
					_accountAchievements = await accountAchievementsTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception reason)
				{
					_logger.LogWarning(reason, "Failed to retrieve unlocked recipes.");
				}
			}
		}

		private async Task<bool> HasAccountPermission(string token)
		{
			int attempt = 0;
			while (attempt < 10)
			{
				if (attempt > 0)
				{
					await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
				}
				try
				{
					return (await _gw2Client.Tokens.GetTokenInfo(token, MissingMemberBehavior.Undefined, CancellationToken.None).ValueOnly().ConfigureAwait(continueOnCapturedContext: false)).Permissions.Contains(Permission.Account);
				}
				catch (Exception reason)
				{
					_logger.LogWarning(reason, "Failed to refresh token info.");
					attempt++;
				}
			}
			return false;
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<AuthorizationInvalidated>(new Func<AuthorizationInvalidated, Task>(OnAuthorizationInvalidated));
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedDyes(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedDyes;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedDyes = await GetUnlockedDyesInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked dyes.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedDyesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Dyes.GetUnlockedColors(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedFinishers(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedFinishers;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedFinishers = await GetUnlockedFinishersInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked finishers.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedFinishersInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<UnlockedFinisher> values = await _gw2Client.Hero.Equipment.Finishers.GetUnlockedFinishers(token, MissingMemberBehavior.Error, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			List<int> list = new List<int>();
			list.AddRange(values.Select((UnlockedFinisher finisher) => finisher.Id));
			return new _003C_003Ez__ReadOnlyList<int>(list);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedGliderSkins(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedGliderSkins;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedGliderSkins = await GetUnlockedGliderSkinsInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked gliders.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedGliderSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Gliders.GetUnlockedGliderSkins(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedJadeBotSkins(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedJadeBotSkins;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedJadeBotSkins = await GetUnlockedJadeBotSkinsInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked jade bots.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedJadeBotSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks) || !_tokenProvider.Grants.Contains(Permission.Inventories))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.JadeBots.GetUnlockedJadeBotSkins(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMailCarriers(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedMailCarriers;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedMailCarriers = await GetUnlockedMailCarriersInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked mail carriers.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMailCarriersInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.MailCarriers.GetUnlockedMailCarriers(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMiniatures(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedMiniatures;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedMiniatures = await GetUnlockedMiniaturesInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked miniatures.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMiniaturesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Miniatures.GetUnlockedMiniatures(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMistChampionSkins(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedMistChampionSkins;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedMistChampionSkins = await GetUnlockedMistChampionSkinsInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked mist champions.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMistChampionSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Pvp.GetUnlockedMistChampions(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedNovelties(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedNovelties;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedNovelties = await GetUnlockedNoveltiesInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked novelties.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedNoveltiesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Novelties.GetUnlockedNovelties(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedOutfits(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedOutfits;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedOutfits = await GetUnlockedOutfitsInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked outfits.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedOutfitsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Outfits.GetUnlockedOutfits(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedRecipes(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedRecipes;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedRecipes = await GetUnlockedRecipesInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked recipes.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedRecipesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Crafting.Recipes.GetUnlockedRecipes(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedWardrobe(CancellationToken cancellationToken)
		{
			try
			{
				IReadOnlyList<int> readOnlyList = _unlockedWardrobe;
				if (readOnlyList == null)
				{
					readOnlyList = (_unlockedWardrobe = await GetUnlockedWardrobeInternal(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
				}
				return readOnlyList;
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked skins.");
				return Array.Empty<int>();
			}
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedWardrobeInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			HashSet<int> obj = await _gw2Client.Hero.Equipment.Wardrobe.GetUnlockedSkins(token, cancellationToken).ValueOnly().ConfigureAwait(continueOnCapturedContext: false);
			int num = 0;
			int[] array = new int[obj.Count];
			foreach (int item in obj)
			{
				int num2 = (array[num] = item);
				num++;
			}
			return new _003C_003Ez__ReadOnlyArray<int>(array);
		}
	}
}
