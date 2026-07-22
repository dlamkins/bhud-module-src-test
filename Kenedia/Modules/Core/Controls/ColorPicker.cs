using System;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Structs;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class ColorPicker : Panel
	{
		private readonly Panel _color_Preview;

		private readonly NumberBox _red_box;

		private readonly NumberBox _green_box;

		private readonly NumberBox _blue_box;

		private readonly NumberBox _alpha_box;

		private int _r;

		private int _g;

		private int _b;

		private int _a;

		private Color _selected_Color;

		public Action<Color> OnColorChangedAction { get; set; }

		public int R
		{
			get
			{
				return _r;
			}
			set
			{
				if (value >= 0 && value <= 255)
				{
					Common.SetProperty(ref _r, value, new ValueChangedEventHandler<int>(SetColor));
				}
			}
		}

		public int G
		{
			get
			{
				return _g;
			}
			set
			{
				if (value >= 0 && value <= 255)
				{
					Common.SetProperty(ref _g, value, new ValueChangedEventHandler<int>(SetColor));
				}
			}
		}

		public int B
		{
			get
			{
				return _b;
			}
			set
			{
				if (value >= 0 && value <= 255)
				{
					Common.SetProperty(ref _b, value, new ValueChangedEventHandler<int>(SetColor));
				}
			}
		}

		public int A
		{
			get
			{
				return _a;
			}
			set
			{
				if (value >= 0 && value <= 255)
				{
					Common.SetProperty(ref _a, value, new ValueChangedEventHandler<int>(SetColor));
				}
			}
		}

		public Color SelectedColor
		{
			get
			{
				return _selected_Color;
			}
			set
			{
				Common.SetProperty(ref _selected_Color, value, new ValueChangedEventHandler<Color>(ApplyColor));
			}
		}

		public Color MultipliedSelectedColor => Color.FromNonPremultiplied(R, G, B, A);

		public event ValueChangedEventHandler<Color> ColorChanged;

		public ColorPicker()
		{
			_color_Preview = new Panel
			{
				Parent = this,
				Location = new Point(0, 0),
				Size = new Point(20),
				BorderWidth = new RectangleDimensions(2),
				BorderColor = Color.Black,
				BackgroundColor = Color.FromNonPremultiplied(R, G, B, A)
			};
			_red_box = new NumberBox
			{
				Parent = this,
				Location = new Point(_color_Preview.Right + 5, 0),
				Value = R,
				MinValue = 0,
				MaxValue = 255,
				ShowButtons = false,
				ValueChangedAction = delegate(int v)
				{
					R = v;
				}
			};
			_green_box = new NumberBox
			{
				Parent = this,
				Location = new Point(_red_box.Right + 5, 0),
				Value = G,
				MinValue = 0,
				MaxValue = 255,
				ShowButtons = false,
				ValueChangedAction = delegate(int v)
				{
					G = v;
				}
			};
			_blue_box = new NumberBox
			{
				Parent = this,
				Location = new Point(_green_box.Right + 5, 0),
				Value = B,
				MinValue = 0,
				MaxValue = 255,
				ShowButtons = false,
				ValueChangedAction = delegate(int v)
				{
					B = v;
				}
			};
			_alpha_box = new NumberBox
			{
				Parent = this,
				Location = new Point(_blue_box.Right + 5, 0),
				Value = A,
				MinValue = 0,
				MaxValue = 255,
				ShowButtons = false,
				ValueChangedAction = delegate(int v)
				{
					A = v;
				}
			};
		}

		private void ApplyColor(object sender, ValueChangedEventArgs<Color> e)
		{
			Color col = e.NewValue;
			_r = col.R;
			_g = col.G;
			_b = col.B;
			_a = col.A;
			_selected_Color = new Color(R, G, B, A);
			ApplyColorsToControls();
		}

		private void ApplyColorsToControls()
		{
			_red_box.Value = R;
			_blue_box.Value = B;
			_green_box.Value = G;
			_alpha_box.Value = A;
			_color_Preview.BackgroundColor = Color.FromNonPremultiplied(R, G, B, A);
		}

		private void SetColor(object sender, ValueChangedEventArgs<int> e)
		{
			Color old_color = _selected_Color;
			_selected_Color = new Color(R, G, B, A);
			ApplyColorsToControls();
			this.ColorChanged?.Invoke(this, new ValueChangedEventArgs<Color>(old_color, SelectedColor));
			OnColorChangedAction?.Invoke(SelectedColor);
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			int padding = 5;
			int preview_width = 20;
			int number_of_boxes = 4;
			int input_width = (base.ContentRegion.Width - preview_width - padding - padding * (number_of_boxes - 1)) / number_of_boxes;
			_color_Preview?.SetLocation(0, 0);
			_color_Preview?.SetSize(preview_width, preview_width);
			int red_x = (_color_Preview?.Right ?? 0) + padding;
			_red_box?.SetLocation(red_x, 0);
			_red_box?.SetSize(input_width);
			int green_x = (_red_box?.Right ?? 0) + padding;
			_green_box?.SetLocation(green_x, 0);
			_green_box?.SetSize(input_width);
			int blue_x = (_green_box?.Right ?? 0) + padding;
			_blue_box?.SetLocation(blue_x, 0);
			_blue_box?.SetSize(input_width);
			int alpha_x = (_blue_box?.Right ?? 0) + padding;
			_alpha_box?.SetLocation(alpha_x, 0);
			_alpha_box?.SetSize(input_width);
		}
	}
}
