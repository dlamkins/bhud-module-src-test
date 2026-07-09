using Blish_HUD.Controls;

namespace rp.spark.UI.Views
{
	internal sealed class ProfileListSearchControls
	{
		public TextBox SearchBox { get; }

		public Dropdown SearchFieldDropdown { get; }

		public Dropdown SortDropdown { get; }

		public ProfileListSearchControls(TextBox searchBox, Dropdown searchFieldDropdown, Dropdown sortDropdown)
		{
			SearchBox = searchBox;
			SearchFieldDropdown = searchFieldDropdown;
			SortDropdown = sortDropdown;
		}
	}
}
