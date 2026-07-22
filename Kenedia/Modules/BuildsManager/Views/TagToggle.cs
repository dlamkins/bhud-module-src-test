using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

		public bool Selected
		{
			[CompilerGenerated]
			get
			{
				return _003CSelected_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(ref _003CSelected_003Ek__BackingField, value, new ValueChangedEventHandler<bool>(OnSelected));
			}
		}

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
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
			base.Size = new Point(TagHeight);
			if (tag == null)
			{
				Logger.GetLogger(typeof(BuildsManager)).Error("TagToggle created with null tag.");
				throw new ArgumentNullException("tag");
			}
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
			AsyncTexture2D texture = (Selected ? _textureEnabled : _textureDisabled);
			if (texture != null)
			{
				spriteBatch.DrawOnCtrl(this, texture, bounds, Tag.TextureRegion, Selected ? Color.White : (Color.Gray * 0.5f));
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
