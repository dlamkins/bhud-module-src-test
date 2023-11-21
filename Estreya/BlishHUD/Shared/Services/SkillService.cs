using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD.Modules.Managers;
using Estreya.BlishHUD.Shared.Extensions;
using Estreya.BlishHUD.Shared.IO;
using Estreya.BlishHUD.Shared.Models.GW2API.Skills;
using Estreya.BlishHUD.Shared.Threading;
using Estreya.BlishHUD.Shared.Utils;
using Flurl.Http;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Estreya.BlishHUD.Shared.Services
{
	public class SkillService : FilesystemAPIService<Skill>
	{
		private struct MissingArcDPSSkill
		{
			public int ID { get; set; }

			public string Name { get; set; }

			public int HintID { get; set; }
		}

		private const string MISSING_SKILLS_FILE_NAME = "missing_skills.json";

		private const string REMAPPED_SKILLS_FILE_NAME = "remapped_skills.json";

		private const string LOCAL_MISSING_SKILL_FILE_NAME = "missingSkills.json";

		private readonly AsyncRef<double> _lastSaveMissingSkill = new AsyncRef<double>(0.0);

		private readonly string _webRootUrl;

		private IFlurlClient _flurlClient;

		private IconService _iconService;

		private SynchronizedCollection<MissingArcDPSSkill> _missingSkillsFromAPIReportedByArcDPS;

		protected override string BASE_FOLDER_STRUCTURE => "skills";

		protected override string FILE_NAME => "skills.json";

		public List<Skill> Skills => base.APIObjectList;

		public static Skill UnknownSkill { get; } = new Skill
		{
			Id = int.MaxValue,
			Name = "Unknown",
			Icon = "62248.png",
			Category = SkillCategory.Skill
		};


		protected override bool ForceAPI => true;

		public SkillService(APIServiceConfiguration configuration, Gw2ApiManager apiManager, IconService iconService, string baseFolderPath, IFlurlClient flurlClient, string webRootUrl)
			: base(apiManager, configuration, baseFolderPath)
		{
			_iconService = iconService;
			_flurlClient = flurlClient;
			_webRootUrl = new Uri(new Uri(webRootUrl), "/gw2/api/v2/skills").ToString();
		}

		protected override Task DoInitialize()
		{
			_missingSkillsFromAPIReportedByArcDPS = new SynchronizedCollection<MissingArcDPSSkill>();
			return Task.CompletedTask;
		}

		protected override void DoUnload()
		{
			_missingSkillsFromAPIReportedByArcDPS?.Clear();
			_missingSkillsFromAPIReportedByArcDPS = null;
			_iconService = null;
			_flurlClient = null;
		}

		protected override async Task Load()
		{
			await LoadMissingSkills();
			await base.Load();
		}

		protected override async Task OnAfterFilesystemLoad(List<Skill> loadedEntitesFromFile)
		{
			ReportProgress("Adding missing skills...");
			await AddMissingSkills(loadedEntitesFromFile);
			ReportProgress("Remap skills...");
			await RemapSkillIds(loadedEntitesFromFile);
			ReportProgress("Loading skill icons...");
			LoadSkillIcons(loadedEntitesFromFile);
		}

		protected override async Task OnAfterLoadFromAPIAfterSave()
		{
			ReportProgress("Adding missing skills...");
			await AddMissingSkills(base.APIObjectList);
			ReportProgress("Remap skills...");
			await RemapSkillIds(base.APIObjectList);
			ReportProgress("Loading skill icons...");
			LoadSkillIcons(base.APIObjectList);
			SignalUpdated();
		}

		protected override async Task<List<Skill>> Fetch(Gw2ApiManager apiManager, IProgress<string> progress, CancellationToken cancellationToken)
		{
			progress.Report("Loading normal skills..");
			Logger.Debug("Loading normal skills..");
			List<Skill> skills2 = ((IEnumerable<Skill>)(await ((IAllExpandableClient<Skill>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Skills()).AllAsync(cancellationToken))).Select((Skill skill) => Skill.FromAPISkill(skill)).ToList();
			Logger.Debug("Loaded normal skills..");
			progress.Report("Loading traits..");
			Logger.Debug("Loading traits..");
			IApiV2ObjectList<Trait> traitResponse = await ((IAllExpandableClient<Trait>)(object)apiManager.get_Gw2ApiClient().get_V2().get_Traits()).AllAsync(cancellationToken);
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
			using Stream remappedSkillsStream = await _flurlClient.Request(_webRootUrl, "remapped_skills.json").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0);
			using ReadProgressStream progressStream = new ReadProgressStream(remappedSkillsStream);
			progressStream.ProgressChanged += delegate(object s, ReadProgressStream.ProgressChangedEventArgs e)
			{
				ReportProgress($"Reading remapped skills... {Math.Round(e.Progress, 0)}%");
			};
			JsonSerializer serializer = JsonSerializer.CreateDefault(_serializerSettings);
			using StreamReader sr = new StreamReader(progressStream);
			using JsonReader reader = new JsonTextReader(sr);
			foreach (RemappedSkillID remappedSkill in serializer.Deserialize<List<RemappedSkillID>>(reader)!)
			{
				ReportProgress($"Remapping skill from {remappedSkill.OriginalID} to {remappedSkill.DestinationID} ({remappedSkill.Comment})");
				List<Skill> skillsToRemap = skills.Where((Skill skill) => skill.Id == remappedSkill.OriginalID).ToList();
				Skill skillToInsert = skills.FirstOrDefault((Skill skill) => skill.Id == remappedSkill.DestinationID);
				if (skillToInsert != null)
				{
					skillToInsert = skillToInsert.CopyWithJson(_serializerSettings);
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
			using ReadProgressStream progressStream = new ReadProgressStream(await _flurlClient.Request(_webRootUrl, "missing_skills.json").GetStreamAsync(default(CancellationToken), (HttpCompletionOption)0));
			progressStream.ProgressChanged += delegate(object s, ReadProgressStream.ProgressChangedEventArgs e)
			{
				ReportProgress($"Reading missing skills... {Math.Round(e.Progress, 0)}%");
			};
			JsonSerializer serializer = JsonSerializer.CreateDefault(_serializerSettings);
			using StreamReader sr = new StreamReader(progressStream);
			using JsonReader reader = new JsonTextReader(sr);
			foreach (MissingSkill missingSkill in serializer.Deserialize<List<MissingSkill>>(reader)!)
			{
				SkillService skillService = this;
				object arg = missingSkill.ID;
				string name = missingSkill.Name;
				string[] nameAliases = missingSkill.NameAliases;
				skillService.ReportProgress($"Adding missing skill {arg} ({name}) with {((nameAliases != null) ? nameAliases.Length : 0)} aliases.");
				skills.Add(new Skill
				{
					Id = missingSkill.ID,
					Name = missingSkill.Name,
					Icon = missingSkill.Icon
				});
				Logger.Debug($"Added missing skill {missingSkill.ID} ({missingSkill.Name})");
				if (missingSkill.NameAliases != null)
				{
					string[] nameAliases2 = missingSkill.NameAliases;
					foreach (string alias in nameAliases2)
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
				skill.LoadTexture(_iconService);
			});
		}

		protected override void DoUpdate(GameTime gameTime)
		{
			UpdateUtil.UpdateAsync(SaveMissingSkills, gameTime, 60000.0, _lastSaveMissingSkill, doLogging: false);
		}

		private async Task SaveMissingSkills()
		{
			await FileUtil.WriteStringAsync(Path.Combine(base.DirectoryPath, "missingSkills.json"), JsonConvert.SerializeObject(_missingSkillsFromAPIReportedByArcDPS.OrderBy((MissingArcDPSSkill skill) => skill.ID), Formatting.Indented));
		}

		private async Task LoadMissingSkills()
		{
			string missingSkillPath = Path.Combine(base.DirectoryPath, "missingSkills.json");
			if (File.Exists(missingSkillPath))
			{
				try
				{
					_missingSkillsFromAPIReportedByArcDPS = JsonConvert.DeserializeObject<SynchronizedCollection<MissingArcDPSSkill>>(await FileUtil.ReadStringAsync(missingSkillPath));
				}
				catch (Exception)
				{
				}
			}
			else
			{
				_missingSkillsFromAPIReportedByArcDPS = new SynchronizedCollection<MissingArcDPSSkill>();
			}
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
			if (_missingSkillsFromAPIReportedByArcDPS == null || _missingSkillsFromAPIReportedByArcDPS.Any((MissingArcDPSSkill m) => m.ID == id))
			{
				return false;
			}
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
			_missingSkillsFromAPIReportedByArcDPS?.Add(new MissingArcDPSSkill
			{
				ID = id,
				Name = name,
				HintID = hintId
			});
			return true;
		}

		public void RemoveMissingSkill(int id)
		{
			List<MissingArcDPSSkill> items = _missingSkillsFromAPIReportedByArcDPS?.Where((MissingArcDPSSkill m) => m.ID == id).ToList();
			if (items == null || !items.Any())
			{
				return;
			}
			foreach (MissingArcDPSSkill item in items)
			{
				_missingSkillsFromAPIReportedByArcDPS?.Remove(item);
			}
		}
	}
}
