using System;
using System.Collections.Generic;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class ItemMapping : IDisposable
	{
		private bool _isDisposed;

		public List<BasicItemMap> Nourishments = new List<BasicItemMap>();

		public List<BasicItemMap> Utilities = new List<BasicItemMap>();

		public List<BasicItemMap> PveRunes = new List<BasicItemMap>();

		public List<BasicItemMap> PvpRunes = new List<BasicItemMap>();

		public List<BasicItemMap> PveSigils = new List<BasicItemMap>();

		public List<BasicItemMap> PvpSigils = new List<BasicItemMap>();

		public List<BasicItemMap> Infusions = new List<BasicItemMap>();

		public List<BasicItemMap> Enrichments = new List<BasicItemMap>();

		public List<BasicItemMap> Trinkets = new List<BasicItemMap>();

		public List<BasicItemMap> Backs = new List<BasicItemMap>();

		public List<BasicItemMap> Weapons = new List<BasicItemMap>();

		public List<BasicItemMap> Armors = new List<BasicItemMap>();

		public List<BasicItemMap> PowerCores = new List<BasicItemMap>();

		public List<BasicItemMap> Relics = new List<BasicItemMap>();

		public List<BasicItemMap> PvpAmulets = new List<BasicItemMap>();

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Nourishments.Clear();
				Utilities.Clear();
				PveRunes.Clear();
				PvpRunes.Clear();
				PveSigils.Clear();
				PvpSigils.Clear();
				Infusions.Clear();
				Enrichments.Clear();
				Trinkets.Clear();
				Backs.Clear();
				Weapons.Clear();
				Armors.Clear();
				PowerCores.Clear();
				PvpAmulets.Clear();
			}
		}
	}
}
