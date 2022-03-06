using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Torlando.SquadTracker
{
	public class PlayerDisplay
	{
		private Player _player;

		private ObservableCollection<Role> _availableRoles;

		private DetailsButton _detailsButton;

		private Dropdown _dropdown1;

		private Dropdown _dropdown2;

		private Image _roleIcon1 = new Image
		{
			Size = new Point(27, 27)
		};

		private Image _roleIcon2 = new Image
		{
			Size = new Point(27, 27)
		};

		private Panel _activePlayerPanel;

		private Panel _formerPlayerPanel;

		private Func<uint, uint, AsyncTexture2D> _iconGetter;

		private const string _placeholderRoleName = "Select a role...";

		public string CharacterName => _player.CharacterName;

		public bool IsFormerSquadMember => _detailsButton.Parent?.Equals(_formerPlayerPanel) ?? false;

		public bool HasChangedCharacters => _player.HasChangedCharacters;

		public bool IsSelf => _player.IsSelf;

		public string AccountName => _player.AccountName;

		public PlayerDisplay(Panel activePlayerPanel, Panel formerPlayerPanel, Player player, Func<uint, uint, AsyncTexture2D> iconGetter, ObservableCollection<Role> availableRoles)
		{
			_activePlayerPanel = activePlayerPanel;
			_formerPlayerPanel = formerPlayerPanel;
			_player = player;
			_iconGetter = iconGetter;
			_availableRoles = availableRoles;
			CreateDetailsButtonAndDropDowns();
			_availableRoles.CollectionChanged += UpdateDropdowns;
			player.PropertyChanged += UpdateDetailsButton;
		}

		public void RemovePlayerFromActivePanel()
		{
			_detailsButton.Parent = _formerPlayerPanel;
		}

		public void MoveFormerSquadMemberToActivePanel()
		{
			_detailsButton.Parent = _activePlayerPanel;
		}

		public void MakeInvisible()
		{
			_detailsButton.Visible = false;
		}

		public void DisposeDetailsButton()
		{
			_detailsButton.Dispose();
			_player.PropertyChanged -= UpdateDetailsButton;
		}

		public void UpdateCharacter(Player player)
		{
			_player = player;
			UpdateDetailsButtonWithNewCharacter();
		}

		private void CreateDetailsButtonAndDropDowns()
		{
			CreateDetailsButtonOnly();
			_dropdown1 = CreateDropdown(_roleIcon1);
			_roleIcon1.Parent = _detailsButton;
			_dropdown2 = CreateDropdown(_roleIcon2);
			_roleIcon2.Parent = _detailsButton;
		}

		private void CreateDetailsButtonOnly()
		{
			_detailsButton = new DetailsButton
			{
				Parent = _activePlayerPanel,
				Text = _player.CharacterName + " (" + _player.AccountName + ")",
				IconSize = DetailsIconSize.Small,
				ShowVignette = true,
				HighlightType = DetailsHighlightType.LightHighlight,
				ShowToggleButton = true,
				Icon = _iconGetter(_player.Profession, _player.CurrentSpecialization),
				Size = new Point(354, 90)
			};
		}

		private void UpdateDetailsButton(object sender, PropertyChangedEventArgs e)
		{
			if (!(e.PropertyName != "CurrentSpecialization") && !IsFormerSquadMember && _player.Profession != 0)
			{
				_detailsButton.Icon = _iconGetter(_player.Profession, _player.CurrentSpecialization);
			}
		}

		private void UpdateDetailsButtonWithNewCharacter()
		{
			_detailsButton.Text = _player.CharacterName + " (" + _player.AccountName + ")";
			_detailsButton.Icon = _iconGetter(_player.Profession, _player.CurrentSpecialization);
			_detailsButton.BasicTooltipText = GetPreviousCharactersToolTipText();
		}

		private Dropdown CreateDropdown(Image roleIcon)
		{
			Dropdown dropdown = new Dropdown
			{
				Parent = _detailsButton,
				Width = 135
			};
			dropdown.Items.Add("Select a role...");
			foreach (Role role2 in _availableRoles.OrderBy((Role role) => role.Name.ToLowerInvariant()))
			{
				dropdown.Items.Add(role2.Name);
			}
			dropdown.ValueChanged += delegate
			{
				roleIcon.Texture = _availableRoles.FirstOrDefault((Role role) => role.Name.Equals(dropdown.SelectedItem))?.Icon ?? null;
			};
			return dropdown;
		}

		private void UpdateDropdowns(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateDropdown(_dropdown1);
			UpdateDropdown(_dropdown2);
		}

		private void UpdateDropdown(Dropdown dropdown)
		{
			string selectedItem = dropdown.SelectedItem;
			dropdown.Items.Clear();
			dropdown.Items.Add("Select a role...");
			foreach (Role role2 in _availableRoles.OrderBy((Role role) => role.Name.ToLowerInvariant()))
			{
				dropdown.Items.Add(role2.Name);
			}
			if (!dropdown.Items.Contains(selectedItem))
			{
				dropdown.SelectedItem = "Select a role...";
			}
		}

		private string GetPreviousCharactersToolTipText()
		{
			StringBuilder tooltip = new StringBuilder("Previously played...").AppendLine();
			foreach (Player character in _player.PreviouslyPlayedCharacters)
			{
				tooltip.AppendLine(Specialization.GetEliteName(character.CurrentSpecialization, character.Profession) + " (" + character.CharacterName + ")");
			}
			return tooltip.ToString().Trim();
		}
	}
}
