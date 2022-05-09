using System;
using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_TemplateSelection : FlowPanel
	{
		public TextBox FilterBox;

		public FlowPanel ContentPanel;

		private Control_ProfessionSelector _ProfessionSelector;

		private List<Control_TemplateEntry> Templates = new List<Control_TemplateEntry>();

		public event EventHandler<TemplateChangedEvent> TemplateChanged;

		public Control_TemplateSelection(Container parent)
			: this()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 3f));
			TextBox val = new TextBox();
			((Control)val).set_Location(new Point(5, 0));
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Control)this).get_Width() - 5);
			((TextInputBase)val).set_PlaceholderText("Search ...");
			FilterBox = val;
			Control_ProfessionSelector control_ProfessionSelector = new Control_ProfessionSelector();
			((Control)control_ProfessionSelector).set_Parent((Container)(object)this);
			((Control)control_ProfessionSelector).set_Size(new Point(((Control)this).get_Width() - 5, ((Control)FilterBox).get_Height()));
			((Control)control_ProfessionSelector).set_Location(new Point(5, ((Control)FilterBox).get_Bottom() + 5));
			_ProfessionSelector = control_ProfessionSelector;
			FlowPanel val2 = new FlowPanel();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - ((Control)this).get_AbsoluteBounds().Y - 5));
			((Control)val2).set_Location(new Point(5, ((Control)_ProfessionSelector).get_Bottom() + 5));
			((Panel)val2).set_CanScroll(true);
			val2.set_FlowDirection((ControlFlowDirection)3);
			ContentPanel = val2;
			Rectangle localBounds = ((Control)FilterBox).get_LocalBounds();
			int left = ((Rectangle)(ref localBounds)).get_Left();
			localBounds = ((Control)_ProfessionSelector).get_LocalBounds();
			((Control)this).set_Location(new Point(left, ((Rectangle)(ref localBounds)).get_Bottom() + 5));
			((Control)this).set_Size(new Point(255, ((Control)parent).get_Height() - ((Control)this).get_AbsoluteBounds().Y - 5));
			Refresh();
			((TextInputBase)FilterBox).add_TextChanged((EventHandler<EventArgs>)FilterBox_TextChanged);
			Control_ProfessionSelector professionSelector = _ProfessionSelector;
			professionSelector.Changed = (EventHandler)Delegate.Combine(professionSelector.Changed, (EventHandler)delegate
			{
				RefreshList();
			});
			BuildsManager.ModuleInstance.Templates_Loaded += ModuleInstance_Templates_Loaded;
			BuildsManager.ModuleInstance.Template_Deleted += ModuleInstance_Templates_Loaded;
			((Container)ContentPanel).add_ChildAdded((EventHandler<ChildChangedEventArgs>)ContentPanel_ChildsChanged);
			((Container)ContentPanel).add_ChildRemoved((EventHandler<ChildChangedEventArgs>)ContentPanel_ChildsChanged);
		}

		private void ContentPanel_ChildsChanged(object sender, ChildChangedEventArgs e)
		{
			bool not_fitting = ((Control)ContentPanel).get_Height() < Templates.Count * 38;
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).set_Width(not_fitting ? (((Control)this).get_Width() - 20) : (((Control)this).get_Width() - 5));
			}
		}

		private void ModuleInstance_Templates_Loaded(object sender, EventArgs e)
		{
			Refresh();
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			((Control)this).OnMoved(e);
		}

		private void RefreshList()
		{
			string filter = ((TextInputBase)FilterBox).get_Text().ToLower();
			foreach (Control_TemplateEntry template in Templates)
			{
				string name = template.Template.Name.ToLower();
				if ((_ProfessionSelector.Professions.Count == 0 || _ProfessionSelector.Professions.Contains(template.Template.Profession)) && name.Contains(filter))
				{
					((Control)template).Show();
				}
				else
				{
					((Control)template).Hide();
				}
			}
			((Control)ContentPanel).Invalidate();
		}

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			RefreshList();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			((Control)FilterBox).set_Width(((Control)this).get_Width() - 5);
			((Control)_ProfessionSelector).set_Width(((Control)this).get_Width() - 5);
			((Control)ContentPanel).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - ((Control)this).get_AbsoluteBounds().Y - 5));
		}

		public void Refresh()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			foreach (Control_TemplateEntry template2 in Templates)
			{
				((Control)template2).Dispose();
			}
			Templates.Clear();
			Templates = new List<Control_TemplateEntry>();
			foreach (Template template in BuildsManager.ModuleInstance.Templates)
			{
				Control_TemplateEntry control_TemplateEntry = new Control_TemplateEntry((Container)(object)ContentPanel, template);
				((Control)control_TemplateEntry).set_Size(new Point(((Control)this).get_Width() - 20, 38));
				Control_TemplateEntry ctrl = control_TemplateEntry;
				ctrl.TemplateChanged += OnTemplateChangedEvent;
				Templates.Add(ctrl);
			}
			ContentPanel_ChildsChanged(null, null);
		}

		protected override void DisposeControl()
		{
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).Dispose();
			}
			((Control)FilterBox).Dispose();
			((Control)_ProfessionSelector).Dispose();
			((Panel)this).DisposeControl();
		}

		private void OnTemplateChangedEvent(object sender, TemplateChangedEvent e)
		{
			this.TemplateChanged?.Invoke(this, new TemplateChangedEvent(e.Template));
		}
	}
}
