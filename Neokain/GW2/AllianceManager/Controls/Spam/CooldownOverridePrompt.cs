using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace Neokain.GW2.AllianceManager.Controls.Spam
{
	public sealed class CooldownOverridePrompt : StandardWindow
	{
		private const int WINDOW_WIDTH = 380;

		private const int WINDOW_HEIGHT = 130;

		private const int PADDING = 10;

		private readonly Label _messageLabel;

		private readonly StandardButton _confirmButton;

		private readonly StandardButton _cancelButton;

		private TaskCompletionSource<bool> _tcs;

		public CooldownOverridePrompt()
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 380, 130), new Rectangle(10, 10, 360, 110))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Cooldown active");
			((WindowBase2)this).set_SavesPosition(false);
			((WindowBase2)this).set_Id("Neokain_GW2_AllianceManager_cooldownOverridePrompt");
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(true);
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_ZIndex(9999);
			((Control)this).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 380) / 2);
			((Control)this).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 130) / 2);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Left(0);
			((Control)val).set_Top(0);
			((Control)val).set_Width(((Container)this).get_ContentRegion().Width);
			val.set_AutoSizeHeight(true);
			val.set_WrapText(true);
			val.set_Text(string.Empty);
			_messageLabel = val;
			int buttonTop = ((Container)this).get_ContentRegion().Height - 35;
			int buttonWidth = 110;
			int buttonSpacing = 10;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Send anyway");
			((Control)val2).set_Width(buttonWidth + 30);
			((Control)val2).set_Left(((Container)this).get_ContentRegion().Width - (buttonWidth + 30) * 2 - buttonSpacing);
			((Control)val2).set_Top(buttonTop);
			_confirmButton = val2;
			((Control)_confirmButton).add_Click((EventHandler<MouseEventArgs>)OnConfirmClicked);
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Cancel");
			((Control)val3).set_Width(buttonWidth);
			((Control)val3).set_Left(((Container)this).get_ContentRegion().Width - buttonWidth);
			((Control)val3).set_Top(buttonTop);
			_cancelButton = val3;
			((Control)_cancelButton).add_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			((Control)this).add_Hidden((EventHandler<EventArgs>)OnWindowHidden);
		}

		public Task<bool> ShowAsync(string reason)
		{
			_tcs?.TrySetResult(result: false);
			_tcs = new TaskCompletionSource<bool>();
			string displayReason = (string.IsNullOrEmpty(reason) ? "unknown time" : reason);
			_messageLabel.set_Text("Spam is still on cooldown (" + displayReason + "). Send anyway?");
			((Control)this).set_Left((((Control)GameService.Graphics.get_SpriteScreen()).get_Width() - 380) / 2);
			((Control)this).set_Top((((Control)GameService.Graphics.get_SpriteScreen()).get_Height() - 130) / 2);
			((Control)this).Show();
			return _tcs.Task;
		}

		private void OnConfirmClicked(object sender, MouseEventArgs e)
		{
			_tcs?.TrySetResult(result: true);
			_tcs = null;
			((Control)this).Hide();
		}

		private void OnCancelClicked(object sender, MouseEventArgs e)
		{
			_tcs?.TrySetResult(result: false);
			_tcs = null;
			((Control)this).Hide();
		}

		private void OnWindowHidden(object sender, EventArgs e)
		{
			_tcs?.TrySetResult(result: false);
			_tcs = null;
		}

		protected override void DisposeControl()
		{
			((Control)this).remove_Hidden((EventHandler<EventArgs>)OnWindowHidden);
			if (_confirmButton != null)
			{
				((Control)_confirmButton).remove_Click((EventHandler<MouseEventArgs>)OnConfirmClicked);
			}
			if (_cancelButton != null)
			{
				((Control)_cancelButton).remove_Click((EventHandler<MouseEventArgs>)OnCancelClicked);
			}
			_tcs?.TrySetCanceled();
			_tcs = null;
			((WindowBase2)this).DisposeControl();
		}
	}
}
