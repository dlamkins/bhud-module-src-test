using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Gw2Sharp.Models;
using Microsoft.Xna.Framework;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class MapTypeFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "maptype";

		private bool _isFiltered;

		private ObservableCollection<MapType> AllowedMapTypes { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public MapTypeFilter(IEnumerable<MapType> allowedMapTypes)
			: this(allowedMapTypes.ToArray())
		{
		}

		public MapTypeFilter(params MapType[] allowedMapTypes)
		{
			AllowedMapTypes = new ObservableCollection<MapType>(allowedMapTypes);
			AllowedMapTypes.CollectionChanged += AllowedMapTypesOnCollectionChanged;
			UpdateFiltered();
		}

		private void AllowedMapTypesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = AllowedMapTypes.All((MapType m) => m != GameService.Gw2Mumble.get_CurrentMap().get_Type());
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("maptype", out var attribute))
			{
				return null;
			}
			return new MapTypeFilter(attribute.GetValueAsEnums<MapType>());
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
