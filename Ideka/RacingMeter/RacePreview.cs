using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Ideka.RacingMeter.Lib;

namespace Ideka.RacingMeter
{
	public class RacePreview : Panel
	{
		private const int Spacing = 10;

		private readonly RacePreviewView _view;

		private readonly Label _timeLabel;

		private readonly TrackBar _progressBar;

		public FullRace? FullRace
		{
			get
			{
				return _view.FullRace;
			}
			set
			{
				_view.FullRace = value;
				_progressBar.set_Value(0f);
			}
		}

		public FullGhost? FullGhost
		{
			get
			{
				return _view.FullGhost;
			}
			set
			{
				if (value?.Ghost?.RaceId == FullRace?.Meta.Id)
				{
					_view.FullGhost = value;
				}
				((Control)_progressBar).set_Enabled(_view.Ghost != null);
				Race race = _view.Race;
				object ghostCheckpoints;
				if (race != null)
				{
					Ghost ghost = _view.Ghost;
					if (ghost != null)
					{
						ghostCheckpoints = ghost.Checkpoints(race);
						goto IL_0084;
					}
				}
				ghostCheckpoints = null;
				goto IL_0084;
				IL_0084:
				GhostCheckpoints = (IReadOnlyList<GhostSnapshot>?)ghostCheckpoints;
				UpdateLabels();
			}
		}

		public IReadOnlyList<GhostSnapshot>? GhostCheckpoints { get; private set; }

		public RacePreview()
			: this()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			((Panel)this).set_Title(Strings.RacePreview);
			((Panel)this).set_ShowTint(true);
			RacePreviewView racePreviewView = new RacePreviewView();
			((Control)racePreviewView).set_Parent((Container)(object)this);
			_view = racePreviewView;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Font(Control.get_Content().get_DefaultFont16());
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			_timeLabel = val;
			TrackBar val2 = new TrackBar();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_SmallStep(true);
			val2.set_MinValue(0f);
			val2.set_MaxValue(1f);
			val2.set_Value(0f);
			_progressBar = val2;
			UpdateLayout();
			_progressBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_view.GhostProgress = _progressBar.get_Value();
				UpdateLabels();
			});
			RacingModule.Server.RemoteRacesChanged += new Action<RemoteRaces>(RemoteRacesChanged);
			RacingModule.LocalData.RacesChanged += new Action<IReadOnlyDictionary<string, FullRace>>(LocalRacesChanged);
			UpdateMaps();
			FullRace = null;
			FullGhost = null;
			_progressBar.set_Value(0f);
		}

		private void LocalRacesChanged(IReadOnlyDictionary<string, FullRace> _)
		{
			UpdateMaps();
		}

		private void RemoteRacesChanged(RemoteRaces _)
		{
			UpdateMaps();
		}

		private void UpdateMaps()
		{
			_view.RepopulateMaps(Enumerable.Empty<int>().Concat(RacingModule.Server.RemoteRaces.Races.Values.Select((FullRace r) => r.Race.MapId)).Concat(RacingModule.LocalData.Races.Values.Select((FullRace r) => r.Race.MapId)));
		}

		private void UpdateLabels()
		{
			_timeLabel.set_Text((FullGhost?.Ghost.SnapshotAt(_progressBar.get_Value()).Time ?? TimeSpan.Zero).Formatted());
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			if (_view != null)
			{
				((Control)_progressBar).set_Left(10);
				((Control)_progressBar).set_Bottom(((Container)this).get_ContentRegion().Height - 10);
				((Control)_progressBar).set_Width(((Container)this).get_ContentRegion().Width - 20);
				((Control)_timeLabel).set_Left(10);
				((Control)_timeLabel).set_Bottom(((Control)_progressBar).get_Top() - 10);
				((Control)_view).set_Left(0);
				((Control)_view).set_Bottom(((Control)_progressBar).get_Top() - 10);
				((Control)_view).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)_view).set_Height(((Control)_view).get_Bottom());
			}
		}

		protected override void DisposeControl()
		{
			RacingModule.Server.RemoteRacesChanged -= new Action<RemoteRaces>(RemoteRacesChanged);
			RacingModule.LocalData.RacesChanged -= new Action<IReadOnlyDictionary<string, FullRace>>(LocalRacesChanged);
			((Panel)this).DisposeControl();
		}
	}
}
