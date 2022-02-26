using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BhModule.Community.Pathing.Utility;
using Blish_HUD;
using Blish_HUD.Contexts;
using Microsoft.Xna.Framework;
using TmfLib.Prototype;

namespace BhModule.Community.Pathing.Behavior.Filter
{
	public class FestivalFilter : IBehavior, IUpdatable, ICanFilter
	{
		public const string PRIMARY_ATTR_NAME = "festival";

		private bool _isFiltered;

		public ObservableCollection<Festival> AllowedFestivals { get; }

		public bool IsFiltered()
		{
			return _isFiltered;
		}

		public FestivalFilter(IEnumerable<Festival> allowedFestivals)
			: this(allowedFestivals.ToArray())
		{
		}

		public FestivalFilter(params Festival[] allowedFestivals)
		{
			AllowedFestivals = new ObservableCollection<Festival>(allowedFestivals);
			AllowedFestivals.CollectionChanged += AllowedFestivalsOnCollectionChanged;
			UpdateFiltered();
		}

		private void UpdateFiltered()
		{
			_isFiltered = !AllowedFestivals.Any((Festival f) => ((Festival)(ref f)).IsActive());
		}

		private void AllowedFestivalsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateFiltered();
		}

		public static IBehavior BuildFromAttributes(AttributeCollection attributes)
		{
			if (!attributes.TryGetAttribute("festival", out var attribute))
			{
				return null;
			}
			return new FestivalFilter(attribute.GetValueAsStrings().Select((Func<string, Festival>)Festival.FromName));
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Unload()
		{
		}
	}
}
