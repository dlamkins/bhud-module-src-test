using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.Services
{
	internal class ResourceService : IDisposable
	{
		private Dictionary<int, string> _profNames;

		private Dictionary<int, AsyncTexture2D> _profIcons;

		private Dictionary<int, string> _eliteNames;

		private Dictionary<int, AsyncTexture2D> _eliteIcons;

		private Resources _resources;

		private IReadOnlyList<Map> _maps;

		private Dictionary<int, AsyncTexture2D> _apiIcons;

		private IReadOnlyList<SoundEffect> _menuClicks;

		private SoundEffect _menuItemClickSfx;

		private readonly IReadOnlyList<string> _loadingText = new List<string>
		{
			"Turning Vault upside down…", "Borrowing wallet…", "Tickling characters…", "High-fiving Deimos…", "Checking on Dhuum's cage…", "Throwing rocks into Mystic Forge…", "Lock-picking Ahdashim…", "Mounting Gorseval…", "Knitting Xera's ribbon…", "Caring for the bees…",
			"Dismantling White Mantle…", "Chasing Skritt…", "Ransacking bags…", "Poking Saul…", "Commanding golems…", "Polishing monocle…", "Running in circles…", "Scratching Slothasor…", "Cleaning Kitty Golem…", "Making sense of inventory…",
			"Pleading for Glenna's assistance…", "Counting achievements…", "Blowing away dust…", "Calling upon spirits…", "Consulting Order of Shadows…", "Bribing Pact troops…", "Bribing Bankers…"
		};

		private const string RESOURCE_PROFILE = "Nika";

		public ResourceService()
		{
			LoadSounds();
			_profNames = new Dictionary<int, string>();
			_profIcons = new Dictionary<int, AsyncTexture2D>();
			_eliteNames = new Dictionary<int, string>();
			_eliteIcons = new Dictionary<int, AsyncTexture2D>();
			_resources = Resources.Empty;
			_apiIcons = new Dictionary<int, AsyncTexture2D>();
			GameService.Overlay.add_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
		}

		public bool HasLoaded()
		{
			if (_resources.IsEmpty)
			{
				ScreenNotification.ShowNotification("Unavailable. Resources not yet loaded.", (NotificationType)2, (Texture2D)null, 4);
				return false;
			}
			return true;
		}

		public async Task LoadAsync(bool localeChange = false)
		{
			await LoadProfessions(localeChange);
			await LoadResources();
		}

		public string GetLoadingSubtitle()
		{
			return _loadingText[RandomUtil.GetRandom(0, _loadingText.Count - 1)];
		}

		public void PlayMenuItemClick()
		{
			_menuItemClickSfx.Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
		}

		public void PlayMenuClick()
		{
			_menuClicks[RandomUtil.GetRandom(0, 3)].Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
		}

		public void AddNewCoffers(Profile profile)
		{
			if (!_resources.IsEmpty && !profile.IsEmpty)
			{
				IEnumerable<Token> coffers2 = profile.Totals.Coffers;
				IEnumerable<Token> coffers = coffers2 ?? Enumerable.Empty<Token>();
				coffers2 = profile.Totals.Tokens;
				IEnumerable<Token> tokens = coffers2 ?? Enumerable.Empty<Token>();
				_resources.Coffers.AddRange(FetchNew(coffers.Concat(tokens)));
			}
		}

		private IEnumerable<Resource> FetchNew(IEnumerable<Token> tokens)
		{
			return from token in tokens
				where _resources.Items.All((Resource x) => x.Id != token.Id)
				select new Resource
				{
					Id = token.Id,
					Name = token.Name
				};
		}

		private void LoadSounds()
		{
			_menuItemClickSfx = ProofLogix.Instance.ContentsManager.GetSound("audio\\menu-item-click.wav");
			_menuClicks = new List<SoundEffect>
			{
				ProofLogix.Instance.ContentsManager.GetSound("audio\\menu-click-1.wav"),
				ProofLogix.Instance.ContentsManager.GetSound("audio\\menu-click-2.wav"),
				ProofLogix.Instance.ContentsManager.GetSound("audio\\menu-click-3.wav"),
				ProofLogix.Instance.ContentsManager.GetSound("audio\\menu-click-4.wav")
			};
		}

		private async Task LoadResources()
		{
			do
			{
				_resources = await ProofLogix.Instance.KpWebApi.GetResources();
				if (_resources.IsEmpty)
				{
					await Task.Delay(TimeSpan.FromSeconds(30.0));
				}
			}
			while (_resources.IsEmpty);
			_maps = await ProofLogix.Instance.Gw2WebApi.GetMaps(_resources.Wings.Select((Raid.Wing wing) => wing.MapId).ToArray());
			AddNewCoffers(await ProofLogix.Instance.KpWebApi.GetProfile("Nika"));
			await ExpandResources(_resources.Items);
		}

		public string GetClassName(int profession, int elite)
		{
			if (!_eliteNames.TryGetValue(elite, out var name))
			{
				if (!_profNames.TryGetValue(profession, out name))
				{
					return string.Empty;
				}
				return name;
			}
			return name;
		}

		public AsyncTexture2D GetClassIcon(int profession, int elite)
		{
			if (!_eliteIcons.TryGetValue(elite, out var icon))
			{
				if (!_profIcons.TryGetValue(profession, out icon))
				{
					return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
				}
				return icon;
			}
			return icon;
		}

		public async Task<AsyncTexture2D> GetApiIcon(int itemId)
		{
			if (itemId == 0)
			{
				return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
			}
			if (_apiIcons.TryGetValue(itemId, out var tex))
			{
				return tex;
			}
			IReadOnlyList<Item> response = await ProofLogix.Instance.Gw2WebApi.GetItems(itemId);
			if (response == null || !response.Any())
			{
				return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
			}
			Item item = response[0];
			if (string.IsNullOrEmpty(RenderUrl.op_Implicit(item.get_Icon())))
			{
				return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
			}
			int assetId = AssetUtil.GetId(RenderUrl.op_Implicit(item.get_Icon()));
			AsyncTexture2D icon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(assetId);
			_apiIcons.Add(itemId, icon);
			return icon;
		}

		public string GetMapName(int mapId)
		{
			Map obj = _maps.FirstOrDefault((Map map) => map.get_Id() == mapId);
			return ((obj != null) ? obj.get_Name() : null) ?? string.Empty;
		}

		public void Dispose()
		{
			GameService.Overlay.remove_UserLocaleChanged((EventHandler<ValueEventArgs<CultureInfo>>)OnUserLocaleChanged);
			_menuItemClickSfx.Dispose();
			foreach (SoundEffect menuClick in _menuClicks)
			{
				menuClick.Dispose();
			}
		}

		private async Task ExpandResources(IEnumerable<Resource> resources)
		{
			IReadOnlyList<Item> items = await ProofLogix.Instance.Gw2WebApi.GetItems(resources.Select((Resource resource) => resource.Id).ToArray());
			if (items == null || !items.Any())
			{
				return;
			}
			foreach (Item item in items)
			{
				Resource resource2 = _resources.Items.FirstOrDefault((Resource resource) => resource.Id == item.get_Id());
				if (resource2 != null)
				{
					resource2.Rarity = ApiEnum<ItemRarity>.op_Implicit(item.get_Rarity());
					resource2.Icon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(AssetUtil.GetId(RenderUrl.op_Implicit(item.get_Icon())));
				}
			}
		}

		private async void OnUserLocaleChanged(object sender, ValueEventArgs<CultureInfo> e)
		{
			await LoadAsync(localeChange: true);
		}

		private async Task LoadProfessions(bool localeChange = false)
		{
			IApiV2ObjectList<Profession> professions = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<Profession>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Professions()).AllAsync(default(CancellationToken)));
			if (professions == null)
			{
				return;
			}
			IApiV2ObjectList<Specialization> specializations = await TaskUtil.RetryAsync(() => ((IAllExpandableClient<Specialization>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
				.get_Specializations()).AllAsync(default(CancellationToken)));
			if (specializations == null)
			{
				return;
			}
			List<Specialization> elites = ((IEnumerable<Specialization>)specializations).Where((Specialization x) => x.get_Elite()).ToList();
			_profNames = ((IEnumerable<Profession>)professions).ToDictionary((Profession x) => (int)(ProfessionType)Enum.Parse(typeof(ProfessionType), x.get_Id(), ignoreCase: true), (Profession x) => x.get_Name());
			_eliteNames = elites.ToDictionary((Specialization x) => x.get_Id(), (Specialization x) => x.get_Name());
			if (!localeChange)
			{
				_profIcons = ((IEnumerable<Profession>)professions).ToDictionary((Profession x) => (int)(ProfessionType)Enum.Parse(typeof(ProfessionType), x.get_Id(), ignoreCase: true), (Profession x) => GameService.Content.get_DatAssetCache().GetTextureFromAssetId(AssetUtil.GetId(RenderUrl.op_Implicit(x.get_IconBig()))));
				_eliteIcons = elites.ToDictionary((Specialization x) => x.get_Id(), delegate(Specialization x)
				{
					//IL_001f: Unknown result type (might be due to invalid IL or missing references)
					DatAssetCache datAssetCache = GameService.Content.get_DatAssetCache();
					RenderUrl? professionIconBig = x.get_ProfessionIconBig();
					return datAssetCache.GetTextureFromAssetId(AssetUtil.GetId(professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null));
				});
			}
		}

		public Resource GetItem(int id)
		{
			return _resources.Items.FirstOrDefault((Resource item) => item.Id == id) ?? Resource.Empty;
		}

		public List<Resource> GetItems()
		{
			return _resources.Items.ToList();
		}

		public List<Resource> GetItemsForFractals()
		{
			IEnumerable<Resource> source;
			if (!_resources.IsEmpty)
			{
				IEnumerable<Resource> fractals = _resources.Fractals;
				source = fractals;
			}
			else
			{
				source = Enumerable.Empty<Resource>();
			}
			return source.ToList();
		}

		public List<Resource> GetGeneralItems()
		{
			IEnumerable<Resource> source;
			if (!_resources.IsEmpty)
			{
				IEnumerable<Resource> generalTokens = _resources.GeneralTokens;
				source = generalTokens;
			}
			else
			{
				source = Enumerable.Empty<Resource>();
			}
			return source.ToList();
		}

		public List<Resource> GetCofferItems()
		{
			IEnumerable<Resource> source;
			if (!_resources.IsEmpty)
			{
				IEnumerable<Resource> coffers = _resources.Coffers;
				source = coffers;
			}
			else
			{
				source = Enumerable.Empty<Resource>();
			}
			return source.ToList();
		}

		public List<Resource> GetItemsForMap(int mapId)
		{
			if (_resources.IsEmpty)
			{
				return Enumerable.Empty<Resource>().ToList();
			}
			return (from x in _resources.Raids.SelectMany((Raid x) => x.Wings)
				where x.MapId == mapId
				select x).SelectMany((Raid.Wing x) => x.Events).SelectMany((Raid.Wing.Event x) => x.GetTokens()).ToList();
		}

		public List<Raid.Wing> GetWings()
		{
			return _resources.Wings.ToList();
		}

		public List<Raid> GetRaids()
		{
			return _resources.Raids;
		}
	}
}
