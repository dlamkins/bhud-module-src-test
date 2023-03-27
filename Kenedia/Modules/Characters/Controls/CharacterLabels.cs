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

		public BitmapFont NameFont { get; set; } = GameService.Content.get_DefaultFont14();


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


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
				Common.SetProperty(ref _data, value, OnDataChanged, triggerOnUpdate: true, "Data");
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
				if (Common.SetProperty(ref _settings, value, OnSettingsChanged, triggerOnUpdate: true, "Settings"))
				{
					if (temp != null)
					{
						temp.AppearanceSettingChanged -= Settings_AppearanceSettingChanged;
					}
					if (_settings != null)
					{
						_settings.AppearanceSettingChanged += Settings_AppearanceSettingChanged;
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
				Common.SetProperty(ref _textureManager, value, OnTextureManagerAdded, triggerOnUpdate: true, "TextureManager");
			}
		}

		public Func<Character_Model> CurrentCharacter { get; set; }

		public FlowPanel Parent { get; }

		public CharacterLabels(FlowPanel parent)
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
			IconLabel iconLabel = new IconLabel();
			((Control)iconLabel).set_Parent((Container)(object)parent);
			iconLabel.AutoSizeWidth = true;
			iconLabel.AutoSizeHeight = true;
			iconLabel.TextColor = Colors.ColonialWhite;
			_nameLabel = iconLabel;
			IconLabel iconLabel2 = new IconLabel();
			((Control)iconLabel2).set_Parent((Container)(object)parent);
			iconLabel2.AutoSizeWidth = true;
			iconLabel2.AutoSizeHeight = true;
			iconLabel2.Icon = AsyncTexture2D.FromAssetId(157085);
			iconLabel2.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_levelLabel = iconLabel2;
			IconLabel iconLabel3 = new IconLabel();
			((Control)iconLabel3).set_Parent((Container)(object)parent);
			iconLabel3.AutoSizeWidth = true;
			iconLabel3.AutoSizeHeight = true;
			_genderLabel = iconLabel3;
			IconLabel iconLabel4 = new IconLabel();
			((Control)iconLabel4).set_Parent((Container)(object)parent);
			iconLabel4.AutoSizeWidth = true;
			iconLabel4.AutoSizeHeight = true;
			_raceLabel = iconLabel4;
			IconLabel iconLabel5 = new IconLabel();
			((Control)iconLabel5).set_Parent((Container)(object)parent);
			iconLabel5.AutoSizeWidth = true;
			iconLabel5.AutoSizeHeight = true;
			_professionLabel = iconLabel5;
			IconLabel iconLabel6 = new IconLabel();
			((Control)iconLabel6).set_Parent((Container)(object)parent);
			iconLabel6.AutoSizeWidth = true;
			iconLabel6.AutoSizeHeight = true;
			iconLabel6.Icon = AsyncTexture2D.FromAssetId(358406);
			iconLabel6.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_mapLabel = iconLabel6;
			CraftingControl craftingControl = new CraftingControl();
			((Control)craftingControl).set_Parent((Container)(object)parent);
			((Control)craftingControl).set_Width(((Control)parent).get_Width());
			((Control)craftingControl).set_Height(20);
			craftingControl.Character = Character;
			_craftingControl = craftingControl;
			IconLabel iconLabel7 = new IconLabel();
			((Control)iconLabel7).set_Parent((Container)(object)parent);
			iconLabel7.AutoSizeWidth = true;
			iconLabel7.AutoSizeHeight = true;
			iconLabel7.Icon = AsyncTexture2D.FromAssetId(1424243);
			iconLabel7.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_ageLabel = iconLabel7;
			IconLabel iconLabel8 = new IconLabel();
			((Control)iconLabel8).set_Parent((Container)(object)parent);
			iconLabel8.AutoSizeWidth = true;
			iconLabel8.AutoSizeHeight = true;
			iconLabel8.Icon = AsyncTexture2D.FromAssetId(593864);
			iconLabel8.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_nextBirthdayLabel = iconLabel8;
			IconLabel iconLabel9 = new IconLabel();
			((Control)iconLabel9).set_Parent((Container)(object)parent);
			iconLabel9.AutoSizeWidth = true;
			iconLabel9.AutoSizeHeight = true;
			iconLabel9.Icon = AsyncTexture2D.FromAssetId(155035);
			iconLabel9.TextureRectangle = new Rectangle(10, 10, 44, 44);
			_lastLoginLabel = iconLabel9;
			IconLabel iconLabel10 = new IconLabel();
			((Control)iconLabel10).set_Parent((Container)(object)parent);
			iconLabel10.AutoSizeWidth = true;
			iconLabel10.AutoSizeHeight = true;
			iconLabel10.TextureRectangle = new Rectangle(2, 2, 28, 28);
			iconLabel10.Icon = AsyncTexture2D.FromAssetId(156909);
			_customIndex = iconLabel10;
			TagFlowPanel tagFlowPanel = new TagFlowPanel();
			((Control)tagFlowPanel).set_Parent((Container)(object)parent);
			tagFlowPanel.Font = _lastLoginLabel.Font;
			((FlowPanel)tagFlowPanel).set_FlowDirection((ControlFlowDirection)0);
			((FlowPanel)tagFlowPanel).set_ControlPadding(new Vector2(3f, 2f));
			((Container)tagFlowPanel).set_HeightSizingMode((SizingMode)1);
			((Control)tagFlowPanel).set_Visible(false);
			TagPanel = tagFlowPanel;
			DataControls = new List<Control>
			{
				(Control)(object)_nameLabel,
				(Control)(object)_customIndex,
				(Control)(object)_levelLabel,
				(Control)(object)_genderLabel,
				(Control)(object)_raceLabel,
				(Control)(object)_professionLabel,
				(Control)(object)_mapLabel,
				(Control)(object)_nextBirthdayLabel,
				(Control)(object)_ageLabel,
				(Control)(object)_lastLoginLabel,
				(Control)(object)_craftingControl,
				(Control)(object)TagPanel
			};
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
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
				_genderLabel.Icon = AsyncTexture2D.op_Implicit(_textureManager?.GetIcon(TextureManager.Icons.Gender));
			}
		}

		public void RecalculateBounds()
		{
			UpdateDataControlsVisibility();
			TagPanel.FitWidestTag(DataControls.Max((Control e) => (e.get_Visible() && e != TagPanel) ? e.get_Width() : 0));
		}

		public void Dispose()
		{
			((IEnumerable<IDisposable>)DataControls)?.DisposeAll();
			LocalizingService.LocaleChanged -= UserLocale_SettingChanged;
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
				_professionLabel.TextureRectangle = ((_professionLabel.Icon.get_Width() == 32) ? new Rectangle(2, 2, 28, 28) : new Rectangle(4, 4, 56, 56));
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
					((Control)t).Dispose();
					_tags.Remove(t);
				}
				foreach (string tag in addTags)
				{
					List<Tag> tags = _tags;
					Tag tag3 = new Tag();
					((Control)tag3).set_Parent((Container)(object)TagPanel);
					tag3.Text = tag;
					tag3.Active = true;
					tag3.ShowDelete = false;
					tag3.CanInteract = false;
					tags.Add(tag3);
				}
				TagPanel.FitWidestTag(DataControls.Max((Control e) => (e.get_Visible() && e != TagPanel) ? e.get_Width() : 0));
			}
			_craftingControl.Character = Character;
		}

		public void UpdateDataControlsVisibility(bool tooltip = false)
		{
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			if (_settings == null)
			{
				return;
			}
			Dictionary<string, ShowCheckPair> settings = _settings.DisplayToggles.get_Value();
			NameFont = GetFont(nameFont: true);
			Font = GetFont();
			((Control)_nameLabel).set_Visible(!settings.TryGetValue("Name", out var name) || (tooltip ? name.ShowTooltip : name.Show));
			_nameLabel.Font = NameFont;
			((Control)_levelLabel).set_Visible(!settings.TryGetValue("Level", out var level) || (tooltip ? level.ShowTooltip : level.Show));
			_levelLabel.Font = Font;
			((Control)_genderLabel).set_Visible(!settings.TryGetValue("Gender", out var gender) || (tooltip ? gender.ShowTooltip : gender.Show));
			_genderLabel.Font = Font;
			((Control)_raceLabel).set_Visible(!settings.TryGetValue("Race", out var race) || (tooltip ? race.ShowTooltip : race.Show));
			_raceLabel.Font = Font;
			((Control)_professionLabel).set_Visible(!settings.TryGetValue("Profession", out var profession) || (tooltip ? profession.ShowTooltip : profession.Show));
			_professionLabel.Font = Font;
			((Control)_lastLoginLabel).set_Visible(!settings.TryGetValue("LastLogin", out var lastlogin) || (tooltip ? lastlogin.ShowTooltip : lastlogin.Show));
			_lastLoginLabel.Font = Font;
			((Control)_ageLabel).set_Visible(!settings.TryGetValue("Age", out var age) || (tooltip ? age.ShowTooltip : age.Show));
			_ageLabel.Font = Font;
			((Control)_nextBirthdayLabel).set_Visible(!settings.TryGetValue("NextBirthday", out var nextbirthday) || (tooltip ? nextbirthday.ShowTooltip : nextbirthday.Show));
			_nextBirthdayLabel.Font = Font;
			((Control)_mapLabel).set_Visible(!settings.TryGetValue("Map", out var map) || (tooltip ? map.ShowTooltip : map.Show));
			_mapLabel.Font = Font;
			((Control)_craftingControl).set_Visible(!settings.TryGetValue("CraftingProfession", out var craftingprofession) || (tooltip ? craftingprofession.ShowTooltip : craftingprofession.Show));
			_craftingControl.Font = Font;
			((Control)_customIndex).set_Visible(!settings.TryGetValue("CustomIndex", out var customindex) || (tooltip ? customindex.ShowTooltip : customindex.Show));
			_customIndex.Font = Font;
			((Control)TagPanel).set_Visible((!settings.TryGetValue("Tags", out var tags) || (tooltip ? tags.ShowTooltip : tags.Show)) && (Character?.Tags.Count ?? 0) > 0);
			TagPanel.Font = Font;
			((Control)_craftingControl).set_Height(Font.get_LineHeight() + 2);
			if (Parent != null)
			{
				((FlowPanel)Parent).set_ControlPadding(new Vector2((float)(Font.get_LineHeight() / 10), (float)(Font.get_LineHeight() / 10)));
				FlowPanel parent = Parent;
				if (parent != null)
				{
					((Control)parent).Invalidate();
				}
			}
		}

		private BitmapFont GetFont(bool nameFont = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			FontSize fontSize = (FontSize)8;
			if (_settings == null)
			{
				return GameService.Content.get_DefaultFont12();
			}
			switch (_settings.PanelSize.get_Value())
			{
			case Settings.PanelSizes.Small:
				fontSize = (FontSize)(nameFont ? 16 : 12);
				break;
			case Settings.PanelSizes.Normal:
				fontSize = (FontSize)(nameFont ? 18 : 14);
				break;
			case Settings.PanelSizes.Large:
				fontSize = (FontSize)(nameFont ? 22 : 18);
				break;
			case Settings.PanelSizes.Custom:
				fontSize = (FontSize)(nameFont ? _settings.CustomCharacterNameFontSize.get_Value() : _settings.CustomCharacterFontSize.get_Value());
				break;
			}
			return GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
		}

		internal void Update()
		{
			if (Character != null && ((Control)_lastLoginLabel).get_Visible())
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
			if (Character != null && ((Control)_nextBirthdayLabel).get_Visible())
			{
				TimeSpan ts = Character.UntilNextBirthday;
				_nextBirthdayLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, Math.Floor(ts.TotalDays), ts.Hours, ts.Minutes, ts.Seconds);
			}
			if (Character != null && ((Control)_ageLabel).get_Visible())
			{
				_ageLabel.Text = string.Format("{1} ({0} Years)", Character.Age, Character.Created.Date.ToString("d"));
			}
			if (_created)
			{
				FlowPanel parent = Parent;
				if (parent != null && ((Control)parent).get_Visible())
				{
					UpdateCharacterInfo();
				}
			}
		}

		private void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			UpdateCharacterInfo();
		}
	}
}
