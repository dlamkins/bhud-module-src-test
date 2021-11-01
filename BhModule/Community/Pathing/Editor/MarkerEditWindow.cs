using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BhModule.Community.Pathing.Editor.Entity;
using BhModule.Community.Pathing.Editor.Panels;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Properties;
using BhModule.Community.Pathing.State;
using Blish_HUD;
using Blish_HUD.Entities;
using TmfLib.Pathable;

namespace BhModule.Community.Pathing.Editor
{
	public class MarkerEditWindow : Form
	{
		private static MarkerEditWindow _activeWindow;

		private IPathingEntity _activePathingEntity;

		private IPackState _packState;

		private CategoryTree _tvCategoryListing;

		private IContainer components;

		private PropertyGrid pgPathingAttributeEditor;

		private SplitContainer splitContainer2;

		private ToolStrip tsPackToolBar;

		private ToolStripComboBox cbPackList;

		private ToolStripSplitButton toolStripSplitButton1;

		private ToolStripMenuItem addCategoryToolStripMenuItem;

		private ToolStripMenuItem addMarkerToolStripMenuItem;

		private ToolStripMenuItem beginTrailToolStripMenuItem;

		private ToolStripButton toolStripButton1;

		private ImageList ilEntityTreeIcons;

		public IPathingEntity ActivePathingEntity
		{
			get
			{
				return _activePathingEntity;
			}
			private set
			{
				if (!object.Equals(_activePathingEntity, value))
				{
					_activePathingEntity = value;
					UpdateViewForPathable();
				}
			}
		}

		public IPackState PackState
		{
			get
			{
				return _packState;
			}
			private set
			{
				if (!object.Equals(_packState, value))
				{
					_packState = value;
					UpdateViewForPackState();
				}
			}
		}

		public static void SetPathingEntity(IPackState packState, IPathingEntity pathingEntity)
		{
			if (_activeWindow == null || _activeWindow.IsDisposed)
			{
				_activeWindow = new MarkerEditWindow();
			}
			if (!_activeWindow.Visible)
			{
				_activeWindow.Show(Form.ActiveForm);
			}
			_activeWindow.ActivePathingEntity = pathingEntity;
			_activeWindow.PackState = packState;
		}

		public static void SetPathingCategory(IPackState packState, PathingCategory pathingCategory)
		{
			if (_activeWindow == null || _activeWindow.IsDisposed)
			{
				_activeWindow = new MarkerEditWindow();
			}
			if (_activeWindow == null)
			{
				_activeWindow = new MarkerEditWindow();
			}
			if (!_activeWindow.Visible)
			{
				_activeWindow.Show(Form.ActiveForm);
			}
			_activeWindow.pgPathingAttributeEditor.SelectedObject = new PathingCategoryEditWrapper(pathingCategory);
			_activeWindow.PackState = packState;
		}

		public static void SetPropertyPanel<T>(IPathingEntity pathingEntity, string attribute, T newPanel) where T : UserControl, IAttributeToolPanel
		{
		}

		public MarkerEditWindow()
		{
			_activeWindow?.Dispose();
			_activeWindow = this;
			InitializeComponent();
		}

		private void UpdateViewForPathable()
		{
			pgPathingAttributeEditor.SelectedObject = ActivePathingEntity;
			StandardMarker marker = ActivePathingEntity as StandardMarker;
			if (marker != null)
			{
				GameService.Graphics.get_World().AddEntity((IEntity)(object)new TranslateTool(marker));
			}
		}

		private void UpdateViewForPackState()
		{
			if (_tvCategoryListing != null)
			{
				_tvCategoryListing.PackState = PackState;
			}
		}

