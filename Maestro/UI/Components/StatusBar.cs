using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class StatusBar : Panel
	{
		public static class Layout
		{
			public static int Height => 26;
		}

		private readonly Label _statusLabel;

		private readonly StandardButton _importButton;

		private int _visibleCount;

		private int _totalCount;

		public int VisibleCount
		{
			get
			{
				return _visibleCount;
			}
			set
			{
				_visibleCount = value;
				UpdateText();
			}
		}

		public int TotalCount
		{
			get
			{
				return _totalCount;
			}
			set
			{
				_totalCount = value;
				UpdateText();
			}
		}

		public event EventHandler ImportClicked;

		public StatusBar(int width)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(width, Layout.Height);
			base.BackgroundColor = Color.get_Transparent();
			_statusLabel = new Label
			{
				Parent = this,
				Location = new Point(0, 0),
				Width = width - 90 - 10,
				Height = base.Height,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroTheme.LightGray,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			_importButton = new StandardButton
			{
				Parent = this,
				Text = "Import",
				Location = new Point(width - 90, 0),
				Size = new Point(90, 26)
			};
			_importButton.Click += delegate
			{
				this.ImportClicked?.Invoke(this, EventArgs.Empty);
			};
		}

		private void UpdateText()
		{
			_statusLabel.Text = ((_visibleCount == _totalCount) ? $"  {_totalCount} songs" : $"  {_visibleCount} of {_totalCount} songs");
		}

		protected override void DisposeControl()
		{
			_statusLabel?.Dispose();
			_importButton?.Dispose();
			base.DisposeControl();
		}
	}
}
