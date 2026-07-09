using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using rp.spark.UI.Controls;

namespace rp.spark.UI.Views
{
	internal static class SparkFormLayout
	{
		public static FlowPanel AddVerticalStack(Container parent, int x, int y, int width, int height, int gap = 10, bool canScroll = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Location(new Point(x, y));
			((Control)val).set_Size(new Point(width, height));
			((Panel)val).set_CanScroll(canScroll);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, (float)gap));
			return val;
		}

		public static FlowPanel AddAutoStack(Container parent, int width, int gap = 5)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(width);
			((Container)val).set_HeightSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			val.set_ControlPadding(new Vector2(0f, (float)gap));
			return val;
		}

		public static FlowPanel AddRow(Container parent, int width, int height, int gap = 10)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			FlowPanel val = new FlowPanel();
			((Control)val).set_Parent(parent);
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			val.set_FlowDirection((ControlFlowDirection)2);
			val.set_ControlPadding(new Vector2((float)gap, 0f));
			return val;
		}

		public static Label AddLabel(Container parent, string text, int width, int height = 25, BitmapFont font = null, Color? textColor = null, bool strokeText = false)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			Label val = new Label();
			val.set_Text(text ?? string.Empty);
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			val.set_Font(font ?? GameService.Content.get_DefaultFont14());
			val.set_TextColor((Color)(((_003F?)textColor) ?? Color.get_White()));
			val.set_StrokeText(strokeText);
			((Control)val).set_Parent(parent);
			return val;
		}

		public static TextBox AddTextBox(Container parent, string text, string placeholderText, int width, int height = 35, int? maxLength = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			TextBox val = new TextBox();
			((TextInputBase)val).set_Text(text ?? string.Empty);
			((TextInputBase)val).set_PlaceholderText(placeholderText ?? string.Empty);
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Parent(parent);
			TextBox textBox = val;
			if (maxLength.HasValue)
			{
				((TextInputBase)textBox).set_MaxLength(maxLength.Value);
			}
			return textBox;
		}

		public static SparkMultiline AddMultilineTextBox(Container parent, string text, string placeholderText, int width, int height, int? maxLength = null)
		{
			SparkMultiline sparkMultiline = new SparkMultiline();
			((TextInputBase)sparkMultiline).set_Text(text ?? string.Empty);
			((TextInputBase)sparkMultiline).set_PlaceholderText(placeholderText ?? string.Empty);
			((Control)sparkMultiline).set_Width(width);
			((Control)sparkMultiline).set_Height(height);
			((Control)sparkMultiline).set_Parent(parent);
			SparkMultiline textBox = sparkMultiline;
			if (maxLength.HasValue)
			{
				((TextInputBase)textBox).set_MaxLength(maxLength.Value);
			}
			textBox.AttachWheelSource(parent);
			return textBox;
		}

		public static SparkMultiline AddLabeledMultilineTextBox(FlowPanel parent, string labelText, string text, string placeholderText, int width, int height, int? maxLength = null)
		{
			FlowPanel parent2 = AddAutoStack((Container)(object)parent, width, 0);
			AddLabel((Container)(object)parent2, labelText, width);
			return AddMultilineTextBox((Container)(object)parent2, text, placeholderText, width, height, maxLength);
		}

		public static TextBox AddLabeledTextBox(FlowPanel parent, string labelText, string text, string placeholderText, int width, int height = 35, int? maxLength = null)
		{
			FlowPanel parent2 = AddAutoStack((Container)(object)parent, width, 0);
			AddLabel((Container)(object)parent2, labelText, width);
			return AddTextBox((Container)(object)parent2, text, placeholderText, width, height, maxLength);
		}

		public static Dropdown AddDropdown(Container parent, IEnumerable<string> options, string selectedItem, int width, int height = 35)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			Dropdown val = new Dropdown();
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Parent(parent);
			Dropdown dropdown = val;
			foreach (string option in options ?? new string[0])
			{
				dropdown.get_Items().Add(option);
			}
			dropdown.set_SelectedItem(selectedItem);
			return dropdown;
		}

		public static Checkbox AddCheckbox(Container parent, string text, bool isChecked, int width, int height = 30)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			Checkbox val = new Checkbox();
			val.set_Text(text ?? string.Empty);
			val.set_Checked(isChecked);
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Parent(parent);
			return val;
		}

		public static StandardButton AddButton(Container parent, string text, int width, int height = 35, bool enabled = true)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			StandardButton val = new StandardButton();
			val.set_Text(text ?? string.Empty);
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Enabled(enabled);
			((Control)val).set_Parent(parent);
			return val;
		}

		public static Panel AddSpacer(Container parent, int width, int height)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			Panel val = new Panel();
			((Control)val).set_Width(width);
			((Control)val).set_Height(height);
			((Control)val).set_Parent(parent);
			return val;
		}
	}
}
