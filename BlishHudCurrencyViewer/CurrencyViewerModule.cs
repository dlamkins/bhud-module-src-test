using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BlishHudCurrencyViewer.Models;
using BlishHudCurrencyViewer.Services;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace BlishHudCurrencyViewer
{
	[Export(typeof(Module))]
	public class CurrencyViewerModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<CurrencyViewerModule>();

		internal ApiPollingService PollingService;

		internal WindowService WindowService;

		internal CurrencyService CurrencyService;

		internal static CurrencyViewerModule ModuleInstance;

		private CornerIcon _cornerIcon;

		internal SettingsManager SettingsManager => base.ModuleParameters.get_SettingsManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		[ImportingConstructor]
		public CurrencyViewerModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			ModuleInstance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
		}

		private void OnApiSubTokenUpdated(object sender, ValueEventArgs<IEnumerable<TokenPermission>> e)
		{
			PollingService?.Invoke();
		}

		protected override async Task LoadAsync()
		{
			try
			{
				WindowService = new WindowService(ContentsManager, SettingsManager);
				CurrencyService = new CurrencyService(Gw2ApiManager, SettingsManager, Logger);
				WindowService.InitializeIfNotExists();
				await CurrencyService.InitializeCurrencySettings();
				PollingService = new ApiPollingService();
				PollingService.ApiPollingTrigger += delegate
				{
					Task.Run(delegate
					{
						RefreshWindow();
					});
				};
			}
			catch (Exception e)
			{
				Logger.Warn(e, e.Message);
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			((Module)this).OnModuleLoaded(e);
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("coins.png")));
			((Control)val).set_BasicTooltipText(((Module)this).get_Name() ?? "");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				WindowService.Toggle();
			});
			RefreshWindow();
		}

		protected override void Update(GameTime gameTime)
		{
			WindowService.Update(gameTime);
			PollingService?.Update(gameTime);
			if (CurrencyService.Update(gameTime))
			{
				RefreshWindow();
			}
		}

		protected async void RefreshWindow()
		{
			List<UserCurrency> userCurrencies = await CurrencyService.GetUserCurrencies();
			WindowService.RedrawWindowContent(userCurrencies);
		}

		protected override void Unload()
		{
			Gw2ApiManager.remove_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)OnApiSubTokenUpdated);
			PollingService?.Dispose();
			WindowService?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			ModuleInstance = null;
		}
	}
}
