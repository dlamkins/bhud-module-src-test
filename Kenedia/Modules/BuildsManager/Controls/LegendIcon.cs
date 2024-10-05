using System;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class LegendIcon : Control
	{
		private Legend? _legend;

		private readonly DetailedTexture _selector = new DetailedTexture(157138, 157140);

		private readonly DetailedTexture _fallBackTexture = new DetailedTexture(157154);

		private readonly DetailedTexture _hoveredFrameTexture = new DetailedTexture(157143)
		{
			TextureRegion = new Rectangle(8, 8, 112, 112)
		};

		private readonly DetailedTexture _noAquaticFlagTexture = new DetailedTexture(157145)
		{
			TextureRegion = new Rectangle(16, 16, 96, 96)
		};

		private readonly DetailedTexture _texture = new DetailedTexture
		{
			TextureRegion = new Rectangle(14, 14, 100, 100)
		};

		public Legend? Legend
		{
			get
			{
				return _legend;
			}
			set
			{
				Common.SetProperty(ref _legend, value, new Action(ApplyLegend));
			}
		}

		public LegendSlotType LegendSlot { get; set; }

		public Action<LegendIcon> LeftClickAction { get; set; }

		public Action<LegendIcon> RightClickAction { get; set; }

		public SkillTooltip SkillTooltip { get; }

		public bool IsActive { get; set; }

		public LegendIcon()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			base.Tooltip = (SkillTooltip = new SkillTooltip());
			base.Size = new Point(48, 62);
		}

		private void ApplyLegend()
		{
			_texture.Texture = Legend?.Swap.Icon;
			SkillTooltip.Skill = Legend?.Swap;
		}

		public override void RecalculateLayout()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			int selectorHeight = (int)(0.189873417721519 * (double)base.Height);
			_selector.Bounds = new Rectangle(0, 0, base.Width, selectorHeight);
			_texture.Bounds = new Rectangle(0, selectorHeight - 1, base.Width, base.Height - selectorHeight - 5);
			_hoveredFrameTexture.Bounds = new Rectangle(0, selectorHeight - 1, base.Width, base.Height - selectorHeight - 5);
			_noAquaticFlagTexture.Bounds = new Rectangle(0, selectorHeight - 1, base.Width, base.Height - selectorHeight - 5);
			base.Size = new Point(48, 62);
			base.ClipsBounds = true;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			if (Legend != null)
			{
				_texture.Draw(this, spriteBatch, base.RelativeMousePosition, IsActive ? Color.get_White() : Color.get_Gray());
			}
			else
			{
				_fallBackTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
			}
			if (base.MouseOver)
			{
				_hoveredFrameTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
			}
			LegendSlotType legendSlot = LegendSlot;
			if (((uint)legendSlot <= 1u) ? true : false)
			{
				Legend? legend = Legend;
				if (legend != null && legend!.Swap.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					_noAquaticFlagTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.get_White());
				}
			}
			_selector?.Draw(this, spriteBatch, base.RelativeMousePosition);
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			LeftClickAction?.Invoke(this);
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			base.OnRightMouseButtonPressed(e);
			RightClickAction?.Invoke(this);
		}
	}
}
