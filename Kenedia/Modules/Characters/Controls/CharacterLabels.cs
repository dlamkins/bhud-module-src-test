using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterLabels : IDisposable
	{
		private readonly IconLabel _nameLabel;

		private readonly IconLabel _levelLabel;

		private readonly IconLabel _professionLabel;

		private readonly IconLabel _raceLabel;

		private readonly IconLabel _genderLabel;

		private readonly IconLabel _mapLabel;

		private readonly CraftingControl _craftingControl;

		private readonly IconLabel _lastLoginLabel;

		private readonly IconLabel _nextBirthdayLabel;

		private readonly IconLabel _ageLabel;

		private readonly IconLabel _customIndex;

		private Character_Model _character;

		private readonly List<Tag> _tags = new List<Tag>();

		private readonly bool _created;

		private TextureManager _textureManager;

		private Data _data;

		private Settings _settings;

		public TagFlowPanel TagPanel { get; }

		public BitmapFont NameFont { get; set; } = GameService.Content.DefaultFont14;


		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont14;


		public Character_Model Character
		{
			get
			{
				return _character;
			}
			set
			{
				Common.SetProperty(ref _character, value);
			}
		}

		public List<Control> DataControls { get; } = new List<Control>();


		public Data Data
		{
			get
			{
				return _data;
			}
			set
			{
				Common.SetProperty(ref _data, value, new PropertyChangedEventHandler(OnDataChanged), triggerOnUpdate: true, "Data");
			}
		}

		public Settings Settings
		{
			get
			{
				return _settings;
			}
			set
			{
				Settings temp = _settings;
				if (Common.SetProperty(ref _settings, value, new PropertyChangedEventHandler(OnSettingsChanged), triggerOnUpdate: true, "Settings"))
				{
					if (temp != null)
					{
						temp.AppearanceSettingChanged -= new EventHandler(Settings_AppearanceSettingChanged);
					}
					if (_settings != null)
					{
						_settings.AppearanceSettingChanged += new EventHandler(Settings_AppearanceSettingChanged);
					}
				}
			}
		}

		public TextureManager TextureManager
		{
			get
			{
				return _textureManager;
			}
			set
			{
				Common.SetProperty(ref _textureManager, value, new PropertyChangedEventHandler(OnTextureManagerAdded), triggerOnUpdate: true, "TextureManager");
			}
		}

		public Func<Character_Model> CurrentCharacter { get; set; }

		public Kenedia.Modules.Core.Controls.FlowPanel Parent { get; }

		public CharacterLabels(Kenedia.Modules.Core.Controls.FlowPanel parent)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			Parent = parent;
			_nameLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				TextColor = ContentService.Colors.ColonialWhite
			};
			_levelLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Icon = AsyncTexture2D.FromAssetId(157085),
				TextureRectangle = new Rectangle(2, 2, 28, 28)
			};
			_genderLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true
			};
			_raceLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true
			};
			_professionLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true
			};
			_mapLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Icon = AsyncTexture2D.FromAssetId(358406),
				TextureRectangle = new Rectangle(2, 2, 28, 28)
			};
			_craftingControl = new CraftingControl
			{
				Parent = parent,
				Width = parent.Width,
				Height = 20,
				Character = Character
			};
			_ageLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Icon = AsyncTexture2D.FromAssetId(1424243),
				TextureRectangle = new Rectangle(2, 2, 28, 28)
			};
			_nextBirthdayLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Icon = AsyncTexture2D.FromAssetId(593864),
				TextureRectangle = new Rectangle(2, 2, 28, 28)
			};
			_lastLoginLabel = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				Icon = AsyncTexture2D.FromAssetId(155035),
				TextureRectangle = new Rectangle(10, 10, 44, 44)
			};
			_customIndex = new IconLabel
			{
				Parent = parent,
				AutoSizeWidth = true,
				AutoSizeHeight = true,
				TextureRectangle = new Rectangle(2, 2, 28, 28),
				Icon = AsyncTexture2D.FromAssetId(156909)
			};
			TagPanel = new TagFlowPanel
			{
				Parent = parent,
				Font = _lastLoginLabel.Font,
				FlowDirection = ControlFlowDirection.LeftToRight,
				ControlPadding = new Vector2(3f, 2f),
				HeightSizingMode = SizingMode.AutoSize,
				Visible = false
			};
			DataControls = new List<Control>
			{
				_nameLabel, _customIndex, _levelLabel, _genderLabel, _raceLabel, _professionLabel, _mapLabel, _nextBirthdayLabel, _ageLabel, _lastLoginLabel,
				_craftingControl, TagPanel
			};
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
			UpdateDataControlsVisibility();
			_created = true;
		}

		private void Settings_AppearanceSettingChanged(object sender, EventArgs e)
		{
			UpdateDataControlsVisibility();
		}

		private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
		{
			_craftingControl.Settings = _settings;
			if (_settings != null)
			{
				RecalculateBounds();
			}
		}

		private void OnDataChanged(object sender, PropertyChangedEventArgs e)
		{
			_craftingControl.Data = _data;
		}

		private void OnTextureManagerAdded(object sender, PropertyChangedEventArgs e)
		{
			if (_genderLabel != null)
			{
				_genderLabel.Icon = (AsyncTexture2D)(_textureManager?.GetIcon(Kenedia.Modules.Characters.Services.TextureManager.Icons.Gender));
			}
		}

		public void RecalculateBounds()
		{
			UpdateDataControlsVisibility();
			TagPanel.FitWidestTag(DataControls.Max((Control e) => (e.Visible && e != TagPanel) ? e.Width : 0));
		}

		public void Dispose()
		{
			DataControls?.DisposeAll();
			LocalizingService.LocaleChanged -= new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}

		public void UpdateCharacterInfo()
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			if (Character == null)
			{
				return;
			}
			_nameLabel.Text = Character.Name;
			_levelLabel.Text = string.Format(strings.LevelAmount, Character.Level);
			_professionLabel.Icon = Character.SpecializationIcon;
			_professionLabel.Text = Character.SpecializationName;
			if (_professionLabel.Icon != null)
			{
				_professionLabel.TextureRectangle = ((_professionLabel.Icon.Width == 32) ? new Rectangle(2, 2, 28, 28) : new Rectangle(4, 4, 56, 56));
			}
			IconLabel genderLabel = _genderLabel;
			Gender gender = Character.Gender;
			genderLabel.Text = ((object)(Gender)(ref gender)).ToString();
			_raceLabel.Text = _data.Races[Character.Race].Name;
			_raceLabel.Icon = _data.Races[Character.Race].Icon;
			_mapLabel.Text = _data.GetMapById(Character.Map).Name;
			_customIndex.Text = string.Format(strings.CustomIndex + " {0}", Character.Index);
			IEnumerable<string> tagLlist = _tags.Select((Tag e) => e.Text);
			List<string> characterTags = Character.Tags.ToList();
			IEnumerable<string> deleteTags = tagLlist.Except(characterTags);
			IEnumerable<string> addTags = characterTags.Except(tagLlist);
			if (deleteTags.Any() || addTags.Any())
			{
				List<Tag> deleteList = new List<Tag>();
				foreach (string tag2 in deleteTags)
				{
					Tag t2 = _tags.FirstOrDefault((Tag e) => e.Text == tag2);
					if (t2 != null)
					{
						deleteList.Add(t2);
					}
				}
				foreach (Tag t in deleteList)
				{
					t.Dispose();
					_tags.Remove(t);
				}
				foreach (string tag in addTags)
				{
					_tags.Add(new Tag
					{
						Parent = TagPanel,
						Text = tag,
						Active = true,
						ShowDelete = false,
						CanInteract = false
					});
				}
				TagPanel.FitWidestTag(DataControls.Max((Control e) => (e.Visible && e != TagPanel) ? e.Width : 0));
			}
			_craftingControl.Character = Character;
		}

		public void UpdateDataControlsVisibility(bool tooltip = false)
		{
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			if (_settings != null)
			{
				Dictionary<string, ShowCheckPair> settings = _settings.DisplayToggles.Value;
				NameFont = GetFont(nameFont: true);
				Font = GetFont();
				_nameLabel.Visible = !settings.TryGetValue("Name", out var name) || (tooltip ? name.ShowTooltip : name.Show);
				_nameLabel.Font = NameFont;
				_levelLabel.Visible = !settings.TryGetValue("Level", out var level) || (tooltip ? level.ShowTooltip : level.Show);
				_levelLabel.Font = Font;
				_genderLabel.Visible = !settings.TryGetValue("Gender", out var gender) || (tooltip ? gender.ShowTooltip : gender.Show);
				_genderLabel.Font = Font;
				_raceLabel.Visible = !settings.TryGetValue("Race", out var race) || (tooltip ? race.ShowTooltip : race.Show);
				_raceLabel.Font = Font;
				_professionLabel.Visible = !settings.TryGetValue("Profession", out var profession) || (tooltip ? profession.ShowTooltip : profession.Show);
				_professionLabel.Font = Font;
				_lastLoginLabel.Visible = !settings.TryGetValue("LastLogin", out var lastlogin) || (tooltip ? lastlogin.ShowTooltip : lastlogin.Show);
				_lastLoginLabel.Font = Font;
				_ageLabel.Visible = !settings.TryGetValue("Age", out var age) || (tooltip ? age.ShowTooltip : age.Show);
				_ageLabel.Font = Font;
				_nextBirthdayLabel.Visible = !settings.TryGetValue("NextBirthday", out var nextbirthday) || (tooltip ? nextbirthday.ShowTooltip : nextbirthday.Show);
				_nextBirthdayLabel.Font = Font;
				_mapLabel.Visible = !settings.TryGetValue("Map", out var map) || (tooltip ? map.ShowTooltip : map.Show);
				_mapLabel.Font = Font;
				_craftingControl.Visible = !settings.TryGetValue("CraftingProfession", out var craftingprofession) || (tooltip ? craftingprofession.ShowTooltip : craftingprofession.Show);
				_craftingControl.Font = Font;
				_customIndex.Visible = !settings.TryGetValue("CustomIndex", out var customindex) || (tooltip ? customindex.ShowTooltip : customindex.Show);
				_customIndex.Font = Font;
				TagPanel.Visible = (!settings.TryGetValue("Tags", out var tags) || (tooltip ? tags.ShowTooltip : tags.Show)) && (Character?.Tags.Count ?? 0) > 0;
				TagPanel.Font = Font;
				_craftingControl.Height = Font.get_LineHeight() + 2;
				if (Parent != null)
				{
					Parent.ControlPadding = new Vector2((float)(Font.get_LineHeight() / 10), (float)(Font.get_LineHeight() / 10));
					Parent?.Invalidate();
				}
			}
		}

		private BitmapFont GetFont(bool nameFont = false)
		{
			ContentService.FontSize fontSize = ContentService.FontSize.Size8;
			if (_settings == null)
			{
				return GameService.Content.DefaultFont12;
			}
			switch (_settings.PanelSize.Value)
			{
			case Kenedia.Modules.Characters.Services.Settings.PanelSizes.Small:
				fontSize = (nameFont ? ContentService.FontSize.Size16 : ContentService.FontSize.Size12);
				break;
			case Kenedia.Modules.Characters.Services.Settings.PanelSizes.Normal:
				fontSize = (nameFont ? ContentService.FontSize.Size18 : ContentService.FontSize.Size14);
				break;
			case Kenedia.Modules.Characters.Services.Settings.PanelSizes.Large:
				fontSize = (nameFont ? ContentService.FontSize.Size22 : ContentService.FontSize.Size18);
				break;
			case Kenedia.Modules.Characters.Services.Settings.PanelSizes.Custom:
				fontSize = (ContentService.FontSize)(nameFont ? _settings.CustomCharacterNameFontSize.Value : _settings.CustomCharacterFontSize.Value);
				break;
			}
			return GameService.Content.GetFont(ContentService.FontFace.Menomonia, fontSize, ContentService.FontStyle.Regular);
		}

		internal void Update()
		{
			if (Character != null && _lastLoginLabel.Visible)
			{
				if (CurrentCharacter?.Invoke() != Character)
				{
					TimeSpan ts2 = DateTimeOffset.UtcNow.Subtract(Character.LastLogin);
					_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, Math.Floor(ts2.TotalDays), ts2.Hours, ts2.Minutes, ts2.Seconds);
				}
				else
				{
					_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, 0, 0, 0, 0);
				}
			}
			if (Character != null && _nextBirthdayLabel.Visible)
			{
				TimeSpan ts = Character.UntilNextBirthday;
				_nextBirthdayLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, Math.Floor(ts.TotalDays), ts.Hours, ts.Minutes, ts.Seconds);
			}
			if (Character != null && _ageLabel.Visible)
			{
				_ageLabel.Text = string.Format("{1} ({0} Years)", Character.Age, Character.Created.Date.ToString("d"));
			}
			if (_created && (Parent?.Visible ?? false))
			{
				UpdateCharacterInfo();
			}
		}

		private void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			UpdateCharacterInfo();
		}
	}
}
