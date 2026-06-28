using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.GuildEmblems;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class HomeView : AccountRestrictedView
	{
		protected Panel _controlsPanel = new Panel();

		protected Label _versionLabel = new Label();

		private GeoGuessWindowStateService WindowState;

		public HomeView(GeoGuessWindowStateService WindowState)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			this.WindowState = WindowState;
			Service.UserManager.AccountUpdated += new EventHandler<Account>(UserManager_AccountUpdated);
		}

		private void UserManager_AccountUpdated(object sender, Account account)
		{
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await base.Load(progress);
			progress.Report("Loading Home view");
			return true;
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Expected O, but got Unknown
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			Container buildPanel2 = buildPanel;
			((Control)_controlsPanel).set_Width(((Control)buildPanel2).get_Width());
			((Control)_controlsPanel).set_Height(50);
			HomePageOption playBtn;
			HomePageOption leaderButton;
			HomePageOption settingsButton;
			HomePageOption helpButton;
			FlowPanel flow = FlowPanelExtensions.BeginFlowFill(new FlowPanel(), buildPanel2).AddControl(new HomePageOption("Play " + Service.Config.Name, Service.Textures.PlayIcon), out playBtn).AddControl(new HomePageOption("View Leaderboards", Service.Textures.LeaderboardIcon), out leaderButton)
				.AddControl(new HomePageOption("Open Settings", GameService.Content.get_DatAssetCache().GetTextureFromAssetId(156027)), out settingsButton)
				.AddControl(new HomePageOption("Help and Info", GameService.Content.get_DatAssetCache().GetTextureFromAssetId(440013)), out helpButton);
			if (_userCheck.Motd != null && _userCheck.Motd.Id > 0)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)flow);
				((Control)val).set_Width(800);
				((Control)val).set_Height(300);
				Panel motdPanel = val;
				FlowPanel obj = FlowPanelExtensions.BeginFlowFill(new FlowPanel(), (Container)(object)motdPanel).AddSpace(25).AddString("Message from " + _userCheck.Motd.AccountName)
					.AddString(_userCheck.Motd.CreatedAt.ToLongDateString() + " " + _userCheck.Motd.CreatedAt.ToLongTimeString())
					.AddString(_userCheck.Motd.Message);
				obj.set_OuterControlPadding(new Vector2(0f, 0f));
				obj.set_ControlPadding(new Vector2(5f, 10f));
				((Panel)obj).set_ShowBorder(false);
			}
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel2);
			((Control)val2).set_Location(new Point(((Control)buildPanel2).get_Width() - 320, ((Control)buildPanel2).get_Height() - 90));
			((Control)val2).set_Width(300);
			val2.set_HorizontalAlignment((HorizontalAlignment)2);
			val2.set_Text("App Version " + Module.MODULE_VERSION);
			_versionLabel = val2;
			((Control)buildPanel2).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				((Control)_versionLabel).set_Location(new Point(((Control)buildPanel2).get_Width() - 320, ((Control)buildPanel2).get_Height() - 90));
			});
			((Control)playBtn).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!Service.UserManager.TutorialState.PublicUnlocked)
				{
					Guild guild = Service.UserManager.Guilds.Find((Guild g) => Service.Config.IsTutorialGuild(g.Id));
					if (guild != null)
					{
						WindowState.SwapToGuildList(guild);
						return;
					}
				}
				WindowState.SwapToGuildSelect();
			});
			((Control)leaderButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				WindowState.SwapToLeaderboards();
			});
			((Control)settingsButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)Service.SettingsWindow).Show();
			});
			((Control)helpButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				WindowState.SwapToHelp();
			});
			flow.set_FlowDirection((ControlFlowDirection)0);
			flow.set_OuterControlPadding(new Vector2(5f, 5f));
			flow.set_ControlPadding(new Vector2(5f, 5f));
		}

		protected override void Unload()
		{
			Service.UserManager.AccountUpdated -= new EventHandler<Account>(UserManager_AccountUpdated);
			base.Unload();
		}
	}
}
