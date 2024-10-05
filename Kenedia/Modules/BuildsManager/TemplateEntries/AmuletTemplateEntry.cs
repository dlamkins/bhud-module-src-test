using System;
using System.Collections.Generic;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class AmuletTemplateEntry : TemplateEntry, IDisposable, IEnrichmentTemplateEntry, IStatTemplateEntry
	{
		private bool _isDisposed;

		private Stat _stat;

		private Enrichment _enrichment;

		public Trinket Amulet { get; private set; }

		public Stat Stat
		{
			get
			{
				return _stat;
			}
			private set
			{
				Common.SetProperty(ref _stat, value);
			}
		}

		public Enrichment Enrichment
		{
			get
			{
				return _enrichment;
			}
			private set
			{
				Common.SetProperty(ref _enrichment, value);
			}
		}

		public event EventHandler<ValueChangedEventArgs<Enrichment>> EnrichmentChanged;

		public event EventHandler<ValueChangedEventArgs<Stat>> StatChanged;

		public AmuletTemplateEntry(TemplateSlotType slot)
		{
			Trinket accessoire = default(Trinket);
			Amulet = ((BuildsManager.Data?.Trinkets?.TryGetValue(92991, out accessoire) ?? false) ? accessoire : null);
			base._002Ector(slot);
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Amulet = null;
				return;
			}
			Trinket trinket = e.NewValue as Trinket;
			if (trinket != null)
			{
				Amulet = trinket;
			}
		}

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[2]
			{
				Stat?.MappedId ?? 0,
				Enrichment?.MappedId ?? 0
			}).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 2;
			if (array2 != null && array2.Length != 0)
			{
				Stat = BuildsManager.Data.Stats.Items.Where((KeyValuePair<int, Stat> e) => e.Value.MappedId == array2[0]).FirstOrDefault().Value;
				Enrichment = BuildsManager.Data.Enrichments.Items.Where<KeyValuePair<int, Enrichment>>((KeyValuePair<int, Enrichment> e) => e.Value.MappedId == array2[1]).FirstOrDefault().Value;
			}
			if (array2 == null || array2.Length == 0)
			{
				return array2;
			}
			return GearTemplateCode.RemoveFromStart(array2, newStartIndex);
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Stat = null;
				Enrichment = null;
				Amulet = null;
			}
		}

		public override bool SetValue(TemplateSlotType slot, TemplateSubSlotType subSlot, object obj)
		{
			switch (subSlot)
			{
			case TemplateSubSlotType.Stat:
			{
				if (obj?.Equals(Stat) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Stat = null;
					return true;
				}
				Stat stat = obj as Stat;
				if (stat != null)
				{
					Stat = stat;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.Enrichment:
			{
				if (obj?.Equals(Enrichment) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Enrichment = null;
					return true;
				}
				Enrichment enrichment = obj as Enrichment;
				if (enrichment != null)
				{
					Enrichment = enrichment;
					return true;
				}
				break;
			}
			}
			return false;
		}
	}
}
