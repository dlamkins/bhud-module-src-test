using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls
{
	public class ListEntry<T> : Control
	{
		private HorizontalAlignment _alignment = (HorizontalAlignment)1;

		private bool _dragDrop;

		private bool _dragging;

		private BitmapFont _font = GameService.Content.get_DefaultFont16();

		private AsyncTexture2D _icon;

		private float _iconMaxHeight = 32f;

		private float _iconMaxWidth = 32f;

		private float _iconMinHeight = 32f;

		private float _iconMinWidth = 32f;

		private string _text;

		private Color _textColor = Color.get_Black();

		public bool DragDrop
		{
			get
			{
				return _dragDrop;
			}
			set
			{
				((Control)this).SetProperty<bool>(ref _dragDrop, value, true, "DragDrop");
			}
		}

		public bool Dragging
		{
			get
			{
				return _dragging;
			}
			internal set
			{
				((Control)this).SetProperty<bool>(ref _dragging, value, true, "Dragging");
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				((Control)this).SetProperty<string>(ref _text, value, true, "Text");
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref _font, value, true, "Font");
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _textColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _textColor, value, true, "TextColor");
			}
		}

		public AsyncTexture2D Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				((Control)this).SetProperty<AsyncTexture2D>(ref _icon, value, true, "Icon");
			}
		}

		public float IconMinWidth
		{
			get
			{
				return _iconMinWidth;
			}
			set
			{
				if (value > _iconMaxWidth)
				{
					_iconMaxWidth = value;
				}
				((Control)this).SetProperty<float>(ref _iconMinWidth, value, true, "IconMinWidth");
			}
		}

		public float IconMaxWidth
		{
			get
			{
				return _iconMaxWidth;
			}
			set
			{
				if (value < _iconMinWidth)
				{
					_iconMinWidth = value;
				}
				((Control)this).SetProperty<float>(ref _iconMaxWidth, value, true, "IconMaxWidth");
			}
		}

		public float IconMinHeight
		{
			get
			{
				return _iconMinHeight;
			}
			set
			{
				if (value > _iconMaxHeight)
				{
					_iconMaxHeight = value;
				}
				((Control)this).SetProperty<float>(ref _iconMinHeight, value, true, "IconMinHeight");
			}
		}

		public float IconMaxHeight
		{
			get
			{
				return _iconMaxHeight;
			}
			set
			{
				if (value < _iconMinHeight)
				{
					_iconMinHeight = value;
				}
				((Control)this).SetProperty<float>(ref _iconMaxHeight, value, true, "IconMaxHeight");
			}
		}

		private float IconWidth
		{
			get
			{
				if (Icon != null)
				{
					return MathHelper.Clamp((float)Icon.get_Width(), IconMinWidth, IconMaxWidth);
				}
				return 0f;
			}
		}

		private float IconHeight
		{
			get
			{
				if (Icon != null)
				{
					return MathHelper.Clamp((float)Icon.get_Height(), IconMinHeight, IconMaxHeight);
				}
				return 0f;
			}
		}

		public HorizontalAlignment Alignment
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _alignment;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<HorizontalAlignment>(ref _alignment, value, true, "Alignment");
			}
		}

		private float IconRightPadding => 20f;

		private RectangleF IconBounds
		{
			get
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Expected I4, but got Unknown
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				float textWidth = Font.MeasureString(Text).Width;
				HorizontalAlignment alignment = Alignment;
				return (RectangleF)((int)alignment switch
				{
					0 => new RectangleF(0f, 0f, IconWidth, IconHeight), 
					1 => new RectangleF((float)(((Control)this).get_Size().X / 2) - textWidth / 2f - IconWidth / 2f - IconRightPadding / 2f, 0f, IconWidth, IconHeight), 
					2 => new RectangleF((float)((Control)this).get_Size().X - textWidth - IconWidth - IconRightPadding, 0f, IconWidth, IconHeight), 
					_ => throw new InvalidOperationException($"Alignment \"{Alignment}\" is not supported."), 
				});
			}
		}

		private RectangleF TextBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				RectangleF iconBounds = IconBounds;
				return new RectangleF(((RectangleF)(ref iconBounds)).get_Right() + IconRightPadding, 0f, (float)((Control)this).get_Size().X - IconBounds.Width, (float)((Control)this).get_Size().Y);
			}
		}

		public T Data { get; set; }

		public ListEntry(string title)
			: this()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Text = title;
		}

		public ListEntry(string title, BitmapFont font)
			: this(title)
		{
			Font = font;
		}

		public override void RecalculateLayout()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			int height = (int)Math.Max(IconBounds.Height, TextBounds.Height);
			height = Math.Max(((Control)this).get_Size().Y, height);
			((Control)this).set_Size(new Point(((Control)this).get_Size().X, height));
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			if (Icon != null && Icon.get_HasSwapped())
			{
				spriteBatch.DrawOnCtrl((Control)(object)this, AsyncTexture2D.op_Implicit(Icon), IconBounds);
			}
			if (!string.IsNullOrWhiteSpace(Text))
			{
				spriteBatch.DrawStringOnCtrl((Control)(object)this, Text, Font, TextBounds, TextColor, wrap: false, (HorizontalAlignment)0, (VerticalAlignment)1);
			}
		}
	}
}
