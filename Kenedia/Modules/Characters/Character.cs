using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Kenedia.Modules.Characters
{
	public class Character
	{
		public ContentsManager contentsManager;

		public Gw2ApiManager apiManager;

		public int _mapid;

		public int _lastmapid;

		public int Years;

		public bool logged_In_Once;

		public bool loaded;

		public List<CharacterCrafting> Crafting;

		public CharacterControl characterControl;

		public int apiIndex;

		public DateTimeOffset Created;

		public DateTime NextBirthday;

		public int Map;

		public bool loginCharacter;

		public bool include = true;

		public bool visible = true;

		public DateTime lastLogin;

		public DateTime LastModified;

		public Label checkbox;

		public Label timeSince;

		public Image icon;

		public List<string> Tags = new List<string>();

		public double seconds { get; set; }

		public string Name { get; set; }

		public string Icon { get; set; }

		public int Level { get; set; }

		public RaceType Race { get; set; }

		public ProfessionType Profession { get; set; }

		public int _Profession { get; set; }

		public Specializations Specialization { get; set; }

		public int _Specialization { get; set; }

		public int spec { get; set; }

		public int mapid { get; set; }

		public bool hadBirthdaySinceLogin()
		{
			for (int i = 1; i < 100; i++)
			{
				DateTime birthDay = Created.AddYears(i).DateTime;
				_ = NextBirthday;
				if (NextBirthday == Module.dateZero && birthDay >= DateTime.UtcNow)
				{
					NextBirthday = birthDay;
					Years = i;
				}
				if (birthDay <= DateTime.UtcNow)
				{
					if (birthDay > lastLogin)
					{
						return true;
					}
					continue;
				}
				return false;
			}
			return false;
		}

		public void Create_UI_Elements()
		{
			if (!loaded)
			{
				characterControl = new CharacterControl(this)
				{
					WidthSizingMode = SizingMode.Standard,
					Parent = Module.CharacterPanel,
					Width = Module.CharacterPanel.Width - 25
				};
			}
			loaded = true;
		}

		public async void UpdateCharacter()
		{
			if (!loaded || apiManager == null)
			{
				return;
			}
			PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
			if (Name == player.Name)
			{
				_mapid = GameService.Gw2Mumble.CurrentMap.Id;
				if (_mapid > 0 && _mapid != _lastmapid)
				{
					_lastmapid = _mapid;
					Map = _mapid;
					((CharacterTooltip)characterControl.Tooltip)._Update();
					Save();
				}
				lastLogin = DateTime.UtcNow.AddSeconds(0.0);
				LastModified = DateTime.UtcNow.AddSeconds(1.0);
				Race = player.Race;
				characterControl.UpdateUI();
				Update_UI_Time();
				UpdateProfession();
			}
		}

		public Texture2D getProfessionTexture(bool includeCustom = true, bool baseIcons = false)
		{
			if (baseIcons)
			{
				if (_Specialization > 0)
				{
					return Textures.SpecializationsWhite[_Specialization];
				}
				if (_Profession <= 9 && _Profession >= 1)
				{
					return Textures.ProfessionsWhite[_Profession];
				}
			}
			else
			{
				if (includeCustom && Icon != null && Icon != "")
				{
					Texture2D[] customImages = Textures.CustomImages;
					foreach (Texture2D Texture in customImages)
					{
						if (Texture != null && Texture.Name == Icon)
						{
							return Texture;
						}
					}
				}
				if (_Specialization > 0)
				{
					return Textures.Specializations[_Specialization];
				}
				if (_Profession <= 9 && _Profession >= 1)
				{
					return Textures.Professions[_Profession];
				}
			}
			return Textures.Icons[1];
		}

		public Texture2D getRaceTexture()
		{
			if (Race.ToString() != "")
			{
				return Textures.Races[(uint)Race];
			}
			return Textures.Icons[1];
		}

		public void UpdateProfession()
		{
			PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
			if (Name == player.Name && (_Specialization != player.Specialization || Profession != player.Profession))
			{
				if (DataManager._Specializations.Length > player.Specialization && DataManager._Specializations[player.Specialization] != null)
				{
					Specialization = (Specializations)player.Specialization;
					_Specialization = player.Specialization;
				}
				else
				{
					Specialization = (Specializations)0;
					_Specialization = 0;
				}
				Profession = player.Profession;
				_Profession = (int)player.Profession;
				characterControl.UpdateUI();
				Save();
			}
		}

		public void UpdateLanguage()
		{
			characterControl.UpdateLanguage();
		}

		public void Update_UI_Time()
		{
			if (loaded)
			{
				seconds = Math.Round(DateTime.UtcNow.Subtract(lastLogin).TotalSeconds);
				characterControl.UpdateUI();
			}
		}

		public void Show()
		{
			if (loaded)
			{
				visible = true;
				characterControl.Show();
			}
		}

		public void Hide()
		{
			if (loaded)
			{
				visible = false;
				characterControl.Hide();
			}
		}

		public void Swap()
		{
			if (!(GameService.Gw2Mumble.PlayerCharacter.Name != Name) && GameService.GameIntegration.Gw2Instance.IsInGame)
			{
				return;
			}
			if (!GameService.Gw2Mumble.CurrentMap.Type.IsCompetitive())
			{
				ScreenNotification.ShowNotification(string.Format(common.Switch, Name), ScreenNotification.NotificationType.Warning);
				if (!GameService.GameIntegration.Gw2Instance.IsInGame)
				{
					for (int i = 0; i < Module.Characters.Count; i++)
					{
						Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.LEFT);
					}
					foreach (Character character in Module.Characters)
					{
						if (character.Name != Name)
						{
							Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RIGHT);
							continue;
						}
						if (Module.Settings.EnterOnSwap.Value)
						{
							Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RETURN);
						}
						break;
					}
				}
				else
				{
					if (!(DateTime.UtcNow.Subtract(Module.lastLogout).TotalSeconds > 1.0))
					{
						return;
					}
					ModifierKeys mods = Module.Settings.LogoutKey.Value.ModifierKeys;
					VirtualKeyShort primary = (VirtualKeyShort)Module.Settings.LogoutKey.Value.PrimaryKey;
					foreach (ModifierKeys mod2 in Enum.GetValues(typeof(ModifierKeys)))
					{
						if (mod2 != 0 && mods.HasFlag(mod2))
						{
							Blish_HUD.Controls.Intern.Keyboard.Press(Module.ModKeyMapping[(int)mod2]);
						}
					}
					Blish_HUD.Controls.Intern.Keyboard.Stroke(primary);
					foreach (ModifierKeys mod in Enum.GetValues(typeof(ModifierKeys)))
					{
						if (mod != 0 && mods.HasFlag(mod))
						{
							Blish_HUD.Controls.Intern.Keyboard.Release(Module.ModKeyMapping[(int)mod]);
						}
					}
					Blish_HUD.Controls.Intern.Keyboard.Stroke(VirtualKeyShort.RETURN);
					Module.lastLogout = DateTime.UtcNow;
					Module.swapCharacter = this;
				}
			}
			else
			{
				ScreenNotification.ShowNotification(common.Error_Competivive, ScreenNotification.NotificationType.Error);
			}
		}

		public void Save()
		{
			Module.saveCharacters = true;
		}
	}
}
