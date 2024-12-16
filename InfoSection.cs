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

	private static Panel _infoTextPanel;

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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
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
		((Control)val).add_MouseEntered((EventHandler<MouseEventArgs>)async delegate
		{
			await AnimatePanel(_infoTextPanel, fadeIn: true);
		});
		((Control)val).add_MouseLeft((EventHandler<MouseEventArgs>)async delegate
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
}
