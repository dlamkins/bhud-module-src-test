using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Effects;
using Glide;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Charr.Timers_BlishHUD.Controls.Effects
{
	public class SimpleScrollingHighlightEffect : ControlEffect
	{
		private const string SPARAM_MASK = "Mask";

		private const string SPARAM_OVERLAY = "Overlay";

		private const string SPARAM_ROLLER = "Roller";

		private const string SPARAM_OPACITY = "Opacity";

		private const float DEFAULT_ANIMATION_DURATION = 1f;

		private float _scrollRoller;

		private float _duration = 1f;

		private bool _forceActive;

		private readonly Effect _scrollEffect;

		private Tween _shaderAnim;

		private bool _active;

		public float ScrollRoller
		{
			get
			{
				return _scrollRoller;
			}
			set
			{
				_scrollRoller = value;
				if (!_forceActive)
				{
					_scrollEffect.get_Parameters().get_Item("Roller").SetValue(_scrollRoller);
				}
			}
		}

		public float Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		public bool ForceActive
		{
			get
			{
				return _forceActive;
			}
			set
			{
				_forceActive = value;
				if (_forceActive)
				{
					Tween shaderAnim = _shaderAnim;
					if (shaderAnim != null)
					{
						shaderAnim.Cancel();
					}
					_scrollEffect.get_Parameters().get_Item("Roller").SetValue(1f);
				}
			}
		}

		public SimpleScrollingHighlightEffect(Control assignedControl)
			: this(assignedControl)
		{
			_scrollEffect = TimersModule.ModuleInstance.Resources.MasterScrollEffect.Clone();
			_scrollEffect.get_Parameters().get_Item("Mask").SetValue((Texture)(object)GameService.Content.GetTexture("156072"));
			_scrollEffect.get_Parameters().get_Item("Overlay").SetValue((Texture)(object)GameService.Content.GetTexture("156071"));
			_scrollEffect.get_Parameters().get_Item("Opacity").SetValue(assignedControl.get_Opacity());
		}

		protected override SpriteBatchParameters GetSpriteBatchParameters()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			return new SpriteBatchParameters((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.LinearWrap, (DepthStencilState)null, (RasterizerState)null, _scrollEffect, (Matrix?)GameService.Graphics.get_UIScaleTransform());
		}

		protected override void OnEnable()
		{
			if (base._enabled && !_forceActive)
			{
				_scrollEffect.get_Parameters().get_Item("Opacity").SetValue(((ControlEffect)this).get_AssignedControl().get_Opacity());
				ScrollRoller = 0f;
				_shaderAnim = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<SimpleScrollingHighlightEffect>(this, (object)new
				{
					ScrollRoller = 1f
				}, _duration, 0f, true);
				_active = true;
			}
		}

		protected override void OnDisable()
		{
			Tween shaderAnim = _shaderAnim;
			if (shaderAnim != null)
			{
				shaderAnim.Cancel();
			}
			_shaderAnim = null;
			ScrollRoller = 0f;
			_active = false;
		}

		public override void PaintEffect(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (_active || _forceActive)
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, ((ControlEffect)this).get_AssignedControl(), Textures.get_Pixel(), bounds, Color.get_Transparent());
			}
		}
	}
}
