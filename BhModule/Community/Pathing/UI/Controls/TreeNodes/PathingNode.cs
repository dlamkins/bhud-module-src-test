using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BhModule.Community.Pathing.UI.Models;
using BhModule.Community.Pathing.UI.Tooltips;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BhModule.Community.Pathing.UI.Controls.TreeNodes
{
	public abstract class PathingNode : TreeNodeBase
	{
		protected Image IconControl;

		protected Label LabelControl;

		protected FlowPanel _detailsPanel;

		protected FlowPanel _propertiesPanel;

		private Color _textColor = Color.get_White();

		protected List<PathingTexture> IconTextures = new List<PathingTexture>();

		protected int IconPaddingTop;

		protected Point IconSize = new Point(30, 30);

		public bool ShowIconTooltip = true;

		private bool _checkable;

		private bool _checked;

		protected bool CheckDisabled;

		protected Checkbox _checkbox;

		private bool _built;

		public EventHandler<CheckChangedEvent> CheckedChanged;

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<Color>(ref _textColor, value, false, "TextColor") && LabelControl != null)
				{
					LabelControl.set_TextColor(_textColor);
				}
			}
		}

		public bool Checkable
		{
			get
			{
				return _checkable;
			}
			set
			{
				if (!((Control)this).SetProperty<bool>(ref _checkable, value, false, "Checkable"))
				{
					return;
				}
				if (value)
				{
					Checkbox checkbox = _checkbox;
					if (checkbox != null)
					{
						((Control)checkbox).Show();
					}
				}
				else
				{
					Checkbox checkbox2 = _checkbox;
					if (checkbox2 != null)
					{
						((Control)checkbox2).Hide();
					}
				}
			}
		}

		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				if (((Control)this).SetProperty<bool>(ref _checked, value, false, "Checked") && _checkbox != null)
				{
					_checkbox.set_Checked(value);
				}
			}
		}

		protected bool DoBuildContextMenu { get; set; } = true;


		protected PathingNode(string name)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			base.Name = name;
			((Control)this).set_EffectBehind((ControlEffect)new ScrollingHighlightEffect((Control)(object)this));
			base.ShowBackground = true;
			base.PanelHeight = 40;
		}

		public virtual void Build()
		{
			if (!_built)
			{
				BuildDetailsPanel();
				BuildCheckbox();
				BuildIcon();
				BuildNameLabel();
				BuildPropertiesPanel();
				if (DoBuildContextMenu)
				{
					BuildContextMenu();
				}
				_built = true;
			}
		}

		private void BuildDetailsPanel()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			if (_detailsPanel != null)
			{
				throw new InvalidOperationException("Requirements panel already exists.");
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)0);
			((Control)val).set_Size(new Point(((Container)this).get_ContentRegion().Width - 240, base.PanelHeight));
			((Control)val).set_Location(new Point(28, 1));
			val.set_ControlPadding(new Vector2(5f, 0f));
			((Panel)val).set_CanScroll(false);
			((Control)val).set_Tooltip(((Control)this).get_Tooltip());
			((Panel)val).set_ShowTint(DevMode);
			_detailsPanel = val;
		}

		private void BuildPropertiesPanel()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			if (_propertiesPanel != null)
			{
				throw new InvalidOperationException("Requirements panel already exists.");
			}
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_FlowDirection((ControlFlowDirection)4);
			((Control)val).set_Size(new Point(220, base.PanelHeight));
			((Control)val).set_Location(new Point(((Control)this).get_Width() - 230, 1));
			val.set_ControlPadding(new Vector2(5f, 0f));
			((Panel)val).set_CanScroll(false);
			((Panel)val).set_ShowTint(DevMode);
			_propertiesPanel = val;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (_propertiesPanel != null)
			{
				((Control)_propertiesPanel).set_Left(e.get_CurrentSize().X - 230);
			}
			((Container)this).OnResized(e);
		}

		protected override void OnParentChanged()
		{
			base.OnParentChanged();
			PathingNode parentNode = ((Control)this).get_Parent() as PathingNode;
			if (parentNode != null && !CheckDisabled)
			{
				CheckDisabled = parentNode.CheckDisabled;
			}
		}

		private void BuildIcon()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			if (IconTextures.Count <= 0)
			{
				return;
			}
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)_detailsPanel);
			((Control)val).set_Size(new Point(IconSize.X * IconTextures.Count + 5, ((Control)this).get_Height()));
			((Control)val).set_Tooltip((IconTextures.Count > 0 && ShowIconTooltip) ? new Tooltip((ITooltipView)(object)new EntityTextureTooltip(IconTextures)) : ((Tooltip)null));
			Panel iconContainer = val;
			int leftPos = 0;
			foreach (PathingTexture item in IconTextures)
			{
				Image val2 = new Image(item.Icon);
				((Control)val2).set_Top(IconPaddingTop);
				((Control)val2).set_Left(leftPos);
				((Control)val2).set_Size(IconSize);
				val2.set_Tint(item.Tint);
				((Control)val2).set_Tooltip(((Control)iconContainer).get_Tooltip());
				((Control)val2).set_Parent((Container)(object)iconContainer);
				leftPos += IconSize.X;
			}
		}

		private void BuildNameLabel()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			Label labelControl = LabelControl;
			if (labelControl != null)
			{
				((Control)labelControl).Dispose();
			}
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)_detailsPanel);
			val.set_Text(base.Name);
			((Control)val).set_Height(base.PanelHeight);
			((Control)val).set_Width(((Control)_detailsPanel).get_Width() - 60 - IconTextures.Count * IconSize.X);
			val.set_Font(GameService.Content.get_DefaultFont16());
			val.set_TextColor(TextColor);
			val.set_WrapText(true);
			val.set_StrokeText(true);
			((Control)val).set_BasicTooltipText(((Control)this).get_BasicTooltipText());
			LabelControl = val;
		}

		public void BuildCheckbox()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			if (Checkable)
			{
				Panel val = new Panel();
				((Control)val).set_Parent((Container)(object)_detailsPanel);
				((Control)val).set_Size(new Point(base.PanelHeight / 2 + 5, base.PanelHeight));
				Panel checkboxContainer = val;
				Checkbox val2 = new Checkbox();
				((Control)val2).set_Parent((Container)(object)checkboxContainer);
				((Control)val2).set_Left(5);
				((Control)val2).set_Size(new Point(base.PanelHeight, base.PanelHeight));
				val2.set_Checked(Checked);
				((Control)val2).set_Enabled(!CheckDisabled);
				_checkbox = val2;
				_checkbox.add_CheckedChanged((EventHandler<CheckChangedEvent>)CheckboxOnCheckedChanged);
			}
		}

		private void CheckboxOnCheckedChanged(object sender, CheckChangedEvent e)
		{
			Checked = e.get_Checked();
			CheckedChanged?.Invoke(sender, e);
		}

		protected virtual void BuildContextMenu()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			ContextMenuStrip menu = ((Control)this).get_Menu();
			if (menu != null)
			{
				((Control)menu).Dispose();
			}
			((Control)this).set_Menu(new ContextMenuStrip());
			BuildCopyName();
			if (Checkable)
			{
				BuildDeselectAdjacentNodes();
			}
		}

		private void BuildCopyName()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrWhiteSpace(base.Name))
			{
				ContextMenuStripItem val = new ContextMenuStripItem("Copy Name");
				((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
				((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
				{
					CopyToClipboard(base.Name, "\"" + base.Name + "\" copied to clipboard");
				});
			}
		}

		private void BuildDeselectAdjacentNodes()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			ContextMenuStripItem val = new ContextMenuStripItem("Deselect Adjacent Categories");
			((Control)val).set_Parent((Container)(object)((Control)this).get_Menu());
			((Control)val).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				DeselectAdjacentNodesExcept(this);
			});
		}

		protected void CopyToClipboard(string value, string message)
		{
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(value).ContinueWith(delegate(Task<bool> t)
			{
				if (!string.IsNullOrWhiteSpace(message) && t.IsCompleted && t.Result)
				{
					ScreenNotification.ShowNotification(string.Format(message, value), (NotificationType)0, (Texture2D)null, 2);
				}
			});
		}

		public void DeselectAdjacentNodesExcept(PathingNode node)
		{
			foreach (Control child in ((Control)this).get_Parent().get_Children())
			{
				PathingNode childNode = child as PathingNode;
				if (childNode != null && childNode.Checkable && child != node)
				{
					childNode.Checked = false;
				}
			}
		}

		protected override void OnShown(EventArgs e)
		{
			if (!_built)
			{
				Build();
			}
			((Control)this).OnShown(e);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			if (((Enum)GameService.Input.get_Keyboard().get_ActiveModifiers()).HasFlag((Enum)(object)(ModifierKeys)1))
			{
				DeselectAdjacentNodesExcept(this);
				return;
			}
			if (_checkbox != null)
			{
				Rectangle absoluteBounds = ((Control)_checkbox).get_AbsoluteBounds();
				if (((Rectangle)(ref absoluteBounds)).Contains(Control.get_Input().get_Mouse().get_Position()))
				{
					return;
				}
			}
			base.OnClick(e);
		}

		protected override void DisposeControl()
		{
			Image iconControl = IconControl;
			if (iconControl != null)
			{
				((Control)iconControl).Dispose();
			}
			Label labelControl = LabelControl;
			if (labelControl != null)
			{
				((Control)labelControl).Dispose();
			}
			base.DisposeControl();
		}
	}
}
