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
	public class ArmorTemplateEntry : TemplateEntry, IDisposable, IStatTemplateEntry, IRuneTemplateEntry, ISingleInfusionTemplateEntry, IArmorTemplateEntry
	{
		private bool _isDisposed;

		private Stat _stat;

		private Infusion _infusion1;

		private Rune _rune;

		private Armor _armor;

		public Armor Armor
		{
			get
			{
				return _armor;
			}
			private set
			{
				Common.SetProperty(ref _armor, value);
			}
		}

		public Rune Rune
		{
			get
			{
				return _rune;
			}
			private set
			{
				Common.SetProperty(ref _rune, value);
			}
		}

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

		public ArmorTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Armor = null;
				return;
			}
			Armor armor = e.NewValue as Armor;
			if (armor != null)
			{
				Armor = armor;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Stat = null;
				Rune = null;
				Infusion1 = null;
				Armor = null;
			}
		}

		public override bool SetValue(TemplateSlotType slot, TemplateSubSlotType subSlot, object obj)
		{
			switch (subSlot)
			{
			case TemplateSubSlotType.Item:
			{
				if (obj?.Equals(base.Item) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					base.Item = null;
					return true;
				}
				Armor armor = obj as Armor;
				if (armor != null)
				{
					base.Item = armor;
					return true;
				}
				break;
			}
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
			case TemplateSubSlotType.Rune:
			{
				if (obj?.Equals(Rune) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Rune = null;
					return true;
				}
				Rune rune = obj as Rune;
				if (rune != null)
				{
					Rune = rune;
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
