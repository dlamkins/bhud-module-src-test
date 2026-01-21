using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Maestro.UI.Main
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

		public string SearchText => ((TextInputBase)_searchBox).get_Text()?.Trim().ToLower() ?? string.Empty;

		public string SelectedSource => _filterButton.SelectedSource;

		public string SelectedInstrument => _filterButton.SelectedInstrument;

		public static bool IsTextInputFocused { get; private set; }

		public static bool WasJustUnfocused { get; set; }

		public event EventHandler SearchChanged;

		public event EventHandler FilterChanged;

		public SongFilterBar(int width)
			: this()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, 36));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 4));
			((Control)val).set_Width(180);
			((TextInputBase)val).set_PlaceholderText("Search songs...");
			_searchBox = val;
			((TextInputBase)_searchBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				this.SearchChanged?.Invoke(this, EventArgs.Empty);
			});
			((TextInputBase)_searchBox).add_InputFocusChanged((EventHandler<ValueEventArgs<bool>>)delegate(object s, ValueEventArgs<bool> e)
			{
				if (!e.get_Value() && IsTextInputFocused)
				{
					WasJustUnfocused = true;
				}
				IsTextInputFocused = e.get_Value();
			});
			FilterButton filterButton = new FilterButton();
			((Control)filterButton).set_Parent((Container)(object)this);
			((Control)filterButton).set_Location(new Point(width - 180, 4));
			((Control)filterButton).set_Width(180);
			_filterButton = filterButton;
			_filterButton.FilterChanged += delegate
			{
				this.FilterChanged?.Invoke(this, EventArgs.Empty);
			};
		}

		protected override void DisposeControl()
		{
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			FilterButton filterButton = _filterButton;
			if (filterButton != null)
			{
				((Control)filterButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
