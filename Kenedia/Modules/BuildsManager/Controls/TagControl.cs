using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TagControl : Control
	{
		private AsyncTexture2D _editIcon = AsyncTexture2D.FromAssetId(157109);

		private TemplateTag _tag;

		private string _displayText = string.Empty;

		private Rectangle _bounds;

		private Rectangle _iconBounds;

		private Rectangle _editIconBounds;

		private Rectangle _editIconTextureRegion;

		private Rectangle _textBounds;

		private BitmapFont _font = Control.Content.DefaultFont14;

		public TemplateTag Tag
		{
			get
			{
				return _tag;
			}
			set
			{
				Common.SetProperty(ref _tag, value, new ValueChangedEventHandler<TemplateTag>(OnTagChanged));
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
				Common.SetProperty(ref _font, value, new ValueChangedEventHandler<BitmapFont>(OnFontChanged));
			}
		}

		public bool Selected { get; set; }

		public Action<bool> OnClicked { get; set; }

		public Color HoverColor { get; set; } = Color.get_White() * 0.2f;


		public Color DisabledColor { get; set; } = Color.get_Transparent();


		public Color ActiveColor { get; set; } = Color.get_Lime() * 0.2f;


		public int FontPadding { get; set; } = 4;


		public Action OnEditClicked { get; set; }

		public TagControl()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			base.Height = Font.get_LineHeight() + FontPadding * 2;
		}

		private void OnFontChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<BitmapFont> e)
		{
			if (_font == null)
			{
				_font = Control.Content.DefaultFont14;
			}
			base.Height = Font.get_LineHeight() + FontPadding * 2;
			ApplyTag();
		}

		private void OnTagChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<TemplateTag> e)
		{
			if (e.NewValue != null)
			{
				e.NewValue!.PropertyChanged += new PropertyChangedEventHandler(NewValue_TemplateChanged);
				ApplyTag();
			}
		}

		private void NewValue_TemplateChanged(object sender, PropertyChangedEventArgs e)
		{
			TemplateTag t = sender as TemplateTag;
			if (t != null)
			{
				ApplyTag(t);
			}
		}

		private void ApplyTag(TemplateTag tag = null)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			int height = Font.get_LineHeight();
			_iconBounds = new Rectangle(FontPadding, FontPadding, height, height);
			_editIconBounds = new Rectangle(base.Width - height - FontPadding, FontPadding, height, height);
			_editIconTextureRegion = new Rectangle(2, 2, 28, 28);
			_textBounds = new Rectangle(((Rectangle)(ref _iconBounds)).get_Right() + 5, ((Rectangle)(ref _iconBounds)).get_Top(), base.Width - _iconBounds.Width - 5 - _editIconBounds.Width - 5, height);
			_displayText = UI.GetDisplayText(Font, Tag?.Name ?? string.Empty, _textBounds.Width);
			base.BasicTooltipText = Tag?.Name ?? string.Empty;
			ActiveColor = Color.get_Lime() * 0.2f;
			HoverColor = Color.get_Lime() * 0.3f;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			base.Height = Font.get_LineHeight() + FontPadding * 2;
			ApplyTag();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, base.MouseOver ? HoverColor : (Selected ? ActiveColor : DisabledColor));
			AsyncTexture2D texture = Tag?.Icon?.Texture;
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl(this, texture, _iconBounds, Tag.Icon.TextureRegion, Color.get_White());
			}
			if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, _editIcon, _editIconBounds, _editIconTextureRegion, Color.get_White());
			}
			spriteBatch.DrawStringOnCtrl(this, _displayText, Font, _textBounds, Color.get_White());
		}

		public void SetSelected(bool selected)
		{
			Selected = selected;
			OnClicked?.Invoke(Selected);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			base.OnClick(e);
			if (((Rectangle)(ref _editIconBounds)).Contains(base.RelativeMousePosition))
			{
				OnEditClicked?.Invoke();
				return;
			}
			Selected = !Selected;
			OnClicked?.Invoke(Selected);
		}
	}
}
