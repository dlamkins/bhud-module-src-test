using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using CinemaModule.Controllers;
using CinemaModule.Controllers.WatchParty;
using CinemaModule.Models.Location;
using CinemaModule.Models.WatchParty;
using CinemaModule.Services;
using CinemaModule.Services.YouTube;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Windows.MainSettings
{
	public class WatchPartyTabView : View
	{
		private static readonly Logger Logger = Logger.GetLogger<WatchPartyTabView>();

		private const int LeftColumnWidth = 340;

		private const int ColumnSpacing = 6;

		private const int TopPadding = 10;

		private const int SidePadding = 23;

		private const int BottomOffset = 110;

		private const int CardVerticalSpacing = 4;

		private const int HostControlsHeight = 140;

		private const int StatusSectionHeight = 100;

		private const int DescriptionSectionHeight = 90;

		private const int HelpSectionHeight = 220;

		private const int CreatePartySectionHeight = 280;

		private const string NothingPlayingText = "Nothing playing";

		private const string NoDescriptionText = "No description";

		private readonly WatchPartyController _controller;

		private readonly YouTubeService _youtubeService;

		private readonly CinemaUserSettings _userSettings;

		private readonly CinemaController _cinemaController;

		private Panel _leftColumn;

		private Panel _rightColumn;

		private Panel _browserPanel;

		private Panel _membersSection;

		private FlowPanel _roomListFlow;

		private FlowPanel _membersFlow;

		private readonly Dictionary<ListCard, WatchPartyRoom> _roomCardMap = new Dictionary<ListCard, WatchPartyRoom>();

		private readonly Dictionary<string, ListCard> _memberCards = new Dictionary<string, ListCard>();

		private FlowPanel _queuePanel;

		private Panel _queueContainer;

		private Panel _hostControlsSection;

		private Panel _statusSection;

		private Panel _createPartySection;

		private Panel _descriptionSection;

		private Panel _helpSection;

		private Label _descriptionLabel;

		private Label _serverStatusLabel;

		private GlowButton _applyLocationButton;

		private ListCard _nowPlayingCard;

		private ListCard _hostNowPlayingCard;

		private Panel _addVideoPanel;

		private string _nowPlayingVideoId;

		private StandardButton _playNextButton;

		private Dropdown _queueLimitDropdown;

		private StandardButton _addToQueueButton;

		private List<string> _lastQueueVideoIds = new List<string>();

		private List<string> _lastMembers = new List<string>();

		private StandardButton _joinRoomButton;

		private StandardButton _leaveRoomButton;

		private StandardButton _createPartyButton;

		private Label _apiWarningLabel;

		private StandardButton _resyncButton;

		private TextBox _partyNameBox;

		private WatchPartyRoom _selectedRoom;

		private bool _isViewActive;

		private readonly Dictionary<ListCard, int> _queueCardIndexMap = new Dictionary<ListCard, int>();

		private readonly Dictionary<string, string> _videoTitleCache = new Dictionary<string, string>();

		public WatchPartyTabView(WatchPartyController controller, YouTubeService youtubeService, CinemaUserSettings userSettings, CinemaController cinemaController)
			: this()
		{
			_controller = controller;
			_youtubeService = youtubeService;
			_userSettings = userSettings;
			_cinemaController = cinemaController;
		}

		protected override void Build(Container buildPanel)
		{
			_isViewActive = true;
			BuildLeftColumn(buildPanel);
			BuildRightColumn(buildPanel);
			SubscribeToEvents();
			if (_controller.IsInRoom)
			{
				ShowRoomView();
			}
			else
			{
				ShowLobbyView();
			}
			ServerStatusCheckLoopAsync();
		}

		private string FormatViewerCount(int count)
		{
			return string.Format("{0} viewer{1}", count, (count != 1) ? "s" : "");
		}

		private void BuildLeftColumn(Container parent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Size(new Point(340, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(23, 10));
			((Control)val).set_Parent(parent);
			_leftColumn = val;
			BuildRoomBrowser();
			BuildMembersSection();
			BuildTitleBarButtons();
			((Control)parent).add_Resized((EventHandler<ResizedEventArgs>)OnParentResized);
		}

		private void BuildRightColumn(Container parent)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			int rightX = 369;
			Panel val = new Panel();
			((Control)val).set_Size(new Point(((Control)parent).get_Width() - rightX - 23 - 30, ((Control)parent).get_Height() - 110));
			((Control)val).set_Location(new Point(rightX, 10));
			((Control)val).set_Parent(parent);
			_rightColumn = val;
			BuildDescriptionSection();
			BuildHostControls();
			BuildStatusSection();
			BuildCreatePartySection();
			BuildHelpSection();
			BuildQueueSection();
		}

		private void OnParentResized(object sender, ResizedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Container parent = (Container)sender;
			int newHeight = ((Control)parent).get_Height() - 110;
			((Control)_leftColumn).set_Size(new Point(340, newHeight));
			int rightX = 369;
			((Control)_rightColumn).set_Size(new Point(((Control)parent).get_Width() - rightX - 23 - 30, newHeight));
			UpdateLeftColumnLayout();
			UpdateLobbyLayout();
			UpdateQueuePosition();
		}

		private void UpdateLobbyLayout()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)_createPartySection).get_Visible())
			{
				int availableHeight = ((Control)_rightColumn).get_Height() - 220 - 5;
				int createPartyHeight = Math.Min(280, availableHeight);
				((Control)_createPartySection).set_Size(new Point(((Control)_rightColumn).get_Width(), createPartyHeight));
			}
		}

		private void BuildTitleBarButtons()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			val.set_Text("Join");
			((Control)val).set_Size(new Point(70, 26));
			((Control)val).set_Location(new Point(260, 5));
			((Control)val).set_Parent((Container)(object)_leftColumn);
			((Control)val).set_Enabled(false);
			((Control)val).set_BasicTooltipText("Join the selected party");
			_joinRoomButton = val;
			((Control)_joinRoomButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				JoinSelectedRoom();
			});
			StandardButton val2 = new StandardButton();
			val2.set_Text("Leave Party");
			((Control)val2).set_Size(new Point(100, 26));
			((Control)val2).set_Location(new Point(230, 5));
			((Control)val2).set_Parent((Container)(object)_leftColumn);
			((Control)val2).set_Visible(false);
			((Control)val2).set_BasicTooltipText("Leave the current party");
			_leaveRoomButton = val2;
			((Control)_leaveRoomButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _controller.LeaveRoomAsync();
			});
		}

		private void BuildRoomBrowser()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Parties");
			((Control)val).set_Size(new Point(340, ((Control)_leftColumn).get_Height()));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Parent((Container)(object)_leftColumn);
			val.set_CanScroll(true);
			_browserPanel = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(0f, 4f));
			((Control)val2).set_Parent((Container)(object)_browserPanel);
			_roomListFlow = val2;
		}

		private async Task RefreshRoomsAsync()
		{
			try
			{
				await _controller.RefreshRoomsAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex)
			{
				Logger.Error("Failed to refresh room list: " + ex.Message);
			}
		}

		private void PopulateRoomList(List<WatchPartyRoom> rooms)
		{
			((Container)_roomListFlow).ClearChildren();
			_roomCardMap.Clear();
			string activeRoomId = _controller.CurrentRoom?.RoomId;
			foreach (WatchPartyRoom room in rooms)
			{
				if (room != null)
				{
					string title = (room.IsPrivate ? ("[Private] " + room.RoomName) : room.RoomName);
					string subtitle = "Host: " + room.HostUsername + " | " + FormatViewerCount(room.MemberCount);
					bool isSelected = room.RoomId == activeRoomId;
					ListCard card = new ListCard((Container)(object)_roomListFlow, title, subtitle, isSelected, 400, null, null, null, showAvatar: false);
					((Control)card).add_Click((EventHandler<MouseEventArgs>)OnRoomCardClicked);
					_roomCardMap[card] = room;
				}
			}
			((Control)_roomListFlow).Invalidate();
			ResetPanelScroll(_browserPanel);
		}

		private void ShowPasswordPrompt(string roomId)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			((Control)_createPartySection).set_Visible(false);
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Enter Party Password");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 100));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			Panel prompt = val;
			TextBox val2 = new TextBox();
			((TextInputBase)val2).set_PlaceholderText("Enter password...");
			((Control)val2).set_Size(new Point(((Control)_rightColumn).get_Width() - 200, 30));
			((Control)val2).set_Location(new Point(10, 10));
			((Control)val2).set_Parent((Container)(object)prompt);
			TextBox pwBox = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Join");
			((Control)val3).set_Size(new Point(80, 30));
			((Control)val3).set_Location(new Point(((Control)_rightColumn).get_Width() - 180, 10));
			((Control)val3).set_Parent((Container)(object)prompt);
			StandardButton okButton = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Cancel");
			((Control)val4).set_Size(new Point(80, 30));
			((Control)val4).set_Location(new Point(((Control)_rightColumn).get_Width() - 90, 10));
			((Control)val4).set_Parent((Container)(object)prompt);
			((Control)okButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _controller.JoinRoomAsync(roomId, ((TextInputBase)pwBox).get_Text());
				((Control)prompt).Dispose();
				if (!_controller.IsInRoom)
				{
					((Control)_createPartySection).set_Visible(true);
				}
			});
			((Control)val4).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)prompt).Dispose();
				((Control)_createPartySection).set_Visible(true);
			});
		}

		private void BuildMembersSection()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			int availableHeight = ((Control)_leftColumn).get_Height();
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Viewers");
			((Control)val).set_Size(new Point(340, availableHeight / 2));
			((Control)val).set_Location(new Point(0, availableHeight / 2 + 5));
			((Control)val).set_Parent((Container)(object)_leftColumn);
			val.set_CanScroll(true);
			((Control)val).set_Visible(false);
			_membersSection = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			val2.set_ControlPadding(new Vector2(0f, 2f));
			((Control)val2).set_Parent((Container)(object)_membersSection);
			_membersFlow = val2;
		}

		private void UpdateLeftColumnLayout()
		{
			UpdateLeftColumnLayout(((Control)_membersSection).get_Visible());
		}

		private void UpdateLeftColumnLayout(bool showMembers)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			int availableHeight = ((Control)_leftColumn).get_Height();
			if (!showMembers)
			{
				((Control)_membersSection).set_Visible(false);
				((Control)_browserPanel).set_Size(new Point(340, availableHeight));
				((Control)_browserPanel).set_Location(new Point(0, 0));
				ResetPanelScroll(_browserPanel);
				((Control)_roomListFlow).Invalidate();
				return;
			}
			int halfHeight = availableHeight / 2;
			((Control)_browserPanel).set_Size(new Point(340, halfHeight - 3));
			((Control)_browserPanel).set_Location(new Point(0, 0));
			((Control)_membersSection).set_Size(new Point(340, halfHeight - 3));
			((Control)_membersSection).set_Location(new Point(0, halfHeight + 3));
			((Control)_membersSection).set_Visible(true);
			ResetPanelScroll(_browserPanel);
			ResetPanelScroll(_membersSection);
			((Control)_roomListFlow).Invalidate();
			((Control)_membersFlow).Invalidate();
		}

		private void ResetPanelScroll(Panel panel)
		{
			panel.set_CanScroll(false);
			panel.set_CanScroll(true);
		}

		private void PopulateMembers(IReadOnlyList<string> members)
		{
			_lastMembers = new List<string>(members);
			((Container)_membersFlow).ClearChildren();
			_memberCards.Clear();
			WatchPartyLocalState state = _controller.CurrentState;
			_ = _controller.LocalGw2Name;
			string hostName = _controller.CurrentRoom?.HostUsername;
			foreach (string member in members)
			{
				bool isMemberHost = member == hostName;
				string displayName = BuildViewerDisplayName(member, isMemberHost);
				string subtitle = FormatMemberInfo(member, state);
				ListCard card = new ListCard((Container)(object)_membersFlow, displayName, subtitle, isSelected: false, 400, null, null, null, showAvatar: false);
				_memberCards[member] = card;
			}
		}

		private string BuildViewerDisplayName(string name, bool isHost)
		{
			if (!isHost)
			{
				return name;
			}
			return "[Host] " + name;
		}

		private void RefreshMemberInfo()
		{
			RefreshMemberInfo(_controller.CurrentState);
		}

		private void RefreshMemberInfo(WatchPartyLocalState state)
		{
			foreach (KeyValuePair<string, ListCard> kvp in _memberCards)
			{
				string formatted = FormatMemberInfo(kvp.Key, state);
				kvp.Value.SetSubtitle(formatted);
			}
		}

		private string FormatMemberInfo(string username, WatchPartyLocalState state)
		{
			if (state == null)
			{
				return "—";
			}
			bool isSelf = string.Equals(username, _controller.LocalGw2Name, StringComparison.OrdinalIgnoreCase);
			string usernameLower = username.ToLowerInvariant();
			MemberState memberState = GetMemberState(isSelf, usernameLower, state);
			double time = GetMemberTime(isSelf, usernameLower, state);
			string text = $"[{memberState}]";
			string timeStr = FormatPlaybackTime(time);
			return text + " " + timeStr;
		}

		private MemberState GetMemberState(bool isSelf, string usernameLower, WatchPartyLocalState state)
		{
			if (isSelf)
			{
				return _controller.LocalMemberState;
			}
			if (state.MemberStates.TryGetValue(usernameLower, out var serverState))
			{
				return serverState;
			}
			return MemberState.Idle;
		}

		private double GetMemberTime(bool isSelf, string usernameLower, WatchPartyLocalState state)
		{
			if (isSelf)
			{
				return _controller.LocalPlaybackTime;
			}
			if (state.MemberTimes.TryGetValue(usernameLower, out var serverTime) && serverTime > 0.0)
			{
				return serverTime;
			}
			if (string.Equals(usernameLower, state.HostUsername, StringComparison.OrdinalIgnoreCase))
			{
				return state.CurrentTime;
			}
			return 0.0;
		}

		private string FormatPlaybackTime(double seconds)
		{
			TimeSpan ts = TimeSpan.FromSeconds(Math.Max(0.0, seconds));
			if (!(ts.TotalHours >= 1.0))
			{
				return $"{ts.Minutes}:{ts.Seconds:D2}";
			}
			return $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
		}

		private void BuildDescriptionSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Party Description");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 90));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			_descriptionSection = val;
			Label val2 = new Label();
			val2.set_Text("");
			((Control)val2).set_Location(new Point(10, 5));
			((Control)val2).set_Size(new Point(((Control)_rightColumn).get_Width() - 60, 50));
			val2.set_WrapText(true);
			((Control)val2).set_Parent((Container)(object)_descriptionSection);
			_descriptionLabel = val2;
			GlowButton val3 = new GlowButton();
			val3.set_Icon(CinemaModule.Instance.TextureService.GetSetScreenIcon());
			((Control)val3).set_Location(new Point(((Control)_rightColumn).get_Width() - 45, 5));
			((Control)val3).set_Parent((Container)(object)_descriptionSection);
			((Control)val3).set_Visible(false);
			((Control)val3).set_BasicTooltipText("Apply the host's shared screen position");
			_applyLocationButton = val3;
			((Control)_applyLocationButton).add_Click((EventHandler<MouseEventArgs>)OnApplyLocationClicked);
		}

		private void OnApplyLocationClicked(object sender, MouseEventArgs e)
		{
			WatchPartyLocalState state = _controller.CurrentState;
			if (state?.SharedLocation != null)
			{
				SavedLocation savedLocation = state.SharedLocation.ToSavedLocation();
				_cinemaController.ApplyLocation(savedLocation);
			}
		}

		private void UpdateApplyLocationButton()
		{
			WatchPartyLocalState state = _controller.CurrentState;
			bool hasSharedLocation = state?.SharedLocation != null;
			((Control)_applyLocationButton).set_Visible(hasSharedLocation);
			if (hasSharedLocation)
			{
				((Control)_applyLocationButton).set_BasicTooltipText("Apply screen position: " + state.SharedLocation.Name);
			}
		}

		private void BuildHostControls()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Host Controls");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 140));
			((Control)val).set_Location(new Point(0, 95));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			_hostControlsSection = val;
			FlowPanel val2 = new FlowPanel();
			val2.set_FlowDirection((ControlFlowDirection)3);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			((Control)val2).set_Parent((Container)(object)_hostControlsSection);
			FlowPanel hostNowPlayingFlow = val2;
			_hostNowPlayingCard = new ListCard((Container)(object)hostNowPlayingFlow, "Nothing playing", "", isSelected: false);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Play Next");
			((Control)val3).set_Size(new Point(90, 30));
			((Control)val3).set_Location(new Point(10, 65));
			((Control)val3).set_Parent((Container)(object)_hostControlsSection);
			((Control)val3).set_Enabled(false);
			((Control)val3).set_BasicTooltipText("Play the next video in the queue");
			_playNextButton = val3;
			((Control)_playNextButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _controller.PlayNextInQueueAsync();
			});
			Label val4 = new Label();
			val4.set_Text("Queue Limit:");
			((Control)val4).set_Location(new Point(110, 71));
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Parent((Container)(object)_hostControlsSection);
			((Control)val4).set_BasicTooltipText("Max videos per user in queue (0 = unlimited)");
			Dropdown val5 = new Dropdown();
			((Control)val5).set_Size(new Point(100, 25));
			((Control)val5).set_Location(new Point(195, 67));
			((Control)val5).set_Parent((Container)(object)_hostControlsSection);
			((Control)val5).set_BasicTooltipText("Max videos per user in queue (0 = unlimited)");
			_queueLimitDropdown = val5;
			_queueLimitDropdown.get_Items().Add("Unlimited");
			for (int i = 1; i <= 5; i++)
			{
				_queueLimitDropdown.get_Items().Add(i.ToString());
			}
			_queueLimitDropdown.set_SelectedItem("Unlimited");
			_queueLimitDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnQueueLimitChanged);
			Checkbox val6 = new Checkbox();
			val6.set_Text("Autoplay next");
			((Control)val6).set_Location(new Point(310, 71));
			val6.set_Checked(_userSettings.WatchPartyAutoplayNext);
			((Control)val6).set_Parent((Container)(object)_hostControlsSection);
			((Control)val6).set_BasicTooltipText("Automatically play the next video when current one ends");
			val6.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				_userSettings.WatchPartyAutoplayNext = e.get_Checked();
			});
		}

		private void BuildStatusSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Now Playing");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 100));
			((Control)val).set_Location(new Point(0, 95));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			_statusSection = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Resync");
			((Control)val2).set_Size(new Point(70, 26));
			((Control)val2).set_Location(new Point(((Control)_rightColumn).get_Width() - 80, 100));
			((Control)val2).set_Parent((Container)(object)_rightColumn);
			((Control)val2).set_Visible(false);
			((Control)val2).set_BasicTooltipText("Reload video (only needed when stuck)");
			_resyncButton = val2;
			((Control)_resyncButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_cinemaController.ForceWatchPartyResync();
			});
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Container)val3).set_WidthSizingMode((SizingMode)2);
			((Container)val3).set_HeightSizingMode((SizingMode)1);
			((Control)val3).set_Parent((Container)(object)_statusSection);
			FlowPanel nowPlayingFlow = val3;
			_nowPlayingCard = new ListCard((Container)(object)nowPlayingFlow, "Nothing playing", "", isSelected: false);
		}

		private void BuildCreatePartySection()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Expected O, but got Unknown
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Expected O, but got Unknown
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Expected O, but got Unknown
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("Create Party");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 280));
			((Control)val).set_Location(new Point(0, 225));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			val.set_CanScroll(true);
			_createPartySection = val;
			int inputWidth = ((Control)_rightColumn).get_Width() - 120 - 20;
			int y = 5;
			Label val2 = new Label();
			val2.set_Text("Party Name:");
			((Control)val2).set_Location(new Point(10, y + 4));
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)_createPartySection);
			TextBox val3 = new TextBox();
			((TextInputBase)val3).set_PlaceholderText("Movie Night, Chill Stream, etc.");
			((Control)val3).set_Size(new Point(inputWidth, 30));
			((Control)val3).set_Location(new Point(120, y));
			((Control)val3).set_Parent((Container)(object)_createPartySection);
			_partyNameBox = val3;
			((TextInputBase)_partyNameBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				UpdateCreatePartyButtonState();
			});
			y += 35;
			Label val4 = new Label();
			val4.set_Text("Description:");
			((Control)val4).set_Location(new Point(10, y + 4));
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Parent((Container)(object)_createPartySection);
			MultilineTextBox val5 = new MultilineTextBox();
			((TextInputBase)val5).set_PlaceholderText("Meet at Lion's Arch, /sqjoin YourName...");
			((Control)val5).set_Size(new Point(inputWidth, 50));
			((Control)val5).set_Location(new Point(120, y));
			((Control)val5).set_Parent((Container)(object)_createPartySection);
			MultilineTextBox descriptionBox = val5;
			y += 55;
			Label val6 = new Label();
			val6.set_Text("Share Location:");
			((Control)val6).set_Location(new Point(10, y + 4));
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Parent((Container)(object)_createPartySection);
			Dropdown val7 = new Dropdown();
			((Control)val7).set_Size(new Point(inputWidth, 30));
			((Control)val7).set_Location(new Point(120, y));
			((Control)val7).set_Parent((Container)(object)_createPartySection);
			Dropdown locationDropdown = val7;
			PopulateLocationDropdown(locationDropdown);
			_userSettings.SavedLocationsChanged += delegate
			{
				PopulateLocationDropdown(locationDropdown);
			};
			y += 35;
			Checkbox val8 = new Checkbox();
			val8.set_Text("Private (password required)");
			((Control)val8).set_Location(new Point(10, y));
			((Control)val8).set_Parent((Container)(object)_createPartySection);
			Checkbox privateCheckbox = val8;
			y += 25;
			TextBox val9 = new TextBox();
			((TextInputBase)val9).set_PlaceholderText("Party password...");
			((Control)val9).set_Size(new Point(inputWidth, 30));
			((Control)val9).set_Location(new Point(120, y));
			((Control)val9).set_Visible(false);
			((Control)val9).set_Parent((Container)(object)_createPartySection);
			TextBox passwordBox = val9;
			privateCheckbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate(object s, CheckChangedEvent e)
			{
				((Control)passwordBox).set_Visible(e.get_Checked());
			});
			y += 45;
			Label val10 = new Label();
			val10.set_Text("API key with Account permission required. (may take a bit to load)");
			((Control)val10).set_Location(new Point(10, y));
			val10.set_AutoSizeWidth(true);
			val10.set_TextColor(Color.get_Orange());
			((Control)val10).set_Parent((Container)(object)_createPartySection);
			((Control)val10).set_Visible(!_controller.IsApiAvailable);
			_apiWarningLabel = val10;
			StandardButton val11 = new StandardButton();
			val11.set_Text("Create Party");
			((Control)val11).set_Size(new Point(120, 30));
			((Control)val11).set_Location(new Point(10, y));
			((Control)val11).set_Parent((Container)(object)_createPartySection);
			((Control)val11).set_Visible(_controller.IsApiAvailable);
			_createPartyButton = val11;
			UpdateCreatePartyButtonState();
			((Control)_createPartyButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!string.IsNullOrWhiteSpace(((TextInputBase)_partyNameBox).get_Text()))
				{
					((Control)_createPartyButton).set_Enabled(false);
					_createPartyButton.set_Text("Creating...");
					try
					{
						WatchPartySharedLocation sharedLocation = null;
						if (locationDropdown.get_SelectedItem() != "None")
						{
							SavedLocation savedLocation = GetSelectedLocation(locationDropdown);
							if (savedLocation != null)
							{
								sharedLocation = WatchPartySharedLocation.FromSavedLocation(savedLocation);
							}
						}
						if (await _controller.CreateRoomAsync(((TextInputBase)_partyNameBox).get_Text(), privateCheckbox.get_Checked(), ((TextInputBase)passwordBox).get_Text(), sharedLocation))
						{
							if (!string.IsNullOrWhiteSpace(((TextInputBase)descriptionBox).get_Text()))
							{
								await _controller.UpdateRoomAsync(((TextInputBase)_partyNameBox).get_Text(), ((TextInputBase)descriptionBox).get_Text());
							}
							((TextInputBase)_partyNameBox).set_Text("");
							((TextInputBase)descriptionBox).set_Text("");
							((TextInputBase)passwordBox).set_Text("");
							privateCheckbox.set_Checked(false);
							locationDropdown.set_SelectedItem("None");
						}
					}
					catch (Exception ex)
					{
						Logger.Error(ex, "Create Party exception");
					}
					finally
					{
						UpdateCreatePartyButtonState();
						_createPartyButton.set_Text("Create Party");
					}
				}
			});
		}

		private void PopulateLocationDropdown(Dropdown dropdown)
		{
			dropdown.get_Items().Clear();
			dropdown.get_Items().Add("None");
			foreach (SavedLocation location in _userSettings.SavedLocations.Locations)
			{
				dropdown.get_Items().Add(location.Name);
			}
			dropdown.set_SelectedItem("None");
		}

		private SavedLocation GetSelectedLocation(Dropdown dropdown)
		{
			if (dropdown.get_SelectedItem() == "None")
			{
				return null;
			}
			return _userSettings.SavedLocations.Locations.Find((SavedLocation l) => l.Name == dropdown.get_SelectedItem());
		}

		private void BuildHelpSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			val.set_ShowBorder(true);
			val.set_Title("How It Works");
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 220));
			((Control)val).set_Location(new Point(0, 0));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			_helpSection = val;
			int y = 5;
			Label val2 = new Label();
			val2.set_Text("Server: Checking...");
			((Control)val2).set_Location(new Point(10, y));
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)_helpSection);
			_serverStatusLabel = val2;
			y += 25;
			Label val3 = new Label();
			val3.set_Text("Watch Party lets you watch YouTube videos in sync with friends.");
			((Control)val3).set_Location(new Point(10, y));
			((Control)val3).set_Size(new Point(((Control)_rightColumn).get_Width() - 30, 20));
			val3.set_WrapText(true);
			((Control)val3).set_Parent((Container)(object)_helpSection);
			y += 25;
			Label val4 = new Label();
			val4.set_Text("• Create or join a party to get started");
			((Control)val4).set_Location(new Point(10, y));
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Parent((Container)(object)_helpSection);
			y += 20;
			Label val5 = new Label();
			val5.set_Text("• Host controls playback for all (stable connection helps)");
			((Control)val5).set_Location(new Point(10, y));
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Parent((Container)(object)_helpSection);
			y += 20;
			Label val6 = new Label();
			val6.set_Text("• Viewers can add videos to the queue");
			((Control)val6).set_Location(new Point(10, y));
			val6.set_AutoSizeWidth(true);
			((Control)val6).set_Parent((Container)(object)_helpSection);
			y += 20;
			Label val7 = new Label();
			val7.set_Text("• Share a screen location so everyone can watch on the same screen");
			((Control)val7).set_Location(new Point(10, y));
			val7.set_AutoSizeWidth(true);
			((Control)val7).set_Parent((Container)(object)_helpSection);
			y += 20;
			Label val8 = new Label();
			val8.set_Text("• If desynced or video not loading, try the Resync button");
			((Control)val8).set_Location(new Point(10, y));
			val8.set_AutoSizeWidth(true);
			((Control)val8).set_Parent((Container)(object)_helpSection);
		}

		private async Task ServerStatusCheckLoopAsync()
		{
			while (_isViewActive)
			{
				try
				{
					await _controller.CheckServerStatusAsync().ConfigureAwait(continueOnCapturedContext: false);
					UpdateServerStatusLabel();
					if (_controller.ServerStatus == ServerStatus.Online || _controller.ServerStatus == ServerStatus.VersionMismatch)
					{
						await RefreshRoomsAsync().ConfigureAwait(continueOnCapturedContext: false);
						return;
					}
				}
				catch (Exception ex)
				{
					Logger.Warn("Server status check failed: " + ex.Message);
				}
				await Task.Delay(3000).ConfigureAwait(continueOnCapturedContext: false);
			}
		}

		private void UpdateServerStatusLabel()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			ServerStatus status = _controller.ServerStatus;
			string serverVersion = _controller.ServerVersion;
			string clientVersion = "2.0.0";
			switch (status)
			{
			case ServerStatus.Checking:
				_serverStatusLabel.set_Text("Server: Checking...");
				_serverStatusLabel.set_TextColor(Color.get_Yellow());
				break;
			case ServerStatus.Online:
				_serverStatusLabel.set_Text("Server: Online (v" + (serverVersion ?? clientVersion) + ")");
				_serverStatusLabel.set_TextColor(Color.get_LightGreen());
				break;
			case ServerStatus.Offline:
				_serverStatusLabel.set_Text("Server: Offline - Watch Party unavailable");
				_serverStatusLabel.set_TextColor(Color.get_Red());
				break;
			case ServerStatus.VersionMismatch:
				_serverStatusLabel.set_Text("Server: v" + serverVersion + " - Please update CinemaHUD to v" + serverVersion);
				_serverStatusLabel.set_TextColor(Color.get_Orange());
				break;
			default:
				_serverStatusLabel.set_Text("Server: Unknown");
				_serverStatusLabel.set_TextColor(Color.get_Gray());
				break;
			}
		}

		private void BuildQueueSection()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			int queueTop = 110;
			Panel val = new Panel();
			((Control)val).set_Size(new Point(((Control)_rightColumn).get_Width(), 40));
			((Control)val).set_Location(new Point(0, queueTop));
			((Control)val).set_Parent((Container)(object)_rightColumn);
			((Control)val).set_Visible(false);
			_addVideoPanel = val;
			TextBox val2 = new TextBox();
			((TextInputBase)val2).set_PlaceholderText("YouTube URL...");
			((Control)val2).set_Size(new Point(((Control)_rightColumn).get_Width() - 120, 30));
			((Control)val2).set_Location(new Point(0, 5));
			((Control)val2).set_Parent((Container)(object)_addVideoPanel);
			TextBox urlBox = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Add to Queue");
			((Control)val3).set_Size(new Point(110, 30));
			((Control)val3).set_Location(new Point(((Control)_rightColumn).get_Width() - 115, 5));
			((Control)val3).set_Parent((Container)(object)_addVideoPanel);
			_addToQueueButton = val3;
			((Control)_addToQueueButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				if (!string.IsNullOrWhiteSpace(((TextInputBase)urlBox).get_Text()))
				{
					await _controller.AddVideoAsync(((TextInputBase)urlBox).get_Text());
					((TextInputBase)urlBox).set_Text("");
				}
			});
			Panel val4 = new Panel();
			val4.set_ShowBorder(true);
			val4.set_Title("Queue");
			((Control)val4).set_Size(new Point(((Control)_rightColumn).get_Width(), ((Control)_rightColumn).get_Height() - queueTop - 50));
			((Control)val4).set_Location(new Point(0, queueTop + 45));
			((Control)val4).set_Parent((Container)(object)_rightColumn);
			val4.set_CanScroll(true);
			((Control)val4).set_Visible(false);
			_queueContainer = val4;
			FlowPanel val5 = new FlowPanel();
			val5.set_FlowDirection((ControlFlowDirection)3);
			((Container)val5).set_WidthSizingMode((SizingMode)2);
			((Container)val5).set_HeightSizingMode((SizingMode)1);
			val5.set_ControlPadding(new Vector2(0f, 4f));
			((Control)val5).set_Parent((Container)(object)_queueContainer);
			_queuePanel = val5;
		}

		private int GetQueueTop()
		{
			return (((Control)_descriptionSection).get_Visible() ? 95 : 0) + (((Control)_hostControlsSection).get_Visible() ? 150 : 110);
		}

		private void UpdateQueuePosition()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			int queueTop = GetQueueTop();
			((Control)_addVideoPanel).set_Location(new Point(0, queueTop));
			((Control)_queueContainer).set_Location(new Point(0, queueTop + 45));
			((Control)_queueContainer).set_Size(new Point(((Control)_rightColumn).get_Width(), ((Control)_rightColumn).get_Height() - queueTop - 50));
		}

		private bool HasQueueChanged(IReadOnlyList<QueueItem> queue)
		{
			if (queue.Count != _lastQueueVideoIds.Count)
			{
				return true;
			}
			for (int i = 0; i < queue.Count; i++)
			{
				if (queue[i].VideoId != _lastQueueVideoIds[i])
				{
					return true;
				}
			}
			return false;
		}

		private bool HasMembersChanged(IReadOnlyList<string> members)
		{
			if (members.Count != _lastMembers.Count)
			{
				return true;
			}
			for (int i = 0; i < members.Count; i++)
			{
				if (members[i] != _lastMembers[i])
				{
					return true;
				}
			}
			return false;
		}

		private void PopulateQueue(IReadOnlyList<QueueItem> queue)
		{
			_lastQueueVideoIds = new List<string>();
			foreach (QueueItem q in queue)
			{
				_lastQueueVideoIds.Add(q.VideoId);
			}
			((Container)_queuePanel).ClearChildren();
			_queueCardIndexMap.Clear();
			((Control)_playNextButton).set_Enabled(queue.Count > 0);
			bool isHost = _controller.IsHost;
			int queueCount = queue.Count;
			for (int i = 0; i < queueCount; i++)
			{
				QueueItem video = queue[i];
				int index = i;
				List<ListCardButton> buttons = (isHost ? CreateQueueItemButtons(index, queueCount) : null);
				string cachedTitle;
				bool hasCachedTitle = _videoTitleCache.TryGetValue(video.VideoId, out cachedTitle);
				ListCard card = new ListCard((Container)(object)_queuePanel, hasCachedTitle ? cachedTitle : "", "Added by " + video.AddedBy, isSelected: false, 400, buttons);
				_queueCardIndexMap[card] = index;
				card.ShowLoading(!hasCachedTitle);
				LoadQueueCardInfoAsync(card, video.VideoId, hasCachedTitle);
			}
		}

		private List<ListCardButton> CreateQueueItemButtons(int index, int queueCount)
		{
			TextureService textureService = CinemaModule.Instance.TextureService;
			List<ListCardButton> buttons = new List<ListCardButton>
			{
				new ListCardButton
				{
					Text = "X",
					Width = 30,
					Tooltip = "Remove from Queue",
					OnClick = delegate
					{
						_controller.RemoveFromQueueAsync(index);
					}
				}
			};
			if (index < queueCount - 1)
			{
				buttons.Add(new ListCardButton
				{
					Icon = textureService.GetArrowDownIcon(),
					Width = 30,
					Tooltip = "Move Down",
					OnClick = delegate
					{
						_controller.ReorderQueueAsync(index, index + 1);
					}
				});
			}
			if (index > 0)
			{
				buttons.Add(new ListCardButton
				{
					Icon = textureService.GetArrowUpIcon(),
					Width = 30,
					Tooltip = "Move Up",
					OnClick = delegate
					{
						_controller.ReorderQueueAsync(index, index - 1);
					}
				});
			}
			return buttons;
		}

		private async Task LoadQueueCardInfoAsync(ListCard card, string videoId, bool titleAlreadyCached)
		{
			if (string.IsNullOrEmpty(videoId))
			{
				card.ShowLoading(show: false);
				return;
			}
			(AsyncTexture2D, YouTubeVideoInfo) result = await FetchVideoInfoAsync(videoId, titleAlreadyCached).ConfigureAwait(continueOnCapturedContext: false);
			if (_isViewActive && _queueCardIndexMap.ContainsKey(card))
			{
				ApplyVideoInfoToCard(card, result.Item1, result.Item2);
			}
		}

		private void ShowLobbyView()
		{
			((Control)_joinRoomButton).set_Visible(true);
			((Control)_joinRoomButton).set_Enabled(false);
			((Control)_leaveRoomButton).set_Visible(false);
			_selectedRoom = null;
			((Control)_hostControlsSection).set_Visible(false);
			((Control)_statusSection).set_Visible(false);
			((Control)_resyncButton).set_Visible(false);
			((Control)_createPartySection).set_Visible(true);
			((Control)_helpSection).set_Visible(true);
			((Control)_descriptionSection).set_Visible(false);
			ResetRoomState();
			UpdateLeftColumnLayout(showMembers: false);
			UpdateLobbyLayout();
			SetRightColumnChildrenVisible(visible: false);
		}

		private void ShowRoomView()
		{
			((Control)_joinRoomButton).set_Visible(false);
			((Control)_leaveRoomButton).set_Visible(true);
			_selectedRoom = null;
			((Control)_createPartySection).set_Visible(false);
			((Control)_helpSection).set_Visible(false);
			UpdateLeftColumnLayout(showMembers: true);
			bool isHost = _controller.IsHost;
			((Control)_hostControlsSection).set_Visible(isHost);
			((Control)_statusSection).set_Visible(!isHost);
			((Control)_resyncButton).set_Visible(!isHost);
			((Control)_descriptionSection).set_Visible(true);
			SetRightColumnChildrenVisible(visible: true);
			UpdateQueuePosition();
			ResetRoomState();
			if (_controller.CurrentState != null)
			{
				PopulateQueue(_controller.CurrentState.Queue);
				UpdateNowPlaying(_controller.CurrentState);
				PopulateMembers(_controller.CurrentState.Members);
				UpdateDescription(_controller.CurrentState);
				UpdateAddToQueueButton(_controller.CurrentState);
			}
		}

		private void ResetRoomState()
		{
			_nowPlayingVideoId = null;
			_lastQueueVideoIds.Clear();
			_lastMembers.Clear();
			_queueCardIndexMap.Clear();
			_memberCards.Clear();
			ResetNowPlayingCard(_nowPlayingCard);
			ResetNowPlayingCard(_hostNowPlayingCard);
			((Control)_playNextButton).set_Enabled(false);
			_descriptionSection.set_Title("Party");
			_descriptionLabel.set_Text("No description");
			((Control)_applyLocationButton).set_Visible(false);
			((Container)_queuePanel).ClearChildren();
			((Container)_membersFlow).ClearChildren();
		}

		private void ResetNowPlayingCard(ListCard card)
		{
			card.Title = "Nothing playing";
			card.SetSubtitle("");
			card.SetAvatar(null);
			card.ShowLoading(show: false);
		}

		private void SetRightColumnChildrenVisible(bool visible)
		{
			foreach (Control child in ((Container)_rightColumn).get_Children())
			{
				Panel p = (Panel)(object)((child is Panel) ? child : null);
				if (p != null && p != _hostControlsSection && p != _statusSection && p != _createPartySection && p != _descriptionSection && p != _helpSection)
				{
					((Control)p).set_Visible(visible);
				}
			}
		}

		private void UpdateNowPlaying(WatchPartyLocalState state)
		{
			string subtitle = ((!state.HasVideo) ? "" : (state.IsPlaying ? "Playing" : "Paused"));
			_nowPlayingCard.SetSubtitle(subtitle);
			_hostNowPlayingCard.SetSubtitle(subtitle);
			if (state.HasVideo && state.CurrentVideoId != _nowPlayingVideoId)
			{
				_nowPlayingVideoId = state.CurrentVideoId;
				string cachedTitle;
				bool hasCachedTitle = _videoTitleCache.TryGetValue(state.CurrentVideoId, out cachedTitle);
				string title = (hasCachedTitle ? (cachedTitle ?? "") : "");
				_nowPlayingCard.Title = title;
				_hostNowPlayingCard.Title = title;
				_nowPlayingCard.ShowLoading(!hasCachedTitle);
				_hostNowPlayingCard.ShowLoading(!hasCachedTitle);
				LoadNowPlayingCardInfoAsync(state.CurrentVideoId, hasCachedTitle);
			}
			else if (!state.HasVideo)
			{
				_nowPlayingVideoId = null;
				_nowPlayingCard.Title = "Nothing playing";
				_nowPlayingCard.SetAvatar(null);
				_hostNowPlayingCard.Title = "Nothing playing";
				_hostNowPlayingCard.SetAvatar(null);
			}
		}

		private void UpdateDescription(WatchPartyLocalState state)
		{
			_descriptionSection.set_Title(string.IsNullOrEmpty(state.RoomName) ? "Party" : state.RoomName);
			_descriptionLabel.set_Text(string.IsNullOrEmpty(state.Description) ? "No description" : state.Description);
			UpdateApplyLocationButton();
		}

		private async Task LoadNowPlayingCardInfoAsync(string videoId, bool titleAlreadyCached)
		{
			if (string.IsNullOrEmpty(videoId))
			{
				_nowPlayingCard.ShowLoading(show: false);
				_hostNowPlayingCard.ShowLoading(show: false);
				return;
			}
			(AsyncTexture2D, YouTubeVideoInfo) result = await FetchVideoInfoAsync(videoId, titleAlreadyCached).ConfigureAwait(continueOnCapturedContext: false);
			if (_isViewActive)
			{
				ApplyVideoInfoToCard(_nowPlayingCard, result.Item1, result.Item2);
				ApplyVideoInfoToCard(_hostNowPlayingCard, result.Item1, result.Item2);
			}
		}

		private async Task<(AsyncTexture2D Thumbnail, YouTubeVideoInfo Info)> FetchVideoInfoAsync(string videoId, bool titleAlreadyCached)
		{
			TextureService textureService = CinemaModule.Instance?.TextureService;
			if (textureService == null)
			{
				return (null, null);
			}
			try
			{
				Task<AsyncTexture2D> thumbnailTask = textureService.GetYouTubeThumbnailAsync(videoId);
				Task<YouTubeVideoInfo> infoTask = (titleAlreadyCached ? Task.FromResult<YouTubeVideoInfo>(null) : _youtubeService.GetVideoInfoAsync(videoId));
				await Task.WhenAll(thumbnailTask, infoTask).ConfigureAwait(continueOnCapturedContext: false);
				YouTubeVideoInfo info = infoTask.Result;
				if (info != null)
				{
					_videoTitleCache[videoId] = info.Title;
				}
				return (thumbnailTask.Result, info);
			}
			catch (Exception ex)
			{
				Logger.Warn("Failed to fetch video info for " + videoId + ": " + ex.Message);
				return (null, null);
			}
		}

		private void ApplyVideoInfoToCard(ListCard card, AsyncTexture2D thumbnail, YouTubeVideoInfo info)
		{
			try
			{
				if (thumbnail != null)
				{
					card.SetAvatar(thumbnail);
				}
				if (info != null)
				{
					card.Title = info.Title;
				}
				card.ShowLoading(show: false);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		private void SubscribeToEvents()
		{
			_controller.RoomsUpdated += OnRoomsUpdated;
			_controller.StateChanged += OnStateChanged;
			_controller.RoomJoined += OnRoomJoined;
			_controller.RoomLeft += OnRoomLeft;
			_controller.HostStatusChanged += OnHostStatusChanged;
			_controller.ErrorOccurred += OnErrorOccurred;
			_controller.MemberBanned += OnMemberBanned;
			_controller.ApiAvailabilityChanged += OnApiAvailabilityChanged;
			_controller.ServerStatusChanged += OnServerStatusChanged;
		}

		private void OnRoomsUpdated(object sender, List<WatchPartyRoom> rooms)
		{
			PopulateRoomList(rooms);
		}

		private void OnStateChanged(object sender, WatchPartyStateArgs e)
		{
			WatchPartyLocalState state = e.State;
			if (state == null)
			{
				return;
			}
			if (e.ChangeType == WatchPartyStateChangeType.MemberTimesUpdated || e.ChangeType == WatchPartyStateChangeType.MemberStatesUpdated)
			{
				RefreshMemberInfo(state);
				return;
			}
			if (HasQueueChanged(state.Queue))
			{
				PopulateQueue(state.Queue);
			}
			UpdateNowPlaying(state);
			UpdateDescription(state);
			UpdateQueueLimitDropdown(state.MaxQueuePerUser);
			UpdateAddToQueueButton(state);
			if (HasMembersChanged(state.Members))
			{
				PopulateMembers(state.Members);
			}
			else
			{
				RefreshMemberInfo(state);
			}
			UpdateHostControlsVisibility();
		}

		private void UpdateHostControlsVisibility()
		{
			if (_controller.IsInRoom)
			{
				bool isHost = _controller.IsHost;
				((Control)_hostControlsSection).set_Visible(isHost);
				((Control)_statusSection).set_Visible(!isHost);
				((Control)_resyncButton).set_Visible(!isHost);
				UpdateQueuePosition();
			}
		}

		private void UpdateAddToQueueButton(WatchPartyLocalState state)
		{
			if (state == null || state.MaxQueuePerUser == 0)
			{
				((Control)_addToQueueButton).set_Enabled(true);
				((Control)_addToQueueButton).set_BasicTooltipText("Add a YouTube video to the queue");
				return;
			}
			string localName = _controller.LocalGw2Name;
			int userQueueCount = 0;
			foreach (QueueItem item in state.Queue)
			{
				if (string.Equals(item.AddedBy, localName, StringComparison.OrdinalIgnoreCase))
				{
					userQueueCount++;
				}
			}
			bool canAdd = userQueueCount < state.MaxQueuePerUser;
			((Control)_addToQueueButton).set_Enabled(canAdd);
			((Control)_addToQueueButton).set_BasicTooltipText(canAdd ? $"Add a YouTube video to the queue ({userQueueCount}/{state.MaxQueuePerUser})" : $"Queue limit reached ({userQueueCount}/{state.MaxQueuePerUser}).");
		}

		private void OnRoomJoined(object sender, EventArgs e)
		{
			Logger.Info("Room joined: " + _controller.CurrentRoom?.RoomName);
			ShowRoomView();
		}

		private void OnHostStatusChanged(object sender, bool isHost)
		{
			UpdateHostControlsVisibility();
			if (_controller.CurrentState != null)
			{
				PopulateQueue(_controller.CurrentState.Queue);
			}
		}

		private void OnRoomLeft(object sender, EventArgs e)
		{
			ShowLobbyView();
		}

		private void OnRoomCardClicked(object sender, MouseEventArgs e)
		{
			ListCard card = sender as ListCard;
			if (card == null || !_roomCardMap.TryGetValue(card, out var room))
			{
				return;
			}
			string activeRoomId = _controller.CurrentRoom?.RoomId;
			if (activeRoomId != null && room.RoomId == activeRoomId)
			{
				_selectedRoom = null;
				((Control)_joinRoomButton).set_Enabled(false);
				return;
			}
			foreach (KeyValuePair<ListCard, WatchPartyRoom> item in _roomCardMap)
			{
				item.Key.IsSelected = false;
			}
			card.IsSelected = true;
			_selectedRoom = room;
			((Control)_joinRoomButton).set_Enabled(_controller.IsApiAvailable);
		}

		private void JoinSelectedRoom()
		{
			if (_selectedRoom != null)
			{
				if (_selectedRoom.IsPrivate)
				{
					ShowPasswordPrompt(_selectedRoom.RoomId);
				}
				else
				{
					_controller.JoinRoomAsync(_selectedRoom.RoomId);
				}
			}
		}

		private void OnErrorOccurred(object sender, string message)
		{
			Logger.Warn("Watch party error: " + message);
		}

		private void OnMemberBanned(object sender, string username)
		{
			if (string.Equals(username, _controller.LocalGw2Name, StringComparison.OrdinalIgnoreCase))
			{
				ScreenNotification.ShowNotification("You have been kicked from the party.", (NotificationType)2, (Texture2D)null, 4);
				ShowLobbyView();
			}
			else if (HasMembersChanged(_controller.CurrentState?.Members ?? new List<string>()))
			{
				PopulateMembers(_controller.CurrentState.Members);
			}
		}

		private void OnApiAvailabilityChanged(object sender, EventArgs e)
		{
			bool isAvailable = _controller.IsApiAvailable;
			((Control)_createPartyButton).set_Visible(isAvailable);
			((Control)_apiWarningLabel).set_Visible(!isAvailable);
			UpdateCreatePartyButtonState();
			if (_selectedRoom != null)
			{
				((Control)_joinRoomButton).set_Enabled(isAvailable);
			}
		}

		private void UpdateCreatePartyButtonState()
		{
			TextBox partyNameBox = _partyNameBox;
			bool hasPartyName = !string.IsNullOrWhiteSpace((partyNameBox != null) ? ((TextInputBase)partyNameBox).get_Text() : null);
			bool apiAvailable = _controller.IsApiAvailable;
			((Control)_createPartyButton).set_Enabled(hasPartyName && apiAvailable);
			if (!apiAvailable)
			{
				((Control)_createPartyButton).set_BasicTooltipText("API key with Account permission required");
			}
			else if (!hasPartyName)
			{
				((Control)_createPartyButton).set_BasicTooltipText("Enter a party name to create a party");
			}
			else
			{
				((Control)_createPartyButton).set_BasicTooltipText("Create a new watch party");
			}
		}

		private void OnServerStatusChanged(object sender, EventArgs e)
		{
			UpdateServerStatusLabel();
		}

		private void OnQueueLimitChanged(object sender, ValueChangedEventArgs e)
		{
			if (_controller.IsHost)
			{
				int limit = ((!(e.get_CurrentValue() == "Unlimited")) ? int.Parse(e.get_CurrentValue()) : 0);
				_controller.UpdateMaxQueuePerUserAsync(limit);
			}
		}

		private void UpdateQueueLimitDropdown(int maxQueuePerUser)
		{
			_queueLimitDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnQueueLimitChanged);
			_queueLimitDropdown.set_SelectedItem((maxQueuePerUser == 0) ? "Unlimited" : maxQueuePerUser.ToString());
			_queueLimitDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnQueueLimitChanged);
		}

		protected override void Unload()
		{
			_isViewActive = false;
			_controller.RoomsUpdated -= OnRoomsUpdated;
			_controller.StateChanged -= OnStateChanged;
			_controller.RoomJoined -= OnRoomJoined;
			_controller.RoomLeft -= OnRoomLeft;
			_controller.HostStatusChanged -= OnHostStatusChanged;
			_controller.ErrorOccurred -= OnErrorOccurred;
			_controller.MemberBanned -= OnMemberBanned;
			_controller.ApiAvailabilityChanged -= OnApiAvailabilityChanged;
			_controller.ServerStatusChanged -= OnServerStatusChanged;
			foreach (ListCard key in _roomCardMap.Keys)
			{
				((Control)key).remove_Click((EventHandler<MouseEventArgs>)OnRoomCardClicked);
			}
			((View<IPresenter>)this).Unload();
		}
	}
}
