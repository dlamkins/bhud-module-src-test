using System.Collections.Generic;
using System.Linq;
using Blish_HUD.Settings;

namespace Estreya.BlishHUD.EventTable.Extensions
{
	public static class SettingEntryExtensions
	{
		public static float GetValue(this SettingEntry<float> settingEntry)
		{
			if (settingEntry == null)
			{
				return 0f;
			}
			(float, float)? range = settingEntry.GetRange();
			if (!range.HasValue)
			{
				return settingEntry.get_Value();
			}
			if (settingEntry.get_Value() > range.Value.Item2)
			{
				return range.Value.Item2;
			}
			if (settingEntry.get_Value() < range.Value.Item1)
			{
				return range.Value.Item1;
			}
			return settingEntry.get_Value();
		}

		public static int GetValue(this SettingEntry<int> settingEntry)
		{
			if (settingEntry == null)
			{
				return 0;
			}
			(int, int)? range = settingEntry.GetRange();
			if (!range.HasValue)
			{
				return settingEntry.get_Value();
			}
			if (settingEntry.get_Value() > range.Value.Item2)
			{
				return range.Value.Item2;
			}
			if (settingEntry.get_Value() < range.Value.Item1)
			{
				return range.Value.Item1;
			}
			return settingEntry.get_Value();
		}

		public static (int Min, int Max)? GetRange(this SettingEntry<int> settingEntry)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (settingEntry == null)
			{
				return null;
			}
			List<IComplianceRequisite> crList = (from cr in SettingComplianceExtensions.GetComplianceRequisite((SettingEntry)(object)settingEntry)
				where cr is IntRangeRangeComplianceRequisite
				select cr).ToList();
			if (crList.Count > 0)
			{
				IntRangeRangeComplianceRequisite intRangeCr = (IntRangeRangeComplianceRequisite)(object)crList[0];
				return new(int, int)?((((IntRangeRangeComplianceRequisite)(ref intRangeCr)).get_MinValue(), ((IntRangeRangeComplianceRequisite)(ref intRangeCr)).get_MaxValue()));
			}
			return null;
		}

		public static (float Min, float Max)? GetRange(this SettingEntry<float> settingEntry)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (settingEntry == null)
			{
				return null;
			}
			List<IComplianceRequisite> crList = (from cr in SettingComplianceExtensions.GetComplianceRequisite((SettingEntry)(object)settingEntry)
				where cr is FloatRangeRangeComplianceRequisite
				select cr).ToList();
			if (crList.Count > 0)
			{
				FloatRangeRangeComplianceRequisite floatRangeCr = (FloatRangeRangeComplianceRequisite)(object)crList[0];
				return new(float, float)?((((FloatRangeRangeComplianceRequisite)(ref floatRangeCr)).get_MinValue(), ((FloatRangeRangeComplianceRequisite)(ref floatRangeCr)).get_MaxValue()));
			}
			return null;
		}

		public static bool IsDisabled(this SettingEntry settingEntry)
		{
			return SettingComplianceExtensions.GetComplianceRequisite(settingEntry)?.Any((IComplianceRequisite cr) => cr is SettingDisabledComplianceRequisite) ?? false;
		}

		public static bool HasValidation<T>(this SettingEntry<T> settingEntry)
		{
			return SettingComplianceExtensions.GetComplianceRequisite((SettingEntry)(object)settingEntry)?.Any((IComplianceRequisite cr) => cr is SettingValidationComplianceRequisite<T>) ?? false;
		}

		public static SettingValidationComplianceRequisite<T> GetValidation<T>(this SettingEntry<T> settingEntry)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			return (SettingValidationComplianceRequisite<T>)(object)SettingComplianceExtensions.GetComplianceRequisite((SettingEntry)(object)settingEntry)?.First((IComplianceRequisite cr) => cr is SettingValidationComplianceRequisite<T>);
		}
	}
}
