using System;
using System.Runtime.CompilerServices;
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

		public Pet? Pet
		{
			[CompilerGenerated]
			get
			{
				return _003CPet_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CPet_003Ek__BackingField, value, delegate(Pet v)
				{
					_003CPet_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Pet>(ApplyPet));
			}
		}

		public PetTooltip()
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

		private void ApplyPet(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Pet> e)
		{
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
			if (Pet != null)
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
			Pet = null;
			_image.Texture = null;
			base.DisposeControl();
		}
	}
}
