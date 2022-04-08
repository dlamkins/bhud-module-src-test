using System.Threading.Tasks;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class CopyModifier : Behavior<StandardMarker>, ICanInteract
	{
		public const string PRIMARY_ATTR_NAME = "copy";

		private const string ATTR_MESSAGE = "copy-message";

		private const string DEFAULT_COPYMESSAGE = "'{0}' copied to clipboard.";

		private readonly IPackState _packState;

		private double _lastTrigger;

		public string CopyValue { get; set; }

		public string CopyMessage { get; set; }

		public CopyModifier(string value, string message, StandardMarker marker, IPackState packState)
			: base(marker)
		{
			_packState = packState;
			CopyValue = value;
			CopyMessage = message;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			IAttribute valueAttr;
			IAttribute messageAttr;
			return new CopyModifier(attributes.TryGetAttribute("copy", out valueAttr) ? valueAttr.GetValueAsString() : "", attributes.TryGetAttribute("copy-message", out messageAttr) ? messageAttr.GetValueAsString() : "'{0}' copied to clipboard.", marker, packState);
		}

		public void Interact(bool autoTriggered)
		{
			if (_packState.UserConfiguration.PackMarkerConsentToClipboard.get_Value() == MarkerClipboardConsentLevel.Never || (_packState.UserConfiguration.PackMarkerConsentToClipboard.get_Value() == MarkerClipboardConsentLevel.OnlyWhenInteractedWith && autoTriggered) || _pathingEntity.BehaviorFiltered || (autoTriggered && GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds - _lastTrigger < _packState.UserResourceStates.Advanced.CopyAttributeRechargeMs))
			{
				return;
			}
			_lastTrigger = GameService.Overlay.get_CurrentGameTime().get_TotalGameTime().TotalMilliseconds;
			ClipboardUtil.get_WindowsClipboardService().SetTextAsync(CopyValue).ContinueWith(delegate(Task<bool> t)
			{
				if (t.IsCompleted && t.Result)
				{
					ScreenNotification.ShowNotification(string.Format(CopyMessage, CopyValue), (NotificationType)0, (Texture2D)null, 2);
				}
			});
		}
	}
}
