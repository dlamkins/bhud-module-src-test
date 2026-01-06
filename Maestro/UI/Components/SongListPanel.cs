using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class SongListPanel : FlowPanel
	{
		public static class Layout
		{
			public const int CardSpacing = 4;

			public const int OuterPadding = 4;

			public const int ScrollbarWidth = 20;
		}

		private readonly SongPlayer _songPlayer;

		private readonly Dictionary<Song, SongCard> _songCards = new Dictionary<Song, SongCard>();

		private Song _selectedSong;

		private readonly int _cardWidth;

		public Song SelectedSong => _selectedSong;

		public event EventHandler<Song> SongSelected;

		public event EventHandler<Song> SongPlayRequested;

		public event EventHandler<int> CountChanged;

		public SongListPanel(SongPlayer songPlayer, int width, int height)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			_cardWidth = width - 20 - 4;
			base.Size = new Point(width, height);
			base.FlowDirection = ControlFlowDirection.SingleTopToBottom;
			base.CanScroll = true;
			base.ShowBorder = true;
			base.ControlPadding = new Vector2(0f, 4f);
			base.OuterControlPadding = new Vector2(4f, 4f);
		}

		public void RefreshSongs(IEnumerable<Song> songs)
		{
			ClearChildren();
			_songCards.Clear();
			foreach (Song song in songs)
			{
				SongCard card = new SongCard(song, _cardWidth)
				{
					Parent = this
				};
				card.PlayClicked += OnCardPlayClicked;
				card.CardClicked += OnCardClicked;
				_songCards[song] = card;
			}
			UpdateCardStates();
			this.CountChanged?.Invoke(this, _songCards.Count);
		}

		private void OnCardPlayClicked(object sender, MouseEventArgs e)
		{
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				SelectSong(card.Song);
				this.SongPlayRequested?.Invoke(this, card.Song);
			}
		}

		private void OnCardClicked(object sender, MouseEventArgs e)
		{
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				SelectSong(card.Song);
			}
		}

		public void SelectSong(Song song)
		{
			_selectedSong = song;
			UpdateCardStates();
			this.SongSelected?.Invoke(this, song);
		}

		public void UpdateCardStates()
		{
			foreach (KeyValuePair<Song, SongCard> kvp in _songCards)
			{
				bool isPlaying = _songPlayer.IsPlaying && _songPlayer.CurrentSong == kvp.Key;
				bool isSelected = kvp.Key == _selectedSong;
				kvp.Value.IsPlaying = isPlaying;
				kvp.Value.IsSelected = isSelected;
			}
		}

		protected override void DisposeControl()
		{
			foreach (SongCard value in _songCards.Values)
			{
				value.PlayClicked -= OnCardPlayClicked;
				value.CardClicked -= OnCardClicked;
				value.Dispose();
			}
			_songCards.Clear();
			base.DisposeControl();
		}
	}
}
