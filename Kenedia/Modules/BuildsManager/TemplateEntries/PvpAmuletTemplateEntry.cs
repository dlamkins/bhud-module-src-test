using System;
using System.Collections.Generic;
using System.Linq;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.BuildsManager.Models.Templates;
using Kenedia.Modules.BuildsManager.Utility;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.BuildsManager.TemplateEntries
{
	public class PvpAmuletTemplateEntry : TemplateEntry, IDisposable, IRuneTemplateEntry
	{
		private bool _isDisposed;

		private PvpAmulet _pvpAmulet;

		private Rune _rune;

		public PvpAmulet PvpAmulet
		{
			get
			{
				return _pvpAmulet;
			}
			private set
			{
				Common.SetProperty(ref _pvpAmulet, value);
			}
		}

		public Rune Rune
		{
			get
			{
				return _rune;
			}
			private set
			{
				Common.SetProperty(ref _rune, value);
			}
		}

		public PvpAmuletTemplateEntry(TemplateSlotType slot)
			: base(slot)
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

		public override byte[] AddToCodeArray(byte[] array)
		{
			return array.Concat(new byte[2]
			{
				PvpAmulet?.MappedId ?? 0,
				Rune?.MappedId ?? 0
			}).ToArray();
		}

		public override byte[] GetFromCodeArray(byte[] array)
		{
			byte[] array2 = array;
			int newStartIndex = 2;
			if (array2 != null && array2.Length != 0)
			{
				PvpAmulet = BuildsManager.Data.PvpAmulets.Items.Values.Where((PvpAmulet e) => e.MappedId == array2[0]).FirstOrDefault();
				Rune = BuildsManager.Data.PvpRunes.Items.Where<KeyValuePair<int, Rune>>((KeyValuePair<int, Rune> e) => e.Value.MappedId == array2[1]).FirstOrDefault().Value;
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
