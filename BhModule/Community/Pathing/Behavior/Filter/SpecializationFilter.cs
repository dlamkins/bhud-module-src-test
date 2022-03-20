using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Microsoft.Xna.Framework;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class SpecializationFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "specialization";

		private bool _isFiltered;

		private ObservableCollection<int> AllowedSpecializations { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public SpecializationFilter(IEnumerable<int> allowedSpecializations)
			: this(allowedSpecializations.ToArray())
		{
		}

		public SpecializationFilter(params int[] allowedSpecializations)
		{
			AllowedSpecializations = new ObservableCollection<int>(allowedSpecializations);
			AllowedSpecializations.CollectionChanged += AllowedSpecializationsOnCollectionChanged;
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)PlayerCharacterOnSpecializationChanged);
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = AllowedSpecializations.All((int p) => p != GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization());
		}

		private void PlayerCharacterOnSpecializationChanged(object sender, ValueEventArgs<int> e)
		{
			UpdateFiltered();
		}

		private void AllowedSpecializationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("specialization", out var attribute))
			{
				return null;
			}
			return new SpecializationFilter(attribute.GetValueAsInts());
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
