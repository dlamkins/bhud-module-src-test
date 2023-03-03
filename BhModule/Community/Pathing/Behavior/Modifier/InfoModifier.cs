using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class InfoModifier : Behavior<StandardMarker>, ICanFocus
	{
		public const string PRIMARY_ATTR_NAME = "info";

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

		public InfoModifier(StandardMarker pathingEntity, string value, IPackState packState)
			: base(pathingEntity)
		{
			_packState = packState;
			InfoValue = value;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			IAttribute valueAttr;
			return new InfoModifier(marker, attributes.TryGetAttribute("info", out valueAttr) ? valueAttr.GetValueAsString() : "", packState);
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
