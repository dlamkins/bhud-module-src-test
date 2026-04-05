using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace SongbookOfTyria.UI.Controls
{
	public class SectionedSeekBar : Panel
	{
		private const int SeekBarControlHeight = 62;

		private const int TopMargin = 4;

		private static readonly Color[] MarkerColors = (Color[])(object)new Color[4]
		{
			new Color(80, 180, 80),
			new Color(180, 180, 80),
			new Color(180, 80, 180),
			new Color(180, 120, 60)
		};

		private readonly List<SectionInfo> _sections = new List<SectionInfo>();

		private readonly List<MarkerInfo> _markers = new List<MarkerInfo>();

		private double _duration;

		private double _currentPosition;

		private int _nextMarkerId = 1;

		private SeekBarControl _seekBarControl;

		public double Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				if (Math.Abs(_duration - value) > 0.001)
				{
					_duration = value;
					if (_seekBarControl != null)
					{
						_seekBarControl.Duration = value;
					}
				}
			}
		}

		public double CurrentPosition
		{
			get
			{
				return _currentPosition;
			}
			set
			{
				if (Math.Abs(_currentPosition - value) > 0.001)
				{
					_currentPosition = value;
					if (_seekBarControl != null)
					{
						_seekBarControl.CurrentPosition = value;
					}
				}
			}
		}

		public event EventHandler<double> SeekRequested;

		public event EventHandler<MarkerInfo> MarkerAdded;

		public event EventHandler<MarkerInfo> MarkerRemoved;

		public event EventHandler<MarkerInfo> MarkerMoved;

		public SectionedSeekBar()
			: this()
		{
			((Control)this).set_Height(66);
			BuildLayout();
		}

		private void BuildLayout()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			SeekBarControl seekBarControl = new SeekBarControl();
			((Control)seekBarControl).set_Width(Math.Max(100, ((Control)this).get_Width() - 10));
			((Control)seekBarControl).set_Height(62);
			((Control)seekBarControl).set_Location(new Point(0, 4));
			((Control)seekBarControl).set_Parent((Container)(object)this);
			_seekBarControl = seekBarControl;
			_seekBarControl.SeekRequested += OnSeekBarSeekRequested;
			_seekBarControl.MarkerRemoved += OnSeekBarMarkerRemoved;
			_seekBarControl.MarkerMoved += OnSeekBarMarkerMoved;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void OnSeekBarSeekRequested(object sender, double time)
		{
			this.SeekRequested?.Invoke(this, time);
		}

		private void OnSeekBarMarkerRemoved(object sender, int markerIndex)
		{
			RemoveMarker(markerIndex);
		}

		private void OnSeekBarMarkerMoved(object sender, int markerIndex)
		{
			if (markerIndex >= 0 && markerIndex < _markers.Count)
			{
				this.MarkerMoved?.Invoke(this, _markers[markerIndex]);
			}
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_seekBarControl != null)
			{
				((Control)_seekBarControl).set_Width(Math.Max(100, ((Control)this).get_Width() - 10));
			}
		}

		public void SetSections(IEnumerable<SectionInfo> sections)
		{
			_sections.Clear();
			if (sections != null)
			{
				_sections.AddRange(sections);
			}
			_seekBarControl?.SetSections(_sections);
		}

		public void SetMarkers(IEnumerable<MarkerInfo> markers)
		{
			_markers.Clear();
			if (markers != null)
			{
				_markers.AddRange(markers);
				_nextMarkerId = ((_markers.Count <= 0) ? 1 : (_markers.Max((MarkerInfo m) => m.Id) + 1));
			}
			_seekBarControl?.SetMarkers(_markers);
		}

		public IReadOnlyList<MarkerInfo> GetMarkers()
		{
			return _markers.AsReadOnly();
		}

		public IReadOnlyList<SectionInfo> GetSections()
		{
			return _sections.AsReadOnly();
		}

		public void AddMarker(double time)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			int colorIndex = _markers.Count % MarkerColors.Length;
			MarkerInfo marker = new MarkerInfo
			{
				Time = time,
				Color = MarkerColors[colorIndex],
				Id = _nextMarkerId++
			};
			_markers.Add(marker);
			_seekBarControl?.SetMarkers(_markers);
			this.MarkerAdded?.Invoke(this, marker);
		}

		public void RemoveMarker(int index)
		{
			if (index >= 0 && index < _markers.Count)
			{
				MarkerInfo marker = _markers[index];
				_markers.RemoveAt(index);
				_seekBarControl?.SetMarkers(_markers);
				this.MarkerRemoved?.Invoke(this, marker);
			}
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			if (_seekBarControl != null)
			{
				_seekBarControl.SeekRequested -= OnSeekBarSeekRequested;
				_seekBarControl.MarkerRemoved -= OnSeekBarMarkerRemoved;
				_seekBarControl.MarkerMoved -= OnSeekBarMarkerMoved;
			}
			((Panel)this).DisposeControl();
		}
	}
}
