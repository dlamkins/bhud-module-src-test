using System;
using System.Diagnostics;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Microsoft.Xna.Framework.Graphics;

namespace rp.spark.UI.Views
{
	public class SparkSettingsView : View
	{
		private const string SparkUrl = "https://getspark.fyi";

		private const int ContentWidth = 660;

		private const int ControlHeight = 30;

		private const int RowGap = 8;

		private const int LeftPadding = 8;

		private readonly Action<Control> _showMenu;

		public SparkSettingsView(Action<Control> showMenu)
			: this()
		{
			_showMenu = showMenu;
		}

		protected override void Build(Container buildPanel)
		{
			BuildSettings(buildPanel);
		}

		private void BuildSettings(Container buildPanel)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel obj = SparkFormLayout.AddAutoStack(buildPanel, 660, 6);
			((Control)obj).set_Left(8);
			SparkFormLayout.AddLabel((Container)(object)obj, "Use the SPARK Menu below to access profiles, player lists, RP tools, privacy controls, and settings.", 660, 30, GameService.Content.get_DefaultFont14(), SparkViewUI.SecondaryTextColor).set_WrapText(true);
			FlowPanel actions = SparkFormLayout.AddRow((Container)(object)obj, 660, 30, 8);
			StandardButton menuButton = SparkFormLayout.AddButton((Container)(object)actions, "Open SPARK Menu", 150, 30);
			((Control)menuButton).set_BasicTooltipText("Open profiles, player lists, RP tools, privacy controls, and settings.");
			((Control)menuButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_showMenu?.Invoke((Control)(object)menuButton);
			});
			StandardButton obj2 = SparkFormLayout.AddButton((Container)(object)actions, "Documentation", 130, 30);
			((Control)obj2).set_BasicTooltipText("Opens a browser page for https://getspark.fyi.");
			((Control)obj2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				OpenDocumentation();
			});
		}

		private static void OpenDocumentation()
		{
			try
			{
				Process.Start(new ProcessStartInfo("https://getspark.fyi")
				{
					UseShellExecute = true
				});
			}
			catch
			{
				ScreenNotification.ShowNotification("Couldn't open the SPARK documentation.", (NotificationType)2, (Texture2D)null, 4);
			}
		}
	}
}
