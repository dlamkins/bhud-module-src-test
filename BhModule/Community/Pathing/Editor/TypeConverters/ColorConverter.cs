using System;
using System.ComponentModel;
using System.Globalization;
using Blish_HUD;
using Microsoft.Xna.Framework;

namespace BhModule.Community.Pathing.Editor.TypeConverters
{
	public class ColorConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (!(sourceType == typeof(string)))
			{
				return base.CanConvertFrom(context, sourceType);
			}
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			Color result = default(Color);
			if (value?.GetType() == typeof(string) && ColorUtil.TryParseHex((string)value, ref result))
			{
				return result;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			Color val = default(Color);
			if (value?.GetType() == typeof(string))
			{
				return ColorUtil.TryParseHex((string)value, ref val);
			}
			return base.IsValid(context, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Color color = (Color)value;
			if (destinationType == typeof(string))
			{
				return $"#{((Color)(ref color)).get_R():X2}{((Color)(ref color)).get_G():X2}{((Color)(ref color)).get_B():X2}";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
