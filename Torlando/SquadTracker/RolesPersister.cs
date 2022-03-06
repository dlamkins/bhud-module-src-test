using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Torlando.SquadTracker
{
	public static class RolesPersister
	{
		private const string ROLES_FILE_NAME = "roles.json";

		public static async Task<ObservableCollection<Role>> LoadRolesFromFileSystem(string directoryPath)
		{
			string rolesFilePath = Path.Combine(directoryPath, "roles.json");
			ObservableCollection<Role> roles;
			if (!File.Exists(rolesFilePath))
			{
				roles = new ObservableCollection<Role>
				{
					new Role("Quickness")
					{
						IconPath = "icons\\quickness.png"
					},
					new Role("Alacrity")
					{
						IconPath = "icons\\alacrity.png"
					},
					new Role("Heal")
					{
						IconPath = "icons\\regeneration.png"
					},
					new Role("Power DPS")
					{
						IconPath = "icons\\power.png"
					},
					new Role("Condi DPS")
					{
						IconPath = "icons\\Condition_Damage.png"
					}
				};
				await SaveRoles(roles, rolesFilePath);
			}
			else
			{
				roles = new ObservableCollection<Role>(await LoadRoles(rolesFilePath));
			}
			roles.CollectionChanged += async delegate
			{
				await SaveRoles(roles, rolesFilePath);
			};
			return roles;
		}

		private static async Task<IEnumerable<Role>> LoadRoles(string filePath)
		{
			return JsonConvert.DeserializeObject<IEnumerable<Role>>(File.ReadAllText(filePath));
		}

		private static async Task SaveRoles(IEnumerable<Role> roles, string filePath)
		{
			string json = JsonConvert.SerializeObject(roles);
			File.WriteAllText(filePath, json);
		}
	}
}
