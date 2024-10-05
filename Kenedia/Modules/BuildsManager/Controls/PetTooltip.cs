using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class PetTooltip : Tooltip
	{
		private readonly DetailedTexture _image = new DetailedTexture
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private readonly Kenedia.Modules.Core.Controls.Label _title;

		private readonly Kenedia.Modules.Core.Controls.Label _id;

		private readonly Kenedia.Modules.Core.Controls.Label _description;

		private Pet? _pet;

		public Pet? Pet
		{
			get
			{
				return _pet;
			}
			set
			{
				Common.SetProperty<Pet>(ref _pet, value, new ValueChangedEventHandler<Pet>(ApplyPet));
			}
		}

		public PetTooltip()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
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

		private void ApplyPet(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Pet> e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			_title.TextColor = ContentService.Colors.Chardonnay;
			_title.Text = Pet?.Name;
			_id.Text = $"{strings.PetId}: {Pet?.Id}";
			_description.Text = Pet?.Description?.Substring(0, Pet!.Description.Length - 5).InterpretItemDescription();
			_image.Texture = Pet?.Icon;
		}

		private void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			ApplyPet(this, null);
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (Pet != null)
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
			Pet = null;
			_image.Texture = null;
			base.DisposeControl();
		}
	}
}
