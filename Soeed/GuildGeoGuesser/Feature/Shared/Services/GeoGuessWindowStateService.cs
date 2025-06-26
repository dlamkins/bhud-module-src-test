using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Controls;
using Soeed.GuildGeoGuesser.Feature.Shared.Models.V2;
using Soeed.GuildGeoGuesser.Settings.Enums;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class GeoGuessWindowStateService : IDisposable
	{
		private int _currentPage;

		private int _totalPages;

		private int ITEMS_PER_PAGE => Service.Settings.PuzzlesPerPage.get_Value().ToInt();

		public List<Guild> Guilds { get; set; } = new List<Guild>();


		public Account Account { get; set; }

		public GeoGuessState State { get; set; }

		public Guild SelectedGuild { get; set; }

		public Puzzle DetailsModel { get; set; }

		public string SearchTerm { get; set; }

		public int CurrentPage => _currentPage;

		public int TotalPages => _totalPages;

		public int ItemsPerPage => ITEMS_PER_PAGE;

		public PuzzleSortEnum CurrentSort => Service.Settings.PuzzleSort.get_Value();

		public void SetTotalPages(int totalItems)
		{
			_totalPages = (int)Math.Ceiling((double)totalItems / (double)ITEMS_PER_PAGE);
			_currentPage = Math.Min(_currentPage, Math.Max(0, _totalPages - 1));
		}

		public void NextPage()
		{
			if (_currentPage < _totalPages - 1)
			{
				_currentPage++;
				SwapToGuildList();
			}
		}

		public void PreviousPage()
		{
			if (_currentPage > 0)
			{
				_currentPage--;
				SwapToGuildList();
			}
		}

		public void GoToPage(int page)
		{
			if (page >= 0 && page < _totalPages)
			{
				_currentPage = page;
				SwapToGuildList();
			}
		}

		public void ResetPagination()
		{
			_currentPage = 0;
			_totalPages = 0;
		}

		public GeoGuessWindowStateService()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			Account val = new Account();
			val.set_Name("Anonymous");
			Account = val;
			SelectedGuild = new Guild();
			DetailsModel = new Puzzle();
			SearchTerm = "";
			base._002Ector();
			Service.UserManager.AccountUpdated += new EventHandler<Account>(UserManager_AccountUpdated);
			Service.UserManager.GuildsUpdated += new EventHandler<IEnumerable<Guild>>(UserManager_GuildsUpdated);
			Service.Settings.PuzzleTypeFilter.add_SettingChanged((EventHandler<ValueChangedEventArgs<PuzzleTypeFilterEnum>>)ListViewFilter_SettingChanged);
			Service.Settings.CameraModeFilter.add_SettingChanged((EventHandler<ValueChangedEventArgs<CameraModeFilterEnum>>)CameraModeFilter_SettingChanged);
			Service.Settings.ShowCameraModeFilter.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCameraModeFilter_SettingChanged);
			Service.Settings.PuzzleSort.add_SettingChanged((EventHandler<ValueChangedEventArgs<PuzzleSortEnum>>)PuzzleSort_SettingChanged);
		}

		private void ListViewFilter_SettingChanged(object sender, ValueChangedEventArgs<PuzzleTypeFilterEnum> e)
		{
			ResetPagination();
			Service.GeoGuessWindow.OpenWindowState();
		}

		private void CameraModeFilter_SettingChanged(object sender, ValueChangedEventArgs<CameraModeFilterEnum> e)
		{
			ResetPagination();
			Service.GeoGuessWindow.OpenWindowState();
		}

		private void ShowCameraModeFilter_SettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			ResetPagination();
			Service.GeoGuessWindow.OpenWindowState();
		}

		private void PuzzleSort_SettingChanged(object sender, ValueChangedEventArgs<PuzzleSortEnum> e)
		{
			ResetPagination();
			Service.GeoGuessWindow.OpenWindowState();
		}

		public void SwapToHome()
		{
			State = GeoGuessState.Home;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public void SwapToGuildSelect()
		{
			State = GeoGuessState.GuildSelect;
			ResetPagination();
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public void SwapToGuildList()
		{
			SwapToGuildList(SelectedGuild);
		}

		public void SwapToGuildList(Guild newSelectedGuild)
		{
			SelectedGuild = newSelectedGuild;
			State = GeoGuessState.GuildPuzzles;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public void SwapToCreate()
		{
			if (GameService.Gw2Mumble.get_CurrentMap().get_IsCompetitiveMode())
			{
				ScreenNotification.ShowNotification("Cannot create puzzles in competitive mode maps (PvP, WvW, etc.)", (NotificationType)6, (Texture2D)null, 3);
				return;
			}
			State = GeoGuessState.PuzzleCreate;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public void SwapToEdit(Puzzle model)
		{
			DetailsModel = model;
			State = GeoGuessState.PuzzleEdit;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public void SelectModelForDetails(Puzzle model)
		{
			DetailsModel = model;
			State = GeoGuessState.PuzzleDetails;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		public async void RefreshDetailsModel()
		{
			try
			{
				Puzzle model = await Service.GeoServerWrapper.GetPuzzleInfoAsync(SelectedGuild.Id.ToString(), DetailsModel.Id.ToString());
				if (model != null)
				{
					DetailsModel = model;
					if (((Control)Service.GeoGuessWindow).get_Visible())
					{
						Service.GeoGuessWindow.OpenWindowState();
					}
				}
			}
			catch (Exception ex)
			{
				Logger.GetLogger<Module>().Error(ex, "Error refreshing details model");
			}
		}

		public void SwapToLeaderboards()
		{
			State = GeoGuessState.Leaderboards;
			if (((Control)Service.GeoGuessWindow).get_Visible())
			{
				Service.GeoGuessWindow.OpenWindowState();
			}
		}

		private void UserManager_GuildsUpdated(object sender, IEnumerable<Guild> e)
		{
			Guilds = (List<Guild>)e;
		}

		private void UserManager_AccountUpdated(object sender, Account e)
		{
			Account = e;
		}

		public void Dispose()
		{
			Service.Settings.PuzzleTypeFilter.remove_SettingChanged((EventHandler<ValueChangedEventArgs<PuzzleTypeFilterEnum>>)ListViewFilter_SettingChanged);
			Service.Settings.CameraModeFilter.remove_SettingChanged((EventHandler<ValueChangedEventArgs<CameraModeFilterEnum>>)CameraModeFilter_SettingChanged);
			Service.Settings.ShowCameraModeFilter.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)ShowCameraModeFilter_SettingChanged);
			Service.Settings.PuzzleSort.remove_SettingChanged((EventHandler<ValueChangedEventArgs<PuzzleSortEnum>>)PuzzleSort_SettingChanged);
			Service.UserManager.AccountUpdated -= new EventHandler<Account>(UserManager_AccountUpdated);
			Service.UserManager.GuildsUpdated -= new EventHandler<IEnumerable<Guild>>(UserManager_GuildsUpdated);
		}
	}
}
