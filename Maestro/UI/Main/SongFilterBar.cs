using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Maestro.UI.Controls;
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

			public const int SearchBoxWidth = 150;

			public const int FilterButtonWidth = 210;
		}

		private readonly TextBox _searchBox;

		private readonly GenericFilterButton _filterButton;

		public string SearchText => ((TextInputBase)_searchBox).get_Text()?.Trim().ToLower() ?? string.Empty;

		public string SelectedSource => _filterButton.SelectedValue1;

		public string SelectedInstrument => _filterButton.SelectedValue2;

		public string SelectedSort => _filterButton.SelectedValue3;

		public static bool IsTextInputFocused { get; private set; }

		public static bool WasJustUnfocused { get; set; }

		public bool IsFilterPanelOpen => _filterButton?.PanelOpen ?? false;

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
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(width, 36));
			((Control)this).set_BackgroundColor(Color.get_Transparent());
			TextBox val = new TextBox();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(0, 4));
			((Control)val).set_Width(150);
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
			GenericFilterButton genericFilterButton = new GenericFilterButton(new FilterSection
			{
				Items = new string[5] { "All", "Bundled", "Created", "Imported", "Community" },
				DefaultValue = "All"
			}, new FilterSection
			{
				Items = new string[5] { "All", "Piano", "Harp", "Lute", "Bass" },
				DefaultValue = "All"
			}, new FilterSection
			{
				Items = new string[2] { "Name A-Z", "Name Z-A" },
				DefaultValue = "Name A-Z"
			});
			((Control)genericFilterButton).set_Parent((Container)(object)this);
			((Control)genericFilterButton).set_Location(new Point(width - 210, 4));
			((Control)genericFilterButton).set_Width(210);
			_filterButton = genericFilterButton;
			_filterButton.FilterChanged += delegate
			{
				this.FilterChanged?.Invoke(this, EventArgs.Empty);
			};
		}

		public void HideFilterPanel()
		{
			_filterButton?.HidePanel();
		}

		protected override void DisposeControl()
		{
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			GenericFilterButton filterButton = _filterButton;
			if (filterButton != null)
			{
				((Control)filterButton).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
