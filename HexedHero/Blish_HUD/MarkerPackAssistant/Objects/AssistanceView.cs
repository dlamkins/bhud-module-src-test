using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules;
using HexedHero.Blish_HUD.MarkerPackAssistant.Managers;
using HexedHero.Blish_HUD.MarkerPackAssistant.Utils;
using Microsoft.Xna.Framework;

namespace HexedHero.Blish_HUD.MarkerPackAssistant.Objects
{
	public class AssistanceView : View
	{
		private CancellationTokenSource UpdaterTaskToken = new CancellationTokenSource();

		private int MapID;

		private float CharX;

		private float CharY;

		private float CharZ;

		private FlowPanel GlobalFlowPanel;

		private Label StatsLabel;

		private FlowPanel MapIDFlowPanel;

		private Label MapIDLabel;

		private Image MapIDImage;

		private StandardButton MapIDButton;

		private FlowPanel CordsFlowPanel;

		private Label CordsLabel;

		private Image CordsImage;

		private StandardButton CordsButton;

		private Label ToolsLabel;

		private FlowPanel RunFlowPanel;

		private TextBox RunTextBox;

		private Image RunImage;

		private StandardButton RunButton;

		private FlowPanel RandomGUIDFlowPanel;

		private Label RandomGUIDLabel;

		private Image RandomGUIDImage;

		private StandardButton RandomGUIDButton;

		private FlowPanel POIFlowPanel;

		private Label POILabel;

		private Image POIImage;

		private StandardButton POIButton;

		public AssistanceView()
			: this()
		{
		}

		protected override void Unload()
		{
			FlowPanel globalFlowPanel = GlobalFlowPanel;
			if (globalFlowPanel != null)
			{
				((Control)globalFlowPanel).Dispose();
			}
			Label statsLabel = StatsLabel;
			if (statsLabel != null)
			{
				((Control)statsLabel).Dispose();
			}
			FlowPanel mapIDFlowPanel = MapIDFlowPanel;
			if (mapIDFlowPanel != null)
			{
				((Control)mapIDFlowPanel).Dispose();
			}
			Label mapIDLabel = MapIDLabel;
			if (mapIDLabel != null)
			{
				((Control)mapIDLabel).Dispose();
			}
			Image mapIDImage = MapIDImage;
			if (mapIDImage != null)
			{
				((Control)mapIDImage).Dispose();
			}
			StandardButton mapIDButton = MapIDButton;
			if (mapIDButton != null)
			{
				((Control)mapIDButton).Dispose();
			}
			FlowPanel cordsFlowPanel = CordsFlowPanel;
			if (cordsFlowPanel != null)
			{
				((Control)cordsFlowPanel).Dispose();
			}
			Label cordsLabel = CordsLabel;
			if (cordsLabel != null)
			{
				((Control)cordsLabel).Dispose();
			}
			Image cordsImage = CordsImage;
			if (cordsImage != null)
			{
				((Control)cordsImage).Dispose();
			}
			StandardButton cordsButton = CordsButton;
			if (cordsButton != null)
			{
				((Control)cordsButton).Dispose();
			}
			Label toolsLabel = ToolsLabel;
			if (toolsLabel != null)
			{
				((Control)toolsLabel).Dispose();
			}
			FlowPanel runFlowPanel = RunFlowPanel;
			if (runFlowPanel != null)
			{
				((Control)runFlowPanel).Dispose();
			}
			TextBox runTextBox = RunTextBox;
			if (runTextBox != null)
			{
				((Control)runTextBox).Dispose();
			}
			Image runImage = RunImage;
			if (runImage != null)
			{
				((Control)runImage).Dispose();
			}
			StandardButton runButton = RunButton;
			if (runButton != null)
			{
				((Control)runButton).Dispose();
			}
			FlowPanel randomGUIDFlowPanel = RandomGUIDFlowPanel;
			if (randomGUIDFlowPanel != null)
			{
				((Control)randomGUIDFlowPanel).Dispose();
			}
			Label randomGUIDLabel = RandomGUIDLabel;
			if (randomGUIDLabel != null)
			{
				((Control)randomGUIDLabel).Dispose();
			}
			Image randomGUIDImage = RandomGUIDImage;
			if (randomGUIDImage != null)
			{
				((Control)randomGUIDImage).Dispose();
			}
			StandardButton randomGUIDButton = RandomGUIDButton;
			if (randomGUIDButton != null)
			{
				((Control)randomGUIDButton).Dispose();
			}
			FlowPanel pOIFlowPanel = POIFlowPanel;
			if (pOIFlowPanel != null)
			{
				((Control)pOIFlowPanel).Dispose();
			}
			Label pOILabel = POILabel;
			if (pOILabel != null)
			{
				((Control)pOILabel).Dispose();
			}
			Image pOIImage = POIImage;
			if (pOIImage != null)
			{
				((Control)pOIImage).Dispose();
			}
			StandardButton pOIButton = POIButton;
			if (pOIButton != null)
			{
				((Control)pOIButton).Dispose();
			}
			UpdaterTaskToken?.Cancel();
		}

