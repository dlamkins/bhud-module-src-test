using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BhModule.Community.Pathing.Properties;

namespace BhModule.Community.Pathing.Editor.Panels
{
	public class Vector3RotationPanel : UserControl
	{
		private IContainer components;

		private Button bttnMoveHere;

		private Button button1;

		private Button button2;

		public Vector3RotationPanel()
		{
			InitializeComponent();
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
			button1 = new System.Windows.Forms.Button();
			bttnMoveHere = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			SuspendLayout();
			button1.Image = BhModule.Community.Pathing.Properties.Resources.this_way_up_64px;
			button1.Location = new System.Drawing.Point(3, 73);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(64, 64);
			button1.TabIndex = 2;
			button1.UseVisualStyleBackColor = true;
			bttnMoveHere.Image = BhModule.Community.Pathing.Properties.Resources.delete_64px;
			bttnMoveHere.Location = new System.Drawing.Point(3, 3);
			bttnMoveHere.Name = "bttnMoveHere";
			bttnMoveHere.Size = new System.Drawing.Size(64, 64);
			bttnMoveHere.TabIndex = 1;
			bttnMoveHere.UseVisualStyleBackColor = true;
			button2.Image = BhModule.Community.Pathing.Properties.Resources.look_64px;
			button2.Location = new System.Drawing.Point(3, 143);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(64, 64);
			button2.TabIndex = 3;
			button2.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(button2);
			base.Controls.Add(button1);
			base.Controls.Add(bttnMoveHere);
			base.Name = "Vector3RotationPanel";
			base.Size = new System.Drawing.Size(535, 210);
			ResumeLayout(false);
		}
	}
}
