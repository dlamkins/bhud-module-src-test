using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using LoreBridge.Controls;
using LoreBridge.Models;
using LoreBridge.Modules;
using LoreBridge.Modules.Area;
using LoreBridge.Modules.Chat;
using LoreBridge.Resources;
using LoreBridge.Services;
using LoreBridge.Utils;
using LoreBridge.Views.SettingsView;
using Microsoft.Xna.Framework;

namespace LoreBridge
{
	[Export(typeof(Module))]
	public class LoreBridge : Module
	{
		private readonly List<Module> _modules = new List<Module>();

		private CornerIcon _cornerIcon;

		private Settings _settings;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public LoreBridge([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_settings = new Settings(settings);
		}

		public override IView GetSettingsView()
		{
			return (IView)(object)new SettingsView(_settings);
		}

		protected override async Task LoadAsync()
		{
			if (!_settings.ContentUsePolicyConfirmation.get_Value())
			{
				if (await PolicyConfirmation.ShowPolicyConfirmationAsync())
				{
					_settings.ContentUsePolicyConfirmation.set_Value(true);
				}
				else
				{
					_settings.ContentUsePolicyConfirmation.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PolicyValueChanged);
				}
			}
			Init();
		}

		protected override void Update(GameTime gameTime)
		{
			if (!_settings.ContentUsePolicyConfirmation.get_Value())
			{
				return;
			}
			Service[] all = Service.All;
			for (int i = 0; i < all.Length; i++)
			{
				all[i].Update(gameTime);
			}
			foreach (Module module in _modules)
			{
				module.Update(gameTime);
			}
		}

		protected override void Unload()
		{
			if (!_settings.ContentUsePolicyConfirmation.get_Value())
			{
				_settings.ContentUsePolicyConfirmation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PolicyValueChanged);
				return;
			}
			foreach (Module module in _modules)
			{
				module.Unload();
			}
			Service[] all = Service.All;
			for (int i = 0; i < all.Length; i++)
			{
				all[i].Unload();
			}
			((Control)_cornerIcon).Dispose();
			Textures.Dispose();
			Fonts.Dispose();
		}

		private void Init()
		{
			if (_settings.ContentUsePolicyConfirmation.get_Value())
			{
				Fonts.Initialize(ContentsManager);
				Textures.Initialize(ContentsManager);
				_cornerIcon = new CornerIcon();
				LoadServices();
				LoadModules();
			}
		}

		private void LoadModules()
		{
			_modules.Add(new Area());
			_modules.Add(new Chat(_cornerIcon));
			foreach (Module module in _modules)
			{
				module.Load(_settings);
			}
		}

		private void LoadServices()
		{
			Service[] all = Service.All;
			for (int i = 0; i < all.Length; i++)
			{
				all[i].Load(_settings);
			}
		}

		private void PolicyValueChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				Init();
				_settings.ContentUsePolicyConfirmation.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)PolicyValueChanged);
			}
		}
	}
}
