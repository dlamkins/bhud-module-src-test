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
				CharacterControl obj = new CharacterControl(this);
				((Container)obj).set_WidthSizingMode((SizingMode)0);
				((Control)obj).set_Parent((Container)(object)Module.CharacterPanel);
				((Control)obj).set_Width(((Control)Module.CharacterPanel).get_Width() - 25);
				characterControl = obj;
			}
			loaded = true;
		}

		public async void UpdateCharacter()
		{
			if (!loaded || apiManager == null)
			{
				return;
			}
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (Name == player.get_Name())
			{
				_mapid = GameService.Gw2Mumble.get_CurrentMap().get_Id();
				if (_mapid > 0 && _mapid != _lastmapid)
				{
					_lastmapid = _mapid;
					Map = _mapid;
					((CharacterTooltip)(object)((Control)characterControl).get_Tooltip())._Update();
					Save();
				}
				lastLogin = DateTime.UtcNow.AddSeconds(0.0);
				LastModified = DateTime.UtcNow.AddSeconds(1.0);
				Race = player.get_Race();
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
				if (includeCustom && Icon != null && Icon != "" && Textures.CustomImages != null)
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
			PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
			if (Name == player.get_Name() && (_Specialization != player.get_Specialization() || Profession != player.get_Profession()))
			{
				if (DataManager._Specializations.Length > player.get_Specialization() && DataManager._Specializations[player.get_Specialization()] != null)
				{
					Specialization = (Specializations)player.get_Specialization();
					_Specialization = player.get_Specialization();
				}
				else
				{
					Specialization = (Specializations)0;
					_Specialization = 0;
				}
				Profession = player.get_Profession();
				_Profession = (int)player.get_Profession();
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
				((Control)characterControl).Show();
			}
		}

		public void Hide()
		{
			if (loaded)
			{
				visible = false;
				((Control)characterControl).Hide();
			}
		}

		public void Swap()
		{
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			if (!(GameService.Gw2Mumble.get_PlayerCharacter().get_Name() != Name) && GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
			{
				return;
			}
			if (!GameService.Gw2Mumble.get_CurrentMap().get_Type().IsCompetitive())
			{
				ScreenNotification.ShowNotification(string.Format(common.Switch, Name), (NotificationType)1, (Texture2D)null, 4);
				if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
				{
					for (int i = 0; i < Module.Characters.Count; i++)
					{
						Keyboard.Stroke((VirtualKeyShort)37, false);
					}
					foreach (Character character in Module.Characters)
					{
						if (character.Name != Name)
						{
							Keyboard.Stroke((VirtualKeyShort)39, false);
							continue;
						}
						if (Module.Settings.EnterOnSwap.get_Value())
						{
							Keyboard.Stroke((VirtualKeyShort)13, false);
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
					ModifierKeys mods = Module.Settings.LogoutKey.get_Value().get_ModifierKeys();
					VirtualKeyShort primary = (VirtualKeyShort)(short)Module.Settings.LogoutKey.get_Value().get_PrimaryKey();
					foreach (ModifierKeys mod2 in Enum.GetValues(typeof(ModifierKeys)))
					{
						if ((int)mod2 != 0 && ((Enum)mods).HasFlag((Enum)(object)mod2))
						{
							Keyboard.Press(Module.ModKeyMapping[mod2], false);
						}
					}
					Keyboard.Stroke(primary, false);
					foreach (ModifierKeys mod in Enum.GetValues(typeof(ModifierKeys)))
					{
						if ((int)mod != 0 && ((Enum)mods).HasFlag((Enum)(object)mod))
						{
							Keyboard.Release(Module.ModKeyMapping[mod], false);
						}
					}
					Keyboard.Stroke((VirtualKeyShort)13, false);
					Module.lastLogout = DateTime.UtcNow;
					Module.swapCharacter = this;
				}
			}
			else
			{
				ScreenNotification.ShowNotification(common.Error_Competivive, (NotificationType)2, (Texture2D)null, 4);
			}
		}

		public void Save()
		{
			Module.saveCharacters = true;
		}
	}
}
