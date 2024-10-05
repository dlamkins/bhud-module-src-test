using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Blish_HUD.Content;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi.V2;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.BuildsManager.DataModels.Items;
using Kenedia.Modules.BuildsManager.Interfaces;
using Kenedia.Modules.Core.Extensions;

namespace Kenedia.Modules.BuildsManager.DataModels.Professions
{
	[DataContract]
	public class Legend : IDisposable, IBaseApiData, IDataMember
	{
		private bool _isDisposed;

		public static (int, int, int, int, int, int) LegendaryAllianceLuxonIds { get; } = (62891, 62719, 62832, 62962, 62878, 62942);


		public static (int, int, int, int, int, int) LegendaryAllianceKurzickIds { get; } = (62749, 62680, 62702, 62941, 62796, 62687);


		public string Name
		{
			get
			{
				return Swap?.Name;
			}
			set
			{
				if (Swap != null)
				{
					Swap.Name = value;
				}
			}
		}

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public Dictionary<int, Skill> Utilities { get; set; } = new Dictionary<int, Skill>();


		[DataMember]
		public Skill Heal { get; set; }

		[DataMember]
		public Skill Elite { get; set; }

		[DataMember]
		public Skill Swap { get; set; }

		[DataMember]
		public int Specialization { get; set; }

		public string Description
		{
			get
			{
				return Swap?.Description;
			}
			set
			{
				if (Swap != null)
				{
					Swap.Description = value;
				}
			}
		}

		public AsyncTexture2D Icon => Swap?.Icon;

		public Legend()
		{
		}

		public Legend(Gw2Sharp.WebApi.V2.Models.Legend legend, Dictionary<int, Skill> skills)
		{
			Apply(legend, skills);
		}

		internal void ApplyLanguage(KeyValuePair<int, Legend> leg)
		{
			Heal.Name = leg.Value.Heal.Name;
			Heal.Description = leg.Value.Heal.Description;
			Swap.Name = leg.Value.Swap.Name;
			Swap.Description = leg.Value.Swap.Description;
			Elite.Name = leg.Value.Elite.Name;
			Elite.Description = leg.Value.Elite.Description;
			foreach (Skill ut in Utilities.Values)
			{
				if (leg.Value.Utilities.TryGetValue(ut.Id, out var utility))
				{
					utility.Name = ut.Name;
					utility.Description = ut.Description;
				}
			}
		}

		public static Legend FromByte(byte id)
		{
			if (new bool?(BuildsManager.Data.Professions[ProfessionType.Revenant].Legends.TryGetValue(id, out var legend)) != true)
			{
				return null;
			}
			return legend;
		}

		internal static Skill SkillFromUShort(ushort paletteId, Legend legend)
		{
			if (legend != null)
			{
				if (legend.Elite.PaletteId == paletteId)
				{
					return legend.Elite;
				}
				if (legend.Heal.PaletteId == paletteId)
				{
					return legend.Heal;
				}
				foreach (KeyValuePair<int, Skill> s in legend.Utilities)
				{
					if (paletteId == s.Value.PaletteId)
					{
						return s.Value;
					}
				}
			}
			return null;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Utilities?.Values?.DisposeAll();
				Utilities.Clear();
				Heal?.Dispose();
				Heal = null;
				Elite?.Dispose();
				Elite = null;
				Swap?.Dispose();
				Swap = null;
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Legend legend, Dictionary<int, Skill> skills)
		{
			if (!int.TryParse(legend.Id.Replace("Legend", ""), out var id) || !skills.TryGetValue(legend.Swap, out var skill))
			{
				return;
			}
			Id = id;
			Swap = skill;
			Specialization = skill.Specialization;
			if (skills.TryGetValue(legend.Heal, out var heal))
			{
				heal.PaletteId = Skill.GetRevPaletteId(heal);
				Heal = heal;
			}
			if (skills.TryGetValue(legend.Elite, out var elite))
			{
				elite.PaletteId = Skill.GetRevPaletteId(elite);
				Elite = elite;
			}
			foreach (int util in legend.Utilities)
			{
				if (skills.TryGetValue(util, out var utility))
				{
					utility.PaletteId = Skill.GetRevPaletteId(utility);
					Utilities.Add(utility.Id, utility);
				}
			}
		}

		internal void Apply(Gw2Sharp.WebApi.V2.Models.Legend legend, IApiV2ObjectList<Gw2Sharp.WebApi.V2.Models.Skill> skills)
		{
			Gw2Sharp.WebApi.V2.Models.Legend legend2 = legend;
			if (!int.TryParse(legend2.Id.Replace("Legend", ""), out var id))
			{
				return;
			}
			Gw2Sharp.WebApi.V2.Models.Skill swap = skills.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id == legend2.Swap);
			Gw2Sharp.WebApi.V2.Models.Skill heal = skills.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id == legend2.Heal);
			Gw2Sharp.WebApi.V2.Models.Skill elite = skills.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id == legend2.Elite);
			IEnumerable<Gw2Sharp.WebApi.V2.Models.Skill> utilities = skills.Where((Gw2Sharp.WebApi.V2.Models.Skill e) => legend2.Utilities.Contains(e.Id));
			Id = id;
			Specialization = (swap?.Specialization).GetValueOrDefault();
			if (Swap == null)
			{
				Skill obj = new Skill(swap)
				{
					PaletteId = Skill.GetRevPaletteId(swap)
				};
				Skill skill = obj;
				Swap = obj;
			}
			Swap.Apply(swap);
			if (Heal == null)
			{
				Skill obj2 = new Skill(heal)
				{
					PaletteId = Skill.GetRevPaletteId(heal)
				};
				Skill skill = obj2;
				Heal = obj2;
			}
			Heal.Apply(heal);
			if (Elite == null)
			{
				Skill obj3 = new Skill(elite)
				{
					PaletteId = Skill.GetRevPaletteId(elite)
				};
				Skill skill = obj3;
				Elite = obj3;
			}
			Elite.Apply(elite);
			Utilities = ((Utilities.Count == 0) ? utilities.ToDictionary((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id, (Gw2Sharp.WebApi.V2.Models.Skill e) => new Skill(e)) : Utilities);
			foreach (KeyValuePair<int, Skill> util in Utilities)
			{
				util.Value.Apply(utilities.FirstOrDefault((Gw2Sharp.WebApi.V2.Models.Skill e) => e.Id == util.Key));
				util.Value.PaletteId = Skill.GetRevPaletteId(util.Value);
			}
		}
	}
}
