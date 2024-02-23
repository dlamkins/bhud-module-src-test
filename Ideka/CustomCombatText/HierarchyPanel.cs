using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.CustomCombatText
{
	public class HierarchyPanel : Panel
	{
		public delegate void HierarchyChangedDelegate(AreaView target, AnchoredRect previousParent);

		private const int Spacing = 10;

		private AreaView? _target;

		private readonly Label _infoLabel;

		private readonly StandardButton _moveUpButton;

		private readonly StandardButton _moveDownButton;

		private readonly StandardButton _changeContainerButton;

		private ContextMenuStrip? _containerMenu;

		public Func<bool>? GetKeepAbsolutePosition { get; set; }

		public IReadOnlyList<AreaView> Siblings { get; private set; } = Array.Empty<AreaView>();


		public AreaView? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				ContextMenuStrip? containerMenu = _containerMenu;
				if (containerMenu != null)
				{
					((Control)containerMenu).Hide();
				}
				Siblings = value?.GetSiblings().ToList() ?? new List<AreaView>();
				StandardButton moveUpButton = _moveUpButton;
				bool enabled;
				((Control)_moveDownButton).set_Enabled(enabled = Siblings.Count > 1);
				((Control)moveUpButton).set_Enabled(enabled);
				((Control)_changeContainerButton).set_Enabled(value != null);
				_infoLabel.set_Text("Container:" + ((value == null) ? "" : (" " + (value.GetParent()?.Model.Describe ?? "(root)"))));
				_target = value;
			}
		}

		public event HierarchyChangedDelegate? HierarchyChanged;

		public HierarchyPanel()
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Expected O, but got Unknown
			((Panel)this).set_Title("Hierarchy");
			((Panel)this).set_ShowTint(true);
			Label val = new Label();
			((Control)val).set_Parent((Container)(object)this);
			_infoLabel = val;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text("Move Up");
			((Control)val2).set_BasicTooltipText("Swap with the previous area on the list.");
			_moveUpButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text("Move Down");
			((Control)val3).set_BasicTooltipText("Swap with the next area on the list.");
			_moveDownButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text("Change Container");
			((Control)val4).set_BasicTooltipText("Move this area to another container.");
			_changeContainerButton = val4;
			UpdateLayout();
			((Control)_moveUpButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreaView target3 = Target;
				if (target3 != null)
				{
					AnchoredRect parent3 = Target!.Parent;
					if (parent3 != null)
					{
						int index2 = (parent3.IndexOfChild(target3) - 1 + parent3.Children.Count) % parent3.Children.Count;
						parent3.RemoveChild(target3);
						parent3.InsertChild(index2, target3);
						this.HierarchyChanged?.Invoke(target3, parent3);
					}
				}
			});
			((Control)_moveDownButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreaView target2 = Target;
				if (target2 != null)
				{
					AnchoredRect parent2 = Target!.Parent;
					if (parent2 != null)
					{
						int index = (parent2.IndexOfChild(target2) + 1) % parent2.Children.Count;
						parent2.RemoveChild(target2);
						parent2.InsertChild(index, target2);
						this.HierarchyChanged?.Invoke(target2, parent2);
					}
				}
			});
			((Control)_changeContainerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Expected O, but got Unknown
				AreaView target = Target;
				AnchoredRect parent;
				if (target != null)
				{
					parent = Target!.Parent;
					if (parent != null)
					{
						ContextMenuStrip? containerMenu = _containerMenu;
						if (containerMenu != null)
						{
							((Control)containerMenu).Dispose();
						}
						_containerMenu = new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)menu);
						_containerMenu!.Show((Control)(object)_changeContainerButton);
					}
				}
				IEnumerable<ContextMenuStripItem> menu()
				{
					bool isRoot = parent == CTextModule.LocalData.AreaViewParent;
					AreaView parentArea = parent as AreaView;
					ContextMenuStripItem val5 = new ContextMenuStripItem("Move to root");
					((Control)val5).set_Enabled(!isRoot);
					((Control)val5).set_BasicTooltipText(isRoot ? "This area is already in the root" : "Remove this area from all containers");
					ContextMenuStripItem root = val5;
					((Control)root).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						reparent(CTextModule.LocalData.AreaViewParent);
					});
					yield return root;
					ContextMenuStripItem val6 = new ContextMenuStripItem("Move out of \"" + (parentArea?.Model.Describe ?? "(root)") + "\"");
					((Control)val6).set_Enabled(!isRoot);
					((Control)val6).set_BasicTooltipText(isRoot ? "Can't move out of the root" : "Move this area out of its container and put it alongside it");
					ContextMenuStripItem @out = val6;
					((Control)@out).add_Click((EventHandler<MouseEventArgs>)delegate
					{
						AnchoredRect anchoredRect = parentArea?.Parent;
						if (anchoredRect == null)
						{
							ScreenNotification.ShowNotification("Move out failed (null parent)", (NotificationType)2, (Texture2D)null, 4);
						}
						else
						{
							reparent(anchoredRect);
						}
					});
					yield return @out;
					foreach (AreaView sibling in Siblings)
					{
						if (sibling != target)
						{
							ContextMenuStripItem move = new ContextMenuStripItem("Move into \"" + sibling.Model.Describe + "\"");
							((Control)move).add_Click((EventHandler<MouseEventArgs>)delegate
							{
								reparent(sibling);
							});
							yield return move;
						}
					}
				}
				void reparent(AnchoredRect newParent)
				{
					AnchoredRect newParent2 = newParent;
					if (GetKeepAbsolutePosition?.Invoke() ?? false)
					{
						target.KeepingAbsolute(delegate
						{
							parent.RemoveChild(target);
							newParent2.AddChild(target);
						});
					}
					else
					{
						parent.RemoveChild(target);
						newParent2.AddChild(target);
					}
					this.HierarchyChanged?.Invoke(target, parent);
				}
			});
			Target = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			if (_infoLabel != null)
			{
				StandardButton moveUpButton = _moveUpButton;
				int left;
				((Control)_moveUpButton).set_Top(left = 10);
				((Control)moveUpButton).set_Left(left);
				((Control)(object)_moveUpButton).ArrangeTopDown(10, (Control)_moveDownButton);
				StandardButton moveUpButton2 = _moveUpButton;
				((Control)_moveDownButton).set_Width(left = 100);
				((Control)moveUpButton2).set_Width(left);
				((Control)(object)_moveUpButton).ArrangeLeftRight(10, (Control)_infoLabel);
				((Control)(object)_infoLabel).MiddleWith((Control)(object)_moveUpButton);
				((Control)(object)_moveDownButton).ArrangeLeftRight(10, (Control)_changeContainerButton);
				((Control)(object)_infoLabel).WidthFillRight(10);
				((Control)(object)_changeContainerButton).WidthFillRight(10);
				((Container)(object)this).MatchHeightToBottom((Control)(object)_moveDownButton, 10);
			}
		}

		protected override void DisposeControl()
		{
			ContextMenuStrip? containerMenu = _containerMenu;
			if (containerMenu != null)
			{
				((Control)containerMenu).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
