using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class SortPanel : Panel
	{
		private readonly Label _label;

		public StandardButton RemoveSortButton { get; }

		public Dropdown Dropdown { get; }

		public SortPanel(Container parent, SortByWithDirection sortByWithDirection)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			((Control)this).set_BackgroundColor(Color.get_Black() * 0.5f);
			((Control)this).set_Height(38);
			((Control)this).set_Parent(parent);
			Label val = new Label();
			val.set_AutoSizeHeight(true);
			val.set_AutoSizeWidth(true);
			((Control)val).set_Top(10);
			((Control)val).set_Left(10);
			((Control)val).set_Parent((Container)(object)this);
			_label = val;
			Dropdown val2 = new Dropdown();
			((Control)val2).set_BackgroundColor(Color.get_Black() * 0.5f);
			((Control)val2).set_Width(300);
			((Control)val2).set_Top(5);
			((Control)val2).set_Left(70);
			((Control)val2).set_Parent((Container)(object)this);
			Dropdown = val2;
			Dictionary<SortByWithDirection, string> dropDownTextDict = GetSortByDropDownTexts();
			foreach (string dropDownText in dropDownTextDict.Values)
			{
				Dropdown.get_Items().Add(dropDownText);
			}
			Dropdown.set_SelectedItem(dropDownTextDict[sortByWithDirection]);
			StandardButton val3 = new StandardButton();
			val3.set_Text("x");
			((Control)val3).set_Width(28);
			((Control)val3).set_Top(5);
			((Control)val3).set_Left(((Control)Dropdown).get_Right() + 5);
			((Control)val3).set_Parent((Container)(object)this);
			RemoveSortButton = val3;
			((Control)this).set_Width(((Control)RemoveSortButton).get_Right() + 5);
		}

		public SortByWithDirection GetSelectedSortBy()
		{
			return GetSortByDropDownTexts().First((KeyValuePair<SortByWithDirection, string> d) => d.Value == Dropdown.get_SelectedItem()).Key;
		}

		public void SetLabelText(string text)
		{
			_label.set_Text(text);
		}

		private static Dictionary<SortByWithDirection, string> GetSortByDropDownTexts()
		{
			SortByWithDirection[] obj = (SortByWithDirection[])Enum.GetValues(typeof(SortByWithDirection));
			Dictionary<SortByWithDirection, string> dropDownTextBySortBy = new Dictionary<SortByWithDirection, string>();
			SortByWithDirection[] array = obj;
			for (int i = 0; i < array.Length; i++)
			{
				SortByWithDirection sortBy = array[i];
				dropDownTextBySortBy[sortBy] = Helper.ConvertEnumValueToTextWithBlanks(sortBy.ToString());
			}
			return dropDownTextBySortBy;
		}
	}
}
