using System;
using System.Collections.Generic;
using System.Linq;
using HsAPI;
using Ideka.NetCommon;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ideka.CustomCombatText
{
	public static class TooltipUtils
	{
		public static string FormatRound(float value)
		{
			return $"{value:#,##0}";
		}

		public static string FormatN3(float value)
		{
			return $"{value:#,##0.###}";
		}

		public static string FormatN3S(float value)
		{
			return ((value >= 0f) ? "+" : "") + FormatN3(value);
		}

		public static string FormatFraction(float value)
		{
			if (value == 0f)
			{
				return "0";
			}
			string sign = ((value < 0f) ? "-" : "");
			value = Math.Abs(value);
			(float, string) tuple;
			switch ((int)Math.Round(value % 1f * 4f))
			{
			case 0:
				if (value % 1f == 0f)
				{
					tuple = (value, "");
					break;
				}
				goto case 1;
			case 1:
				tuple = ((float)Math.Floor(value), "¼");
				break;
			case 2:
				tuple = ((float)Math.Floor(value), "½");
				break;
			case 3:
				tuple = ((float)Math.Floor(value), "¾");
				break;
			default:
				tuple = ((float)Math.Ceiling(value), "");
				break;
			}
			string fraction;
			(value, fraction) = tuple;
			return string.Format("{0}{1}{2}", sign, (value > 0f) ? ((object)value) : "", fraction);
		}

		public static string? FormatText(string? text, params string[] args)
		{
			if (text == null)
			{
				return null;
			}
			foreach (var item in args.Enumerate())
			{
				int i = item.index;
				string arg = item.item;
				text = text!.Replace($"%str{i + 1}%", arg);
			}
			text = text!.Replace("[lbracket]", "[").Replace("[rbracket]", "]").Replace("[null]", "")
				.Replace("<BR>", "\n")
				.Replace("<br>", "\n")
				.Replace("%%", "%");
			return text;
		}

		public static string FormatDuration(int milliseconds)
		{
			float x = (float)milliseconds / 1000f;
			if (!(x >= 3600f))
			{
				if (x > 60f)
				{
					return FormatFraction(x / 60f) + "min";
				}
				return FormatFraction(x) + "s";
			}
			return FormatFraction(x / 3600f) + "h";
		}

		public static string ResolveInflections(string text, int count = 1)
		{
			return text.Replace("[s]", (count > 1) ? "s" : "");
		}

		public static Skill ResolveOverrides(Skill skill, TooltipContext context)
		{
			TooltipContext context2 = context;
			ContextOverrideContextGroup @override = skill.OverrideGroups.FirstOrDefault((ContextOverrideContextGroup g) => g.Context.Any(delegate(ContextFlag c)
			{
				GameMode gameMode = context2.GameMode;
				switch (c)
				{
				case ContextFlag.Pve:
					switch (gameMode)
					{
					case GameMode.Pve:
					case GameMode.NotPvp:
					case GameMode.Fractals:
						return true;
					}
					break;
				case ContextFlag.Pvp:
					if (gameMode == GameMode.Pvp)
					{
						return true;
					}
					break;
				case ContextFlag.Wvw:
					if (gameMode == GameMode.Wvw)
					{
						return true;
					}
					break;
				case ContextFlag.Any:
					return true;
				}
				return false;
			}));
			Skill result = ((@override == null) ? Clone(skill) : Merge(skill, @override));
			result.Blocks = Clone(skill.Blocks);
			if (@override != null && @override.Blocks.Any())
			{
				int end = Math.Max(result.Blocks.Count, @override.Blocks.Count);
				for (int j = 0; j < end && @override.Blocks.Count > j; j++)
				{
					FactBlock overrideBlock = @override.Blocks[j];
					if (result.Blocks.Count <= j)
					{
						List<FactBlock> blocks = result.Blocks;
						int index = j;
						FactBlock obj = new FactBlock
						{
							Description = overrideBlock.Description
						};
						List<int> traitRequirements = overrideBlock.TraitRequirements;
						List<int> list = new List<int>(traitRequirements.Count);
						list.AddRange(traitRequirements);
						obj.TraitRequirements = list;
						blocks[index] = obj;
					}
					if (!overrideBlock.Facts.Any())
					{
						continue;
					}
					List<Fact> facts = result.Blocks[j].Facts;
					foreach (Fact fact2 in overrideBlock.Facts)
					{
						if (!fact2.RequiresTrait.Any())
						{
							int? insertBefore2 = fact2.InsertBefore;
							if (insertBefore2.HasValue)
							{
								int insertBefore = insertBefore2.GetValueOrDefault();
								facts.Insert(insertBefore, fact2);
							}
							else
							{
								facts.Add(fact2);
							}
						}
					}
				}
			}
			List<FactBlock> finalBlocks = new List<FactBlock>();
			foreach (FactBlock block in result.Blocks)
			{
				if (block.TraitRequirements.Any())
				{
					continue;
				}
				if (block.Facts.Any())
				{
					List<Fact> finalFacts = new List<Fact>();
					int toSkip = 0;
					for (int i = 0; i < block.Facts.Count; i++)
					{
						Fact fact = block.Facts[i];
						if (!fact.RequiresTrait.Any() && toSkip-- <= 0)
						{
							finalFacts.Add(fact);
							toSkip = fact.SkipNext.GetValueOrDefault();
						}
					}
					block.Facts = finalFacts;
				}
				finalBlocks.Add(block);
			}
			result.Blocks = finalBlocks;
			return result;
		}

		private static T Merge<T>(T a, object b)
		{
			JObject? obj = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(a)) ?? throw new Exception();
			JObject jB = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(b)) ?? throw new Exception();
			obj!.Merge(jB, new JsonMergeSettings
			{
				MergeArrayHandling = MergeArrayHandling.Replace
			});
			T val = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
			if (val == null)
			{
				throw new Exception();
			}
			return val;
		}

		private static T Clone<T>(T a)
		{
			T val = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(a));
			if (val == null)
			{
				throw new Exception();
			}
			return val;
		}
	}
}
