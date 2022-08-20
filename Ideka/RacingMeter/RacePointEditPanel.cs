using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.NetCommon;
using Ideka.RacingMeterLib;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class RacePointEditPanel : Panel
	{
		private const int Spacing = 10;

		private readonly RaceEditor _editor;

		private readonly Vector3Box _posBox;

		private readonly FloatBox _radiusBox;

		private readonly StandardButton _radius5mButton;

		private readonly StandardButton _radius300inButton;

		private readonly StandardButton _radius15mButton;

		private readonly EnumDropdown<RacePointType> _typeSelect;

		private readonly StandardButton _snapXYZButton;

		private readonly StandardButton _snapXYButton;

		private readonly StandardButton _removeButton;

		private readonly StandardButton _prevSwapButton;

		private readonly StandardButton _nextSwapButton;

		public RacePointEditPanel(RaceEditor editor)
			: this()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Expected O, but got Unknown
			_editor = editor;
			((Panel)this).set_ShowTint(true);
			((Panel)this).set_Title(" ");
			Vector3Box vector3Box = new Vector3Box();
			((Control)vector3Box).set_Parent((Container)(object)this);
			_posBox = vector3Box;
			FloatBox floatBox = new FloatBox();
			((Control)floatBox).set_Parent((Container)(object)this);
			floatBox.Label = Strings.Radius;
			floatBox.MinValue = 1f;
			_radiusBox = floatBox;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.Meters.Format(5));
			_radius5mButton = val;
			val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.Inches.Format(300));
			_radius300inButton = val;
			val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text(Strings.Meters.Format(15));
			_radius15mButton = val;
			EnumDropdown<RacePointType> enumDropdown = new EnumDropdown<RacePointType>();
			((Control)enumDropdown).set_Parent((Container)(object)this);
			enumDropdown.Label = Strings.PointType;
			_typeSelect = enumDropdown;
			StandardButton val2 = new StandardButton();
			((Control)val2).set_Parent((Container)(object)this);
			val2.set_Text(Strings.AvatarSnapXYZ);
			_snapXYZButton = val2;
			StandardButton val3 = new StandardButton();
			((Control)val3).set_Parent((Container)(object)this);
			val3.set_Text(Strings.AvatarSnapXY);
			_snapXYButton = val3;
			StandardButton val4 = new StandardButton();
			((Control)val4).set_Parent((Container)(object)this);
			val4.set_Text(Strings.Delete);
			_removeButton = val4;
			StandardButton val5 = new StandardButton();
			((Control)val5).set_Parent((Container)(object)this);
			val5.set_Text(Strings.SwapPrev);
			_prevSwapButton = val5;
			StandardButton val6 = new StandardButton();
			((Control)val6).set_Parent((Container)(object)this);
			val6.set_Text(Strings.SwapNext);
			_nextSwapButton = val6;
			_posBox.TempValue += _editor.MovePointPreview;
			_posBox.ValueCommitted += _editor.MovePoint;
			_radiusBox.TempValue += _editor.ResizePointPreview;
			_radiusBox.TempClear += _editor.ResizePointPreview;
			_radiusBox.ValueCommitted += _editor.ResizePoint;
			((Control)_radius5mButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.ResizePoint(5f);
			});
			((Control)_radius300inButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.ResizePoint(7.62f);
			});
			((Control)_radius15mButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.ResizePoint(15f);
			});
			_typeSelect.ValueCommitted += _editor.SetPointType;
			((Control)_snapXYZButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				_editor.MovePoint(RacingModule.Measurer.Pos.Meters);
			});
			((Control)_snapXYButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				Vector3 meters = RacingModule.Measurer.Pos.Meters;
				_editor.MovePoint(meters.X, meters.Y);
			});
			((Control)_removeButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.RemoveCheckpoint();
			});
			((Control)_prevSwapButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SwapCheckpoint(previous: true);
			});
			((Control)_nextSwapButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SwapCheckpoint(previous: false);
			});
			_editor.PointSelected += PointSelected;
			_editor.PointModified += PointModified;
			_editor.PointRemoved += PointRemoved;
			_editor.PointInserted += PointInserted;
			_editor.PointSwapped += PointSwapped;
			UpdateLayout();
			UpdateValues();
		}

		private void UpdateValues()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			RacePoint selected = _editor.Selected;
			((Control)this).set_Visible(selected != null);
			if (((Control)this).get_Visible())
			{
				UpdateTitle();
				_posBox.Value = selected.Position;
				_radiusBox.Value = selected.Radius;
				_typeSelect.Value = selected.Type;
			}
		}

		private void UpdateTitle()
		{
			((Panel)this).set_Title(_editor.Describe(_editor.Selected) ?? " ");
		}

		private void PointSelected(RacePoint _)
		{
			UpdateValues();
		}

		private void PointModified(RacePoint point)
		{
			if (point == _editor.Selected)
			{
				UpdateValues();
			}
		}

		private void PointRemoved(RacePoint _)
		{
			UpdateTitle();
		}

		private void PointInserted(RacePoint _)
		{
			UpdateTitle();
		}

		private void PointSwapped(RacePoint _, bool __)
		{
			UpdateTitle();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			if (_editor != null)
			{
				((Control)_snapXYZButton).set_Left(10);
				((Control)(object)_snapXYZButton).ArrangeLeftRight(10, (Control)_snapXYButton, (Control)_removeButton);
				((Control)_prevSwapButton).set_Left(10);
				((Control)(object)_prevSwapButton).ArrangeLeftRight(10, (Control)_nextSwapButton);
				if (((Container)this).get_ContentRegion().Width != ((Control)_removeButton).get_Right() + 10)
				{
					((Container)(object)this).SetContentRegionWidth(((Control)_removeButton).get_Right() + 10);
					return;
				}
				((Control)_posBox).set_Left(10);
				((Control)_posBox).set_Top(10);
				((Control)_posBox).set_Width(((Container)this).get_ContentRegion().Width - 20);
				((Control)_radiusBox).set_Top(((Control)_posBox).get_Bottom() + 10);
				StandardButton radius5mButton = _radius5mButton;
				StandardButton radius300inButton = _radius300inButton;
				int num;
				((Control)_radius15mButton).set_Top(num = ((Control)_radiusBox).get_Top() + ((Control)_radiusBox).get_Height() / 2 - ((Control)_radius5mButton).get_Height() / 2);
				int top;
				((Control)radius300inButton).set_Top(top = num);
				((Control)radius5mButton).set_Top(top);
				StandardButton radius5mButton2 = _radius5mButton;
				StandardButton radius300inButton2 = _radius300inButton;
				((Control)_radius15mButton).set_Width(num = 50);
				((Control)radius300inButton2).set_Width(top = num);
				((Control)radius5mButton2).set_Width(top);
				((Control)_radius15mButton).set_Right(((Container)this).get_ContentRegion().Width - 10);
				((Control)_radius300inButton).set_Right(((Control)_radius15mButton).get_Left() - 10);
				((Control)_radius5mButton).set_Right(((Control)_radius300inButton).get_Left() - 10);
				((Control)_radiusBox).set_Right(((Control)_radius5mButton).get_Left() - 10);
				((Control)_radiusBox).set_Width(((Control)_radiusBox).get_Right() - 10);
				((Control)_typeSelect).set_Left(10);
				((Control)_typeSelect).set_Top(((Control)_radius5mButton).get_Bottom() + 10);
				((Control)_typeSelect).set_Width(((Container)this).get_ContentRegion().Width - 20);
				StandardButton snapXYZButton = _snapXYZButton;
				StandardButton snapXYButton = _snapXYButton;
				((Control)_removeButton).set_Top(num = ((Control)_typeSelect).get_Bottom() + 10);
				((Control)snapXYButton).set_Top(top = num);
				((Control)snapXYZButton).set_Top(top);
				StandardButton prevSwapButton = _prevSwapButton;
				((Control)_nextSwapButton).set_Top(top = ((Control)_snapXYZButton).get_Bottom() + 10);
				((Control)prevSwapButton).set_Top(top);
				((Container)(object)this).SetContentRegionHeight(((Control)_nextSwapButton).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			_editor.PointSelected -= PointSelected;
			_editor.PointModified -= PointModified;
			_editor.PointRemoved -= PointRemoved;
			_editor.PointInserted -= PointInserted;
			_editor.PointSwapped -= PointSwapped;
			((Panel)this).DisposeControl();
		}
	}
}
