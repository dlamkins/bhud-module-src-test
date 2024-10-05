using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.DataModels.Professions;
using Kenedia.Modules.BuildsManager.Models;
using Kenedia.Modules.BuildsManager.Services;
using Kenedia.Modules.BuildsManager.Views;
using Kenedia.Modules.Core.DataModels;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.DataModels
{
	[DataContract]
	public class Race : IDisposable, IDataMember
	{
		private bool _isDisposed;

		private AsyncTexture2D _icon;

		private AsyncTexture2D _hoveredIcon;

		[DataMember]
		public Races Id { get; set; }

		public string Name
		{
			get
			{
				return Names.Text;
			}
			set
			{
				Names.Text = value;
			}
		}

		[DataMember]
		public LocalizedString Names { get; protected set; } = new LocalizedString();


		[DataMember]
		public Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> Skills { get; } = new Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill>();


		public AsyncTexture2D Icon
		{
			get
			{
				if (_icon != null)
				{
					return _icon;
				}
				_icon = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\races\\" + Id.ToString().ToLower() + ".png");
				return _icon;
			}
		}

		public AsyncTexture2D HoveredIcon
		{
			get
			{
				if (_hoveredIcon != null)
				{
					return _hoveredIcon;
				}
				_hoveredIcon = (AsyncTexture2D)BaseModule<BuildsManager, MainWindow, Settings, Paths>.ModuleInstance.ContentsManager.GetTexture("textures\\races\\" + Id.ToString().ToLower() + "_hovered.png");
				return _hoveredIcon;
			}
		}

		public Race()
		{
		}

		public Race(Gw2Sharp.WebApi.V2.Models.Race race)
		{
			Apply(race);
		}

		public Race(Gw2Sharp.WebApi.V2.Models.Race race, Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills)
			: this(race)
		{
			if (!Enum.TryParse<Races>(race.Id, out var _))
			{
				return;
			}
			foreach (int id in race.Skills)
			{
				if (skills.TryGetValue(id, out var skill))
				{
					skill.Categories = SkillCategoryType.Racial;
					Skills.Add(id, skill);
				}
			}
		}

		internal void UpdateLanguage(Gw2Sharp.WebApi.V2.Models.Race race, Dictionary<int, Kenedia.Modules.BuildsManager.DataModels.Professions.Skill> skills)
		{
			if (!Enum.TryParse<Races>(race.Id, out var _))
			{
				return;
			}
			Name = race.Name;
			foreach (int id in race.Skills)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill;
				bool exists = Skills.TryGetValue(id, out skill);
				if (skills.TryGetValue(id, out var allSkillsSkill))
				{
					if (skill == null)
					{
						skill = allSkillsSkill;
					}
					skill.Name = allSkillsSkill.Name;
					skill.Description = allSkillsSkill.Description;
					allSkillsSkill.Categories = SkillCategoryType.Racial;
					skill.Categories = SkillCategoryType.Racial;
					if (!exists)
					{
						Skills.Add(id, skill);
					}
				}
			}
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_icon = null;
				_hoveredIcon = null;
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Race race)
		{
			if (Enum.TryParse<Races>(race.Id, out var racetype))
			{
				Name = race.Name;
				Id = racetype;
			}
		}

		public void Apply(Gw2Sharp.WebApi.V2.Models.Race race, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> skills, IReadOnlyDictionary<int, int> skillsByPalette)
		{
			Apply(race);
			if (!Enum.TryParse<Races>(race.Id, out var _))
			{
				return;
			}
			foreach (int id in race.Skills)
			{
				Kenedia.Modules.BuildsManager.DataModels.Professions.Skill skill;
				bool num = Skills.TryGetValue(id, out skill);
				if (skill == null)
				{
					skill = new Kenedia.Modules.BuildsManager.DataModels.Professions.Skill();
				}
				skill.PaletteId = skillsByPalette.FirstOrDefault((KeyValuePair<int, int> e) => e.Value == id).Key;
				Gw2Sharp.WebApi.V2.Models.Skill apiSkill = skills.Where((Gw2Sharp.WebApi.V2.Models.Skill x) => x.Id == id).FirstOrDefault();
				if (apiSkill != null)
				{
					skill.Apply(apiSkill);
				}
				skill.Categories = SkillCategoryType.Racial;
				if (!num)
				{
					Skills.Add(id, skill);
				}
			}
		}
	}
}
