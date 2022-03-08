using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.Content;
using Blish_HUD.Controls;

namespace Torlando.SquadTracker
{
	public class PlayerCollection
	{
		private ObservableCollection<PlayerDisplay> _playerDisplays;

		private IDictionary<string, Player> _players;

		private ConcurrentDictionary<string, CommonFields.Player> _arcPlayersInSquad;

		private Panel _activePlayerPanel;

		private Panel _formerPlayerPanel;

		public PlayerCollection(ConcurrentDictionary<string, CommonFields.Player> arcPlayersInSquad, Panel activePlayerPanel, Panel formerPlayerPanel)
		{
			_playerDisplays = new ObservableCollection<PlayerDisplay>();
			_players = new ConcurrentDictionary<string, Player>();
			_arcPlayersInSquad = arcPlayersInSquad;
			_activePlayerPanel = activePlayerPanel;
			_formerPlayerPanel = formerPlayerPanel;
		}

		public void AddPlayer(CommonFields.Player arcPlayer, Func<uint, uint, AsyncTexture2D> iconGetter, ObservableCollection<Role> availableRoles)
		{
			if (_players.TryGetValue(arcPlayer.AccountName, out var existingPlayer))
			{
				TryGetPlayer(arcPlayer, out var playerDisplay);
				if (playerDisplay != null && playerDisplay.IsFormerSquadMember)
				{
					playerDisplay.MoveFormerSquadMemberToActivePanel();
				}
				if (arcPlayer.CharacterName != existingPlayer.CharacterName)
				{
					Player newCharacter = new Player(arcPlayer, existingPlayer);
					playerDisplay.UpdateCharacter(newCharacter);
					_players[arcPlayer.AccountName] = newCharacter;
				}
			}
			else
			{
				Player player = new Player(arcPlayer);
				_players.Add(player.AccountName, player);
				_playerDisplays.Add(new PlayerDisplay(_activePlayerPanel, _formerPlayerPanel, player, iconGetter, availableRoles));
			}
		}

		public void UpdatePlayerSpecialization(string characterName, uint newSpec)
		{
			if (Specialization.EliteCodes.Contains((int)newSpec))
			{
				Player player2 = _players.Values.Concat(_players.ToList().SelectMany((KeyValuePair<string, Player> player) => player.Value.PreviouslyPlayedCharacters)).FirstOrDefault((Player player) => player.CharacterName == characterName);
				if (player2 != null && player2.CurrentSpecialization != newSpec)
				{
					player2.CurrentSpecialization = newSpec;
				}
			}
		}

		public void RemovePlayerFromActivePanel(CommonFields.Player arcPlayer)
		{
			for (int retries = 3; retries > 0; retries--)
			{
				if (TryGetPlayer(arcPlayer, out var playerToRemove))
				{
					playerToRemove.RemovePlayerFromActivePanel();
					break;
				}
				Thread.Sleep(3000);
			}
		}

		public void ClearFormerPlayers()
		{
			foreach (PlayerDisplay player in _playerDisplays)
			{
				if (player.IsFormerSquadMember)
				{
					player.DisposeDetailsButton();
				}
			}
		}

		private CommonFields.Player GetArcPlayer(string characterName)
		{
			return _arcPlayersInSquad.First((KeyValuePair<string, CommonFields.Player> x) => x.Value.CharacterName.Equals(characterName)).Value;
		}

		private bool TryGetPlayer(CommonFields.Player arcPlayer, out PlayerDisplay playerDisplay)
		{
			playerDisplay = _playerDisplays.FirstOrDefault((PlayerDisplay x) => x.AccountName.Equals(arcPlayer.AccountName));
			return playerDisplay != null;
		}
	}
}
