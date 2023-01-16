using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class InfoModifier : Behavior<StandardMarker>, ICanFocus
	{
		public const string PRIMARY_ATTR_NAME = "info";

		private const string ATTR_RANGE = "inforange";

		private const float DEFAULT_INFORANGE = 2f;

		private readonly IPackState _packState;

		private string _infoValue;

		private bool _inFocus;

		public string InfoValue
		{
			get
			{
				return _infoValue;
			}
			set
			{
				if (!string.Equals(_infoValue, value))
				{
					_packState.UiStates.RemoveInfoString(_infoValue);
					_infoValue = value;
					if (_inFocus)
					{
						Focus();
					}
				}
			}
		}

		public InfoModifier(StandardMarker pathingEntity, string value, float range, IPackState packState)
			: base(pathingEntity)
		{
			_packState = packState;
			InfoValue = value;
			if (pathingEntity.TriggerRange == _packState.UserResourceStates.Population.MarkerPopulationDefaults.TriggerRange)
			{
				pathingEntity.TriggerRange = range;
			}
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			IAttribute valueAttr;
			IAttribute rangeAttr;
			return new InfoModifier(marker, attributes.TryGetAttribute("info", out valueAttr) ? valueAttr.GetValueAsString() : "", attributes.TryGetAttribute("inforange", out rangeAttr) ? rangeAttr.GetValueAsFloat(2f) : 2f, packState);
		}

		public void Focus()
		{
			if (!_pathingEntity.BehaviorFiltered)
			{
				_inFocus = true;
				_packState.UiStates.AddInfoString(InfoValue);
			}
		}

		public void Unfocus()
		{
			_inFocus = false;
			_packState.UiStates.RemoveInfoString(InfoValue);
		}

		public override void Unload()
		{
			_packState.UiStates.RemoveInfoString(InfoValue);
		}
	}
}
