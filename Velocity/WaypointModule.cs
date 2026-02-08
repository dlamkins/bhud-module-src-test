using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Velocity
{
	[Export(typeof(Module))]
	public class WaypointModule : Module
	{
		private static readonly Logger Logger = Logger.GetLogger<WaypointModule>();

		private CornerIcon _cornerIcon;

		private Blish_HUD.Controls.Panel _mainWindow;

		private Blish_HUD.Controls.Label _titleLabel;

		private StandardButton _closeButton;

		private Blish_HUD.Controls.TextBox _searchBox;

		private FlowPanel _resultsPanel;

		private Blish_HUD.Controls.Label _loadingLabel;

		private bool _isDragging;

		private Point _dragStart = Point.Zero;

		private float _targetOpacity;

		private float _currentOpacity;

		private bool _isOpen;

		private bool _isDataLoaded;

		private SettingEntry<KeyBinding> _toggleHotkey;

		private SettingEntry<WindowPositionType> _windowPositionSetting;

		private List<WaypointData> _waypointCache = new List<WaypointData>();

		private const int WINDOW_WIDTH = 340;

		private const int HEADER_HEIGHT = 110;

		private const int ITEM_HEIGHT = 35;

		private const int MAX_WINDOW_HEIGHT = 500;

		[ImportingConstructor]
		public WaypointModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
			: base(moduleParameters)
		{
		}

		protected override void DefineSettings(SettingCollection settings)
		{
			_toggleHotkey = settings.DefineSetting("ToggleKey", new KeyBinding(ModifierKeys.Ctrl | ModifierKeys.Shift, Microsoft.Xna.Framework.Input.Keys.F), () => "Search Hotkey", () => "Press to open search.");
			_windowPositionSetting = settings.DefineSetting("WindowPosition", WindowPositionType.Center, () => "Default Position", () => "Where the bar appears.");
			_toggleHotkey.Value.Enabled = true;
			_toggleHotkey.Value.Activated += OnHotkeyPressed;
		}

		private void OnHotkeyPressed(object sender, EventArgs e)
		{
			if (_mainWindow != null)
			{
				ToggleWindow();
			}
		}

		private void ToggleWindow()
		{
			_isOpen = !_isOpen;
			if (_isOpen)
			{
				_mainWindow.Visible = true;
				_targetOpacity = 1f;
				ApplyWindowPosition();
				_searchBox.Focused = true;
			}
			else
			{
				_targetOpacity = 0f;
			}
		}

		private void ApplyWindowPosition()
		{
			Blish_HUD.Controls.Screen screen = GameService.Graphics.SpriteScreen;
			int x = 0;
			int y = screen.Height / 2 - _mainWindow.Height / 2;
			x = _windowPositionSetting.Value switch
			{
				WindowPositionType.Left => 100, 
				WindowPositionType.Right => screen.Width - _mainWindow.Width - 100, 
				_ => screen.Width / 2 - _mainWindow.Width / 2, 
			};
			_mainWindow.Location = new Point(x, y);
		}

		protected override async Task LoadAsync()
		{
			_ = 1;
			try
			{
				IGw2WebApiV2Client client = GameService.Gw2WebApi.AnonymousConnection.Client.V2;
				int[] continents = new int[2] { 1, 2 };
				int[] array = continents;
				foreach (int contId in array)
				{
					foreach (int floorId in (await client.Continents.GetAsync(contId)).Floors)
					{
						foreach (ContinentFloorRegion value in (await client.Continents[contId].Floors.GetAsync(floorId)).Regions.Values)
						{
							foreach (ContinentFloorRegionMap value2 in value.Maps.Values)
							{
								foreach (ContinentFloorRegionMapPoi poi in value2.PointsOfInterest.Values)
								{
									if (poi.Type == PoiType.Waypoint && !_waypointCache.Any((WaypointData w) => w.Id == poi.Id))
									{
										_waypointCache.Add(new WaypointData
										{
											Name = poi.Name,
											Id = poi.Id,
											ChatLink = poi.ChatLink
										});
									}
								}
							}
						}
					}
				}
				_isDataLoaded = true;
				if (_loadingLabel != null)
				{
					_loadingLabel.Visible = false;
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed expansion waypoint load.");
			}
		}

		protected override void OnModuleLoaded(EventArgs e)
		{
			AsyncTexture2D icon = GameService.Content.DatAssetCache.GetTextureFromAssetId(156014);
			_mainWindow = new Blish_HUD.Controls.Panel
			{
				Parent = GameService.Graphics.SpriteScreen,
				Size = new Point(340, 110),
				BackgroundColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(10, 10, 15, 240),
				ShowBorder = true,
				Visible = false,
				Opacity = 0f,
				ZIndex = 40
			};
			_titleLabel = new Blish_HUD.Controls.Label
			{
				Parent = _mainWindow,
				Text = "V.E.L.O.C.I.T.Y.",
				Location = new Point(0, 15),
				Width = 340,
				Height = 40,
				HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center,
				Font = GameService.Content.DefaultFont32,
				TextColor = Microsoft.Xna.Framework.Color.LightBlue,
				StrokeText = true
			};
			_closeButton = new StandardButton
			{
				Parent = _mainWindow,
				Text = "X",
				Width = 25,
				Height = 25,
				Location = new Point(305, 5),
				BackgroundColor = Microsoft.Xna.Framework.Color.Maroon
			};
			_closeButton.Click += delegate
			{
				ToggleWindow();
			};
			_mainWindow.LeftMouseButtonPressed += delegate
			{
				if (GameService.Input.Mouse.Position.Y - _mainWindow.Location.Y < 60)
				{
					_isDragging = true;
					_dragStart = GameService.Input.Mouse.Position - _mainWindow.Location;
				}
			};
			_mainWindow.LeftMouseButtonReleased += delegate
			{
				_isDragging = false;
			};
			_searchBox = new Blish_HUD.Controls.TextBox
			{
				Parent = _mainWindow,
				PlaceholderText = "Search Waypoints...",
				Width = 300,
				Location = new Point(20, 65)
			};
			_searchBox.TextChanged += OnSearchTextChanged;
			_loadingLabel = new Blish_HUD.Controls.Label
			{
				Parent = _mainWindow,
				Text = "Syncing Expansions...",
				Location = new Point(20, 95),
				Width = 300,
				HorizontalAlignment = Blish_HUD.Controls.HorizontalAlignment.Center,
				TextColor = Microsoft.Xna.Framework.Color.Orange,
				Visible = !_isDataLoaded
			};
			_resultsPanel = new FlowPanel
			{
				Parent = _mainWindow,
				Location = new Point(30, _searchBox.Bottom + 10),
				Width = 300,
				Height = 0,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				CanScroll = true,
				ShowBorder = false,
				Visible = false
			};
			_cornerIcon = new CornerIcon
			{
				IconName = "Search Waypoints",
				Icon = icon,
				Priority = 5
			};
			_cornerIcon.Click += delegate
			{
				ToggleWindow();
			};
			base.OnModuleLoaded(e);
		}

		protected override void Update(GameTime gameTime)
		{
			if (_mainWindow != null && _mainWindow.Visible && _isDragging)
			{
				_mainWindow.Location = GameService.Input.Mouse.Position - _dragStart;
			}
			if (_mainWindow != null)
			{
				_currentOpacity = MathHelper.Lerp(_currentOpacity, _targetOpacity, 0.05f);
				_mainWindow.Opacity = _currentOpacity;
				if (_currentOpacity < 0.01f && !_isOpen)
				{
					_mainWindow.Visible = false;
				}
			}
		}

		private void OnSearchTextChanged(object sender, EventArgs e)
		{
			string query = _searchBox.Text.ToLower();
			_resultsPanel.ClearChildren();
			if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
			{
				_resultsPanel.Visible = false;
				_mainWindow.Height = 110;
				return;
			}
			List<WaypointData> matches = _waypointCache.Where((WaypointData wp) => wp.Name != null && wp.Name.ToLower().Contains(query)).Take(100).ToList();
			if (matches.Count > 0)
			{
				foreach (WaypointData wp2 in matches)
				{
					StandardButton standardButton = new StandardButton();
					standardButton.Text = wp2.Name;
					standardButton.Width = 280;
					standardButton.Parent = _resultsPanel;
					standardButton.BackgroundColor = Microsoft.Xna.Framework.Color.FromNonPremultiplied(40, 40, 40, 200);
					standardButton.Click += delegate
					{
						CopyWaypointToClipboard(wp2);
					};
				}
				int newResultsHeight = Math.Min(matches.Count * 35 + 10, 390);
				_resultsPanel.Height = newResultsHeight;
				_resultsPanel.Visible = true;
				_mainWindow.Height = 110 + newResultsHeight + 10;
			}
			else
			{
				_resultsPanel.Visible = false;
				_mainWindow.Height = 110;
			}
		}

		public void CopyWaypointToClipboard(WaypointData wp)
		{
			if (wp != null)
			{
				Clipboard.SetText(wp.ChatLink);
				ScreenNotification.ShowNotification("Linked: " + wp.Name);
				ToggleWindow();
			}
		}

		protected override void Unload()
		{
			_toggleHotkey.Value.Activated -= OnHotkeyPressed;
			_cornerIcon?.Dispose();
			_mainWindow?.Dispose();
			_waypointCache.Clear();
		}
	}
}
