using System;
using System.Collections.Generic;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Microsoft.Xna.Framework;

namespace NpcFinder.Controls
{
	public sealed class ChangelogWindow : StandardWindow
	{
		private readonly IReadOnlyList<string> _pages;

		private readonly IReadOnlyList<string> _pageTitles;

		private int _pageIndex;

		private Panel _root;

		private Panel _viewport;

		private StandardButton _btnLatest;

		private StandardButton _btnOlder;

		private Label _pageInfo;

		private Label _text;

		public ChangelogWindow(AsyncTexture2D background, string changelogText)
			: this(background, new string[1] { changelogText ?? "" }, new string[1] { "Latest" })
		{
		}

		public ChangelogWindow(AsyncTexture2D background, IReadOnlyList<string> pages, IReadOnlyList<string> pageTitles = null)
			: this(background, new Rectangle(5, 60, 580, 590), new Rectangle(30, 20, 580, 550))
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			((WindowBase2)this).set_Title("Abattele's NPC Finder - Changelog");
			((WindowBase2)this).set_CanResize(false);
			((WindowBase2)this).set_SavesPosition(true);
			IReadOnlyList<string> pages2;
			if (pages == null || pages.Count <= 0)
			{
				IReadOnlyList<string> readOnlyList = new string[1] { "" };
				pages2 = readOnlyList;
			}
			else
			{
				pages2 = pages;
			}
			_pages = pages2;
			_pageTitles = ((pageTitles != null && pageTitles.Count == _pages.Count) ? pageTitles : null);
			BuildUi();
			SetPage(0);
		}

		public void SetPages(IReadOnlyList<string> pages, IReadOnlyList<string> pageTitles = null)
		{
			_ = pages?.Count;
		}

		public void SetText(string changelogText)
		{
			if (_pages.Count > 0)
			{
				_text.set_Text(changelogText ?? "");
			}
			UpdateHeader();
		}

		private void BuildUi()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Expected O, but got Unknown
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Expected O, but got Unknown
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Expected O, but got Unknown
			Rectangle cr = ((Container)this).get_ContentRegion();
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			((Control)val).set_Location(new Point(cr.X, cr.Y));
			((Control)val).set_Size(new Point(cr.Width, cr.Height));
			((Control)val).set_ClipsBounds(true);
			_root = val;
			Panel val2 = new Panel();
			((Control)val2).set_Parent((Container)(object)_root);
			((Control)val2).set_Location(new Point(0, 0));
			((Control)val2).set_Size(((Control)_root).get_Size());
			((Control)val2).set_ClipsBounds(true);
			_viewport = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)_viewport);
			((Control)val3).set_Location(new Point(8, 420));
			((Control)val3).set_Size(new Point(90, 28));
			val3.set_Text("Latest");
			_btnLatest = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)_viewport);
			((Control)val4).set_Location(new Point(104, 420));
			((Control)val4).set_Size(new Point(130, 28));
			val4.set_Text("Older versions");
			_btnOlder = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)_viewport);
			((Control)val5).set_Location(new Point(244, 425));
			val5.set_AutoSizeWidth(true);
			val5.set_AutoSizeHeight(true);
			val5.set_Text("");
			_pageInfo = val5;
			((Control)_btnLatest).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				SetPage(0);
			});
			((Control)_btnOlder).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				GoOlder();
			});
			Label val6 = new Label();
			((Control)val6).set_Parent((Container)(object)_viewport);
			((Control)val6).set_Location(new Point(8, 8));
			val6.set_AutoSizeHeight(true);
			((Control)val6).set_Width(((Control)_viewport).get_Width() - 16);
			val6.set_WrapText(true);
			val6.set_Text("");
			_text = val6;
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)_root).set_Location(new Point(contentRegion.X, contentRegion.Y));
				((Control)_root).set_Size(new Point(contentRegion.Width, contentRegion.Height));
				((Control)_viewport).set_Size(((Control)_root).get_Size());
				if (_text != null)
				{
					((Control)_text).set_Width(((Control)_viewport).get_Width() - 16);
				}
			});
		}

		private void GoOlder()
		{
			if (_pages.Count <= 1)
			{
				SetPage(0);
				return;
			}
			if (_pageIndex == 0)
			{
				_pageIndex = 1;
			}
			else
			{
				_pageIndex++;
				if (_pageIndex >= _pages.Count)
				{
					_pageIndex = 1;
				}
			}
			SetPage(_pageIndex);
		}

		private void SetPage(int index)
		{
			if (_pages != null && _pages.Count != 0)
			{
				index = Math.Max(0, Math.Min(index, _pages.Count - 1));
				_pageIndex = index;
				_text.set_Text(_pages[_pageIndex] ?? "");
				UpdateHeader();
			}
		}

		private void UpdateHeader()
		{
			if (_pageInfo == null)
			{
				return;
			}
			if (_pages == null || _pages.Count == 0)
			{
				_pageInfo.set_Text("");
				return;
			}
			string title = null;
			if (_pageTitles != null && _pageIndex >= 0 && _pageIndex < _pageTitles.Count)
			{
				title = _pageTitles[_pageIndex];
			}
			if (string.IsNullOrWhiteSpace(title))
			{
				title = ((_pageIndex == 0) ? "Latest" : $"Older ({_pageIndex}/{_pages.Count - 1})");
			}
			_pageInfo.set_Text(title);
		}
	}
}
