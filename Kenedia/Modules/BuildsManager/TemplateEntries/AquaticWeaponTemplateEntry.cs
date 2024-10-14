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
	public class AquaticWeaponTemplateEntry : TemplateEntry, IDisposable, IStatTemplateEntry, IDoubleSigilTemplateEntry, ISingleSigilTemplateEntry, IDoubleInfusionTemplateEntry, ISingleInfusionTemplateEntry, IWeaponTemplateEntry
	{
		private bool _isDisposed;

		private Weapon _weapon;

		private Sigil _sigil1;

		private Sigil _sigil2;

		private Infusion _infusion1;

		private Infusion _infusion2;

		private Stat _stat;

		public Weapon Weapon
		{
			get
			{
				return _weapon;
			}
			private set
			{
				Common.SetProperty(ref _weapon, value);
			}
		}

		public Sigil Sigil1
		{
			get
			{
				return _sigil1;
			}
			private set
			{
				Common.SetProperty(ref _sigil1, value);
			}
		}

		public Sigil Sigil2
		{
			get
			{
				return _sigil2;
			}
			private set
			{
				Common.SetProperty(ref _sigil2, value);
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

		public AquaticWeaponTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Weapon = null;
				return;
			}
			Weapon weapon = e.NewValue as Weapon;
			if (weapon != null)
			{
				Weapon = weapon;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Weapon = null;
				Stat = null;
				Sigil1 = null;
				Sigil2 = null;
				Infusion1 = null;
				Infusion2 = null;
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
				Weapon weapon = obj as Weapon;
				if (weapon != null)
				{
					base.Item = weapon;
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
			case TemplateSubSlotType.Sigil1:
			{
				if (obj?.Equals(Sigil1) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Sigil1 = null;
					return true;
				}
				Sigil sigil = obj as Sigil;
				if (sigil != null)
				{
					Sigil1 = sigil;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.Sigil2:
			{
				if (obj?.Equals(Sigil2) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Sigil2 = null;
					return true;
				}
				Sigil sigil2 = obj as Sigil;
				if (sigil2 != null)
				{
					Sigil2 = sigil2;
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
				Infusion infusion = obj as Infusion;
				if (infusion != null)
				{
					Infusion2 = infusion;
					return true;
				}
				break;
			}
			}
			return false;
		}
	}
}
