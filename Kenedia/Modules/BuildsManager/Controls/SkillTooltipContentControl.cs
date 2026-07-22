using System;
using System.Runtime.CompilerServices;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Res;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class SkillTooltipContentControl : Control
	{
		private readonly DetailedTexture _image = new DetailedTexture
		{
			TextureRegion = new Rectangle(14, 14, 100, 100)
		};

		private readonly Label _title;

		private readonly Label _id;

		private readonly Label _description;

		public Skill Skill
		{
			[CompilerGenerated]
			get
			{
				return _003CSkill_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CSkill_003Ek__BackingField, value, delegate(Skill v)
				{
					_003CSkill_003Ek__BackingField = v;
				}, new ValueChangedEventHandler<Skill>(SetSkill));
			}
		}

		public Color TitleColor { get; set; } = ContentService.Colors.Chardonnay;


		public string? Title { get; private set; }

		public string? Id { get; private set; }

		public string? Description { get; private set; }

		public Rectangle TitleBounds { get; private set; }

		public Rectangle IdBounds { get; private set; }

		public Rectangle DescriptionBounds { get; private set; }

		public SkillTooltipContentControl()
		{
			_image.Bounds = new Rectangle(4, 4, 48, 48);
			LocalizingService.LocaleChanged += new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
			base.Width = 300;
		}

		public SkillTooltipContentControl(Skill skill)
			: this()
		{
			SetSkill(skill);
		}

		private void SetSkill(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Skill> e)
		{
			SetSkill(e.NewValue);
		}

		private void SetSkill(Skill skill)
		{
			Title = skill?.Name;
			Id = $"{strings.SkillId}: {skill?.Id}";
			Description = skill?.Description.InterpretItemDescription();
			_image.Texture = TexturesService.GetAsyncTexture(skill?.IconAssetId);
			base.Height = _image.Bounds.Bottom + 10 + UI.GetTextHeight(Control.Content.DefaultFont14, Description, base.Width);
			RecalculateLayout();
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Id))
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			TitleBounds = new Rectangle(_image.Bounds.Right + 5, _image.Bounds.Top, base.Width - _image.Bounds.Right - 5, Control.Content.DefaultFont18.LineHeight);
			IdBounds = new Rectangle(_image.Bounds.Right + 5, TitleBounds.Bottom + 8, base.Width - _image.Bounds.Right - 5, Control.Content.DefaultFont12.LineHeight);
			DescriptionBounds = new Rectangle(_image.Bounds.Left, _image.Bounds.Bottom + 10, base.Width - 10, base.Height - _image.Bounds.Bottom - 10);
		}

		protected override void DisposeControl()
		{
			_image.Texture = null;
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			base.DisposeControl();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_image.Draw(this, spriteBatch);
			spriteBatch.DrawStringOnCtrl(this, Title, Control.Content.DefaultFont18, TitleBounds, TitleColor);
			spriteBatch.DrawStringOnCtrl(this, Id, Control.Content.DefaultFont12, IdBounds, Color.White);
			spriteBatch.DrawStringOnCtrl(this, Description, Control.Content.DefaultFont14, DescriptionBounds, Color.White, wrap: true, HorizontalAlignment.Left, VerticalAlignment.Top);
		}

		public void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			SetSkill(Skill);
		}
	}
}
