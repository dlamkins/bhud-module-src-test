using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Tortle.PlayerMarker.Entity;
using Tortle.PlayerMarker.Models;
using Tortle.PlayerMarker.Services;
using Tortle.PlayerMarker.Util;
using Tortle.PlayerMarker.Views;

namespace Tortle.PlayerMarker
{
	[Export(typeof(Module))]
	public class PlayerMarkerModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<PlayerMarkerModule>();

		private readonly Tortle.PlayerMarker.Entity.PlayerMarker _playerMarker;

		private readonly TextureCache _textureCache;

		private readonly ModuleSettings _moduleSettings;

		private readonly SettingsView _settingsView;

		[ImportingConstructor]
		public PlayerMarkerModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ContentsManager contentsManager = base.ModuleParameters.get_ContentsManager();
			DirectoriesManager directoriesManager = base.ModuleParameters.get_DirectoriesManager();
			_playerMarker = new Tortle.PlayerMarker.Entity.PlayerMarker();
			_textureCache = new TextureCache(directoriesManager, contentsManager);
			_moduleSettings = new ModuleSettings(_textureCache, _playerMarker);
			_settingsView = new SettingsView(_textureCache, _moduleSettings, contentsManager);
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			Logger.Info("Initializing settings");
			_moduleSettings.Initialize(settings);
		}

		public override IView GetSettingsView()
		{
			Logger.Info("Returning settings view");
			return (IView)(object)_settingsView;
		}

		protected override async Task LoadAsync()
		{
			Logger.Info("Loading module");
			await _textureCache.Load(_moduleSettings.DefaultMarkerFileNames);
			NormalizeSettings();
			_textureCache.Get(_moduleSettings.ImageName.get_Value());
			_settingsView.LoadTextures();
			GameService.Graphics.get_World().AddEntity((IEntity)(object)_playerMarker);
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			float diameterPx = _moduleSettings.Size.get_Value();
			_playerMarker.Visible = _moduleSettings.Enabled.get_Value();
			_playerMarker.MarkerColor = ConversionUtil.ToRgb(_moduleSettings.Color.get_Value());
			_playerMarker.MarkerOpacity = _moduleSettings.Opacity.get_Value();
			_playerMarker.MarkerTexture = _textureCache.Get(_moduleSettings.ImageName.get_Value());
			_playerMarker.Size = new Vector3(diameterPx, diameterPx, 0f);
			_playerMarker.VerticalOffset = _moduleSettings.VerticalOffset.get_Value();
			Logger.Info("Marker properties set");
			_playerMarker.UpdateMarker();
			Logger.Info("Finished loading module");
			((Module)this).OnModuleLoaded(e);
		}

		protected override void Unload()
		{
			_moduleSettings.Dispose();
			_textureCache.Dispose();
			_settingsView.Dispose();
			GameService.Graphics.get_World().RemoveEntity((IEntity)(object)_playerMarker);
		}

		private void NormalizeSettings()
		{
			if (!_textureCache.ContainsKey(_moduleSettings.ImageName.get_Value()))
			{
				Logger.Warn("Resetting {setting} setting back to default", new object[1] { "ImageName" });
				_moduleSettings.ImageName.set_Value(_moduleSettings.DefaultMarkerFileNames[0]);
			}
			if (!ColorPresets.Colors.ContainsKey(_moduleSettings.Color.get_Value()))
			{
				Logger.Warn("Resetting {setting} setting back to default", new object[1] { "Color" });
				_moduleSettings.Color.set_Value("White");
			}
		}
	}
}
