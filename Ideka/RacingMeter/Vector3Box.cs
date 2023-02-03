using System;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;

namespace Ideka.RacingMeter
{
	public class Vector3Box : Container
	{
		public const int InnerSpacing = 10;

		public const int BoxWidth = 90;

		private int _spacing;

		private readonly FloatBox _x;

		private readonly FloatBox _y;

		private readonly FloatBox _z;

		public Vector3 Value
		{
			get
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				return new Vector3(_x.Value, _y.Value, _z.Value);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				_x.Value = value.X;
				_y.Value = value.Y;
				_z.Value = value.Z;
			}
		}

		public int Spacing
		{
			get
			{
				return _spacing;
			}
			set
			{
				_spacing = value;
				UpdateLayout();
			}
		}

		public float XScale
		{
			set
			{
				FloatBox x = _x;
				FloatBox y = _y;
				float num2 = (_z.XScale = value);
				float num5 = (x.XScale = (y.XScale = num2));
			}
		}

		public float YScale
		{
			set
			{
				FloatBox x = _x;
				FloatBox y = _y;
				float num2 = (_z.YScale = value);
				float num5 = (x.YScale = (y.YScale = num2));
			}
		}

		public float Scale
		{
			set
			{
				FloatBox x = _x;
				FloatBox y = _y;
				float num2 = (_z.Scale = value);
				float num5 = (x.Scale = (y.Scale = num2));
			}
		}

		public event Action<Vector3>? ValueCommitted;

		public event Action<Vector3>? TempValue;

		private static string Labeler(string x)
		{
			return "[ " + x + " ]";
		}

		public Vector3Box()
			: this()
		{
			FloatBox floatBox = new FloatBox();
			((Control)floatBox).set_Parent((Container)(object)this);
			floatBox.Label = Labeler("X");
			floatBox.Spacing = 10;
			floatBox.ControlWidth = 90;
			_x = floatBox;
			FloatBox floatBox2 = new FloatBox();
			((Control)floatBox2).set_Parent((Container)(object)this);
			floatBox2.Label = Labeler("Y");
			floatBox2.Spacing = 10;
			floatBox2.ControlWidth = 90;
			_y = floatBox2;
			FloatBox floatBox3 = new FloatBox();
			((Control)floatBox3).set_Parent((Container)(object)this);
			floatBox3.Label = Labeler("Z");
			floatBox3.Spacing = 10;
			floatBox3.ControlWidth = 90;
			_z = floatBox3;
			_x.TempValue += delegate(float v)
			{
				temp(v);
				void temp(float? x = null, float? y = null, float? z = null)
				{
					//IL_0068: Unknown result type (might be due to invalid IL or missing references)
					this.TempValue?.Invoke(new Vector3(x ?? _x.Value, y ?? _y.Value, z ?? _z.Value));
				}
			};
			_y.TempValue += delegate(float v)
			{
				float? y2 = v;
				Vector3Box._003C_002Ector_003Eg__temp_007C25_0((float?)null, y2, (float?)null);
			};
			_z.TempValue += delegate(float v)
			{
				float? z2 = v;
				Vector3Box._003C_002Ector_003Eg__temp_007C25_0((float?)null, (float?)null, z2);
			};
			_x.ValueCommitted += delegate
			{
				value();
				void value()
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					this.ValueCommitted?.Invoke(Value);
				}
			};
			_y.ValueCommitted += delegate
			{
				Vector3Box._003C_002Ector_003Eg__value_007C25_4();
			};
			_z.ValueCommitted += delegate
			{
				Vector3Box._003C_002Ector_003Eg__value_007C25_4();
			};
			Spacing = 20;
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			((Container)this).OnResized(e);
			UpdateLayout();
		}

		private void UpdateLayout()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (_x != null)
			{
				FloatBox x = _x;
				FloatBox y = _y;
				int num;
				((Control)_z).set_Width(num = (int)((float)(((Container)this).get_ContentRegion().Width - Spacing * 2) / 3f));
				int width;
				((Control)y).set_Width(width = num);
				((Control)x).set_Width(width);
				((Control)_x).set_Left(0);
				((Control)_y).set_Left(((Control)_x).get_Right() + Spacing);
				((Control)_z).set_Left(((Control)_y).get_Right() + Spacing);
				((Container)(object)this).SetContentRegionHeight(((Control)_z).get_Bottom());
			}
		}
	}
}
