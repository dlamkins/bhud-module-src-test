using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blish_HUD;
using Humanizer;

namespace BhModule.Community.Pathing.Scripting.Console
{
	public class ConsoleWindow : Form
	{
		private readonly PathingModule _module;

		private int _lastLogMessage;

		private readonly Dictionary<ScriptMessageLogLevel, Color> _logLevelColors = new Dictionary<ScriptMessageLogLevel, Color>
		{
			{
				ScriptMessageLogLevel.System,
				Color.SlateGray
			},
			{
				ScriptMessageLogLevel.Info,
				Color.Black
			},
			{
				ScriptMessageLogLevel.Warn,
				Color.Orange
			},
			{
				ScriptMessageLogLevel.Error,
				Color.IndianRed
			}
		};

		private IContainer components;

		private StatusStrip statusStrip1;

		private Timer tOutputPoll;

		private SplitContainer splitContainer1;

		private Label label2;

		private Button btnReloadPacks;

		private Label label1;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private Button btnCopyOutput;

		private SplitContainer splitContainer2;

		private TextBox textBox1;

		private Label label3;

		private TreeView tvWatchWindow;

		private Panel panel1;

		private ToolStripStatusLabel tsslScriptFrameTime;

		private RichTextBox rtbOutput;

		private Button btnClearOutput;

		private ContextMenuStrip cmsWatchWindow;

		private ToolStripMenuItem tsWatchGlobal;

		private ToolStripMenuItem tsWatchCustom;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem tsClearWatchList;

		public ConsoleWindow()
		{
			InitializeComponent();
		}

		public ConsoleWindow(PathingModule pathingModule)
			: this()
		{
			_module = pathingModule;
		}

		private WatchTreeNode CreateOrUpdateNode(string objectName)
		{
			WatchTreeNode existingNode = null;
			foreach (object node in tvWatchWindow.Nodes)
			{
				WatchTreeNode wtn = node as WatchTreeNode;
				if (wtn != null && wtn.ObjectName == objectName)
				{
					existingNode = wtn;
					break;
				}
			}
			if (existingNode == null)
			{
				existingNode = new WatchTreeNode(objectName);
				tvWatchWindow.Nodes.Add(existingNode);
			}
			return existingNode;
		}

		private void tOutputPoll_Tick(object sender, EventArgs e)
		{
			try
			{
				btnReloadPacks.Enabled = !_module.PackInitiator.IsLoading;
				tsslScriptFrameTime.Text = _module.ScriptEngine.FrameExecutionTime.Humanize();
				ToolStripStatusLabel toolStripStatusLabel = tsslScriptFrameTime;
				double totalMilliseconds = _module.ScriptEngine.FrameExecutionTime.TotalMilliseconds;
				Color color2 = (toolStripStatusLabel.ForeColor = ((totalMilliseconds > 3.0) ? Color.IndianRed : ((!(totalMilliseconds > 1.0)) ? Color.Black : Color.Orange)));
				KeyValuePair<string, object>[] array = _module.ScriptEngine.Global.Debug.WatchValues.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<string, object> watchValue = array[i];
					CreateOrUpdateNode(watchValue.Key).Refresh(watchValue.Value);
				}
				if (_lastLogMessage > _module.ScriptEngine.OutputMessages.Count)
				{
					_lastLogMessage = 0;
				}
				while (_lastLogMessage < _module.ScriptEngine.OutputMessages.Count)
				{
					ScriptMessage newMessage = _module.ScriptEngine.OutputMessages[_lastLogMessage];
					string metaLine = $"[{newMessage.Timestamp} | {newMessage.Source}] ";
					AppendScriptConsoleOutput(metaLine, _logLevelColors[ScriptMessageLogLevel.System]);
					AppendScriptConsoleOutput(newMessage.Message.Replace(Environment.NewLine, Environment.NewLine + new string(' ', metaLine.Length)) ?? "", _logLevelColors[newMessage.LogLevel], addNewLine: true);
					_lastLogMessage++;
				}
			}
			catch (Exception)
			{
			}
		}

		private void AppendScriptConsoleOutput(string text, Color color, bool addNewLine = false)
		{
			rtbOutput.SuspendLayout();
			rtbOutput.SelectionColor = color;
			rtbOutput.AppendText(addNewLine ? (text + Environment.NewLine) : text);
			rtbOutput.ScrollToCaret();
			rtbOutput.ResumeLayout();
		}

		private async void btnCopyOutput_Click(object sender, EventArgs e)
		{
			try
			{
				await ClipboardUtil.get_WindowsClipboardService().SetTextAsync(rtbOutput.Text);
			}
			catch
			{
			}
		}

