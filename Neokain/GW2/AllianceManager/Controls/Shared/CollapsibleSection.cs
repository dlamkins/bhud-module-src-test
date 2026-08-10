using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	public class CollapsibleSection : Panel
	{
		private const int SECTION_HEADER_HEIGHT = 45;

		private const int SECTION_HEADER_PADDING = 10;

		private const int CONTENT_TOP_PADDING = 5;

		private readonly Label _headerLabel;

		private readonly Label _toggleIndicator;

		private readonly Panel _contentPanel;

		private bool _isExpanded = true;

		private int _expandedHeight = 400;

		private string _headerText;

		private const string ExpandedIndicator = "▼";

		private const string CollapsedIndicator = "▶";

		public string HeaderText
		{
			get
			{
				return _headerText;
			}
			set
			{
				_headerText = value;
				if (_headerLabel != null)
				{
					_headerLabel.set_Text(value);
				}
			}
		}

		public bool IsExpanded
		{
			get
			{
				return _isExpanded;
			}
			set
			{
				if (_isExpanded != value)
				{
					_isExpanded = value;
					UpdateExpandedState();
				}
			}
		}

		public int ExpandedContentHeight
		{
			get
			{
				return _expandedHeight;
			}
			set
			{
				_expandedHeight = value;
				if (_isExpanded)
				{
					((Control)this).set_Height(_expandedHeight + 45 + 5);
				}
			}
		}

		public Panel ContentPanel => _contentPanel;

		public event EventHandler<bool> ExpandedChanged;

		public CollapsibleSection(string headerText)
			: this()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			_headerText = headerText;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Height(45);
			((Container)val).set_WidthSizingMode((SizingMode)2);
			((Control)val).set_Top(0);
			((Control)val).set_Left(0);
			Panel headerPanel = val;
			((Control)headerPanel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				Toggle();
			});
			Label val2 = new Label();
			((Control)val2).set_Parent((Container)(object)headerPanel);
			val2.set_Text("▼");
			val2.set_Font(GameService.Content.get_DefaultFont18());
			val2.set_AutoSizeWidth(true);
			val2.set_AutoSizeHeight(true);
			((Control)val2).set_Left(10);
			((Control)val2).set_Top(10);
			_toggleIndicator = val2;
			Label val3 = new Label();
			((Control)val3).set_Parent((Container)(object)headerPanel);
			val3.set_Text(headerText);
			val3.set_Font(GameService.Content.get_DefaultFont18());
			val3.set_AutoSizeWidth(true);
			val3.set_AutoSizeHeight(true);
			((Control)val3).set_Left(35);
			((Control)val3).set_Top(13);
			_headerLabel = val3;
			Panel val4 = new Panel();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Top(50);
			((Control)val4).set_Left(0);
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)2);
			_contentPanel = val4;
			((Control)this).set_Height(_expandedHeight + 45 + 5);
		}

		public void Toggle()
		{
			IsExpanded = !IsExpanded;
		}

		private void UpdateExpandedState()
		{
			if (_isExpanded)
			{
				((Control)this).set_Height(_expandedHeight + 45 + 5);
				((Control)_contentPanel).set_Visible(true);
				_toggleIndicator.set_Text("▼");
			}
			else
			{
				((Control)this).set_Height(45);
				((Control)_contentPanel).set_Visible(false);
				_toggleIndicator.set_Text("▶");
			}
			this.ExpandedChanged?.Invoke(this, _isExpanded);
		}

		public override void RecalculateLayout()
		{
			((Panel)this).RecalculateLayout();
			if (_contentPanel != null && _isExpanded)
			{
				((Control)_contentPanel).set_Height(((Control)this).get_Height() - 45 - 5);
			}
		}
	}
}
