using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.Properties;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Editor.Panels
{
	public class Vector3PositionToolPanel : UserControl, IAttributeToolPanel
	{
		private IPathingEntity _pathingEntity;

		private string _activeAttribute;

		private IContainer components;

		private Button bttnMoveHere;

		private Button bttnTranslateTool;

		private Button button1;

		public Vector3PositionToolPanel()
		{
			InitializeComponent();
		}

		public void SetTarget(IPathingEntity pathingEntity, string attribute)
		{
			_pathingEntity = pathingEntity;
			_activeAttribute = attribute;
		}

		private void UpdateValue(Vector3 newValue)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (_activeAttribute == "Position")
			{
				StandardMarker marker = _pathingEntity as StandardMarker;
				if (marker != null)
				{
					marker.Position = newValue;
				}
			}
		}

		private void bttnMoveHere_Click(object sender, EventArgs e)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			UpdateValue(GameService.Gw2Mumble.get_PlayerCharacter().get_Position());
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
			bttnTranslateTool = new System.Windows.Forms.Button();
			bttnMoveHere = new System.Windows.Forms.Button();
			button1 = new System.Windows.Forms.Button();
			SuspendLayout();
			bttnTranslateTool.BackgroundImage = BhModule.Community.Pathing.Properties.Resources.arrow_up_left_right_64px;
			bttnTranslateTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			bttnTranslateTool.Location = new System.Drawing.Point(106, 2);
			bttnTranslateTool.Margin = new System.Windows.Forms.Padding(2);
			bttnTranslateTool.Name = "bttnTranslateTool";
			bttnTranslateTool.Size = new System.Drawing.Size(48, 48);
			bttnTranslateTool.TabIndex = 1;
			bttnTranslateTool.UseVisualStyleBackColor = true;
			bttnMoveHere.BackgroundImage = BhModule.Community.Pathing.Properties.Resources.here_64px;
			bttnMoveHere.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			bttnMoveHere.Location = new System.Drawing.Point(2, 2);
			bttnMoveHere.Margin = new System.Windows.Forms.Padding(2);
			bttnMoveHere.Name = "bttnMoveHere";
			bttnMoveHere.Size = new System.Drawing.Size(48, 48);
			bttnMoveHere.TabIndex = 0;
			bttnMoveHere.UseVisualStyleBackColor = true;
			bttnMoveHere.Click += new System.EventHandler(bttnMoveHere_Click);
			button1.BackgroundImage = BhModule.Community.Pathing.Properties.Resources.move_64px;
			button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			button1.Location = new System.Drawing.Point(54, 2);
			button1.Margin = new System.Windows.Forms.Padding(2);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(48, 48);
			button1.TabIndex = 2;
			button1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(button1);
			base.Controls.Add(bttnTranslateTool);
			base.Controls.Add(bttnMoveHere);
			base.Margin = new System.Windows.Forms.Padding(2);
			base.Name = "Vector3PositionToolPanel";
			base.Size = new System.Drawing.Size(401, 171);
			ResumeLayout(false);
		}
	}
}
