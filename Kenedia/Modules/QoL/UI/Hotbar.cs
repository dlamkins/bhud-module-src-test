using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.QoL.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.QoL.UI
{
	public class Hotbar : Container
	{
		private class Sizes
		{
			public Point Collapsed = Point.get_Zero();

			public Point Expanded = Point.get_Zero();

			public Point Delta = Point.get_Zero();

			public bool isExpanded;

			public Point Size
			{
				get
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					if (!isExpanded)
					{
						return Collapsed;
					}
					return Expanded;
				}
			}
		}

		private List<Hotbar_Button> SubControls = new List<Hotbar_Button>();

		private FlowPanel FlowPanel;

		private AsyncTexture2D Background;

		private AsyncTexture2D Expand;

		private AsyncTexture2D Expand_Hovered;

		private ExpandDirection _ExpandDirection;

		private Sizes FlowPanelSizes = new Sizes();

		private Sizes HotBarSizes = new Sizes();

		private bool Expanded;

		private bool Dragging;

		private Point DraggingStart;

		private Point DraggingDestination;

		private int BtnPadding = 8;

		public Point ButtonSize = new Point(24, 24);

		private Point ExpanderSize = new Point(32, 32);

		private Rectangle ExpanderBounds = new Rectangle(0, 0, 32, 32);

		private Point _PreSize = Point.get_Zero();

		private Point _PreLocation = Point.get_Zero();

		public ExpandDirection ExpandDirection
		{
			get
			{
				return _ExpandDirection;
			}
			set
			{
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				OnExpandDirectionChanged(this, null);
				if (value == _ExpandDirection)
				{
					return;
				}
				if (FlowPanel != null)
				{
					switch (value)
					{
					case ExpandDirection.LeftToRight:
						FlowPanel.set_FlowDirection((ControlFlowDirection)2);
						((Control)FlowPanel).set_Location(new Point(0, 0));
						break;
					case ExpandDirection.RightToLeft:
						FlowPanel.set_FlowDirection((ControlFlowDirection)5);
						((Control)FlowPanel).set_Location(new Point(ExpanderSize.X, 0));
						break;
					case ExpandDirection.TopToBottom:
						FlowPanel.set_FlowDirection((ControlFlowDirection)3);
						((Control)FlowPanel).set_Location(new Point(0, 0));
						break;
					case ExpandDirection.BottomToTop:
						FlowPanel.set_FlowDirection((ControlFlowDirection)7);
						((Control)FlowPanel).set_Location(new Point(0, ExpanderSize.X));
						break;
					}
				}
				_ExpandDirection = value;
				AdjustSize();
			}
		}

		public Point CollapsedSize => HotBarSizes.Collapsed;

		private event EventHandler ExpandDirectionChanged;

		public Hotbar()
			: this()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			Texture2D texture = QoL.ModuleInstance.TextureManager.getBackground(_Backgrounds.No3);
			Background = AsyncTexture2D.op_Implicit(Texture2DExtension.GetRegion(texture, 25, 25, texture.get_Width() - 25, texture.get_Height() - 25));
			Expand = AsyncTexture2D.op_Implicit(QoL.ModuleInstance.TextureManager.getIcon(_Icons.Expand));
			Expand_Hovered = AsyncTexture2D.op_Implicit(QoL.ModuleInstance.TextureManager.getIcon(_Icons.Expand_Hovered));
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_ControlPadding(new Vector2(2f, 2f));
			val.set_OuterControlPadding(new Vector2(4f, 4f));
			val.set_FlowDirection((ControlFlowDirection)0);
			((Control)val).set_Location(new Point(0, 0));
			FlowPanel = val;
			FlowPanelSizes.Collapsed = ((Control)FlowPanel).get_Size();
			FlowPanelSizes.Expanded = ((Control)FlowPanel).get_Size();
			HotBarSizes.Collapsed = ((Control)this).get_Size();
			HotBarSizes.Expanded = ((Control)this).get_Size();
			ExpandDirection = ExpandDirection.BottomToTop;
		}

		private void OnExpandDirectionChanged(object sender, EventArgs e)
		{
			this.ExpandDirectionChanged?.Invoke(this, EventArgs.Empty);
			AdjustSize();
		}

		public void AddButton(Hotbar_Button btn)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			((Control)btn).set_Parent((Container)(object)FlowPanel);
			((Control)btn).set_Visible(btn.SubModule.Active);
			((Control)btn).set_Size(ButtonSize);
			btn.Index = SubControls.Count;
			btn.SubModule.Toggled += SubModule_Toggled;
			SubControls.Add(btn);
			AdjustSize();
			SubModule_Toggled(null, null);
		}

		private void AdjustSize()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0505: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_0569: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_0615: Unknown result type (might be due to invalid IL or missing references)
			int size = ExpanderSize.X + BtnPadding / 2;
			int visible = SubControls.Where((Hotbar_Button e) => e.SubModule.Active).Count();
			switch (ExpandDirection)
			{
			case ExpandDirection.LeftToRight:
				FlowPanelSizes.Collapsed = new Point(visible * (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), ExpanderSize.X);
				FlowPanelSizes.Expanded = new Point(SubControls.Count * (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), ExpanderSize.X);
				FlowPanelSizes.Delta = new Point(FlowPanelSizes.Expanded.X - FlowPanelSizes.Collapsed.X, FlowPanelSizes.Expanded.Y - FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Collapsed = new Point(ExpanderSize.X + FlowPanelSizes.Collapsed.X, size);
				HotBarSizes.Expanded = new Point(ExpanderSize.X + FlowPanelSizes.Expanded.X, size);
				HotBarSizes.Delta = new Point(HotBarSizes.Expanded.X - HotBarSizes.Collapsed.X, HotBarSizes.Expanded.Y - HotBarSizes.Collapsed.Y);
				break;
			case ExpandDirection.RightToLeft:
				FlowPanelSizes.Collapsed = new Point(visible * (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), size);
				FlowPanelSizes.Expanded = new Point(SubControls.Count * (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), size);
				FlowPanelSizes.Delta = new Point(FlowPanelSizes.Expanded.X - FlowPanelSizes.Collapsed.X, FlowPanelSizes.Expanded.Y - FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Collapsed = new Point(ExpanderSize.X + FlowPanelSizes.Collapsed.X, size);
				HotBarSizes.Expanded = new Point(ExpanderSize.X + FlowPanelSizes.Expanded.X, size);
				HotBarSizes.Delta = new Point(HotBarSizes.Expanded.X - HotBarSizes.Collapsed.X, HotBarSizes.Expanded.Y - HotBarSizes.Collapsed.Y);
				break;
			case ExpandDirection.TopToBottom:
				FlowPanelSizes.Collapsed = new Point(size, visible * (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
				FlowPanelSizes.Expanded = new Point(size, SubControls.Count * (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
				FlowPanelSizes.Delta = new Point(FlowPanelSizes.Expanded.X - FlowPanelSizes.Collapsed.X, FlowPanelSizes.Expanded.Y - FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Collapsed = new Point(size, ExpanderSize.Y + FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Expanded = new Point(size, ExpanderSize.Y + FlowPanelSizes.Expanded.Y);
				HotBarSizes.Delta = new Point(HotBarSizes.Expanded.X - HotBarSizes.Collapsed.X, HotBarSizes.Expanded.Y - HotBarSizes.Collapsed.Y);
				break;
			case ExpandDirection.BottomToTop:
				FlowPanelSizes.Collapsed = new Point(size, visible * (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
				FlowPanelSizes.Expanded = new Point(size, SubControls.Count * (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
				FlowPanelSizes.Delta = new Point(FlowPanelSizes.Expanded.X - FlowPanelSizes.Collapsed.X, FlowPanelSizes.Expanded.Y - FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Collapsed = new Point(size, ExpanderSize.Y + FlowPanelSizes.Collapsed.Y);
				HotBarSizes.Expanded = new Point(size, ExpanderSize.Y + FlowPanelSizes.Expanded.Y);
				HotBarSizes.Delta = new Point(HotBarSizes.Expanded.X - HotBarSizes.Collapsed.X, HotBarSizes.Expanded.Y - HotBarSizes.Collapsed.Y);
				break;
			}
		}

		private void SubModule_Toggled(object sender, EventArgs e)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			FlowPanel.SortChildren<Hotbar_Button>((Comparison<Hotbar_Button>)((Hotbar_Button a, Hotbar_Button b) => ((Control)b).get_Visible().CompareTo(((Control)a).get_Visible())));
			SubModule submodule = (SubModule)sender;
			if (!(_PreLocation != Point.get_Zero()) || !((Control)this).get_MouseOver())
			{
				return;
			}
			if (submodule.Active)
			{
				if (ExpandDirection == ExpandDirection.BottomToTop)
				{
					_PreLocation = new Point(_PreLocation.X, _PreLocation.Y - (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
				}
				else if (ExpandDirection == ExpandDirection.RightToLeft)
				{
					_PreLocation = new Point(_PreLocation.X - (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), _PreLocation.Y);
				}
			}
			else if (ExpandDirection == ExpandDirection.BottomToTop)
			{
				_PreLocation = new Point(_PreLocation.X, _PreLocation.Y + (ButtonSize.Y + (int)FlowPanel.get_ControlPadding().Y));
			}
			else if (ExpandDirection == ExpandDirection.RightToLeft)
			{
				_PreLocation = new Point(_PreLocation.X + (ButtonSize.X + (int)FlowPanel.get_ControlPadding().X), _PreLocation.Y);
			}
		}

		public void RemoveButton(Hotbar_Button btn)
		{
			if (SubControls.Contains(btn))
			{
				SubControls.Remove(btn);
			}
			btn.SubModule.Toggled -= SubModule_Toggled;
			((Control)btn).Dispose();
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			Dragging = (int)Control.get_Input().get_Keyboard().get_ActiveModifiers() == 2;
			DraggingStart = (Dragging ? ((Control)this).get_RelativeMousePosition() : Point.get_Zero());
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			Dragging = false;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).UpdateContainer(gameTime);
			Dragging = Dragging && ((Control)this).get_MouseOver();
			Expanded = ((Control)this).get_MouseOver() || ((Control)FlowPanel).get_MouseOver() || (int)Control.get_Input().get_Keyboard().get_ActiveModifiers() == 2;
			FlowPanelSizes.isExpanded = Expanded;
			HotBarSizes.isExpanded = Expanded;
			AdjustSize();
			if (Dragging)
			{
				if (ExpandDirection == ExpandDirection.RightToLeft)
				{
					((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
						.Add(new Point(-DraggingStart.X, -DraggingStart.Y)));
					_PreLocation = Control.get_Input().get_Mouse().get_Position()
						.Add(new Point(-DraggingStart.X + HotBarSizes.Delta.X, -DraggingStart.Y));
				}
				else
				{
					((Control)this).set_Location(Control.get_Input().get_Mouse().get_Position()
						.Add(new Point(-DraggingStart.X, -DraggingStart.Y)));
					_PreLocation = Control.get_Input().get_Mouse().get_Position()
						.Add(new Point(-DraggingStart.X, -DraggingStart.Y + HotBarSizes.Delta.Y));
				}
			}
			if (Expanded)
			{
				if (!(HotBarSizes.Size != ((Control)this).get_Size()))
				{
					return;
				}
				_PreLocation = ((_PreLocation == Point.get_Zero()) ? ((Control)this).get_Location() : _PreLocation);
				foreach (Hotbar_Button subControl in SubControls)
				{
					((Control)subControl).set_Visible(true);
				}
				((Control)FlowPanel).Invalidate();
				((Control)FlowPanel).set_Size(FlowPanelSizes.Size);
				((Control)this).set_Size(HotBarSizes.Size);
				if (ExpandDirection == ExpandDirection.BottomToTop)
				{
					((Control)this).set_Location(new Point(_PreLocation.X, _PreLocation.Y - HotBarSizes.Delta.Y));
				}
				else if (ExpandDirection == ExpandDirection.RightToLeft)
				{
					((Control)this).set_Location(new Point(_PreLocation.X - HotBarSizes.Delta.X, _PreLocation.Y));
				}
			}
			else
			{
				if (!(HotBarSizes.Size != ((Control)this).get_Size()))
				{
					return;
				}
				foreach (Hotbar_Button subControl2 in SubControls)
				{
					((Control)subControl2).set_Visible(subControl2.SubModule.Active);
				}
				FlowPanel.SortChildren<Hotbar_Button>((Comparison<Hotbar_Button>)((Hotbar_Button a, Hotbar_Button b) => ((Control)b).get_Visible().CompareTo(((Control)a).get_Visible())));
				((Control)FlowPanel).Invalidate();
				((Control)FlowPanel).set_Size(FlowPanelSizes.Size);
				((Control)this).set_Size(HotBarSizes.Size);
				if (_PreLocation != Point.get_Zero())
				{
					if (ExpandDirection == ExpandDirection.BottomToTop)
					{
						((Control)this).set_Location(_PreLocation);
					}
					else if (ExpandDirection == ExpandDirection.RightToLeft)
					{
						((Control)this).set_Location(_PreLocation);
					}
					_PreLocation = Point.get_Zero();
				}
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			int pad = 8;
			float rotation = 0f;
			int expanderSize = ExpanderSize.X;
			switch (ExpandDirection)
			{
			case ExpandDirection.LeftToRight:
				ExpanderBounds = new Rectangle(((Rectangle)(ref bounds)).get_Right() - expanderSize + 6, bounds.Y + 6, expanderSize - pad, expanderSize - pad);
				rotation = 0f;
				break;
			case ExpandDirection.RightToLeft:
				ExpanderBounds = new Rectangle(((Rectangle)(ref bounds)).get_Left() + expanderSize - pad / 2, bounds.Y + expanderSize - 2, expanderSize - pad, expanderSize - pad);
				rotation = 3.15f;
				break;
			case ExpandDirection.TopToBottom:
				ExpanderBounds = new Rectangle(expanderSize - 2, ((Control)this).get_Height() - expanderSize + pad / 2, expanderSize - pad, expanderSize - pad);
				rotation = 1.55f;
				break;
			case ExpandDirection.BottomToTop:
				ExpanderBounds = new Rectangle(6, expanderSize - 4, expanderSize - pad, expanderSize - pad);
				rotation = -1.55f;
				break;
			}
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Background), bounds, (Rectangle?)bounds, new Color(96, 96, 96, 105), 0f, default(Vector2), (SpriteEffects)0);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Expand), ExpanderBounds, (Rectangle?)Expand.get_Texture().get_Bounds(), Color.get_White(), rotation, default(Vector2), (SpriteEffects)0);
			Color color = Color.get_Black();
			Rectangle rect = bounds;
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 2, rect.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - 1, rect.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 2, ((Rectangle)(ref rect)).get_Top(), 2, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref rect)).get_Right() - 1, ((Rectangle)(ref rect)).get_Top(), 1, rect.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Draw(spriteBatch, drawBounds, scissor);
		}

		protected override void DisposeControl()
		{
			((Container)this).DisposeControl();
			foreach (Hotbar_Button btn in SubControls)
			{
				if (btn.SubModule != null)
				{
					btn.SubModule.Toggled -= SubModule_Toggled;
				}
			}
			SubControls?.Clear();
			FlowPanel flowPanel = FlowPanel;
			if (flowPanel != null)
			{
				((Control)flowPanel).Dispose();
			}
			Background = null;
		}
	}
}
