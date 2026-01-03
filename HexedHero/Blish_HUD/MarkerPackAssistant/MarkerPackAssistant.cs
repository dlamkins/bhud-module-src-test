using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using HexedHero.Blish_HUD.MarkerPackAssistant.Managers;

namespace HexedHero.Blish_HUD.MarkerPackAssistant
{
	[Export(typeof(Module))]
	public class MarkerPackAssistant : Module
	{
		private SettingCollection _settingsCollection;

		public readonly Logger Logger = Logger.GetLogger(typeof(MarkerPackAssistant));

		public static MarkerPackAssistant Instance { get; private set; }

		public ModuleParameters Module { get; private set; }

		[ImportingConstructor]
		public MarkerPackAssistant([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Module = moduleParameters;
			Instance = this;
		}

		protected override void Initialize()
		{
			_ = WindowManager.Instance;
			_ = ModuleSettingsManager.Instance;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settingsCollection = settings;
			ModuleSettingsManager.Instance.DefineSettings(settings);
		}

		protected override void Unload()
		{
			WindowManager.Instance.Unload();
			ModuleSettingsManager.Instance.Unload();
			Instance = null;
		}

		public override IView GetSettingsView()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			return (IView)new SettingsView((SettingCollection)(((object)_settingsCollection) ?? ((object)new SettingCollection(false))), -1);
		}
	}
}
