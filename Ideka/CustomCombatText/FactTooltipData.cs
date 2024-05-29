using System;
using System.Collections.Generic;
using System.Linq;
using HsAPI;
using Ideka.NetCommon;

namespace Ideka.CustomCombatText
{
	public class FactTooltipData
	{
		private static readonly Skill MissingBuff = new Skill
		{
			Id = 0,
			Name = "Missing Buff"
		};

		public int? PrefixIconId { get; }

		public int? IconId { get; }

		public int BuffApplyCount { get; } = 1;


		public int BuffDuration { get; }

		public string Text { get; }

		public FactTooltipData(Fact fact, double weaponStrength, TooltipContext context)
		{
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			IconId = fact.Icon;
			Text = "Unknown Fact";
			AdjustByAttributeAndLevelFact alFact = fact as AdjustByAttributeAndLevelFact;
			if (alFact != null)
			{
				double value2 = ((double)alFact.Value + Math.Pow(context.CharacterLevel, alFact.LevelExponent) * (double)alFact.LevelMultiplier) * (double)alFact.HitCount;
				BaseAttribute? attribute = alFact.Attribute;
				if (attribute.HasValue)
				{
					BaseAttribute attr = attribute.GetValueOrDefault();
					double attrVal = context.Stats[attr];
					value2 += attrVal * (double)alFact.AttributeMultiplier.GetValueOrDefault() * (double)alFact.HitCount;
				}
				Text = (TooltipUtils.FormatText(alFact.Text) ?? alFact.Attribute?.ToString()) + ": " + TooltipUtils.FormatRound(value2);
				return;
			}
			AttributeAdjustFact adFact = fact as AttributeAdjustFact;
			if (adFact != null)
			{
				double value = MathUtils.Scale(context.CharacterLevel, 0.0, 80.0, adFact.Range[0], adFact.Range[1]);
				Text = (TooltipUtils.FormatText(adFact.Text) ?? adFact.Target.ToString()) + ": " + TooltipUtils.FormatN3S(value);
				return;
			}
			BuffFact bFact = fact as BuffFact;
			if (bFact != null)
			{
				Skill buff4 = getBuffOrFallback(bFact.Buff);
				IconId = buff4.Icon ?? IconId;
				BuffDuration = bFact.Duration;
				int applyCount2 = bFact.ApplyCount;
				Text = TooltipUtils.FormatText(TooltipUtils.ResolveInflections(fact.Text ?? buff4.NameBrief ?? buff4.Name, context.CharacterGender)) + ((BuffDuration > 0) ? ("(" + TooltipUtils.FormatDuration(BuffDuration) + ")") : "") + ": " + DescribeBuff(buff4, ref applyCount2, BuffDuration, context);
				BuffApplyCount = applyCount2;
				return;
			}
			BuffBriefFact bbFact = fact as BuffBriefFact;
			if (bbFact != null)
			{
				Skill buff3 = getBuffOrFallback(bbFact.Buff);
				IconId = buff3.Icon ?? IconId;
				Text = TooltipUtils.FormatText(bbFact.Text, buff3.Name) ?? "";
				return;
			}
			DistanceFact distFact = fact as DistanceFact;
			if (distFact != null)
			{
				Text = TooltipUtils.FormatText(distFact.Text) + ": " + TooltipUtils.FormatRound(distFact.Distance);
				return;
			}
			NumberFact numFact = fact as NumberFact;
			if (numFact != null)
			{
				Text = TooltipUtils.FormatText(numFact.Text) + ": " + TooltipUtils.FormatFraction(numFact.Value);
				return;
			}
			PercentFact pctFact = fact as PercentFact;
			if (pctFact != null)
			{
				Text = TooltipUtils.FormatText(pctFact.Text) + ": " + TooltipUtils.FormatFraction(pctFact.Percent) + "%";
				return;
			}
			PercentHpSelfDamageFact phsFact = fact as PercentHpSelfDamageFact;
			if (phsFact != null)
			{
				Text = TooltipUtils.FormatText(phsFact.Text) + ": " + $"{Math.Floor((double)((float)context.Health * phsFact.Percent) * 0.01)} " + "(" + TooltipUtils.FormatFraction(phsFact.Percent) + "% HP pool)";
				return;
			}
			PercentLifeForceCostFact plcFact = fact as PercentLifeForceCostFact;
			if (plcFact != null)
			{
				Text = TooltipUtils.FormatText(plcFact.Text) + ": " + TooltipUtils.FormatN3(Math.Round((double)((float)context.BaseHealth * plcFact.Percent) * 0.01));
				return;
			}
			PercentHealthFact phFact = fact as PercentHealthFact;
			if (phFact != null)
			{
				Text = TooltipUtils.FormatText(phFact.Text) + ": " + TooltipUtils.FormatFraction(phFact.Percent) + "% (" + TooltipUtils.FormatRound((double)((float)context.Health * phFact.Percent) * 0.01) + " HP)";
				return;
			}
			PercentLifeForceGainFact plgFact = fact as PercentLifeForceGainFact;
			if (plgFact != null)
			{
				Text = TooltipUtils.FormatText(plgFact.Text) + ": " + TooltipUtils.FormatFraction(plgFact.Percent) + "% (" + TooltipUtils.FormatRound((double)context.Health * 0.69 * (double)plgFact.Percent * 0.01) + " HP)";
				return;
			}
			DamageFact dmgFact = fact as DamageFact;
			if (dmgFact != null)
			{
				double damage = (double)(dmgFact.DmgMultiplier * (float)dmgFact.HitCount) * weaponStrength * context.Stats[BaseAttribute.Power] / (double)context.TargetArmor;
				double critChance = Math.Min(0.05 + (context.Stats[BaseAttribute.Precision] - 1000.0) / 21.0 * 0.01, 1.0);
				double critDamage = 1.5 * context.Stats[BaseAttribute.Ferocity] / 15.0 * 0.01;
				double damageFromCrit = damage * critChance * (critDamage - 1.0);
				damage += damageFromCrit;
				Text = TooltipUtils.FormatText(dmgFact.Text) + ((dmgFact.HitCount > 1) ? $"({dmgFact.HitCount}x)" : "") + ": " + TooltipUtils.FormatRound(damage);
				return;
			}
			TimeFact timeFact = fact as TimeFact;
			if (timeFact != null)
			{
				Text = TooltipUtils.FormatText(timeFact.Text) + ": " + TooltipUtils.FormatFraction((double)timeFact.Duration * 0.001) + " " + ((timeFact.Duration == 1000) ? "second" : "seconds");
				return;
			}
			ComboFieldFact fieldFact = fact as ComboFieldFact;
			if (fieldFact != null)
			{
				string text = TooltipUtils.FormatText(fieldFact.Text);
				Text = text + ": " + fieldFact.FieldType switch
				{
					ComboFieldType.Air => "Air", 
					ComboFieldType.Dark => "Dark", 
					ComboFieldType.Fire => "Fire", 
					ComboFieldType.Ice => "Ice", 
					ComboFieldType.Light => "Light", 
					ComboFieldType.Lightning => "Lightning", 
					ComboFieldType.Poison => "Poison", 
					ComboFieldType.Smoke => "Smoke", 
					ComboFieldType.Ethereal => "Ethereal", 
					ComboFieldType.Water => "Water", 
					_ => "Unknown Field", 
				};
				return;
			}
			ComboFinisherFact fnshFact = fact as ComboFinisherFact;
			if (fnshFact != null)
			{
				string text2 = TooltipUtils.FormatText(fnshFact.Text);
				Text = text2 + ": " + fnshFact.FinisherType switch
				{
					ComboFinisherType.Blast => "Blast", 
					ComboFinisherType.Leap => "Leap", 
					ComboFinisherType.Projectile => "Physical Projectile", 
					ComboFinisherType.Projectile20 => "Physical Projectile (20% Chance)", 
					ComboFinisherType.Whirl => "Whirl", 
					_ => "Unknown Finisher", 
				};
				return;
			}
			AttributeConversionFact attrFact = fact as AttributeConversionFact;
			if (attrFact != null)
			{
				Text = $"Gain {attrFact.Target} Based on a Percentage of {attrFact.Source}: {attrFact.Percent}%";
				return;
			}
			NoDataFact noFact = fact as NoDataFact;
			if (noFact != null)
			{
				Text = TooltipUtils.FormatText(noFact.Text) ?? "";
				return;
			}
			PrefixedBuffFact pbFact = fact as PrefixedBuffFact;
			if (pbFact != null)
			{
				PrefixIconId = getBuffOrFallback(pbFact.Prefix).Icon ?? IconId;
				Skill buff2 = getBuffOrFallback(pbFact.Buff);
				IconId = buff2.Icon;
				BuffDuration = pbFact.Duration;
				int applyCount = pbFact.ApplyCount;
				Text = TooltipUtils.FormatText(fact.Text ?? buff2.NameBrief ?? buff2.Name) + ((BuffDuration > 0) ? ("(" + TooltipUtils.FormatDuration(BuffDuration) + ")") : "") + ": " + DescribeBuff(buff2, ref applyCount, BuffDuration, context);
				BuffApplyCount = applyCount;
				return;
			}
			PrefixedBuffBriefFact pbbFact = fact as PrefixedBuffBriefFact;
			if (pbbFact != null)
			{
				PrefixIconId = getBuffOrFallback(pbbFact.Prefix).Icon ?? IconId;
				Skill buff = getBuffOrFallback(pbbFact.Buff);
				IconId = buff.Icon;
				Text = (TooltipUtils.FormatText(pbbFact.Text) ?? buff.NameBrief ?? buff.Name) ?? "";
				return;
			}
			RangeFact rangeFact = fact as RangeFact;
			if (rangeFact != null)
			{
				float? min2 = rangeFact.Min;
				string text3;
				if (min2.HasValue)
				{
					float min = min2.GetValueOrDefault();
					text3 = "Range: " + TooltipUtils.FormatRound(min) + " - " + TooltipUtils.FormatRound(rangeFact.Max);
				}
				else
				{
					text3 = "Range: " + TooltipUtils.FormatRound(rangeFact.Max);
				}
				Text = text3;
			}
			else if (fact is StunBreakFact)
			{
				Text = "Breaks Stun";
			}
			static Skill getBuffOrFallback(int buffId)
			{
				if (!CTextModule.HsSkills.TryGetValue(buffId, out var buff5))
				{
					return MissingBuff;
				}
				return buff5;
			}
		}

