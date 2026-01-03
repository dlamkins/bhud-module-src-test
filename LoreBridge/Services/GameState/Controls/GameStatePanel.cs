using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace LoreBridge.Services.GameState.Controls
{
	public class GameStatePanel : IDisposable
	{
		private readonly FlowPanel _flowPanel;

		public GameStatePanel()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_FlowDirection((ControlFlowDirection)3);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Control)val).set_Location(new Point(60, 35));
			_flowPanel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)_flowPanel);
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_White());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			val2.set_ShowShadow(true);
			val2.set_Text("Cutscene: False");
			Label label1 = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)_flowPanel);
			val3.set_Font(GameService.Content.get_DefaultFont16());
			val3.set_TextColor(Color.get_White());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			val3.set_ShowShadow(true);
			val3.set_Text("Dialog: False");
			Label label2 = val3;
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)_flowPanel);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			val4.set_TextColor(Color.get_White());
			val4.set_AutoSizeWidth(true);
			val4.set_AutoSizeHeight(true);
			val4.set_ShowShadow(true);
			val4.set_Text("Vista/Cutscene: False");
			Label label3 = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_flowPanel);
			val5.set_Font(GameService.Content.get_DefaultFont16());
			val5.set_TextColor(Color.get_White());
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			val5.set_ShowShadow(true);
			val5.set_Text("In game: False");
			Label label4 = val5;
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_flowPanel);
			val6.set_Font(GameService.Content.get_DefaultFont16());
			val6.set_TextColor(Color.get_White());
			val6.set_AutoSizeWidth(true);
			val6.set_AutoSizeHeight(true);
			val6.set_ShowShadow(true);
			val6.set_Text("Loading/Char Select/Map: False");
			Label label5 = val6;
			Label val7 = new Label();
			((Control)val7).set_Parent((Container)(object)_flowPanel);
			val7.set_Font(GameService.Content.get_DefaultFont16());
			val7.set_TextColor(Color.get_White());
			val7.set_AutoSizeWidth(true);
			val7.set_AutoSizeHeight(true);
			val7.set_ShowShadow(true);
			val7.set_Text("Unknown: False");
			Label label6 = val7;
			Label val8 = new Label();
			((Control)val8).set_Parent((Container)(object)_flowPanel);
			val8.set_Font(GameService.Content.get_DefaultFont16());
			val8.set_TextColor(Color.get_Green());
			val8.set_AutoSizeWidth(true);
			val8.set_AutoSizeHeight(true);
			val8.set_ShowShadow(true);
			val8.set_Text("None: True");
			Label label7 = val8;
			Service.GameState.GameStateChanged += delegate(object o, GameStateType e)
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				label1.set_Text("Cutscene: " + (e == GameStateType.Cutscene));
				label1.set_TextColor((e == GameStateType.Cutscene) ? Color.get_Green() : Color.get_White());
				label2.set_Text("Dialog: " + (e == GameStateType.Dialog));
				label2.set_TextColor((e == GameStateType.Dialog) ? Color.get_Green() : Color.get_White());
				label3.set_Text("Vista/Cutscene: " + (e == GameStateType.VistaOrCutscene));
				label3.set_TextColor((e == GameStateType.VistaOrCutscene) ? Color.get_Green() : Color.get_White());
				label4.set_Text("Is game: " + (e == GameStateType.InGame));
				label4.set_TextColor((e == GameStateType.InGame) ? Color.get_Green() : Color.get_White());
				label5.set_Text("Loading/Char Select/Map: " + (e == GameStateType.LoadingOrCharacterSelection));
				label5.set_TextColor((e == GameStateType.LoadingOrCharacterSelection) ? Color.get_Green() : Color.get_White());
				label6.set_Text("Unknown: " + (e == GameStateType.Unknown));
				label6.set_TextColor((e == GameStateType.Unknown) ? Color.get_Green() : Color.get_White());
				label7.set_Text("None: " + (e == GameStateType.None));
				label7.set_TextColor((e == GameStateType.None) ? Color.get_Green() : Color.get_White());
			};
		}

		public void Dispose()
		{
			((Control)_flowPanel).Dispose();
		}
	}
}