		private void btnClearOutput_Click(object sender, EventArgs e)
		{
			rtbOutput.Clear();
		}

		private void btnReloadPacks_Click(object sender, EventArgs e)
		{
			btnReloadPacks.Enabled = false;
			_module.PackInitiator.ReloadPacks();
		}

		private void tsClearWatchList_Click(object sender, EventArgs e)
		{
			_module.ScriptEngine.Global.Debug.ClearWatch();
		}

		private void tsWatchGlobal_Click(object sender, EventArgs e)
		{
			_module.ScriptEngine.EvalScript("Debug:Watch(\"_G\", _G)");
		}

		private void tsWatchCustom_Click(object sender, EventArgs e)
		{
			InputDiag input = new InputDiag("Specify the global variable to watch:", "Watch");
			if (input.ShowDialog() == DialogResult.OK)
			{
				_module.ScriptEngine.EvalScript("Debug:Watch(\"" + input.UserInput.Replace("\"", "") + "\", " + input.UserInput + ")");
			}
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
			tOutputPoll = new System.Windows.Forms.Timer(components);
			statusStrip1 = new System.Windows.Forms.StatusStrip();
			toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			tsslScriptFrameTime = new System.Windows.Forms.ToolStripStatusLabel();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			btnClearOutput = new System.Windows.Forms.Button();
			splitContainer2 = new System.Windows.Forms.SplitContainer();
			rtbOutput = new System.Windows.Forms.RichTextBox();
			textBox1 = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			btnCopyOutput = new System.Windows.Forms.Button();
			label2 = new System.Windows.Forms.Label();
			tvWatchWindow = new System.Windows.Forms.TreeView();
			cmsWatchWindow = new System.Windows.Forms.ContextMenuStrip(components);
			tsWatchGlobal = new System.Windows.Forms.ToolStripMenuItem();
			tsWatchCustom = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tsClearWatchList = new System.Windows.Forms.ToolStripMenuItem();
			panel1 = new System.Windows.Forms.Panel();
			btnReloadPacks = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel1.SuspendLayout();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			cmsWatchWindow.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			tOutputPoll.Enabled = true;
			tOutputPoll.Interval = 250;
			tOutputPoll.Tick += new System.EventHandler(tOutputPoll_Tick);
			statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { toolStripStatusLabel1, tsslScriptFrameTime });
			statusStrip1.Location = new System.Drawing.Point(0, 562);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Size = new System.Drawing.Size(1162, 22);
			statusStrip1.TabIndex = 3;
			statusStrip1.Text = "statusStrip1";
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new System.Drawing.Size(72, 17);
			toolStripStatusLabel1.Text = "Frame Time:";
			tsslScriptFrameTime.Name = "tsslScriptFrameTime";
			tsslScriptFrameTime.Size = new System.Drawing.Size(29, 17);
			tsslScriptFrameTime.Text = "0ms";
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(0, 0);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Panel1.Controls.Add(btnClearOutput);
			splitContainer1.Panel1.Controls.Add(splitContainer2);
			splitContainer1.Panel1.Controls.Add(btnCopyOutput);
			splitContainer1.Panel1.Controls.Add(label2);
			splitContainer1.Panel2.Controls.Add(tvWatchWindow);
			splitContainer1.Panel2.Controls.Add(panel1);
			splitContainer1.Panel2.Controls.Add(label1);
			splitContainer1.Size = new System.Drawing.Size(1162, 562);
			splitContainer1.SplitterDistance = 863;
			splitContainer1.TabIndex = 4;
			btnClearOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnClearOutput.Location = new System.Drawing.Point(659, 4);
			btnClearOutput.Name = "btnClearOutput";
			btnClearOutput.Size = new System.Drawing.Size(96, 23);
			btnClearOutput.TabIndex = 4;
			btnClearOutput.Text = "Clear Output";
			btnClearOutput.UseVisualStyleBackColor = true;
			btnClearOutput.Click += new System.EventHandler(btnClearOutput_Click);
			splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer2.Location = new System.Drawing.Point(0, 32);
			splitContainer2.Name = "splitContainer2";
			splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			splitContainer2.Panel1.Controls.Add(rtbOutput);
			splitContainer2.Panel2.Controls.Add(textBox1);
			splitContainer2.Panel2.Controls.Add(label3);
			splitContainer2.Panel2Collapsed = true;
			splitContainer2.Size = new System.Drawing.Size(863, 530);
			splitContainer2.SplitterDistance = 382;
			splitContainer2.TabIndex = 3;
			rtbOutput.AutoWordSelection = true;
			rtbOutput.BackColor = System.Drawing.Color.White;
			rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			rtbOutput.Font = new System.Drawing.Font("Consolas", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rtbOutput.HideSelection = false;
			rtbOutput.Location = new System.Drawing.Point(0, 0);
			rtbOutput.Name = "rtbOutput";
			rtbOutput.ReadOnly = true;
			rtbOutput.Size = new System.Drawing.Size(863, 530);
			rtbOutput.TabIndex = 0;
			rtbOutput.Text = "";
			rtbOutput.WordWrap = false;
			textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			textBox1.Location = new System.Drawing.Point(0, 32);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(150, 14);
			textBox1.TabIndex = 3;
			label3.BackColor = System.Drawing.Color.LightSkyBlue;
			label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label3.Dock = System.Windows.Forms.DockStyle.Top;
			label3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label3.Location = new System.Drawing.Point(0, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(150, 32);
			label3.TabIndex = 2;
			label3.Text = "   Eval";
			label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnCopyOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnCopyOutput.Location = new System.Drawing.Point(761, 4);
			btnCopyOutput.Name = "btnCopyOutput";
			btnCopyOutput.Size = new System.Drawing.Size(96, 23);
			btnCopyOutput.TabIndex = 2;
			btnCopyOutput.Text = "Copy Output";
			btnCopyOutput.UseVisualStyleBackColor = true;
			btnCopyOutput.Click += new System.EventHandler(btnCopyOutput_Click);
			label2.BackColor = System.Drawing.Color.LightSkyBlue;
			label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label2.Dock = System.Windows.Forms.DockStyle.Top;
			label2.Font = new System.Drawing.Font("Arial Narrow", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label2.Location = new System.Drawing.Point(0, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(863, 32);
			label2.TabIndex = 1;
			label2.Text = "   Script Output";
			label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			tvWatchWindow.ContextMenuStrip = cmsWatchWindow;
			tvWatchWindow.Dock = System.Windows.Forms.DockStyle.Fill;
			tvWatchWindow.Location = new System.Drawing.Point(0, 32);
			tvWatchWindow.Name = "tvWatchWindow";
			tvWatchWindow.Size = new System.Drawing.Size(295, 470);
			tvWatchWindow.TabIndex = 3;
			cmsWatchWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { tsWatchGlobal, tsWatchCustom, toolStripSeparator1, tsClearWatchList });
			cmsWatchWindow.Name = "cmsWatchWindow";
			cmsWatchWindow.Size = new System.Drawing.Size(160, 76);
			tsWatchGlobal.Name = "tsWatchGlobal";
			tsWatchGlobal.Size = new System.Drawing.Size(159, 22);
			tsWatchGlobal.Text = "Watch _G";
			tsWatchGlobal.Click += new System.EventHandler(tsWatchGlobal_Click);
			tsWatchCustom.Name = "tsWatchCustom";
			tsWatchCustom.Size = new System.Drawing.Size(159, 22);
			tsWatchCustom.Text = "Watch...";
			tsWatchCustom.Click += new System.EventHandler(tsWatchCustom_Click);
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
			tsClearWatchList.Name = "tsClearWatchList";
			tsClearWatchList.Size = new System.Drawing.Size(159, 22);
			tsClearWatchList.Text = "Clear Watch List";
			tsClearWatchList.Click += new System.EventHandler(tsClearWatchList_Click);
			panel1.Controls.Add(btnReloadPacks);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(0, 502);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(295, 60);
			panel1.TabIndex = 2;
			btnReloadPacks.Location = new System.Drawing.Point(16, 18);
			btnReloadPacks.Name = "btnReloadPacks";
			btnReloadPacks.Size = new System.Drawing.Size(96, 23);
			btnReloadPacks.TabIndex = 1;
			btnReloadPacks.Text = "Reload Packs";
			btnReloadPacks.UseVisualStyleBackColor = true;
			btnReloadPacks.Click += new System.EventHandler(btnReloadPacks_Click);
			label1.BackColor = System.Drawing.Color.PeachPuff;
			label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			label1.Dock = System.Windows.Forms.DockStyle.Top;
			label1.Font = new System.Drawing.Font("Arial Narrow", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label1.Location = new System.Drawing.Point(0, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(295, 32);
			label1.TabIndex = 0;
			label1.Text = "   Watch Window";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1162, 584);
			base.Controls.Add(splitContainer1);
			base.Controls.Add(statusStrip1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.Name = "ConsoleWindow";
			base.ShowInTaskbar = false;
			Text = "Script Console";
			base.TopMost = true;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			splitContainer2.Panel1.ResumeLayout(false);
			splitContainer2.Panel2.ResumeLayout(false);
			splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			cmsWatchWindow.ResumeLayout(false);
			panel1.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
