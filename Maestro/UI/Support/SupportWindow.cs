using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Maestro.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Maestro.UI.Support
{
	public class SupportWindow : StandardWindow
	{
		private static class Layout
		{
			public const int WindowWidth = 420;

			public const int WindowHeight = 326;

			public const int ContentWidth = 390;

			public const int IntroHeight = 84;

			public const int MethodHeight = 68;

			public const int ActionButtonWidth = 112;
		}

		private sealed class SupportFooter : Control
		{
			private const string Message = "Every song played, shared, and enjoyed already means a lot.";

			protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				BitmapFont font = GameService.Content.get_DefaultFont12();
				int textWidth = (int)Math.Ceiling(font.MeasureString("Every song played, shared, and enjoyed already means a lot.").Width);
				int totalWidth = textWidth + 5 + 14;
				int startX = Math.Max(0, (base._size.X - totalWidth) / 2);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, "Every song played, shared, and enjoyed already means a lot.", font, new Rectangle(startX, 0, textWidth, base._size.Y), MaestroTheme.MutedCream, false, (HorizontalAlignment)0, (VerticalAlignment)1);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, MaestroIcons.Support, new Rectangle(startX + textWidth + 5, (base._size.Y - 14) / 2, 14, 14), MaestroTheme.SupportPink);
			}

			public SupportFooter()
				: this()
			{
			}
		}

		private static readonly Logger Logger = Logger.GetLogger<SupportWindow>();

		private static Texture2D _backgroundTexture;

		private readonly StandardButton _copyButton;

		public SupportWindow()
			: this(GetBackground(), new Rectangle(0, 0, 420, 326), new Rectangle(15, 20, 390, 326))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Expected O, but got Unknown
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Support");
			((WindowBase2)this).set_Subtitle("Maestro");
			((WindowBase2)this).set_Emblem(Module.Instance.ContentsManager.GetTexture("support-emblem.png"));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_Id("SupportWindow_v1");
			((WindowBase2)this).set_CanResize(false);
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			int currentY = 2;
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, currentY));
			((Control)val).set_Size(new Point(390, 28));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_Text("Thanks for using Maestro!");
			val.set_TextColor(MaestroTheme.CreamWhite);
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(12, currentY + 32));
			((Control)val2).set_Size(new Point(366, 48));
			val2.set_Font(GameService.Content.get_DefaultFont14());
			val2.set_Text("Your support helps fund future updates\nand community features.");
			val2.set_TextColor(MaestroTheme.MutedCream);
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			currentY += 94;
			CreateMethodLabels(currentY, "Support on Ko-fi", "Support future updates and community features.");
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			((Control)val3).set_Location(new Point(278, currentY + 21));
			((Control)val3).set_Size(new Point(112, 26));
			val3.set_Text("Open Ko-fi");
			((Control)val3).set_BasicTooltipText("Open ko-fi.com/aex in your browser");
			((Control)val3).add_Click((EventHandler<MouseEventArgs>)OnKoFiClicked);
			currentY += 68;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Location(new Point(0, currentY));
			((Control)val4).set_Size(new Point(390, 1));
			((Control)val4).set_BackgroundColor(MaestroTheme.SubtleBorder);
			currentY += 10;
			CreateMethodLabels(currentY, "Send in-game gold", "Guild Wars 2 account: Aexor.6238");
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Location(new Point(278, currentY + 21));
			((Control)val5).set_Size(new Point(112, 26));
			val5.set_Text("Copy account");
			((Control)val5).set_BasicTooltipText("Copy Aexor.6238");
			_copyButton = val5;
			((Control)_copyButton).add_Click((EventHandler<MouseEventArgs>)OnCopyClicked);
			currentY += 78;
			SupportFooter supportFooter = new SupportFooter();
			((Control)supportFooter).set_Parent((Container)(object)this);
			((Control)supportFooter).set_Location(new Point(0, currentY));
			((Control)supportFooter).set_Size(new Point(390, 34));
		}

		private static Texture2D GetBackground()
		{
			return _backgroundTexture ?? (_backgroundTexture = MaestroTheme.CreateWindowBackground(420, 326));
		}

		private void CreateMethodLabels(int y, string title, string description)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(8, y + 7));
			((Control)val).set_Size(new Point(254, 26));
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_Text(title);
			val.set_TextColor(MaestroTheme.CreamWhite);
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(8, y + 35));
			((Control)val2).set_Size(new Point(254, 28));
			val2.set_Font(GameService.Content.get_DefaultFont12());
			val2.set_Text(description);
			val2.set_TextColor(MaestroTheme.MutedCream);
		}

		private void OnKoFiClicked(object sender, MouseEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo("https://ko-fi.com/aex")
				{
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to open Ko-fi support link");
				ScreenNotification.ShowNotification("Could not open Ko-fi. Visit ko-fi.com/aex in your browser.", (NotificationType)2, (Texture2D)null, 4);
			}
		}

		private async void OnCopyClicked(object sender, MouseEventArgs e)
		{
			((Control)_copyButton).set_Enabled(false);
			_copyButton.set_Text("Copying...");
			try
			{
				if (!(await ClipboardUtil.get_WindowsClipboardService().SetTextAsync("Aexor.6238")))
				{
					ShowCopyFailure();
					return;
				}
				_copyButton.set_Text("Copied");
				ScreenNotification.ShowNotification("Aexor.6238 copied to clipboard.", (NotificationType)0, (Texture2D)null, 4);
			}
			catch (Exception ex)
			{
				Logger.Error(ex, "Failed to copy the support account name");
				ShowCopyFailure();
			}
			finally
			{
				((Control)_copyButton).set_Enabled(true);
			}
		}

		protected override void OnShown(EventArgs e)
		{
			_copyButton.set_Text("Copy account");
			((Control)this).OnShown(e);
		}

		private void ShowCopyFailure()
		{
			_copyButton.set_Text("Copy account");
			ScreenNotification.ShowNotification("Could not copy the account name. Use Aexor.6238.", (NotificationType)2, (Texture2D)null, 4);
		}
	}
}