		private void MarkerEditWindow_Shown(object sender, EventArgs e)
		{
			_tvCategoryListing = new CategoryTree
			{
				Parent = splitContainer2.Panel1,
				Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right),
				Size = new Size(splitContainer2.Panel1.Width, splitContainer2.Panel1.Height - tsPackToolBar.Bottom),
				Location = new Point(0, tsPackToolBar.Bottom),
				ImageList = ilEntityTreeIcons,
				ShowRootLines = true,
				HideSelection = false
			};
			UpdateViewForPackState();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BhModule.Community.Pathing.Editor.MarkerEditWindow));
			pgPathingAttributeEditor = new System.Windows.Forms.PropertyGrid();
			splitContainer2 = new System.Windows.Forms.SplitContainer();
			tsPackToolBar = new System.Windows.Forms.ToolStrip();
			cbPackList = new System.Windows.Forms.ToolStripComboBox();
			toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			addCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			addMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			beginTrailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			ilEntityTreeIcons = new System.Windows.Forms.ImageList(components);
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel1.SuspendLayout();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			tsPackToolBar.SuspendLayout();
			SuspendLayout();
			pgPathingAttributeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			pgPathingAttributeEditor.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			pgPathingAttributeEditor.Location = new System.Drawing.Point(0, 0);
			pgPathingAttributeEditor.Name = "pgPathingAttributeEditor";
			pgPathingAttributeEditor.Size = new System.Drawing.Size(317, 280);
			pgPathingAttributeEditor.TabIndex = 0;
			splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			splitContainer2.Location = new System.Drawing.Point(0, 0);
			splitContainer2.Name = "splitContainer2";
			splitContainer2.Panel1.Controls.Add(tsPackToolBar);
			splitContainer2.Panel2.Controls.Add(pgPathingAttributeEditor);
			splitContainer2.Size = new System.Drawing.Size(564, 280);
			splitContainer2.SplitterDistance = 243;
			splitContainer2.TabIndex = 1;
			tsPackToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { cbPackList, toolStripSplitButton1, toolStripButton1 });
			tsPackToolBar.Location = new System.Drawing.Point(0, 0);
			tsPackToolBar.Name = "tsPackToolBar";
			tsPackToolBar.Size = new System.Drawing.Size(243, 25);
			tsPackToolBar.TabIndex = 1;
			tsPackToolBar.Text = "toolStrip1";
			cbPackList.Name = "cbPackList";
			cbPackList.Size = new System.Drawing.Size(121, 25);
			toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { addCategoryToolStripMenuItem, addMarkerToolStripMenuItem, beginTrailToolStripMenuItem });
			toolStripSplitButton1.Image = BhModule.Community.Pathing.Properties.Resources.add;
			toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripSplitButton1.Name = "toolStripSplitButton1";
			toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
			toolStripSplitButton1.Text = "toolStripSplitButton1";
			addCategoryToolStripMenuItem.Image = BhModule.Community.Pathing.Properties.Resources.box;
			addCategoryToolStripMenuItem.Name = "addCategoryToolStripMenuItem";
			addCategoryToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			addCategoryToolStripMenuItem.Text = "Add Category";
			addMarkerToolStripMenuItem.Image = BhModule.Community.Pathing.Properties.Resources.shape_square;
			addMarkerToolStripMenuItem.Name = "addMarkerToolStripMenuItem";
			addMarkerToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			addMarkerToolStripMenuItem.Text = "Add Marker";
			beginTrailToolStripMenuItem.Image = BhModule.Community.Pathing.Properties.Resources.arrow_merge;
			beginTrailToolStripMenuItem.Name = "beginTrailToolStripMenuItem";
			beginTrailToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			beginTrailToolStripMenuItem.Text = "Record Trail";
			toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			toolStripButton1.Image = BhModule.Community.Pathing.Properties.Resources.bin_closed;
			toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			toolStripButton1.Name = "toolStripButton1";
			toolStripButton1.Size = new System.Drawing.Size(23, 22);
			toolStripButton1.Text = "toolStripButton1";
			ilEntityTreeIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilEntityTreeIcons.ImageStream");
			ilEntityTreeIcons.TransparentColor = System.Drawing.Color.Transparent;
			ilEntityTreeIcons.Images.SetKeyName(0, "category");
			ilEntityTreeIcons.Images.SetKeyName(1, "marker");
			ilEntityTreeIcons.Images.SetKeyName(2, "trail");
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(564, 280);
			base.Controls.Add(splitContainer2);
			Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "MarkerEditWindow";
			base.ShowInTaskbar = false;
			Text = "Edit Marker";
			base.TopMost = true;
			base.Shown += new System.EventHandler(MarkerEditWindow_Shown);
			splitContainer2.Panel1.ResumeLayout(false);
			splitContainer2.Panel1.PerformLayout();
			splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			tsPackToolBar.ResumeLayout(false);
			tsPackToolBar.PerformLayout();
			ResumeLayout(false);
		}
	}
}
