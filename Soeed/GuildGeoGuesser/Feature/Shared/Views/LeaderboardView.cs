using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class LeaderboardView : AccountRestrictedView
	{
		[CompilerGenerated]
		private GeoGuessWindowStateService _003CWindowState_003EP;

		private LeaderboardStats _stats;

		public LeaderboardView(GeoGuessWindowStateService WindowState)
		{
			_003CWindowState_003EP = WindowState;
			_stats = new LeaderboardStats();
			base._002Ector(showBackButton: true);
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			await base.Load(progress);
			progress.Report("Loading leaderboard statistics...");
			LeaderboardStats stats = await Service.GeoServerWrapper.GetLeaderboardStatsAsync();
			if (stats != null)
			{
				_stats = stats;
			}
			return true;
		}

		protected override void DoBuild(Container buildPanel)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			val.set_Text("Leaderboards");
			((Control)val).set_Location(new Point(50, 5));
			((Control)val).set_Width(250);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val.set_TextColor(Color.get_LightGoldenrodYellow());
			((Control)new LeaderboardStatBestScoreControl(_stats.BestScore)).set_Parent((Container)(object)_flowPanel);
			foreach (LeaderboardStat stat in _stats.Stats)
			{
				((Control)new LeaderboardStatControl(stat)).set_Parent((Container)(object)_flowPanel);
			}
			((Panel)_flowPanel).set_CanScroll(true);
			_flowPanel.set_FlowDirection((ControlFlowDirection)0);
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			_003CWindowState_003EP.SwapToHome();
		}

		protected override void Unload()
		{
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			base.Unload();
		}
	}
}
