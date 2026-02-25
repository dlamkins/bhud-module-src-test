using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Models;
using CinemaModule.Services;
using Microsoft.Xna.Framework;

namespace CinemaModule.UI.Chat
{
	public class TwitchChatPanel : Panel
	{
		private const int MessagePadding = 5;

		private const int MaxVisibleMessages = 300;

		private const int ScrollBarWidth = 8;

		private const int InputAreaHeight = 40;

		private const int SendButtonWidth = 50;

		private const int PauseButtonWidth = 80;

		private readonly TwitchChatService _chatService;

		private readonly List<TwitchChatMessage> _messages = new List<TwitchChatMessage>();

		private readonly List<TwitchChatMessage> _bufferedMessages = new List<TwitchChatMessage>();

		private readonly object _messagesLock = new object();

		private readonly List<Panel> _messagePanels = new List<Panel>();

		private int _currentYOffset;

		private Panel _messageContainer;

		private Panel _inputPanel;

		private TextBox _inputBox;

		private StandardButton _sendButton;

		private StandardButton _pauseButton;

		private Label _statusLabel;

		private Label _loginStatusLabel;

		private bool _isDisposed;

		private bool _isPaused;

		public event EventHandler<string> MessageSent;

		public TwitchChatPanel(TwitchChatService chatService)
			: this()
		{
			_chatService = chatService;
			_chatService.MessageReceived += OnChatMessageReceived;
			_chatService.ConnectionStateChanged += OnConnectionStateChanged;
			BuildLayout();
		}

