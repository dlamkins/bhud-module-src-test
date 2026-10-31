using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace SeaStove.Gw2Tldr.Data
{
	public sealed class GuideEntry
	{
		public string Category { get; set; }

		public string Group { get; set; }

		public string Name { get; set; }

		public JObject Details { get; set; }

		public string DisplayName => Category + " / " + Group + " / " + Name;

		public string Id => DisplayName;

		public string TypeLabel
		{
			get
			{
				if (!(Category == "raids") && !(Category == "strikes"))
				{
					if (!(Category == "fractals"))
					{
						if (!(Category == "metas"))
						{
							return "Dungeon";
						}
						return "Meta";
					}
					return "Fractal";
				}
				return "Raid encounter";
			}
		}

		public string Section
		{
			get
			{
				if (Category == "raids")
				{
					string group = Group;
					if (group != null)
					{
						int length = group.Length;
						if (length == 6)
						{
							switch (group[5])
							{
							case '1':
								if (!(group == "Wing 1"))
								{
									break;
								}
								goto IL_00de;
							case '2':
								if (!(group == "Wing 2"))
								{
									break;
								}
								goto IL_00de;
							case '3':
								if (!(group == "Wing 3"))
								{
									break;
								}
								goto IL_00de;
							case '4':
								if (!(group == "Wing 4"))
								{
									break;
								}
								goto IL_00de;
							case '5':
								if (!(group == "Wing 5"))
								{
									break;
								}
								goto IL_00e4;
							case '6':
								if (!(group == "Wing 6"))
								{
									break;
								}
								goto IL_00e4;
							case '7':
								if (!(group == "Wing 7"))
								{
									break;
								}
								goto IL_00e4;
							case '8':
								{
									if (!(group == "Wing 8"))
									{
										break;
									}
									return "Janthir Wilds";
								}
								IL_00de:
								return "Heart of Thorns";
								IL_00e4:
								return "Path of Fire";
							}
						}
					}
				}
				if (Category == "strikes" && Group == "Base Game")
				{
					return "Core Game";
				}
				return Group;
			}
		}

		public string Metadata
		{
			get
			{
				List<string> parts = new List<string> { TypeLabel, Section };
				if (Category == "raids" && Section != Group)
				{
					parts.Add(Group);
				}
				string zone = (string)Details.get_Item("zone");
				if (!string.IsNullOrWhiteSpace(zone))
				{
					parts.Add(zone);
				}
				return string.Join(" • ", parts);
			}
		}

		public bool HasChallengeMode
		{
			get
			{
				JToken obj = Details.get_Item("cm_mechanics");
				JToken obj2 = ((obj is JArray) ? obj : null);
				if (obj2 == null)
				{
					return false;
				}
				return ((JContainer)obj2).get_Count() > 0;
			}
		}

		public IReadOnlyList<Mechanic> GetMechanics(bool challengeMode)
		{
			JToken obj = Details.get_Item(challengeMode ? "cm_mechanics" : "mechanics");
			return ((obj != null) ? obj.ToObject<List<Mechanic>>() : null) ?? new List<Mechanic>();
		}

		[IteratorStateMachine(typeof(_003CGetSourceLinks_003Ed__29))]
		public IEnumerable<KeyValuePair<string, string>> GetSourceLinks(bool challengeMode)
		{
			return new _003CGetSourceLinks_003Ed__29(-2)
			{
				_003C_003E4__this = this,
				_003C_003E3__challengeMode = challengeMode
			};
		}
	}
}
