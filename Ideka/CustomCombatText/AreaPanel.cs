using Blish_HUD.Controls;
using Ideka.BHUDCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class AreaPanel : Panel
	{
		private const int Spacing = 10;

		private AreaView? _target;

		private readonly HierarchyPanel _hierarchyPanel;

		private readonly AnchoringPanel _anchoringPanel;

		private readonly AreaCommonPanel _areaCommonPanel;

		private readonly ReceiverPanel _receiverPanel;

		private readonly TypeContainerPanel _typeContainerPanel;

		private readonly TypeScrollPanel _typeScrollPanel;

		private readonly TypeTopPanel _typeTopPanel;

		public AreaView? Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = null;
				_hierarchyPanel.Target = value;
				_anchoringPanel.Target = value;
				_areaCommonPanel.Target = value;
				_receiverPanel.Receivers = value?.Model.Receivers;
				_target = value;
				UpdateTitle();
				UpdateType();
			}
		}

		public event HierarchyPanel.HierarchyChangedDelegate? HierarchyChanged
		{
			add
			{
				_hierarchyPanel.HierarchyChanged += value;
			}
			remove
			{
				_hierarchyPanel.HierarchyChanged -= value;
			}
		}

		public event AreaCommonPanel.NameChangedDelegate? NameChanged
		{
			add
			{
				_areaCommonPanel.NameChanged += value;
			}
			remove
			{
				_areaCommonPanel.NameChanged -= value;
			}
		}

		public event AreaCommonPanel.TypeChangedDelegate? TypeChanged
		{
			add
			{
				_areaCommonPanel.TypeChanged += value;
			}
			remove
			{
				_areaCommonPanel.TypeChanged -= value;
			}
		}

		public AreaPanel()
			: this()
		{
			HierarchyPanel hierarchyPanel = new HierarchyPanel();
			((Control)hierarchyPanel).set_Parent((Container)(object)this);
			_hierarchyPanel = hierarchyPanel;
			AnchoringPanel anchoringPanel = new AnchoringPanel();
			((Control)anchoringPanel).set_Parent((Container)(object)this);
			_anchoringPanel = anchoringPanel;
			AreaCommonPanel areaCommonPanel = new AreaCommonPanel();
			((Control)areaCommonPanel).set_Parent((Container)(object)this);
			_areaCommonPanel = areaCommonPanel;
			ReceiverPanel receiverPanel = new ReceiverPanel();
			((Control)receiverPanel).set_Parent((Container)(object)this);
			_receiverPanel = receiverPanel;
			TypeContainerPanel typeContainerPanel = new TypeContainerPanel();
			((Control)typeContainerPanel).set_Parent((Container)(object)this);
			_typeContainerPanel = typeContainerPanel;
			TypeScrollPanel typeScrollPanel = new TypeScrollPanel();
			((Control)typeScrollPanel).set_Parent((Container)(object)this);
			_typeScrollPanel = typeScrollPanel;
			TypeTopPanel typeTopPanel = new TypeTopPanel();
			((Control)typeTopPanel).set_Parent((Container)(object)this);
			_typeTopPanel = typeTopPanel;
			UpdateLayout();
			_hierarchyPanel.GetKeepAbsolutePosition = () => _anchoringPanel.KeepAbsolutePosition;
			NameChanged += delegate
			{
				UpdateTitle();
			};
			TypeChanged += delegate
			{
				UpdateType();
			};
			Target = null;
		}

		private void UpdateTitle()
		{
			((Panel)this).set_Title("Area" + ((_target == null) ? "" : (": " + _target!.Model.Describe)));
		}

		private void UpdateType()
		{
			_typeContainerPanel.Target = Target?.ModelType as ModelTypeContainer;
			_typeScrollPanel.Target = Target?.ModelType as ModelTypeScroll;
			_typeTopPanel.Target = Target?.ModelType as ModelTypeTop;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			if (_anchoringPanel != null)
			{
				((Control)_anchoringPanel).set_Top(10);
				((Control)_anchoringPanel).set_Right(((Container)this).get_ContentRegion().Width - 10);
				((Control)_anchoringPanel).set_Width(400);
				((Control)(object)_anchoringPanel).ArrangeRightLeft(10, (Control)_hierarchyPanel);
				((Control)(object)_hierarchyPanel).WidthFillLeft(10);
				((Control)(object)_hierarchyPanel).ArrangeTopDown(10, (Control)_areaCommonPanel, (Control)_typeContainerPanel);
				AreaCommonPanel areaCommonPanel = _areaCommonPanel;
				TypeContainerPanel typeContainerPanel = _typeContainerPanel;
				TypeScrollPanel typeScrollPanel = _typeScrollPanel;
				int width;
				((Control)_typeTopPanel).set_Width(width = ((Control)_hierarchyPanel).get_Width());
				int num;
				((Control)typeScrollPanel).set_Width(num = width);
				int width2;
				((Control)typeContainerPanel).set_Width(width2 = num);
				((Control)areaCommonPanel).set_Width(width2);
				TypeScrollPanel typeScrollPanel2 = _typeScrollPanel;
				Point location;
				((Control)_typeTopPanel).set_Location(location = ((Control)_typeContainerPanel).get_Location());
				((Control)typeScrollPanel2).set_Location(location);
				((Control)(object)_anchoringPanel).ArrangeTopDown(10, (Control)_receiverPanel);
				((Control)(object)_receiverPanel).WidthFillRight(10);
				((Control)(object)_receiverPanel).HeightFillDown(10);
			}
		}
	}
}
