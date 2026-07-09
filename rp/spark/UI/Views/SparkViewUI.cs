using System.Collections.Generic;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace rp.spark.UI.Views
{
	internal static class SparkViewUI
	{
		public static readonly Color SecondaryTextColor = new Color(220, 220, 220);

		public static readonly Color WarningTextColor = new Color(255, 170, 40);

		public static Label AddLabel(Container parent, string text, int x, int y, int width, int height = 25, BitmapFont font = null, Color? textColor = null, bool strokeText = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(text ?? string.Empty);
			val.set_TextColor((Color)(((_003F?)textColor) ?? Color.get_White()));
			val.set_StrokeText(strokeText);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Control)val).set_Parent(parent);
			Label label = val;
			if (font != null)
			{
				label.set_Font(font);
			}
			return label;
		}

		public static StandardButton AddButton(Container parent, string text, int x, int y, int width, int height = 35, bool enabled = true)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			val.set_Text(text ?? string.Empty);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Control)val).set_Parent(parent);
			((Control)val).set_Enabled(enabled);
			return val;
		}

		public static Checkbox AddCheckbox(Container parent, string text, bool isChecked, int x, int y, int width, int height = 30)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			val.set_Text(text ?? string.Empty);
			val.set_Checked(isChecked);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Control)val).set_Parent(parent);
			return val;
		}

		public static Dropdown AddDropdown(Container parent, IEnumerable<string> options, string selectedItem, int x, int y, int width, int height = 35)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Control)val).set_Parent(parent);
			Dropdown dropdown = val;
			foreach (string option in options ?? new string[0])
			{
				dropdown.get_Items().Add(option);
			}
			dropdown.set_SelectedItem(selectedItem);
			return dropdown;
		}

		public static TextBox AddTextBox(Container parent, string text, string placeholderText, int x, int y, int width, int height = 35, int? maxLength = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_Text(text ?? string.Empty);
			((TextInputBase)val).set_PlaceholderText(placeholderText ?? string.Empty);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Control)val).set_Parent(parent);
			TextBox textBox = val;
			if (maxLength.HasValue)
			{
				((TextInputBase)textBox).set_MaxLength(maxLength.Value);
			}
			return textBox;
		}
	}
}