		private void BuildLayout()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Expected O, but got Unknown
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Expected O, but got Unknown
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			((Control)this).set_BackgroundColor(new Color(0, 0, 0, 50));
			((Panel)this).set_ShowBorder(true);
			((Control)this).set_ClipsBounds(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(5, 5));
			((Control)val).set_Size(new Point(200, 20));
			val.set_Text("Disconnected");
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(Color.get_Gray());
			_statusLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(210, 7));
			((Control)val2).set_Size(new Point(((Control)this).get_Width() - 220, 18));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_HorizontalAlignment((HorizontalAlignment)2);
			_loginStatusLabel = val2;
			Panel val3 = new Panel();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(0, 30));
			((Control)val3).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 30 - 40 - 10));
			val3.set_CanScroll(false);
			val3.set_ShowBorder(false);
			((Control)val3).set_ClipsBounds(true);
			_messageContainer = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(1, ((Control)this).get_Height() - 40 - 5));
			((Control)val4).set_Size(new Point(((Control)this).get_Width() - 10, 40));
			((Control)val4).set_BackgroundColor(Color.get_Transparent());
			val4.set_ShowBorder(false);
			_inputPanel = val4;
			TextBox val5 = new TextBox();
			((Control)val5).set_Parent((Container)(object)_inputPanel);
			((Control)val5).set_Location(new Point(5, 5));
			((Control)val5).set_Size(new Point(((Control)_inputPanel).get_Width() - 50 - 80 - 20, 25));
			((TextInputBase)val5).set_PlaceholderText("[Say]");
			((Control)val5).set_BackgroundColor(new Color(50, 50, 50));
			_inputBox = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)_inputPanel);
			((Control)val6).set_Location(new Point(((Control)_inputPanel).get_Width() - 50 - 80 - 10, 5));
			((Control)val6).set_Size(new Point(80, 25));
			val6.set_Text("Pause");
			_pauseButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)_inputPanel);
			((Control)val7).set_Location(new Point(((Control)_inputPanel).get_Width() - 50 - 5, 5));
			((Control)val7).set_Size(new Point(50, 25));
			val7.set_Text("Send");
			_sendButton = val7;
			_inputBox.add_EnterPressed((EventHandler<EventArgs>)OnInputEnterPressed);
			((Control)_sendButton).add_Click((EventHandler<MouseEventArgs>)OnSendButtonClicked);
			((Control)_pauseButton).add_Click((EventHandler<MouseEventArgs>)OnPauseButtonClicked);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
			SyncConnectionStatus();
			UpdateLoginStatusLabel();
		}

		private void SyncConnectionStatus()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			bool isConnected = _chatService.IsConnected;
			_statusLabel.set_Text(isConnected ? "Connected" : "Disconnected");
			_statusLabel.set_TextColor(isConnected ? Color.get_LightGreen() : Color.get_Gray());
		}

		public void RefreshAuthStatus()
		{
			SyncConnectionStatus();
			UpdateLoginStatusLabel();
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			((Control)_statusLabel).set_Size(new Point(200, 20));
			((Control)_loginStatusLabel).set_Size(new Point(((Control)this).get_Width() - 220, 18));
			((Control)_messageContainer).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - 30 - 40 - 10));
			((Control)_inputPanel).set_Location(new Point(1, ((Control)this).get_Height() - 40 - 5 - 3));
			((Control)_inputPanel).set_Size(new Point(((Control)this).get_Width() - 10, 40));
			((Control)_inputBox).set_Size(new Point(((Control)_inputPanel).get_Width() - 50 - 80 - 20, 25));
			((Control)_sendButton).set_Location(new Point(((Control)_inputPanel).get_Width() - 50 - 5, 5));
			int pauseX = (((Control)_sendButton).get_Visible() ? (((Control)_inputPanel).get_Width() - 50 - 80 - 10) : (((Control)_inputPanel).get_Width() - 80 - 5));
			((Control)_pauseButton).set_Location(new Point(pauseX, 5));
			UpdateLoginStatusLabel();
			RefreshMessageDisplay();
		}

		private void OnChatMessageReceived(object sender, TwitchChatMessageEventArgs e)
		{
			if (_isDisposed)
			{
				return;
			}
			if (_isPaused)
			{
				lock (_messagesLock)
				{
					_bufferedMessages.Add(e.Message);
				}
				UpdatePauseButtonText();
			}
			else
			{
				AddMessageToDisplay(e.Message);
			}
		}

		private void OnConnectionStateChanged(object sender, TwitchChatConnectionEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			_statusLabel.set_Text(e.Status);
			_statusLabel.set_TextColor(e.IsConnected ? Color.get_LightGreen() : Color.get_Gray());
			UpdateLoginStatusLabel();
			if (!e.IsConnected)
			{
				ClearMessages();
			}
		}

		private void AddMessageToDisplay(TwitchChatMessage message)
		{
			if (_messageContainer == null || _isDisposed)
			{
				return;
			}
			int availableWidth = ((Control)_messageContainer).get_Width() - 8 - 10;
			Panel panel = CreateMessagePanel(message, availableWidth, _currentYOffset);
			lock (_messagesLock)
			{
				_messages.Add(message);
				_messagePanels.Add(panel);
				_currentYOffset += ((Control)panel).get_Height() + 2;
				while (_messages.Count > 300)
				{
					RemoveOldestMessage();
				}
			}
			if (!_isPaused)
			{
				ScrollToBottom();
			}
		}

		private void ClearMessages()
		{
			lock (_messagesLock)
			{
				DisposePanels();
				_messages.Clear();
				_bufferedMessages.Clear();
			}
			((Container)_messageContainer).set_VerticalScrollOffset(0);
		}

		private void DisposePanels()
		{
			foreach (Panel messagePanel in _messagePanels)
			{
				((Control)messagePanel).Dispose();
			}
			_messagePanels.Clear();
			_currentYOffset = 0;
		}

		private void RemoveOldestMessage()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			if (_messagePanels.Count == 0)
			{
				return;
			}
			Panel obj = _messagePanels[0];
			int removedHeight = ((Control)obj).get_Height() + 2;
			((Control)obj).Dispose();
			_messagePanels.RemoveAt(0);
			_messages.RemoveAt(0);
			foreach (Panel panel in _messagePanels)
			{
				((Control)panel).set_Location(new Point(((Control)panel).get_Location().X, ((Control)panel).get_Location().Y - removedHeight));
			}
			_currentYOffset -= removedHeight;
		}

		private void ScrollToBottom()
		{
			int contentHeight = _currentYOffset + 5;
			int containerHeight = ((Control)_messageContainer).get_Height();
			((Container)_messageContainer).set_VerticalScrollOffset((contentHeight > containerHeight) ? (contentHeight - containerHeight) : 0);
		}

		private void RefreshMessageDisplay()
		{
			List<TwitchChatMessage> messagesToRebuild;
			lock (_messagesLock)
			{
				messagesToRebuild = new List<TwitchChatMessage>(_messages);
				DisposePanels();
			}
			int availableWidth = ((Control)_messageContainer).get_Width() - 8 - 10;
			lock (_messagesLock)
			{
				foreach (TwitchChatMessage message in messagesToRebuild)
				{
					Panel panel = CreateMessagePanel(message, availableWidth, _currentYOffset);
					_messagePanels.Add(panel);
					_currentYOffset += ((Control)panel).get_Height() + 2;
				}
			}
			if (!_isPaused)
			{
				ScrollToBottom();
			}
		}

		private Panel CreateMessagePanel(TwitchChatMessage message, int availableWidth, int yOffset)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_messageContainer);
			((Control)val).set_Location(new Point(5, yOffset));
			((Control)val).set_Size(new Point(availableWidth, 0));
			((Control)val).set_BackgroundColor(Color.get_Transparent());
			((Control)val).set_ClipsBounds(true);
			Panel panel = val;
			string timestamp = message.Timestamp.ToString("[HH:mm] ");
			string displayName = SanitizeForDisplay(message.DisplayName);
			string messageText = SanitizeForDisplay(message.Message);
			Color userColor = EnsureVisibleColor(message.UserColor);
			FormattedLabel messageLabel = new FormattedLabelBuilder().SetWidth(availableWidth).AutoSizeHeight().Wrap()
				.CreatePart(timestamp, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					builder.SetTextColor(Color.get_Gray());
				})
				.CreatePart(displayName + ": ", (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					builder.SetTextColor(userColor);
				})
				.CreatePart(messageText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder builder)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					builder.SetTextColor(message.IsAction ? userColor : Color.get_White());
				})
				.Build();
			((Control)messageLabel).set_Parent((Container)(object)panel);
			((Control)messageLabel).set_Location(new Point(0, 2));
			int totalHeight = Math.Max(20, ((Control)messageLabel).get_Height() + 3);
			((Control)panel).set_Size(new Point(availableWidth, totalHeight));
			return panel;
		}

		private string SanitizeForDisplay(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			return text.Replace('_', '-');
		}

		private Color EnsureVisibleColor(Color color)
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if ((0.299f * (float)(int)((Color)(ref color)).get_R() + 0.587f * (float)(int)((Color)(ref color)).get_G() + 0.114f * (float)(int)((Color)(ref color)).get_B()) / 255f < 0.15f)
			{
				return new Color(180, 180, 180);
			}
			return color;
		}

		private void OnInputEnterPressed(object sender, EventArgs e)
		{
			SendCurrentMessage();
		}

		private void OnSendButtonClicked(object sender, MouseEventArgs e)
		{
			SendCurrentMessage();
		}

		private void OnPauseButtonClicked(object sender, MouseEventArgs e)
		{
			TogglePause();
		}

		private void TogglePause()
		{
			_isPaused = !_isPaused;
			_messageContainer.set_CanScroll(_isPaused);
			if (!_isPaused)
			{
				FlushBufferedMessages();
			}
			UpdatePauseButtonText();
		}

		private void FlushBufferedMessages()
		{
			List<TwitchChatMessage> messagesToAdd;
			lock (_messagesLock)
			{
				messagesToAdd = new List<TwitchChatMessage>(_bufferedMessages);
				_bufferedMessages.Clear();
			}
			foreach (TwitchChatMessage message in messagesToAdd)
			{
				AddMessageToDisplay(message);
			}
		}

		private void UpdatePauseButtonText()
		{
			if (!_isPaused)
			{
				_pauseButton.set_Text("Pause");
				return;
			}
			int bufferedCount;
			lock (_messagesLock)
			{
				bufferedCount = _bufferedMessages.Count;
			}
			_pauseButton.set_Text((bufferedCount > 0) ? $"Resume ({bufferedCount})" : "Resume");
		}

		private void SendCurrentMessage()
		{
			string text = ((TextInputBase)_inputBox).get_Text()?.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				((TextInputBase)_inputBox).set_Text(string.Empty);
				this.MessageSent?.Invoke(this, text);
			}
		}

		private void UpdateLoginStatusLabel()
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			bool isAuthenticated = _chatService.IsAuthenticated;
			bool canSend = isAuthenticated && _chatService.IsConnected;
			_loginStatusLabel.set_Text(isAuthenticated ? ("as: " + _chatService.Username) : "Not logged in - read only");
			_loginStatusLabel.set_TextColor(isAuthenticated ? Color.get_LightGreen() : Color.get_Gray());
			((Control)_sendButton).set_Visible(isAuthenticated);
			((Control)_sendButton).set_Enabled(canSend);
			((Control)_inputBox).set_Visible(isAuthenticated);
			((Control)_inputBox).set_Enabled(canSend);
			int pauseX = (isAuthenticated ? (((Control)_inputPanel).get_Width() - 50 - 80 - 10) : (((Control)_inputPanel).get_Width() - 80 - 5));
			((Control)_pauseButton).set_Location(new Point(pauseX, 5));
		}

		protected override void DisposeControl()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_chatService.MessageReceived -= OnChatMessageReceived;
				_chatService.ConnectionStateChanged -= OnConnectionStateChanged;
				_inputBox.remove_EnterPressed((EventHandler<EventArgs>)OnInputEnterPressed);
				((Control)_sendButton).remove_Click((EventHandler<MouseEventArgs>)OnSendButtonClicked);
				((Control)_pauseButton).remove_Click((EventHandler<MouseEventArgs>)OnPauseButtonClicked);
				((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
				((Panel)this).DisposeControl();
			}
		}
	}
}
