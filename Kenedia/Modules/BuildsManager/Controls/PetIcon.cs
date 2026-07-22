using System;
using System.Runtime.CompilerServices;
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

		public PetSlotType PetSlot { get; set; }

		public Rectangle PawRegion { get; set; }

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
				}, new Action(ApplyPet));
			}
		}

		public PetIcon()
		{
			base.FallBackTexture = AsyncTexture2D.FromAssetId(156797);
		}

		private void ApplyPet()
		{
			base.Texture = Pet?.Icon;
			base.TextureRegion = new Rectangle(16, 16, 200, 200);
		}

		public override void Draw(Control ctrl, SpriteBatch spriteBatch, Point? mousePos = null, Color? color = null, Color? bgColor = null, bool? forceHover = null, float? rotation = null, Vector2? origin = null)
		{
			base.Draw(ctrl, spriteBatch, mousePos, color, bgColor, forceHover, rotation, origin);
			Color valueOrDefault = color.GetValueOrDefault();
			if (!color.HasValue)
			{
				valueOrDefault = Color.White;
				color = valueOrDefault;
			}
			Vector2 valueOrDefault2 = origin.GetValueOrDefault();
			if (!origin.HasValue)
			{
				valueOrDefault2 = Vector2.Zero;
				origin = valueOrDefault2;
			}
			float valueOrDefault3 = rotation.GetValueOrDefault();
			if (!rotation.HasValue)
			{
				valueOrDefault3 = 0f;
				rotation = valueOrDefault3;
			}
			base.Hovered = mousePos.HasValue && PawRegion.Contains(mousePos.Value);
			if (base.Hovered)
			{
				spriteBatch.DrawOnCtrl(ctrl, _paw, PawRegion, _paw.Bounds, color.Value, rotation.Value, origin.Value);
			}
		}
	}
}
