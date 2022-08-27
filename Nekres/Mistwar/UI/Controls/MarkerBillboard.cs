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

		public void Toggle(bool forceHide = false, float tDuration = 0.1f, bool silent = false)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			silent = silent || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch();
			if (forceHide || !GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || base._visible)
			{
				base._visible = false;
				if (silent)
				{
					((Control)this).Hide();
					return;
				}
				GameService.Content.PlaySoundEffectByName("window-close");
				((TweenerImpl)GameService.Animation.get_Tweener()).Tween<MarkerBillboard>(this, (object)new
				{
					Opacity = 0f
				}, tDuration, 0f, true).OnComplete((Action)((Control)this).Hide);
				return;
			}
			base._visible = true;
			((Control)this).Show();
			if (!silent)
			{
				GameService.Content.PlaySoundEffectByName("page-open-" + RandomUtil.GetRandom(1, 3));
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
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Invalid comparison between Unknown and I4
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			if (!GameUtil.IsAvailable() || !GameService.Gw2Mumble.get_CurrentMap().get_Type().IsWvWMatch() || !((Control)this).get_Visible() || WvwObjectives == null)
			{
				return;
			}
			Rectangle absoluteBounds = ((Control)((Control)this).get_Parent()).get_AbsoluteBounds();
			((Control)this).set_Size(((Rectangle)(ref absoluteBounds)).get_Size());
			List<WvwObjectiveEntity> distanceSort = (from x in WvwObjectives
				where x.Icon != null
				orderby x.WorldPosition.Distance(GameService.Gw2Mumble.get_PlayerCamera().get_Position())
				select x).ToList();
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
						spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, objectiveEntity.Opacity, MathUtil.Clamp(MistwarModule.ModuleInstance.MarkerScaleSetting.get_Value() / 100f, 0f, 1f));
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
