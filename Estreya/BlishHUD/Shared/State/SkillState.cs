using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.Models.GW2API.Skills;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.State
{
	public class SkillState : APIState<Skill>
	{
		private const string BASE_FOLDER_STRUCTURE = "skills";

		private const string FILE_NAME = "skills.json";

		private const string SKILL_FOLDER_NAME = "skills";

		private const string MISSING_SKILLS_FILE_NAME = "missing_skills.json";

		private const string REMAPPED_SKILLS_FILE_NAME = "remapped_skills.json";

		private const string LOCAL_MISSING_SKILL_FILE_NAME = "missingSkills.json";

		private const string LAST_UPDATED_FILE_NAME = "last_updated.txt";

		private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";

		private IconState _iconState;

		private readonly string _baseFolderPath;

		private IFlurlClient _flurlClient;

		private readonly string _fileRootUrl;

		private AsyncRef<double> _lastSaveMissingSkill = new AsyncRef<double>(0.0);

		private static readonly Dictionary<int, int> _remappedSkillIds = new Dictionary<int, int>
		{
			{ 43485, 42297 },
			{ 46808, 42297 },
			{ 46821, 42297 },
			{ 46824, 42297 },
			{ 59601, 1947 },
			{ 49084, 1877 },
			{ 54935, 29921 },
			{ 30176, 29921 },
			{ 1377, 1512 },
			{ 42366, 2159 },
			{ 42984, 2101 },
			{ 9113, 9118 },
			{ 9114, 9115 },
			{ 9119, 9120 },
			{ 17047, 9120 },
			{ 42639, 2063 },
			{ 1623, 2871 },
			{ 45895, 2089 },
			{ 2672, 5683 },
			{ 42133, 2643 },
			{ 13655, 582 },
			{ 30207, 1899 },
			{ 30235, 58090 },
			{ 13515, 1916 },
			{ 5586, 5493 },
			{ 5580, 5495 },
			{ 5585, 5492 },
			{ 5575, 5494 },
			{ 13133, 13132 },
			{ 68121, 68079 },
			{ 59579, 59562 },
			{ 63264, 63348 },
			{ 48894, 1808 },
			{ 10243, 10192 },
			{ 62592, 62597 }
		};

		private static readonly Dictionary<int, (string Name, string RenderUrl)> _missingSkills = new Dictionary<int, (string, string)>
		{
			{
				717,
				("Protection", "CD77D1FAB7B270223538A8F8ECDA1CFB044D65F4/102834")
			},
			{
				718,
				("Regeneration", "F69996772B9E18FD18AD0AABAB25D7E3FC42F261/102835")
			},
			{
				719,
				("Swiftness", "20CFC14967E67F7A3FD4A4B8722B4CF5B8565E11/102836")
			},
			{
				720,
				("Blinded", "102837.png")
			},
			{
				723,
				("Poison", "559B0AF9FB5E1243D2649FAAE660CCB338AACC19/102840")
			},
			{
				725,
				("Fury", "96D90DF84CAFE008233DD1C2606A12C1A0E68048/102842")
			},
			{
				726,
				("Vigor", "58E92EBAF0DB4DA7C4AC04D9B22BCA5ECF0100DE/102843")
			},
			{
				727,
				("Immobile", "102844.png")
			},
			{
				736,
				("Bleeding", "79FF0046A5F9ADA3B4C4EC19ADB4CB124D5F0021/102848")
			},
			{
				737,
				("Burning", "B47BF5803FED2718D7474EAF9617629AD068EE10/102849")
			},
			{
				740,
				("Might", "2FA9DF9D6BC17839BBEA14723F1C53D645DDB5E1/102852")
			},
			{
				742,
				("Weakness", "102853.png")
			},
			{
				743,
				("Aegis", "DFB4D1B50AE4D6A275B349E15B179261EE3EB0AF/102854")
			},
			{
				762,
				("Determined", "102763.png")
			},
			{
				770,
				("Downed", "102763.png")
			},
			{
				791,
				("Fear", "102869.png")
			},
			{
				833,
				("Daze", "433474.png")
			},
			{
				861,
				("Confusion", "289AA0A4644F0E044DED3D3F39CED958E1DDFF53/102880")
			},
			{
				872,
				("Stun", "522727.png")
			},
			{
				873,
				("Resolution", "D104A6B9344A2E2096424A3C300E46BC2926E4D7/2440718")
			},
			{
				890,
				("Revealed", "102887.png")
			},
			{
				910,
				("Poisoned", "102840.png")
			},
			{
				1066,
				("Resurrect", "2261500.png")
			},
			{
				1122,
				("Stability", "3D3A1C2D6D791C05179AB871902D28782C65C244/415959")
			},
			{
				1187,
				("Quickness", "D4AB6401A6D6917C3D4F230764452BCCE1035B0D/1012835")
			},
			{
				5974,
				("Superspeed", "103458.png")
			},
			{
				13017,
				("Stealth", "62777.png")
			},
			{
				2643,
				("Tail Spin", "1770527.png")
			},
			{
				41993,
				("Cannonball", "1770525.png")
			},
			{
				19426,
				("Torment", "10BABF2708CA3575730AC662A2E72EC292565B08/598887")
			},
			{
				17495,
				("Regeneration", "F69996772B9E18FD18AD0AABAB25D7E3FC42F261/102835")
			},
			{
				17674,
				("Regeneration", "F69996772B9E18FD18AD0AABAB25D7E3FC42F261/102835")
			},
			{
				23276,
				("Breakbar Change", "433474.png")
			},
			{
				26766,
				("Slow", "961397.png")
			},
			{
				26980,
				("Resistance", "50BAC1B8E10CFAB9E749A5D910D4A9DCF29EBB7C/961398")
			},
			{
				30328,
				("Alacrity", "4FDAC2113B500104121753EF7E026E45C141E94D/1938787")
			},
			{
				40530,
				("Tome of Justice", "2710AF269B38A4A365089BC7B3C9389B354DE59D/1770472")
			},
			{
				41258,
				("Chapter 1: Searing Spell", "1770473.png")
			},
			{
				40635,
				("Chapter 2: Igniting Burst", "1770474.png")
			},
			{
				42449,
				("Chapter 3: Heated Rebuke", "1770475.png")
			},
			{
				40015,
				("Chapter 4: Scorched Aftermath", "1770476.png")
			},
			{
				41957,
				("Epilogue: Ashes of the Just", "1770477.png")
			},
			{
				46298,
				("Tome of Resolve", "E206770FD62BB63B71F56209F34BF99392BADF9E/1770478")
			},
			{
				40787,
				("Chapter 1: Desert Bloom", "1770479.png")
			},
			{
				40679,
				("Chapter 2: Radiant Recovery", "1770480.png")
			},
			{
				45128,
				("Chapter 3: Azure Sun", "1770481.png")
			},
			{
				42008,
				("Chapter 4: Shining River", "1770482.png")
			},
			{
				44871,
				("Epilogue: Eternal Oasis", "1770483.png")
			},
			{
				43508,
				("Tome of Courage", "49B3D2E829962602205B09619770E1650BF07108/1770466")
			},
			{
				41968,
				("Chapter 2: Daring Challenge", "1770468.png")
			},
			{
				41836,
				("Chapter 3: Valiant Bulwark", "1770469.png")
			},
			{
				43194,
				("Epilogue: Unbroken Lines", "1770471.png")
			},
			{
				53285,
				("Rune of the Monk", "220738.png")
			},
			{
				63348,
				("Jade Energy Shot", "103434.png")
			},
			{
				1656,
				("Whirling Assault", "102998.png")
			},
			{
				33611,
				("Leader of the Pack III", "66520.png")
			},
			{
				62554,
				("Cutter Burst", "2479385.png")
			}
		};

		private ConcurrentDictionary<int, (string Name, int HintId)> _missingSkillsFromAPIReportedByArcDPS;

		private string DirectoryPath => Path.Combine(_baseFolderPath, "skills");

		public static Skill UnknownSkill { get; } = new Skill
		{
			Id = int.MaxValue,
			Name = "Unknown",
			Icon = "62248.png",
			Category = SkillCategory.Skill
		};


		public SkillState(APIStateConfiguration configuration, Gw2ApiManager apiManager, IconState iconState, string baseFolderPath, IFlurlClient flurlClient, string fileRootUrl)
			: base(apiManager, configuration)
		{
			_iconState = iconState;
			_baseFolderPath = baseFolderPath;
			_flurlClient = flurlClient;
			_fileRootUrl = fileRootUrl;
		}

		protected override Task DoInitialize()
		{
			_missingSkillsFromAPIReportedByArcDPS = new ConcurrentDictionary<int, (string, int)>();
			return Task.CompletedTask;
		}

		protected override void DoUnload()
		{
			_missingSkillsFromAPIReportedByArcDPS?.Clear();
			_missingSkillsFromAPIReportedByArcDPS = null;
			_iconState = null;
			_flurlClient = null;
		}

		protected override async Task Load()
		{
			_ = 9;
			try
			{
				await LoadMissingSkills();
				UnknownSkill.LoadTexture(_iconState);
				if (!(await ShouldLoadFiles()))
				{
					await base.Load();
					await Save();
					await AddMissingSkills(base.APIObjectList);
					await RemapSkillIds(base.APIObjectList);
					Logger.Debug("Loading skill icons..");
					LoadSkillIcons(base.APIObjectList);
					Logger.Debug("Loaded skill icons..");
				}
				else
				{
					try
					{
						base.Loading = true;
						List<Skill> skills = JsonConvert.DeserializeObject<List<Skill>>(await FileUtil.ReadStringAsync(Path.Combine(DirectoryPath, "skills.json")));
						await AddMissingSkills(skills);
						await RemapSkillIds(skills);
						LoadSkillIcons(skills);
						using (await _apiObjectListLock.LockAsync())
						{
							base.APIObjectList.AddRange(skills);
						}
					}
					finally
					{
						base.Loading = false;
						SignalCompletion();
					}
				}
				Logger.Debug("Loaded {0} skills.", new object[1] { base.APIObjectList.Count });
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, "Failed loading skills:");
			}
		}

		private async Task<bool> ShouldLoadFiles()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				return false;
			}
			if (!File.Exists(Path.Combine(DirectoryPath, "skills.json")))
			{
				return false;
			}
			string lastUpdatedFilePath = Path.Combine(DirectoryPath, "last_updated.txt");
			if (File.Exists(lastUpdatedFilePath))
			{
				if (!DateTime.TryParseExact(await FileUtil.ReadStringAsync(lastUpdatedFilePath), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lastUpdated))
				{
					Logger.Debug("Failed parsing last updated.");
					return false;
				}
				return DateTime.UtcNow - new DateTime(lastUpdated.Ticks, DateTimeKind.Utc) <= TimeSpan.FromDays(5.0);
			}
			return false;
		}

		protected override async Task<List<Skill>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress)
		{
			progress.Report("Loading normal skills..");
			Logger.Debug("Loading normal skills..");
			List<Skill> skills2 = ((IEnumerable<Skill>)(await ((IAllExpandableClient<Skill>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Skills()).AllAsync(_cancellationTokenSource.Token))).Select((Skill skill) => Skill.FromAPISkill(skill)).ToList();
			Logger.Debug("Loaded normal skills..");
			progress.Report("Loading traits..");
			Logger.Debug("Loading traits..");
			IApiV2ObjectList<Trait> traitResponse = await ((IAllExpandableClient<Trait>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Traits()).AllAsync(_cancellationTokenSource.Token);
			skills2 = skills2.Concat(((IEnumerable<Trait>)traitResponse).Select((Trait trait) => Skill.FromAPITrait(trait))).ToList();
			Logger.Debug("Loaded traits..");
			progress.Report("Loading trait skills..");
			Logger.Debug("Loading trait skills..");
			IEnumerable<TraitSkill> traitSkills = ((IEnumerable<Trait>)traitResponse).Where((Trait trait) => trait.get_Skills() != null).SelectMany((Trait trait) => trait.get_Skills());
			skills2 = skills2.Concat(traitSkills.Select((TraitSkill traitSkill) => Skill.FromAPITraitSkill(traitSkill))).ToList();
			Logger.Debug("Loaded trait skills..");
			return skills2.ToList();
		}

		private async Task RemapSkillIds(List<Skill> skills)
		{
			foreach (RemappedSkillID remappedSkill in JsonConvert.DeserializeObject<List<RemappedSkillID>>(await _flurlClient.Request(_fileRootUrl, "skills", "remapped_skills.json").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0))!)
			{
				List<Skill> skillsToRemap = skills.Where((Skill skill) => skill.Id == remappedSkill.OriginalID).ToList();
				Skill skillToInsert = skills.FirstOrDefault((Skill skill) => skill.Id == remappedSkill.DestinationID);
				if (skillToInsert != null)
				{
					skillToInsert = skillToInsert.Copy();
					skillToInsert.Id = remappedSkill.OriginalID;
					skillsToRemap.ForEach(delegate(Skill skillToRemap)
					{
						skills.Remove(skillToRemap);
					});
					skills.Add(skillToInsert);
					Logger.Debug($"Remapped skill from {remappedSkill.OriginalID} to {remappedSkill.DestinationID} ({remappedSkill.Comment})");
				}
			}
		}

		private async Task AddMissingSkills(List<Skill> skills)
		{
			foreach (MissingSkill missingSkill in JsonConvert.DeserializeObject<List<MissingSkill>>(await _flurlClient.Request(_fileRootUrl, "skills", "missing_skills.json").GetStringAsync(default(CancellationToken), (HttpCompletionOption)0))!)
			{
				skills.Add(new Skill
				{
					Id = missingSkill.ID,
					Name = missingSkill.Name,
					Icon = missingSkill.Icon
				});
				Logger.Debug($"Added missing skill {missingSkill.ID} ({missingSkill.Name})");
				if (missingSkill.NameAliases != null)
				{
					string[] nameAliases = missingSkill.NameAliases;
					foreach (string alias in nameAliases)
					{
						skills.Add(new Skill
						{
							Id = missingSkill.ID,
							Name = alias,
							Icon = missingSkill.Icon
						});
						Logger.Debug($"Added missing skill alias {missingSkill.ID} ({alias})");
					}
				}
			}
		}

		private void LoadSkillIcons(List<Skill> skills)
		{
			skills.ForEach(delegate(Skill skill)
			{
				skill.LoadTexture(_iconState);
			});
		}

		protected override async Task Save()
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, recursive: true);
			}
			Directory.CreateDirectory(DirectoryPath);
			using (await _apiObjectListLock.LockAsync())
			{
				await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "skills.json"), JsonConvert.SerializeObject(base.APIObjectList));
			}
			await CreateLastUpdatedFile();
		}

		protected override void DoUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(SaveMissingSkills, gameTime, 60000.0, _lastSaveMissingSkill);
		}

		private async Task SaveMissingSkills()
		{
			await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "missingSkills.json"), JsonConvert.SerializeObject(_missingSkillsFromAPIReportedByArcDPS.OrderBy((KeyValuePair<int, (string Name, int HintId)> skill) => skill.Value).ToDictionary((KeyValuePair<int, (string Name, int HintId)> skill) => skill.Key, (KeyValuePair<int, (string Name, int HintId)> skill) => skill.Value), Formatting.Indented));
		}

		private async Task LoadMissingSkills()
		{
			string missingSkillPath = Path.Combine(DirectoryPath, "missingSkills.json");
			if (File.Exists(missingSkillPath))
			{
				try
				{
					_missingSkillsFromAPIReportedByArcDPS = JsonConvert.DeserializeObject<ConcurrentDictionary<int, (string, int)>>(await FileUtil.ReadStringAsync(missingSkillPath));
				}
				catch (Exception)
				{
				}
			}
			else
			{
				_missingSkillsFromAPIReportedByArcDPS = new ConcurrentDictionary<int, (string, int)>();
			}
		}

		private async Task CreateLastUpdatedFile()
		{
			await FileUtil.WriteStringAsync(Path.Combine(DirectoryPath, "last_updated.txt"), DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
		}

		public Skill GetByName(string name)
		{
			if (base.Loading)
			{
				return null;
			}
			using (_apiObjectListLock.Lock())
			{
				foreach (Skill skill in base.APIObjectList)
				{
					if (skill.Name == name)
					{
						return skill;
					}
				}
				return null;
			}
		}

		public Skill GetBy(Predicate<Skill> predicate)
		{
			if (base.Loading)
			{
				return null;
			}
			using (_apiObjectListLock.Lock())
			{
				return base.APIObjectList.Find(predicate);
			}
		}

		public Skill GetById(int id)
		{
			if (base.Loading)
			{
				return null;
			}
			using (_apiObjectListLock.Lock())
			{
				foreach (Skill skill in base.APIObjectList)
				{
					if (skill.Id == id)
					{
						return skill;
					}
				}
				return null;
			}
		}

		public bool AddMissingSkill(int id, string name)
		{
			int hintId = -1;
			using (_apiObjectListLock.Lock())
			{
				foreach (Skill skill in base.APIObjectList)
				{
					if (skill.Name == name)
					{
						hintId = skill.Id;
						break;
					}
				}
			}
			return _missingSkillsFromAPIReportedByArcDPS?.TryAdd(id, (name, hintId)) ?? false;
		}

		public void RemoveMissingSkill(int id)
		{
			_missingSkillsFromAPIReportedByArcDPS?.TryRemove(id, out var _);
		}
	}
}
