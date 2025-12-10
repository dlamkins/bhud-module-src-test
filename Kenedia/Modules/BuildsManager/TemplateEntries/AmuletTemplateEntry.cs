using System;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class AmuletTemplateEntry : TemplateEntry, IDisposable, IEnrichmentTemplateEntry, IStatTemplateEntry
	{
		private bool _isDisposed;

		public Trinket Amulet { get; private set; }

		public Stat Stat
		{
			[CompilerGenerated]
			get
			{
				return _003CStat_003Ek__BackingField;
			}
			private set
			{
				Common.SetProperty(ref _003CStat_003Ek__BackingField, value);
			}
		}

		public Enrichment Enrichment
		{
			[CompilerGenerated]
			get
			{
				return _003CEnrichment_003Ek__BackingField;
			}
			private set
			{
				Common.SetProperty(ref _003CEnrichment_003Ek__BackingField, value);
			}
		}

		public event EventHandler<ValueChangedEventArgs<Enrichment>> EnrichmentChanged;

		public event EventHandler<ValueChangedEventArgs<Stat>> StatChanged;

		public AmuletTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			Trinket accessoire = default(Trinket);
			Amulet = ((base.Data?.Trinkets?.TryGetValue(92991, out accessoire) ?? false) ? accessoire : null);
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
