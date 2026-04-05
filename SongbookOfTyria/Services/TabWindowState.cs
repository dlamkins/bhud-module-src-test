using Microsoft.Xna.Framework;
using SongbookOfTyria.UI.Controls.Notation;

namespace SongbookOfTyria.Services
{
	public class TabWindowState
	{
		public int LocationX { get; set; }

		public int LocationY { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public NotationFontSize FontSize { get; set; } = NotationFontSize.Size20;


		public bool AutoScrollEnabled { get; set; }

		public float ScrollSpeed { get; set; } = 30f;


		public bool IsPracticeMode { get; set; }

		public Point GetLocation()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Point(LocationX, LocationY);
		}

		public Point GetSize()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return new Point(Width, Height);
		}

		public void SetLocation(Point location)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			LocationX = location.X;
			LocationY = location.Y;
		}

		public void SetSize(Point size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Width = size.X;
			Height = size.Y;
		}
	}
}
