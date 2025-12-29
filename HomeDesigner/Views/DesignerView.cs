using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner.Loader;
using HomeDesigner.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HomeDesigner.Views
{
	public class DesignerView : View
	{
		private readonly ContentsManager contents;

		private Dictionary<BlueprintObject, Quaternion> _startRotations = new Dictionary<BlueprintObject, Quaternion>();

		private Dictionary<BlueprintObject, Vector3> _startPositions = new Dictionary<BlueprintObject, Vector3>();

		private List<int> textboxDecorations = new List<int> { 455, 186, 171, 336 };

		private FlowPanel mainPanel;

		private FlowPanel decoPanel = new FlowPanel();

		private Dictionary<int, FlowPanel> categoryPanels = new Dictionary<int, FlowPanel>();

		private RendererControl rendererControl;

		private BlueprintRenderer blueprintRenderer;

		private int selectedModelKey = -1;

		private FlowPanel selectedObjectsPanel = new FlowPanel();

		private Toolbar toolbar;

		private Image menuImage;

		private ContextMenuStrip menu;

		private Image currentImage;

		private StandardButton placeButton;

		private StandardButton toolbarButton;

		private Container buildPanel;

		private FlowPanel bottomPanel;

		private Label instructionPanel;

		public Vector3 CopiedPivot { get; private set; }

		public DesignerView(RendererControl rendererControl, BlueprintRenderer blueprintRenderer, ContentsManager contents)
			: this()
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			this.rendererControl = rendererControl;
			this.blueprintRenderer = blueprintRenderer;
			this.contents = contents;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Expected O, but got Unknown
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Expected O, but got Unknown
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Expected O, but got Unknown
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Expected O, but got Unknown
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Expected O, but got Unknown
			toolbar = new Toolbar(contents, rendererControl, this);
			this.buildPanel = buildPanel;
			Image val = new Image(AsyncTexture2D.op_Implicit(GameService.Content.GetTexture("155052")));
			((Control)val).set_Parent(buildPanel);
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)val).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Right() - 62, 0));
			((Control)val).set_Size(new Point(32, 32));
			menuImage = val;
			ContextMenuStrip val2 = new ContextMenuStrip();
			((Control)val2).set_Parent(buildPanel);
			((Control)val2).set_Size(new Point(150, 50));
			((Control)val2).set_Visible(false);
			menu = val2;
			((Control)menu.AddMenuItem("Save Template")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveTemplate();
			});
			((Control)menu.AddMenuItem("Save Selection")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveSelectionTemplate();
			});
			((Control)menu.AddMenuItem("Load Template")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadTemplate();
			});
			((Control)menu.AddMenuItem("Add Template")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddTemplate();
			});
			((Control)menu.AddMenuItem("Remove All Objects")).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				RemoveAllObjects();
			});
			((Control)menuImage).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				menu.Show((Control)(object)menuImage);
			});
			FlowPanel val3 = new FlowPanel();
			((Control)val3).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height));
			contentRegion = buildPanel.get_ContentRegion();
			((Control)val3).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Left(), 40));
			val3.set_FlowDirection((ControlFlowDirection)3);
			((Control)val3).set_Parent(buildPanel);
			mainPanel = val3;
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)mainPanel);
			((Control)val4).set_Width(((Container)mainPanel).get_ContentRegion().Width - 30);
			((Control)val4).set_Height((int)((double)((Container)mainPanel).get_ContentRegion().Height / 1.5));
			val4.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val4).set_CanScroll(true);
			((Panel)val4).set_ShowBorder(true);
			val4.set_ControlPadding(new Vector2(5f, 5f));
			decoPanel = val4;
			createCategoryPanels();
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)mainPanel);
			val5.set_Text("Place Blueprint");
			((Control)val5).set_Width(((Container)mainPanel).get_ContentRegion().Width - 30);
			((Control)val5).set_Height(45);
			placeButton = val5;
			((Control)placeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				PlaceSelectedModel();
			});
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)mainPanel);
			val6.set_Text("Toolbar");
			((Control)val6).set_Size(new Point(140, 30));
			((Control)val6).set_Visible(true);
			toolbarButton = val6;
			((Control)toolbarButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((WindowBase2)toolbar).ToggleWindow();
			});
			FlowPanel val7 = new FlowPanel();
			((Control)val7).set_Parent((Container)(object)mainPanel);
			((Control)val7).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width - 30, ((Container)mainPanel).get_ContentRegion().Height / 4));
			val7.set_FlowDirection((ControlFlowDirection)2);
			bottomPanel = val7;
			FlowPanel val8 = new FlowPanel();
			((Control)val8).set_Parent((Container)(object)bottomPanel);
			((Control)val8).set_Size(new Point(((Container)bottomPanel).get_ContentRegion().Width / 2 - 10, ((Container)bottomPanel).get_ContentRegion().Height));
			val8.set_FlowDirection((ControlFlowDirection)3);
			((Panel)val8).set_CanScroll(true);
			((Panel)val8).set_ShowBorder(true);
			selectedObjectsPanel = val8;
			RefreshSelectedList();
			Label val9 = new Label();
			((Control)val9).set_Parent((Container)(object)bottomPanel);
			((Control)val9).set_Size(new Point(((Container)bottomPanel).get_ContentRegion().Width / 2 - 10, 50));
			val9.set_Text("Hover here for Tipps [ ]");
			((Control)val9).set_BasicTooltipText("\n- For editing click once, don't hold mouse\n- Press T to cancel your edit\n- Press 7 to create a copy at the new position\n- Hold Alt to select multiple objects\n- Hold Shift when rotating to rotate in 45° steps\n- Rectangle/Lasso selections are set at the base ground, so check perspective when being above/below\n- Rectangle/Lasso selections are not limited by height\n- Backup Templates before overwriting your files. Just in case");
			val9.set_WrapText(true);
			instructionPanel = val9;
			((Control)buildPanel).add_Resized((EventHandler<ResizedEventArgs>)resized);
		}

		public void SubscribeSelectionEvents()
		{
			GameService.Input.get_Mouse().add_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseSelection);
		}

		public void UnsubscribeSelectionEvents()
		{
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseSelection);
		}

		private void OnLeftMouseSelection(object sender, MouseEventArgs e)
		{
			rendererControl.OnSelectionClick(this);
		}

		public void SetTransformMode(RendererControl.TransformMode mode)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			rendererControl.currentMode = mode;
			Color.get_LightGreen();
			Color.get_Transparent();
			switch (mode)
			{
			case RendererControl.TransformMode.Translate:
				rendererControl.gizmoMode = RendererControl.GizmoMode.Translate;
				break;
			case RendererControl.TransformMode.Rotate:
				rendererControl.gizmoMode = RendererControl.GizmoMode.Rotate;
				break;
			case RendererControl.TransformMode.Scale:
				rendererControl.gizmoMode = RendererControl.GizmoMode.Scale;
				break;
			}
		}

		private void createCategoryPanels()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			categoryPanels.Clear();
			foreach (KeyValuePair<int, string> cat in blueprintRenderer.decoCategories)
			{
				FlowPanel val = new FlowPanel();
				((Control)val).set_Parent((Container)(object)decoPanel);
				((Control)val).set_Width(((Container)decoPanel).get_ContentRegion().Width - 20);
				((Panel)val).set_Title(cat.Value);
				val.set_FlowDirection((ControlFlowDirection)0);
				((Panel)val).set_CanCollapse(true);
				((Panel)val).set_ShowBorder(true);
				((Container)val).set_HeightSizingMode((SizingMode)1);
				((Container)val).set_AutoSizePadding(new Point(5, 5));
				val.set_ControlPadding(new Vector2(5f, 5f));
				val.set_OuterControlPadding(new Vector2(5f, 5f));
				FlowPanel panel = val;
				categoryPanels.Add(cat.Key, panel);
			}
			fillCategoryPanels();
		}

		private void fillCategoryPanels()
		{
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			foreach (KeyValuePair<int, FlowPanel> categoryPanel in categoryPanels)
			{
				((Container)categoryPanel.Value).ClearChildren();
			}
			foreach (KeyValuePair<int, Decoration> deco in blueprintRenderer.decorationLut.decorations)
			{
				foreach (int cat in deco.Value.categories)
				{
					int key = deco.Key;
					AsyncTexture2D texture = blueprintRenderer.decoIconDict[key];
					Image val = new Image(texture);
					((Control)val).set_Parent((Container)(object)categoryPanels[cat]);
					((Control)val).set_Size(new Point(64, 64));
					((Control)val).set_BasicTooltipText(deco.Value.name);
					Image img = val;
					((Control)img).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						selectedModelKey = key;
						setCurrentImage(img);
					});
				}
			}
		}

		private void setCurrentImage(Image image)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (currentImage != null)
			{
				currentImage.set_Tint(Color.get_White());
				((Control)currentImage).set_BackgroundColor(Color.get_Transparent());
				((Control)currentImage).set_Opacity(100f);
			}
			image.set_Tint(new Color(173, 216, 250, 128));
			((Control)image).set_BackgroundColor(Color.get_LightBlue());
			((Control)image).set_Opacity(75f);
			currentImage = image;
		}

		public void RefreshSelectedList()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			((Container)selectedObjectsPanel).ClearChildren();
			if (!rendererControl.SelectedObjects.Any())
			{
				Label val = new Label();
				((Control)val).set_Parent((Container)(object)selectedObjectsPanel);
				val.set_Text("No Objects Selected.");
				val.set_AutoSizeWidth(true);
				return;
			}
			foreach (BlueprintObject obj in rendererControl.SelectedObjects)
			{
				Panel val2 = new Panel();
				((Control)val2).set_Parent((Container)(object)selectedObjectsPanel);
				((Control)val2).set_Width(((Container)selectedObjectsPanel).get_ContentRegion().Width - 10);
				((Control)val2).set_Height(25);
				Panel row = val2;
				Label val3 = new Label();
				((Control)val3).set_Parent((Container)(object)row);
				val3.set_Text(obj.Name);
				((Control)val3).set_Location(new Point(5, 5));
				val3.set_AutoSizeWidth(true);
			}
		}

		private void PlaceSelectedModel()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (selectedModelKey == -1)
			{
				return;
			}
			BlueprintObject newObj = new BlueprintObject
			{
				ModelKey = selectedModelKey.ToString(),
				Name = blueprintRenderer.decorationLut.decorations[selectedModelKey].name,
				Position = GameService.Gw2Mumble.get_PlayerCharacter().get_Position(),
				Rotation = new Vector3(0f, 0f, 0f),
				Id = selectedModelKey,
				Scale = 1f,
				InternalId = rendererControl.internalObjectId,
				IsOriginal = true
			};
			if (textboxDecorations.Contains(selectedModelKey))
			{
				TextDialog textDialog = new TextDialog(contents);
				newObj.payloadPT = "1";
				newObj.payloadV = "1";
				newObj.payloadValue = "";
				textDialog.TextConfirmed += delegate(string payload)
				{
					newObj.payloadValue = payload;
				};
				((Control)textDialog).Show();
			}
			rendererControl.internalObjectId++;
			rendererControl.AddObject(newObj);
			rendererControl.updateHistoryList();
		}

		public void RemoveSelectedObject()
		{
			rendererControl.Objects.RemoveAll((BlueprintObject o) => rendererControl.SelectedObjects.Contains(o));
			rendererControl.SelectedObjects.Clear();
			rendererControl.updateHistoryList();
			ScreenNotification.ShowNotification("Selection Removed", (NotificationType)0, (Texture2D)null, 4);
		}

		private void RemoveAllObjects()
		{
			ConfirmDialog confirmDialog = new ConfirmDialog(contents, "Do you really want to remove all blueprints?");
			confirmDialog.confirmed += delegate(bool result)
			{
				if (result)
				{
					rendererControl.SelectedObjects.Clear();
					rendererControl.Objects.Clear();
					rendererControl.updateHistoryList();
					ScreenNotification.ShowNotification("All Blueprints Removed", (NotificationType)0, (Texture2D)null, 4);
				}
			};
			((Control)confirmDialog).Show();
		}

		public void CopySelectedObject()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (rendererControl.SelectedObjects.Count == 0)
			{
				ScreenNotification.ShowNotification("No Object Selected", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			CopiedPivot = rendererControl.getPivotObject();
			rendererControl.CopiedObjects.Clear();
			foreach (BlueprintObject obj in rendererControl.SelectedObjects)
			{
				BlueprintObject copy = new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = obj.Position,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					CachedWorld = obj.CachedWorld,
					InternalId = obj.InternalId,
					IsOriginal = false,
					Selected = false
				};
				rendererControl.internalObjectId++;
				rendererControl.CopiedObjects.Add(copy);
			}
			ScreenNotification.ShowNotification("Selection Copied", (NotificationType)0, (Texture2D)null, 4);
		}

		public void PasteObject()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			if (rendererControl.CopiedObjects.Count == 0)
			{
				ScreenNotification.ShowNotification("No Object To Paste", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			Vector3 oldPivot = CopiedPivot;
			foreach (BlueprintObject obj in rendererControl.CopiedObjects)
			{
				Vector3 offset = obj.Position - oldPivot;
				Vector3 newPosition = GameService.Gw2Mumble.get_PlayerCharacter().get_Position() + offset;
				BlueprintObject copy = new BlueprintObject
				{
					ModelKey = obj.ModelKey,
					Id = obj.Id,
					Name = obj.Name,
					Position = newPosition,
					Rotation = obj.Rotation,
					RotationQuaternion = obj.RotationQuaternion,
					Scale = obj.Scale,
					InternalId = rendererControl.internalObjectId,
					IsOriginal = true,
					Selected = false
				};
				rendererControl.AddObject(copy);
			}
			rendererControl.updateHistoryList();
			ScreenNotification.ShowNotification("Copied Objects Pasted", (NotificationType)0, (Texture2D)null, 4);
		}

		private void SaveTemplate()
		{
			string mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id().ToString();
			XDocument xmlDoc = XmlLoader.SaveBlueprintObjectsToXml(rendererControl.Objects, mapId);
			if (xmlDoc == null)
			{
				ScreenNotification.ShowNotification("An error occoured. There seem to be no decorations.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			SaveDialog saveDialog = new SaveDialog(contents, xmlDoc);
			saveDialog.TemplateSaved += delegate
			{
				ScreenNotification.ShowNotification("Template Saved", (NotificationType)0, (Texture2D)null, 4);
			};
			((Control)saveDialog).Show();
		}

		private void SaveSelectionTemplate()
		{
			string mapId = GameService.Gw2Mumble.get_CurrentMap().get_Id().ToString();
			XDocument xmlDoc = XmlLoader.SaveBlueprintObjectsToXml(rendererControl.SelectedObjects, mapId);
			if (xmlDoc == null)
			{
				ScreenNotification.ShowNotification("An error occoured. Seems like no decorations were selected.", (NotificationType)0, (Texture2D)null, 4);
				return;
			}
			SaveDialog saveDialog = new SaveDialog(contents, xmlDoc);
			saveDialog.TemplateSaved += delegate
			{
				ScreenNotification.ShowNotification("Template Saved", (NotificationType)0, (Texture2D)null, 4);
			};
			((Control)saveDialog).Show();
		}

		private void LoadTemplate()
		{
			LoadDialog loadDialog = new LoadDialog(contents);
			loadDialog.TemplateSelected += delegate(string path)
			{
				try
				{
					rendererControl.Objects.Clear();
					rendererControl.SelectedObjects.Clear();
					foreach (BlueprintObject current in XmlLoader.LoadBlueprintObjectsFromXml(path))
					{
						current.InternalId = rendererControl.internalObjectId;
						rendererControl.internalObjectId++;
						current.IsOriginal = true;
						rendererControl.Objects.Add(current);
					}
					rendererControl.updateWorld();
					ScreenNotification.ShowNotification("Template Loaded", (NotificationType)0, (Texture2D)null, 4);
				}
				catch (Exception ex)
				{
					ScreenNotification.ShowNotification("Loading Failed\n" + ex.Message, (NotificationType)0, (Texture2D)null, 4);
				}
				rendererControl.resetHistory();
			};
			((Control)loadDialog).Show();
		}

		private void AddTemplate()
		{
			LoadDialog loadDialog = new LoadDialog(contents);
			loadDialog.TemplateSelected += delegate(string path)
			{
				try
				{
					rendererControl.SelectedObjects.Clear();
					foreach (BlueprintObject current in XmlLoader.LoadBlueprintObjectsFromXml(path))
					{
						current.InternalId = rendererControl.internalObjectId;
						rendererControl.internalObjectId++;
						current.IsOriginal = true;
						rendererControl.Objects.Add(current);
					}
					rendererControl.updateWorld();
					rendererControl.updateHistoryList();
					ScreenNotification.ShowNotification("Template Added", (NotificationType)0, (Texture2D)null, 4);
				}
				catch
				{
					ScreenNotification.ShowNotification("Loading Failed", (NotificationType)0, (Texture2D)null, 4);
				}
			};
			((Control)loadDialog).Show();
		}

		private void resized(object sender, ResizedEventArgs e)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			Image obj = menuImage;
			Rectangle contentRegion = buildPanel.get_ContentRegion();
			((Control)obj).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Right() - 62, 0));
			FlowPanel obj2 = mainPanel;
			contentRegion = buildPanel.get_ContentRegion();
			((Control)obj2).set_Location(new Point(((Rectangle)(ref contentRegion)).get_Left(), 40));
			((Control)mainPanel).set_Size(new Point(buildPanel.get_ContentRegion().Width, buildPanel.get_ContentRegion().Height));
			((Control)decoPanel).set_Width(((Container)mainPanel).get_ContentRegion().Width - 30);
			((Control)decoPanel).set_Height((int)((double)((Container)mainPanel).get_ContentRegion().Height / 1.5));
			((Control)placeButton).set_Width(((Container)mainPanel).get_ContentRegion().Width - 30);
			((Control)bottomPanel).set_Size(new Point(((Container)mainPanel).get_ContentRegion().Width - 30, ((Container)mainPanel).get_ContentRegion().Height / 3));
			((Control)selectedObjectsPanel).set_Size(new Point(((Container)bottomPanel).get_ContentRegion().Width / 2 - 30, ((Container)bottomPanel).get_ContentRegion().Height));
			((Control)instructionPanel).set_Size(new Point(((Container)bottomPanel).get_ContentRegion().Width / 2 - 30, 20));
			foreach (KeyValuePair<int, FlowPanel> categoryPanel in categoryPanels)
			{
				((Control)categoryPanel.Value).set_Width(((Container)decoPanel).get_ContentRegion().Width - 20);
			}
		}

		public void unload()
		{
			toolbar?.unload();
			((Control)menu.AddMenuItem("Save Template")).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveTemplate();
			});
			((Control)menu.AddMenuItem("Save Selection")).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				SaveSelectionTemplate();
			});
			((Control)menu.AddMenuItem("Load Template")).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				LoadTemplate();
			});
			((Control)menu.AddMenuItem("Add Template")).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				AddTemplate();
			});
			((Control)menu.AddMenuItem("Remove All Objects")).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				RemoveAllObjects();
			});
			((Control)menuImage).remove_Click((EventHandler<MouseEventArgs>)delegate
			{
				menu.Show((Control)(object)menuImage);
			});
			((Control)buildPanel).remove_Resized((EventHandler<ResizedEventArgs>)resized);
			GameService.Input.get_Mouse().remove_LeftMouseButtonPressed((EventHandler<MouseEventArgs>)OnLeftMouseSelection);
			Toolbar obj = toolbar;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			foreach (KeyValuePair<int, FlowPanel> categoryPanel in categoryPanels)
			{
				((Container)categoryPanel.Value).ClearChildren();
			}
		}
	}
}
