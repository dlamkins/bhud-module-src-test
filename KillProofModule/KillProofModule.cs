using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using KillProofModule.Controls;
using KillProofModule.Controls.Views;
using KillProofModule.Manager;
using KillProofModule.Models;
using Microsoft.Xna.Framework.Graphics;

namespace KillProofModule
{
	[Export(typeof(Module))]
	public class KillProofModule : Module
	{
		internal static readonly Logger Logger = Logger.GetLogger(typeof(KillProofModule));

		internal static KillProofModule ModuleInstance;

		internal SettingEntry<bool> SmartPingMenuEnabled;

		internal SettingEntry<bool> AutomaticClearEnabled;

		internal SettingEntry<string> SPM_DropdownSelection;

		internal SettingEntry<string> SPM_WingSelection;

		internal SettingEntry<int> SPM_Repetitions;

		private Dictionary<int, AsyncTexture2D> _professionRenderRepository;

		private Dictionary<int, AsyncTexture2D> _eliteRenderRepository;

		private Dictionary<int, AsyncTexture2D> _tokenRenderRepository;

		private Texture2D _killProofIconTexture;

		internal string KillProofTabName = "KillProof";

		internal Resources Resources;

		internal PartyManager PartyManager;

		internal SmartPingMenu SmartPingMenu;

		private WindowTab _moduleTab;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public KillProofModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			SPM_Repetitions = settings.DefineSetting<int>("SmartPingRepetitions", 10, "Smart Ping Repetitions", "Indicates how often a value should be repeated before proceeding to the next reduction.", (SettingTypeRendererDelegate)null);
			SettingCollection selfManagedSettings = settings.AddSubCollection("Managed Settings", false, false);
			SmartPingMenuEnabled = selfManagedSettings.DefineSetting<bool>("SmartPingMenuEnabled", false, (Func<string>)null, (Func<string>)null);
			AutomaticClearEnabled = selfManagedSettings.DefineSetting<bool>("AutomaticClearEnabled", false, (Func<string>)null, (Func<string>)null);
			SPM_DropdownSelection = selfManagedSettings.DefineSetting<string>("SmartPingMenuDropdownSelection", "", (Func<string>)null, (Func<string>)null);
			SPM_WingSelection = selfManagedSettings.DefineSetting<string>("SmartPingMenuWingSelection", "W1", (Func<string>)null, (Func<string>)null);
		}

		protected override void Initialize()
		{
			_tokenRenderRepository = new Dictionary<int, AsyncTexture2D>();
			_eliteRenderRepository = new Dictionary<int, AsyncTexture2D>();
			_professionRenderRepository = new Dictionary<int, AsyncTexture2D>();
			_killProofIconTexture = ContentsManager.GetTexture("killproof_icon.png");
		}

