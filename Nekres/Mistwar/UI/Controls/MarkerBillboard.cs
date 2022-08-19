using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.Mistwar.Entities;

namespace Nekres.Mistwar.UI.Controls
{
	internal class MarkerBillboard : Control
	{
		public IEnumerable<WvwObjectiveEntity> WvwObjectives;

		private BitmapFont _font;

		public MarkerBillboard()
			: this()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			_font = Control.get_Content().GetFont((FontFace)0, (FontSize)24, (FontStyle)0);
			base._spriteBatchParameters = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
		}

		public void Toggle(float tDuration = 0.1f, bool silent = false)
		{
			if (base._visible)
			{
				if (GameUtil.IsUiAvailable())
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
				}
			}
			else
			{
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
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Invalid comparison between Unknown and I4
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			if (!((Control)this).get_Visible() || WvwObjectives == null)
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
				distanceSort = new List<WvwObjectiveEntity> { distanceSort[0] };
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
						spriteBatch.DrawWvwObjectiveOnCtrl((Control)(object)this, objectiveEntity, dest, objectiveEntity.Opacity, _font);
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
