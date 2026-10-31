using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SeaStove.Gw2Tldr.Data
{
	public static class GuideEdits
	{
		public static GuideEntry Apply(GuideEntry original, JObject edits)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			if (original == null)
			{
				return null;
			}
			JToken obj = ((edits != null) ? edits.get_Item(original.Id) : null);
			JToken obj2 = ((obj is JObject) ? obj : null);
			JObject details = (JObject)((JToken)original.Details).DeepClone();
			JToken obj3 = ((obj2 != null) ? ((JObject)obj2).get_Item("mechanics") : null);
			JArray normal = (JArray)(object)((obj3 is JArray) ? obj3 : null);
			if (normal != null)
			{
				details.set_Item("mechanics", ((JToken)normal).DeepClone());
			}
			JToken obj4 = ((obj2 != null) ? ((JObject)obj2).get_Item("cm_mechanics") : null);
			JArray cm = (JArray)(object)((obj4 is JArray) ? obj4 : null);
			if (cm != null)
			{
				details.set_Item("cm_mechanics", ((JToken)cm).DeepClone());
			}
			return new GuideEntry
			{
				Category = original.Category,
				Group = original.Group,
				Name = original.Name,
				Details = details
			};
		}

		public static string Export(GuideEntry guide)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			JObject val = new JObject();
			val.set_Item("format", JToken.op_Implicit("gw2tldr-guide-notes"));
			val.set_Item("schemaVersion", JToken.op_Implicit(1));
			val.set_Item("guideId", JToken.op_Implicit(guide.Id));
			val.set_Item("mechanics", (JToken)(object)Notes(guide.GetMechanics(challengeMode: false)));
			val.set_Item("cm_mechanics", (JToken)(object)Notes(guide.GetMechanics(challengeMode: true)));
			return ((JToken)val).ToString((Formatting)0, Array.Empty<JsonConverter>());
			static JArray Notes(IEnumerable<Mechanic> notes)
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				return new JArray((object)notes.Select((Func<Mechanic, JObject>)delegate(Mechanic note)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0005: Unknown result type (might be due to invalid IL or missing references)
					//IL_001b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_0048: Expected O, but got Unknown
					JObject val2 = new JObject();
					val2.set_Item("text", JToken.op_Implicit(note.Text));
					val2.set_Item("type", JToken.op_Implicit(note.Type));
					val2.set_Item("color", JToken.op_Implicit(note.Color));
					return val2;
				}));
			}
		}

		public static JObject ParseAll(string input, IEnumerable<GuideEntry> catalog)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Invalid comparison between Unknown and I4
			if (string.IsNullOrWhiteSpace(input) || input.Length > 2000000)
			{
				throw new ArgumentException("Paste a valid export (up to 2,000,000 characters).");
			}
			JObject payload = JObject.Parse(input);
			if (payload.get_Item("format") == null && payload.get_Item("schemaVersion") == null)
			{
				return ParseWebsiteExport(payload, catalog);
			}
			JToken obj = payload.get_Item("schemaVersion");
			if (obj == null || (int)obj.get_Type() != 6 || (int)payload.get_Item("schemaVersion") != 1)
			{
				throw new ArgumentException("Unsupported export format or version.");
			}
			object obj2;
			switch ((string)payload.get_Item("format"))
			{
			default:
				obj2 = null;
				break;
			case "gw2tldr-guide-submissions":
			case "gw2tldr-guide-edits":
			{
				JToken obj3 = payload.get_Item("guides");
				obj2 = ((obj3 is JArray) ? obj3 : null);
				break;
			}
			case "gw2tldr-guide-notes":
				obj2 = (object)new JArray((object)((JToken)payload).DeepClone());
				break;
			}
			JArray entries = (JArray)obj2;
			if (entries == null || ((JContainer)entries).get_Count() == 0)
			{
				throw new ArgumentException("No guide notes found in this export.");
			}
			HashSet<string> ids = new HashSet<string>(catalog.Select((GuideEntry guide) => guide.Id), StringComparer.Ordinal);
			JObject result = new JObject();
			foreach (JToken item in entries)
			{
				JObject guide2 = (JObject)(object)((item is JObject) ? item : null);
				if (guide2 != null)
				{
					JToken obj4 = guide2.get_Item("guideId");
					if (obj4 != null && (int)obj4.get_Type() == 8)
					{
						string id = (string)guide2.get_Item("guideId");
						if (!ids.Contains(id))
						{
							throw new ArgumentException("Unknown encounter: " + id);
						}
						if (result.get_Item(id) != null)
						{
							throw new ArgumentException("Duplicate encounter: " + id);
						}
						result.set_Item(id, (JToken)(object)ValidateNotes(guide2));
						continue;
					}
				}
				throw new ArgumentException("A guide is missing its encounter ID.");
			}
			return result;
		}

		private static JObject ParseWebsiteExport(JObject payload, IEnumerable<GuideEntry> catalog)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			JObject result = new JObject();
			foreach (JProperty category in payload.Properties())
			{
				if (GuideCatalog.Categories.Contains(category.get_Name()))
				{
					JToken value = category.get_Value();
					JObject guides = (JObject)(object)((value is JObject) ? value : null);
					if (guides != null)
					{
						foreach (JProperty entry in guides.Properties())
						{
							List<GuideEntry> matches = catalog.Where((GuideEntry guide) => (guide.Category == category.get_Name() || (category.get_Name() == "raids" && guide.Category == "strikes")) && Regex.Replace(guide.Name.ToLowerInvariant(), "[^a-z0-9]+", "-").TrimEnd('-') == entry.get_Name()).ToList();
							if (matches.Count != 1)
							{
								throw new ArgumentException("Unknown or ambiguous encounter: " + category.get_Name() + " / " + entry.get_Name());
							}
							if (result.get_Item(matches[0].Id) == null)
							{
								JToken value2 = entry.get_Value();
								JObject notes = (JObject)(object)((value2 is JObject) ? value2 : null);
								if (notes != null)
								{
									JObject normalized = (JObject)((JToken)notes).DeepClone();
									if (normalized.get_Item("cm_mechanics") == null)
									{
										normalized.set_Item("cm_mechanics", (JToken)new JArray());
									}
									result.set_Item(matches[0].Id, (JToken)(object)ValidateNotes(normalized));
									continue;
								}
							}
							throw new ArgumentException("Duplicate or invalid encounter: " + entry.get_Name());
						}
						continue;
					}
				}
				throw new ArgumentException("Invalid category: " + category.get_Name());
			}
			if (((JContainer)result).get_Count() == 0)
			{
				throw new ArgumentException("No guide notes found in this export.");
			}
			return result;
		}

		private static JObject ValidateNotes(JObject payload)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Invalid comparison between Unknown and I4
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Invalid comparison between Unknown and I4
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			Dictionary<string, string> validTypes = new Dictionary<string, string>
			{
				["movement"] = "blue",
				["group"] = "green",
				["combat"] = "orange",
				["danger"] = "red",
				["timer"] = "purple",
				["shield"] = "teal",
				["info"] = "gray"
			};
			JObject result = new JObject();
			string[] array = new string[2] { "mechanics", "cm_mechanics" };
			foreach (string field in array)
			{
				JToken obj = payload.get_Item(field);
				JArray notes = (JArray)(object)((obj is JArray) ? obj : null);
				if (notes == null || ((JContainer)notes).get_Count() > 500)
				{
					throw new ArgumentException("Invalid " + field + " notes.");
				}
				JArray cleaned = new JArray();
				foreach (JToken item in notes)
				{
					JObject note = (JObject)(object)((item is JObject) ? item : null);
					if (note != null)
					{
						JToken obj2 = note.get_Item("text");
						if (obj2 != null && (int)obj2.get_Type() == 8)
						{
							JToken obj3 = note.get_Item("type");
							if (obj3 != null && (int)obj3.get_Type() == 8 && !string.IsNullOrWhiteSpace((string)note.get_Item("text")) && ((string)note.get_Item("text")).Length <= 4000 && validTypes.TryGetValue((string)note.get_Item("type"), out var color))
							{
								JObject val = new JObject();
								val.set_Item("text", JToken.op_Implicit((string)note.get_Item("text")));
								val.set_Item("type", JToken.op_Implicit((string)note.get_Item("type")));
								val.set_Item("color", JToken.op_Implicit(color));
								cleaned.Add((JToken)val);
								continue;
							}
						}
					}
					throw new ArgumentException("A note has invalid text or mechanic type.");
				}
				result.set_Item(field, (JToken)(object)cleaned);
			}
			result.set_Item("baseline", (JToken)(object)JValue.CreateNull());
			return result;
		}
	}
}
