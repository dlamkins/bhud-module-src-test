using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class PetIcon : DetailedTexture
	{
		private readonly AsyncTexture2D _paw = AsyncTexture2D.FromAssetId(156797);

		private readonly AsyncTexture2D _pawPressed = AsyncTexture2D.FromAssetId(156796);

		private Pet? _pet;

		public PetSlotType PetSlot { get; set; }

		public Rectangle PawRegion { get; set; }

		public Pet? Pet
		{
			get
			{
				return _pet;
			}
			set
			{
				Common.SetProperty(ref _pet, value, new Action(ApplyPet));
			}
		}

		public PetIcon()
		{
			base.FallBackTexture = AsyncTexture2D.FromAssetId(156797);
		}

		private void ApplyPet()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			base.Texture = Pet?.Icon;
			base.TextureRegion = new Rectangle(16, 16, 200, 200);
		}

		public override void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.get_White();
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.get_Zero();
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			int hovered;
			if (mousePos.HasValue)
			{
				Rectangle pawRegion = PawRegion;
				hovered = (((Rectangle)(ref pawRegion)).Contains(mousePos.Value) ? 1 : 0);
			}
			else
			{
				hovered = 0;
			}
			base.Hovered = (byte)hovered != 0;
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, _paw, PawRegion, _paw.Bounds, color.Value, rotation.Value, origin.Value, (SpriteEffects)0);
			}
		}
	}
}
