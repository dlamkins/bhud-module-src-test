using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class TopBar : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<TopBar>();

		private const string KoFiUrl = "https://ko-fi.com/ideka";

		private const int Spacing = 10;

		private readonly StandardButton _koFiButton;

		private readonly Label _madeByLabel;

		private readonly Label _statusLabel;

		private readonly StandardButton _onlineButton;

		private readonly Label _raceLabel;

		private readonly StandardButton _unloadRaceButton;

		private readonly Label _ghostLabel;

		private readonly StandardButton _unloadGhostButton;

		public TopBar()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.KoFiButton);
			val.set_Icon(AsyncTexture2D.op_Implicit(RacingModule.ContentsManager.GetTexture("KoFiIcon.png")));
			_koFiButton = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)0);
			val2.set_Text(StringExtensions.Format(Strings.CreatedBy, $"{RacingModule.Name} v{RacingModule.Version}", "Ideka"));
			_madeByLabel = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Visible(false);
			val3.set_Text("Online Racing");
			_onlineButton = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			_statusLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			_raceLabel = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.UnloadRace);
			((Control)val6).set_Enabled(false);
			_unloadRaceButton = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_AutoSizeWidth(true);
			val7.set_AutoSizeHeight(true);
			_ghostLabel = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(Strings.UnloadGhost);
			((Control)val8).set_Enabled(false);
			_unloadGhostButton = val8;
			((Control)_unloadRaceButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RacingModule.Racer.FullRace = null;
			});
			((Control)_unloadGhostButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RacingModule.Racer.FullGhost = null;
				RacingModule.Racer.SpecificGhostLoaded = true;
			});
			((Control)_koFiButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/ideka");
			});
			((Control)_onlineButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RacingModule.Racer.CurrentMode = Racer.Mode.Online;
			});
			RacingModule.Racer.RaceLoaded += new Action<FullRace>(RaceLoaded);
			RacingModule.Racer.GhostLoaded += new Action<FullGhost>(GhostLoaded);
			UpdateLayout();
			RacingModule.Server.CheckVersion(RacingModule.Version.ToString()).Done(Logger, null).ContinueWith(delegate
			{
				UpdateLabels();
			});
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			((Control)_koFiButton).set_Width(150);
			((Control)_koFiButton).set_Height(((Container)this).get_ContentRegion().Height);
			((Control)(object)_koFiButton).ArrangeLeftRight(10, (Control)_madeByLabel, (Control)_onlineButton, (Control)_statusLabel);
			((Control)(object)_koFiButton).AlignMiddle();
			((Control)(object)_madeByLabel).AlignMiddle();
			((Control)(object)_onlineButton).AlignMiddle();
			((Control)(object)_statusLabel).AlignMiddle();
			StandardButton unloadRaceButton = _unloadRaceButton;
			int width;
			((Control)_unloadGhostButton).set_Right(width = ((Container)this).get_ContentRegion().Width);
			((Control)unloadRaceButton).set_Right(width);
			((Control)_unloadRaceButton).set_Bottom(((Container)this).get_ContentRegion().Height / 2);
			((Control)_unloadGhostButton).set_Top(((Container)this).get_ContentRegion().Height / 2);
			UpdateLabels();
		}

		private void UpdateLabels()
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			_raceLabel.set_Text(StringExtensions.Format(Strings.CurrentRaceLabel, RacingModule.Racer.FullRace.Describe() ?? Strings.None));
			((Control)(object)_unloadRaceButton).ArrangeRightLeft(10, (Control)_raceLabel);
			((Control)(object)_raceLabel).MiddleWith((Control)(object)_unloadRaceButton);
			_ghostLabel.set_Text(StringExtensions.Format(Strings.CurrentGhostLabel, RacingModule.Racer.FullGhost.Describe(shortVersion: true) ?? Strings.None));
			((Control)(object)_unloadGhostButton).ArrangeRightLeft(10, (Control)_ghostLabel);
			((Control)(object)_ghostLabel).MiddleWith((Control)(object)_unloadGhostButton);
			Label statusLabel = _statusLabel;
			Label statusLabel2 = _statusLabel;
			Label statusLabel3 = _statusLabel;
			(string, Color, string) tuple = RacingModule.Server.Online switch
			{
				Server.OnlineStatus.Yes => (Strings.ServerStatusYes, Color.get_White(), ""), 
				Server.OnlineStatus.Unchecked => (Strings.SupportStatusUnchecked, Color.get_White(), Strings.SupportStatusTooltipUnchecked), 
				Server.OnlineStatus.No => (Strings.SupportStatusNo, Color.get_DarkOrange(), Strings.SupportStatusTooltipNo), 
				_ => (Strings.SupportStatusFaulted, Color.get_DarkOrange(), Strings.SupportStatusTooltipFaulted), 
			};
			string item;
			statusLabel.set_Text(item = tuple.Item1);
			Color item2;
			statusLabel2.set_TextColor(item2 = tuple.Item2);
			((Control)statusLabel3).set_BasicTooltipText(item = tuple.Item3);
		}

		private void RaceLoaded(FullRace race)
		{
			((Control)_unloadRaceButton).set_Enabled(race != null);
			UpdateLabels();
		}

		private void GhostLoaded(FullGhost ghost)
		{
			((Control)_unloadGhostButton).set_Enabled(ghost != null);
			UpdateLabels();
		}

		protected override void DisposeControl()
		{
			RacingModule.Racer.RaceLoaded -= new Action<FullRace>(RaceLoaded);
			RacingModule.Racer.GhostLoaded -= new Action<FullGhost>(GhostLoaded);
			AsyncTexture2D icon = _onlineButton.get_Icon();
			if (icon != null)
			{
				icon.Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
