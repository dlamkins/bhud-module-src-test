using System;
using System.Linq;
using Blish_HUD.Controls;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class LobbyRaceInfoPanel : FlowPanel
	{
		private const int Spacing = 10;

		private readonly RacingClient Client;

		private readonly Label _raceNameLabel;

		private readonly Label _raceAuthorLabel;

		private readonly Label _raceModifiedLabel;

		private readonly Label _raceMapLabel;

		private readonly Label _raceTypeLabel;

		private readonly Label _raceCheckpointsLabel;

		private readonly Label _raceLengthLabel;

		private readonly Label _raceLoopingLabel;

		public LobbyRaceInfoPanel(RacingClient _client)
			: this()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Expected O, but got Unknown
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			Client = _client;
			((Panel)this).set_Title("Selected Race");
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(Vector2.get_UnitY() * 10f / 2f);
			((FlowPanel)this).set_OuterControlPadding(Vector2.get_One() * 10f / 2f);
			((Panel)this).set_ShowTint(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Font(Control.get_Content().get_DefaultFont16());
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			_raceNameLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			_raceAuthorLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Font(Control.get_Content().get_DefaultFont16());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			_raceModifiedLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Font(Control.get_Content().get_DefaultFont16());
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			_raceMapLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Font(Control.get_Content().get_DefaultFont16());
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			_raceTypeLabel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Font(Control.get_Content().get_DefaultFont16());
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			_raceCheckpointsLabel = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Font(Control.get_Content().get_DefaultFont16());
			val7.set_AutoSizeHeight(true);
			val7.set_AutoSizeWidth(true);
			_raceLengthLabel = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Font(Control.get_Content().get_DefaultFont16());
			val8.set_AutoSizeHeight(true);
			val8.set_AutoSizeWidth(true);
			((Control)val8).set_BasicTooltipText("Looping races can be run with multiple laps.");
			_raceLoopingLabel = val8;
			Client.LobbyRaceUpdated += new Action<FullRace>(RaceUpdated);
			RaceUpdated(Client.Lobby?.Race);
			UpdateLayout();
		}

		private void RaceUpdated(FullRace? race)
		{
			string name = race?.Race.Name ?? Strings.None;
			_raceNameLabel.set_Text(name);
			((Control)_raceNameLabel).set_BasicTooltipText(name);
			string author = ((race != null && race!.IsLocal) ? Strings.RaceLocal : race?.Meta.AuthorName);
			_raceAuthorLabel.set_Text(StringExtensions.Format(Strings.RaceAuthorLabel, author ?? ""));
			((Control)_raceAuthorLabel).set_BasicTooltipText((race != null && race!.IsLocal) ? Strings.RaceLocalAuthorTooltip : author);
			string modifiedRelative = race?.Meta.Modified.ToRelativeDateUtc();
			string modified = ((race != null) ? $"{race!.Meta.Modified.ToLocalTime()}" : null);
			_raceModifiedLabel.set_Text(StringExtensions.Format(Strings.RaceUpdatedLabel, modifiedRelative ?? ""));
			((Control)_raceModifiedLabel).set_BasicTooltipText(modified);
			string type = ((race != null) ? (race!.Race.Type.Describe() ?? Strings.RaceTypeUnknown) : null);
			_raceTypeLabel.set_Text(StringExtensions.Format(Strings.RaceTypeLabel, type ?? ""));
			string mapName = ((race != null) ? RacingModule.MapData.Describe(race!.Race.MapId) : null);
			_raceMapLabel.set_Text(StringExtensions.Format(Strings.RaceMapLabel, mapName ?? ""));
			string looping = ((race == null) ? "" : ((race!.Race.LoopStartPoint == null) ? "No" : "Yes"));
			_raceLoopingLabel.set_Text("Looping: " + looping);
			_raceCheckpointsLabel.set_Text(Strings.RaceCheckpointsLabel.Format(race?.Race.Checkpoints.Count() ?? 0));
			_raceLengthLabel.set_Text(StringExtensions.Format(Strings.RaceLengthLabel, $"{race?.Race.GetLength() ?? 0f:N0}"));
			UpdateLayout();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (Client != null)
			{
				((Container)(object)this).SetContentRegionHeight(((Control)_raceLoopingLabel).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			Client.LobbyRaceUpdated -= new Action<FullRace>(RaceUpdated);
			((FlowPanel)this).DisposeControl();
		}
	}
}
