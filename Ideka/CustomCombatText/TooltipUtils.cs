using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gw2Sharp.WebApi.V2.Models;
using HsAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ideka.CustomCombatText
{
	public static class TooltipUtils
	{
		private static readonly Regex ArgRegex = new Regex("%(?:str|num)(\\d)%");

		private static readonly Regex PluralRegex = new Regex("(\\S+)\\[pl:\"(.+?)\"]");

		private static readonly Regex GenderRegex = new Regex("(\\S+)\\[f:\"(.+?)\"]");

		public static string FormatRound(double value)
		{
			return $"{value:#,##0}";
		}

		public static string FormatN3(double value)
		{
			return $"{value:#,##0.###}";
		}

		public static string FormatN3S(double value)
		{
			return ((value >= 0.0) ? "+" : "") + FormatN3(value);
		}

		public static string FormatFraction(double value)
		{
			if (value == 0.0)
			{
				return "0";
			}
			string sign = ((value < 0.0) ? "-" : "");
			value = Math.Abs(value);
			(double, string) tuple;
			switch ((int)Math.Round(value % 1.0 * 4.0))
			{
			case 0:
				if (value % 1.0 == 0.0)
				{
					tuple = (value, "");
					break;
				}
				goto case 1;
			case 1:
				tuple = (Math.Floor(value), "¼");
				break;
			case 2:
				tuple = (Math.Floor(value), "½");
				break;
			case 3:
				tuple = (Math.Floor(value), "¾");
				break;
			default:
				tuple = (Math.Ceiling(value), "");
				break;
			}
			string fraction;
			(value, fraction) = tuple;
			return sign + ((value > 0.0) ? FormatRound(value) : "") + fraction;
		}

		public static string? FormatText(string? text, params string[] args)
		{
			HashSet<int> consumed;
			return FormatText(text, out consumed, args);
		}

		public static string? FormatText(string? text, out HashSet<int> consumed, params string[] args)
		{
			string[] args2 = args;
			if (text == null)
			{
				consumed = new HashSet<int>();
				return null;
			}
			HashSet<int> localConsumed = new HashSet<int>();
			text = ArgRegex.Replace(text, delegate(Match match)
			{
				if (match.Groups.Count <= 1 || !int.TryParse(match.Groups[1].Value, out var result))
				{
					return "";
				}
				result--;
				localConsumed.Add(result);
				return (args2.Length <= result) ? "" : args2[result];
			});
			consumed = localConsumed;
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

		public static string ResolveInflections(string text, Gender gender, int count = 1)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Invalid comparison between Unknown and I4
			text = text.Replace("[s]", (count > 1) ? "s" : "");
			text = PluralRegex.Replace(text, (count > 1) ? "$2" : "$1");
			text = GenderRegex.Replace(text, ((int)gender == 2) ? "$2" : "$1");
			return text;
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