		private static string? DescribeBuff(Skill buff, ref int applyCount, int duration, TooltipContext context)
		{
			TooltipContext context2 = context;
			if (buff.DescriptionBrief != null)
			{
				HashSet<int> consumed2;
				string? result = TooltipUtils.FormatText(buff.DescriptionBrief, out consumed2, TooltipUtils.FormatFraction(applyCount));
				if (consumed2.Contains(0))
				{
					applyCount = 0;
				}
				return result;
			}
			if (!buff.Modifiers.Any())
			{
				HashSet<int> consumed;
				string? result2 = TooltipUtils.FormatText(buff.Description, out consumed, TooltipUtils.FormatFraction(applyCount));
				if (consumed.Contains(0))
				{
					applyCount = 0;
				}
				return result2;
			}
			List<Modifier> modifiers = buff.Modifiers.Where((Modifier m) => !m.TargetTraitReq.HasValue && !m.SourceTraitReq.HasValue && (!m.Mode.HasValue || m.Mode == context2.GameMode)).ToList();
			Dictionary<int, (Modifier, double)> merged = new Dictionary<int, (Modifier, double)>();
			for (int i = 0; i < modifiers.Count; i++)
			{
				Modifier modifier = modifiers[i];
				double value = CalculateModifier(modifier, context2);
				BaseAttribute? sourceAttribute = modifier.SourceAttribute;
				if (sourceAttribute.HasValue)
				{
					BaseAttribute attr = sourceAttribute.GetValueOrDefault();
					value *= context2.Stats[attr];
				}
				merged[modifier.Id] = (merged.TryGetValue(modifier.Id, out var previous) ? (previous.Item1, previous.Item2 + value) : (modifier, value));
				if (modifier.Flags.Contains(ModifierFlag.SkipNextEntry))
				{
					i++;
				}
			}
			List<string> final = new List<string>();
			foreach (var value3 in merged.Values)
			{
				var (modifier2, value2) = value3;
				if (modifier2.Flags.Contains(ModifierFlag.Subtract))
				{
					value2 -= 100.0;
				}
				if (duration > 0 && modifier2.Flags.Contains(ModifierFlag.MulByDuration))
				{
					double thisDuration = (double)duration * 0.001;
					if (modifier2.Flags.Contains(ModifierFlag.DivDurationBy3))
					{
						thisDuration /= 3.0;
					}
					if (modifier2.Flags.Contains(ModifierFlag.DivDurationBy10))
					{
						thisDuration /= 10.0;
					}
					value2 *= thisDuration;
				}
				if (!modifier2.Flags.Contains(ModifierFlag.NonStacking))
				{
					value2 *= (double)applyCount;
				}
				string strValue = (modifier2.Flags.Contains(ModifierFlag.FormatFraction) ? TooltipUtils.FormatFraction(value2) : TooltipUtils.FormatRound(value2));
				if (modifier2.Flags.Contains(ModifierFlag.FormatPercent))
				{
					strValue = ((value2 > 0.0) ? "+" : "") + strValue + "%";
				}
				strValue = strValue + " " + modifier2.Description;
				final.Add(strValue);
			}
			return string.Join(", ", final);
		}

