using System;
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

		private Skill _skill;

		public Skill Skill
		{
			get
			{
				return _skill;
			}
			set
			{
				Common.SetProperty(ref _skill, value, new ValueChangedEventHandler<Skill>(SetSkill));
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
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
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			Title = skill?.Name;
			Id = $"{strings.SkillId}: {skill?.Id}";
			Description = skill?.Description.InterpretItemDescription();
			_image.Texture = skill?.Icon;
			Rectangle bounds = _image.Bounds;
			base.Height = ((Rectangle)(ref bounds)).get_Bottom() + 10 + UI.GetTextHeight(Control.Content.DefaultFont14, Description, base.Width);
			RecalculateLayout();
		}

		public override void Draw(SpriteBatch spriteBatch, Rectangle drawBounds, Rectangle scissor)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Id))
			{
				base.Draw(spriteBatch, drawBounds, scissor);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			Rectangle val = _image.Bounds;
			int num = ((Rectangle)(ref val)).get_Right() + 5;
			val = _image.Bounds;
			int top = ((Rectangle)(ref val)).get_Top();
			int width = base.Width;
			val = _image.Bounds;
			TitleBounds = new Rectangle(num, top, width - ((Rectangle)(ref val)).get_Right() - 5, Control.Content.DefaultFont18.get_LineHeight());
			val = _image.Bounds;
			int num2 = ((Rectangle)(ref val)).get_Right() + 5;
			val = TitleBounds;
			int num3 = ((Rectangle)(ref val)).get_Bottom() + 8;
			int width2 = base.Width;
			val = _image.Bounds;
			IdBounds = new Rectangle(num2, num3, width2 - ((Rectangle)(ref val)).get_Right() - 5, Control.Content.DefaultFont12.get_LineHeight());
			val = _image.Bounds;
			int left = ((Rectangle)(ref val)).get_Left();
			val = _image.Bounds;
			int num4 = ((Rectangle)(ref val)).get_Bottom() + 10;
			int num5 = base.Width - 10;
			int height = base.Height;
			val = _image.Bounds;
			DescriptionBounds = new Rectangle(left, num4, num5, height - ((Rectangle)(ref val)).get_Bottom() - 10);
		}

		protected override void DisposeControl()
		{
			_image.Texture = null;
			LocalizingService.LocaleChanged -= new EventHandler<Blish_HUD.ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			base.DisposeControl();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			_image.Draw(this, spriteBatch);
			spriteBatch.DrawStringOnCtrl(this, Title, Control.Content.DefaultFont18, TitleBounds, TitleColor);
			spriteBatch.DrawStringOnCtrl(this, Id, Control.Content.DefaultFont12, IdBounds, Color.get_White());
			spriteBatch.DrawStringOnCtrl(this, Description, Control.Content.DefaultFont14, DescriptionBounds, Color.get_White(), wrap: true, HorizontalAlignment.Left, VerticalAlignment.Top);
		}

		public void UserLocale_SettingChanged(object sender, Blish_HUD.ValueChangedEventArgs<Locale> e)
		{
			SetSkill(Skill);
		}
	}
}
