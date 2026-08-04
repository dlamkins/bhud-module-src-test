using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Oberyn.AnglerAssociate.Controls;
using Oberyn.AnglerAssociate.Services;

namespace Oberyn.AnglerAssociate
{
	[Export(typeof(Module))]
	public class AnglerAssociateModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger(typeof(AnglerAssociateModule));

		private CornerIcon _cornerIcon;

		private StandardWindow _mainWindow;

		private MainView _mainView;

		private Timer _bannerRefreshTimer;

		private const int ContentWidth = 1052;

		private const int ContentHeight = 568;

		internal static AnglerAssociateModule Instance { get; private set; }

		internal Gw2ApiManager Gw2ApiManager => base.ModuleParameters.get_Gw2ApiManager();

		internal ContentsManager ContentsManager => base.ModuleParameters.get_ContentsManager();

		internal DirectoriesManager DirectoriesManager => base.ModuleParameters.get_DirectoriesManager();

		internal AchievementProgressService AchievementProgress { get; private set; }

		[ImportingConstructor]
		public AnglerAssociateModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: this(moduleParameters)
		{
			Instance = this;
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
			try
			{
				Logger.Info("Angler Associate initializing.");
				AchievementProgress = new AchievementProgressService();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Angler Associate failed during Initialize().");
				throw;
			}
		}

		protected override async Task LoadAsync()
		{
			try
			{
				Logger.Info("Angler Associate loaded.");
				BuildWindow();
				BuildContent();
				BuildCornerIcon();
				_bannerRefreshTimer = new Timer(delegate
				{
					GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate
					{
						_mainView.RefreshBanners();
					});
				}, null, TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0));
				Gw2ApiManager.add_SubtokenUpdated((EventHandler<ValueEventArgs<IEnumerable<TokenPermission>>>)async delegate
				{
					await AchievementProgress.RefreshAsync();
				});
				await AchievementProgress.RefreshAsync();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Angler Associate failed during LoadAsync().");
				throw;
			}
		}

		private void BuildWindow()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			Texture2D windowBackground = ContentsManager.GetTexture("icons/background2.png");
			StandardWindow val = new StandardWindow(windowBackground, new Rectangle(0, 0, 1172, 667), new Rectangle(60, 50, 1052, 568));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title("Angler Associate");
			((Control)val).set_Location(new Point(50, 50));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id("FishingBuddy_MainWindow");
			_mainWindow = val;
		}

		private void BuildContent()
		{
			MainView mainView = new MainView(ContentsManager, AchievementProgress, 568);
			((Control)mainView).set_Parent((Container)(object)_mainWindow);
			((Control)mainView).set_Width(1052);
			((Control)mainView).set_Height(568);
			_mainView = mainView;
		}

		private void BuildCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("icons/icon.png")));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(ContentsManager.GetTexture("icons/icon_hover.png")));
			((Control)val).set_BasicTooltipText("Angler Associate");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (((Control)_mainWindow).get_Visible())
				{
					((Control)_mainWindow).Hide();
				}
				else
				{
					((Control)_mainWindow).Show();
				}
			});
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			_bannerRefreshTimer?.Dispose();
			CornerIcon cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				((Control)cornerIcon).Dispose();
			}
			StandardWindow mainWindow = _mainWindow;
			if (mainWindow != null)
			{
				((Control)mainWindow).Dispose();
			}
			Instance = null;
		}
	}
}
