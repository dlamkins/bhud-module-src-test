using System;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class StandardBehaviorFilter : Behavior<StandardMarker>, ICanFilter, ICanInteract
	{
		public const string PRIMARY_ATTR_NAME = "behavior";

		private readonly StandardPathableBehavior _behaviorMode;

		private readonly IPackState _packState;

		public StandardBehaviorFilter(StandardPathableBehavior behaviorMode, IPackState packState, StandardMarker marker)
			: base(marker)
		{
			_behaviorMode = behaviorMode;
			_packState = packState;
		}

		public bool IsFiltered()
		{
			if (_pathingEntity.InvertBehavior)
			{
				if (_behaviorMode == StandardPathableBehavior.OnceDailyPerCharacter)
				{
					return !_packState.BehaviorStates.IsBehaviorHidden(_behaviorMode, _pathingEntity.Guid.Xor(GameService.Gw2Mumble.get_PlayerCharacter().get_Name().ToGuid()));
				}
				return !_packState.BehaviorStates.IsBehaviorHidden(_behaviorMode, _pathingEntity.Guid);
			}
			if (_behaviorMode == StandardPathableBehavior.OnceDailyPerCharacter)
			{
				return _packState.BehaviorStates.IsBehaviorHidden(_behaviorMode, _pathingEntity.Guid.Xor(GameService.Gw2Mumble.get_PlayerCharacter().get_Name().ToGuid()));
			}
			return _packState.BehaviorStates.IsBehaviorHidden(_behaviorMode, _pathingEntity.Guid);
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, IPackState packState, StandardMarker marker)
		{
			return new StandardBehaviorFilter((StandardPathableBehavior)attributes["behavior"].GetValueAsInt(), packState, marker);
		}

		public void Interact(bool autoTriggered)
		{
			switch (_behaviorMode)
			{
			case StandardPathableBehavior.ReappearOnMapChange:
			case StandardPathableBehavior.OnlyVisibleBeforeActivation:
			case StandardPathableBehavior.OncePerInstance:
				_packState.BehaviorStates.AddFilteredBehavior(_behaviorMode, _pathingEntity.Guid);
				break;
			case StandardPathableBehavior.ReappearOnDailyReset:
				_packState.BehaviorStates.AddFilteredBehavior(_pathingEntity.Guid, DateTime.UtcNow.Date.AddDays(1.0));
				break;
			case StandardPathableBehavior.ReappearAfterTimer:
				_packState.BehaviorStates.AddFilteredBehavior(_pathingEntity.Guid, DateTime.UtcNow.AddSeconds(_pathingEntity.ResetLength));
				break;
			case StandardPathableBehavior.OnceDailyPerCharacter:
				_packState.BehaviorStates.AddFilteredBehavior(_pathingEntity.Guid.Xor(GameService.Gw2Mumble.get_PlayerCharacter().get_Name().ToGuid()), DateTime.UtcNow.Date.AddDays(1.0));
				break;
			case StandardPathableBehavior.ReappearOnWeeklyReset:
			{
				DateTime now = DateTime.UtcNow;
				TimeSpan sevenThirtyAm = new TimeSpan(7, 30, 0);
				int daysUntilMonday = (int)(1 - now.DayOfWeek + 7) % 7;
				if (daysUntilMonday == 0 && now.TimeOfDay > sevenThirtyAm)
				{
					daysUntilMonday = 7;
				}
				_packState.BehaviorStates.AddFilteredBehavior(_pathingEntity.Guid, now.Date.AddDays(daysUntilMonday).Add(sevenThirtyAm));
				break;
			}
			}
		}
	}
}
