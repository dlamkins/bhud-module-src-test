using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using CinemaModule.Services;
using Microsoft.Xna.Framework;

namespace CinemaHUD.UI.Windows.SettingsSmall
{
	public class TwitchAuthWindow : SmallWindow
	{
		private static readonly Logger Logger = Logger.GetLogger<TwitchAuthWindow>();

		private readonly TwitchAuthService _authService;

		private readonly Action<string, string> _onTokensChanged;

		private FlowPanel _contentPanel;

		private Label _statusLabel;

		private Label _privacyNoteLabel;

		private Label _codeLabel;

		private Label _instructionLabel;

		private FlowPanel _codeSection;

		private StandardButton _actionButton;

		private StandardButton _cancelAuthButton;

		private StandardButton _openBrowserButton;

		private StandardButton _closeButton;

		private string _currentCode;

		private string _verificationUri;

		public TwitchAuthWindow(TwitchAuthService authService, Action<string, string> onTokensChanged)
			: base("Twitch Login")
		{
			_authService = authService;
			_onTokensChanged = onTokensChanged;
			_authService.AuthStatusChanged += OnAuthStatusChanged;
			_authService.DeviceCodeReceived += OnDeviceCodeReceived;
			Initialize();
		}

		protected override void BuildContent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_OuterControlPadding(new Vector2(20f, 20f));
			val.set_ControlPadding(new Vector2(0f, 15f));
			((Control)val).set_Parent((Container)(object)this);
			_contentPanel = val;
			BuildHeader();
			BuildCodeSection();
			BuildButtons();
			UpdateUIForAuthStatus();
		}

