using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Controls.Shared;
using Neokain.GW2.AllianceManager.Services;
using Neokain.GW2.AllianceManager.Utils;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Accounts;
using Neokain.GW2.WebClient.Models.Alliances;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Windows
{
	public class AllianceBroadcastWindow : StandardWindow
	{
		private const int WINDOW_WIDTH = 350;

		private const int TITLE_BAR_HEIGHT = 40;

		private const int CONTENT_HEIGHT_COLLAPSED = 150;

		private const int CONTENT_HEIGHT_COLLAPSED_NO_DROPDOWN = 120;

		private const int CONTENT_HEIGHT_EXPANDED = 280;

		private const int PADDING = 10;

		private const int MAX_HISTORY = 20;

		private const int HISTORY_ITEM_HEIGHT = 25;

		private const int MAX_VISIBLE_HISTORY = 5;

		private readonly Module _module;

		private readonly Gw2WebClient _webClient;

		private List<AllianceMembershipDto> _allianceMemberships;

		private List<GuildMembershipDto> _allianceGuilds = new List<GuildMembershipDto>();

		private AllianceMembershipDto _selectedAlliance;

		private Guid _accountId;

		private bool _isLoadingGuilds;

		private Label _allianceLabel;

		private Dropdown _allianceDropdown;

		private AutoHeightTextBox _textBox;

		private Label _validationLabel;

		private StandardButton _sendButton;

		private StandardButton _historyButton;

		private FlowPanel _historyPanel;

		private Label _guildStatusLabel;

		private bool _historyExpanded;

		private bool _isSending;

		private bool _showDropdown;

		private int _textBoxHeight = 35;

		public AllianceBroadcastWindow(Module module, Gw2WebClient webClient, List<AllianceMembershipDto> allianceMemberships)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 350, 170), new Rectangle(10, 40, 330, 120))
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			_module = module ?? throw new ArgumentNullException("module");
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			_allianceMemberships = allianceMemberships ?? new List<AllianceMembershipDto>();
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Alliance Broadcast");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("Neokain_GW2_AllianceManager_allianceBroadcastWindow");
			((Control)this).set_Top(100);
			((Control)this).set_Left(100);
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(false);
			_showDropdown = _allianceMemberships.Count > 1;
			UpdateWindowSize();
			CreateControls();
			if (_allianceMemberships.Count == 1)
			{
				_selectedAlliance = _allianceMemberships[0];
				LoadAllianceGuildsAsync();
			}
		}

		private void UpdateWindowSize()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			int contentHeight = 0;
			if (_showDropdown)
			{
				contentHeight += 30;
			}
			contentHeight += _textBoxHeight + 5;
			contentHeight += 30;
			contentHeight += 25;
			contentHeight += 30;
			if (_historyExpanded)
			{
				contentHeight += 135;
			}
			int windowHeight = 40 + contentHeight + 10;
			((Control)this).set_Size(new Point(350, windowHeight));
			((Container)this).set_ContentRegion(new Rectangle(10, 40, 330, contentHeight));
		}

		private void CreateControls()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Expected O, but got Unknown
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Expected O, but got Unknown
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Expected O, but got Unknown
			int contentWidth = ((Container)this).get_ContentRegion().Width;
			int currentTop = 0;
			if (_showDropdown)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)this);
				((Control)val).set_Left(0);
				((Control)val).set_Top(currentTop);
				((Control)val).set_Width(60);
				((Control)val).set_Height(24);
				val.set_Text("Alliance:");
				_allianceLabel = val;
				Dropdown val2 = new Dropdown();
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Left(65);
				((Control)val2).set_Top(currentTop);
				((Control)val2).set_Width(contentWidth - 65);
				((Control)val2).set_Height(24);
				_allianceDropdown = val2;
				foreach (AllianceMembershipDto alliance in _allianceMemberships)
				{
					_allianceDropdown.get_Items().Add("[" + alliance.AllianceTag + "] " + alliance.AllianceName);
				}
				_allianceDropdown.add_ValueChanged((EventHandler<ValueChangedEventArgs>)OnAllianceSelectionChanged);
				currentTop += 30;
			}
			AutoHeightTextBox autoHeightTextBox = new AutoHeightTextBox((BitmapFont)(object)_module.FontService.DejaVuSansDefault);
			((Control)autoHeightTextBox).set_Parent((Container)(object)this);
			((Control)autoHeightTextBox).set_Left(0);
			((Control)autoHeightTextBox).set_Top(currentTop);
			((Control)autoHeightTextBox).set_Width(contentWidth);
			autoHeightTextBox.PlaceholderText = "Enter message to broadcast...";
			_textBox = autoHeightTextBox;
			_textBox.TextChanged += OnTextChanged;
			_textBox.HeightChanged += OnTextBoxHeightChanged;
			_textBoxHeight = ((Control)_textBox).get_Height();
			currentTop += _textBoxHeight + 5;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Left(0);
			((Control)val3).set_Top(currentTop);
			((Control)val3).set_Width(contentWidth - 70);
			((Control)val3).set_Height(24);
			val3.set_Text("Length: 0 / 199");
			val3.set_TextColor(Color.get_LightGreen());
			_validationLabel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Left(contentWidth - 60);
			((Control)val4).set_Top(currentTop - 3);
			((Control)val4).set_Width(60);
			((Control)val4).set_Height(26);
			val4.set_Text("Send");
			((Control)val4).set_Enabled(false);
			_sendButton = val4;
			((Control)_sendButton).add_Click((EventHandler<MouseEventArgs>)OnSendClicked);
			currentTop += 30;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Left(0);
			((Control)val5).set_Top(currentTop);
			((Control)val5).set_Width(contentWidth);
			((Control)val5).set_Height(20);
			val5.set_Text(_showDropdown ? "Select an alliance" : "Loading guilds...");
			val5.set_TextColor(Color.get_Gray());
			val5.set_Font(GameService.Content.get_DefaultFont12());
			_guildStatusLabel = val5;
			currentTop += 25;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			((Control)val6).set_Left(0);
			((Control)val6).set_Top(currentTop);
			((Control)val6).set_Width(100);
			((Control)val6).set_Height(24);
			val6.set_Text("▶ History");
			((Control)val6).set_BasicTooltipText("Show/hide message history");
			_historyButton = val6;
			((Control)_historyButton).add_Click((EventHandler<MouseEventArgs>)OnHistoryToggleClicked);
			currentTop += 30;
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)this);
			((Control)val7).set_Left(0);
			((Control)val7).set_Top(currentTop);
			((Control)val7).set_Width(contentWidth);
			((Control)val7).set_Height(125);
			val7.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val7).set_CanScroll(true);
			((Control)val7).set_Visible(false);
			_historyPanel = val7;
			RefreshHistoryList();
		}

		private void OnAllianceSelectionChanged(object sender, ValueChangedEventArgs e)
		{
			int selectedIndex = ((_allianceDropdown.get_SelectedItem() != null) ? _allianceDropdown.get_Items().IndexOf(_allianceDropdown.get_SelectedItem()) : (-1));
			if (selectedIndex >= 0 && selectedIndex < _allianceMemberships.Count)
			{
				_selectedAlliance = _allianceMemberships[selectedIndex];
				LoadAllianceGuildsAsync();
				return;
			}
			_selectedAlliance = null;
			_allianceGuilds.Clear();
			UpdateGuildStatusLabel();
			UpdateValidation();
		}

		private async Task LoadAllianceGuildsAsync()
		{
			if (_selectedAlliance == null || _isLoadingGuilds)
			{
				return;
			}
			_isLoadingGuilds = true;
			_guildStatusLabel.set_Text("Loading guilds...");
			_guildStatusLabel.set_TextColor(Color.get_Gray());
			try
			{
				if (_accountId == Guid.Empty)
				{
					AccountDataDto account = await _webClient.GetMyAccount();
					if (account != null)
					{
						_accountId = account.Id;
					}
				}
				if (_accountId == Guid.Empty)
				{
					_guildStatusLabel.set_Text("Failed to get account");
					_guildStatusLabel.set_TextColor(Color.get_Red());
					return;
				}
				AllianceDetailDto allianceDetail = await _webClient.GetAllianceDetail(_selectedAlliance.AllianceId);
				if (allianceDetail == null || allianceDetail.GuildIds == null)
				{
					_guildStatusLabel.set_Text("Failed to load alliance");
					_guildStatusLabel.set_TextColor(Color.get_Red());
					return;
				}
				List<GuildMembershipDto> allMemberships = await _webClient.GetAccountGuildMemberships(_accountId);
				if (allMemberships == null)
				{
					_guildStatusLabel.set_Text("Failed to load guilds");
					_guildStatusLabel.set_TextColor(Color.get_Red());
					return;
				}
				HashSet<Guid> allianceGuildIds = new HashSet<Guid>(allianceDetail.GuildIds);
				_allianceGuilds = allMemberships.Where((GuildMembershipDto m) => allianceGuildIds.Contains(m.GuildId)).ToList();
				UpdateGuildStatusLabel();
			}
			catch (Exception ex)
			{
				_guildStatusLabel.set_Text("Error: " + ex.Message);
				_guildStatusLabel.set_TextColor(Color.get_Red());
				_allianceGuilds.Clear();
			}
			finally
			{
				_isLoadingGuilds = false;
				UpdateValidation();
			}
		}

		private void UpdateGuildStatusLabel()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (_selectedAlliance == null)
			{
				_guildStatusLabel.set_Text("Select an alliance");
				_guildStatusLabel.set_TextColor(Color.get_Gray());
			}
			else if (_allianceGuilds.Count == 0)
			{
				_guildStatusLabel.set_Text("No guilds found in this alliance");
				_guildStatusLabel.set_TextColor(Color.get_Yellow());
			}
			else
			{
				_guildStatusLabel.set_Text($"Will broadcast to {_allianceGuilds.Count} guild(s)");
				_guildStatusLabel.set_TextColor(Color.get_LightGreen());
			}
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			UpdateValidation();
		}

		private void OnTextBoxHeightChanged(object sender, EventArgs e)
		{
			if (_textBox != null)
			{
				int textBoxHeight = _textBoxHeight;
				_textBoxHeight = ((Control)_textBox).get_Height();
				if (textBoxHeight != _textBoxHeight)
				{
					RepositionControlsBelowTextBox();
					UpdateWindowSize();
				}
			}
		}

		private void RepositionControlsBelowTextBox()
		{
			if (_textBox != null)
			{
				int currentTop = ((Control)_textBox).get_Top() + ((Control)_textBox).get_Height() + 5;
				if (_validationLabel != null)
				{
					((Control)_validationLabel).set_Top(currentTop);
				}
				if (_sendButton != null)
				{
					((Control)_sendButton).set_Top(currentTop - 3);
				}
				currentTop += 30;
				if (_guildStatusLabel != null)
				{
					((Control)_guildStatusLabel).set_Top(currentTop);
				}
				currentTop += 25;
				if (_historyButton != null)
				{
					((Control)_historyButton).set_Top(currentTop);
				}
				currentTop += 30;
				if (_historyPanel != null)
				{
					((Control)_historyPanel).set_Top(currentTop);
				}
			}
		}

		private void UpdateValidation()
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			string obj = _textBox.Text ?? string.Empty;
			(int min, int max) tuple = TextInterpolationService.CalculateMinMaxLength(obj);
			int min = tuple.min;
			int max = tuple.max;
			bool hasValidText = !string.IsNullOrWhiteSpace(obj);
			bool hasGuilds = _allianceGuilds.Count > 0;
			bool hasAlliance = _selectedAlliance != null;
			if (min > 199)
			{
				_validationLabel.set_Text($"Too long: {min}-{max} / 199");
				_validationLabel.set_TextColor(Color.get_Red());
				((Control)_sendButton).set_Enabled(false);
			}
			else if (max > 199)
			{
				_validationLabel.set_Text($"May exceed: {min}-{max} / 199");
				_validationLabel.set_TextColor(Color.get_Yellow());
				((Control)_sendButton).set_Enabled(hasValidText && hasGuilds && hasAlliance && !_isSending && !_isLoadingGuilds);
			}
			else
			{
				_validationLabel.set_Text($"Length: {min}-{max} / 199");
				_validationLabel.set_TextColor(Color.get_LightGreen());
				((Control)_sendButton).set_Enabled(hasValidText && hasGuilds && hasAlliance && !_isSending && !_isLoadingGuilds);
			}
		}

		private async void OnSendClicked(object sender, MouseEventArgs e)
		{
			string text = _textBox.Text?.Trim();
			if (string.IsNullOrEmpty(text) || _isSending)
			{
				return;
			}
			if (_allianceGuilds.Count == 0)
			{
				ScreenNotification.ShowNotification("No alliance guilds available", (NotificationType)1, (Texture2D)null, 4);
				return;
			}
			_isSending = true;
			((Control)_sendButton).set_Enabled(false);
			_sendButton.set_Text("...");
			try
			{
				if (!(await SpamUtil.SpamToAllianceAsync(_selectedAlliance.AllianceId, text, _webClient, _module)))
				{
					ScreenNotification.ShowNotification("Failed to send broadcast", (NotificationType)2, (Texture2D)null, 4);
					return;
				}
				AddToHistory(text);
				_textBox.Text = "";
				string allianceInfo = ((_selectedAlliance != null) ? ("[" + _selectedAlliance.AllianceTag + "]") : "");
				ScreenNotification.ShowNotification("Broadcast sent to alliance " + allianceInfo, (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception ex)
			{
				ScreenNotification.ShowNotification("Broadcast failed: " + ex.Message, (NotificationType)2, (Texture2D)null, 4);
			}
			finally
			{
				_isSending = false;
				_sendButton.set_Text("Send");
				UpdateValidation();
			}
		}

		private void OnHistoryToggleClicked(object sender, MouseEventArgs e)
		{
			_historyExpanded = !_historyExpanded;
			_historyButton.set_Text(_historyExpanded ? "▼ History" : "▶ History");
			((Control)_historyPanel).set_Visible(_historyExpanded);
			UpdateWindowSize();
		}

		private void AddToHistory(string text)
		{
			List<string> history = _module.BroadcastHistory?.get_Value() ?? new List<string>();
			history.Remove(text);
			history.Insert(0, text);
			while (history.Count > 20)
			{
				history.RemoveAt(history.Count - 1);
			}
			if (_module.BroadcastHistory != null)
			{
				_module.BroadcastHistory.set_Value(history);
			}
			RefreshHistoryList();
		}

		private void RefreshHistoryList()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Expected O, but got Unknown
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			((Container)_historyPanel).ClearChildren();
			List<string> history = _module.BroadcastHistory?.get_Value() ?? new List<string>();
			if (history.Count == 0)
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)_historyPanel);
				val.set_Text("No history yet");
				val.set_TextColor(Color.get_Gray());
				((Control)val).set_Width(((Control)_historyPanel).get_Width() - 20);
				((Control)val).set_Height(25);
				return;
			}
			foreach (string item in history)
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)_historyPanel);
				((Control)val2).set_Width(((Control)_historyPanel).get_Width() - 20);
				((Control)val2).set_Height(25);
				Panel itemPanel = val2;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)itemPanel);
				((Control)val3).set_Left(5);
				((Control)val3).set_Top(3);
				((Control)val3).set_Width(((Control)itemPanel).get_Width() - 10);
				((Control)val3).set_Height(19);
				val3.set_Text(TruncateText(item, 45));
				((Control)val3).set_BasicTooltipText(item);
				string capturedItem = item;
				((Control)itemPanel).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnHistoryItemClicked(capturedItem);
				});
				((Control)val3).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					OnHistoryItemClicked(capturedItem);
				});
			}
		}

		private void OnHistoryItemClicked(string text)
		{
			_textBox.Text = text;
			UpdateValidation();
		}

		private string TruncateText(string text, int maxLength)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			if (text.Length <= maxLength)
			{
				return text;
			}
			return text.Substring(0, maxLength - 3) + "...";
		}

		public void UpdateAllianceMemberships(List<AllianceMembershipDto> allianceMemberships)
		{
			_allianceMemberships = allianceMemberships ?? new List<AllianceMembershipDto>();
			if (_allianceDropdown != null)
			{
				_allianceDropdown.get_Items().Clear();
				foreach (AllianceMembershipDto alliance in _allianceMemberships)
				{
					_allianceDropdown.get_Items().Add("[" + alliance.AllianceTag + "] " + alliance.AllianceName);
				}
			}
			if (_allianceMemberships.Count == 1)
			{
				_selectedAlliance = _allianceMemberships[0];
				LoadAllianceGuildsAsync();
			}
			else if (_selectedAlliance != null && !_allianceMemberships.Any((AllianceMembershipDto a) => a.AllianceId == _selectedAlliance.AllianceId))
			{
				_selectedAlliance = null;
				_allianceGuilds.Clear();
				UpdateGuildStatusLabel();
				UpdateValidation();
			}
		}

		protected override void DisposeControl()
		{
			if (_textBox != null)
			{
				_textBox.TextChanged -= OnTextChanged;
				_textBox.HeightChanged -= OnTextBoxHeightChanged;
			}
			if (_allianceDropdown != null)
			{
				_allianceDropdown.remove_ValueChanged((EventHandler<ValueChangedEventArgs>)OnAllianceSelectionChanged);
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
