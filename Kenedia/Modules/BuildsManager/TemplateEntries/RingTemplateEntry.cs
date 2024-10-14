using System;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Stats;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class RingTemplateEntry : TemplateEntry, IDisposable, ITripleInfusionTemplateEntry, IDoubleInfusionTemplateEntry, ISingleInfusionTemplateEntry, IStatTemplateEntry
	{
		private bool _isDisposed;

		private Infusion _infusion1;

		private Infusion _infusion2;

		private Infusion _infusion3;

		private Stat _stat;

		public Trinket Ring { get; private set; }

		public Infusion Infusion1
		{
			get
			{
				return _infusion1;
			}
			private set
			{
				Common.SetProperty(ref _infusion1, value);
			}
		}

		public Infusion Infusion2
		{
			get
			{
				return _infusion2;
			}
			private set
			{
				Common.SetProperty(ref _infusion2, value);
			}
		}

		public Infusion Infusion3
		{
			get
			{
				return _infusion3;
			}
			private set
			{
				Common.SetProperty(ref _infusion3, value);
			}
		}

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

		public RingTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			Trinket ring = default(Trinket);
			Ring = ((base.Data?.Trinkets.TryGetValue(91234, out ring) ?? false) ? ring : null);
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Ring = null;
				return;
			}
			Trinket trinket = e.NewValue as Trinket;
			if (trinket != null)
			{
				Ring = trinket;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Ring = null;
				Infusion1 = null;
				Infusion2 = null;
				Infusion3 = null;
				Stat = null;
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
			case TemplateSubSlotType.Infusion1:
			{
				if (obj?.Equals(Infusion1) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Infusion1 = null;
					return true;
				}
				Infusion infusion2 = obj as Infusion;
				if (infusion2 != null)
				{
					Infusion1 = infusion2;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.Infusion2:
			{
				if (obj?.Equals(Infusion2) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Infusion2 = null;
					return true;
				}
				Infusion infusion3 = obj as Infusion;
				if (infusion3 != null)
				{
					Infusion2 = infusion3;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.Infusion3:
			{
				if (obj?.Equals(Infusion3) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Infusion3 = null;
					return true;
				}
				Infusion infusion = obj as Infusion;
				if (infusion != null)
				{
					Infusion3 = infusion;
					return true;
				}
				break;
			}
			}
			return false;
		}
	}
}