		protected override async Task LoadAsync()
		{
			Resources = await KillProofApi.LoadResources();
			await LoadTokenIcons();
			await LoadProfessionIcons();
			await LoadEliteIcons();
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			PartyManager = new PartyManager();
			SmartPingMenu = new SmartPingMenu();
			_moduleTab = GameService.Overlay.get_BlishHudWindow().AddTab(KillProofTabName, AsyncTexture2D.op_Implicit(_killProofIconTexture), (Func<IView>)(() => (IView)(object)new MainView()), 0);
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Unload()
		{
			GameService.Overlay.get_BlishHudWindow().RemoveTab(_moduleTab);
			SmartPingMenu?.Dispose();
			ModuleInstance = null;
		}

		private async Task LoadTokenIcons()
		{
			IEnumerable<Token> tokenRenderUrlRepository = Resources.GetAllTokens();
			foreach (Token token in tokenRenderUrlRepository)
			{
				_tokenRenderRepository.Add(token.Id, new AsyncTexture2D());
				string renderUri = token.Icon;
				await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToByteArrayAsync(renderUri, default(CancellationToken))
					.ContinueWith(delegate(Task<byte[]> textureDataResponse)
					{
						if (textureDataResponse.Exception != null)
						{
							Logger.Warn((Exception)textureDataResponse.Exception, "Request to render service for " + renderUri + " failed.");
						}
						else
						{
							using MemoryStream memoryStream = new MemoryStream(textureDataResponse.Result);
							Texture2D val = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
							_tokenRenderRepository[token.Id].SwapTexture(val);
						}
					});
			}
		}

		private async Task<IReadOnlyList<Profession>> LoadProfessions()
		{
			try
			{
				return await ((IBulkAliasExpandableClient<Profession, ProfessionType>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).ManyAsync(Enum.GetValues(typeof(ProfessionType)).Cast<ProfessionType>(), default(CancellationToken));
			}
			catch (Exception ex) when (ex is ServiceUnavailableException || ex is UnexpectedStatusException)
			{
				Logger.Warn(CommonStrings.WebApiDown);
				return null;
			}
		}

		private async Task LoadProfessionIcons()
		{
			IReadOnlyList<Profession> professions = await LoadProfessions();
			if (professions == null)
			{
				return;
			}
			foreach (Profession profession in professions)
			{
				int id = (int)Enum.GetValues(typeof(ProfessionType)).Cast<ProfessionType>().ToList()
					.Find((ProfessionType x) => ((object)(ProfessionType)(ref x)).ToString().Equals(profession.get_Id(), StringComparison.InvariantCultureIgnoreCase));
				_professionRenderRepository.Add(id, new AsyncTexture2D());
				string renderUri = RenderUrl.op_Implicit(profession.get_IconBig());
				await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToByteArrayAsync(renderUri, default(CancellationToken))
					.ContinueWith(delegate(Task<byte[]> textureDataResponse)
					{
						if (textureDataResponse.Exception != null)
						{
							Logger.Warn((Exception)textureDataResponse.Exception, "Request to render service for " + renderUri + " failed.");
						}
						else
						{
							using MemoryStream memoryStream = new MemoryStream(textureDataResponse.Result);
							Texture2D val = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
							_professionRenderRepository[id].SwapTexture(val);
						}
					});
			}
		}

		private async Task LoadEliteIcons()
		{
			IReadOnlyList<Specialization> specializations;
			try
			{
				IApiV2ObjectList<int> ids = await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).IdsAsync(default(CancellationToken));
				specializations = await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).ManyAsync((IEnumerable<int>)ids, default(CancellationToken));
			}
			catch (Exception ex) when (ex is ServiceUnavailableException || ex is UnexpectedStatusException)
			{
				Logger.Warn(CommonStrings.WebApiDown);
				return;
			}
			foreach (Specialization specialization in specializations)
			{
				if (!specialization.get_Elite())
				{
					continue;
				}
				_eliteRenderRepository.Add(specialization.get_Id(), new AsyncTexture2D());
				RenderUrl? professionIconBig = specialization.get_ProfessionIconBig();
				string renderUri = (professionIconBig.HasValue ? RenderUrl.op_Implicit(professionIconBig.GetValueOrDefault()) : null);
				await Gw2ApiManager.get_Gw2ApiClient().get_Render().DownloadToByteArrayAsync(renderUri, default(CancellationToken))
					.ContinueWith(delegate(Task<byte[]> textureDataResponse)
					{
						if (textureDataResponse.Exception != null)
						{
							Logger.Warn((Exception)textureDataResponse.Exception, "Request to render service for " + renderUri + " failed.");
						}
						else
						{
							using MemoryStream memoryStream = new MemoryStream(textureDataResponse.Result);
							Texture2D val = Texture2D.FromStream(GameService.Graphics.get_GraphicsDevice(), (Stream)memoryStream);
							_eliteRenderRepository[specialization.get_Id()].SwapTexture(val);
						}
					});
			}
		}

		public AsyncTexture2D GetProfessionRender(Player player)
		{
			if (((Player)(ref player)).get_Elite() != 0)
			{
				return _eliteRenderRepository[(int)((Player)(ref player)).get_Elite()];
			}
			if (((Player)(ref player)).get_Profession() != 0)
			{
				return _professionRenderRepository[(int)((Player)(ref player)).get_Profession()];
			}
			return AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("common/733268"));
		}

		public AsyncTexture2D GetTokenRender(int key)
		{
			if (_tokenRenderRepository.ContainsKey(key))
			{
				return _tokenRenderRepository[key];
			}
			return AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("deleted_item"));
		}
	}
}
