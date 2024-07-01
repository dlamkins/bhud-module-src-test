using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class PanelStack : Panel
	{
		private IUIPanel _currentPanel;

		private readonly Stack<IUIPanel> _stack = new Stack<IUIPanel>();

		private readonly TabbedWindow _targetWindow;

		private readonly WindowTab _tab;

		private readonly IUIPanel _homePanel;

		public IUIPanel CurrentPanel
		{
			get
			{
				return _currentPanel;
			}
			private set
			{
				_tab.set_Icon(AsyncTexture2D.op_Implicit(value.Icon));
				WindowTab tab = _tab;
				string caption;
				((WindowBase)_targetWindow).set_Subtitle(caption = value.Caption);
				tab.set_Name(caption);
				((Control)value.Panel).set_Parent((Container)(object)this);
				_currentPanel = value;
				UpdateLayout();
			}
		}

		public PanelStack(TabbedWindow targetWindow, int priority, Func<PanelStack, IUIPanel> makeHomePanel)
			: this()
		{
			_targetWindow = targetWindow;
			_tab = _targetWindow.AddTab("", (AsyncTexture2D)null, (Panel)(object)this, priority);
			CurrentPanel = (_currentPanel = (_homePanel = makeHomePanel(this)));
		}

		public void Push(IUIPanel panel)
		{
			_stack.Push(panel);
			((Control)CurrentPanel.Panel).set_Parent((Container)null);
			CurrentPanel = panel;
		}

		public void GoBack()
		{
			if (_stack.Any())
			{
				IUIPanel iUIPanel = _stack.Pop();
				((Control)iUIPanel.Panel).set_Parent((Container)null);
				((Control)iUIPanel.Panel).Dispose();
			}
			CurrentPanel = (_stack.Any() ? _stack.Peek() : _homePanel);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (CurrentPanel != null)
			{
				((Control)CurrentPanel.Panel).set_Location(Point.get_Zero());
				Panel panel = CurrentPanel.Panel;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)panel).set_Size(((Rectangle)(ref contentRegion)).get_Size());
			}
		}

		protected override void DisposeControl()
		{
			_targetWindow.RemoveTab(_tab);
			foreach (IUIPanel item in _stack)
			{
				((Control)item.Panel).Dispose();
			}
			((Control)_homePanel.Panel).Dispose();
			((Panel)this).DisposeControl();
		}
	}
}
