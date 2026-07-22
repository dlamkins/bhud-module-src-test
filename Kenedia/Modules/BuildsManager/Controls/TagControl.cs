using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

		private string _displayText = string.Empty;

		private Rectangle _bounds;

		private Rectangle _iconBounds;

		private Rectangle _editIconBounds;

		private Rectangle _editIconTextureRegion;

		private Rectangle _textBounds;

		public TemplateTag Tag
		{
			[CompilerGenerated]
			get
			{
				return _003CTag_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTag_003Ek__BackingField, value, delegate(TemplateTag v)
				{
					_003CTag_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<TemplateTag>(OnTagChanged));
			}
		}

		public BitmapFont Font
		{
			[CompilerGenerated]
			get
			{
				return _003CFont_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CFont_003Ek__BackingField, value, delegate(BitmapFont v)
				{
					_003CFont_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<BitmapFont>(OnFontChanged));
			}
		}

		public bool Selected { get; set; }

		public Action<bool> OnClicked { get; set; }

		public Color HoverColor { get; set; }

		public Color DisabledColor { get; set; }

		public Color ActiveColor { get; set; }

		public int FontPadding { get; set; }

		public Action OnEditClicked { get; set; }

		public TagControl()
		{
			_003CFont_003Ek__BackingField = Control.Content.DefaultFont14;
			HoverColor = Color.White * 0.2f;
			DisabledColor = Color.Transparent;
			ActiveColor = Color.Lime * 0.2f;
			FontPadding = 4;
			base._002Ector();
			base.Height = Font.LineHeight + FontPadding * 2;
		}

		private void OnFontChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<BitmapFont> e)
		{
			if (Font == null)
			{
				BitmapFont bitmapFont = (Font = Control.Content.DefaultFont14);
			}
			base.Height = Font.LineHeight + FontPadding * 2;
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
			int height = Font.LineHeight;
			_iconBounds = new Rectangle(FontPadding, FontPadding, height, height);
			_editIconBounds = new Rectangle(base.Width - height - FontPadding, FontPadding, height, height);
			_editIconTextureRegion = new Rectangle(2, 2, 28, 28);
			_textBounds = new Rectangle(_iconBounds.Right + 5, _iconBounds.Top, base.Width - _iconBounds.Width - 5 - _editIconBounds.Width - 5, height);
			_displayText = UI.GetDisplayText(Font, Tag?.Name ?? string.Empty, _textBounds.Width);
			base.BasicTooltipText = Tag?.Name ?? string.Empty;
			ActiveColor = Color.Lime * 0.2f;
			HoverColor = Color.Lime * 0.3f;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			base.Height = Font.LineHeight + FontPadding * 2;
			ApplyTag();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			if (base.Enabled)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, base.MouseOver ? HoverColor : (Selected ? ActiveColor : DisabledColor));
			}
			AsyncTexture2D texture = Tag?.Icon?.Texture;
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl(this, texture, _iconBounds, Tag.TextureRegion, Color.White);
			}
			if (base.MouseOver && base.Enabled)
			{
				spriteBatch.DrawOnCtrl(this, _editIcon, _editIconBounds, _editIconTextureRegion, Color.White);
			}
			spriteBatch.DrawStringOnCtrl(this, _displayText, Font, _textBounds, Color.White);
		}

		public void SetSelected(bool selected)
		{
			Selected = selected;
			OnClicked?.Invoke(Selected);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (_editIconBounds.Contains(base.RelativeMousePosition))
			{
				OnEditClicked?.Invoke();
				return;
			}
			Selected = !Selected;
			OnClicked?.Invoke(Selected);
		}
	}
}
