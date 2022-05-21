using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Gw2Sharp.Models;
using Kenedia.Modules.BuildsManager.Strings;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.BuildsManager
{
	public class Control_TemplateSelection : FlowPanel
	{
		private bool ResizeChilds;

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
			//IL_0076: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Parent(parent);
			((FlowPanel)this).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)this).set_ControlPadding(new Vector2(0f, 3f));
			TextBox val = new TextBox();
			((Control)val).set_Location(new Point(5, 0));
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Width(((Control)this).get_Width() - 5);
			((TextInputBase)val).set_PlaceholderText(common.Search + " ...");
			FilterBox = val;
			BuildsManager.ModuleInstance.LanguageChanged += ModuleInstance_LanguageChanged;
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
			((Control)this).set_Size(new Point(255, ((Control)parent).get_Height() - (((Control)this).get_AbsoluteBounds().Y - 5)));
			Refresh();
			((TextInputBase)FilterBox).add_TextChanged((EventHandler<EventArgs>)FilterBox_TextChanged);
			Control_ProfessionSelector professionSelector = _ProfessionSelector;
			professionSelector.Changed = (EventHandler)Delegate.Combine(professionSelector.Changed, (EventHandler)delegate
			{
				RefreshList();
			});
			BuildsManager.ModuleInstance.Templates_Loaded += ModuleInstance_Templates_Loaded;
			BuildsManager.ModuleInstance.Template_Deleted += ModuleInstance_Template_Deleted;
			((Container)ContentPanel).add_ChildAdded((EventHandler<ChildChangedEventArgs>)ContentPanel_ChildsChanged);
			((Container)ContentPanel).add_ChildRemoved((EventHandler<ChildChangedEventArgs>)ContentPanel_ChildsChanged);
		}

		private void ModuleInstance_Template_Deleted(object sender, EventArgs e)
		{
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).Dispose();
			}
			Templates = new List<Control_TemplateEntry>();
			Refresh();
		}

		public void SetSelection()
		{
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			_ProfessionSelector.Professions.Clear();
			_ProfessionSelector.Professions.Add(BuildsManager.Data.Professions.Find(delegate(API.Profession e)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				string id = e.Id;
				ProfessionType profession = player.get_Profession();
				return id == ((object)(ProfessionType)(ref profession)).ToString();
			}));
			RefreshList();
		}

		private void ModuleInstance_LanguageChanged(object sender, EventArgs e)
		{
			((TextInputBase)FilterBox).set_PlaceholderText(common.Search + " ...");
		}

		private void ContentPanel_ChildsChanged(object sender, ChildChangedEventArgs e)
		{
			ResizeChilds = true;
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (!ResizeChilds)
			{
				return;
			}
			bool not_fitting = ((Control)ContentPanel).get_Height() < Templates.Where((Control_TemplateEntry e) => ((Control)e).get_Visible()).ToList().Count * 38;
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).set_Width(not_fitting ? (((Control)this).get_Width() - 20) : (((Control)this).get_Width() - 5));
			}
			ResizeChilds = false;
		}

		private void ModuleInstance_Templates_Loaded(object sender, EventArgs e)
		{
			Refresh();
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			((Control)this).OnMoved(e);
		}

		public void RefreshList()
		{
			string filter = ((TextInputBase)FilterBox).get_Text().ToLower();
			API.Profession prof = BuildsManager.ModuleInstance.CurrentProfession;
			ContentPanel.SortChildren<Control_TemplateEntry>((Comparison<Control_TemplateEntry>)delegate(Control_TemplateEntry a, Control_TemplateEntry b)
			{
				int num = (b.Template.Build.Profession == prof).CompareTo(a.Template.Build.Profession == prof);
				if (num == 0)
				{
					num = a.Template.Build.Profession.Id.CompareTo(b.Template.Build.Profession.Id);
				}
				if (num == 0 && a.Template.Specialization != null)
				{
					num = a.Template.Specialization.Id.CompareTo(b.Template.Specialization?.Id);
				}
				if (num == 0)
				{
					num = a.Template.Name.CompareTo(b.Template.Name);
				}
				return num;
			});
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
			ResizeChilds = true;
			((Control)ContentPanel).Invalidate();
		}

		private void FilterBox_TextChanged(object sender, EventArgs e)
		{
			RefreshList();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).OnResized(e);
			((Control)FilterBox).set_Width(((Control)this).get_Width() - 5);
			((Control)_ProfessionSelector).set_Width(((Control)this).get_Width() - 5);
			((Control)ContentPanel).set_Size(new Point(((Control)this).get_Width(), ((Control)this).get_Height() - ((Control)this).get_LocalBounds().Y));
		}

		public void Refresh()
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			foreach (Template template in BuildsManager.ModuleInstance.Templates)
			{
				if (Templates.Find((Control_TemplateEntry e) => e.Template == template) == null)
				{
					Control_TemplateEntry control_TemplateEntry = new Control_TemplateEntry((Container)(object)ContentPanel, template);
					((Control)control_TemplateEntry).set_Size(new Point(((Control)this).get_Width() - 20, 38));
					Control_TemplateEntry ctrl = control_TemplateEntry;
					ctrl.TemplateChanged += OnTemplateChangedEvent;
					Templates.Add(ctrl);
					template.Deleted += delegate
					{
						((Control)ctrl).Dispose();
						Templates.Remove(ctrl);
					};
				}
			}
			RefreshList();
		}

		public void Clear()
		{
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).Dispose();
			}
			Templates.Clear();
		}

		protected override void DisposeControl()
		{
			foreach (Control_TemplateEntry template in Templates)
			{
				((Control)template).Dispose();
			}
			Templates.Clear();
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
