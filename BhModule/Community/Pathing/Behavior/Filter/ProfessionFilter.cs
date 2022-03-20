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
	public class ProfessionFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "profession";

		private bool _isFiltered;

		private ObservableCollection<ProfessionType> AllowedProfessions { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public ProfessionFilter(IEnumerable<ProfessionType> allowedProfessions)
			: this(allowedProfessions.ToArray())
		{
		}

		public ProfessionFilter(params ProfessionType[] allowedProfessions)
		{
			AllowedProfessions = new ObservableCollection<ProfessionType>(allowedProfessions);
			AllowedProfessions.CollectionChanged += AllowedProfessionsOnCollectionChanged;
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)PlayerCharacterOnNameChanged);
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = AllowedProfessions.All((ProfessionType p) => p != GameService.Gw2Mumble.get_PlayerCharacter().get_Profession());
		}

		private void PlayerCharacterOnNameChanged(object sender, ValueEventArgs<string> e)
		{
			UpdateFiltered();
		}

		private void AllowedProfessionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("profession", out var attribute))
			{
				return null;
			}
			return new ProfessionFilter(attribute.GetValueAsEnums<ProfessionType>());
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
