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

		public FlowPanel ContentRegion;

		public FlowPanel Tags;

		public List<string> _Tags;

		public WindowBase _Parent;

		public Label _switchInfoLabel;

		public CharacterTooltip(Character character)
			: this()
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Expected O, but got Unknown
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Expected O, but got Unknown
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c0: Expected O, but got Unknown
			assignedCharacter = character;
			Character c = assignedCharacter;
			((Control)this).add_Shown((EventHandler<EventArgs>)delegate
			{
				_Update();
			});
			((Control)this).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
			});
			FlowPanel val = new FlowPanel();
			((Control)val).set_Location(new Point(0, 0));
			((Container)val).set_HeightSizingMode((SizingMode)1);
			((Container)val).set_WidthSizingMode((SizingMode)1);
			val.set_FlowDirection((ControlFlowDirection)3);
			((Control)val).set_Parent((Container)(object)this);
			ContentRegion = val;
			IconLabel obj = new IconLabel
			{
				Texture = Textures.Icons[22]
			};
			((Control)obj).set_Parent((Container)(object)ContentRegion);
			obj.Text = c.Name;
			obj.Font = new ContentService().GetFont((FontFace)0, (FontSize)20, (FontStyle)0);
			obj.Gap = 4;
			_Name = obj;
			Separator separator = new Separator();
			((Control)separator).set_Parent((Container)(object)ContentRegion);
			_Separator = separator;
			IconLabel obj2 = new IconLabel
			{
				Texture = Textures.Races[(uint)c.Race],
				Text = DataManager.getRaceName(c.Race.ToString())
			};
			((Control)obj2).set_Parent((Container)(object)ContentRegion);
			_Race = obj2;
			IconLabel obj3 = new IconLabel
			{
				Texture = Textures.Icons[32],
				Text = string.Format(common.Level, c.Level)
			};
			((Control)obj3).set_Parent((Container)(object)ContentRegion);
			_Level = obj3;
			IconLabel obj4 = new IconLabel
			{
				Texture = Textures.Icons[29],
				Text = DataManager.getMapName(c.Map)
			};
			((Control)obj4).set_Parent((Container)(object)ContentRegion);
			_Map = obj4;
			DateTime zeroTime = new DateTime(1, 1, 1);
			TimeSpan span = DateTime.UtcNow - c.Created.UtcDateTime;
			IconLabel iconLabel = new IconLabel
			{
				Texture = Textures.Icons[26],
				Text = c.Created.ToString("G") + " (" + ((zeroTime + span).Year - 1) + " " + common.Years + ")"
			};
			((Control)iconLabel).set_Parent((Container)(object)ContentRegion);
			_Created = iconLabel;
			span = c.NextBirthday - DateTime.UtcNow;
			iconLabel = new IconLabel
			{
				Texture = Textures.Icons[17],
				Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00} " + common.UntilBirthday, span.Hours, span.Minutes, span.Seconds, span.Days)
			};
			((Control)iconLabel).set_Parent((Container)(object)ContentRegion);
			_NextBirthday = iconLabel;
			foreach (CharacterCrafting crafting in c.Crafting)
			{
				iconLabel = new IconLabel();
				((Control)iconLabel).set_Parent((Container)(object)ContentRegion);
				iconLabel.Text = DataManager.getCraftingName(crafting.Id) + " (" + crafting.Rating + "/" + ((crafting.Id == 4 || crafting.Id == 7) ? 400 : 500) + ")";
				iconLabel.Texture = (crafting.Active ? Textures.Crafting[crafting.Id] : Textures.CraftingDisabled[crafting.Id]);
				iconLabel._Crafting = crafting;
				IconLabel ctrl = iconLabel;
				ctrl.Label.set_TextColor((!crafting.Active) ? Color.LightGray : ctrl.Label.get_TextColor());
				_CraftingProfessions.Add(ctrl);
			}
			TimeSpan t = TimeSpan.FromSeconds(c.seconds);
			iconLabel = new IconLabel
			{
				Texture = Textures.Icons[31],
				Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days)
			};
			((Control)iconLabel).set_Parent((Container)(object)ContentRegion);
			_LastLogin = iconLabel;
			Panel val2 = new Panel();
			((Container)val2).set_WidthSizingMode((SizingMode)2);
			((Control)val2).set_Parent((Container)(object)ContentRegion);
			Panel p = val2;
			Label val3 = new Label();
			val3.set_Text("- " + string.Format(common.DoubleClickToSwap, assignedCharacter.Name) + " -");
			((Control)val3).set_Parent((Container)(object)p);
			val3.set_Font(ContentService.GetFont((FontFace)0, (FontSize)12, (FontStyle)0));
			val3.set_HorizontalAlignment((HorizontalAlignment)1);
			val3.set_TextColor(Color.LightGray);
			_switchInfoLabel = val3;
			((Control)p).add_Resized((EventHandler<ResizedEventArgs>)delegate
			{
				((Control)_switchInfoLabel).set_Width(((Control)p).get_Width());
				((Control)p).set_Height(((Control)_switchInfoLabel).get_Height() + 5);
			});
			FlowPanel val4 = new FlowPanel();
			((Control)val4).set_Parent((Container)(object)ContentRegion);
			val4.set_OuterControlPadding(new Vector2(2f, 2f));
			val4.set_ControlPadding(new Vector2(5f, 2f));
			((Container)val4).set_WidthSizingMode((SizingMode)2);
			((Container)val4).set_HeightSizingMode((SizingMode)1);
			Tags = val4;
			((Control)this).Invalidate();
			_Update();
		}

		public void _Update()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
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
			((Container)Tags).ClearChildren();
			foreach (string tag in c.Tags)
			{
				new TagEntry(tag, c, Tags, showButton: false, contentService.GetFont((FontFace)0, (FontSize)12, (FontStyle)0));
			}
		}
	}
}
