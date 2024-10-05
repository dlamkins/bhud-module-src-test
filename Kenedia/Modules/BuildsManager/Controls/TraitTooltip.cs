using System;
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

		private Trait _trait;

		public Trait Trait
		{
			get
			{
				return _trait;
			}
			set
			{
				Common.SetProperty(ref _trait, value, new ValueChangedEventHandler<Trait>(ApplyTrait));
			}
		}

		public Func<string> SetLocalizedTooltip { get; set; }

		public TraitTooltip()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			base.AutoSizePadding = new Point(5);
			DetailedTexture image = _image;
			Rectangle imageBounds = default(Rectangle);
			((Rectangle)(ref imageBounds))._002Ector(4, 4, 48, 48);
			image.Bounds = imageBounds;
			_title = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont16.get_LineHeight(),
				AutoSizeWidth = true,
				Location = new Point(((Rectangle)(ref imageBounds)).get_Right(), ((Rectangle)(ref imageBounds)).get_Top()),
				Font = Control.Content.DefaultFont16
			};
			_id = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Height = Control.Content.DefaultFont12.get_LineHeight(),
				AutoSizeWidth = true,
				Location = new Point(((Rectangle)(ref imageBounds)).get_Right(), _title.Bottom),
				Font = Control.Content.DefaultFont12,
				TextColor = Color.get_White() * 0.8f
			};
			_description = new Kenedia.Modules.Core.Controls.Label
			{
				Parent = this,
				Width = 300,
				AutoSizeHeight = true,
				Location = new Point(((Rectangle)(ref imageBounds)).get_Left(), ((Rectangle)(ref imageBounds)).get_Bottom() + 10),
				Font = Control.Content.DefaultFont14,
				WrapText = true
			};
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		private void ApplyTrait(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Trait> e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			_title.TextColor = ContentService.Colors.Chardonnay;
			_title.Text = Trait?.Name;
			_id.Text = $"{strings.TraitId}: {Trait?.Id}";
			_description.Text = Trait?.Description.InterpretItemDescription() ?? strings.MissingInfoFromAPI;
			if (Trait != null)
			{
				_image.Texture = Trait.Icon;
				int padding = (Trait.Icon?.Width ?? 0) / 16;
				_image.TextureRegion = new Rectangle(padding, padding, Trait.Icon.Width - padding * 2, Trait.Icon.Height - padding * 2);
			}
		}

		public void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			ApplyTrait(this, null);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (Trait != null)
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
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
