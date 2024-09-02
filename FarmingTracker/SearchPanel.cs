using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace FarmingTracker
{
	public class SearchPanel : Panel
	{
		private readonly TextBox _searchTextBox;

		private readonly StandardButton _clearSearchButton;

		public SearchPanel(Services services, Container parent)
			: this()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			SearchPanel searchPanel = this;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Control)this).set_Parent(parent);
			TextBox val = new TextBox();
			((TextInputBase)val).set_Text(services.SearchTerm);
			((TextInputBase)val).set_PlaceholderText("Search...");
			((Control)val).set_Left(4);
			((Control)val).set_Width(300);
			((Control)val).set_BasicTooltipText("Search for items and currencies that include the search term in their name (case insensitive).");
			((Control)val).set_Parent((Container)(object)this);
			_searchTextBox = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("x");
			((Control)val2).set_Top(((Control)_searchTextBox).get_Top());
			((Control)val2).set_Left(((Control)_searchTextBox).get_Right() - 30);
			((Control)val2).set_Width(30);
			((Control)val2).set_BasicTooltipText("Clear search input");
			((Control)val2).set_Visible(!string.IsNullOrWhiteSpace(((TextInputBase)_searchTextBox).get_Text()));
			((Control)val2).set_Parent((Container)(object)this);
			_clearSearchButton = val2;
			((Control)_clearSearchButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				((TextInputBase)searchPanel._searchTextBox).set_Text("");
			});
			((TextInputBase)_searchTextBox).add_TextChanged((EventHandler<EventArgs>)delegate
			{
				bool visible = !string.IsNullOrWhiteSpace(((TextInputBase)searchPanel._searchTextBox).get_Text());
				((Control)searchPanel._clearSearchButton).set_Visible(visible);
				services.SearchTerm = ((TextInputBase)searchPanel._searchTextBox).get_Text();
				services.UpdateLoop.TriggerUpdateUi();
			});
			SetSize(((Control)parent).get_Width());
		}

		public void UpdateSize(int width)
		{
			SetSize(width);
		}

		private void SetSize(int width)
		{
			((Control)_searchTextBox).set_Width((width > 300) ? 300 : width);
			((Control)_clearSearchButton).set_Left(((Control)_searchTextBox).get_Right() - 30);
		}
	}
}
