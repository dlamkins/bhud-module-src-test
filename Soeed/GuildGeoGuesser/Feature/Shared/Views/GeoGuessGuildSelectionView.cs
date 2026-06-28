using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;
using Soeed.GuildGeoGuesser.Utils;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Views
{
	public class GeoGuessGuildSelectionView : AccountRestrictedView
	{
		private readonly GeoGuessWindowStateService _windowState;

		public GeoGuessGuildSelectionView(GeoGuessWindowStateService WindowState)
			: base(showBackButton: true)
		{
			_windowState = WindowState;
			Service.UserManager.GuildsUpdated += new EventHandler<IEnumerable<Guild>>(UserManager_GuildsUpdated);
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
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Expected O, but got Unknown
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			((Control)_backButton).add_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_controlsHeader);
			val.set_Text("Select a Guild to begin");
			((Control)val).set_Location(new Point(50, 5));
			((Control)val).set_Width(250);
			((Control)val).set_Height(40);
			val.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)24, (FontStyle)0));
			val.set_TextColor(Color.get_LightGoldenrodYellow());
			ReloadButton reloadButton = new ReloadButton();
			((Control)reloadButton).set_Parent((Container)(object)_controlsHeader);
			((Control)reloadButton).set_Location(new Point(305, 0));
			((Control)reloadButton).set_BasicTooltipText("Refresh the guild list");
			((Control)reloadButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_windowState.SwapToGuildSelect();
			});
			_flowPanel.set_FlowDirection((ControlFlowDirection)0);
			((Panel)_flowPanel).set_CanScroll(true);
			if (_windowState.Guilds == null || _windowState.Guilds.Count == 0)
			{
				Label val2 = new Label();
				((Control)val2).set_Parent((Container)(object)_flowPanel);
				val2.set_Text("Loading guilds...\n\nIf this persists, please check your API key permissions.");
				((Control)val2).set_Width(((Control)buildPanel).get_Width() - 20);
				((Control)val2).set_Height(100);
				val2.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)18, (FontStyle)0));
				val2.set_TextColor(Color.get_LightBlue());
				val2.set_HorizontalAlignment((HorizontalAlignment)1);
				val2.set_VerticalAlignment((VerticalAlignment)1);
				return;
			}
			foreach (Guild guild in _windowState.Guilds)
			{
				FlowPanel flowPanel = _flowPanel;
				Panel val3 = new Panel();
				((Control)val3).set_Width(400);
				((Control)val3).set_Height(130);
				flowPanel.AddControl<Panel>(val3, out Panel guildPanel);
				GuildEmblemControl guildEmblemControl = new GuildEmblemControl(guild);
				((Control)guildEmblemControl).set_Parent((Container)(object)guildPanel);
				if (!Service.UserManager.TutorialState.PublicUnlocked && Service.Config.IsTutorialGuild(guild.Id))
				{
					Label val4 = new Label();
					((Control)val4).set_Parent((Container)(object)guildPanel);
					val4.set_Text("Practice Mode");
					val4.set_AutoSizeWidth(true);
					((Control)val4).set_Height(20);
					((Control)val4).set_Location(new Point(5, ((Control)guildPanel).get_Height() - 22));
					val4.set_Font(GameService.Content.GetFont((FontFace)0, (FontSize)14, (FontStyle)2));
					val4.set_TextColor(Color.get_LightGreen());
				}
				((Control)guildEmblemControl).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					_windowState.SwapToGuildList(guild);
				});
			}
		}

		private void UserManager_GuildsUpdated(object sender, IEnumerable<Guild> e)
		{
			_windowState.SwapToGuildSelect();
		}

		private void GoBackClickHandler(object sender, MouseEventArgs e)
		{
			_windowState.SwapToHome();
		}

		protected override void Unload()
		{
			((Control)_backButton).remove_Click((EventHandler<MouseEventArgs>)GoBackClickHandler);
			Service.UserManager.GuildsUpdated -= new EventHandler<IEnumerable<Guild>>(UserManager_GuildsUpdated);
			base.Unload();
		}
	}
}
