using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.Models;
using Maestro.Services;
using Maestro.Settings;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Maestro.UI.Playlist
{
	public class PlaylistDrawerWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 250;

			public const int WindowHeight = 386;

			public const int ContentWidth = 220;

			public const int ContentHeight = 365;

			public const int ContentPaddingX = 15;

			public const int ButtonSpacing = 8;

			public const int ButtonWidth = 40;

			public const int ButtonHeight = 30;

			public const int CardSpacing = 4;

			public const int OuterPadding = 6;

			public const int DragHandleHalfWidth = 6;

			public const int ListTop = 6;

			public const int FooterButtonY = 329;

			public const int ListHeight = 317;
		}

		private const float ANIMATION_DURATION = 0.15f;

		private const int SLIDE_DISTANCE = 80;

		private static Texture2D _backgroundTexture;

		private readonly PlaylistService _playlistService;

		private readonly List<QueueSongCard> _cards = new List<QueueSongCard>();

		private readonly int _cardWidth;

		private StandardButton _clearButton;

		private IconButton _playButton;

		private IconButton _shuffleButton;

		private IconButton _repeatButton;

		private FlowPanel _songList;

		private QueueSongCard _draggingCard;

		private QueueSongCard _dragGhost;

		private int _dragTargetIndex = -1;

		private bool _isPlayingFromQueue;

		private bool _isAnimating;

		private float _animationProgress;

		private int _targetX;

		private Panel _instrumentOverlay;

		private Label _instrumentOverlayLabel;

		public event EventHandler PlayQueueRequested;

		public PlaylistDrawerWindow(PlaylistService playlistService)
			: this(GetBackground(), new Rectangle(0, 0, 250, 386), new Rectangle(15, 20, 220, 365))
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			_playlistService = playlistService;
			_cardWidth = 208;
			((WindowBase2)this).set_Title("");
			((WindowBase2)this).set_CanClose(true);
			((WindowBase2)this).set_CanResize(false);
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_Id("MaestroQueueDrawer_v5");
			BuildContent();
			SubscribeToEvents();
			InitFromSettings();
			RefreshCards();
		}

		public void SetQueuePlaybackMode(bool isPlaying)
		{
			_isPlayingFromQueue = isPlaying;
			UpdatePlayButtonText();
		}

		public void ShowInstrumentConfirmation(InstrumentType instrument)
		{
			_instrumentOverlayLabel.set_Text($"Equip {instrument} and press play\nin Maestro to continue");
			((Control)_instrumentOverlay).set_Visible(true);
		}

		public void HideInstrumentConfirmation()
		{
			((Control)_instrumentOverlay).set_Visible(false);
		}

		public void ShowWithAnimation(int targetX, int targetY)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			_targetX = targetX;
			((Control)this).set_Location(new Point(targetX - 80, targetY));
			_animationProgress = 0f;
			_isAnimating = true;
			((WindowBase2)this).Show();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).UpdateContainer(gameTime);
			if (_isAnimating)
			{
				_animationProgress += (float)gameTime.get_ElapsedGameTime().TotalSeconds / 0.15f;
				if (_animationProgress >= 1f)
				{
					_animationProgress = 1f;
					_isAnimating = false;
					((Control)this).set_Location(new Point(_targetX, ((Control)this).get_Location().Y));
				}
				else
				{
					float eased = EaseOutCubic(_animationProgress);
					int currentX = (int)((float)(_targetX - 80) + 80f * eased);
					((Control)this).set_Location(new Point(currentX, ((Control)this).get_Location().Y));
				}
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			if (((WindowBase2)this).get_MouseOverExitButton() && ((WindowBase2)this).get_CanClose())
			{
				((Control)this).Hide();
			}
		}

		protected override void DisposeControl()
		{
			UnsubscribeFromEvents();
			DisposeDragGhost();
			DisposeCards();
			DisposeControls();
			((WindowBase2)this).DisposeControl();
		}

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateDrawerBackground(250, 386));
		}

		private void BuildContent()
		{
			BuildSongList();
			BuildButtons();
			BuildInstrumentOverlay();
		}

		private void BuildButtons()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			int x = 18;
			IconButton iconButton = new IconButton(MaestroIcons.Shuffle, MaestroTheme.IconGlyph);
			((Control)iconButton).set_Parent((Container)(object)this);
			((Control)iconButton).set_BasicTooltipText("Shuffle: Off");
			((Control)iconButton).set_Location(new Point(x, 329));
			((Control)iconButton).set_Width(40);
			((Control)iconButton).set_Height(30);
			_shuffleButton = iconButton;
			((Control)_shuffleButton).add_Click((EventHandler<MouseEventArgs>)OnShuffleClicked);
			x += 48;
			IconButton iconButton2 = new IconButton(MaestroIcons.Repeat, MaestroTheme.IconGlyph);
			((Control)iconButton2).set_Parent((Container)(object)this);
			((Control)iconButton2).set_BasicTooltipText("Repeat: Off");
			((Control)iconButton2).set_Location(new Point(x, 329));
			((Control)iconButton2).set_Width(40);
			((Control)iconButton2).set_Height(30);
			_repeatButton = iconButton2;
			((Control)_repeatButton).add_Click((EventHandler<MouseEventArgs>)OnRepeatClicked);
			x += 48;
			IconButton iconButton3 = new IconButton(MaestroIcons.Trash, MaestroTheme.IconGlyph);
			((Control)iconButton3).set_Parent((Container)(object)this);
			((Control)iconButton3).set_BasicTooltipText("Clear queue");
			((Control)iconButton3).set_Location(new Point(x, 329));
			((Control)iconButton3).set_Width(40);
			((Control)iconButton3).set_Height(30);
			_clearButton = (StandardButton)(object)iconButton3;
			((Control)_clearButton).add_Click((EventHandler<MouseEventArgs>)OnClearClicked);
			x += 48;
			IconButton iconButton4 = new IconButton(MaestroIcons.Play, MaestroTheme.IconGlyph);
			((Control)iconButton4).set_Parent((Container)(object)this);
			((Control)iconButton4).set_BasicTooltipText("Play queue");
			((Control)iconButton4).set_Location(new Point(x, 329));
			((Control)iconButton4).set_Width(40);
			((Control)iconButton4).set_Height(30);
			_playButton = iconButton4;
			((Control)_playButton).add_Click((EventHandler<MouseEventArgs>)OnPlayClicked);
		}

		private void BuildSongList()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 6));
			((Control)val).set_Size(new Point(220, 317));
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, 4f));
			val.set_OuterControlPadding(new Vector2(6f, 6f));
			((Panel)val).set_CanScroll(true);
			((Panel)val).set_ShowBorder(true);
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			_songList = val;
		}

		private void BuildInstrumentOverlay()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 6));
			((Control)val).set_Size(new Point(220, 317));
			((Control)val).set_BackgroundColor(new Color(20, 25, 35, 240));
			((Control)val).set_ZIndex(100);
			((Control)val).set_Visible(false);
			_instrumentOverlay = val;
			int labelHeight = 60;
			int centerY = (317 - labelHeight) / 2;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_instrumentOverlay);
			val2.set_Text("");
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_AutoSizeWidth(false);
			val2.set_AutoSizeHeight(false);
			((Control)val2).set_Width(220);
			((Control)val2).set_Height(labelHeight);
			((Control)val2).set_Location(new Point(0, centerY));
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			val2.set_TextColor(new Color(200, 220, 255));
			_instrumentOverlayLabel = val2;
		}

		private void SubscribeToEvents()
		{
			_playlistService.QueueChanged += OnQueueChanged;
			_playlistService.CurrentChanged += OnCurrentChanged;
			Control.get_Input().get_Mouse().add_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			Control.get_Input().get_Mouse().add_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
		}

		private void OnQueueChanged(object sender, EventArgs e)
		{
			RefreshCards();
		}

		private void OnClearClicked(object sender, MouseEventArgs e)
		{
			_playlistService.Clear();
		}

		private void OnPlayClicked(object sender, MouseEventArgs e)
		{
			this.PlayQueueRequested?.Invoke(this, EventArgs.Empty);
		}

		private void InitFromSettings()
		{
			ModuleSettings settings = Module.Instance.Settings;
			_playlistService.Repeat = settings.Repeat.get_Value();
			_playlistService.Shuffle = settings.ShuffleEnabled.get_Value();
			UpdateRepeatVisual();
			UpdateShuffleVisual();
		}

		private void OnShuffleClicked(object sender, MouseEventArgs e)
		{
			_playlistService.Shuffle = !_playlistService.Shuffle;
			Module.Instance.Settings.ShuffleEnabled.set_Value(_playlistService.Shuffle);
			UpdateShuffleVisual();
		}

		private void OnRepeatClicked(object sender, MouseEventArgs e)
		{
			RepeatMode next = ((_playlistService.Repeat == RepeatMode.Off) ? RepeatMode.All : ((_playlistService.Repeat == RepeatMode.All) ? RepeatMode.One : RepeatMode.Off));
			_playlistService.Repeat = next;
			Module.Instance.Settings.Repeat.set_Value(next);
			UpdateRepeatVisual();
		}

		private void UpdateShuffleVisual()
		{
			bool on = _playlistService.Shuffle;
			_shuffleButton.Selected = on;
			((Control)_shuffleButton).set_BasicTooltipText(on ? "Shuffle On" : "Shuffle Off");
		}

		private void UpdateRepeatVisual()
		{
			switch (_playlistService.Repeat)
			{
			case RepeatMode.All:
				_repeatButton.Selected = true;
				((Control)_repeatButton).set_BasicTooltipText("Repeat all songs");
				break;
			case RepeatMode.One:
				_repeatButton.Selected = true;
				((Control)_repeatButton).set_BasicTooltipText("Repeat current song");
				break;
			default:
				_repeatButton.Selected = false;
				((Control)_repeatButton).set_BasicTooltipText("Repeat off");
				break;
			}
		}

		private void OnCurrentChanged(object sender, EventArgs e)
		{
			UpdateCurrentHighlight();
		}

		private void UpdateCurrentHighlight()
		{
			int currentIndex = _playlistService.CurrentIndex;
			for (int i = 0; i < _cards.Count; i++)
			{
				_cards[i].IsCurrent = i == currentIndex;
			}
		}

		private void OnCardRemoveRequested(object sender, EventArgs e)
		{
			QueueSongCard card = sender as QueueSongCard;
			if (card != null)
			{
				_playlistService.RemoveAt(card.Index);
			}
		}

		private void OnCardDragStarted(object sender, EventArgs e)
		{
			_draggingCard = sender as QueueSongCard;
			if (_draggingCard != null)
			{
				((Control)_draggingCard).set_Opacity(0.3f);
				CreateDragGhost();
			}
		}

		private void OnCardDragEnded(object sender, EventArgs e)
		{
			FinalizeDrag();
		}

		private void OnGlobalMouseReleased(object sender, MouseEventArgs e)
		{
			if (_draggingCard != null)
			{
				FinalizeDrag();
			}
		}

		private void OnGlobalMouseMoved(object sender, MouseEventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (_dragGhost != null)
			{
				Point offset = GetDragHandleOffset();
				((Control)_dragGhost).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
					.X - offset.X, Control.get_Input().get_Mouse().get_Position()
					.Y - offset.Y));
			}
		}

		private void RefreshCards()
		{
			ClearCards();
			CreateCards();
			UpdateCurrentHighlight();
			UpdateButtonStates();
		}

		private void ClearCards()
		{
			foreach (QueueSongCard card in _cards)
			{
				card.RemoveRequested -= OnCardRemoveRequested;
				card.DragStarted -= OnCardDragStarted;
				card.DragEnded -= OnCardDragEnded;
				((Control)card).Dispose();
			}
			_cards.Clear();
		}

		private void CreateCards()
		{
			IReadOnlyList<Song> queue = _playlistService.Queue;
			for (int i = 0; i < queue.Count; i++)
			{
				QueueSongCard queueSongCard = new QueueSongCard(queue[i], i, _cardWidth);
				((Control)queueSongCard).set_Parent((Container)(object)_songList);
				QueueSongCard card = queueSongCard;
				card.RemoveRequested += OnCardRemoveRequested;
				card.DragStarted += OnCardDragStarted;
				card.DragEnded += OnCardDragEnded;
				_cards.Add(card);
			}
		}

		private void UpdateButtonStates()
		{
			bool hasItems = _playlistService.HasItems;
			((Control)_clearButton).set_Enabled(hasItems);
			((Control)_playButton).set_Enabled(hasItems);
			UpdatePlayButtonText();
		}

		private void UpdatePlayButtonText()
		{
			bool isNext = _isPlayingFromQueue && _playlistService.HasItems;
			_playButton.IconTexture = (isNext ? MaestroIcons.Next : MaestroIcons.Play);
			((Control)_playButton).set_BasicTooltipText(isNext ? "Next" : "Play queue");
		}

		private static Point GetDragHandleOffset()
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			return new Point(18, 20);
		}

		private void CreateDragGhost()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			Point offset = GetDragHandleOffset();
			QueueSongCard queueSongCard = new QueueSongCard(_draggingCard.Song, _draggingCard.Index, _cardWidth);
			((Control)queueSongCard).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)queueSongCard).set_ZIndex(2147483615);
			((Control)queueSongCard).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
				.X - offset.X, Control.get_Input().get_Mouse().get_Position()
				.Y - offset.Y));
			queueSongCard.IsGhost = true;
			_dragGhost = queueSongCard;
		}

		private void FinalizeDrag()
		{
			if (_draggingCard != null)
			{
				((Control)_draggingCard).set_Opacity(1f);
				_draggingCard.EndDrag();
				UpdateDragTarget();
				ApplyDragResult();
			}
			ResetDragState();
		}

		private void ApplyDragResult()
		{
			if (_dragTargetIndex >= 0)
			{
				int fromIndex = _playlistService.IndexOf(_draggingCard.Song);
				if (fromIndex >= 0 && fromIndex != _dragTargetIndex)
				{
					_playlistService.Move(fromIndex, _dragTargetIndex);
				}
			}
		}

		private void UpdateDragTarget()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			Rectangle absoluteBounds = ((Control)_songList).get_AbsoluteBounds();
			if (!((Rectangle)(ref absoluteBounds)).Contains(Control.get_Input().get_Mouse().get_Position()))
			{
				_dragTargetIndex = -1;
				return;
			}
			int mouseY = Control.get_Input().get_Mouse().get_Position()
				.Y - ((Control)_songList).get_AbsoluteBounds().Y + ((Container)_songList).get_VerticalScrollOffset();
			int cardHeight = 44;
			_dragTargetIndex = Math.Max(0, Math.Min(_playlistService.Count - 1, mouseY / cardHeight));
		}

		private void ResetDragState()
		{
			QueueSongCard dragGhost = _dragGhost;
			if (dragGhost != null)
			{
				((Control)dragGhost).Dispose();
			}
			_dragGhost = null;
			_draggingCard = null;
			_dragTargetIndex = -1;
		}

		private static float EaseOutCubic(float t)
		{
			return 1f - (float)Math.Pow(1f - t, 3.0);
		}

		private void UnsubscribeFromEvents()
		{
			_playlistService.QueueChanged -= OnQueueChanged;
			_playlistService.CurrentChanged -= OnCurrentChanged;
			((Control)_clearButton).remove_Click((EventHandler<MouseEventArgs>)OnClearClicked);
			((Control)_playButton).remove_Click((EventHandler<MouseEventArgs>)OnPlayClicked);
			((Control)_shuffleButton).remove_Click((EventHandler<MouseEventArgs>)OnShuffleClicked);
			((Control)_repeatButton).remove_Click((EventHandler<MouseEventArgs>)OnRepeatClicked);
			Control.get_Input().get_Mouse().remove_LeftMouseButtonReleased((EventHandler<MouseEventArgs>)OnGlobalMouseReleased);
			Control.get_Input().get_Mouse().remove_MouseMoved((EventHandler<MouseEventArgs>)OnGlobalMouseMoved);
		}

		private void DisposeDragGhost()
		{
			QueueSongCard dragGhost = _dragGhost;
			if (dragGhost != null)
			{
				((Control)dragGhost).Dispose();
			}
		}

		private void DisposeCards()
		{
			foreach (QueueSongCard card in _cards)
			{
				card.RemoveRequested -= OnCardRemoveRequested;
				card.DragStarted -= OnCardDragStarted;
				card.DragEnded -= OnCardDragEnded;
				((Control)card).Dispose();
			}
			_cards.Clear();
		}

		private void DisposeControls()
		{
			IconButton shuffleButton = _shuffleButton;
			if (shuffleButton != null)
			{
				((Control)shuffleButton).Dispose();
			}
			IconButton repeatButton = _repeatButton;
			if (repeatButton != null)
			{
				((Control)repeatButton).Dispose();
			}
			StandardButton clearButton = _clearButton;
			if (clearButton != null)
			{
				((Control)clearButton).Dispose();
			}
			IconButton playButton = _playButton;
			if (playButton != null)
			{
				((Control)playButton).Dispose();
			}
			FlowPanel songList = _songList;
			if (songList != null)
			{
				((Control)songList).Dispose();
			}
			Panel instrumentOverlay = _instrumentOverlay;
			if (instrumentOverlay != null)
			{
				((Control)instrumentOverlay).Dispose();
			}
		}
	}
}
