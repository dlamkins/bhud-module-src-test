using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BhModule.Community.Pathing.Scripting.Console
{
	public class InputDiag : Form
	{
		private IContainer components;

		private Label lblDiagDescription;

		private TextBox textBox1;

		private Button btnCancel;

		private Button btnOk;

		public string UserInput => textBox1.Text;

		public InputDiag()
		{
			InitializeComponent();
		}

		public InputDiag(string prompt, string title)
			: this()
		{
			lblDiagDescription.Text = prompt;
			Text = title;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
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
			lblDiagDescription = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			btnCancel = new System.Windows.Forms.Button();
			btnOk = new System.Windows.Forms.Button();
			SuspendLayout();
			lblDiagDescription.Location = new System.Drawing.Point(13, 13);
			lblDiagDescription.Name = "lblDiagDescription";
			lblDiagDescription.Size = new System.Drawing.Size(479, 45);
			lblDiagDescription.TabIndex = 0;
			lblDiagDescription.Text = "lblDiagDescription";
			lblDiagDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBox1.Location = new System.Drawing.Point(16, 61);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(476, 20);
			textBox1.TabIndex = 1;
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(417, 96);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(75, 23);
			btnCancel.TabIndex = 2;
			btnCancel.Text = "Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			btnCancel.Click += new System.EventHandler(btnCancel_Click);
			btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOk.Location = new System.Drawing.Point(336, 96);
			btnOk.Name = "btnOk";
			btnOk.Size = new System.Drawing.Size(75, 23);
			btnOk.TabIndex = 3;
			btnOk.Text = "OK";
			btnOk.UseVisualStyleBackColor = true;
			btnOk.Click += new System.EventHandler(btnOk_Click);
			base.AcceptButton = btnOk;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btnCancel;
			base.ClientSize = new System.Drawing.Size(504, 131);
			base.Controls.Add(btnOk);
			base.Controls.Add(btnCancel);
			base.Controls.Add(textBox1);
			base.Controls.Add(lblDiagDescription);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "InputDiag";
			Text = "InputDiag";
			base.TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
