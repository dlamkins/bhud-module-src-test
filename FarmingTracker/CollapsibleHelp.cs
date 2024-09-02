using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace FarmingTracker
{
	public class CollapsibleHelp : Panel
	{
		private const int HELP_LABEL_BORDER_SIZE = 10;

		private const string SHOW_HELP_BUTTON_TEXT = "Show Help";

		private const string HIDE_HELP_BUTTON_TEXT = "Hide Help";

		private bool _isHelpExpanded;

		private readonly Label _label;

		private int _expandedHeight;

		private readonly Panel _blackContainer;

		private int _expandedWidth;

		private readonly int _collapsedHeight;

		private readonly int _collapsedWidth;

		public CollapsibleHelp(string helpText, int expandedWidth, Container parent)
			: this()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			CollapsibleHelp collapsibleHelp = this;
			StandardButton val = new StandardButton();
			val.set_Text(GetHelpButtonText());
			((Control)val).set_BackgroundColor(Color.get_Yellow());
			((Control)val).set_Width(100);
			((Control)val).set_Top(10);
			((Control)val).set_Left(10);
			StandardButton button = val;
			_collapsedHeight = ((Control)button).get_Height() + 35;
			_collapsedWidth = ((Control)button).get_Width() + 35;
			((Panel)this).set_ShowBorder(true);
			((Control)this).set_Parent(parent);
			Label val2 = new Label();
			val2.set_Text(helpText);
			val2.set_VerticalAlignment((VerticalAlignment)0);
			val2.set_WrapText(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Top(_collapsedHeight - 20);
			((Control)val2).set_Left(10);
			_label = val2;
			Panel val3 = new Panel();
			((Control)val3).set_BackgroundColor(Color.get_Black() * 0.5f);
			((Control)val3).set_Top(4);
			((Control)val3).set_Left(4);
			((Control)val3).set_Parent((Container)(object)this);
			_blackContainer = val3;
			((Control)button).set_Parent((Container)(object)_blackContainer);
			((Control)_label).set_Parent((Container)(object)_blackContainer);
			((Control)button).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				collapsibleHelp._isHelpExpanded = !collapsibleHelp._isHelpExpanded;
				button.set_Text(collapsibleHelp.GetHelpButtonText());
				collapsibleHelp.UpdateSize(collapsibleHelp._expandedWidth);
			});
			UpdateSize(expandedWidth);
		}

		public void UpdateSize(int expandedWidth)
		{
			UpdateHeight();
			UpdateWidth(expandedWidth);
		}

		private void UpdateHeight()
		{
			_expandedHeight = ((Control)_label).get_Height() + _collapsedHeight + 10;
			((Control)this).set_Height(_isHelpExpanded ? _expandedHeight : _collapsedHeight);
			((Control)_blackContainer).set_Height(_expandedHeight);
		}

		private void UpdateWidth(int expandedWith)
		{
			_expandedWidth = expandedWith;
			((Control)_label).set_Width(expandedWith - 20 - 20);
			((Control)_blackContainer).set_Width(expandedWith - 20);
			((Control)this).set_Width(_isHelpExpanded ? _expandedWidth : _collapsedWidth);
		}

		private string GetHelpButtonText()
		{
			if (!_isHelpExpanded)
			{
				return "Show Help";
			}
			return "Hide Help";
		}
	}
}
