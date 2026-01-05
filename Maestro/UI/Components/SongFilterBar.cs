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

			public const int DropdownWidth = 150;
		}

		private readonly TextBox _searchBox;

		private readonly Dropdown _instrumentFilter;

		public string SearchText => ((TextInputBase)_searchBox).get_Text()?.Trim().ToLower() ?? string.Empty;

		public string SelectedInstrument => _instrumentFilter.get_SelectedItem();

		public event EventHandler SearchChanged;

		public event EventHandler<ValueChangedEventArgs> FilterChanged;

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
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
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
			Dropdown val2 = new Dropdown();
			((Control)val2).set_Parent((Container)(object)this);
			((Control)val2).set_Location(new Point(width - 150, 4));
			((Control)val2).set_Width(150);
			_instrumentFilter = val2;
			_instrumentFilter.get_Items().Add("All Instruments");
			_instrumentFilter.get_Items().Add("Piano");
			_instrumentFilter.get_Items().Add("Harp");
			_instrumentFilter.get_Items().Add("Lute");
			_instrumentFilter.get_Items().Add("Bass");
			_instrumentFilter.set_SelectedItem("All Instruments");
			_instrumentFilter.add_ValueChanged((EventHandler<ValueChangedEventArgs>)delegate(object s, ValueChangedEventArgs e)
			{
				this.FilterChanged?.Invoke(this, e);
			});
		}

		protected override void DisposeControl()
		{
			TextBox searchBox = _searchBox;
			if (searchBox != null)
			{
				((Control)searchBox).Dispose();
			}
			Dropdown instrumentFilter = _instrumentFilter;
			if (instrumentFilter != null)
			{
				((Control)instrumentFilter).Dispose();
			}
			((Panel)this).DisposeControl();
		}
	}
}
