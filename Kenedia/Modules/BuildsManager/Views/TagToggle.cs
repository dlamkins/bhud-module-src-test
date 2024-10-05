using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Views
{
	public class TagToggle : Control
	{
		public static int TagHeight = 25;

		private AsyncTexture2D _textureEnabled;

		private AsyncTexture2D _textureDisabled;

		private Func<string> _setLocalizedTooltip;

		private bool _selected;

		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				Common.SetProperty(ref _selected, value, new ValueChangedEventHandler<bool>(OnSelected));
			}
		}

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				if (value != null)
				{
					base.BasicTooltipText = value?.Invoke();
				}
			}
		}

		public TemplateTag Tag { get; private set; }

		public Action<TemplateTag> OnSelectedChanged { get; internal set; }

		public TagToggle(TemplateTag tag)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(TagHeight);
			Tag = tag;
			Tag.PropertyChanged += new PropertyChangedEventHandler(Tag_PropertyChanged);
			Tag.Icon.Texture.TextureSwapped += Texture_TextureSwapped;
			base.BasicTooltipText = Tag.Name;
			_textureEnabled = Tag.Icon.Texture;
			_textureDisabled = (AsyncTexture2D)Tag.Icon.Texture.Texture.ToGrayScaledPalettable();
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		private void Texture_TextureSwapped(object sender, Blish_HUD.ValueChangedEventArgs<Texture2D> e)
		{
			_textureEnabled = Tag.Icon.Texture;
			_textureDisabled = (AsyncTexture2D)Tag.Icon.Texture.Texture.ToGrayScaledPalettable();
		}

		public void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		private void Tag_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (!(propertyName == "Name"))
			{
				if (propertyName == "AssetId")
				{
					_textureEnabled = Tag.Icon.Texture;
					_textureDisabled = (AsyncTexture2D)Tag.Icon.Texture.Texture.ToGrayScaledPalettable();
				}
			}
			else
			{
				base.BasicTooltipText = Tag.Name;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D texture = (Selected ? _textureEnabled : _textureDisabled);
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl((Control)this, (Texture2D)texture, bounds, Selected ? Color.get_White() : (Color.get_Gray() * 0.5f));
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			Selected = !Selected;
		}

		private void OnSelected(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<bool> e)
		{
			if (OnSelectedChanged != null)
			{
				OnSelectedChanged(Tag);
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}
	}
}
