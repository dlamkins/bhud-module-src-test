using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blish_HUD.ArcDps.Common;

namespace Torlando.SquadTracker
{
	public class Player : INotifyPropertyChanged
	{
		private uint _currentSpecialization;

		public string AccountName { get; private set; }

		public bool IsSelf { get; private set; }

		public string CharacterName { get; private set; }

		public uint Profession { get; private set; }

		public bool HasChangedCharacters
		{
			get
			{
				HashSet<Player> previouslyPlayedCharacters = PreviouslyPlayedCharacters;
				if (previouslyPlayedCharacters == null)
				{
					return false;
				}
				return previouslyPlayedCharacters.Count > 0;
			}
		}

		public HashSet<Player> PreviouslyPlayedCharacters { get; set; }

		public uint CurrentSpecialization
		{
			get
			{
				return _currentSpecialization;
			}
			set
			{
				_currentSpecialization = value;
				OnPropertyChanged("CurrentSpecialization");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Player(CommonFields.Player arcPlayer, Player previousCharacter = null)
		{
			AccountName = arcPlayer.AccountName;
			IsSelf = arcPlayer.Self;
			CharacterName = arcPlayer.CharacterName;
			Profession = arcPlayer.Profession;
			_currentSpecialization = arcPlayer.Elite;
			if (previousCharacter != null)
			{
				PreviouslyPlayedCharacters = previousCharacter.PreviouslyPlayedCharacters;
				PreviouslyPlayedCharacters.Add(previousCharacter);
			}
			else
			{
				PreviouslyPlayedCharacters = new HashSet<Player>();
			}
		}

		private void OnPropertyChanged([CallerMemberName] string name = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
