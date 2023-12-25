using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Ideka.RacingMeter.Lib;
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

		private readonly StandardButton _snapButton;

		public RacePointEditPanel(RaceEditor editor)
			: this()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
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
			StandardButton val7 = new StandardButton();
			((Control)val7).set_Parent((Container)(object)this);
			val7.set_Text("Snap");
			((Control)val7).set_Visible(false);
			_snapButton = val7;
			((Control)_snapButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				_editor.SnapPoint();
			});
			UpdateLayout();
			_posBox.TempValue += new Action<Vector3>(_editor.MovePointPreview);
			_posBox.ValueCommitted += new Action<Vector3>(_editor.MovePoint);
			_radiusBox.TempValue += new Action<float>(_editor.ResizePointPreview);
			_radiusBox.TempClear += new Action(_editor.ResizePointPreview);
			_radiusBox.ValueCommitted += new Action<float>(_editor.ResizePoint);
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
			_typeSelect.ValueCommitted += new Action<RacePointType>(_editor.SetPointType);
			((Control)_snapXYZButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				_editor.MovePoint(_editor.Measurer.Pos.Meters);
			});
			((Control)_snapXYButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				Vector3 meters = _editor.Measurer.Pos.Meters;
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
			_editor.PointSelected += new Action<RacePoint>(PointSelected);
			_editor.PointModified += new Action<RacePoint>(PointModified);
			_editor.PointRemoved += new Action<RacePoint>(PointRemoved);
			_editor.PointInserted += new Action<RacePoint>(PointInserted);
			_editor.PointSwapped += new Action<RacePoint, bool>(PointSwapped);
			UpdateValues();
		}

		private void UpdateValues()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			RacePoint selected = _editor.Selected;
			if (selected == null)
			{
				((Control)this).set_Visible(false);
				return;
			}
			((Control)this).set_Visible(true);
			UpdateTitle();
			_posBox.Value = selected.Position;
			_radiusBox.Value = selected.Radius;
			_typeSelect.Value = selected.Type;
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
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			if (_editor != null)
			{
				((Control)_snapXYZButton).set_Left(10);
				((Control)(object)_snapXYZButton).ArrangeLeftRight(10, (Control)_snapXYButton, (Control)_removeButton);
				((Control)_prevSwapButton).set_Left(10);
				((Control)(object)_prevSwapButton).ArrangeLeftRight(10, (Control)_nextSwapButton, (Control)_snapButton);
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
				StandardButton nextSwapButton = _nextSwapButton;
				((Control)_snapButton).set_Top(num = ((Control)_snapXYZButton).get_Bottom() + 10);
				((Control)nextSwapButton).set_Top(top = num);
				((Control)prevSwapButton).set_Top(top);
				((Container)(object)this).SetContentRegionHeight(((Control)_nextSwapButton).get_Bottom() + 10);
			}
		}

		protected override void DisposeControl()
		{
			_editor.PointSelected -= new Action<RacePoint>(PointSelected);
			_editor.PointModified -= new Action<RacePoint>(PointModified);
			_editor.PointRemoved -= new Action<RacePoint>(PointRemoved);
			_editor.PointInserted -= new Action<RacePoint>(PointInserted);
			_editor.PointSwapped -= new Action<RacePoint, bool>(PointSwapped);
			((Panel)this).DisposeControl();
		}
	}
}
