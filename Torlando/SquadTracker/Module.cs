using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.ArcDps;
using Blish_HUD.ArcDps.Common;
using Blish_HUD.ArcDps.Models;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Torlando.SquadTracker
{
	[Export(typeof(Blish_HUD.Modules.Module))]
	public class Module : Blish_HUD.Modules.Module
	{
		private const string MODULE_FOLDER_NAME = "squadtracker";

		private static readonly Logger Logger = Logger.GetLogger<Module>();

		private ObservableCollection<Role> _customRoles;

		private IReadOnlyDictionary<uint, AsyncTexture2D> _professionIcons;

		private IReadOnlyDictionary<uint, AsyncTexture2D> _specializationIcons;

		private WindowTab _windowTab;

		private Panel _tabPanel;

		private FlowPanel _squadMembersPanel;

		private FlowPanel _formerSquadMembersPanel;

		private RolesPanel _rolesPanel;

		private MenuItem _squadMembersMenu;

		private MenuItem _squadRolesMenu;

		private Panel _menu;

		private StandardButton _clearFormerSquadButton;

		private List<DetailsButton> _playersDetails = new List<DetailsButton>();

		private PlayerCollection _playerCollection;

		private ConcurrentDictionary<string, CommonFields.Player> _arcPlayers;

		internal SettingsManager SettingsManager => ModuleParameters.SettingsManager;

		internal ContentsManager ContentsManager => ModuleParameters.ContentsManager;

		internal DirectoriesManager DirectoriesManager => ModuleParameters.DirectoriesManager;

		internal Gw2ApiManager Gw2ApiManager => ModuleParameters.Gw2ApiManager;

		[ImportingConstructor]
		public Module([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
		}

		protected override void Initialize()
		{
		}

		protected override async Task LoadAsync()
		{
			await LoadSpecializationIconsAsync();
			await LoadRoles();
		}

		private async Task LoadSpecializationIconsAsync()
		{
			Connection connection = new Connection();
			using Gw2Client client = new Gw2Client(connection);
			IGw2WebApiV2Client webApiClient = client.WebApi.V2;
			_professionIcons = (await webApiClient.Professions.AllAsync()).ToDictionary((Profession profession) => (uint)profession.Code, (Profession profession) => GameService.Content.GetRenderServiceTexture((string)profession.IconBig));
			_specializationIcons = (await webApiClient.Specializations.ManyAsync(Specialization.EliteCodes)).ToDictionary((Gw2Sharp.WebApi.V2.Models.Specialization spec) => (uint)spec.Id, delegate(Gw2Sharp.WebApi.V2.Models.Specialization spec)
			{
				ContentService content = GameService.Content;
				RenderUrl? professionIconBig = spec.ProfessionIconBig;
				return content.GetRenderServiceTexture(professionIconBig.HasValue ? ((string)professionIconBig.GetValueOrDefault()) : null);
			});
		}

		private async Task LoadRoles()
		{
			string directoryName2 = DirectoriesManager.RegisteredDirectories.First((string directoryName) => directoryName == "squadtracker");
			_customRoles = await RolesPersister.LoadRolesFromFileSystem(DirectoriesManager.GetFullDirectoryPath(directoryName2));
			foreach (Role role in _customRoles)
			{
				if (string.IsNullOrEmpty(role.IconPath))
				{
					continue;
				}
				try
				{
					if (role.IconPath.StartsWith("icons"))
					{
						role.Icon = ContentsManager.GetTexture(role.IconPath);
						continue;
					}
					if (!System.IO.File.Exists(role.IconPath))
					{
						return;
					}
					using FileStream textureStream = System.IO.File.Open(role.IconPath, FileMode.Open);
					if (textureStream != null)
					{
						Logger.Debug("Successfully loaded texture {dataReaderFilePath}.", role.IconPath);
						role.Icon = TextureUtil.FromStreamPremultiplied(GameService.Graphics.GraphicsDevice, textureStream);
					}
				}
				catch (Exception e)
				{
					Logger.Warn("Could not load texture " + role.IconPath + ": " + e.Message);
				}
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			_tabPanel = BuildPanel(GameService.Overlay.BlishHudWindow.ContentRegion);
			_windowTab = GameService.Overlay.BlishHudWindow.AddTab("Squad Tracker", ContentsManager.GetTexture("textures\\commandertag.png"), _tabPanel);
			GameService.ArcDps.Common.Activate();
			GameService.ArcDps.Common.PlayerAdded += PlayerAddedEvent;
			GameService.ArcDps.Common.PlayerRemoved += PlayerRemovedEvent;
			GameService.ArcDps.RawCombatEvent += RawCombatEvent;
			_playerCollection = new PlayerCollection(_arcPlayers, _squadMembersPanel, _formerSquadMembersPanel);
			base.OnModuleLoaded(e);
		}

		private void PlayerAddedEvent(CommonFields.Player player)
		{
			_arcPlayers = (ConcurrentDictionary<string, CommonFields.Player>)GameService.ArcDps.Common.PlayersInSquad;
			_playerCollection.AddPlayer(player, GetSpecializationIcon, _customRoles);
			_squadMembersPanel.BasicTooltipText = "";
		}

		private void RawCombatEvent(object sender, RawCombatEventArgs e)
		{
			Ag ag = e.CombatEvent.Src;
			_playerCollection.UpdatePlayerSpecialization(ag.Name, ag.Elite);
		}

		private void PlayerRemovedEvent(CommonFields.Player player)
		{
			_arcPlayers = (ConcurrentDictionary<string, CommonFields.Player>)GameService.ArcDps.Common.PlayersInSquad;
			_playerCollection.RemovePlayerFromActivePanel(player);
		}

		private Panel BuildPanel(Microsoft.Xna.Framework.Rectangle panelBounds)
		{
			Panel panel = new Panel
			{
				CanScroll = false,
				Size = panelBounds.Size
			};
			SetupMenu(panel);
			_rolesPanel = new RolesPanel(panel, _customRoles, _menu.Width + 10);
			_squadMembersMenu.Click += delegate
			{
				_squadMembersPanel.Visible = true;
				_formerSquadMembersPanel.Visible = true;
				_rolesPanel.MainPanel.Visible = false;
				_clearFormerSquadButton.Visible = true;
			};
			_squadRolesMenu.Click += delegate
			{
				_squadMembersPanel.Visible = false;
				_rolesPanel.MainPanel.Visible = true;
				_formerSquadMembersPanel.Visible = false;
				_clearFormerSquadButton.Visible = false;
			};
			_squadMembersPanel.BasicTooltipText = "You loaded Blish HUD after starting Guild Wars 2. Please change maps to refresh.";
			return panel;
		}

		private void SetupMenu(Panel basePanel)
		{
			_menu = new Panel
			{
				Title = "Squad Tracker Menu",
				ShowBorder = true,
				Size = Panel.MenuStandard.Size,
				Parent = basePanel
			};
			Menu menuCategories = new Menu
			{
				Size = _menu.ContentRegion.Size,
				MenuItemHeight = 40,
				Parent = _menu,
				CanSelect = true
			};
			_squadMembersMenu = menuCategories.AddMenuItem("Squad Members");
			_squadMembersMenu.Select();
			_squadRolesMenu = menuCategories.AddMenuItem("Squad Roles");
			_squadMembersPanel = new FlowPanel
			{
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(8f, 8f),
				Parent = basePanel,
				Location = new Point(_menu.Right + 10, _menu.Top),
				CanScroll = true,
				Size = new Point(basePanel.Width - _menu.Width - 5, 530),
				Title = "Current Squad Members",
				ShowBorder = true
			};
			_formerSquadMembersPanel = new FlowPanel
			{
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(8f, 8f),
				Parent = basePanel,
				Location = new Point(_menu.Right + 10, _squadMembersPanel.Bottom + 10),
				CanScroll = true,
				Size = new Point(basePanel.Width - _menu.Width - 5, 150),
				Title = "Former Squad Members",
				ShowBorder = true
			};
			_clearFormerSquadButton = new StandardButton
			{
				Parent = basePanel,
				Text = "Clear",
				Location = new Point(_formerSquadMembersPanel.Right - 135, _formerSquadMembersPanel.Top + 5)
			};
			_clearFormerSquadButton.Click += delegate
			{
				_playerCollection.ClearFormerPlayers();
			};
		}

		protected override void Update(GameTime gameTime)
		{
		}

		protected override void Unload()
		{
			Console.WriteLine("unloaded");
		}

		private AsyncTexture2D GetSpecializationIcon(uint professionCode, uint specializationCode)
		{
			if (specializationCode == 0)
			{
				return _professionIcons[professionCode];
			}
			return _specializationIcons[specializationCode];
		}

		private void SetupPlaceholderPlayers()
		{
			new DetailsButton
			{
				Parent = _squadMembersPanel,
				Text = "placeholder 1",
				IconSize = DetailsIconSize.Small,
				ShowVignette = true,
				HighlightType = DetailsHighlightType.LightHighlight,
				ShowToggleButton = true,
				Icon = GameService.Content.GetRenderServiceTexture("https://render.guildwars2.com/file/A84BD2D74D3239451E3FF4EFC0F6A146F3F6653E/1770224.png"),
				Size = new Point(354, 90)
			};
			DetailsButton placeHolderPlayer2 = new DetailsButton
			{
				Parent = _squadMembersPanel,
				Text = "placeholder 2",
				IconSize = DetailsIconSize.Small,
				ShowVignette = true,
				HighlightType = DetailsHighlightType.LightHighlight,
				ShowToggleButton = true,
				Icon = GameService.Content.GetRenderServiceTexture("https://render.guildwars2.com/file/2BE4F4AB7F69206BBDABB20CACB1DC7911B33F4E/1770212.png"),
				Size = new Point(354, 90)
			};
			Dropdown dropDown = new Dropdown
			{
				Parent = placeHolderPlayer2,
				Width = 150
			};
			Dropdown dropDown2 = new Dropdown
			{
				Parent = placeHolderPlayer2,
				Width = 150
			};
			dropDown.Items.Add("test item 1");
			dropDown.Items.Add("test item 2");
			dropDown2.Items.Add("test item 1");
			dropDown2.Items.Add("test item 2");
			_customRoles.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
			{
				if (e.Action.ToString().Equals("Remove"))
				{
					foreach (object current in e.OldItems)
					{
						if (dropDown.SelectedItem.Equals(current.ToString()))
						{
							dropDown.SelectedItem = "test item 1";
						}
						if (dropDown2.SelectedItem.Equals(current.ToString()))
						{
							dropDown2.SelectedItem = "test item 1";
						}
						dropDown.Items.Remove(current.ToString());
						dropDown2.Items.Remove(current.ToString());
					}
				}
				else
				{
					foreach (object current2 in e.NewItems)
					{
						dropDown.Items.Add(current2.ToString());
						dropDown2.Items.Add(current2.ToString());
					}
				}
			};
		}
	}
}
