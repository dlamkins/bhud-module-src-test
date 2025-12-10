using System;
using System.Runtime.CompilerServices;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PvpAmuletTemplateEntry : TemplateEntry, IDisposable, IRuneTemplateEntry
	{
		private bool _isDisposed;

		public PvpAmulet PvpAmulet
		{
			[CompilerGenerated]
			get
			{
				return _003CPvpAmulet_003Ek__BackingField;
			}
			private set
			{
				Common.SetProperty(ref _003CPvpAmulet_003Ek__BackingField, value);
			}
		}

		public Rune Rune
		{
			[CompilerGenerated]
			get
			{
				return _003CRune_003Ek__BackingField;
			}
			private set
			{
				Common.SetProperty(ref _003CRune_003Ek__BackingField, value);
			}
		}

		public PvpAmuletTemplateEntry(TemplateSlotType slot, Data data)
			: base(slot, data)
		{
		}

		protected override void OnItemChanged(object sender, ValueChangedEventArgs<BaseItem> e)
		{
			base.OnItemChanged(sender, e);
			if (e.NewValue == null)
			{
				PvpAmulet = null;
				return;
			}
			PvpAmulet pvpAmulet = e.NewValue as PvpAmulet;
			if (pvpAmulet != null)
			{
				PvpAmulet = pvpAmulet;
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				PvpAmulet = null;
				Rune = null;
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
				PvpAmulet pvpAmulet = obj as PvpAmulet;
				if (pvpAmulet != null)
				{
					base.Item = pvpAmulet;
					return true;
				}
				break;
			}
			case TemplateSubSlotType.Rune:
			{
				if (obj?.Equals(Rune) ?? false)
				{
					return false;
				}
				if (obj == null)
				{
					Rune = null;
					return true;
				}
				Rune rune = obj as Rune;
				if (rune != null)
				{
					Rune = rune;
					return true;
				}
				break;
			}
			}
			return false;
		}
	}
}
