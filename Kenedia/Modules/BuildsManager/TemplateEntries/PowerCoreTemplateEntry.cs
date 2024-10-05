using System;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PowerCoreTemplateEntry : TemplateEntry, IDisposable
	{
		private bool _isDisposed;

		private PowerCore _powerCore;

		public PowerCore PowerCore
		{
			get
			{
				return _powerCore;
			}
			private set
			{
				Common.SetProperty(ref _powerCore, value);
			}
		}

		public PowerCoreTemplateEntry(TemplateSlotType slot)
			: base(slot)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				PowerCore = null;
				return;
			}
			PowerCore powerCore = e.NewValue as PowerCore;
			if (powerCore != null)
			{
				PowerCore = powerCore;
			}
		}

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[1] { PowerCore?.MappedId ?? 0 }).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 1;
			if (array2 != null && array2.Length != 0)
			{
				PowerCore = BuildsManager.Data.PowerCores.Values.Where((PowerCore e) => e.MappedId == array2[0]).FirstOrDefault();
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
				PowerCore = null;
			}
		}

		public override bool SetValue(TemplateSlotType slot, TemplateSubSlotType subSlot, object obj)
		{
			if (subSlot == TemplateSubSlotType.Item)
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
				PowerCore powerCore = obj as PowerCore;
				if (powerCore != null)
				{
					base.Item = powerCore;
					return true;
				}
			}
			return false;
		}
	}
}
