using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Extended;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Audio;
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

		private Dictionary<int, AsyncTexture2D> _apiIcons;

		private IReadOnlyList<SoundEffect> _menuClicks;

		private SoundEffect _menuItemClickSfx;

		private readonly IReadOnlyList<string> _loadingText = new List<string>
		{
			"Turning Vault upside down...", "Borrowing wallet...", "Tickling characters...", "High-fiving Deimos...", "Checking on Dhuum's cage...", "Throwing rocks into Mystic Forge...", "Lock-picking Ahdashim...", "Mounting Gorseval...", "Knitting Xera's ribbon...", "Caring for the bees...",
			"Dismantling White Mantle...", "Chasing Skritt...", "Ransacking bags...", "Poking Saul...", "Commanding golems...", "Polishing monocle...", "Running in circles...", "Scratching Slothasor...", "Cleaning Kitty Golem...", "Making sense of inventory...",
			"Pleading for Glenna's assistance...", "Counting achievements...", "Blowing away dust...", "Calling upon spirits...", "Consulting Order of Shadows...", "Bribing Pact troops...", "Bribing Bankers..."
		};

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

		public void AddNewResources(Profile profile)
		{
			if (_resources.IsEmpty || profile.IsEmpty)
			{
				return;
			}
			IEnumerable<Token> coffers = profile.Totals.Coffers;
			foreach (Token token3 in (coffers ?? Enumerable.Empty<Token>()).Where((Token token) => _resources.Items.All((Resource x) => x.Id != token.Id)))
			{
				_resources.Coffers.Add(new Resource
				{
					Id = token3.Id,
					Name = token3.Name
				});
			}
			foreach (Token token2 in from token in profile.Totals.GetTokens(excludeCoffers: true)
				where _resources.Items.All((Resource x) => x.Id != token.Id)
				select token)
			{
				_resources.GeneralTokens.Add(new Resource
				{
					Id = token2.Id,
					Name = token2.Name
				});
			}
		}

		public async Task LoadAsync(bool localeChange = false)
		{
			await LoadProfessions(localeChange);
			await LoadResources();
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
			_resources = await ProofLogix.Instance.KpWebApi.GetResources();
			foreach (Raid.Wing wing in _resources.Wings)
			{
				Raid.Wing wing2 = wing;
				wing2.Name = await GetMapName(wing.MapId);
			}
			AddNewResources(await ProofLogix.Instance.KpWebApi.GetProfile("Nika"));
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

		public AsyncTexture2D GetApiIcon(int itemId)
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			if (itemId == 0)
			{
				return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
			}
			if (_apiIcons.TryGetValue(itemId, out var tex))
			{
				return tex;
			}
			Item response = null;
			try
			{
				response = ((IBulkExpandableClient<Item, int>)(object)GameService.Gw2WebApi.get_AnonymousConnection().get_Client().get_V2()
					.get_Items()).GetAsync(itemId, default(CancellationToken)).Result;
			}
			catch (Exception e)
			{
				if (!(e is NotFoundException) && !(e is BadRequestException) && !(e is AuthorizationRequiredException))
				{
					if (!(e is TooManyRequestsException))
					{
						if (e is RequestException || e is RequestException<string>)
						{
							ProofLogix.Logger.Trace(e, e.Message);
						}
						else
						{
							ProofLogix.Logger.Error(e, e.Message);
						}
					}
					else
					{
						ProofLogix.Logger.Warn(e, "No icon could be loaded due to being rate limited by the API.");
					}
				}
				else
				{
					ProofLogix.Logger.Trace(e, e.Message);
				}
			}
			if (response == null || string.IsNullOrEmpty(RenderUrl.op_Implicit(response.get_Icon())))
			{
				return AsyncTexture2D.op_Implicit(Textures.get_TransparentPixel());
			}
			int assetId = AssetUtil.GetId(RenderUrl.op_Implicit(response.get_Icon()));
			AsyncTexture2D icon = GameService.Content.get_DatAssetCache().GetTextureFromAssetId(assetId);
			_apiIcons.Add(itemId, icon);
			return icon;
		}

		public async Task<string> GetMapName(int mapId)
		{
			Map obj = await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Map, int>)(object)ProofLogix.Instance.Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Maps()).GetAsync(mapId, default(CancellationToken)));
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
				_profIcons = ((IEnumerable<Profession>)professions).ToDictionary((Profession x) => (int)(ProfessionType)Enum.Parse(typeof(ProfessionType), x.get_Id(), ignoreCase: true), delegate(Profession x)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					ContentService content = GameService.Content;
					RenderUrl iconBig = x.get_IconBig();
					return content.GetRenderServiceTexture(((object)(RenderUrl)(ref iconBig)).ToString());
				});
				_eliteIcons = elites.ToDictionary((Specialization x) => x.get_Id(), (Specialization x) => GameService.Content.GetRenderServiceTexture(x.get_ProfessionIconBig().ToString()));
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

		public List<Resource> GetGeneralItems(bool includeOld = false)
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

		public List<Resource> GetCofferItems(bool includeOld = false)
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
