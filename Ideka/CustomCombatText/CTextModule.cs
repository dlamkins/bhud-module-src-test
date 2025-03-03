using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Ideka.BHUDCommon;
using Ideka.CustomCombatText.Bridge;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	[Export(typeof(Module))]
	public class CTextModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<CTextModule>();

		private readonly DisposableCollection _dc = new DisposableCollection();

		private HttpClient _httpClient;

		private ModuleSettings _settings;

		private StyleSettings _style;

		private SkillData _skillData;

		private TraitData _traitData;

		private SpecializationData _specData;

		private HsSkillData _hsSkillData;

		private HsPaletteData _hsPaletteData;

		private ConfirmationModal _confirmationModal;

		private LocalData _localData;

		private IconBBoxes _iconBBoxes;

		private readonly FontAssets _fontAssets = new FontAssets();

		private ViewControl _viewControl;

		private PanelStack _panelStack;

		private readonly FixedSizedQueue<LogEntry> _log = new FixedSizedQueue<LogEntry>();

		private BridgeService _bridgeService;

		private SettingsView? _settingsView;

		private static CTextModule Instance { get; set; } = null;


		internal static string Name => ((Module)Instance).get_Name();

		internal static ContentsManager ContentsManager => ((Module)Instance).ModuleParameters.get_ContentsManager();

		internal static Gw2ApiManager Gw2ApiManager => ((Module)Instance).ModuleParameters.get_Gw2ApiManager();

		internal static string BasePath => ((Module)Instance).ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("customcombattext");

		internal static string CachePath => "Cache";

		internal static string SkillCachePath => Path.Combine(CachePath, "Skills.json");

		internal static string TraitCachePath => Path.Combine(CachePath, "Traits.json");

		internal static string SpecCachePath => Path.Combine(CachePath, "Specs.json");

		internal static string HsSkillCachePath => Path.Combine(CachePath, "HsSkills.json");

		internal static string HsPaletteCachePath => Path.Combine(CachePath, "HsPalettes.json");

		internal static string FontPath => "Fonts";

		internal static string DefaultFontPath => Path.Combine("Fonts", "ITC Avant Garde Gothic Medium.ttf");

		internal static string StylePath => "Style.json";

		internal static string ViewsDataPath => "Views.json";

		internal static HttpClient HttpClient => Instance._httpClient;

		internal static ModuleSettings Settings => Instance._settings;

		internal static StyleSettings Style => Instance._style;

		internal static SkillData SkillData => Instance._skillData;

		internal static TraitData TraitData => Instance._traitData;

		internal static SpecializationData SpecData => Instance._specData;

		internal static HsSkillData HsSkillData => Instance._hsSkillData;

		internal static HsPaletteData HsPaletteData => Instance._hsPaletteData;

		internal static ConfirmationModal ConfirmationModal => Instance._confirmationModal;

		internal static LocalData LocalData => Instance._localData;

		internal static IconBBoxes IconBBoxes => Instance._iconBBoxes;

		internal static FontAssets FontAssets => Instance._fontAssets;

		internal static AreaView? Selected
		{
			get
			{
				AreasPanel areasPanel = Instance._panelStack.CurrentPanel as AreasPanel;
				if (areasPanel == null || !((Control)(object)areasPanel).IsVisible())
				{
					return null;
				}
				return areasPanel.Selected;
			}
		}

		internal static FixedSizedQueue<LogEntry> Log => Instance._log;

		internal static event Action<LogEntry>? EntryLogged;

		[ImportingConstructor]
		public CTextModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		public static string ExtractAndRead(string builtinPath)
		{
			string outer = Path.Combine(BasePath, builtinPath);
			if (!File.Exists(outer))
			{
				using Stream file = ContentsManager.GetFileStream(builtinPath);
				using StreamReader reader = new StreamReader(file);
				Directory.CreateDirectory(Path.GetDirectoryName(outer));
				File.WriteAllText(outer, reader.ReadToEnd());
			}
			return File.ReadAllText(outer);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = _dc.Add(new ModuleSettings(settings));
			_style = _dc.Add(new StyleSettings(settings));
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)(_settingsView = new SettingsView(base.ModuleParameters.get_SettingsManager().get_ModuleSettings()));
		}

		protected override void Initialize()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			((Module)this).Initialize();
			_httpClient = _dc.Add<HttpClient>(new HttpClient());
			CancellationTokenSource cts = new CancellationTokenSource();
			_dc.Add(new WhenDisposed(new Action(cts.Cancel)));
			_skillData = _dc.Add(new SkillData());
			_traitData = _dc.Add(new TraitData());
			_specData = _dc.Add(new SpecializationData());
			_hsSkillData = _dc.Add(new HsSkillData(_httpClient));
			_hsPaletteData = _dc.Add(new HsPaletteData(_httpClient));
			Task.Run(async delegate
			{
				await Task.WhenAll<Task>(_skillData.StartLoad(Path.Combine(BasePath, SkillCachePath), ContentsManager, SkillCachePath, cts.Token), _traitData.StartLoad(Path.Combine(BasePath, TraitCachePath), ContentsManager, TraitCachePath, cts.Token), _specData.StartLoad(Path.Combine(BasePath, SpecCachePath), ContentsManager, SpecCachePath, cts.Token), _hsSkillData.StartLoad(Path.Combine(BasePath, HsSkillCachePath), ContentsManager, HsSkillCachePath, cts.Token), _hsPaletteData.StartLoad(Path.Combine(BasePath, HsPaletteCachePath), ContentsManager, HsPaletteCachePath, cts.Token));
			}, cts.Token);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			((Module)this).OnModuleLoaded(e);
			_confirmationModal = _dc.Add<ConfirmationModal>(new ConfirmationModal(AsyncTexture2D.op_Implicit(_dc.Add<Texture2D>(ContentsManager.GetTexture("Tooltip.png")))));
			_localData = new LocalData();
			_iconBBoxes = _dc.Add(new IconBBoxes());
			DisposableCollection dc = _dc;
			ViewControl viewControl = new ViewControl(_localData.AreaViewParent);
			((Control)viewControl).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)viewControl).set_ZIndex(50);
			_viewControl = dc.Add<ViewControl>(viewControl);
			_panelStack = _dc.Add<PanelStack>(new PanelStack(GameService.Overlay.get_BlishHudWindow(), Name.GetHashCode(), (PanelStack panelStack) => new AreasPanel(panelStack)));
			LocalData.ReloadViews();
			_dc.Add(_settings.MessageLogLength.OnChangedAndNow(delegate(int x)
			{
				_log.Size = x;
			}));
			_bridgeService = _dc.Add(new BridgeService());
			_bridgeService.RawCombatEvent += new BridgeService.CombatEventDelegate(ArcDpsEvent);
		}

		protected override void Update(GameTime gameTime)
		{
			((Module)this).Update(gameTime);
			if (_settingsView != null)
			{
				_settingsView!.Status = "Bridge Status: " + (_bridgeService.IsActive ? "active" : "inactive") + "\n" + $"Restarts: {_bridgeService.Loops}";
			}
		}

		private void ArcDpsEvent(ArraySegment<byte> data, CombatEventType type, CombatEvent cbt)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			if ((int)type != 1)
			{
				Ev ev = cbt.get_Ev();
				if (((ev != null) ? new byte?(ev.get_Result()) : null) != 10)
				{
					return;
				}
			}
			IEnumerable<Message> enumerable = MessageContext.Interpret(cbt);
			if (enumerable.Any())
			{
				byte[] buffer = new byte[data.Count];
				Array.Copy(data.Array, data.Offset, buffer, 0, data.Count);
				LogEntry entry = MessageContext.Log(buffer);
				_log.Enqueue(entry);
				CTextModule.EntryLogged?.Invoke(entry);
			}
			foreach (Message message in enumerable)
			{
				_viewControl.ReceiveMessage(message);
			}
		}

		protected override void Unload()
		{
			try
			{
				_dc.Dispose();
			}
			finally
			{
				Instance = null;
			}
		}
	}
}
