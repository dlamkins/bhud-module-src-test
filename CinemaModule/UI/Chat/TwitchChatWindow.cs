using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Services;
using CinemaModule.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CinemaModule.UI.Chat
{
	public class TwitchChatWindow : StandardWindow
	{
		private static readonly Logger Logger = Logger.GetLogger<TwitchChatWindow>();

		private static readonly Rectangle DefaultWindowRegion = new Rectangle(0, 0, 439, 514);

		private static readonly Rectangle DefaultContentRegion = new Rectangle(10, 20, 429, 490);

		private const int MinWindowWidth = 350;

		private const int MinWindowHeight = 400;

		private const int LockButtonSize = 32;

		private const int LockButtonMargin = 8;

		private const int TitleLeftMargin = 16;

		private const int TitleBarHeight = 40;

		private const int CloseButtonWidth = 45;

		private const int TitleRightMargin = 50;

		private const int ContentPadding = 20;

		private readonly TwitchChatService _chatService;

		private readonly TwitchAuthService _authService;

		private readonly CinemaUserSettings _settings;

		private readonly AsyncTexture2D _lockIcon;

		private readonly AsyncTexture2D _lockActiveIcon;

		private TwitchChatPanel _chatPanel;

		private string _currentChannel;

		private bool _isLocked;

		private Rectangle _lockButtonBounds;

		private Point _lockedPosition;

		private string _windowTitle = "Twitch Chat";

		public string CurrentChannel => _currentChannel;

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}
			set
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				_isLocked = value;
				((WindowBase2)this).set_CanResize(!value);
				_settings.TwitchChatWindowLocked = value;
				if (value)
				{
					_lockedPosition = ((Control)this).get_Location();
				}
			}
		}

		public TwitchChatWindow(TwitchChatService chatService, TwitchAuthService authService, CinemaUserSettings settings)
			: this(CinemaModule.Instance.TextureService.GetChatBackground(), DefaultWindowRegion, DefaultContentRegion)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			_chatService = chatService;
			_authService = authService;
			_settings = settings;
			_lockIcon = CinemaModule.Instance.TextureService.GetLockIcon();
			_lockActiveIcon = CinemaModule.Instance.TextureService.GetLockActiveIcon();
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)this).set_Title("");
			((WindowBase2)this).set_Emblem((Texture2D)null);
			((WindowBase2)this).set_Id("CinemaModule_TwitchChatWindow");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(false);
			((Control)this).set_ZIndex(-9001);
			((Control)this).set_Location(new Point((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - ((Control)this).get_Width()) / 2, (((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - ((Control)this).get_Height()) / 2));
			RestoreSavedSize();
			_isLocked = _settings.TwitchChatWindowLocked;
			((WindowBase2)this).set_CanResize(!_isLocked);
			BuildWindowContent();
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnWindowResized);
			_authService.AuthStatusChanged += OnAuthStatusChanged;
		}

		private void OnAuthStatusChanged(object sender, TwitchAuthStatusEventArgs e)
		{
			_chatPanel.RefreshAuthStatus();
		}

		private void OnWindowResized(object sender, ResizedEventArgs e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Width() < 350 || ((Control)this).get_Height() < 400)
			{
				((Control)this).set_Size(new Point(Math.Max(((Control)this).get_Width(), 350), Math.Max(((Control)this).get_Height(), 400)));
			}
			_settings.TwitchChatWindowSize = ((Control)this).get_Size();
			((Control)_chatPanel).set_Size(new Point(((Container)this).get_ContentRegion().Width - 20, ((Container)this).get_ContentRegion().Height));
		}

		private void RestoreSavedSize()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Point savedSize = _settings.TwitchChatWindowSize;
			if (savedSize.X >= 350 && savedSize.Y >= 400)
			{
				((Control)this).set_Size(savedSize);
			}
		}

		private void BuildWindowContent()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			TwitchChatPanel twitchChatPanel = new TwitchChatPanel(_chatService);
			((Control)twitchChatPanel).set_Parent((Container)(object)this);
			((Control)twitchChatPanel).set_Location(Point.get_Zero());
			((Control)twitchChatPanel).set_Size(new Point(((Container)this).get_ContentRegion().Width - 20, ((Container)this).get_ContentRegion().Height));
			_chatPanel = twitchChatPanel;
			_chatPanel.MessageSent += OnChatMessageSent;
		}

		private async void OnChatMessageSent(object sender, string message)
		{
			if (_authService.IsAuthenticated)
			{
				try
				{
					await _chatService.SendMessageAsync(message);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to send chat message");
				}
			}
		}

		public async void ConnectToChannel(string channelName)
		{
			if (!string.IsNullOrWhiteSpace(channelName) && (!(_currentChannel == channelName.ToLowerInvariant()) || !_chatService.IsConnected))
			{
				_currentChannel = channelName.ToLowerInvariant();
				_settings.TwitchChatWindowChannel = _currentChannel;
				_windowTitle = "Chat - #" + _currentChannel;
				if (_authService.IsAuthenticated)
				{
					_chatService.SetCredentials(_authService.Username, _authService.AccessToken);
				}
				try
				{
					await _chatService.ConnectAsync(_currentChannel);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, "Failed to connect to channel: " + channelName);
				}
			}
		}

		public async void Disconnect()
		{
			try
			{
				await _chatService.DisconnectAsync();
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to disconnect from chat");
			}
			_currentChannel = null;
			_settings.TwitchChatWindowChannel = "";
			_windowTitle = "Twitch Chat";
		}

		public override void Show()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).Show();
			_settings.TwitchChatWindowOpen = true;
			((WindowBase2)this).set_CanResize(!_isLocked);
			if (_isLocked)
			{
				_lockedPosition = ((Control)this).get_Location();
			}
		}

		public override void Hide()
		{
			((WindowBase2)this).Hide();
			_settings.TwitchChatWindowOpen = false;
		}

		protected override void DisposeControl()
		{
			_chatPanel.MessageSent -= OnChatMessageSent;
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnWindowResized);
			_authService.AuthStatusChanged -= OnAuthStatusChanged;
			_chatService.DisconnectAsync();
			((WindowBase2)this).DisposeControl();
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).PaintAfterChildren(spriteBatch, bounds);
			Rectangle titleBounds = default(Rectangle);
			((Rectangle)(ref titleBounds))._002Ector(16, 0, ((Control)this).get_Width() - 16 - 50, 40);
			SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, _windowTitle, Control.get_Content().get_DefaultFont32(), titleBounds, Colors.ColonialWhite, false, (HorizontalAlignment)0, (VerticalAlignment)1);
			PaintLockButton(spriteBatch);
		}

		private void PaintLockButton(SpriteBatch spriteBatch)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			_lockButtonBounds = new Rectangle(((Control)this).get_AbsoluteBounds().X + ((Control)this).get_AbsoluteBounds().Width - 32 - 8 - 45, ((Control)this).get_AbsoluteBounds().Y + 8, 32, 32);
			AsyncTexture2D texture = (_isLocked ? _lockActiveIcon : _lockIcon);
			if (texture != null && texture.get_HasSwapped())
			{
				Color color = (Color)(((Rectangle)(ref _lockButtonBounds)).Contains(GameService.Input.get_Mouse().get_Position()) ? Color.get_White() : new Color(220, 220, 220));
				spriteBatch.Draw(AsyncTexture2D.op_Implicit(texture), _lockButtonBounds, color);
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Point absoluteMousePos = GameService.Input.get_Mouse().get_Position();
			if (!((Rectangle)(ref _lockButtonBounds)).Contains(absoluteMousePos))
			{
				((WindowBase2)this).OnClick(e);
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Point absoluteMousePos = GameService.Input.get_Mouse().get_Position();
			if (((Rectangle)(ref _lockButtonBounds)).Contains(absoluteMousePos))
			{
				IsLocked = !IsLocked;
				return;
			}
			if (_isLocked)
			{
				bool num = e.get_MousePosition().Y < 40;
				bool isOnCloseButton = e.get_MousePosition().X > ((Control)this).get_Width() - 45 && e.get_MousePosition().Y < 40;
				if (num && !isOnCloseButton)
				{
					return;
				}
			}
			((WindowBase2)this).OnLeftMouseButtonPressed(e);
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).UpdateContainer(gameTime);
			if (_isLocked && ((WindowBase2)this).get_Dragging() && ((Control)this).get_Location() != _lockedPosition)
			{
				((Control)this).set_Location(_lockedPosition);
			}
		}
	}
}
