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
	public class AccessoireTemplateEntry : TemplateEntry, IDisposable, ISingleInfusionTemplateEntry, IStatTemplateEntry
	{
		private bool _isDisposed;

		private Stat _stat;

		private Infusion _infusion;

		public Trinket Accessoire { get; private set; }

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

		public Infusion Infusion1
		{
			get
			{
				return _infusion;
			}
			private set
			{
				Common.SetProperty(ref _infusion, value);
			}
		}

		public AccessoireTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnDataLoaded()
		{
			base.OnDataLoaded();
			Trinket accessoire = default(Trinket);
			Accessoire = ((base.Data?.Trinkets.TryGetValue(81908, out accessoire) ?? false) ? accessoire : null);
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Accessoire = null;
				return;
			}
			Trinket trinket = e.NewValue as Trinket;
			if (trinket != null)
			{
				Accessoire = trinket;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Stat = null;
				Infusion1 = null;
				Accessoire = null;
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
				Infusion infusion = obj as Infusion;
				if (infusion != null)
				{
					Infusion1 = infusion;
					return true;
				}
				break;
			}
			}
			return false;
		}
	}
}
