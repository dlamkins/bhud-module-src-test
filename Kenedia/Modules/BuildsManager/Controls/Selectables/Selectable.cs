using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls.Selectables
{
	public class Selectable<IBaseApiData> : Control
	{
		private Rectangle _textureBounds;

		private IBaseApiData? _data;

		protected AsyncTexture2D? Texture;

		public IBaseApiData? Data
		{
			get
			{
				return _data;
			}
			set
			{
				Common.SetProperty<IBaseApiData>(ref _data, value, new ValueChangedEventHandler<IBaseApiData>(ApplyData));
			}
		}

		public SelectableType Type { get; private set; }

		public Rectangle TextureRegion { get; private set; }

		public Action<IBaseApiData?> OnClickAction { get; set; }

		public bool IsSelected { get; set; }

		public bool HighlightSelected { get; set; } = true;


		public bool HighlightHovered { get; set; } = true;


		public Selectable()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			base.Size = new Point(64);
		}

		protected virtual void ApplyData(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<IBaseApiData> e)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			if (Data == null)
			{
				return;
			}
			IBaseApiData data = Data;
			Legend legend = data as Legend;
			if (legend == null)
			{
				Skill skill = data as Skill;
				if (skill == null)
				{
					Pet pet = data as Pet;
					if (pet != null)
					{
						Type = SelectableType.Pet;
						TextureRegion = new Rectangle(16, 16, 200, 200);
						Texture = pet.Icon;
						base.ClipsBounds = false;
						HighlightHovered = false;
						HighlightSelected = false;
					}
				}
				else
				{
					AsyncTexture2D skillIcon = TexturesService.GetAsyncTexture(skill.IconAssetId);
					Type = SelectableType.Skill;
					int sPadding = (int)((double)skillIcon.Width * (7.0 / 64.0));
					TextureRegion = new Rectangle(sPadding, sPadding, skillIcon.Width - sPadding * 2, skillIcon.Height - sPadding * 2);
					Texture = skillIcon;
					base.ClipsBounds = true;
				}
			}
			else
			{
				AsyncTexture2D legendIcon = TexturesService.GetAsyncTexture(legend.Swap.IconAssetId);
				Type = SelectableType.Legend;
				int lPadding = (int)((double)legendIcon.Width * (7.0 / 64.0));
				TextureRegion = new Rectangle(lPadding, lPadding, legendIcon.Width - lPadding * 2, legendIcon.Height - lPadding * 2);
				Texture = legendIcon;
				base.ClipsBounds = true;
			}
		}

		public override void RecalculateLayout()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (Data != null)
			{
				int pad = 48;
				_textureBounds = ((!(Data is Pet)) ? base.LocalBounds : base.LocalBounds.Add(new Rectangle(-pad, -pad, pad * 2, pad * 2)));
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			if (Data != null)
			{
				int pad = 16;
				_textureBounds = ((!(Data is Pet)) ? bounds : bounds.Add(new Rectangle(-pad, -pad, pad * 2, pad * 2)));
				if (Texture != null)
				{
					spriteBatch.DrawOnCtrl(this, Texture, _textureBounds, TextureRegion, Color.get_White());
				}
				if (HighlightSelected && IsSelected)
				{
					spriteBatch.DrawFrame(this, bounds, ContentService.Colors.ColonialWhite, 3);
				}
				if (HighlightHovered && base.MouseOver)
				{
					spriteBatch.DrawFrame(this, bounds, ContentService.Colors.ColonialWhite, 2);
				}
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			if (Data != null)
			{
				OnClickAction?.Invoke(Data);
			}
		}
	}
}
