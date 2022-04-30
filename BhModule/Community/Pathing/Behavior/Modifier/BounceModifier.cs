using System;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Glide;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class BounceModifier : Behavior<StandardMarker>, ICanFocus
	{
		public enum BounceBehavior
		{
			Bounce,
			Rise
		}

		public const string PRIMARY_ATTR_NAME = "bounce";

		private const string ATTR_DELAY = "bounce-delay";

		private const string ATTR_HEIGHT = "bounce-height";

		private const string ATTR_DURATION = "bounce-duration";

		private readonly IPackState _packState;

		private readonly float _originalVerticalOffset;

		private const BounceBehavior DEFAULT_BOUNCEBEHAVIOR = BounceBehavior.Bounce;

		private const float DEFAULT_BOUNCEDELAY = 0f;

		private const float DEFAULT_BOUNCEHEIGHT = 2f;

		private const float DEFAULT_BOUNCEDURATION = 1f;

		private Tween _bounceAnimation;

		public BounceBehavior Behavior { get; set; }

		public float BounceDelay { get; set; }

		public float BounceHeight { get; set; }

		public float BounceDuration { get; set; }

		public BounceModifier(BounceBehavior bounceBehavior, float delay, float height, float duration, StandardMarker marker, IPackState packState)
			: base(marker)
		{
			_packState = packState;
			Behavior = bounceBehavior;
			BounceDelay = delay;
			BounceHeight = height;
			BounceDuration = duration;
			_originalVerticalOffset = _pathingEntity.HeightOffset;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			IAttribute behaviorAttr;
			IAttribute delayAttr;
			IAttribute heightAttr;
			IAttribute durationAttr;
			return new BounceModifier(attributes.TryGetAttribute("bounce", out behaviorAttr) ? behaviorAttr.GetValueAsEnum<BounceBehavior>() : BounceBehavior.Bounce, attributes.TryGetAttribute("bounce-delay", out delayAttr) ? delayAttr.GetValueAsFloat() : 0f, attributes.TryGetAttribute("bounce-height", out heightAttr) ? heightAttr.GetValueAsFloat(2f) : 2f, attributes.TryGetAttribute("bounce-duration", out durationAttr) ? durationAttr.GetValueAsFloat(1f) : 1f, marker, packState);
		}

		public void Focus()
		{
			if (!_packState.UserConfiguration.PackAllowMarkersToAnimate.get_Value())
			{
				return;
			}
			Tween bounceAnimation = _bounceAnimation;
			if (bounceAnimation != null)
			{
				bounceAnimation.CancelAndComplete();
			}
			if (!_pathingEntity.BehaviorFiltered)
			{
				_bounceAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<StandardMarker>(_pathingEntity, (object)new
				{
					HeightOffset = _originalVerticalOffset + BounceHeight
				}, BounceDuration, BounceDelay, true).From((object)new
				{
					HeightOffset = _originalVerticalOffset
				});
				switch (Behavior)
				{
				case BounceBehavior.Bounce:
					_bounceAnimation = _bounceAnimation.Ease((Func<float, float>)Ease.QuadInOut).Repeat(-1).Reflect();
					break;
				case BounceBehavior.Rise:
					_bounceAnimation = _bounceAnimation.Ease((Func<float, float>)Ease.QuartInOut);
					break;
				}
			}
		}

		public void Unfocus()
		{
			Tween bounceAnimation = _bounceAnimation;
			if (bounceAnimation != null)
			{
				bounceAnimation.Cancel();
			}
			_bounceAnimation = ((TweenerImpl)GameService.Animation.get_Tweener()).Tween<StandardMarker>(_pathingEntity, (object)new
			{
				HeightOffset = _originalVerticalOffset
			}, _pathingEntity.HeightOffset / 2f, 0f, true).Ease((Func<float, float>)Ease.BounceOut);
		}

		public override void Unload()
		{
			Tween bounceAnimation = _bounceAnimation;
			if (bounceAnimation != null)
			{
				bounceAnimation.Cancel();
			}
		}
	}
}
