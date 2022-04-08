using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Characters.Strings;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters
{
	public class CharacterTooltip : Tooltip
	{
		public static ContentService ContentService = new ContentService();

		public Character assignedCharacter;

		public IconLabel _Name;

		public IconLabel _Race;

		public IconLabel _Level;

		public IconLabel _Map;

		public IconLabel _Created;

		public IconLabel _NextBirthday;

		public IconLabel _LastLogin;

		public List<IconLabel> _CraftingProfessions = new List<IconLabel>();

		public Separator _Separator;

		public new FlowPanel ContentRegion;

		public FlowPanel Tags;

		public List<string> _Tags;

		public WindowBase _Parent;

		public Label _switchInfoLabel;

		public CharacterTooltip(Character character)
		{
			assignedCharacter = character;
			Character c = assignedCharacter;
			base.Shown += delegate
			{
				_Update();
			};
			base.Resized += delegate
			{
			};
			ContentRegion = new FlowPanel
			{
				Location = new Point(0, 0),
				HeightSizingMode = SizingMode.AutoSize,
				WidthSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				Parent = this
			};
			_Name = new IconLabel
			{
				Texture = Textures.Icons[22],
				Parent = ContentRegion,
				Text = c.Name,
				Font = new ContentService().GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size20, ContentService.FontStyle.Regular),
				Gap = 4
			};
			_Separator = new Separator
			{
				Parent = ContentRegion
			};
			_Race = new IconLabel
			{
				Texture = Textures.Races[(uint)c.Race],
				Text = DataManager.getRaceName(c.Race.ToString()),
				Parent = ContentRegion
			};
			_Level = new IconLabel
			{
				Texture = Textures.Icons[32],
				Text = string.Format(common.Level, c.Level),
				Parent = ContentRegion
			};
			_Map = new IconLabel
			{
				Texture = Textures.Icons[29],
				Text = DataManager.getMapName(c.Map),
				Parent = ContentRegion
			};
			DateTime zeroTime = new DateTime(1, 1, 1);
			TimeSpan span = DateTime.UtcNow - c.Created.UtcDateTime;
			_Created = new IconLabel
			{
				Texture = Textures.Icons[26],
				Text = c.Created.ToString("G") + " (" + ((zeroTime + span).Year - 1) + " " + common.Years + ")",
				Parent = ContentRegion
			};
			span = c.NextBirthday - DateTime.UtcNow;
			_NextBirthday = new IconLabel
			{
				Texture = Textures.Icons[17],
				Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00} " + common.UntilBirthday, span.Hours, span.Minutes, span.Seconds, span.Days),
				Parent = ContentRegion
			};
			foreach (CharacterCrafting crafting in c.Crafting)
			{
				IconLabel ctrl = new IconLabel
				{
					Parent = ContentRegion,
					Text = DataManager.getCraftingName(crafting.Id) + " (" + crafting.Rating + "/" + ((crafting.Id == 4 || crafting.Id == 7) ? 400 : 500) + ")",
					Texture = (crafting.Active ? Textures.Crafting[crafting.Id] : Textures.CraftingDisabled[crafting.Id]),
					_Crafting = crafting
				};
				ctrl.Label.TextColor = ((!crafting.Active) ? Color.LightGray : ctrl.Label.TextColor);
				_CraftingProfessions.Add(ctrl);
			}
			TimeSpan t = TimeSpan.FromSeconds(c.seconds);
			_LastLogin = new IconLabel
			{
				Texture = Textures.Icons[31],
				Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days),
				Parent = ContentRegion
			};
			Panel p = new Panel
			{
				WidthSizingMode = SizingMode.Fill,
				Parent = ContentRegion
			};
			_switchInfoLabel = new Label
			{
				Text = "- " + string.Format(common.DoubleClickToSwap, assignedCharacter.Name) + " -",
				Parent = p,
				Font = ContentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size12, ContentService.FontStyle.Regular),
				HorizontalAlignment = HorizontalAlignment.Center,
				TextColor = Color.LightGray
			};
			p.Resized += delegate
			{
				_switchInfoLabel.Width = p.Width;
				p.Height = _switchInfoLabel.Height + 5;
			};
			Tags = new FlowPanel
			{
				Parent = ContentRegion,
				OuterControlPadding = new Vector2(2f, 2f),
				ControlPadding = new Vector2(5f, 2f),
				WidthSizingMode = SizingMode.Fill,
				HeightSizingMode = SizingMode.AutoSize
			};
			Invalidate();
			_Update();
		}

		public void _Update()
		{
			if (assignedCharacter == null || assignedCharacter.characterControl == null)
			{
				return;
			}
			ContentService contentService = new ContentService();
			Character c = assignedCharacter;
			TimeSpan t = TimeSpan.FromSeconds(c.seconds);
			_LastLogin.Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days);
			DateTime zeroTime = new DateTime(1, 1, 1);
			TimeSpan span = DateTime.UtcNow - c.Created.UtcDateTime;
			_Created.Text = c.Created.ToString("G") + " (" + ((zeroTime + span).Year - 1) + " " + common.Years + ")";
			span = c.NextBirthday - DateTime.UtcNow;
			_NextBirthday.Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00} " + common.UntilBirthday, span.Hours, span.Minutes, span.Seconds, span.Days);
			_Level.Text = string.Format(common.Level, c.Level);
			_Map.Text = DataManager.getMapName(c.Map);
			if (assignedCharacter.Crafting.Count > 0)
			{
				foreach (IconLabel iconLabel in _CraftingProfessions)
				{
					iconLabel.Text = DataManager.getCraftingName(iconLabel._Crafting.Id) + " (" + iconLabel._Crafting.Rating + "/" + ((iconLabel._Crafting.Id == 4 || iconLabel._Crafting.Id == 7) ? 400 : 500) + ")";
				}
			}
			if (c.Tags == null || (_Tags != null && _Tags.SequenceEqual(c.Tags)))
			{
				return;
			}
			_Tags = new List<string>(c.Tags);
			Tags.ClearChildren();
			foreach (string tag in c.Tags)
			{
				new TagEntry(tag, c, Tags, showButton: false, contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size12, ContentService.FontStyle.Regular));
			}
		}
	}
}
