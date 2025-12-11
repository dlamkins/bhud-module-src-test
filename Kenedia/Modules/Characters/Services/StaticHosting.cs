using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blish_HUD;
using Kenedia.Modules.Core.Converter;
using Kenedia.Modules.Core.Services;
using Newtonsoft.Json;
using SemVer;

namespace Kenedia.Modules.Characters.Services
{
	public class StaticHosting : Kenedia.Modules.Core.Services.StaticHosting
	{
		public class Versions
		{
			[JsonConverter(typeof(SemverVersionConverter))]
			public Version Professions { get; set; } = new Version("0.0.0", false);


			[JsonConverter(typeof(SemverVersionConverter))]
			public Version Specializations { get; set; } = new Version("0.0.0", false);


			[JsonConverter(typeof(SemverVersionConverter))]
			public Version Races { get; set; } = new Version("0.0.0", false);


			[JsonConverter(typeof(SemverVersionConverter))]
			public Version CraftingProfessions { get; set; } = new Version("0.0.0", false);


			[JsonConverter(typeof(SemverVersionConverter))]
			public Version Maps { get; set; } = new Version("0.0.0", false);


			[JsonIgnore]
			public Version this[string key]
			{
				get
				{
					//IL_0047: Unknown result type (might be due to invalid IL or missing references)
					//IL_004d: Expected O, but got Unknown
					string key2 = key;
					PropertyInfo property = typeof(Versions).GetProperties().FirstOrDefault((PropertyInfo p) => p.Name.ToLower() == key2.ToLower());
					if ((object)property != null)
					{
						object value = property.GetValue(this);
						Version version = (Version)((value is Version) ? value : null);
						if (version != null)
						{
							return version;
						}
					}
					return new Version("0.0.0", false);
				}
			}
		}

		public override string BaseUrl { get; } = "https://bhm.blishhud.com/Kenedia.Modules.Characters/";


		public StaticHosting(Logger logger)
			: base(logger)
		{
		}

		public async Task<Versions> GetStaticVersions()
		{
			try
			{
				return await GetStaticContent<Versions>("Versions.json");
			}
			catch (Exception ex)
			{
				base.Logger.Warn($"{ex}");
			}
			return new Versions();
		}
	}
}
