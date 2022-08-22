using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TTBlockersStuff.Language;

namespace Lorf.BH.TTBlockersStuff.Settings
{
	internal class ColorPickerWindow : Container, IWindow
	{
		private int textureMarginLeft;

		private int textureMarginTop;

		private int textureMarginRight;

		private int textureMarginBottom;

		private Gw2Sharp.WebApi.V2.Models.Color originalColor;

		private static readonly Texture2D _textureWindowTexture = GameService.Content.GetTexture("controls/window/502049");

		private readonly Microsoft.Xna.Framework.Rectangle _normalizedWindowRegion = new Microsoft.Xna.Framework.Rectangle(0, 0, 400, 400);

		private readonly string pickName;

		private readonly ColorBox colorBox;

		private ColorPicker picker;

		private StandardButton _acceptBttn;

		private StandardButton _cancelBttn;

		private Microsoft.Xna.Framework.Rectangle _windowRegion;

		public bool TopMost => false;

		public double LastInteraction => double.MaxValue;

		public bool CanClose => false;

		public bool CanCloseWithEscape => true;

		public event EventHandler<EventArgs> AssignmentAccepted;

		public event EventHandler<EventArgs> AssignmentCanceled;

		private void OnAssignmentAccepted(EventArgs e)
		{
			this.AssignmentAccepted?.Invoke(this, e);
			Dispose();
		}

		private void OnAssignmentCanceled(EventArgs e)
		{
			colorBox.Color = originalColor;
			this.AssignmentCanceled?.Invoke(this, e);
			Dispose();
		}

		public ColorPickerWindow(string name, ColorBox box)
		{
			pickName = name;
			colorBox = box;
			originalColor = colorBox.Color;
			base.BackgroundColor = Microsoft.Xna.Framework.Color.Black * 0.3f;
			base.Size = new Point(_normalizedWindowRegion.Width, _normalizedWindowRegion.Height);
			ZIndex = 2147483614;
			base.Visible = false;
			BuildChildElements();
		}

		protected override void OnShown(EventArgs e)
		{
			Invalidate();
			base.OnShown(e);
		}

		private int doABitOfMath(float size, bool height)
		{
			float originalSize = (height ? _textureWindowTexture.Height : _textureWindowTexture.Width);
			float newSize = (height ? base.Height : base.Width);
			return (int)(size * (newSize / originalSize));
		}

		private void BuildChildElements()
		{
			textureMarginLeft = doABitOfMath(36f, height: false);
			textureMarginTop = doABitOfMath(26f, height: true);
			textureMarginRight = doABitOfMath(72f, height: false);
			textureMarginBottom = doABitOfMath(35f, height: true);
			Label assignInputsLbl = new Label
			{
				Text = pickName,
				Location = new Point(textureMarginLeft + 18, textureMarginTop + 18),
				ShowShadow = true,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Parent = this,
				Font = GameService.Content.DefaultFont16
			};
			_cancelBttn = new StandardButton
			{
				Text = Translations.ColorSelectionButtonTextCancel,
				Location = new Point(base.Width - textureMarginRight - 100 - 18, base.Height - textureMarginBottom - 18 - 25),
				Width = 100,
				Height = 25,
				Parent = this
			};
			_acceptBttn = new StandardButton
			{
				Text = Translations.ColorSelectionButtonTextOk,
				Width = 100,
				Height = 25,
				Parent = this
			};
			_acceptBttn.Location = new Point(_cancelBttn.Left - 8 - _acceptBttn.Width, _cancelBttn.Top);
			_cancelBttn.Click += delegate
			{
				OnAssignmentCanceled(EventArgs.Empty);
			};
			_acceptBttn.Click += delegate
			{
				OnAssignmentAccepted(EventArgs.Empty);
			};
			picker = new ColorPicker();
			picker.Visible = true;
			picker.Parent = this;
			picker.Size = new Point(base.Width - assignInputsLbl.Left - 36, _cancelBttn.Top - assignInputsLbl.Bottom - 36);
			picker.CanScroll = true;
			picker.Location = new Point(assignInputsLbl.Left, assignInputsLbl.Bottom + 18);
			picker.AssociatedColorBox = colorBox;
			foreach (Gw2Sharp.WebApi.V2.Models.Color color in Module.Instance.Colors)
			{
				picker.Colors.Add(color);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			Container parent = base.Parent;
			if (parent != null)
			{
				_size = parent.Size;
				_windowRegion = RectangleExtension.OffsetBy(r2: new Point(_size.X / 2 - _normalizedWindowRegion.Width / 2, _size.Y / 2 - _normalizedWindowRegion.Height / 2), r1: _normalizedWindowRegion);
				base.ContentRegion = _windowRegion;
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Rectangle bounds)
		{
			spriteBatch.DrawOnCtrl(this, _textureWindowTexture, _windowRegion);
		}

		public void BringWindowToFront()
		{
		}
	}
}
