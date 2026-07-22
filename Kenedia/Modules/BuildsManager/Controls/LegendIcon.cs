using System;
using System.Runtime.CompilerServices;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.BuildsManager.Controls
{
	public class LegendIcon : Control
	{
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
			[CompilerGenerated]
			get
			{
				return _003CLegend_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CLegend_003Ek__BackingField, value, delegate(Legend v)
				{
					_003CLegend_003Ek__BackingField = v;
				}, new Action(ApplyLegend));
			}
		}

		public LegendSlotType LegendSlot { get; set; }

		public Action<LegendIcon> LeftClickAction { get; set; }

		public Action<LegendIcon> RightClickAction { get; set; }

		public SkillTooltip SkillTooltip { get; }

		public bool IsActive { get; set; }

		public LegendIcon()
		{
			base.Tooltip = (SkillTooltip = new SkillTooltip());
			base.Size = new Point(48, 62);
		}

		private void ApplyLegend()
		{
			_texture.Texture = TexturesService.GetAsyncTexture(Legend?.Swap.IconAssetId);
			SkillTooltip.Skill = Legend?.Swap;
		}

		public override void RecalculateLayout()
		{
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
			if (Legend != null)
			{
				_texture.Draw(this, spriteBatch, base.RelativeMousePosition, IsActive ? Color.White : Color.Gray);
			}
			else
			{
				_fallBackTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
			}
			if (base.MouseOver)
			{
				_hoveredFrameTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
			}
			LegendSlotType legendSlot = LegendSlot;
			if (((uint)legendSlot <= 1u) ? true : false)
			{
				Legend? legend = Legend;
				if (legend != null && legend!.Swap.Flags.HasFlag(SkillFlag.NoUnderwater))
				{
					_noAquaticFlagTexture.Draw(this, spriteBatch, base.RelativeMousePosition, Color.White);
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
