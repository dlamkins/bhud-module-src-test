using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Ideka.RacingMeter.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RacePreview : Panel
	{
		private const int Spacing = 10;

		private readonly RacePreviewView _view;

		private readonly Label _timeLabel;

		private readonly StandardButton _playButton;

		private readonly TrackBar _progressBar;

		private readonly MeasurerGhost _ghostMeasurer;

		private readonly AnchoredRect _meterContainer;

		private bool _isPlaying;

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
				_view.FullGhost = ((value?.Ghost?.RaceId == FullRace?.Meta.Id) ? value : null);
				_isPlaying = false;
				TrackBar progressBar = _progressBar;
				bool enabled;
				((Control)_playButton).set_Enabled(enabled = _view.Ghost != null);
				((Control)progressBar).set_Enabled(enabled);
				Ghost ghost2 = _view.Ghost;
				if (ghost2 != null)
				{
					_progressBar.set_MaxValue((float)ghost2.Time.TotalSeconds);
				}
				Race race = _view.Race;
				object ghostCheckpoints;
				if (race != null)
				{
					Ghost ghost = _view.Ghost;
					if (ghost != null)
					{
						ghostCheckpoints = ghost.Checkpoints(race);
						goto IL_00c8;
					}
				}
				ghostCheckpoints = null;
				goto IL_00c8;
				IL_00c8:
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
			//IL_006c: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
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
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			_playButton = val2;
			TrackBar val3 = new TrackBar();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_SmallStep(true);
			val3.set_MinValue(0f);
			val3.set_MaxValue(1f);
			val3.set_Value(0f);
			_progressBar = val3;
			_meterContainer = new AnchoredRect
			{
				SizeDelta = new Vector2(400f, 100f),
				Anchor = new Vector2(0.5f, 0.5f)
			};
			_ghostMeasurer = new MeasurerGhost();
			_meterContainer.AddChild(BeetleMeter.Construct(_ghostMeasurer, () => null));
			UpdateLayout();
			((Control)_playButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_isPlaying = !_isPlaying;
			});
			_progressBar.add_ValueChanged((EventHandler<ValueEventArgs<float>>)delegate
			{
				_view.GhostProgress = TimeSpan.FromSeconds(_progressBar.get_Value());
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
			_timeLabel.set_Text(TimeSpan.FromSeconds(_progressBar.get_Value()).Formatted());
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			_meterContainer.Update(gameTime);
			if (_isPlaying)
			{
				TrackBar progressBar = _progressBar;
				progressBar.set_Value(progressBar.get_Value() + (float)gameTime.get_ElapsedGameTime().TotalSeconds);
			}
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
			FullGhost fg = FullGhost;
			if (fg != null)
			{
				Ghost ghost = fg.Ghost;
				if (ghost != null)
				{
					SpriteBatchExtensions.Begin(spriteBatch, ((Control)this).get_SpriteBatchParameters());
					_ghostMeasurer.Update(ghost, TimeSpan.FromSeconds(_progressBar.get_Value()));
					spriteBatch.End();
				}
			}
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			if (_view != null)
			{
				((Control)_playButton).set_Left(10);
				((Control)_playButton).set_Bottom(((Container)this).get_ContentRegion().Height - 10);
				((Control)_playButton).set_Width(((Control)_playButton).get_Height());
				((Control)(object)_playButton).ArrangeLeftRight(10, (Control)_progressBar);
				((Control)(object)_progressBar).MiddleWith((Control)(object)_playButton);
				((Control)(object)_progressBar).WidthFillRight(10);
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
