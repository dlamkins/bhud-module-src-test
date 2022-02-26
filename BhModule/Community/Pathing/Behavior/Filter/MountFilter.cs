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
	public class MountFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "mount";

		private bool _isFiltered;

		private ObservableCollection<MountType> AllowedMounts { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public MountFilter(IEnumerable<MountType> allowedMounts)
			: this(allowedMounts.ToArray())
		{
		}

		public MountFilter(params MountType[] allowedMounts)
		{
			AllowedMounts = new ObservableCollection<MountType>(allowedMounts);
			AllowedMounts.CollectionChanged += AllowedMountsOnCollectionChanged;
			GameService.Gw2Mumble.get_PlayerCharacter().add_CurrentMountChanged((EventHandler<ValueEventArgs<MountType>>)PlayerCharacterOnCurrentMountChanged);
			UpdateFiltered();
		}

		private void PlayerCharacterOnCurrentMountChanged(object sender, ValueEventArgs<MountType> e)
		{
			UpdateFiltered();
		}

		private void AllowedMountsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = AllowedMounts.All((MountType m) => m != GameService.Gw2Mumble.get_PlayerCharacter().get_CurrentMount());
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("mount", out var attribute))
			{
				return null;
			}
			return new MountFilter(attribute.GetValueAsEnums<MountType>());
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
