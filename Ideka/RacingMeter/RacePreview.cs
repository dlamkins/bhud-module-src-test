using System;
using System.Collections.Generic;
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

		public FullRace FullRace
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

		public FullGhost FullGhost
		{
			get
			{
				return _view.FullGhost;
			}
			set
			{
				_view.FullGhost = ((FullRace?.Race == null) ? null : value);
				((Control)_progressBar).set_Enabled(FullGhost?.Ghost != null);
				GhostCheckpoints = FullGhost?.Ghost?.Checkpoints(FullRace.Race);
				UpdateLabels();
			}
		}

		public IReadOnlyList<GhostSnapshot> GhostCheckpoints { get; private set; }

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
			//IL_0089: Expected O, but got Unknown
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
			_progressBar = val2;
			_progressBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_view.GhostProgress = _progressBar.get_Value();
				UpdateLabels();
			});
			FullRace = null;
			FullGhost = null;
			_progressBar.set_Value(0f);
			UpdateLayout();
		}

		private void UpdateLabels()
		{
			_timeLabel.set_Text((FullGhost?.Ghost?.SnapshotAt(_progressBar.get_Value()).Time ?? TimeSpan.Zero).Formatted());
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
	}
}
