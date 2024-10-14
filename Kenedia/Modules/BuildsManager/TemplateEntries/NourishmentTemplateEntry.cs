using System;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
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

		public NourishmentTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
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
