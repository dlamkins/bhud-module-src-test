using System;
using Kenedia.Modules.BuildsManager.Models.Templates;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class TemplateSlotChangedEventArgs : EventArgs
	{
		public TemplateSlotType Slot { get; }

		public TemplateSubSlotType SubSlotType { get; }

		public object Value { get; }

		public TemplateSlotChangedEventArgs(TemplateSlotType slot, TemplateSubSlotType subSlotType, object value)
		{
			Slot = slot;
			SubSlotType = subSlotType;
			Value = value;
		}
	}
}
