using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class StatusBar : Panel
	{
		public static class Layout
		{
			public const int Height = 24;
		}

		private readonly Label _statusLabel;

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

		public StatusBar(int width)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(width, 24);
			base.BackgroundColor = Color.get_Transparent();
			_statusLabel = new Label
			{
				Parent = this,
				Location = new Point(0, 0),
				Width = base.Width,
				Height = base.Height,
				Font = GameService.Content.DefaultFont12,
				TextColor = MaestroColors.LightGray,
				HorizontalAlignment = HorizontalAlignment.Left
			};
		}

		private void UpdateText()
		{
			_statusLabel.Text = ((_visibleCount == _totalCount) ? $"  {_totalCount} songs" : $"  {_visibleCount} of {_totalCount} songs");
		}

		protected override void DisposeControl()
		{
			_statusLabel?.Dispose();
			base.DisposeControl();
		}
	}
}
