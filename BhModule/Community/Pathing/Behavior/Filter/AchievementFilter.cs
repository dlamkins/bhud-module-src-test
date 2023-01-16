using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.UI.Tooltips;
using BhModule.Community.Pathing.Utility;
using Blish_HUD.Common.UI.Views;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class AchievementFilter : Behavior<IPathingEntity>, ICanFilter, ICanInteract, ICanFocus
	{
		public const string PRIMARY_ATTR_NAME = "achievement";

		public const string ATTR_ID = "achievementid";

		public const string ATTR_BIT = "achievementbit";

		private readonly IPackState _packState;

		private bool _triggered;

		public int AchievementId { get; set; }

		public int AchievementBit { get; set; }

		public AchievementFilter(int achievementId, int achievementBit, IPathingEntity pathingEntity, IPackState packState)
			: base(pathingEntity)
		{
			AchievementId = achievementId;
			AchievementBit = achievementBit;
			_packState = packState;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, IPathingEntity pathingEntity, IPackState packState)
		{
			IAttribute idAttr;
			IAttribute bitAttr;
			return new AchievementFilter(attributes.TryGetAttribute("achievementid", out idAttr) ? idAttr.GetValueAsInt() : 0, attributes.TryGetAttribute("achievementbit", out bitAttr) ? bitAttr.GetValueAsInt() : (-1), pathingEntity, packState);
		}

		public bool IsFiltered()
		{
			if (!_triggered)
			{
				return _packState.AchievementStates.IsAchievementHidden(AchievementId, AchievementBit);
			}
			return true;
		}

		public string FilterReason()
		{
			return "Hidden because you've completed the achievement associated with this marker.";
		}

		public void Interact(bool autoTriggered)
		{
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
			_triggered = true;
		}

		public void Focus()
		{
			_packState.UiStates.Interact.ShowInteract(_pathingEntity, (ITooltipView)(object)new AchievementTooltipView(AchievementId));
		}

		public void Unfocus()
		{
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}

		public override void Unload()
		{
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}
	}
}
