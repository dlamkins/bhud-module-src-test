using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using HomeDesigner.Views;
using Microsoft.Xna.Framework;

namespace HomeDesigner.Windows
{
	public class Toolbar : StandardWindow
	{
		private ContentsManager contents;

		private RendererControl rendererControl;

		private FlowPanel mainPanel;

		private Image moveIcon;

		private Image rotateIcon;

		private Image scaleIcon;

		private Image axisIcon;

		private Image copyIcon;

		private Image pasteIcon;

		private Image removeIcon;

		private Image rectangleIcon;

		private Image lassoIcon;

		public Toolbar(ContentsManager contents, RendererControl rendererControl, DesignerView designerView)
			: this(contents.GetTexture("WindowBackground.png"), new Rectangle(40, 26, 913, 750), new Rectangle(40, 26, 913, 750))
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Expected O, but got Unknown
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Expected O, but got Unknown
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Expected O, but got Unknown
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Expected O, but got Unknown
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Expected O, but got Unknown
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Expected O, but got Unknown
			//IL_042c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0463: Expected O, but got Unknown
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d1: Expected O, but got Unknown
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_051c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Expected O, but got Unknown
			//IL_0576: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_058a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Expected O, but got Unknown
			//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0603: Unknown result type (might be due to invalid IL or missing references)
			//IL_060a: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Expected O, but got Unknown
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_065c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0678: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Expected O, but got Unknown
			Toolbar toolbar = this;
			this.contents = contents;
			this.rendererControl = rendererControl;
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Size(new Point(730, 190));
			((Control)this).set_Location(new Point(680, 950));
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_Title("");
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Undo");
			((Control)val).set_Location(new Point(15, 0));
			((Control)val).set_Size(new Point(50, 30));
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (rendererControl.historyPosition > 1)
				{
					rendererControl.ClearSelection();
					rendererControl.historyPosition--;
					rendererControl.loadHistory();
					rendererControl.ClearSelection();
				}
			});
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Redo");
			((Control)val2).set_Location(new Point(75, 0));
			((Control)val2).set_Size(new Point(50, 30));
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (rendererControl.historyPosition < rendererControl.HistoryList.Count)
				{
					rendererControl.ClearSelection();
					rendererControl.historyPosition++;
					rendererControl.loadHistory();
					rendererControl.ClearSelection();
				}
			});
			Checkbox val3 = new Checkbox();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Set Multi");
			((Control)val3).set_BasicTooltipText("Check to enable selection multiple rectanlges/lassos.\nUncheck to make a new selection each rectangle/lasso.");
			((Control)val3).set_Location(new Point(570, 0));
			((Control)val3).set_Size(new Point(100, 30));
			Checkbox multiSelect = val3;
			multiSelect.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				if (multiSelect.get_Checked())
				{
					rendererControl.multiLassoSelect = true;
				}
				else
				{
					rendererControl.multiLassoSelect = false;
				}
			});
			Checkbox val4 = new Checkbox();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Set Base");
			((Control)val4).set_BasicTooltipText("Check to set Rectangle/Lasso's base to player's height.\nUncheck to set base to ground level.");
			((Control)val4).set_Location(new Point(490, 0));
			((Control)val4).set_Size(new Point(100, 30));
			Checkbox selectionBase = val4;
			selectionBase.add_CheckedChanged((EventHandler<CheckChangedEvent>)delegate
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (selectionBase.get_Checked())
				{
					rendererControl.planeZ = GameService.Gw2Mumble.get_PlayerCharacter().get_Position().Z;
				}
				else if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1596)
				{
					rendererControl.planeZ = 1f;
				}
				else if (GameService.Gw2Mumble.get_CurrentMap().get_Id() == 1558)
				{
					rendererControl.planeZ = 15f;
				}
			});
			FlowPanel val5 = new FlowPanel();
			((Panel)val5).set_ShowBorder(false);
			((Control)val5).set_Size(new Point(((Container)this).get_ContentRegion().Width, ((Container)this).get_ContentRegion().Height));
			((Control)val5).set_Location(new Point(0, 35));
			val5.set_FlowDirection((ControlFlowDirection)2);
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_ControlPadding(new Vector2(15f, 5f));
			val5.set_OuterControlPadding(new Vector2(15f, 5f));
			mainPanel = val5;
			Image val6 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Move.png")));
			((Control)val6).set_Size(new Point(64, 64));
			((Control)val6).set_BasicTooltipText("Move");
			((Control)val6).set_ZIndex(1);
			((Control)val6).set_Parent((Container)(object)mainPanel);
			val6.set_Tint(new Color(250, 250, 80, 128));
			moveIcon = val6;
			((Control)moveIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.SetTransformMode(RendererControl.TransformMode.Translate);
				toolbar.highlightIcon(toolbar.moveIcon);
			});
			Image val7 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Rotate.png")));
			((Control)val7).set_Size(new Point(64, 64));
			((Control)val7).set_BasicTooltipText("Rotate");
			((Control)val7).set_ZIndex(1);
			((Control)val7).set_Parent((Container)(object)mainPanel);
			rotateIcon = val7;
			((Control)rotateIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.SetTransformMode(RendererControl.TransformMode.Rotate);
				toolbar.highlightIcon(toolbar.rotateIcon);
			});
			Image val8 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Scale.png")));
			((Control)val8).set_Size(new Point(64, 64));
			((Control)val8).set_BasicTooltipText("Scale");
			((Control)val8).set_ZIndex(1);
			((Control)val8).set_Parent((Container)(object)mainPanel);
			scaleIcon = val8;
			((Control)scaleIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.SetTransformMode(RendererControl.TransformMode.Scale);
				toolbar.highlightIcon(toolbar.scaleIcon);
			});
			Image val9 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Axis_World.png")));
			((Control)val9).set_Size(new Point(64, 64));
			((Control)val9).set_BasicTooltipText("Axis");
			((Control)val9).set_ZIndex(1);
			((Control)val9).set_Parent((Container)(object)mainPanel);
			axisIcon = val9;
			((Control)axisIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				if (rendererControl._rotationSpace == RendererControl.RotationSpace.Local)
				{
					rendererControl._rotationSpace = RendererControl.RotationSpace.World;
					rendererControl.setPivotRotation(Quaternion.get_Identity());
					rendererControl.updateGizmos();
					toolbar.axisIcon.set_Texture(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Axis_world.png")));
					((Control)toolbar.axisIcon).set_BasicTooltipText("Axis: World");
				}
				else if (rendererControl._rotationSpace == RendererControl.RotationSpace.World)
				{
					rendererControl._rotationSpace = RendererControl.RotationSpace.Local;
					if (rendererControl.SelectedObjects.Count > 0)
					{
						rendererControl.setPivotRotation(rendererControl.SelectedObjects[0].RotationQuaternion);
					}
					rendererControl.updateGizmos();
					((Control)toolbar.axisIcon).set_BasicTooltipText("Axis: Local");
					toolbar.axisIcon.set_Texture(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Axis_Local.png")));
				}
			});
			Image val10 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Copy.png")));
			((Control)val10).set_Size(new Point(64, 64));
			((Control)val10).set_BasicTooltipText("Copy");
			((Control)val10).set_ZIndex(1);
			((Control)val10).set_Parent((Container)(object)mainPanel);
			copyIcon = val10;
			((Control)copyIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.CopySelectedObject();
			});
			Image val11 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Paste.png")));
			((Control)val11).set_Size(new Point(64, 64));
			((Control)val11).set_BasicTooltipText("Paste");
			((Control)val11).set_ZIndex(1);
			((Control)val11).set_Parent((Container)(object)mainPanel);
			pasteIcon = val11;
			((Control)pasteIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.PasteObject();
			});
			Image val12 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Rectangle.png")));
			((Control)val12).set_Size(new Point(64, 64));
			((Control)val12).set_BasicTooltipText("Rectangle Selection");
			((Control)val12).set_ZIndex(1);
			((Control)val12).set_Parent((Container)(object)mainPanel);
			rectangleIcon = val12;
			((Control)rectangleIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				designerView.UnsubscribeSelectionEvents();
				toolbar.lassoIcon.set_Tint(Color.get_White());
				rendererControl.StartRectangleSelection();
				designerView.SubscribeSelectionEvents();
			});
			Image val13 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Lasso.png")));
			((Control)val13).set_Size(new Point(64, 64));
			((Control)val13).set_BasicTooltipText("Lasso Selection");
			((Control)val13).set_ZIndex(1);
			((Control)val13).set_Parent((Container)(object)mainPanel);
			lassoIcon = val13;
			((Control)lassoIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				designerView.UnsubscribeSelectionEvents();
				designerView.SubscribeSelectionEvents();
				if (rendererControl._selectionMode == RendererControl.SelectionMode.None)
				{
					toolbar.lassoIcon.set_Tint(new Color(250, 250, 80, 128));
				}
				else if (rendererControl._selectionMode == RendererControl.SelectionMode.PolygonPoints)
				{
					toolbar.lassoIcon.set_Tint(Color.get_White());
				}
				rendererControl.PolygonSelection();
			});
			Image val14 = new Image(AsyncTexture2D.op_Implicit(contents.GetTexture("Icons/Remove.png")));
			((Control)val14).set_Size(new Point(64, 64));
			((Control)val14).set_BasicTooltipText("Remove Selection");
			((Control)val14).set_ZIndex(1);
			((Control)val14).set_Parent((Container)(object)mainPanel);
			removeIcon = val14;
			((Control)removeIcon).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				designerView.RemoveSelectedObject();
			});
		}

		private void highlightIcon(Image image)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			moveIcon.set_Tint(Color.get_White());
			rotateIcon.set_Tint(Color.get_White());
			scaleIcon.set_Tint(Color.get_White());
			image.set_Tint(new Color(250, 250, 80, 128));
		}

		public void unload()
		{
			AsyncTexture2D texture = moveIcon.get_Texture();
			if (texture != null)
			{
				texture.Dispose();
			}
			AsyncTexture2D texture2 = rotateIcon.get_Texture();
			if (texture2 != null)
			{
				texture2.Dispose();
			}
			AsyncTexture2D texture3 = scaleIcon.get_Texture();
			if (texture3 != null)
			{
				texture3.Dispose();
			}
			AsyncTexture2D texture4 = axisIcon.get_Texture();
			if (texture4 != null)
			{
				texture4.Dispose();
			}
			AsyncTexture2D texture5 = copyIcon.get_Texture();
			if (texture5 != null)
			{
				texture5.Dispose();
			}
			AsyncTexture2D texture6 = pasteIcon.get_Texture();
			if (texture6 != null)
			{
				texture6.Dispose();
			}
			AsyncTexture2D texture7 = removeIcon.get_Texture();
			if (texture7 != null)
			{
				texture7.Dispose();
			}
			AsyncTexture2D texture8 = rectangleIcon.get_Texture();
			if (texture8 != null)
			{
				texture8.Dispose();
			}
			AsyncTexture2D texture9 = lassoIcon.get_Texture();
			if (texture9 != null)
			{
				texture9.Dispose();
			}
			Image obj = moveIcon;
			if (obj != null)
			{
				((Control)obj).Dispose();
			}
			Image obj2 = rotateIcon;
			if (obj2 != null)
			{
				((Control)obj2).Dispose();
			}
			Image obj3 = scaleIcon;
			if (obj3 != null)
			{
				((Control)obj3).Dispose();
			}
			Image obj4 = axisIcon;
			if (obj4 != null)
			{
				((Control)obj4).Dispose();
			}
			Image obj5 = copyIcon;
			if (obj5 != null)
			{
				((Control)obj5).Dispose();
			}
			Image obj6 = pasteIcon;
			if (obj6 != null)
			{
				((Control)obj6).Dispose();
			}
			Image obj7 = removeIcon;
			if (obj7 != null)
			{
				((Control)obj7).Dispose();
			}
			Image obj8 = rectangleIcon;
			if (obj8 != null)
			{
				((Control)obj8).Dispose();
			}
			Image obj9 = lassoIcon;
			if (obj9 != null)
			{
				((Control)obj9).Dispose();
			}
		}
	}
}
