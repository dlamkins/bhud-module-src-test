using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.BHUDCommon.AnchoredRect;

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
				AreaView target2 = Target;
				if (target2 != null)
				{
					AnchoredRect parent2 = Target!.Parent;
					if (parent2 != null)
					{
						int index2 = (parent2.IndexOfChild(target2) - 1 + parent2.Children.Count) % parent2.Children.Count;
						parent2.RemoveChild(target2);
						parent2.InsertChild(index2, target2);
						this.HierarchyChanged?.Invoke(target2, parent2);
					}
				}
			});
			((Control)_moveDownButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				AreaView target = Target;
				if (target != null)
				{
					AnchoredRect parent = Target!.Parent;
					if (parent != null)
					{
						int index = (parent.IndexOfChild(target) + 1) % parent.Children.Count;
						parent.RemoveChild(target);
						parent.InsertChild(index, target);
						this.HierarchyChanged?.Invoke(target, parent);
					}
				}
			});
			((Control)_changeContainerButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Expected O, but got Unknown
				_003C_003Ec__DisplayClass22_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass22_0();
				CS_0024_003C_003E8__locals0._003C_003E4__this = this;
				CS_0024_003C_003E8__locals0.target = Target;
				if (CS_0024_003C_003E8__locals0.target != null)
				{
					CS_0024_003C_003E8__locals0.parent = Target!.Parent;
					if (CS_0024_003C_003E8__locals0.parent != null)
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
				[IteratorStateMachine(typeof(_003C_003Ec__DisplayClass22_0._003C_003C_002Dctor_003Eg__menu_007C4_003Ed))]
				IEnumerable<ContextMenuStripItem> menu()
				{
					return new _003C_003Ec__DisplayClass22_0._003C_003C_002Dctor_003Eg__menu_007C4_003Ed(-2)
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals0
					};
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
