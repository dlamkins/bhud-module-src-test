using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class ShowHideModifier : Behavior<StandardMarker>, ICanInteract, ICanFocus
	{
		public const string SHOW_PRIMARY_ATTR_NAME = "show";

		public const string HIDE_PRIMARY_ATTR_NAME = "hide";

		private readonly IPackState _packState;

		public PathingCategory Category { get; set; }

		public bool ShowOnInteract { get; set; }

		public ShowHideModifier(PathingCategory category, bool showOnInteract, StandardMarker marker, IPackState packState)
			: base(marker)
		{
			_packState = packState;
			Category = category;
			ShowOnInteract = showOnInteract;
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes, StandardMarker marker, IPackState packState)
		{
			if (attributes.TryGetAttribute("show", out var showAttr))
			{
				return new ShowHideModifier(packState.RootCategory.GetOrAddCategoryFromNamespace(showAttr.GetValueAsString()), showOnInteract: true, marker, packState);
			}
			if (attributes.TryGetAttribute("hide", out var hideAttr))
			{
				return new ShowHideModifier(packState.RootCategory.GetOrAddCategoryFromNamespace(hideAttr.GetValueAsString()), showOnInteract: false, marker, packState);
			}
			return null;
		}

		public void Interact(bool autoTriggered)
		{
			if (!_pathingEntity.BehaviorFiltered)
			{
				_packState.CategoryStates.SetInactive(Category, !ShowOnInteract);
			}
		}

		public void Focus()
		{
			_packState.UiStates.Interact.ShowInteract(_pathingEntity, (ShowOnInteract ? "Show" : "Hide") + " '" + Category.Namespace + "' category {0}");
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