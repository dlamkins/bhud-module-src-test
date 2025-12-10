using System;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PowerCoreTemplateEntry : TemplateEntry, IDisposable
	{
		private bool _isDisposed;

		public PowerCore PowerCore
		{
			[CompilerGenerated]
			get
			{
				return _003CPowerCore_003Ek__BackingField;
			}
			private set
			{
				Common.SetProperty(ref _003CPowerCore_003Ek__BackingField, value);
			}
		}

		public PowerCoreTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
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
