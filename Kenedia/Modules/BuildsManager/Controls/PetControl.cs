using System;
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

		private readonly DetailedTexture _petTexture = new DetailedTexture
		{
			TextureRegion = new Rectangle(16, 16, 200, 200)
		};

		private readonly DetailedTexture _emptySlotTexture = new DetailedTexture(157154)
		{
			TextureRegion = new Rectangle(14, 14, 100, 100)
		};

		private Pet? _pet;

		public PetSlotType PetSlot { get; set; }

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

		public Action<PetControl> LeftClickAction { get; set; }

		public Action<PetControl> RightClickAction { get; set; }

		public PetControl()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			base.Tooltip = new PetTooltip();
		}

		private void ApplyPet(object sender, ValueChangedEventArgs<Pet> e)
		{
			_petTexture.Texture = Pet?.Icon;
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
			if (Pet == null)
			{
				_emptySlotTexture?.Draw(this, spriteBatch);
			}
			if (base.MouseOver)
			{
				_highlight?.Draw(this, spriteBatch);
			}
		}

		public override void RecalculateLayout()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_petTexture.Bounds = new Rectangle(0, 0, base.Width, base.Height);
			_highlight.Bounds = _petTexture.Bounds;
			Point p = default(Point);
			((Point)(ref p))._002Ector(base.Width / 2, base.Height / 2);
			Point s = default(Point);
			((Point)(ref s))._002Ector(64, 15);
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
