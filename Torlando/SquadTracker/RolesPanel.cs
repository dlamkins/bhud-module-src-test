using System.Collections.ObjectModel;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Torlando.SquadTracker
{
	public class RolesPanel
	{
		private ObservableCollection<Role> _roles;

		public Panel MainPanel { get; private set; }

		private TextBox NewRoleTextBox { get; set; }

		private StandardButton AddButton { get; set; }

		private Label ErrorLabel { get; set; }

		public Panel RolesFlowPanel { get; private set; }

		public RolesPanel(Panel basePanel, ObservableCollection<Role> roles, int marginLeft)
		{
			_roles = roles;
			BuildPanel(basePanel, marginLeft);
		}

		private void BuildPanel(Panel basePanel, int marginLeft)
		{
			MainPanel = new Panel
			{
				Parent = basePanel,
				Location = new Point(marginLeft, basePanel.Top),
				Size = new Point(basePanel.Width - marginLeft, basePanel.Height),
				Visible = false
			};
			NewRoleTextBox = new TextBox
			{
				Parent = MainPanel,
				PlaceholderText = "Create a new role…"
			};
			AddButton = new StandardButton
			{
				Parent = MainPanel,
				Text = "Add",
				Location = new Point(NewRoleTextBox.Right + 5, MainPanel.Top),
				Size = new Point(50, NewRoleTextBox.Height)
			};
			ErrorLabel = new Label
			{
				Parent = MainPanel,
				TextColor = Color.OrangeRed,
				Location = new Point(AddButton.Right + 5, MainPanel.Top + 3),
				AutoSizeWidth = true
			};
			RolesFlowPanel = new FlowPanel
			{
				Parent = MainPanel,
				Location = new Point(NewRoleTextBox.Left, NewRoleTextBox.Bottom + 10),
				Title = "Currently Defined Roles",
				Size = new Point(MainPanel.Width, MainPanel.Height - NewRoleTextBox.Height - 10),
				CanScroll = true,
				ShowBorder = true,
				ControlPadding = new Vector2(8f, 8f)
			};
			foreach (Role role in _roles)
			{
				CreateRoleButton(role);
			}
			NewRoleTextBox.EnterPressed += delegate
			{
				AddNewRole();
			};
			AddButton.Click += delegate
			{
				AddNewRole();
			};
		}

		private void AddNewRole()
		{
			string newRoleName = NewRoleTextBox.Text.Trim();
			if (_roles.Any((Role role) => role.Name == newRoleName))
			{
				ErrorLabel.Text = "A role with this name already exists.";
				return;
			}
			ErrorLabel.Text = string.Empty;
			Role role2 = new Role(newRoleName);
			_roles.Add(role2);
			CreateRoleButton(role2);
			NewRoleTextBox.Text = string.Empty;
		}

		private void CreateRoleButton(Role role)
		{
			DetailsButton newRoleButton = new DetailsButton
			{
				Parent = RolesFlowPanel,
				Text = role.Name,
				HighlightType = DetailsHighlightType.LightHighlight,
				ShowVignette = true,
				ShowToggleButton = true,
				Icon = role.Icon,
				IconSize = DetailsIconSize.Small
			};
			StandardButton standardButton = new StandardButton();
			standardButton.Parent = newRoleButton;
			standardButton.Text = "Remove";
			standardButton.Click += delegate
			{
				_roles.Remove(role);
				RolesFlowPanel.RemoveChild(newRoleButton);
			};
		}
	}
}
