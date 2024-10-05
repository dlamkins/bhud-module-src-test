using System;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PvpRelicTemplateEntry : TemplateEntry, IDisposable
	{
		private bool _isDisposed;

		private Relic _relic;

		public Relic Relic
		{
			get
			{
				return _relic;
			}
			private set
			{
				Common.SetProperty(ref _relic, value);
			}
		}

		public PvpRelicTemplateEntry(TemplateSlotType slot)
			: base(slot)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				Relic = null;
				return;
			}
			Relic relic = e.NewValue as Relic;
			if (relic != null)
			{
				Relic = relic;
			}
		}

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[1] { Relic?.MappedId ?? 0 }).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 1;
			if (array2 != null && array2.Length != 0)
			{
				Relic = BuildsManager.Data.PvpRelics.Values.Where((Relic e) => e.MappedId == array2[0]).FirstOrDefault();
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
				Relic = null;
			}
		}

		public override bool SetValue(TemplateSlotType slot, TemplateSubSlotType subSlot, object? obj)
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
				Relic relic = obj as Relic;
				if (relic != null)
				{
					base.Item = relic;
					return true;
				}
			}
			return false;
		}
	}
}
