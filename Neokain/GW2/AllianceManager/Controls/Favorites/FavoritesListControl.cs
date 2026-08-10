using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.AllianceManager.Repositories;
using Neokain.GW2.WebClient.Models.Spams;

namespace Neokain.GW2.AllianceManager.Controls.Favorites
{
	public class FavoritesListControl : Panel
	{
		private const int ROW_HEIGHT = 30;

		private const int PADDING = 5;

		private const int EDIT_BUTTON_HEIGHT = 30;

		private readonly ISpamFavoriteRepository _repository;

		private readonly FlowPanel _contentPanel;

		private readonly Label _loadingLabel;

		private readonly Label _emptyLabel;

		private readonly StandardButton _editButton;

		private readonly List<FavoriteRowControl> _rows = new List<FavoriteRowControl>();

		private Timer _refreshTimer;

		private bool _editMode;

		private int _currentMapId;

		public int CurrentMapId => _currentMapId;

		public event EventHandler<SpamFavoriteDto> FavoriteUseRequested;

		public event EventHandler<Guid> FavoriteRemoveRequested;

		public FavoritesListControl(ISpamFavoriteRepository repository)
			: this()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Expected O, but got Unknown
			_repository = repository ?? throw new ArgumentNullException("repository");
			((Control)this).set_Width(350);
			((Control)this).set_Height(400);
			((Panel)this).set_CanScroll(true);
			_currentMapId = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Loading favorites...");
			val.set_AutoSizeWidth(true);
			val.set_AutoSizeHeight(true);
			((Control)val).set_Left(5);
			((Control)val).set_Top(5);
			_loadingLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("No favorites yet.\nAdd spams to favorites from the spam details view.");
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Left(5);
			((Control)val2).set_Top(5);
			((Control)val2).set_Visible(false);
			_emptyLabel = val2;
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Left(0);
			((Control)val3).set_Top(0);
			((Control)val3).set_Width(((Control)this).get_Width());
			((Control)val3).set_Height(((Control)this).get_Height() - 30 - 5);
			((Panel)val3).set_CanScroll(true);
			((Control)val3).set_Visible(false);
			_contentPanel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Edit");
			((Control)val4).set_BasicTooltipText("Toggle edit mode to reorder or remove favorites");
			((Control)val4).set_Left(5);
			((Control)val4).set_Top(((Control)this).get_Height() - 30);
			((Control)val4).set_Width(60);
			((Control)val4).set_Height(24);
			_editButton = val4;
			((Control)_editButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				ToggleEditMode();
			});
			_refreshTimer = new Timer(1000.0);
			_refreshTimer.Elapsed += delegate
			{
				UpdateCooldownStatus();
			};
			_refreshTimer.Start();
			if (Module.Instance?.SpamOrchestrator != null)
			{
				Module.Instance.SpamOrchestrator.SpamStateChanged += OnSpamStateChanged;
			}
		}

		private void ToggleEditMode()
		{
			_editMode = !_editMode;
			_editButton.set_Text(_editMode ? "Done" : "Edit");
			foreach (FavoriteRowControl row in _rows)
			{
				row.SetEditMode(_editMode);
			}
		}

		public async Task LoadFavoritesAsync()
		{
			try
			{
				((Control)_loadingLabel).set_Visible(true);
				((Control)_emptyLabel).set_Visible(false);
				((Control)_contentPanel).set_Visible(false);
				ClearRows();
				List<SpamFavoriteDto> favorites = await _repository.GetFavoritesAsync();
				((Control)_loadingLabel).set_Visible(false);
				if (favorites == null || favorites.Count == 0)
				{
					((Control)_emptyLabel).set_Visible(true);
					return;
				}
				foreach (SpamFavoriteDto favorite in favorites.OrderBy((SpamFavoriteDto f) => f.DisplayOrder))
				{
					AddRowControl(favorite);
				}
				((Control)_contentPanel).set_Visible(true);
			}
			catch (Exception ex)
			{
				_loadingLabel.set_Text("Error: " + ex.Message);
			}
		}

		public void AddFavorite(SpamFavoriteDto favorite)
		{
			if (favorite != null)
			{
				((Control)_emptyLabel).set_Visible(false);
				((Control)_contentPanel).set_Visible(true);
				AddRowControl(favorite);
			}
		}

		private void AddRowControl(SpamFavoriteDto favorite)
		{
			FavoriteRowControl favoriteRowControl = new FavoriteRowControl(favorite, () => _currentMapId);
			((Control)favoriteRowControl).set_Parent((Container)(object)_contentPanel);
			((Control)favoriteRowControl).set_Width(((Control)_contentPanel).get_Width() - 20);
			FavoriteRowControl row = favoriteRowControl;
			row.UseClicked += delegate(object s, SpamFavoriteDto fav)
			{
				this.FavoriteUseRequested?.Invoke(this, fav);
			};
			row.RemoveClicked += delegate(object s, Guid id)
			{
				this.FavoriteRemoveRequested?.Invoke(this, id);
			};
			row.MoveUpClicked += delegate
			{
				OnMoveUp(row);
			};
			row.MoveDownClicked += delegate
			{
				OnMoveDown(row);
			};
			_rows.Add(row);
		}

		private async void OnMoveUp(FavoriteRowControl row)
		{
			int index = _rows.IndexOf(row);
			if (index > 0)
			{
				List<FavoriteRowControl> rows = _rows;
				int index2 = index;
				List<FavoriteRowControl> rows2 = _rows;
				int index3 = index - 1;
				FavoriteRowControl value = _rows[index - 1];
				FavoriteRowControl value2 = _rows[index];
				rows[index2] = value;
				rows2[index3] = value2;
				RebuildRowOrder();
				await SaveOrderAsync();
			}
		}

		private async void OnMoveDown(FavoriteRowControl row)
		{
			int index = _rows.IndexOf(row);
			if (index >= 0 && index < _rows.Count - 1)
			{
				List<FavoriteRowControl> rows = _rows;
				int index2 = index;
				List<FavoriteRowControl> rows2 = _rows;
				int index3 = index + 1;
				FavoriteRowControl value = _rows[index + 1];
				FavoriteRowControl value2 = _rows[index];
				rows[index2] = value;
				rows2[index3] = value2;
				RebuildRowOrder();
				await SaveOrderAsync();
			}
		}

		private void RebuildRowOrder()
		{
			foreach (FavoriteRowControl row in _rows)
			{
				((Control)row).set_Parent((Container)null);
			}
			foreach (FavoriteRowControl row2 in _rows)
			{
				((Control)row2).set_Parent((Container)(object)_contentPanel);
			}
		}

		private async Task SaveOrderAsync()
		{
			try
			{
				List<Guid> orderedIds = _rows.Select((FavoriteRowControl r) => r.Favorite.Id).ToList();
				await _repository.ReorderFavoritesAsync(new SpamFavoriteReorderDto
				{
					FavoriteIds = orderedIds
				});
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Failed to save order: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		public void RemoveFavorite(Guid favoriteId)
		{
			FavoriteRowControl row = _rows.FirstOrDefault((FavoriteRowControl r) => r.Favorite.Id == favoriteId);
			if (row != null)
			{
				_rows.Remove(row);
				((Control)row).Dispose();
			}
			if (_rows.Count == 0)
			{
				((Control)_contentPanel).set_Visible(false);
				((Control)_emptyLabel).set_Visible(true);
			}
		}

		public void UpdateFavorite(SpamFavoriteDto favorite)
		{
			_rows.FirstOrDefault((FavoriteRowControl r) => r.Favorite.Id == favorite.Id)?.UpdateFavorite(favorite);
		}

		private void UpdateCooldownStatus()
		{
			foreach (FavoriteRowControl row in _rows)
			{
				row.UpdateCooldownDisplay();
			}
		}

		private void OnMapChanged(object sender, ValueEventArgs<int> e)
		{
			_currentMapId = e.get_Value();
			UpdateCooldownStatus();
		}

		private void OnSpamStateChanged(object sender, EventArgs e)
		{
			UpdateCooldownStatus();
		}

		private void ClearRows()
		{
			foreach (FavoriteRowControl row in _rows)
			{
				((Control)row).Dispose();
			}
			_rows.Clear();
		}

		protected override void DisposeControl()
		{
			GameService.Gw2Mumble.get_CurrentMap().remove_MapChanged((EventHandler<ValueEventArgs<int>>)OnMapChanged);
			if (Module.Instance?.SpamOrchestrator != null)
			{
				Module.Instance.SpamOrchestrator.SpamStateChanged -= OnSpamStateChanged;
			}
			_refreshTimer?.Stop();
			_refreshTimer?.Dispose();
			ClearRows();
			((Panel)this).DisposeControl();
		}
	}
}
