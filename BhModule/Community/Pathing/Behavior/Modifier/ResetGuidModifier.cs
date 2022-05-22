using System;
using System.Collections.Generic;
using System.Linq;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class ResetGuidModifier : Behavior<StandardMarker>, ICanInteract, ICanFocus
	{
		public const string PRIMARY_ATTR_NAME = "resetguid";

		private readonly IPackState _packState;

		public List<Guid> TargetGuids { get; set; }

		public ResetGuidModifier(IEnumerable<Guid> targetGuids, StandardMarker marker, IPackState packState)
			: base(marker)
		{
			_packState = packState;
			TargetGuids = targetGuids.ToList();
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			IAttribute valueAttr;
			return new ResetGuidModifier(attributes.TryGetAttribute("resetguid", out valueAttr) ? valueAttr.GetValueAsGuids() : Enumerable.Empty<Guid>(), marker, packState);
		}

		public void Interact(bool autoTriggered)
		{
			if (_pathingEntity.BehaviorFiltered)
			{
				return;
			}
			foreach (Guid guid in TargetGuids)
			{
				_packState.BehaviorStates.ClearHiddenBehavior(guid);
			}
		}

		public void Focus()
		{
			_packState.UiStates.Interact.ShowInteract(_pathingEntity, string.Format("Will unhide {0} marker{1} {{0}}", TargetGuids.Count, (TargetGuids.Count == 1) ? "" : "s"));
		}

		public void Unfocus()
		{
			_packState.UiStates.Interact.DisconnectInteract(_pathingEntity);
		}
	}
}
