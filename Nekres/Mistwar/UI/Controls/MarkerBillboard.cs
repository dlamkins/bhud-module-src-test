using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MarkerBillboard : Control
	{
		public IEnumerable<WvwObjectiveEntity> WvwObjectives;

		public MarkerBillboard()
			: this()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			base._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
		}

		public void Toggle(bool forceHide = false, float tDuration = 0.1f)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (forceHide || !GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || base._visible)
			{
				base._visible = false;
				((Control)this).set_Visible(false);
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MarkerBillboard>(this, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)((Control)this).Hide);
			}
			else
			{
				base._visible = true;
				((Control)this).set_Visible(true);
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MarkerBillboard>(this, (object)new
				{
					Opacity = 1f
				}, 0.35f, 0f, true);
			}
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Invalid comparison between Unknown and I4
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			if (!GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || !((Control)this).get_Visible() || WvwObjectives == null)
			{
				return;
			}
			Rectangle absoluteBounds = ((Control)((Control)this).get_Parent()).get_AbsoluteBounds();
			((Control)this).set_Size(((Rectangle)(ref absoluteBounds)).get_Size());
			IEnumerable<WvwObjectiveEntity> objectives = WvwObjectives.Where((WvwObjectiveEntity x) => x.Icon != null);
			if (MistwarModule.ModuleInstance.HideAlliedMarkersSetting.get_Value())
			{
				objectives = objectives.Where((WvwObjectiveEntity x) => x.Owner != MistwarModule.ModuleInstance.WvwService.CurrentTeam);
			}
			List<WvwObjectiveEntity> distanceSort = objectives.OrderByDescending((WvwObjectiveEntity x) => x.GetDistance()).ToList();
			if (MistwarModule.ModuleInstance.HideInCombatSetting.get_Value() && GameService.Gw2Mumble.get_PlayerCharacter().get_IsInCombat())
			{
				WvwObjectiveEntity closest = distanceSort.LastOrDefault();
				if (closest == null)
				{
					return;
				}
				distanceSort = new List<WvwObjectiveEntity> { closest };
			}
			Rectangle dest = default(Rectangle);
			Rectangle screenBounds = default(Rectangle);
			foreach (WvwObjectiveEntity objectiveEntity in distanceSort)
			{
				if (objectiveEntity.Icon == null || (!MistwarModule.ModuleInstance.DrawRuinMarkersSetting.get_Value() && (int)objectiveEntity.Type == 6))
				{
					continue;
				}
				Vector3 dir = GameService.Gw2Mumble.get_PlayerCamera().get_Position() - objectiveEntity.WorldPosition;
				double value = 180.0 / Math.PI * GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Angle(dir);
				Matrix trs = Matrix.CreateScale(1f) * Matrix.CreateTranslation(objectiveEntity.WorldPosition);
				Vector2 transformed = Vector3.Transform(((Matrix)(ref trs)).get_Translation(), GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection()).Flatten();
				int width = objectiveEntity.Icon.get_Width();
				int height = objectiveEntity.Icon.get_Height();
				((Rectangle)(ref dest))._002Ector((int)transformed.X, (int)transformed.Y, width, height);
				((Rectangle)(ref screenBounds))._002Ector(0, 0, bounds.Width, bounds.Height);
				int left = ((Rectangle)(ref screenBounds)).get_Left() + dest.Width / 2;
				int right = ((Rectangle)(ref screenBounds)).get_Right() - dest.Width;
				int top = ((Rectangle)(ref screenBounds)).get_Top() + dest.Height / 2;
				int bottom = ((Rectangle)(ref screenBounds)).get_Bottom() - dest.Height;
				if (Math.Abs(value) < 90.0)
				{
					if (!MistwarModule.ModuleInstance.MarkerStickySetting.get_Value())
					{
						continue;
					}
					dest.Y = bottom;
				}
				if (MistwarModule.ModuleInstance.MarkerStickySetting.get_Value())
				{
					dest.X = (int)MathUtil.Clamp(dest.X, left, right);
					dest.Y = (int)MathUtil.Clamp(dest.Y, top, bottom);
				}
				float minScale = 0.2f;
				float maxScale = MathUtil.Clamp(MistwarModule.ModuleInstance.MarkerScaleSetting.get_Value() / 100f, minScale, 1f);
				float scale = maxScale;
				float distance = objectiveEntity.GetDistance();
				if (!MistwarModule.ModuleInstance.MarkerFixedSizeSetting.get_Value() && distance > 400f)
				{
					scale = MathUtil.Clamp((float)Math.Sqrt(Math.Abs(1f - distance / 1500f)), minScale, maxScale);
				}
				spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, objectiveEntity.Opacity, scale, drawName: true, MistwarModule.ModuleInstance.DrawDistanceSetting.get_Value());
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}
	}
}
