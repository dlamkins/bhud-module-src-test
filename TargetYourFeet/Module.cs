using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using TargetYourFeet.Features.TargetYourFeet.Services;
using TargetYourFeet.Settings.Services;
using TargetYourFeet.Settings.Views;

namespace TargetYourFeet
{
	[Export(typeof(Module))]
	public class Module : Module
	{
		internal static readonly Logger ModuleLogger = Logger.GetLogger<Module>();

		protected SettingService _settings;

		protected MouseMoveService _mouseMoveService;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Service.ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = new SettingService(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new ModuleMainSettingsView(_settings);
		}

		protected override Task LoadAsync()
		{
			_mouseMoveService = new MouseMoveService(_settings);
			return Task.CompletedTask;
		}

		protected override void Unload()
		{
			_mouseMoveService?.Dispose();
		}

		protected override void Update(GameTime gameTime)
		{
		}
	}
}
