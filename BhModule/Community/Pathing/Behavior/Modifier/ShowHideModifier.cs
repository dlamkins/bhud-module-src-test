using System;
using BhModule.Community.Pathing.Entity;
using BhModule.Community.Pathing.State;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using TmfLib.Pathable;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Modifier
{
	public class ShowHideModifier : Behavior<StandardMarker>, ICanInteract, ICanFocus
	{
		private static readonly Logger Logger = Logger.GetLogger<ShowHideModifier>();

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
				string attrValue2 = showAttr.GetValueAsString();
				if (!string.IsNullOrWhiteSpace(attrValue2))
				{
					PathingCategory category2 = null;
					try
					{
						category2 = packState.RootCategory.GetOrAddCategoryFromNamespace(attrValue2);
					}
					catch (Exception e2)
					{
						Logger.Warn(e2, "Failed to load " + showAttr.Name + "=\"" + attrValue2 + "\".");
					}
					if (category2 != null)
					{
						return new ShowHideModifier(category2, showOnInteract: true, marker, packState);
					}
				}
			}
			if (attributes.TryGetAttribute("hide", out var hideAttr))
			{
				string attrValue = hideAttr.GetValueAsString();
				if (!string.IsNullOrWhiteSpace(attrValue))
				{
					PathingCategory category = null;
					try
					{
						category = packState.RootCategory.GetOrAddCategoryFromNamespace(attrValue);
					}
					catch (Exception e)
					{
						Logger.Warn(e, "Failed to load " + hideAttr.Name + "=\"" + attrValue + "\".");
					}
					if (category != null)
					{
						return new ShowHideModifier(category, showOnInteract: false, marker, packState);
					}
				}
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
