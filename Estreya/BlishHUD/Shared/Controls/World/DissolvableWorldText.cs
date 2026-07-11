using System;
using Blish_HUD;
using Blish_HUD.Entities;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Estreya.BlishHUD.Shared.Controls.World
{
	public class DissolvableWorldText : WorldText
	{
		private Logger Logger = Logger.GetLogger(typeof(DissolvableWorldText));

		private float _amount;

		public float Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				_amount = value;
				Effect effect = base.EffectParameters.get_Effect();
				if (effect != null)
				{
					effect.get_Parameters().get_Item("Amount").SetValue(_amount);
				}
			}
		}

		public Action<DissolvableWorldText> OnBeforeRender { get; set; } = delegate
		{
		};


		public DissolvableWorldText(Func<string> getText, BitmapFont font, Vector3 position, float scale, Color color, ContentsManager contentsManager)
			: base(getText, font, position, scale, color)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			Effect dissolveEffectForText = contentsManager.GetEffect("effects/dissolve.mgfx");
			dissolveEffectForText.get_Parameters().get_Item("Amount").SetValue(0f);
			dissolveEffectForText.get_Parameters().get_Item("Opacity").SetValue(1f);
			dissolveEffectForText.get_Parameters().get_Item("Glow").SetValue(false);
			base.EffectParameters.set_Effect(dissolveEffectForText);
		}

		protected override void InternalRender(GraphicsDevice graphicsDevice, IWorld world, ICamera camera)
		{
			OnBeforeRender?.Invoke(this);
			base.InternalRender(graphicsDevice, world, camera);
		}
	}
}
