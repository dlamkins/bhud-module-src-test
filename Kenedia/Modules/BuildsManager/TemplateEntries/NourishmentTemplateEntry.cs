using System;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class NourishmentTemplateEntry : TemplateEntry, IDisposable
	{
		private bool _isDisposed;

		private Nourishment _nourishment;

		public Nourishment Nourishment
		{
			get
			{
				return _nourishment;
			}
			private set
			{
				Common.SetProperty(ref _nourishment, value);
			}
		}

		public NourishmentTemplateEntry(TemplateSlotType slot)
			: base(slot)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Nourishment = null;
				return;
			}
			Nourishment nourishment = e.NewValue as Nourishment;
			if (nourishment != null)
			{
				Nourishment = nourishment;
			}
		}

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[1] { Nourishment?.MappedId ?? 0 }).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 1;
			if (array2 != null && array2.Length != 0)
			{
				Nourishment = BuildsManager.Data.Nourishments.Values.Where((Nourishment e) => e.MappedId == array2[0]).FirstOrDefault();
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
				Nourishment = null;
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
				Nourishment nourishment = obj as Nourishment;
				if (nourishment != null)
				{
					base.Item = nourishment;
					return true;
				}
			}
			return false;
		}
	}
}
