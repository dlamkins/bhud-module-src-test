using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
using GuildWars2.Authorization;
using GuildWars2.Hero.Achievements;
using GuildWars2.Hero.Equipment.Finishers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SL.Common;

namespace SL.ChatLinks
{
	public sealed class AccountUnlocks : IDisposable
	{
		private readonly ILogger<AccountUnlocks> _logger;

		private readonly Gw2Client _gw2Client;

		private readonly ITokenProvider _tokenProvider;

		private readonly IEventAggregator _eventAggregator;

		private readonly IMemoryCache _memoryCache;

		public bool IsAuthorized => _tokenProvider.IsAuthorized;

		public AccountUnlocks(ILogger<AccountUnlocks> logger, Gw2Client gw2Client, ITokenProvider tokenProvider, IEventAggregator eventAggregator, IMemoryCache memoryCache)
		{
			ThrowHelper.ThrowIfNull(eventAggregator, "eventAggregator");
			_logger = logger;
			_gw2Client = gw2Client;
			_tokenProvider = tokenProvider;
			_eventAggregator = eventAggregator;
			_memoryCache = memoryCache;
			eventAggregator.Subscribe(new Action<AuthorizationInvalidated>(OnAuthorizationInvalidated));
			eventAggregator.Subscribe(new Action<MapChanged>(OnMapChanged));
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

		public async ValueTask<IReadOnlyList<AccountAchievement>> GetAchievementProgress(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Progression))
				{
					return await _memoryCache.GetOrCreateAsync("achievements_progress", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<AccountAchievement>, MessageContext) obj = await _gw2Client.Hero.Achievements.GetAccountAchievements(token, MissingMemberBehavior.Error, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<AccountAchievement> value = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return value.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<AccountAchievement>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve account achievements.");
				return Array.Empty<AccountAchievement>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedDyes(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_dyes", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Dyes.GetUnlockedColors(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked dyes.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedFinishers(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_finishers", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<UnlockedFinisher>, MessageContext) obj = await _gw2Client.Hero.Equipment.Finishers.GetUnlockedFinishers(token, MissingMemberBehavior.Error, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<UnlockedFinisher> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.Select((UnlockedFinisher finisher) => finisher.Id).ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked finishers.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedGliderSkins(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_glider_skins", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Gliders.GetUnlockedGliderSkins(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked glider skins.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedJadeBotSkins(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks) && _tokenProvider.Grants.Contains(Permission.Inventories))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_jade_bot_skins", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.JadeBots.GetUnlockedJadeBotSkins(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked jade bots.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMailCarriers(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_mail_carriers", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.MailCarriers.GetUnlockedMailCarriers(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked mail carriers.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMiniatures(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_miniatures", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Miniatures.GetUnlockedMiniatures(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked miniatures.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMistChampionSkins(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_mist_champion_skins", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Pvp.GetUnlockedMistChampions(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked mist champions.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedNovelties(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_novelties", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Novelties.GetUnlockedNovelties(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked novelties.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedOutfits(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_outfits", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Outfits.GetUnlockedOutfits(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked outfits.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedRecipes(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_recipes", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Crafting.Recipes.GetUnlockedRecipes(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked recipes.");
				return Array.Empty<int>();
			}
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedWardrobe(CancellationToken cancellationToken)
		{
			try
			{
				if (_tokenProvider.Grants.Contains(Permission.Unlocks))
				{
					return await _memoryCache.GetOrCreateAsync("unlocked_wardrobe", async delegate(ICacheEntry entry)
					{
						string token = await _tokenProvider.GetTokenAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						(HashSet<int>, MessageContext) obj = await _gw2Client.Hero.Equipment.Wardrobe.GetUnlockedSkins(token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						HashSet<int> values = obj.Item1;
						MessageContext context = obj.Item2;
						entry.AbsoluteExpiration = context.Expires;
						return values.ToImmutableArray();
					}).ConfigureAwait(continueOnCapturedContext: false);
				}
				return Array.Empty<int>();
			}
			catch (Exception reason)
			{
				_logger.LogWarning(reason, "Failed to retrieve unlocked skins.");
				return Array.Empty<int>();
			}
		}

		private void OnAuthorizationInvalidated(AuthorizationInvalidated _)
		{
			ClearCache();
		}

		private void OnMapChanged(MapChanged _)
		{
			ClearCache();
		}

		private void ClearCache()
		{
			_memoryCache.Remove("achievements_progress");
			_memoryCache.Remove("unlocked_dyes");
			_memoryCache.Remove("unlocked_finishers");
			_memoryCache.Remove("unlocked_glider_skins");
			_memoryCache.Remove("unlocked_jade_bot_skins");
			_memoryCache.Remove("unlocked_mail_carriers");
			_memoryCache.Remove("unlocked_miniatures");
			_memoryCache.Remove("unlocked_mist_champion_skins");
			_memoryCache.Remove("unlocked_novelties");
			_memoryCache.Remove("unlocked_outfits");
			_memoryCache.Remove("unlocked_recipes");
			_memoryCache.Remove("unlocked_wardrobe");
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<AuthorizationInvalidated>(new Action<AuthorizationInvalidated>(OnAuthorizationInvalidated));
			_eventAggregator.Unsubscribe<MapChanged>(new Action<MapChanged>(OnMapChanged));
			ClearCache();
		}
	}
}