		protected override void Build(Container container)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Expected O, but got Unknown
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Expected O, but got Unknown
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Expected O, but got Unknown
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Expected O, but got Unknown
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Expected O, but got Unknown
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Expected O, but got Unknown
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Expected O, but got Unknown
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Expected O, but got Unknown
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Expected O, but got Unknown
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Expected O, but got Unknown
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Expected O, but got Unknown
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Expected O, but got Unknown
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_059a: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Expected O, but got Unknown
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0604: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Expected O, but got Unknown
			UpdaterTaskToken = new CancellationTokenSource();
			FlowPanel val = new FlowPanel();
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Size(new Point(280, 180));
			((Control)val).set_Parent(container);
			GlobalFlowPanel = val;
			Label val2 = new Label();
			val2.set_Text("Stats:");
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(new Point(75, 25));
			((Control)val2).set_Parent((Container)(object)GlobalFlowPanel);
			StatsLabel = val2;
			FlowPanel val3 = new FlowPanel();
			val3.set_FlowDirection((ControlFlowDirection)2);
			((Control)val3).set_Size(new Point(280, 25));
			((Control)val3).set_Parent((Container)(object)GlobalFlowPanel);
			MapIDFlowPanel = val3;
			StandardButton val4 = new StandardButton();
			val4.set_Text("Copy");
			((Control)val4).set_Location(new Point(0, 0));
			((Control)val4).set_Size(new Point(70, 25));
			((Control)val4).set_Parent((Container)(object)MapIDFlowPanel);
			MapIDButton = val4;
			((Control)MapIDButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				MapIDButton.set_Text("Copied");
				((Control)MapIDButton).set_Enabled(false);
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(MapID.ToString());
				await Task.Delay(333);
				MapIDButton.set_Text("Copy");
				((Control)MapIDButton).set_Enabled(true);
			});
			Image val5 = new Image();
			((Control)val5).set_Location(new Point(0, 0));
			((Control)val5).set_Size(new Point(25, 25));
			val5.set_Texture(AsyncTexture2D.FromAssetId(716655));
			((Control)val5).set_Parent((Container)(object)MapIDFlowPanel);
			MapIDImage = val5;
			Label val6 = new Label();
			val6.set_Text("ERROR");
			((Control)val6).set_Location(new Point(0, 0));
			((Control)val6).set_Size(new Point(75, 25));
			((Control)val6).set_Parent((Container)(object)MapIDFlowPanel);
			MapIDLabel = val6;
			FlowPanel val7 = new FlowPanel();
			val7.set_FlowDirection((ControlFlowDirection)2);
			((Control)val7).set_Size(new Point(280, 25));
			((Control)val7).set_Parent((Container)(object)GlobalFlowPanel);
			CordsFlowPanel = val7;
			StandardButton val8 = new StandardButton();
			val8.set_Text("Copy");
			((Control)val8).set_Location(new Point(0, 0));
			((Control)val8).set_Size(new Point(70, 25));
			((Control)val8).set_Parent((Container)(object)CordsFlowPanel);
			CordsButton = val8;
			((Control)CordsButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				CordsButton.set_Text("Copied");
				((Control)CordsButton).set_Enabled(false);
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(FormattableString.Invariant($"xpos=\"{CharX}\" ypos=\"{CharY}\" zpos=\"{CharZ}\""));
				await Task.Delay(333);
				CordsButton.set_Text("Copy");
				((Control)CordsButton).set_Enabled(true);
			});
			Image val9 = new Image();
			((Control)val9).set_Location(new Point(0, 0));
			((Control)val9).set_Size(new Point(25, 25));
			val9.set_Texture(AsyncTexture2D.FromAssetId(716655));
			((Control)val9).set_Parent((Container)(object)CordsFlowPanel);
			CordsImage = val9;
			Label val10 = new Label();
			val10.set_Text("ERROR");
			((Control)val10).set_Location(new Point(0, 0));
			((Control)val10).set_Size(new Point(260, 25));
			((Control)val10).set_Parent((Container)(object)CordsFlowPanel);
			CordsLabel = val10;
			Label val11 = new Label();
			val11.set_Text("Tools:");
			((Control)val11).set_Location(new Point(0, 0));
			((Control)val11).set_Size(new Point(75, 25));
			((Control)val11).set_Parent((Container)(object)GlobalFlowPanel);
			ToolsLabel = val11;
			FlowPanel val12 = new FlowPanel();
			val12.set_FlowDirection((ControlFlowDirection)2);
			((Control)val12).set_Size(new Point(280, 25));
			((Control)val12).set_Parent((Container)(object)GlobalFlowPanel);
			RunFlowPanel = val12;
			StandardButton val13 = new StandardButton();
			val13.set_Text("Run");
			((Control)val13).set_Location(new Point(0, 0));
			((Control)val13).set_Size(new Point(70, 25));
			((Control)val13).set_Parent((Container)(object)RunFlowPanel);
			RunButton = val13;
			((Control)RunButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				RunButton.set_Text("Running");
				((Control)RunButton).set_Enabled(false);
				await Task.Run(async delegate
				{
					string path = ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.get_Value();
					if (File.Exists(path) && Path.GetExtension(path).Equals(".bat"))
					{
						Process process = new Process();
						process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
						process.StartInfo.FileName = path;
						process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						process.StartInfo.CreateNoWindow = true;
						process.Start();
						if (!process.WaitForExit(30000))
						{
							process.Kill();
							RunButton.set_Text("KILLED");
							await Task.Delay(3000);
						}
						await Task.Delay(25);
						foreach (ModuleManager moduleManager in GameService.Module.get_Modules())
						{
							if (moduleManager.get_Manifest().get_Namespace().ToLower()
								.Equals("bh.community.pathing") && moduleManager.get_Enabled())
							{
								Reflection.ReloadPathingMarkers(moduleManager);
								await Task.Delay(250);
							}
						}
					}
					else
					{
						RunButton.set_Text("INVALID");
						await Task.Delay(1000);
					}
				});
				RunButton.set_Text("Run");
				((Control)RunButton).set_Enabled(true);
			});
			Image val14 = new Image();
			((Control)val14).set_Location(new Point(0, 0));
			((Control)val14).set_Size(new Point(25, 25));
			val14.set_Texture(AsyncTexture2D.FromAssetId(716655));
			((Control)val14).set_Parent((Container)(object)RunFlowPanel);
			RunImage = val14;
			TextBox val15 = new TextBox();
			((TextInputBase)val15).set_Text(ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.get_Value());
			((TextInputBase)val15).set_Font(GameService.Content.get_DefaultFont12());
			((Control)val15).set_Location(new Point(0, 0));
			((Control)val15).set_Size(new Point(180, 25));
			((Control)val15).set_Parent((Container)(object)RunFlowPanel);
			RunTextBox = val15;
			((TextInputBase)RunTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				ModuleSettingsManager.Instance.ModuleSettings.MarkerPackBuildPath.set_Value(((TextInputBase)RunTextBox).get_Text());
			});
			FlowPanel val16 = new FlowPanel();
			val16.set_FlowDirection((ControlFlowDirection)2);
			((Control)val16).set_Size(new Point(280, 25));
			((Control)val16).set_Parent((Container)(object)GlobalFlowPanel);
			RandomGUIDFlowPanel = val16;
			StandardButton val17 = new StandardButton();
			val17.set_Text("Copy");
			((Control)val17).set_Location(new Point(0, 0));
			((Control)val17).set_Size(new Point(70, 25));
			((Control)val17).set_Parent((Container)(object)RandomGUIDFlowPanel);
			RandomGUIDButton = val17;
			((Control)RandomGUIDButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				RandomGUIDButton.set_Text("Copied");
				((Control)RandomGUIDButton).set_Enabled(false);
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync(Common.GetRandomGUID());
				await Task.Delay(333);
				RandomGUIDButton.set_Text("Copy");
				((Control)RandomGUIDButton).set_Enabled(true);
			});
			Image val18 = new Image();
			((Control)val18).set_Location(new Point(0, 0));
			((Control)val18).set_Size(new Point(25, 25));
			val18.set_Texture(AsyncTexture2D.FromAssetId(716655));
			((Control)val18).set_Parent((Container)(object)RandomGUIDFlowPanel);
			RandomGUIDImage = val18;
			Label val19 = new Label();
			val19.set_Text("Random GUID (Base64)");
			((Control)val19).set_Location(new Point(0, 0));
			((Control)val19).set_Size(new Point(200, 25));
			((Control)val19).set_Parent((Container)(object)RandomGUIDFlowPanel);
			RandomGUIDLabel = val19;
			FlowPanel val20 = new FlowPanel();
			val20.set_FlowDirection((ControlFlowDirection)2);
			((Control)val20).set_Size(new Point(280, 25));
			((Control)val20).set_Parent((Container)(object)GlobalFlowPanel);
			POIFlowPanel = val20;
			StandardButton val21 = new StandardButton();
			val21.set_Text("Copy");
			((Control)val21).set_Location(new Point(0, 0));
			((Control)val21).set_Size(new Point(70, 25));
			((Control)val21).set_Parent((Container)(object)POIFlowPanel);
			POIButton = val21;
			((Control)POIButton).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				POIButton.set_Text("Copied");
				((Control)POIButton).set_Enabled(false);
				string Map = $"{MapID}";
				string Position = FormattableString.Invariant($"xpos=\"{CharX}\" ypos=\"{CharY}\" zpos=\"{CharZ}\"");
				string randomGUID = Common.GetRandomGUID();
				ClipboardUtil.get_WindowsClipboardService().SetTextAsync("<POI MapID=\"" + Map + "\" " + Position + " GUID=\"" + randomGUID + "\"/>");
				await Task.Delay(333);
				POIButton.set_Text("Copy");
				((Control)POIButton).set_Enabled(true);
			});
			Image val22 = new Image();
			((Control)val22).set_Location(new Point(0, 0));
			((Control)val22).set_Size(new Point(25, 25));
			val22.set_Texture(AsyncTexture2D.FromAssetId(716655));
			((Control)val22).set_Parent((Container)(object)POIFlowPanel);
			POIImage = val22;
			Label val23 = new Label();
			val23.set_Text("Create POI");
			((Control)val23).set_Location(new Point(0, 0));
			((Control)val23).set_Size(new Point(200, 25));
			((Control)val23).set_Parent((Container)(object)POIFlowPanel);
			POILabel = val23;
			UpdateMapID(null, null);
			UpdateCords(null, null);
			GameService.Gw2Mumble.get_CurrentMap().add_MapChanged((EventHandler<ValueEventArgs<int>>)UpdateMapID);
			Task.Run(async delegate
			{
				while (!UpdaterTaskToken.Token.IsCancellationRequested)
				{
					UpdateCords(null, null);
					await Task.Delay(25, UpdaterTaskToken.Token);
				}
			});
		}

		public void UpdateMapID(object sender, ValueEventArgs<int> e)
		{
			MapID = GameService.Gw2Mumble.get_CurrentMap().get_Id();
			MapIDLabel.set_Text("Map ID: %id%".Replace("%id%", MapID.ToString()));
		}

		public void UpdateCords(object sender, ValueEventArgs<int> e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			CharX = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().X;
			CharY = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
			CharZ = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Y;
			CordsLabel.set_Text("XYZ: %location%".Replace("%location%", CharX.ToString("F2") + ", " + CharY.ToString("F2") + ", " + CharZ.ToString("F2")));
		}
	}
}
