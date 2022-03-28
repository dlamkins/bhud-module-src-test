using System;
using System.Collections.Generic;
using System.IO;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Controls.Extern;
using Blish_HUD.Controls.Intern;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Blish_HUD.Modules.Managers;
using Gw2Sharp.Models;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace Kenedia.Modules.Characters
{
	public class Character
	{
		public ContentsManager contentsManager;

		public Gw2ApiManager apiManager;

		public int _mapid;

		public int _lastmapid;

		public bool logged_In_Once;

		public bool loaded;

		public List<CharacterCrafting> Crafting;

		public CharacterControl characterControl;

		public Tooltip tooltip;

		public Image classImage;

		public Label nameLabel;

		public Label timeLabel;

		public Image switchButton;

		public Image birthdayImage;

		public List<Image> craftingImages;

		public int apiIndex;

		public DateTimeOffset Created;

		public int map;

		public bool visible = true;

		public DateTime lastLogin;

		public DateTime LastModified;

		public Label checkbox;

		public Label timeSince;

		public Image icon;

		public List<string> Tags = new List<string>();

		public double seconds { get; set; }

		public string Name { get; set; }

		public int Level { get; set; }

		public RaceType Race { get; set; }

		public ProfessionType Profession { get; set; }

		public int _Profession { get; set; }

		public Specializations Specialization { get; set; }

		public int _Specialization { get; set; }

		public int spec { get; set; }

		public int mapid { get; set; }

		private void MainPanel_MouseLeft(object sender, MouseEventArgs e)
		{
			((Panel)sender).BackgroundTexture = null;
		}

		private void MainPanel_MouseEntered(object sender, MouseEventArgs e)
		{
			((Panel)sender).BackgroundTexture = Textures.Icons[20];
		}

		public void Create_UI_Elements()
		{
			ContentService contentService = new ContentService();
			characterControl = new CharacterControl
			{
				Parent = Module.CharacterPanel,
				Height = 60,
				Width = Module.CharacterPanel.Width - 20 - 5,
				ShowBorder = true,
				assignedCharacter = this,
				Tooltip = new CharacterTooltip
				{
					Parent = characterControl,
					assignedCharacter = this
				}
			};
			characterControl.Click += delegate
			{
				if (Module.subWindow.Visible)
				{
					if (!switchButton.MouseOver && Module.subWindow.assignedCharacter == this)
					{
						Module.subWindow.Hide();
					}
					if (Module.subWindow.assignedCharacter != this)
					{
						Module.subWindow.setCharacter(this);
					}
				}
				else if (!switchButton.MouseOver)
				{
					Module.subWindow.Show();
					Module.filterWindow.Hide();
					if (Module.subWindow.assignedCharacter != this)
					{
						Module.subWindow.setCharacter(this);
					}
				}
			};
			characterControl.MouseEntered += MainPanel_MouseEntered;
			characterControl.MouseLeft += MainPanel_MouseLeft;
			CharacterTooltip tooltp = (CharacterTooltip)characterControl.Tooltip;
			tooltp.Shown += delegate
			{
				tooltp._Update();
			};
			classImage = new Image
			{
				Location = new Point(0, 0),
				Texture = getProfessionTexture(),
				Size = new Point(48, 48),
				Parent = characterControl,
				Tooltip = tooltp
			};
			nameLabel = new Label
			{
				Location = new Point(53, 0),
				Text = Name,
				Parent = characterControl,
				Height = characterControl.Height / 2,
				Width = characterControl.Width - 165,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Regular),
				VerticalAlignment = VerticalAlignment.Middle,
				Tooltip = tooltp
			};
			new Image
			{
				Texture = Textures.Icons[19],
				Parent = characterControl,
				Location = new Point(48, characterControl.Height / 2 - 6),
				Size = new Point(characterControl.Width - 165, 4),
				Tooltip = tooltp
			};
			timeLabel = new Label
			{
				Location = new Point(53, characterControl.Height / 2 - 2),
				Text = "00:00:00",
				Parent = characterControl,
				Height = 16,
				Width = characterControl.Width - 165,
				Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size12, ContentService.FontStyle.Regular),
				VerticalAlignment = VerticalAlignment.Middle,
				Tooltip = tooltp
			};
			birthdayImage = new Image
			{
				Texture = Textures.Icons[17],
				Parent = characterControl,
				Location = new Point(characterControl.Width - 150, characterControl.Height / 2 - 2),
				Size = new Point(20, 20),
				Visible = false
			};
			if (Crafting.Count > 0)
			{
				FlowPanel craftingPanel = new FlowPanel
				{
					Location = new Point(characterControl.Width - 45 - 48 - 10, 0),
					Parent = characterControl,
					Height = characterControl.Height,
					Width = 55,
					FlowDirection = ControlFlowDirection.LeftToRight
				};
				string ttp = "";
				craftingImages = new List<Image>();
				foreach (CharacterCrafting crafting in Crafting)
				{
					if (crafting.Active)
					{
						craftingImages.Add(new Image
						{
							Texture = Textures.Crafting[crafting.Id],
							Size = new Point(24, 24),
							Parent = craftingPanel,
							Enabled = false
						});
						ttp = ttp + Enum.GetName(typeof(Crafting), crafting.Id) + " (" + crafting.Rating + ")" + Environment.NewLine;
					}
				}
				ttp = ttp.TrimEnd();
				foreach (Image craftingImage in craftingImages)
				{
					craftingImage.BasicTooltipText = ttp;
				}
				craftingPanel.BasicTooltipText = ttp;
			}
			switchButton = new Image
			{
				Location = new Point(characterControl.Width - 45, 10),
				Texture = Textures.Icons[12],
				Size = new Point(32, 32),
				Parent = characterControl,
				BasicTooltipText = string.Format(common.Switch, Name)
			};
			switchButton.Click += SwitchButton_Click;
			switchButton.MouseEntered += SwitchButton_MouseEntered;
			switchButton.MouseLeft += SwitchButton_MouseLeft;
			tooltp._Create();
			loaded = true;
		}

		private void SwitchButton_Click(object sender, MouseEventArgs e)
		{
			Swap();
		}

		private void SwitchButton_MouseLeft(object sender, MouseEventArgs e)
		{
			switchButton.Texture = Textures.Icons[12];
		}

		private void SwitchButton_MouseEntered(object sender, MouseEventArgs e)
		{
			switchButton.Texture = Textures.Icons[21];
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
					map = _mapid;
					((CharacterTooltip)characterControl.Tooltip)._Update();
					Save();
				}
				lastLogin = DateTime.UtcNow.AddSeconds(0.0);
				LastModified = DateTime.UtcNow.AddSeconds(1.0);
				Race = player.Race;
				birthdayImage.Visible = false;
				Update_UI_Time();
				UpdateProfession();
			}
		}

		public Texture2D getProfessionTexture()
		{
			if (_Specialization > 0)
			{
				return Textures.Specializations[_Specialization];
			}
			if (_Profession <= 9 && _Profession >= 1)
			{
				return Textures.Professions[_Profession];
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
				classImage.Texture = getProfessionTexture();
				Save();
			}
		}

		public void UpdateTooltips()
		{
		}

		public void Update_UI_Time()
		{
			if (!loaded)
			{
				return;
			}
			seconds = Math.Round(DateTime.UtcNow.Subtract(lastLogin).TotalSeconds);
			UpdateTooltips();
			TimeSpan t = TimeSpan.FromSeconds(seconds);
			if (timeLabel != null)
			{
				timeLabel.Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days);
			}
			if (!birthdayImage.Visible)
			{
				for (int i = 1; i < 100; i++)
				{
					DateTime birthDay = Created.AddYears(i).DateTime;
					if (!(birthDay <= DateTime.UtcNow))
					{
						break;
					}
					if (birthDay > lastLogin)
					{
						birthdayImage.Visible = true;
						birthdayImage.BasicTooltipText = Name + " had Birthday! They are now " + i + " years old.";
					}
				}
			}
			if (characterControl.Tooltip.Visible)
			{
				((CharacterTooltip)characterControl.Tooltip)._Update();
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
			if (Module.API_Account == null)
			{
				return;
			}
			List<JsonCharacter> _data = new List<JsonCharacter>();
			foreach (Character c in Module.Characters)
			{
				JsonCharacter jsonCharacter = new JsonCharacter
				{
					Name = c.Name,
					Race = c.Race,
					Specialization = c._Specialization,
					Profession = c._Profession,
					Crafting = c.Crafting,
					lastLogin = c.lastLogin,
					apiIndex = c.apiIndex,
					Created = c.Created,
					LastModified = c.LastModified,
					map = c.map,
					Level = c.Level,
					Tags = string.Join("|", c.Tags)
				};
				_data.Add(jsonCharacter);
			}
			string json = JsonConvert.SerializeObject(_data.ToArray());
			File.WriteAllText(Module.CharactersPath, json);
		}
	}
}
