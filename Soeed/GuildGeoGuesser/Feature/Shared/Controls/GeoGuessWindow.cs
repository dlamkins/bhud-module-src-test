using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Feature.Shared.Views;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Controls
{
	public class GeoGuessWindow : StandardWindow
	{
		private GeoGuessWindowStateService _windowState = new GeoGuessWindowStateService();

		private static AsyncTexture2D Background => GameService.Content.get_DatAssetCache().GetTextureFromAssetId(155985);

		private static Rectangle SettingPanelRegion => new Rectangle(40, 26, 913, 691);

		private static Rectangle SettingPanelContentRegion => new Rectangle(40, 26, 913, 691);

		private static Point SettingPanelWindowSize => new Point(850, 600);

		public GeoGuessWindowStateService State => _windowState;

		public GeoGuessWindow()
			: this(Background, SettingPanelRegion, SettingPanelContentRegion, SettingPanelWindowSize)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Id("Soeed_Guild_Geo_Guess_Main_Window");
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Emblem(AsyncTexture2D.op_Implicit(Service.Textures.Emblem));
			((WindowBase2)this).set_Title(Service.Config.Name);
			((WindowBase2)this).set_CanResize(true);
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			Service.UserManager.AccountUpdated += new EventHandler<Account>(UserManager_AccountUpdated);
			Service.Settings.MainWindowToggle.get_Value().set_Enabled(true);
			Service.Settings.MainWindowToggle.get_Value().add_Activated((EventHandler<EventArgs>)WindowToggleActivated);
		}

		private void WindowToggleActivated(object sender, EventArgs e)
		{
			WindowToggle();
		}

		public void WindowToggle()
		{
			if (((Control)this).get_Visible())
			{
				((Control)this).Hide();
			}
			else
			{
				OpenWindowState();
			}
		}

		public void OpenWindowState()
		{
			switch (_windowState.State)
			{
			case GeoGuessState.TutorialList:
				((StandardWindow)this).Show((IView)(object)new TutorialListingView(_windowState));
				break;
			case GeoGuessState.TutorialPuzzle:
				((StandardWindow)this).Show((IView)(object)new TutorialPuzzleView(_windowState));
				break;
			case GeoGuessState.PuzzleDetails:
				((StandardWindow)this).Show((IView)(object)new GeoGuessDetailsView(_windowState));
				break;
			case GeoGuessState.GuildPuzzles:
				((StandardWindow)this).Show((IView)(object)new GeoGuessListingView(_windowState));
				break;
			case GeoGuessState.PuzzleCreate:
				((StandardWindow)this).Show((IView)(object)new GeoGuessCreateView(_windowState));
				break;
			case GeoGuessState.PuzzleEdit:
				((StandardWindow)this).Show((IView)(object)new GeoGuessEditView(_windowState));
				break;
			case GeoGuessState.Leaders:
			case GeoGuessState.Leaderboards:
				((StandardWindow)this).Show((IView)(object)new LeaderboardView(_windowState));
				break;
			case GeoGuessState.Help:
				((StandardWindow)this).Show((IView)(object)new HelpView(_windowState));
				break;
			case GeoGuessState.GuildSelect:
				((StandardWindow)this).Show((IView)(object)new GeoGuessGuildSelectionView(_windowState));
				break;
			default:
				((StandardWindow)this).Show((IView)(object)new HomeView(_windowState));
				break;
			}
		}

		private void UserManager_AccountUpdated(object sender, Account account)
		{
			((WindowBase2)this).set_Subtitle(account.get_Name() ?? "");
		}

		protected override void DisposeControl()
		{
			Service.Settings.MainWindowToggle.get_Value().set_Enabled(false);
			Service.Settings.MainWindowToggle.get_Value().remove_Activated((EventHandler<EventArgs>)WindowToggleActivated);
			Service.UserManager.AccountUpdated -= new EventHandler<Account>(UserManager_AccountUpdated);
			_windowState?.Dispose();
		}
	}
}