		private static double CalculateModifier(Modifier modifier, TooltipContext context)
		{
			if (modifier.SourceAttribute.HasValue)
			{
				return context.Stats[modifier.SourceAttribute.Value] * (double)modifier.BaseAmount * 0.01;
			}
			int level = context.CharacterLevel;
			double attribute = 0.0;
			switch (modifier.Formula)
			{
			case BuffScaling.NoScaling:
				level = 0;
				break;
			case BuffScaling.BuffFormulaType11:
				attribute = -1.0;
				break;
			case BuffScaling.ConditionDamageSquared:
				level *= level;
				goto case BuffScaling.ConditionDamage;
			case BuffScaling.ConditionDamage:
				attribute = context.Stats[BaseAttribute.ConditionDamage];
				break;
			case BuffScaling.HealingPowerSquared:
				level *= level;
				goto case BuffScaling.HealingPower;
			case BuffScaling.HealingPower:
				attribute = context.Stats[BaseAttribute.HealingPower];
				break;
			case BuffScaling.PowerSquared:
				level *= level;
				goto case BuffScaling.Power;
			case BuffScaling.Power:
				attribute = context.Stats[BaseAttribute.Power];
				break;
			case BuffScaling.FerocitySquared:
				level *= level;
				goto case BuffScaling.Ferocity;
			case BuffScaling.Ferocity:
				attribute = context.Stats[BaseAttribute.Ferocity];
				break;
			default:
				level = 0;
				break;
			case BuffScaling.BuffLevelLinear:
			case BuffScaling.SpawnScaleLinear:
			case BuffScaling.TargetLevelLinear:
				break;
			}
			return (double)(modifier.BaseAmount + (float)level * modifier.FormulaParam1) + attribute * (double)modifier.FormulaParam2;
		}
	}
}
