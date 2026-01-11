using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Components
{
	public class SongFilterBar : Panel
	{
		public static class Layout
		{
			public const int Height = 36;

			public const int ControlY = 4;

			public const int SearchBoxX = 0;

			public const int SearchBoxWidth = 180;

			public const int FilterButtonWidth = 180;
		}

		private readonly TextBox _searchBox;

		private readonly FilterButton _filterButton;

		public string SearchText => _searchBox.Text?.Trim().ToLower() ?? string.Empty;

		public string SelectedSource => _filterButton.SelectedSource;

		public string SelectedInstrument => _filterButton.SelectedInstrument;

		public event EventHandler SearchChanged;

		public event EventHandler FilterChanged;

		public SongFilterBar(int width)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(width, 36);
			base.BackgroundColor = Color.get_Transparent();
			_searchBox = new TextBox
			{
				Parent = this,
				Location = new Point(0, 4),
				Width = 180,
				PlaceholderText = "Search songs..."
			};
			_searchBox.TextChanged += delegate
			{
				this.SearchChanged?.Invoke(this, EventArgs.Empty);
			};
			_filterButton = new FilterButton
			{
				Parent = this,
				Location = new Point(width - 180, 4),
				Width = 180
			};
			_filterButton.FilterChanged += delegate
			{
				this.FilterChanged?.Invoke(this, EventArgs.Empty);
			};
		}

		protected override void DisposeControl()
		{
			_searchBox?.Dispose();
			_filterButton?.Dispose();
			base.DisposeControl();
		}
	}
}
