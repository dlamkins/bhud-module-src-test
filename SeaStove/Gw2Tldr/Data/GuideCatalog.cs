using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SeaStove.Gw2Tldr.Data
{
	public sealed class GuideCatalog
	{
		public static readonly string[] Categories = new string[5] { "raids", "strikes", "fractals", "metas", "dungeons" };

		private JObject _snapshotData;

		public IReadOnlyList<GuideEntry> Guides { get; private set; }

		public string SourceCommit { get; private set; }

		public string FractalLevels(GuideEntry guide)
		{
			if (guide?.Category != "fractals")
			{
				return null;
			}
			JObject snapshotData = _snapshotData;
			object obj;
			if (snapshotData == null)
			{
				obj = null;
			}
			else
			{
				JToken obj2 = snapshotData.get_Item("fractalLevels");
				obj = ((obj2 != null) ? obj2.get_Item((object)guide.Name) : null);
			}
			JObject levels = (JObject)((obj is JObject) ? obj : null);
			if (levels == null)
			{
				return null;
			}
			IEnumerable<string> parts = from item in new string[4] { "Initiate", "Adept", "Expert", "Master" }.Select((string tier, int index) => new { tier, index }).Where(item =>
				{
					JToken obj3 = levels.get_Item(item.tier);
					JArray val = (JArray)(object)((obj3 is JArray) ? obj3 : null);
					return val != null && ((JContainer)val).get_Count() > 0;
				})
				select "T" + (item.index + 1) + ": " + string.Join(", ", ((JContainer)(JArray)levels.get_Item(item.tier)).Values<int>());
			string result = string.Join("  ", parts);
			if (result.Length != 0)
			{
				return result;
			}
			return null;
		}

		public int? FractalLevel(GuideEntry guide, int tier)
		{
			if (guide?.Category != "fractals" || tier < 1 || tier > 4)
			{
				return null;
			}
			string[] names = new string[4] { "Initiate", "Adept", "Expert", "Master" };
			JObject snapshotData = _snapshotData;
			object obj;
			if (snapshotData == null)
			{
				obj = null;
			}
			else
			{
				JToken obj2 = snapshotData.get_Item("fractalLevels");
				if (obj2 == null)
				{
					obj = null;
				}
				else
				{
					JToken obj3 = obj2.get_Item((object)guide.Name);
					obj = ((obj3 != null) ? obj3.get_Item((object)names[tier - 1]) : null);
				}
			}
			JArray values = (JArray)((obj is JArray) ? obj : null);
			if (values != null && ((JContainer)values).get_Count() != 0)
			{
				return ((JContainer)values).Values<int>().Min();
			}
			return null;
		}

		public List<GuideEntry> SortFractals(IEnumerable<GuideEntry> guides, int tier)
		{
			return guides.OrderBy((GuideEntry guide) => (tier != 0) ? FractalLevel(guide, tier).GetValueOrDefault(int.MaxValue) : 0).ThenBy((GuideEntry guide) => guide.Name, StringComparer.OrdinalIgnoreCase).ThenBy((GuideEntry guide) => guide.DisplayName, StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		public MetaSchedule.Occurrence NextMeta(GuideEntry guide, DateTimeOffset now)
		{
			if (!(guide?.Category == "metas"))
			{
				return null;
			}
			return MetaSchedule.Next(_snapshotData, guide.Name, now);
		}

		public static GuideCatalog Load(Stream stream)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if (stream == null)
			{
				throw new InvalidDataException("Bundled guides are missing. Run npm run blishhud:export before building.");
			}
			JObject snapshot;
			using (StreamReader reader = new StreamReader(stream))
			{
				JsonTextReader jsonReader = new JsonTextReader((TextReader)reader);
				try
				{
					snapshot = JObject.Load((JsonReader)(object)jsonReader);
				}
				finally
				{
					((IDisposable)jsonReader)?.Dispose();
				}
			}
			if (((int?)snapshot.get_Item("schemaVersion")).GetValueOrDefault() != 1)
			{
				throw new InvalidDataException("Unsupported guide snapshot version. Update the GW2 TLDR module.");
			}
			JToken obj = snapshot.get_Item("data");
			JObject data = (JObject)(object)((obj is JObject) ? obj : null);
			if (data == null)
			{
				throw new InvalidDataException("Guide snapshot has no data.");
			}
			List<GuideEntry> guides = new List<GuideEntry>();
			string[] categories = Categories;
			foreach (string category in categories)
			{
				JToken obj2 = data.get_Item(category);
				foreach (JProperty group in ((JObject)(((obj2 is JObject) ? obj2 : null) ?? throw new InvalidDataException("Missing guide category: " + category))).Properties())
				{
					JToken value = group.get_Value();
					foreach (JProperty entry in ((JObject)(((value is JObject) ? value : null) ?? throw new InvalidDataException("Invalid guide group: " + group.get_Name()))).Properties())
					{
						JToken value2 = entry.get_Value();
						JObject details = (JObject)(object)((value2 is JObject) ? value2 : null);
						if (details != null)
						{
							JToken obj3 = details.get_Item("mechanics");
							JArray mechanics = (JArray)(object)((obj3 is JArray) ? obj3 : null);
							if (mechanics != null && ((JContainer)mechanics).get_Count() != 0)
							{
								string[] array = new string[2] { "mechanics", "cm_mechanics" };
								foreach (string field in array)
								{
									if (details.get_Item(field) == null)
									{
										continue;
									}
									JToken obj4 = details.get_Item(field);
									foreach (JToken item in (JArray)(((obj4 is JArray) ? obj4 : null) ?? throw new InvalidDataException("Invalid mechanics: " + entry.get_Name())))
									{
										if (!(item is JObject) || string.IsNullOrWhiteSpace((string)item.get_Item((object)"text")) || string.IsNullOrWhiteSpace((string)item.get_Item((object)"type")) || string.IsNullOrWhiteSpace((string)item.get_Item((object)"color")))
										{
											throw new InvalidDataException("Invalid mechanic: " + entry.get_Name());
										}
									}
								}
								ValidateOptionalFields(details, entry.get_Name());
								guides.Add(new GuideEntry
								{
									Category = category,
									Group = group.get_Name(),
									Name = entry.get_Name(),
									Details = details
								});
								continue;
							}
						}
						throw new InvalidDataException("Missing mechanics: " + entry.get_Name());
					}
				}
			}
			if (guides.Count == 0)
			{
				throw new InvalidDataException("The guide snapshot is empty.");
			}
			return new GuideCatalog
			{
				Guides = guides,
				SourceCommit = (string)snapshot.get_Item("sourceCommit"),
				_snapshotData = data
			};
		}

		private static bool IsAbsent(JToken token)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			if (token != null)
			{
				return (int)token.get_Type() == 10;
			}
			return true;
		}

		private static void ValidateOptionalFields(JObject details, string name)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			JToken zone = details.get_Item("zone");
			if (!IsAbsent(zone) && (int)zone.get_Type() != 8)
			{
				throw new InvalidDataException("Invalid zone: " + name);
			}
			JToken waypoints = details.get_Item("waypoints");
			if (!IsAbsent(waypoints))
			{
				JArray points = (JArray)(object)((waypoints is JArray) ? waypoints : null);
				if (points == null || ((IEnumerable<JToken>)points).Any((JToken point) => (int)point.get_Type() != 8))
				{
					throw new InvalidDataException("Invalid waypoints: " + name);
				}
			}
			JToken links = details.get_Item("guideLinks");
			if (IsAbsent(links))
			{
				return;
			}
			foreach (JProperty mode in ((JObject)(((links is JObject) ? links : null) ?? throw new InvalidDataException("Invalid guideLinks: " + name))).Properties())
			{
				if (!IsAbsent(mode.get_Value()) && ((JObject)(/*isinst with value type is only supported in some contexts*/ ?? throw new InvalidDataException("Invalid guideLinks mode: " + name))).Properties().Any((JProperty source) => !IsAbsent(source.get_Value()) && (int)source.get_Value().get_Type() != 8))
				{
					throw new InvalidDataException("Invalid guideLinks URL: " + name);
				}
			}
		}

		public IReadOnlyList<GuideEntry> Search(string category, string query)
		{
			query = (query ?? "").Trim();
			string[] terms = query.Split(new char[4] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			return Guides.Where((GuideEntry guide) => (string.IsNullOrEmpty(category) || guide.Category == category) && terms.All((string term) => Contains(guide.Name, term) || Contains(guide.Group, term) || Contains(guide.Section, term) || Contains(guide.Category, term) || Contains(guide.TypeLabel, term) || (guide.Category == "strikes" && Contains("strike", term)) || (guide.Category == "dungeons" && Contains("path story", term)) || Contains(((string)guide.Details.get_Item("zone")) ?? "", term) || guide.GetMechanics(challengeMode: false).Concat(guide.GetMechanics(challengeMode: true)).Any((Mechanic mechanic) => Contains(mechanic.Text, term)))).OrderBy((GuideEntry guide) => guide.Name, StringComparer.OrdinalIgnoreCase).ThenBy((GuideEntry guide) => guide.DisplayName, StringComparer.OrdinalIgnoreCase)
				.ToList();
		}

		private static bool Contains(string text, string query)
		{
			return text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}
