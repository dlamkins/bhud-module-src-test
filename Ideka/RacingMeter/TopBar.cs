using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class TopBar : Container
	{
		private static readonly Logger Logger = Logger.GetLogger<TopBar>();

		private const string KoFiUrl = "https://ko-fi.com/ideka";

		private const int Spacing = 10;

		private readonly Label _statusLabel;

		private readonly Label _raceLabel;

		private readonly StandardButton _unloadRaceButton;

		private readonly Label _ghostLabel;

		private readonly StandardButton _unloadGhostButton;

		private readonly Label _madeByLabel;

		private readonly StandardButton _koFiButton;

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
			//IL_00c2: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
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
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			_statusLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			_raceLabel = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.UnloadRace);
			((Control)val5).set_Enabled(false);
			_unloadRaceButton = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			_ghostLabel = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.UnloadGhost);
			((Control)val7).set_Enabled(false);
			_unloadGhostButton = val7;
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
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			((Control)_koFiButton).set_Location(Point.get_Zero());
			((Control)_koFiButton).set_Width(150);
			((Control)_koFiButton).set_Height(((Container)this).get_ContentRegion().Height);
			((Control)(object)_koFiButton).ArrangeLeftRight(10, (Control)_madeByLabel);
			((Control)(object)_madeByLabel).AlignMiddle();
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
			((Control)(object)_madeByLabel).ArrangeLeftRight(10, (Control)_statusLabel);
			((Control)(object)_statusLabel).AlignMiddle();
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
			AsyncTexture2D icon = _koFiButton.get_Icon();
			if (icon != null)
			{
				icon.Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
