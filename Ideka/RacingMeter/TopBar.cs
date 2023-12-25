using System;
using System.Diagnostics;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class TopBar : Container
	{
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

		public event Action? OnlineRequested;

		public event Action? UnloadRace;

		public event Action? UnloadGhost;

		public TopBar()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Expected O, but got Unknown
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Expected O, but got Unknown
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.KoFiButton);
			val.set_Icon(AsyncTexture2D.op_Implicit(RacingModule.ContentsManager.GetTexture("KoFiIcon.png")));
			_koFiButton = val;
			((Control)_koFiButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Process.Start("https://ko-fi.com/ideka");
			});
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
			val3.set_Text(Strings.OnlineRacing);
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
			UpdateLayout();
			((Control)_onlineButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.OnlineRequested?.Invoke();
			});
			((Control)_unloadRaceButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.UnloadRace?.Invoke();
			});
			((Control)_unloadGhostButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				this.UnloadGhost?.Invoke();
			});
			ServerStatusChanged(RacingModule.Server.Online);
			RacingModule.Server.StatusChanged += new Action<Server.OnlineStatus>(ServerStatusChanged);
			RaceLoaded(null);
			GhostLoaded(null);
		}

		public void RaceLoaded(FullRace? fullRace)
		{
			((Control)_unloadRaceButton).set_Enabled(fullRace != null);
			_raceLabel.set_Text(StringExtensions.Format(Strings.LabelCurrentRace, fullRace?.Describe() ?? Strings.None));
			PositionLabels();
		}

		public void GhostLoaded(FullGhost? fullGhost)
		{
			((Control)_unloadGhostButton).set_Enabled(fullGhost != null);
			_ghostLabel.set_Text(StringExtensions.Format(Strings.LabelCurrentGhost, fullGhost?.Describe(shortVersion: true) ?? Strings.None));
			PositionLabels();
		}

		private void ServerStatusChanged(Server.OnlineStatus status)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			Label statusLabel = _statusLabel;
			Label statusLabel2 = _statusLabel;
			Label statusLabel3 = _statusLabel;
			(string, Color, string) tuple = status switch
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

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			if (_koFiButton != null)
			{
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
				PositionLabels();
			}
		}

		private void PositionLabels()
		{
			((Control)(object)_unloadRaceButton).ArrangeRightLeft(10, (Control)_raceLabel);
			((Control)(object)_raceLabel).MiddleWith((Control)(object)_unloadRaceButton);
			((Control)(object)_unloadGhostButton).ArrangeRightLeft(10, (Control)_ghostLabel);
			((Control)(object)_ghostLabel).MiddleWith((Control)(object)_unloadGhostButton);
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.StatusChanged -= new Action<Server.OnlineStatus>(ServerStatusChanged);
			_koFiButton.get_Icon().Dispose();
			((Container)this).DisposeControl();
		}
	}
}
