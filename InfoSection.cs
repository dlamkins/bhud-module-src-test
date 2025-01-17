using System;
using System.Threading.Tasks;
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

	private static Image _infoIcon;

	private static Panel _infoTextPanel;

	private static Panel _infoTextPanelBackground;

	private static Label _infoText;

	public static void InitializeInfoPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		Image val = new Image();
		((Control)val).set_Parent((Container)(object)_decorWindow);
		((Control)val).set_Location(new Point(((Control)_decorWindow).get_Width() - 120, 5));
		((Control)val).set_Width(35);
		((Control)val).set_Height(35);
		val.set_Texture(AsyncTexture2D.op_Implicit(_info));
		_infoIcon = val;
		Panel val2 = new Panel();
		((Control)val2).set_Parent((Container)(object)_decorWindow);
		((Control)val2).set_Size(new Point(300, 60));
		((Control)val2).set_Location(new Point(((Control)_decorWindow).get_Width() - 430, -5));
		val2.set_ShowBorder(true);
		((Control)val2).set_Visible(false);
		((Control)val2).set_Opacity(0f);
		((Control)val2).set_ZIndex(1);
		_infoTextPanel = val2;
		Panel val3 = new Panel();
		((Control)val3).set_Parent((Container)(object)_infoTextPanel);
		((Control)val3).set_Size(new Point(300, 60));
		((Control)val3).set_Location(new Point(0, 0));
		((Control)val3).set_BackgroundColor(new Color(0, 0, 0, 205));
		((Control)val3).set_ZIndex(0);
		_infoTextPanelBackground = val3;
		Label val4 = new Label();
		((Control)val4).set_Parent((Container)(object)_infoTextPanel);
		val4.set_Text("    Click on the name or the image\n            to copy its name.");
		((Control)val4).set_Location(new Point(10, 0));
		((Control)val4).set_Width(270);
		((Control)val4).set_Height(45);
		val4.set_ShowShadow(true);
		val4.set_WrapText(true);
		val4.set_StrokeText(true);
		val4.set_TextColor(Color.get_White());
		val4.set_ShadowColor(new Color(0, 0, 0));
		val4.set_Font(GameService.Content.get_DefaultFont16());
		_infoText = val4;
		((Control)_infoIcon).add_MouseEntered((EventHandler<MouseEventArgs>)async delegate
		{
			await AnimatePanel(_infoTextPanel, fadeIn: true);
		});
		((Control)_infoIcon).add_MouseLeft((EventHandler<MouseEventArgs>)async delegate
		{
			await AnimatePanel(_infoTextPanel, fadeIn: false);
		});
	}

	private static async Task AnimatePanel(Panel panel, bool fadeIn)
	{
		float targetOpacity = (fadeIn ? 1f : 0f);
		float step = (fadeIn ? 0.1f : (-0.1f));
		if (fadeIn)
		{
			((Control)panel).set_Visible(true);
		}
		while ((fadeIn && ((Control)panel).get_Opacity() < targetOpacity) || (!fadeIn && ((Control)panel).get_Opacity() > targetOpacity))
		{
			((Control)panel).set_Opacity(MathHelper.Clamp(((Control)panel).get_Opacity() + step, 0f, 1f));
			await Task.Delay(16);
		}
		((Control)panel).set_Opacity(targetOpacity);
		if (!fadeIn)
		{
			((Control)panel).set_Visible(false);
		}
	}

	public static void UpdateInfoText(string newText)
	{
		_infoText.set_Text(newText);
	}

	public static void UpdateInfoVisible(bool visible)
	{
		if (visible)
		{
			((Control)_infoIcon).set_Visible(true);
		}
		if (!visible)
		{
			((Control)_infoIcon).set_Visible(false);
		}
	}
}
