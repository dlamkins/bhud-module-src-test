using System;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Humanizer;
using Humanizer.Localisation;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class StandardBehaviorFilter : Behavior<StandardMarker>, ICanFilter, ICanInteract, ICanFocus
	{
		public const string PRIMARY_ATTR_NAME = "behavior";

		private readonly StandardPathableBehavior _behaviorMode;

		private readonly IPackState _packState;

		public StandardBehaviorFilter(StandardPathableBehavior behaviorMode, StandardMarker marker, IPackState packState)
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

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			return new StandardBehaviorFilter((StandardPathableBehavior)attributes["behavior"].GetValueAsInt(), marker, packState);
		}

		public void Interact(bool autoTriggered)
		{
			switch (_behaviorMode)
			{
			case StandardPathableBehavior.AlwaysVisible:
				return;
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
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}

		public void Focus()
		{
			string interactText = null;
			switch (_behaviorMode)
			{
			case StandardPathableBehavior.AlwaysVisible:
				return;
			case StandardPathableBehavior.ReappearOnMapChange:
				interactText = "Hide marker until map change {0}";
				break;
			case StandardPathableBehavior.ReappearOnDailyReset:
				interactText = "Hide marker until daily reset (" + (DateTime.UtcNow.Date.AddDays(1.0) - DateTime.UtcNow).Humanize(2, null, TimeUnit.Week, TimeUnit.Second) + ") {0}";
				break;
			case StandardPathableBehavior.OnlyVisibleBeforeActivation:
				interactText = "Hide marker permanently {0}";
				break;
			case StandardPathableBehavior.ReappearAfterTimer:
				interactText = "Hide marker for " + TimeSpan.FromSeconds(_pathingEntity.ResetLength).Humanize(4) + " {0}";
				break;
			case StandardPathableBehavior.ReappearOnMapReset:
				interactText = "Hide marker until map reset {0}";
				break;
			case StandardPathableBehavior.OncePerInstance:
				interactText = "Hide marker permanently on this instance {0}";
				break;
			case StandardPathableBehavior.OnceDailyPerCharacter:
				interactText = "Hide marker for this character until daily reset {0}";
				break;
			case StandardPathableBehavior.ReappearOnWeeklyReset:
				interactText = "Hide marker until weekly reset {0}";
				break;
			default:
				interactText = "Interact with behavior {0}";
				break;
			}
			_packState.UiStates.Interact.ShowInteract(_pathingEntity, interactText);
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