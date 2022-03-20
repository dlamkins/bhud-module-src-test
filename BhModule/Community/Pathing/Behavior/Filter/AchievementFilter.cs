using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class AchievementFilter : IBehavior, IUpdatable, ICanFilter, ICanInteract
	{
		public const string PRIMARY_ATTR_NAME = "achievement";

		public const string ATTR_ID = "achievementid";

		public const string ATTR_BIT = "achievementbit";

		private readonly IPackState _packState;

		private bool _triggered;

		public int AchievementId { get; set; }

		public int AchievementBit { get; set; }

		public AchievementFilter(int achievementId, int achievementBit, IPackState packState)
		{
			AchievementId = achievementId;
			AchievementBit = achievementBit;
			_packState = packState;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, IPackState packState)
		{
			IAttribute idAttr;
			IAttribute bitAttr;
			return new AchievementFilter(attributes.TryGetAttribute("achievementid", out idAttr) ? idAttr.GetValueAsInt() : 0, attributes.TryGetAttribute("achievementbit", out bitAttr) ? bitAttr.GetValueAsInt() : (-1), packState);
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}

		public bool IsFiltered()
		{
			if (!_triggered)
			{
				return _packState.AchievementStates.IsAchievementHidden(AchievementId, AchievementBit);
			}
			return true;
		}

		public void Interact(bool autoTriggered)
		{
			_triggered = true;
		}
	}
}