		private void BuildHeader()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text("Link your Twitch account");
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			Label val2 = new Label();
			val2.set_Text("Not connected");
			val2.set_TextColor(Color.get_Gray());
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)_contentPanel);
			_statusLabel = val2;
			Label val3 = new Label();
			val3.set_Text("Your token is stored locally only and is never shared.");
			val3.set_TextColor(Color.get_LightGray());
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Parent((Container)(object)_contentPanel);
			_privacyNoteLabel = val3;
		}

		private void BuildCodeSection()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(0f, 12f));
			((Control)val).set_Visible(false);
			((Control)val).set_Parent((Container)(object)_contentPanel);
			_codeSection = val;
			Label val2 = new Label();
			val2.set_Text("--------");
			val2.set_Font(GameService.Content.get_DefaultFont32());
			val2.set_TextColor(new Color(145, 70, 255));
			val2.set_AutoSizeHeight(true);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Parent((Container)(object)_codeSection);
			_codeLabel = val2;
			Label val3 = new Label();
			val3.set_Text("Enter code and authorize CinemaHUD");
			val3.set_AutoSizeHeight(true);
			val3.set_AutoSizeWidth(true);
			((Control)val3).set_Parent((Container)(object)_codeSection);
			_instructionLabel = val3;
			FlowPanel val4 = new FlowPanel();
			val4.set_FlowDirection((ControlFlowDirection)0);
			((Container)val4).set_WidthSizingMode((SizingMode)1);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			val4.set_ControlPadding(new Vector2(10f, 0f));
			((Control)val4).set_Parent((Container)(object)_codeSection);
			FlowPanel buttonRow = val4;
			StandardButton val5 = new StandardButton();
			val5.set_Text("Open Activation Page");
			((Control)val5).set_Width(150);
			((Control)val5).set_Parent((Container)(object)buttonRow);
			_openBrowserButton = val5;
			((Control)_openBrowserButton).add_Click((EventHandler<MouseEventArgs>)OnOpenBrowserClicked);
			StandardButton val6 = new StandardButton();
			val6.set_Text("Cancel");
			((Control)val6).set_Width(80);
			((Control)val6).set_Parent((Container)(object)buttonRow);
			_cancelAuthButton = val6;
			((Control)_cancelAuthButton).add_Click((EventHandler<MouseEventArgs>)OnCancelAuthClicked);
		}

		private void BuildButtons()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)0);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_ControlPadding(new Vector2(10f, 0f));
			val.set_OuterControlPadding(new Vector2(0f, 10f));
			((Control)val).set_Parent((Container)(object)_contentPanel);
			FlowPanel buttonPanel = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Login");
			((Control)val2).set_Width(100);
			((Control)val2).set_Parent((Container)(object)buttonPanel);
			_actionButton = val2;
			((Control)_actionButton).add_Click((EventHandler<MouseEventArgs>)OnActionButtonClicked);
			StandardButton val3 = new StandardButton();
			val3.set_Text("Close");
			((Control)val3).set_Width(80);
			((Control)val3).set_Parent((Container)(object)buttonPanel);
			_closeButton = val3;
			((Control)_closeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((Control)this).Hide();
			});
		}

		private void OnActionButtonClicked(object sender, MouseEventArgs e)
		{
			if (_authService.IsAuthenticated)
			{
				_authService.LogoutAsync();
				return;
			}
			((Control)_actionButton).set_Visible(false);
			((Control)_closeButton).set_Visible(false);
			_authService.StartDeviceAuthFlowAsync();
		}

		private void OnCancelAuthClicked(object sender, MouseEventArgs e)
		{
			_authService.CancelPendingAuth();
			_currentCode = null;
			UpdateUIForAuthStatus();
		}

		private void OnOpenBrowserClicked(object sender, MouseEventArgs e)
		{
			if (!string.IsNullOrEmpty(_verificationUri))
			{
				try
				{
					Process.Start(_verificationUri);
				}
				catch (Exception ex)
				{
					Logger.Warn(ex, "Failed to open browser");
				}
			}
		}

		private void OnDeviceCodeReceived(object sender, DeviceCodeEventArgs e)
		{
			_currentCode = e.UserCode;
			_verificationUri = e.VerificationUri;
			_codeLabel.set_Text(e.UserCode);
			((Control)_codeSection).set_Visible(true);
		}

		private void OnAuthStatusChanged(object sender, TwitchAuthStatusEventArgs e)
		{
			Logger.Debug($"Auth status changed: {e.Status} - {e.Message}");
			switch (e.Status)
			{
			case TwitchAuthStatus.Authenticated:
				_currentCode = null;
				_onTokensChanged?.Invoke(e.AccessToken, e.RefreshToken);
				UpdateUIForAuthStatus();
				break;
			case TwitchAuthStatus.NotAuthenticated:
				_currentCode = null;
				_onTokensChanged?.Invoke(null, null);
				UpdateUIForAuthStatus();
				break;
			case TwitchAuthStatus.Failed:
			case TwitchAuthStatus.Cancelled:
				UpdateUIForAuthStatus();
				break;
			case TwitchAuthStatus.WaitingForUser:
				break;
			}
		}

		private void UpdateUIForAuthStatus()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			if (_authService.IsAuthenticated)
			{
				_statusLabel.set_Text("Connected as: " + _authService.Username);
				_statusLabel.set_TextColor(new Color(100, 200, 100));
				_actionButton.set_Text("Logout");
				((Control)_actionButton).set_Visible(true);
				((Control)_closeButton).set_Visible(true);
				((Control)_codeSection).set_Visible(false);
				((Control)_privacyNoteLabel).set_Visible(false);
			}
			else
			{
				_statusLabel.set_Text("Not connected");
				_statusLabel.set_TextColor(Color.get_Gray());
				_actionButton.set_Text("Login");
				bool hasActiveCode = !string.IsNullOrEmpty(_currentCode);
				((Control)_actionButton).set_Visible(!hasActiveCode);
				((Control)_closeButton).set_Visible(!hasActiveCode);
				((Control)_codeSection).set_Visible(hasActiveCode);
				((Control)_privacyNoteLabel).set_Visible(!hasActiveCode);
			}
		}

		protected override void DisposeControl()
		{
			_authService.AuthStatusChanged -= OnAuthStatusChanged;
			_authService.DeviceCodeReceived -= OnDeviceCodeReceived;
			_authService.CancelPendingAuth();
			((WindowBase2)this).DisposeControl();
		}
	}
}
