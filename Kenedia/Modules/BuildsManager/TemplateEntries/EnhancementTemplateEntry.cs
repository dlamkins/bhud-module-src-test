using System;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class EnhancementTemplateEntry : TemplateEntry, IDisposable
	{
		private bool _isDisposed;

		private Enhancement _enhancement;

		public Enhancement Enhancement
		{
			get
			{
				return _enhancement;
			}
			private set
			{
				Common.SetProperty(ref _enhancement, value);
			}
		}

		public EnhancementTemplateEntry(TemplateSlotType slot)
			: base(slot)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Enhancement = null;
				return;
			}
			Enhancement enhancement = e.NewValue as Enhancement;
			if (enhancement != null)
			{
				Enhancement = enhancement;
			}
		}

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[1] { Enhancement?.MappedId ?? 0 }).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 1;
			if (array2 != null && array2.Length != 0)
			{
				Enhancement = BuildsManager.Data.Enhancements.Items.Values.Where((Enhancement e) => e.MappedId == array2[0]).FirstOrDefault();
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
				Enhancement = null;
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
				Enhancement enhancement = obj as Enhancement;
				if (enhancement != null)
				{
					base.Item = enhancement;
					return true;
				}
			}
			return false;
		}
	}
}
