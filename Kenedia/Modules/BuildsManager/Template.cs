using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Kenedia.Modules.BuildsManager
{
	public class Template
	{
		public enum _TrinketSlots
		{
			Back,
			Amulet,
			Ring1,
			Ring2,
			Accessoire1,
			Accessoire2
		}

		public enum _AmorSlots
		{
			Helmet,
			Shoulders,
			Chest,
			Gloves,
			Leggings,
			Boots
		}

		public enum _WeaponSlots
		{
			Weapon1_MainHand,
			Weapon1_OffHand,
			Weapon2_MainHand,
			Weapon2_OffHand
		}

		private enum _AquaticWeaponSlots
		{
			AquaticWeapon1,
			AquaticWeapon2
		}

		public Template_json Template_json;

		private string _Name;

		public API.Profession Profession;

		public API.Specialization Specialization;

		public string Path;

		public GearTemplate Gear = new GearTemplate();

		public BuildTemplate Build;

		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (_Name == null || Path != null)
				{
					_Name = value;
				}
			}
		}

		public event EventHandler Edit;

		public event EventHandler Deleted;

		public Template(string name, string build, string gear)
		{
			Template_json = new Template_json
			{
				Name = name,
				BuildCode = build,
				GearCode = gear
			};
			Name = name;
			Path = BuildsManager.Paths.builds + "Builds.json";
			Build = new BuildTemplate(Template_json.BuildCode);
			Gear = new GearTemplate(Template_json.GearCode);
			Profession = BuildsManager.Data.Professions.Find((API.Profession e) => e.Id == Build?.Profession?.Id);
			Specialization = ((Profession == null) ? null : Build.SpecLines.Find((SpecLine e) => e.Specialization?.Elite ?? false)?.Specialization);
		}

		public Template(string path = null)
		{
			if (path != null && File.Exists(path))
			{
				Template_json template = JsonConvert.DeserializeObject<Template_json>(File.ReadAllText(path));
				if (template != null)
				{
					Template_json = template;
					Name = template.Name;
					Path = BuildsManager.Paths.builds + "Builds.json";
					Build = new BuildTemplate(Template_json.BuildCode);
					Gear = new GearTemplate(Template_json.GearCode);
					Profession = BuildsManager.Data.Professions.Find((API.Profession e) => e.Id == Build?.Profession.Id);
					Specialization = ((Profession == null) ? null : Build.SpecLines.Find((SpecLine e) => e.Specialization.Elite)?.Specialization);
				}
			}
			else
			{
				Gear = new GearTemplate();
				Build = new BuildTemplate();
				Name = "[No Name Set]";
				Template_json = new Template_json();
				Profession = BuildsManager.ModuleInstance.CurrentProfession;
				Path = BuildsManager.Paths.builds + "Builds.json";
				SetChanged();
			}
		}

		public void Reset()
		{
			Name = "[No Name Set]";
			Template_json = new Template_json
			{
				Name = Name
			};
			Specialization = null;
			foreach (TemplateItem trinket in Gear.Trinkets)
			{
				trinket.Stat = null;
			}
			foreach (Armor_TemplateItem item in Gear.Armor)
			{
				item.Stat = null;
				item.Rune = null;
			}
			foreach (Weapon_TemplateItem weapon in Gear.Weapons)
			{
				weapon.WeaponType = API.weaponType.Unkown;
				weapon.Stat = null;
				weapon.Sigil = null;
			}
			foreach (AquaticWeapon_TemplateItem aquaticWeapon in Gear.AquaticWeapons)
			{
				aquaticWeapon.WeaponType = API.weaponType.Unkown;
				aquaticWeapon.Stat = null;
				aquaticWeapon.Sigils = new List<API.SigilItem>
				{
					new API.SigilItem(),
					new API.SigilItem()
				};
			}
			Build = new BuildTemplate();
			Profession = BuildsManager.ModuleInstance.CurrentProfession;
			Path = BuildsManager.Paths.builds;
			SetChanged();
		}

		public void Delete()
		{
			if (Name == "[No Name Set]")
			{
				BuildsManager.ModuleInstance.Templates.Remove(this);
				BuildsManager.ModuleInstance.OnTemplate_Deleted();
			}
			else if (Path != null)
			{
				BuildsManager.ModuleInstance.Templates.Remove(this);
				if (Path != null && Name != null)
				{
					Save();
					BuildsManager.ModuleInstance.OnTemplate_Deleted();
					this.Deleted?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public void Save_Unformatted()
		{
			if (Path == null || Name == null || Name == "[No Name Set]")
			{
				return;
			}
			string path = Path;
			BuildsManager.Logger.Debug("Saving: {0} in {1}.", new object[2] { Name, Path });
			FileInfo fi = null;
			try
			{
				fi = new FileInfo(path);
			}
			catch (ArgumentException)
			{
			}
			catch (PathTooLongException)
			{
			}
			catch (NotSupportedException)
			{
			}
			if (Name.Contains("/") || Name.Contains("\\") || fi == null)
			{
				ScreenNotification.ShowNotification(Name + " is not a valid Name!", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			Template_json.Name = Name;
			Template_json.BuildCode = Build?.ParseBuildCode();
			Template_json.GearCode = Gear?.TemplateCode;
			BuildsManager.getCultureString();
			File.WriteAllText(Path, JsonConvert.SerializeObject((object)(from e in BuildsManager.ModuleInstance.Templates
				where e.Path == Path
				select e into a
				select a.Template_json).ToList()));
		}

		public void Save()
		{
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Expected O, but got Unknown
			if (Path == null || Name == null || Name == "[No Name Set]")
			{
				return;
			}
			string path = Path;
			BuildsManager.Logger.Debug("Saving: {0} in {1}.", new object[2] { Name, Path });
			FileInfo fi = null;
			try
			{
				fi = new FileInfo(path);
			}
			catch (ArgumentException)
			{
			}
			catch (PathTooLongException)
			{
			}
			catch (NotSupportedException)
			{
			}
			if (Name.Contains("/") || Name.Contains("\\") || fi == null)
			{
				ScreenNotification.ShowNotification(Name + " is not a valid Name!", (NotificationType)2, (Texture2D)null, 4);
				return;
			}
			Template_json.Name = Name;
			Template_json.BuildCode = Build?.ParseBuildCode();
			Template_json.GearCode = Gear?.TemplateCode;
			StringBuilder sb = new StringBuilder();
			bool first = true;
			foreach (Template_json template in (from e in BuildsManager.ModuleInstance.Templates
				where e.Path == Path
				select e into a
				select a.Template_json).ToList())
			{
				StringWriter stringWriter = new StringWriter(sb);
				if (!first)
				{
					sb.Append("," + Environment.NewLine);
				}
				JsonWriter writer = (JsonWriter)new JsonTextWriter((TextWriter)stringWriter);
				try
				{
					writer.set_Formatting((Formatting)1);
					writer.WriteStartObject();
					writer.WritePropertyName("Name");
					writer.WriteValue(template.Name);
					writer.WritePropertyName("BuildCode");
					writer.WriteValue(template.BuildCode);
					writer.WritePropertyName("GearCode");
					writer.WriteValue(template.GearCode);
					writer.WriteEndObject();
				}
				finally
				{
					((IDisposable)writer)?.Dispose();
				}
				first = false;
			}
			File.WriteAllText(Path, "[" + sb.ToString() + "]");
		}

		private void OnEdit(object sender, EventArgs e)
		{
			this.Edit?.Invoke(this, EventArgs.Empty);
			Save();
		}

		public void SetChanged()
		{
			OnEdit(null, EventArgs.Empty);
		}
	}
}
