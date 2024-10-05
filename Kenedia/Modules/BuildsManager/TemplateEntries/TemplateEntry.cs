using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public abstract class TemplateEntry : IItemTemplateEntry
	{
		private BaseItem _item;

		public TemplateSlotType Slot { get; }

		public BaseItem Item
		{
			get
			{
				return _item;
			}
			set
			{
				Common.SetProperty(ref _item, value, new ValueChangedEventHandler<BaseItem>(OnItemChanged));
			}
		}

		public TemplateEntry(TemplateSlotType slot)
		{
			Slot = slot;
		}

		protected virtual void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
		}

		public abstract byte[] AddToCodeArray(byte[] array);

		public abstract byte[] GetFromCodeArray(byte[] array);

		public abstract bool SetValue(TemplateSlotType slot, TemplateSubSlotType subSlot, object? obj);
	}
}
