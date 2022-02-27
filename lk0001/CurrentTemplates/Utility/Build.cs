using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gw2Sharp.WebApi.V2.Models;

namespace lk0001.CurrentTemplates.Utility
{
	internal class Build
	{
		public string Name;

		public string ChatCode;

		public Constants.Profession Profession;

		public List<BuildSpecialization> Specializations;

		private string SerializedBuild;

		private static readonly Regex chatCodeRegex = new Regex("\\[&((?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?)\\]", RegexOptions.Compiled);

		public Build(string name, string chatCode)
		{
			Name = name;
			ChatCode = chatCode;
			MatchCollection matches = chatCodeRegex.Matches(chatCode);
			if (matches.Count == 1 && matches[0].Groups.Count == 2)
			{
				string byteString = Encoding.UTF8.GetString(Convert.FromBase64String(matches[0].Groups[1].Value));
				int[] bytes = new int[byteString.Length];
				for (int i = 0; i < byteString.Length; i++)
				{
					bytes[i] = byteString[i];
				}
				if (bytes.Length != 0 && bytes[0] != 13)
				{
					throw new Exception("Wrong header type");
				}
				if (bytes.Length >= 44)
				{
					Profession = (Constants.Profession)bytes[1];
					Specializations = new List<BuildSpecialization>();
					for (int s = 0; s < 3; s++)
					{
						int offset = s * 2;
						int[] traits = new int[3];
						for (int t = 0; t < 3; t++)
						{
							traits[t] = (bytes[offset + 3] >> t * 2) & 3;
						}
						Specializations.Add(new BuildSpecialization((Constants.Specialization)bytes[offset + 2], traits));
					}
					Specializations.Sort((BuildSpecialization x, BuildSpecialization y) => x.Id.CompareTo(y.Id));
					SerializedBuild = Serialize();
					return;
				}
				throw new Exception("Invalid build template");
			}
			throw new Exception("Invalid format");
		}

		public bool EquivalentTo(IReadOnlyList<BuildTemplateSpecialization> specializations)
		{
			return SerializedBuild == SerializeFromApi(specializations);
		}

		public string Serialize()
		{
			return string.Join("-", Specializations.Select(delegate(BuildSpecialization x)
			{
				int id = (int)x.Id;
				return id + "-" + string.Join("-", x.Traits.Select((int y) => y.ToString()));
			}));
		}

		public static string SerializeFromApi(IReadOnlyList<BuildTemplateSpecialization> specializations)
		{
			try
			{
				List<BuildTemplateSpecialization> specs = new List<BuildTemplateSpecialization>();
				for (int i = 0; i < 3; i++)
				{
					if (!specializations[i].get_Id().HasValue)
					{
						return "empty";
					}
					specs.Add(specializations[i]);
				}
				specs.Sort((BuildTemplateSpecialization x, BuildTemplateSpecialization y) => x.get_Id().Value.CompareTo(y.get_Id().Value));
				return string.Join("-", specs.Select((BuildTemplateSpecialization x) => x.get_Id() + "-" + string.Join("-", from y in x.get_Traits()
					select y.ToString())));
			}
			catch (Exception ex)
			{
				Module.Logger.Debug(ex, "Failed to serialize build from API.");
				return "error";
			}
		}
	}
}
