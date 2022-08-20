using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ideka.RacingMeter
{
	public class RacePointsEditPanel : Container
	{
		private const int Spacing = 10;

		private readonly Dictionary<RacePoint, MenuItem> _items = new Dictionary<RacePoint, MenuItem>();

		private readonly RaceEditor _editor;

		private readonly Panel _panel;

		private readonly Scrollbar _scrollbar;

		private readonly Menu _menu;

		private readonly StandardButton _nearestButton;

		private readonly StandardButton _deselectButton;

		private readonly StandardButton _prevButton;

		private readonly StandardButton _nextButton;

		private readonly StandardButton _insertBeforeButton;

		private readonly StandardButton _insertAfterButton;

		private float _scrollTarget = -1f;

		private Control _scrollItem;

		public RacePointsEditPanel(RaceEditor editor)
			: this()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Expected O, but got Unknown
			_editor = editor;
			Panel val = new Panel();
			((Control)val).set_Parent((Container)(object)this);
			val.set_ShowBorder(true);
			val.set_CanScroll(true);
			val.set_Title(Strings.RacePoints);
			_panel = val;
			_scrollbar = ((IEnumerable)((Container)this).get_Children()).OfType<Scrollbar>().First();
			Menu val2 = new Menu();
			((Control)val2).set_Parent((Container)(object)_panel);
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Container)val2).set_HeightSizingMode((SizingMode)1);
			_menu = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.SelectNearest);
			_nearestButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(Strings.Deselect);
			_deselectButton = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.Prev);
			_prevButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.Next);
			_nextButton = val6;
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text(Strings.InsertBefore);
			_insertBeforeButton = val7;
			StandardButton val8 = new StandardButton();
			((Control)val8).set_Parent((Container)(object)this);
			val8.set_Text(Strings.InsertAfter);
			_insertAfterButton = val8;
			((Control)_nearestButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SelectNearest();
			});
			((Control)_deselectButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Select(null);
			});
			((Control)_prevButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SelectPrevious();
			});
			((Control)_nextButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SelectNext();
			});
			((Control)_insertBeforeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.InsertCheckpoint(before: true);
			});
			((Control)_insertAfterButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.InsertCheckpoint(before: false);
			});
			_editor.RaceLoaded += RaceLoaded;
			_editor.PointSelected += PointSelected;
			_editor.PointRemoved += PointRemoved;
			_editor.PointInserted += PointInserted;
			_editor.PointSwapped += PointSwapped;
			_editor.PointModified += PointModified;
			UpdateLayout();
			RaceLoaded(_editor.FullRace);
		}

		private void RaceLoaded(FullRace fullRace)
		{
			((Container)_menu).ClearChildren();
			_items.Clear();
			StandardButton nearestButton = _nearestButton;
			StandardButton deselectButton = _deselectButton;
			StandardButton prevButton = _prevButton;
			StandardButton nextButton = _nextButton;
			StandardButton insertBeforeButton = _insertBeforeButton;
			bool flag;
			((Control)_insertAfterButton).set_Enabled(flag = fullRace != null);
			bool flag2;
			((Control)insertBeforeButton).set_Enabled(flag2 = flag);
			bool flag3;
			((Control)nextButton).set_Enabled(flag3 = flag2);
			bool flag4;
			((Control)prevButton).set_Enabled(flag4 = flag3);
			bool enabled;
			((Control)deselectButton).set_Enabled(enabled = flag4);
			((Control)nearestButton).set_Enabled(enabled);
			if (fullRace == null)
			{
				return;
			}
			foreach (RacePoint point in fullRace.Race.RacePoints)
			{
				AddPointItem(point);
			}
			RebuildMenu();
		}

		private void PointRemoved(RacePoint point)
		{
			_items.Remove(point);
			SaveScroll();
			RebuildMenu();
		}

		private void PointInserted(RacePoint point)
		{
			AddPointItem(point);
			SaveScroll();
			RebuildMenu();
		}

		private void PointSwapped(RacePoint point, bool before)
		{
			RebuildMenu();
		}

		private void PointModified(RacePoint obj)
		{
			if (_items.TryGetValue(obj, out var item))
			{
				item.set_Text(_editor.Describe(obj));
			}
		}

		private void PointSelected(RacePoint point)
		{
			MenuItem control;
			if (point == null)
			{
				MenuItem selectedMenuItem = _menu.get_SelectedMenuItem();
				if (selectedMenuItem != null)
				{
					selectedMenuItem.Deselect();
				}
			}
			else if (_items.TryGetValue(point, out control))
			{
				_menu.set_CanSelect(true);
				_menu.Select(control);
				_menu.set_CanSelect(false);
				_scrollItem = (Control)(object)control;
			}
		}

		private MenuItem AddPointItem(RacePoint point)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_0060: Expected O, but got Unknown
			if (_items.TryGetValue(point, out var old))
			{
				_items.Remove(point);
				((Control)old).set_Parent((Container)null);
				((Control)old).Dispose();
			}
			Dictionary<RacePoint, MenuItem> items = _items;
			RacePoint key = point;
			MenuItem val = new MenuItem();
			MenuItem val2 = val;
			items[key] = val;
			((Control)val2).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.Select(point);
			});
			return val2;
		}

		private void RebuildMenu()
		{
			((Container)_menu).ClearChildren();
			foreach (RacePoint point in _editor.Race.RacePoints)
			{
				MenuItem obj = AddPointItem(point);
				obj.set_Text(_editor.Describe(point));
				((Control)obj).set_Parent((Container)(object)_menu);
			}
			PointSelected(_editor.Selected);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (_editor != null)
			{
				((Control)_insertAfterButton).set_Left(0);
				StandardButton insertAfterButton = _insertAfterButton;
				Rectangle contentRegion = ((Container)this).get_ContentRegion();
				((Control)insertAfterButton).set_Bottom(((Rectangle)(ref contentRegion)).get_Bottom());
				((Control)(object)_insertAfterButton).ArrangeBottomUp(10, (Control)_insertBeforeButton, (Control)_deselectButton, (Control)_nearestButton, (Control)_panel);
				((Control)(object)_nearestButton).ArrangeLeftRight(10, (Control)_prevButton);
				((Control)(object)_prevButton).WidthFillRight();
				((Control)(object)_deselectButton).ArrangeLeftRight(10, (Control)_nextButton);
				((Control)(object)_nextButton).WidthFillRight();
				((Control)_panel).set_Width(((Container)this).get_ContentRegion().Width);
				((Control)(object)_panel).HeightFillUp();
			}
		}

		private void SaveScroll()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			_scrollTarget = _scrollbar.get_ScrollDistance() * (float)(((Control)_menu).get_Bottom() - ((Container)_panel).get_ContentRegion().Height);
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintBeforeChildren(spriteBatch, bounds);
			if (_scrollItem != null)
			{
				_scrollTarget = _panel.NearestScrollTarget(_scrollItem);
				_scrollItem = null;
			}
			if (_scrollTarget >= 0f)
			{
				_scrollbar.set_ScrollDistance(_scrollTarget / (float)(((Control)_menu).get_Bottom() - ((Container)_panel).get_ContentRegion().Height));
				_scrollTarget = -1f;
			}
		}

		protected override void DisposeControl()
		{
			_editor.RaceLoaded -= RaceLoaded;
			_editor.PointSelected -= PointSelected;
			_editor.PointRemoved -= PointRemoved;
			_editor.PointInserted -= PointInserted;
			_editor.PointSwapped -= PointSwapped;
			_editor.PointModified -= PointModified;
			((Container)this).DisposeControl();
		}
	}
}
