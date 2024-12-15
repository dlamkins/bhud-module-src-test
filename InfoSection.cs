using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using DecorBlishhudModule;
using DecorBlishhudModule.CustomControls.CustomTab;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class InfoSection
{
	private static CustomTabbedWindow2 _decorWindow = DecorModule.DecorModuleInstance.DecorWindow;

	private static Texture2D _info = DecorModule.DecorModuleInstance.Info;

	private static Panel _infoTextPanel;

	private static Label _infoText;

	public static async Task InitializeInfoPanel()
	{
		Image val = new Image();
		((Control)val).set_Parent((Container)(object)_decorWindow);
		((Control)val).set_Location(new Point(((Control)_decorWindow).get_Width() - 120, 5));
		((Control)val).set_Width(35);
		((Control)val).set_Height(35);
		val.set_Texture(AsyncTexture2D.op_Implicit(_info));
		Panel val2 = new Panel();
		((Control)val2).set_Parent((Container)(object)_decorWindow);
		((Control)val2).set_Size(new Point(300, 60));
		((Control)val2).set_Location(new Point(((Control)_decorWindow).get_Width() - 430, -5));
		val2.set_ShowBorder(true);
		((Control)val2).set_Visible(false);
		((Control)val2).set_Opacity(0f);
		_infoTextPanel = val2;
		Label val3 = new Label();
		((Control)val3).set_Parent((Container)(object)_infoTextPanel);
		val3.set_Text("    Click on the name or the image\n            to copy its name.");
		((Control)val3).set_Location(new Point(10, 0));
		((Control)val3).set_Width(270);
		((Control)val3).set_Height(45);
		val3.set_ShowShadow(true);
		val3.set_WrapText(true);
		val3.set_StrokeText(true);
		val3.set_TextColor(Color.get_White());
		val3.set_ShadowColor(new Color(0, 0, 0));
		val3.set_Font(GameService.Content.get_DefaultFont16());
		_infoText = val3;
		((Control)val).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
		{
			AnimatePanel(_infoTextPanel, fadeIn: true);
		});
		((Control)val).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
		{
			AnimatePanel(_infoTextPanel, fadeIn: false);
		});
	}

	private static async Task AnimatePanel(Panel panel, bool fadeIn)
	{
		_ = fadeIn;
		if (fadeIn)
		{
			((Control)panel).set_Visible(true);
		}
		Timer timer = new Timer
		{
			Interval = 1
		};
		timer.Tick += delegate
		{
			Panel obj = panel;
			((Control)obj).set_Opacity(((Control)obj).get_Opacity() + (fadeIn ? 0.1f : (-0.1f)));
			((Control)panel).set_Opacity(MathHelper.Clamp(((Control)panel).get_Opacity(), 0f, 1f));
			if (fadeIn && ((Control)panel).get_Opacity() >= 1f)
			{
				((Control)panel).set_Opacity(1f);
				timer.Stop();
			}
			else if (!fadeIn && ((Control)panel).get_Opacity() <= 0f)
			{
				((Control)panel).set_Opacity(0f);
				((Control)panel).set_Visible(false);
				timer.Stop();
			}
		};
		timer.Start();
	}

	public static void UpdateInfoText(string newText)
	{
		_infoText.set_Text(newText);
	}
}
