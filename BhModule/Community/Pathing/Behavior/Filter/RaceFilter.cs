using System;
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
	public class RaceFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "race";

		private bool _isFiltered;

		private ObservableCollection<RaceType> AllowedRaces { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public RaceFilter(IEnumerable<RaceType> allowedRaces)
			: this(allowedRaces.ToArray())
		{
		}

		public RaceFilter(params RaceType[] allowedRaces)
		{
			AllowedRaces = new ObservableCollection<RaceType>(allowedRaces);
			AllowedRaces.CollectionChanged += AllowedRacesOnCollectionChanged;
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacterOnNameChanged);
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = AllowedRaces.All((RaceType r) => r != GameService.Gw2Mumble.get_PlayerCharacter().get_Race());
		}

		private void PlayerCharacterOnNameChanged(object sender, ValueEventArgs<string> e)
		{
			UpdateFiltered();
		}

		private void AllowedRacesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("race", out var attribute))
			{
				return null;
			}
			return new RaceFilter(attribute.GetValueAsEnums<RaceType>());
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
