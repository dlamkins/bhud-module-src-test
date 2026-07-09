using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace rp.spark.UI.Controls
{
	internal sealed class PageListControls
	{
		private const int ButtonWidth = 90;

		private const int ControlHeight = 30;

		private const int Gap = 10;

		private readonly PageList _pageList;

		private readonly Action _pageChanged;

		private readonly StandardButton _previousButton;

		private readonly StandardButton _nextButton;

		private readonly Label _pageLabel;

		private int _itemCount;

		public Panel Root { get; }

		public Point Location
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)Root).get_Location();
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Control)Root).set_Location(value);
			}
		}

		public PageListControls(Container parent, PageList pageList, int width, Action pageChanged)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Expected O, but got Unknown
			_pageList = pageList ?? throw new ArgumentNullException("pageList");
			_pageChanged = pageChanged;
			Panel val = new Panel();
			val.set_ShowBorder(false);
			((Control)val).set_Size(new Point(width, 30));
			((Control)val).set_Parent(parent);
			Root = val;
			StandardButton val2 = new StandardButton();
			val2.set_Text("Previous");
			((Control)val2).set_Location(Point.get_Zero());
			((Control)val2).set_Size(new Point(90, 30));
			((Control)val2).set_Parent((Container)(object)Root);
			_previousButton = val2;
			StandardButton val3 = new StandardButton();
			val3.set_Text("Next");
			((Control)val3).set_Location(new Point(width - 90, 0));
			((Control)val3).set_Size(new Point(90, 30));
			((Control)val3).set_Parent((Container)(object)Root);
			_nextButton = val3;
			Label val4 = new Label();
			val4.set_Text(string.Empty);
			val4.set_Font(GameService.Content.get_DefaultFont12());
			val4.set_TextColor(new Color(220, 220, 220));
			((Control)val4).set_Location(new Point(100, 4));
			((Control)val4).set_Size(new Point(Math.Max(0, width - 200), 24));
			((Control)val4).set_Parent((Container)(object)Root);
			_pageLabel = val4;
			((Control)_previousButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_pageList.Previous())
				{
					Update(_itemCount);
					_pageChanged?.Invoke();
				}
			});
			((Control)_nextButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (_pageList.Next(_itemCount))
				{
					Update(_itemCount);
					_pageChanged?.Invoke();
				}
			});
			Update(0);
		}

		public void Update(int itemCount)
		{
			_itemCount = Math.Max(0, itemCount);
			_pageList.Clamp(_itemCount);
			int pageCount = _pageList.GetPageCount(_itemCount);
			((Control)_previousButton).set_Enabled(_pageList.PageIndex > 0);
			((Control)_nextButton).set_Enabled(_pageList.PageIndex + 1 < pageCount);
			_pageLabel.set_Text($"Page {_pageList.PageIndex + 1} of {pageCount}.");
		}
	}
}
