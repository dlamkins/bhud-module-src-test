using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
using Ideka.NetCommon;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Ideka.CustomCombatText
{
	[Export(typeof(Module))]
	public class CTextModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<CTextModule>();

		private readonly DisposableCollection _dc = new DisposableCollection();

		private ModuleSettings _settings;

		private StyleSettings _style;

		private SkillData _skillData;

		private TraitData _traitData;

		private IReadOnlyDictionary<int, SkillFallback> _skillFallbacks;

		private ConfirmationModal _confirmationModal;

		private LocalData _localData;

		private IconBBoxes _iconBBoxes;

		private readonly FontAssets _fontAssets = new FontAssets();

		private ViewControl _viewControl;

		private MainPanel _mainPanel;

		private static CTextModule Instance { get; set; } = null;


		internal static string Name => ((Module)Instance).get_Name();

		internal static ContentsManager ContentsManager => ((Module)Instance).ModuleParameters.get_ContentsManager();

		internal static Gw2ApiManager Gw2ApiManager => ((Module)Instance).ModuleParameters.get_Gw2ApiManager();

		internal static string BasePath => ((Module)Instance).ModuleParameters.get_DirectoriesManager().GetFullDirectoryPath("customcombattext");

		internal static string CachePath => "Cache";

		internal static string SkillCachePath => Path.Combine(CachePath, "Skills.json");

		internal static string TraitCachePath => Path.Combine(CachePath, "Traits.json");

		internal static string FontPath => "Fonts";

		internal static string DefaultFontPath => Path.Combine("Fonts", "ITC Avant Garde Gothic Medium.ttf");

		internal static string SkillFallbacksPath => "SkillFallbacks.json";

		internal static string StylePath => "Style.json";

		internal static string ViewsDataPath => "Views.json";

		internal static ModuleSettings Settings => Instance._settings;

		internal static StyleSettings Style => Instance._style;

		internal static SkillData SkillData => Instance._skillData;

		internal static TraitData TraitData => Instance._traitData;

		internal static IReadOnlyDictionary<int, SkillFallback> SkillFallbacks => Instance._skillFallbacks;

		internal static ConfirmationModal ConfirmationModal => Instance._confirmationModal;

		internal static LocalData LocalData => Instance._localData;

		internal static IconBBoxes IconBBoxes => Instance._iconBBoxes;

		internal static FontAssets FontAssets => Instance._fontAssets;

		internal static AreaView? Selected => Instance._mainPanel.Selected;

		[ImportingConstructor]
		public CTextModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(base.ModuleParameters.get_SettingsManager().get_ModuleSettings());
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

		protected override async Task LoadAsync()
		{
			_skillData = _dc.Add(new SkillData());
			await _skillData.StartLoad(Path.Combine(BasePath, SkillCachePath), ContentsManager, SkillCachePath);
			_traitData = _dc.Add(new TraitData());
			await _traitData.StartLoad(Path.Combine(BasePath, TraitCachePath), ContentsManager, TraitCachePath);
			Dictionary<int, SkillFallback> read = JsonConvert.DeserializeObject<Dictionary<int, SkillFallback>>(ExtractAndRead(SkillFallbacksPath));
			if (read == null)
			{
				Logger.Warn("Failed to load Skill Fallbacks.");
			}
			_skillFallbacks = read ?? new Dictionary<int, SkillFallback>();
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
			GameService.ArcDps.add_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDpsEvent);
			_mainPanel = _dc.Add<MainPanel>(new MainPanel());
			LocalData.ReloadViews();
		}

		private void ArcDpsEvent(object sender, RawCombatEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.get_EventType() != 1)
			{
				Ev ev = e.get_CombatEvent().get_Ev();
				if (((ev != null) ? new byte?(ev.get_Result()) : null) != 10)
				{
					return;
				}
			}
			foreach (Message message in Message.Interpret(e.get_CombatEvent()))
			{
				_viewControl.ReceiveMessage(message);
			}
		}

		protected override void Unload()
		{
			GameService.ArcDps.remove_RawCombatEvent((EventHandler<RawCombatEventArgs>)ArcDpsEvent);
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
