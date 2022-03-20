using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BhModule.Community.Pathing.Editor.Panels
{
	public class BehaviorPanel : UserControl
	{
		private IContainer components;

		private Label label1;

		private ComboBox comboBox1;

		private Button button1;

		private Panel panel1;

		public BehaviorPanel()
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
			label1 = new System.Windows.Forms.Label();
			comboBox1 = new System.Windows.Forms.ComboBox();
			button1 = new System.Windows.Forms.Button();
			panel1 = new System.Windows.Forms.Panel();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(8, 11);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(64, 17);
			label1.TabIndex = 0;
			label1.Text = "Behaviors";
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(78, 8);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(168, 25);
			comboBox1.TabIndex = 1;
			button1.Location = new System.Drawing.Point(252, 8);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 25);
			button1.TabIndex = 2;
			button1.Text = "Activate";
			button1.UseVisualStyleBackColor = true;
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panel1.Location = new System.Drawing.Point(0, 39);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(535, 171);
			panel1.TabIndex = 3;
			base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(panel1);
			base.Controls.Add(button1);
			base.Controls.Add(comboBox1);
			base.Controls.Add(label1);
			Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Name = "BehaviorPanel";
			base.Size = new System.Drawing.Size(535, 210);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
