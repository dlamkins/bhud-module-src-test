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
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Invalid comparison between Unknown and I4
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
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
				distanceSort = (distanceSort.IsNullOrEmpty() ? distanceSort : distanceSort.Take(1).ToList());
			}
			Rectangle dest = default(Rectangle);
			foreach (WvwObjectiveEntity objectiveEntity in distanceSort)
			{
				if (objectiveEntity.Icon != null && (MistwarModule.ModuleInstance.DrawRuinMarkersSetting.get_Value() || (int)objectiveEntity.Type != 6))
				{
					Vector3 dir = GameService.Gw2Mumble.get_PlayerCamera().get_Position() - objectiveEntity.WorldPosition;
					if (!(Math.Abs(180.0 / Math.PI * GameService.Gw2Mumble.get_PlayerCamera().get_Forward().Angle(dir)) < 90.0))
					{
						Matrix trs = Matrix.CreateScale(1f) * Matrix.CreateTranslation(objectiveEntity.WorldPosition);
						Vector2 transformed = Vector3.Transform(((Matrix)(ref trs)).get_Translation(), GameService.Gw2Mumble.get_PlayerCamera().get_WorldViewProjection()).Flatten();
						int width = objectiveEntity.Icon.get_Width();
						int height = objectiveEntity.Icon.get_Height();
						((Rectangle)(ref dest))._002Ector((int)transformed.X, (int)transformed.Y, width, height);
						spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, objectiveEntity.Opacity, MathUtil.Clamp(MistwarModule.ModuleInstance.MarkerScaleSetting.get_Value() / 100f, 0f, 1f), drawName: true, MistwarModule.ModuleInstance.DrawDistanceSetting.get_Value());
					}
				}
			}
		}

		protected override CaptureType CapturesInput()
		{
			return (CaptureType)0;
		}
	}
}
