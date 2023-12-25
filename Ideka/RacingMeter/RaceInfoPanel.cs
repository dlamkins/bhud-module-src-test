using System;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class RaceInfoPanel : Container
	{
		private const int Spacing = 10;

		private FullRace? _fullRace;

		private readonly FlowPanel _panel;

		private readonly Label _raceAuthorLabel;

		private readonly Label _raceModifiedLabel;

		private readonly Label _raceMapLabel;

		private readonly Label _raceTypeLabel;

		private readonly Label _raceCheckpointsLabel;

		private readonly Label _raceLengthLabel;

		private readonly StandardButton _runRaceButton;

		public FullRace? FullRace
		{
			get
			{
				return _fullRace;
			}
			set
			{
				_fullRace = value;
				((Control)_runRaceButton).set_Enabled(FullRace != null);
				((Panel)_panel).set_Title(FullRace?.Race.Name ?? Strings.None);
				string author = ((FullRace?.IsLocal ?? false) ? Strings.RaceLocal : FullRace?.Meta.AuthorName);
				_raceAuthorLabel.set_Text(StringExtensions.Format(Strings.LabelRaceAuthor, author ?? ""));
				((Control)_raceAuthorLabel).set_BasicTooltipText((FullRace?.IsLocal ?? false) ? Strings.RaceLocalAuthorTooltip : author);
				string modifiedRelative = ((FullRace != null) ? FullRace!.Meta.Modified.ToRelativeDateUtc() : null);
				string modified = ((FullRace != null) ? $"{FullRace!.Meta.Modified.ToLocalTime()}" : null);
				_raceModifiedLabel.set_Text(StringExtensions.Format(Strings.LabelRaceUpdated, modifiedRelative ?? ""));
				((Control)_raceModifiedLabel).set_BasicTooltipText(modified);
				string type = ((FullRace != null) ? (FullRace!.Race.Type.Describe() ?? Strings.RaceTypeUnknown) : null);
				_raceTypeLabel.set_Text(StringExtensions.Format(Strings.LabelRaceType, type ?? ""));
				string mapName = ((FullRace != null) ? RacingModule.MapData.Describe(FullRace!.Race.MapId) : null);
				_raceMapLabel.set_Text(StringExtensions.Format(Strings.LabelRaceMap, mapName ?? ""));
				_raceCheckpointsLabel.set_Text(Strings.LabelRaceCheckpoints.Format(FullRace?.Race.Checkpoints.Count() ?? 0));
				_raceLengthLabel.set_Text(StringExtensions.Format(Strings.LabelRaceLength, $"{FullRace?.Race.GetLength() ?? 0f:N0}"));
			}
		}

		public event Action<FullRace>? RaceRequested;

		public RaceInfoPanel()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(Vector2.get_UnitY() * 10f / 2f);
			val.set_OuterControlPadding(Vector2.get_One() * 10f / 2f);
			((Panel)val).set_ShowTint(true);
			_panel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_panel);
			val2.set_Font(Control.get_Content().get_DefaultFont16());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			_raceAuthorLabel = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_panel);
			val3.set_Font(Control.get_Content().get_DefaultFont16());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			_raceModifiedLabel = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_panel);
			val4.set_Font(Control.get_Content().get_DefaultFont16());
			val4.set_AutoSizeHeight(true);
			val4.set_AutoSizeWidth(true);
			_raceMapLabel = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_panel);
			val5.set_Font(Control.get_Content().get_DefaultFont16());
			val5.set_AutoSizeHeight(true);
			val5.set_AutoSizeWidth(true);
			_raceTypeLabel = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_panel);
			val6.set_Font(Control.get_Content().get_DefaultFont16());
			val6.set_AutoSizeHeight(true);
			val6.set_AutoSizeWidth(true);
			_raceCheckpointsLabel = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_panel);
			val7.set_Font(Control.get_Content().get_DefaultFont16());
			val7.set_AutoSizeHeight(true);
			val7.set_AutoSizeWidth(true);
			_raceLengthLabel = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(Strings.RunRace);
			_runRaceButton = val8;
			UpdateLayout();
			((Control)_runRaceButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (FullRace?.Race != null)
				{
					this.RaceRequested?.Invoke(FullRace);
				}
			});
			FullRace = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (_panel != null)
			{
				((Control)_runRaceButton).set_Bottom(((Container)this).get_ContentRegion().Height);
				((Control)(object)_runRaceButton).ArrangeBottomUp(10, (Control)_panel);
				((Control)_panel).set_Left(0);
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)(object)_panel).HeightFillUp();
				((Control)(object)_runRaceButton).CenterWith((Control)(object)_panel);
			}
		}
	}
}
