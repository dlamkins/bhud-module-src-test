using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class LoadingOverlay : Container
	{
		private const int ButtonWidth = 180;

		private const int ButtonHeight = 30;

		private const int ButtonGap = 12;

		private readonly Label _messageLabel;

		private readonly LoadingSpinner _spinner;

		private readonly Panel _backgroundPanel;

		private readonly StandardButton _reconnectButton;

		private readonly StandardButton _openConnectionButton;

		public Action OnReconnect;

		public Action OnOpenConnectionTab;

		public string Message
		{
			get
			{
				Label messageLabel = _messageLabel;
				if (messageLabel == null)
				{
					return null;
				}
				return messageLabel.get_Text();
			}
		}

		public LoadingOverlay(string message = "Loading...")
			: this()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			((Control)this).set_ZIndex(999);
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_BackgroundColor(new Color(0, 0, 0, 180));
			((Control)val).set_Width(((Control)this).get_Width());
			((Control)val).set_Height(((Control)this).get_Height());
			_backgroundPanel = val;
			LoadingSpinner val2 = new LoadingSpinner();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Visible(false);
			_spinner = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(message);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_TextColor(Color.get_White());
			_messageLabel = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Reconnect");
			((Control)val4).set_Width(180);
			((Control)val4).set_Height(30);
			((Control)val4).set_Visible(false);
			_reconnectButton = val4;
			((Control)_reconnectButton).add_Click((EventHandler<MouseEventArgs>)OnReconnectClick);
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text("Open Connection tab");
			((Control)val5).set_Width(180);
			((Control)val5).set_Height(30);
			_openConnectionButton = val5;
			((Control)_openConnectionButton).add_Click((EventHandler<MouseEventArgs>)OnOpenConnectionClick);
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)OnResized);
		}

		private void OnReconnectClick(object sender, MouseEventArgs e)
		{
			OnReconnect?.Invoke();
		}

		private void OnOpenConnectionClick(object sender, MouseEventArgs e)
		{
			OnOpenConnectionTab?.Invoke();
		}

		private void OnResized(object sender, ResizedEventArgs e)
		{
			PositionElements();
		}

		private void PositionElements()
		{
			((Control)_backgroundPanel).set_Width(((Control)this).get_Width());
			((Control)_backgroundPanel).set_Height(((Control)this).get_Height());
			int centerY = ((Control)this).get_Height() / 2;
			if (_spinner != null)
			{
				((Control)_spinner).set_Left((((Control)this).get_Width() - ((Control)_spinner).get_Width()) / 2);
				((Control)_spinner).set_Top(centerY - ((Control)_spinner).get_Height() - 10);
			}
			if (_messageLabel != null)
			{
				((Control)_messageLabel).set_Left((((Control)this).get_Width() - ((Control)_messageLabel).get_Width()) / 2);
				((Control)_messageLabel).set_Top(centerY);
			}
			Label messageLabel = _messageLabel;
			int rowTop = ((messageLabel != null) ? ((Control)messageLabel).get_Bottom() : centerY) + 20;
			LayoutButtonRow(rowTop);
		}

		private void LayoutButtonRow(int rowTop)
		{
			if (_reconnectButton != null && _openConnectionButton != null)
			{
				bool visible = ((Control)_reconnectButton).get_Visible();
				int rowWidth = (visible ? 372 : 180);
				int rowLeft = (((Control)this).get_Width() - rowWidth) / 2;
				if (visible)
				{
					((Control)_reconnectButton).set_Left(rowLeft);
					((Control)_reconnectButton).set_Top(rowTop);
					((Control)_openConnectionButton).set_Left(rowLeft + 180 + 12);
					((Control)_openConnectionButton).set_Top(rowTop);
				}
				else
				{
					((Control)_openConnectionButton).set_Left(rowLeft);
					((Control)_openConnectionButton).set_Top(rowTop);
				}
			}
		}

		public void SetState(string message, bool canReconnect, bool showSpinner)
		{
			_messageLabel.set_Text(message ?? string.Empty);
			((Control)_reconnectButton).set_Visible(canReconnect);
			if (_spinner != null)
			{
				((Control)_spinner).set_Visible(showSpinner);
			}
			PositionElements();
		}

		public void UpdateMessage(string message)
		{
			_messageLabel.set_Text(message);
			PositionElements();
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).OnShown(e);
			PositionElements();
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Resized((EventHandler<ResizedEventArgs>)OnResized);
			if (_reconnectButton != null)
			{
				((Control)_reconnectButton).remove_Click((EventHandler<MouseEventArgs>)OnReconnectClick);
			}
			if (_openConnectionButton != null)
			{
				((Control)_openConnectionButton).remove_Click((EventHandler<MouseEventArgs>)OnOpenConnectionClick);
			}
			LoadingSpinner spinner = _spinner;
			if (spinner != null)
			{
				((Control)spinner).Dispose();
			}
			Label messageLabel = _messageLabel;
			if (messageLabel != null)
			{
				((Control)messageLabel).Dispose();
			}
			StandardButton reconnectButton = _reconnectButton;
			if (reconnectButton != null)
			{
				((Control)reconnectButton).Dispose();
			}
			StandardButton openConnectionButton = _openConnectionButton;
			if (openConnectionButton != null)
			{
				((Control)openConnectionButton).Dispose();
			}
			Panel backgroundPanel = _backgroundPanel;
			if (backgroundPanel != null)
			{
				((Control)backgroundPanel).Dispose();
			}
			((Container)this).DisposeControl();
		}
	}
}
