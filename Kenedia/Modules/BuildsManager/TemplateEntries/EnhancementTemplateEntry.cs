using System;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
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

		public EnhancementTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
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
