using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class TraitTooltip : Tooltip, ILocalizable
	{
		private readonly DetailedTexture _image = new DetailedTexture
		{
			TextureRegion = new Rectangle(14, 14, 100, 100)
		};

		private readonly Kenedia.Modules.Core.Controls.Label _title;

		private readonly Kenedia.Modules.Core.Controls.Label _id;

		private readonly Kenedia.Modules.Core.Controls.Label _description;

		public Trait Trait
		{
			[CompilerGenerated]
			get
			{
				return _003CTrait_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CTrait_003Ek__BackingField, value, delegate(Trait v)
				{
					_003CTrait_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Trait>(ApplyTrait));
			}
		}

		public Func<string> SetLocalizedTooltip { get; set; }

		public TraitTooltip()
		{
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.AutoSizePadding = new Point(5);
			Rectangle imageBounds = (_image.Bounds = new Rectangle(4, 4, 48, 48));
			_title = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont16.LineHeight,
				AutoSizeWidth = true,
				Location = new Point(imageBounds.Right, imageBounds.Top),
				Font = Control.Content.DefaultFont16
			};
			_id = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont12.LineHeight,
				AutoSizeWidth = true,
				Location = new Point(imageBounds.Right, _title.Bottom),
				Font = Control.Content.DefaultFont12,
				TextColor = Color.White * 0.8f
			};
			_description = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Width = 300,
				AutoSizeHeight = true,
				Location = new Point(imageBounds.Left, imageBounds.Bottom + 10),
				Font = Control.Content.DefaultFont14,
				WrapText = true
			};
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		private void ApplyTrait(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Trait> e)
		{
			_title.TextColor = ContentService.Colors.Chardonnay;
			_title.Text = Trait?.Name;
			_id.Text = $"{strings.TraitId}: {Trait?.Id}";
			_description.Text = Trait?.Description.InterpretItemDescription() ?? strings.MissingInfoFromAPI;
			if (Trait != null)
			{
				_image.Texture = TexturesService.GetAsyncTexture(Trait.IconAssetId);
				if (_image.Texture != null)
				{
					int padding = _image.Texture.Width / 16;
					_image.TextureRegion = new Rectangle(padding, padding, _image.Texture.Width - padding * 2, _image.Texture.Height - padding * 2);
				}
			}
		}

		public void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			ApplyTrait(this, null);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			if (Trait != null)
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintBeforeChildren(spriteBatch, bounds);
			_image.Draw(this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			Trait = null;
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			_image?.Dispose();
			base.DisposeControl();
		}
	}
}
