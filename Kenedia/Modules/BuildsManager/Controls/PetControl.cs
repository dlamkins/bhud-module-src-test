using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class PetControl : Control
	{
		private readonly DetailedTexture _highlight = new DetailedTexture(156844)
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private readonly DetailedTexture _selector = new DetailedTexture(157138, 157140);

		private readonly DetailedTexture _petTexture = new DetailedTexture(156794)
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private readonly DetailedTexture _emptySlotTexture = new DetailedTexture(157154)
		{
			TextureRegion = new Rectangle(14, 14, 100, 100)
		};

		public PetSlotType PetSlot { get; set; }

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

		public Action<PetControl> LeftClickAction { get; set; }

		public Action<PetControl> RightClickAction { get; set; }

		public PetControl()
		{
			base.Tooltip = new PetTooltip();
		}

		private void ApplyPet(object sender, ValueChangedEventArgs<Pet> e)
		{
			_petTexture.Texture = ((Pet?.Icon == null) ? AsyncTexture2D.FromAssetId(156794) : Pet?.Icon);
			PetTooltip petTooltip = base.Tooltip as PetTooltip;
			if (petTooltip != null)
			{
				petTooltip.Pet = Pet;
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			RecalculateLayout();
			_selector?.Draw(this, spriteBatch);
			_petTexture?.Draw(this, spriteBatch);
			if (base.MouseOver)
			{
				_highlight?.Draw(this, spriteBatch);
			}
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
			_petTexture.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_highlight.Bounds = _petTexture.Bounds;
			Point p = new Point(base.Width / 2, base.Height / 2);
			Point s = new Point(64, 15);
			_selector.Bounds = new Rectangle(p.X - s.X / 2 + 4, p.Y - 36, s.X, s.Y);
			_emptySlotTexture.Bounds = new Rectangle(p.X - s.X / 2 + 4, p.Y - 36 + _selector.Bounds.Height, s.X, s.X);
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			base.OnRightMouseButtonPressed(e);
			RightClickAction?.Invoke(this);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			LeftClickAction?.Invoke(this);
		}
	}
}
