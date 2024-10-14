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
	public class WeaponTemplateEntry : TemplateEntry, IDisposable, IWeaponTemplateEntry, IStatTemplateEntry, ISingleSigilTemplateEntry, IPvpSigilTemplateEntry, ISingleInfusionTemplateEntry
	{
		private bool _isDisposed;

		private Weapon _weapon;

		private Sigil _sigil1;

		private Sigil _pvpSigil;

		private Infusion _infusion1;

		private Stat _stat;

		private WeaponTemplateEntry _pairedWeapon;

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

		public Sigil PvpSigil
		{
			get
			{
				return _pvpSigil;
			}
			private set
			{
				Common.SetProperty(ref _pvpSigil, value);
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

		public WeaponTemplateEntry PairedWeapon
		{
			get
			{
				return _pairedWeapon;
			}
			set
			{
				Common.SetProperty(ref _pairedWeapon, value);
			}
		}

		public WeaponTemplateEntry(TemplateSlotType slot, Data data)
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
				Sigil1 = null;
				PvpSigil = null;
				Infusion1 = null;
				Stat = null;
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
				Sigil sigil2 = obj as Sigil;
				if (sigil2 != null)
				{
					Sigil1 = sigil2;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.PvpSigil:
			{
				if (obj?.Equals(PvpSigil) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					PvpSigil = null;
					return true;
				}
				Sigil sigil = obj as Sigil;
				if (sigil != null)
				{
					PvpSigil = sigil;
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
