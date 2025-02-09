using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GuildWars2;
using GuildWars2.Authorization;
using GuildWars2.Hero.Equipment.Finishers;
using SL.Common;

namespace SL.ChatLinks
{
	public sealed class Hero : IDisposable
	{
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

		public bool InventoriesAvailable
		{
			get
			{
				if (IsAuthorized)
				{
					return _tokenProvider.Grants.Contains(Permission.Inventories);
				}
				return false;
			}
		}

		public bool UnlocksAvailable
		{
			get
			{
				if (IsAuthorized)
				{
					return _tokenProvider.Grants.Contains(Permission.Unlocks);
				}
				return false;
			}
		}

		public Hero(Gw2Client gw2Client, ITokenProvider tokenProvider, IEventAggregator eventAggregator)
		{
			_gw2Client = gw2Client;
			_tokenProvider = tokenProvider;
			_eventAggregator = eventAggregator;
			eventAggregator.Subscribe(new Func<AuthorizationInvalidated, ValueTask>(OnAuthorizationInvalidated));
		}

		private async ValueTask OnAuthorizationInvalidated(AuthorizationInvalidated _)
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
			_unlockedFinishers = await unlockedFinishersTask;
			_unlockedGliderSkins = await unlockedGliderSkinsTask;
			_unlockedJadeBotSkins = await unlockedJadeBotSkinsTask;
			_unlockedMailCarriers = await unlockedMailCarriersTask;
			_unlockedMiniatures = await unlockedMiniaturesTask;
			_unlockedMistChampionSkins = await unlockedMistChampionSkinsTask;
			_unlockedNovelties = await unlockedNoveltiesTask;
			_unlockedOutfits = await unlockedOutfitsTask;
			_unlockedWardrobe = await unlockedWardrobeTask;
			_unlockedRecipes = await unlockedRecipesTask;
		}

		public void Dispose()
		{
			_eventAggregator.Unsubscribe<AuthorizationInvalidated>(new Func<AuthorizationInvalidated, ValueTask>(OnAuthorizationInvalidated));
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedDyes(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedDyes;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedDyes = await GetUnlockedDyesInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedDyesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Dyes.GetUnlockedColors(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedFinishers(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedFinishers;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedFinishers = await GetUnlockedFinishersInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedFinishersInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Finishers.GetUnlockedFinishers(token, MissingMemberBehavior.Error, cancellationToken).ValueOnly()).Select((UnlockedFinisher finisher) => finisher.Id).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedGliderSkins(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedGliderSkins;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedGliderSkins = await GetUnlockedGliderSkinsInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedGliderSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Gliders.GetUnlockedGliderSkins(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedJadeBotSkins(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedJadeBotSkins;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedJadeBotSkins = await GetUnlockedJadeBotSkinsInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedJadeBotSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks) || !_tokenProvider.Grants.Contains(Permission.Inventories))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.JadeBots.GetUnlockedJadeBotSkins(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMailCarriers(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedMailCarriers;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedMailCarriers = await GetUnlockedMailCarriersInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMailCarriersInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.MailCarriers.GetUnlockedMailCarriers(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMiniatures(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedMiniatures;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedMiniatures = await GetUnlockedMiniaturesInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMiniaturesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Miniatures.GetUnlockedMiniatures(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedMistChampionSkins(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedMistChampionSkins;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedMistChampionSkins = await GetUnlockedMistChampionSkinsInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedMistChampionSkinsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Pvp.GetUnlockedMistChampions(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedNovelties(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedNovelties;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedNovelties = await GetUnlockedNoveltiesInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedNoveltiesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Novelties.GetUnlockedNovelties(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedOutfits(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedOutfits;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedOutfits = await GetUnlockedOutfitsInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedOutfitsInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Outfits.GetUnlockedOutfits(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedRecipes(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedRecipes;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedRecipes = await GetUnlockedRecipesInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedRecipesInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Crafting.Recipes.GetUnlockedRecipes(token, cancellationToken).ValueOnly()).ToImmutableList();
		}

		public async ValueTask<IReadOnlyList<int>> GetUnlockedWardrobe(CancellationToken cancellationToken)
		{
			IReadOnlyList<int> readOnlyList = _unlockedWardrobe;
			if (readOnlyList == null)
			{
				readOnlyList = (_unlockedWardrobe = await GetUnlockedWardrobeInternal(cancellationToken));
			}
			return readOnlyList;
		}

		private async ValueTask<IReadOnlyList<int>> GetUnlockedWardrobeInternal(CancellationToken cancellationToken)
		{
			if (!_tokenProvider.Grants.Contains(Permission.Unlocks))
			{
				return Array.Empty<int>();
			}
			string token = await _tokenProvider.GetTokenAsync(cancellationToken);
			return (await _gw2Client.Hero.Equipment.Wardrobe.GetUnlockedSkins(token, cancellationToken).ValueOnly()).ToImmutableList();
		}
	}
}
