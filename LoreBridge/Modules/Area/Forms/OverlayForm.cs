using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoreBridge.Modules.Area.Forms
{
	public sealed class OverlayForm : Form
	{
		private readonly Pen _pen = new Pen(Color.White, 2f);

		private Rectangle? _rectangle;

		private Point _startMousePos;

		private readonly Timer _fadeInTimer;

		private readonly Timer _fadeOutTimer;

		private double _currentOpacity;

		private readonly double _targetOpacity = 0.5;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams obj = base.CreateParams;
				obj.ExStyle |= 134217728;
				return obj;
			}
		}

		public event EventHandler<Rectangle> AreaSelected;

		public event EventHandler<bool> Hidden;

		public OverlayForm()
		{
			BackColor = Color.Black;
			base.TransparencyKey = Color.Black;
			base.FormBorderStyle = FormBorderStyle.None;
			base.Bounds = Screen.PrimaryScreen.Bounds;
			base.AllowTransparency = true;
			base.ShowInTaskbar = false;
			base.TopMost = true;
			Cursor = Cursors.Cross;
			DoubleBuffered = true;
			base.Opacity = 0.0;
			_fadeInTimer = new Timer();
			_fadeInTimer.Interval = 16;
			_fadeInTimer.Tick += OnFadeInTick;
			_fadeOutTimer = new Timer();
			_fadeOutTimer.Interval = 16;
			_fadeOutTimer.Tick += OnFadeOutTick;
			base.MouseDown += OnMouseDown;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			base.TransparencyKey = Color.Red;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			StartFadeIn();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_fadeInTimer?.Stop();
				_fadeInTimer?.Dispose();
				_fadeOutTimer?.Stop();
				_fadeOutTimer?.Dispose();
				_pen?.Dispose();
				base.MouseDown -= OnMouseDown;
				base.MouseMove -= OnMouseMove;
				base.MouseUp -= OnMouseUp;
			}
			base.Dispose(disposing);
		}

		private new void Hide()
		{
			this.Hidden?.Invoke(this, e: true);
			base.Hide();
		}

		private void StartFadeIn()
		{
			_fadeInTimer.Start();
		}

		private void OnFadeInTick(object sender, EventArgs e)
		{
			_currentOpacity += 0.15;
			if (_currentOpacity >= _targetOpacity)
			{
				_currentOpacity = _targetOpacity;
				_fadeInTimer.Stop();
			}
			base.Opacity = _currentOpacity;
		}

		private void StartFadeOut()
		{
			_fadeInTimer.Stop();
			_currentOpacity = base.Opacity;
			_fadeOutTimer.Start();
		}

		private void OnFadeOutTick(object sender, EventArgs e)
		{
			_currentOpacity -= 0.15;
			if (_currentOpacity <= 0.0)
			{
				_currentOpacity = 0.0;
				_fadeOutTimer.Stop();
				Hide();
			}
			base.Opacity = _currentOpacity;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (_rectangle.HasValue)
			{
				e.Graphics.DrawRectangle(_pen, _rectangle.Value);
			}
			base.OnPaint(e);
		}

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			MouseButtons button = e.Button;
			if (button <= MouseButtons.Right)
			{
				switch (button)
				{
				case MouseButtons.Left:
					_startMousePos = e.Location;
					base.MouseMove += OnMouseMove;
					base.MouseUp += OnMouseUp;
					break;
				case MouseButtons.Right:
					StartFadeOut();
					break;
				}
			}
			else if (button != MouseButtons.Middle && button != MouseButtons.XButton1)
			{
				_ = 16777216;
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			_rectangle = GetRectangle(_startMousePos, e.Location);
			Invalidate();
		}

		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			StartFadeOut();
			if (e.Button == MouseButtons.Left)
			{
				Rectangle rectangle = GetRectangle(_startMousePos, e.Location);
				if (rectangle.Width > 10 && rectangle.Height > 10)
				{
					this.AreaSelected?.Invoke(this, rectangle);
				}
			}
		}

		private static Rectangle GetRectangle(Point point1, Point point2)
		{
			return new Rectangle(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), Math.Abs(point1.X - point2.X), Math.Abs(point1.Y - point2.Y));
		}
	}
}
