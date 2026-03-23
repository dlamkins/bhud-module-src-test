using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using SongbookOfTyria.Models;
using SongbookOfTyria.Services;
using SongbookOfTyria.UI.Controls.Containers;

namespace SongbookOfTyria.UI.Views.Helpers
{
	public class TabCardListManager : IDisposable
	{
		private readonly TextureService _textureService;

		private readonly UserSettingsService _userSettingsService;

		private readonly Dictionary<string, TabListCard> _tabCards = new Dictionary<string, TabListCard>();

		private FlowPanel _cardsPanel;

		private CancellationTokenSource _renderCts;

		private bool _isRenderingCards;

		private bool _disposed;

		public bool IsRendering => _isRenderingCards;

		public event EventHandler<MusicTab> CardClicked;

		public event EventHandler<MusicTab> FavoriteToggled;

		public event EventHandler RenderingStarted;

		public event EventHandler RenderingCompleted;

		public TabCardListManager(TextureService textureService, UserSettingsService userSettingsService)
		{
			_textureService = textureService;
			_userSettingsService = userSettingsService;
		}

		public void SetCardsPanel(FlowPanel cardsPanel)
		{
			_cardsPanel = cardsPanel;
		}

		public void RefreshCards(List<MusicTab> displayedTabs)
		{
			if (_cardsPanel != null)
			{
				CancelCurrentRender();
				_renderCts = new CancellationTokenSource();
				if (displayedTabs == null || displayedTabs.Count == 0)
				{
					((Container)_cardsPanel).ClearChildren();
					this.RenderingCompleted?.Invoke(this, EventArgs.Empty);
				}
				else
				{
					RefreshCardsInternalAsync(displayedTabs, _renderCts.Token);
				}
			}
		}

		private void CancelCurrentRender()
		{
			CancellationTokenSource oldCts = _renderCts;
			if (oldCts == null)
			{
				return;
			}
			try
			{
				oldCts.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			finally
			{
				oldCts.Dispose();
			}
		}

		private async Task RefreshCardsInternalAsync(List<MusicTab> displayedTabs, CancellationToken cancellationToken)
		{
			_isRenderingCards = true;
			bool spinnerShown = false;
			try
			{
				List<MusicTab> tabsToDisplay = displayedTabs.ToList();
				List<MusicTab> tabsNeedingCards = tabsToDisplay.Where((MusicTab t) => !_tabCards.ContainsKey(GetCardKey(t))).ToList();
				if (tabsNeedingCards.Count > 20)
				{
					spinnerShown = true;
					this.RenderingStarted?.Invoke(this, EventArgs.Empty);
				}
				await CreateCardsInBatchesAsync(tabsNeedingCards, cancellationToken);
				if (!cancellationToken.IsCancellationRequested)
				{
					ReorderCards(tabsToDisplay);
					await WaitForCardsLayoutAsync(cancellationToken);
				}
			}
			catch (OperationCanceledException)
			{
			}
			finally
			{
				_isRenderingCards = false;
				if (spinnerShown || !cancellationToken.IsCancellationRequested)
				{
					this.RenderingCompleted?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private async Task CreateCardsInBatchesAsync(List<MusicTab> tabsNeedingCards, CancellationToken cancellationToken)
		{
			for (int i = 0; i < tabsNeedingCards.Count; i += 10)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				foreach (MusicTab item in tabsNeedingCards.Skip(i).Take(10))
				{
					string key = GetCardKey(item);
					TabListCard card = new TabListCard(item, _textureService, _userSettingsService, null);
					card.CardClicked += OnCardClicked;
					card.FavoriteToggled += OnFavoriteToggled;
					_tabCards[key] = card;
				}
				if (tabsNeedingCards.Count > 10)
				{
					await Task.Delay(1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}

		private void ReorderCards(List<MusicTab> tabsToDisplay)
		{
			using (((Control)_cardsPanel).SuspendLayoutContext())
			{
				((Container)_cardsPanel).ClearChildren();
				foreach (MusicTab item in tabsToDisplay)
				{
					string key = GetCardKey(item);
					if (_tabCards.TryGetValue(key, out var card))
					{
						card.InvalidateStripeIndex();
						((Control)card).set_Parent((Container)(object)_cardsPanel);
					}
				}
			}
		}

		private async Task WaitForCardsLayoutAsync(CancellationToken cancellationToken)
		{
			int elapsedMs = 0;
			await Task.Delay(16, cancellationToken);
			for (elapsedMs += 16; elapsedMs < 3000; elapsedMs += 16)
			{
				cancellationToken.ThrowIfCancellationRequested();
				FlowPanel cardsPanel = _cardsPanel;
				Control[] children = ((cardsPanel == null) ? null : ((Container)cardsPanel).get_Children()?.ToArray());
				if (children != null && children.Length != 0)
				{
					Control[] visibleChildren = children.Where((Control c) => c.get_Visible()).ToArray();
					if (visibleChildren.Length != 0)
					{
						Control firstChild = visibleChildren.First();
						Control lastChild = visibleChildren.Last();
						if (visibleChildren.Length == 1)
						{
							if (firstChild.get_Height() > 0)
							{
								break;
							}
						}
						else if (firstChild.get_Height() > 0 && lastChild.get_Height() > 0 && lastChild.get_Top() > firstChild.get_Top())
						{
							break;
						}
					}
				}
				await Task.Delay(16, cancellationToken);
			}
		}

		private static string GetCardKey(MusicTab tab)
		{
			return $"tab:{tab.Id}";
		}

		private void OnCardClicked(object sender, MusicTab tab)
		{
			this.CardClicked?.Invoke(this, tab);
		}

		private void OnFavoriteToggled(object sender, MusicTab tab)
		{
			this.FavoriteToggled?.Invoke(this, tab);
		}

		public void ClearAllCards()
		{
			foreach (TabListCard value in _tabCards.Values)
			{
				value.CardClicked -= OnCardClicked;
				value.FavoriteToggled -= OnFavoriteToggled;
				((Control)value).Dispose();
			}
			_tabCards.Clear();
			FlowPanel cardsPanel = _cardsPanel;
			if (cardsPanel != null)
			{
				((Container)cardsPanel).ClearChildren();
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				CancelCurrentRender();
				_renderCts = null;
				ClearAllCards();
			}
		}
	}
}
