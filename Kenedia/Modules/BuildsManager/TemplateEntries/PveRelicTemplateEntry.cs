using System;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PveRelicTemplateEntry : TemplateEntry, IDisposable
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

		public PveRelicTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
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
