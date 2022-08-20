using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class MetaPanel : Panel
	{
		private IPanelOverride _panelOverride;

		private readonly TabbedWindow _targetWindow;

		private readonly IPanelOverride _mainPanel;

		private readonly WindowTab _tab;

		public IPanelOverride PanelOverride
		{
			get
			{
				return _panelOverride;
			}
			set
			{
				//IL_0067: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				IPanelOverride panelOverride = _panelOverride;
				if (panelOverride != null)
				{
					((Control)panelOverride.Panel).Dispose();
				}
				_panelOverride = value;
				((Control)_mainPanel.Panel).set_Parent((Container)null);
				IPanelOverride finalPanel = _panelOverride ?? _mainPanel;
				((Control)finalPanel.Panel).set_Parent((Container)(object)this);
				_tab.set_Icon(AsyncTexture2D.op_Implicit(finalPanel.Icon));
				((Control)finalPanel.Panel).set_Location(Point.get_Zero());
				Panel panel = finalPanel.Panel;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)panel).set_Size(((Rectangle)(ref contentRegion)).get_Size());
				UpdateLayout();
			}
		}

		public MetaPanel(TabbedWindow targetWindow)
			: this()
		{
			_targetWindow = targetWindow;
			_mainPanel = new MainPanel();
			_tab = _targetWindow.AddTab(_mainPanel.Caption, AsyncTexture2D.op_Implicit(_mainPanel.Icon), (Panel)(object)this);
			PanelOverride = null;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			IPanelOverride finalPanel = _panelOverride ?? _mainPanel;
			if (finalPanel != null)
			{
				((Control)finalPanel.Panel).set_Location(Point.get_Zero());
				Panel panel = finalPanel.Panel;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)panel).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			}
		}

		protected override void DisposeControl()
		{
			IPanelOverride panelOverride = _panelOverride;
			if (panelOverride != null)
			{
				((Control)panelOverride.Panel).Dispose();
			}
			IPanelOverride mainPanel = _mainPanel;
			if (mainPanel != null)
			{
				((Control)mainPanel.Panel).Dispose();
			}
			_targetWindow.RemoveTab(_tab);
			((Panel)this).DisposeControl();
		}
	}
}
