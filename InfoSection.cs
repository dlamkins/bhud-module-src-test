using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using DecorBlishhudModule;
using DecorBlishhudModule.CustomControls.CustomTab;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

public class InfoSection
{
	private static CustomTabbedWindow2 _decorWindow = DecorModule.DecorModuleInstance.DecorWindow;

	private static FlowPanel _infoContainer;

	private static FlowPanel _firstPanel;

	private static FlowPanel _secondPanel;

	public static void InitializeInfoPanel()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		int containerWidth = 550;
		int containerHeight = 150;
		FlowPanel val = new FlowPanel();
		((Control)val).set_Parent((Container)(object)_decorWindow);
		((Control)val).set_Location(new Point(((Control)_decorWindow).get_Width() - containerWidth - 100, -2));
		((Control)val).set_Size(new Point(containerWidth, containerHeight));
		val.set_FlowDirection((ControlFlowDirection)2);
		((Panel)val).set_CanScroll(false);
		val.set_ControlPadding(new Vector2(4f, 0f));
		_infoContainer = val;
		((Control)_infoContainer).set_ZIndex(100);
		FlowPanel val2 = new FlowPanel();
		((Control)val2).set_Width((int)((double)((Control)_infoContainer).get_Width() * 0.4));
		((Control)val2).set_Height(((Control)_infoContainer).get_Height());
		((Control)val2).set_Parent((Container)(object)_infoContainer);
		val2.set_FlowDirection((ControlFlowDirection)3);
		val2.set_ControlPadding(new Vector2(0f, 2f));
		((Panel)val2).set_CanScroll(false);
		_firstPanel = val2;
		FlowPanel val3 = new FlowPanel();
		((Control)val3).set_Width(((Control)_firstPanel).get_Width() * 4);
		((Control)val3).set_Height(((Control)_infoContainer).get_Height());
		((Control)val3).set_Parent((Container)(object)_infoContainer);
		val3.set_FlowDirection((ControlFlowDirection)3);
		val3.set_ControlPadding(new Vector2(0f, 2f));
		((Panel)val3).set_CanScroll(false);
		_secondPanel = val3;
		SetInfo(new(string, string)[3]
		{
			("test/empty.png", "                                                  "),
			("test/click.png", "Double-click on an icon to go to its wiki page."),
			("test/copy.png", "Click on the name or the image to copy its name.")
		});
	}

	public static void SetInfo((string iconPath, string text)[] items)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		((Container)_firstPanel).ClearChildren();
		((Container)_secondPanel).ClearChildren();
		for (int i = 0; i < items.Length; i++)
		{
			int iconSize = 22;
			Size2 textSize = GameService.Content.get_DefaultFont14().MeasureString(items[i].text);
			int panelHeight = Math.Max(iconSize, (int)textSize.Height);
			FlowPanel parentPanel = ((i == 0) ? _firstPanel : _secondPanel);
			int panelWidth = iconSize + 4 + (int)textSize.Width;
			Panel val = new Panel();
			((Control)val).set_Size(new Point(panelWidth, panelHeight));
			((Control)val).set_Parent((Container)(object)parentPanel);
			Panel panel = val;
			Image val2 = new Image();
			((Control)val2).set_Parent((Container)(object)panel);
			((Control)val2).set_Size(new Point(iconSize, iconSize));
			((Control)val2).set_Location(new Point(0, (panelHeight - iconSize) / 2));
			val2.set_Texture(AsyncTexture2D.op_Implicit(DecorModule.DecorModuleInstance.ContentsManager.GetTexture(items[i].iconPath)));
			Image icon = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)panel);
			((Control)val3).set_Location(new Point(((Control)icon).get_Right() + 4, 0));
			((Control)val3).set_Size(new Point(panelWidth + (((Control)icon).get_Width() + 4), panelHeight));
			val3.set_Text(items[i].text);
			val3.set_Font(GameService.Content.get_DefaultFont14());
			val3.set_TextColor(Color.get_White());
			val3.set_StrokeText(false);
			val3.set_ShowShadow(false);
			val3.set_WrapText(true);
		}
	}

	public static void UpdateInfoVisible(bool visible)
	{
		((Control)_infoContainer).set_Visible(visible);
	}
}
