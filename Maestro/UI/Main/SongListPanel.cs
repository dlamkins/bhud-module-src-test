using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services.Playback;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Main
{
	public class SongListPanel : FlowPanel
	{
		public static class Layout
		{
			public const int Height = 280;

			public const int CardSpacing = 4;

			public const int OuterPadding = 4;

			public const int ScrollbarWidth = 12;
		}

		private readonly SongPlayer _songPlayer;

		private readonly Dictionary<Song, SongCard> _songCards = new Dictionary<Song, SongCard>();

		private readonly int _cardWidth;

		public Song SelectedSong { get; private set; }

		public event EventHandler<Song> SongSelected;

		public event EventHandler<Song> SongPlayRequested;

		public event EventHandler<Song> SongDeleteRequested;

		public event EventHandler<Song> AddToQueueRequested;

		public event EventHandler<int> CountChanged;

		public SongListPanel(SongPlayer songPlayer, int contentWidth)
			: this()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			_songPlayer = songPlayer;
			_cardWidth = contentWidth - 12 - 4;
			((Control)this).set_Size(new Point(contentWidth, 280));
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((Panel)this).set_CanScroll(true);
			((Panel)this).set_ShowBorder(true);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 4f));
			((FlowPanel)this).set_OuterControlPadding(new Vector2(4f, 4f));
		}

		public void RefreshSongs(IEnumerable<Song> songs)
		{
			((Container)this).ClearChildren();
			_songCards.Clear();
			foreach (Song song in songs)
			{
				SongCard songCard = new SongCard(song, _cardWidth);
				((Control)songCard).set_Parent((Container)(object)this);
				SongCard card = songCard;
				card.PlayClicked += OnCardPlayClicked;
				card.CardClicked += OnCardClicked;
				card.DeleteRequested += OnCardDeleteRequested;
				card.AddToQueueRequested += OnCardAddToQueueRequested;
				_songCards[song] = card;
			}
			UpdateCardStates();
			this.CountChanged?.Invoke(this, _songCards.Count);
		}

		private void OnCardPlayClicked(object sender, MouseEventArgs e)
		{
			Control focusedControl = Control.get_FocusedControl();
			TextInputBase textInput = (TextInputBase)(object)((focusedControl is TextInputBase) ? focusedControl : null);
			if (textInput != null)
			{
				textInput.set_Focused(false);
			}
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				SelectSong(card.Song);
				this.SongPlayRequested?.Invoke(this, card.Song);
			}
		}

		private void OnCardClicked(object sender, MouseEventArgs e)
		{
			Control focusedControl = Control.get_FocusedControl();
			TextInputBase textInput = (TextInputBase)(object)((focusedControl is TextInputBase) ? focusedControl : null);
			if (textInput != null)
			{
				textInput.set_Focused(false);
			}
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				SelectSong(card.Song);
			}
		}

		private void OnCardDeleteRequested(object sender, EventArgs e)
		{
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				this.SongDeleteRequested?.Invoke(this, card.Song);
			}
		}

		private void OnCardAddToQueueRequested(object sender, EventArgs e)
		{
			SongCard card = sender as SongCard;
			if (card?.Song != null)
			{
				this.AddToQueueRequested?.Invoke(this, card.Song);
			}
		}

		public void SelectSong(Song song)
		{
			SelectedSong = song;
			UpdateCardStates();
			this.SongSelected?.Invoke(this, song);
		}

		public void UpdateCardStates()
		{
			foreach (KeyValuePair<Song, SongCard> kvp in _songCards)
			{
				bool isPlaying = _songPlayer.IsPlaying && _songPlayer.CurrentSong == kvp.Key;
				bool isSelected = kvp.Key == SelectedSong;
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
				value.DeleteRequested -= OnCardDeleteRequested;
				value.AddToQueueRequested -= OnCardAddToQueueRequested;
				((Control)value).Dispose();
			}
			_songCards.Clear();
			((FlowPanel)this).DisposeControl();
		}
	}
}
