using System;
using System.Collections.Generic;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Ideka.BHUDCommon;
using Ideka.NetCommon;
using Microsoft.Xna.Framework;

namespace Ideka.CustomCombatText
{
	public class StyleSettings : Style, IDisposable
	{
		private readonly DisposableCollection _dc = new DisposableCollection();

		public StyleSettings(SettingCollection settings)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			_dc.Add(settings.Generic<Color>("BaseColor", BaseColor, () => "Base Color", () => "Text color for items that don't have any other color assinged.")).OnChangedAndNow(delegate(Color x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				BaseColor = x;
			});
			_dc.Add(settings.Generic<Color>("BarrierColor", base.BarrierColor.GetValueOrDefault(), () => "Barrier Damage Color", () => "Text color for barrier damage.")).OnChangedAndNow(delegate(Color x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				base.BarrierColor = x;
			});
			_dc.Add(settings.Generic<Color>("PetColor", PetColor.GetValueOrDefault(), () => "Pet Color", () => "Text color for entities that you control as pets.")).OnChangedAndNow(delegate(Color x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				PetColor = x;
			});
			foreach (var item in new List<(ProfessionType, string, string, string)>
			{
				((ProfessionType)6, "Elementalist", "Elementalist", "elementalist"),
				((ProfessionType)3, "Engineer", "Engineer", "engineer"),
				((ProfessionType)1, "Guardian", "Guardian", "guardian"),
				((ProfessionType)7, "Mesmer", "Mesmer", "mesmer"),
				((ProfessionType)8, "Necromancer", "Necromancer", "necromancer"),
				((ProfessionType)4, "Ranger", "Ranger", "ranger"),
				((ProfessionType)9, "Revenant", "Revenant", "revenant"),
				((ProfessionType)5, "Thief", "Thief", "thief"),
				((ProfessionType)2, "Warrior", "Warrior", "warrior")
			})
			{
				var (type3, key2, name, fullName2) = item;
				_dc.Add(settings.Generic<Color>(key2 + "ProfessionColor", get(type3), () => name + " Color", () => "Text color for " + fullName2 + " entities.")).OnChangedAndNow(delegate(Color x)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					set(type3, x);
					void set(ProfessionType type, Color color)
					{
						//IL_0006: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						ProfessionColors[type] = color;
					}
				});
				Color get(ProfessionType type)
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					if (!ProfessionColors.TryGetValue(type, out var color2))
					{
						return default(Color);
					}
					return color2;
				}
			}
			_dc.Add(settings.Generic<Color>("DefaultEntityColor", DefaultEntityColor, () => "Default Entity Color", () => "Text color for entities that don't fit any other category.")).OnChangedAndNow(delegate(Color x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				DefaultEntityColor = x;
			});
			foreach (var item2 in new List<(EventResult, string)>
			{
				(EventResult.Strike, "regular strikes"),
				(EventResult.Crit, "critical strikes"),
				(EventResult.Glance, "glancing strikes"),
				(EventResult.Block, "blocked strikes"),
				(EventResult.Evade, "evaded strikes"),
				(EventResult.Invuln, "absorbed strikes"),
				(EventResult.Miss, "blinded strikes"),
				(EventResult.Bleeding, "bleeding ticks"),
				(EventResult.Burning, "burning ticks"),
				(EventResult.Poison, "poison ticks"),
				(EventResult.Confusion, "confusion ticks"),
				(EventResult.Torment, "torment ticks"),
				(EventResult.DamageTick, "other damage over time ticks"),
				(EventResult.Heal, "heals"),
				(EventResult.HealTick, "heals over time"),
				(EventResult.Barrier, "barrier application"),
				(EventResult.Interrupt, "interrupts"),
				(EventResult.Breakbar, "breakbar damage")
			})
			{
				EventResult type2 = item2.Item1;
				string fullName = item2.Item2;
				string key = type2.GetEnumMemberValue() ?? type2.ToString();
				_dc.Add(settings.Generic<Color>(key + "ResultColor", getColor(type2), () => type2.Describe() + " Color", () => "Text color for " + fullName + ".")).OnChangedAndNow(delegate(Color x)
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					setColor(type2, x);
					void setColor(EventResult type, Color color)
					{
						//IL_0027: Unknown result type (might be due to invalid IL or missing references)
						ResultFormat resultFormat6;
						if (!ResultFormats.TryGetValue(type, out var f2))
						{
							ResultFormat resultFormat5 = (ResultFormats[type] = new ResultFormat());
							resultFormat6 = resultFormat5;
						}
						else
						{
							resultFormat6 = f2;
						}
						resultFormat6.Color = color;
					}
				});
				_dc.Add(settings.Generic(key + "ResultText", getText(type2), () => type2.Describe() + " Text", () => "Pop-up text for " + fullName + ".")).OnChangedAndNow(delegate(string x)
				{
					setText(type2, x);
					void setText(EventResult type, string text)
					{
						ResultFormat resultFormat3;
						if (!ResultFormats.TryGetValue(type, out var f))
						{
							ResultFormat resultFormat2 = (ResultFormats[type] = new ResultFormat());
							resultFormat3 = resultFormat2;
						}
						else
						{
							resultFormat3 = f;
						}
						resultFormat3.Text = (string.IsNullOrEmpty(text) ? null : text);
					}
				});
				Color getColor(EventResult type)
				{
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					if (!ResultFormats.TryGetValue(type, out var format2))
					{
						return default(Color);
					}
					return format2.Color;
				}
				string getText(EventResult type)
				{
					ResultFormat format;
					return (ResultFormats.TryGetValue(type, out format) ? format.Text : null) ?? "";
				}
			}
		}

		public void Dispose()
		{
			_dc.Dispose();
		}
	}
}
