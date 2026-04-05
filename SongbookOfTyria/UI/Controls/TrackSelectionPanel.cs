using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using SongbookOfTyria.Models;

namespace SongbookOfTyria.UI.Controls
{
	public sealed class TrackSelectionPanel : FlowPanel
	{
		private const int TrackTabHeight = 24;

		private readonly MidiData _midiData;

		private readonly List<StandardButton> _trackButtons = new List<StandardButton>();

		private int _selectedTrackIndex;

		public int SelectedTrackIndex => _selectedTrackIndex;

		public event EventHandler<int> TrackChanged;

		public TrackSelectionPanel(MidiData midiData, int width)
			: this()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			_midiData = midiData;
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)2);
			((Control)this).set_Width(width);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)this).set_ControlPadding(new Vector2(3f, 0f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(5f, 0f));
			BuildTrackTabs();
		}

		private void BuildTrackTabs()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			if (_midiData?.Tracks == null || _midiData.Tracks.Count <= 1)
			{
				return;
			}
			for (int i = 0; i < _midiData.Tracks.Count; i++)
			{
				string displayName = _midiData.Tracks[i].GetDisplayName();
				int trackIndex = i;
				StandardButton val = new StandardButton();
				val.set_Text(displayName);
				((Control)val).set_Width(Math.Max(70, displayName.Length * 7 + 16));
				((Control)val).set_Height(24);
				((Control)val).set_Parent((Container)(object)this);
				StandardButton button = val;
				((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					SelectTrack(trackIndex);
				});
				_trackButtons.Add(button);
			}
			UpdateTrackButtonStates();
		}

		public void SelectTrack(int trackIndex)
		{
			if (trackIndex >= 0 && _midiData?.Tracks != null && trackIndex < _midiData.Tracks.Count && trackIndex != _selectedTrackIndex)
			{
				_selectedTrackIndex = trackIndex;
				UpdateTrackButtonStates();
				this.TrackChanged?.Invoke(this, _selectedTrackIndex);
			}
		}

		private void UpdateTrackButtonStates()
		{
			for (int i = 0; i < _trackButtons.Count; i++)
			{
				((Control)_trackButtons[i]).set_Enabled(i != _selectedTrackIndex);
			}
		}

		public string GetSelectedTrackNotation()
		{
			if (_midiData?.Tracks == null || _selectedTrackIndex >= _midiData.Tracks.Count)
			{
				return null;
			}
			MidiTrack track = _midiData.Tracks[_selectedTrackIndex];
			if (string.IsNullOrEmpty(track.Notation))
			{
				return null;
			}
			return track.Notation;
		}
	}
}
