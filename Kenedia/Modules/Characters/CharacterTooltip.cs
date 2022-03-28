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
		public Character assignedCharacter;

		public Image _nameTexture;

		public Label _nameLabel;

		public Image _levelTexture;

		public Label _levelLabel;

		public Image _raceTexture;

		public Label _raceLabel;

		public Image _mapTexture;

		public Label _mapLabel;

		public Image _createdTexture;

		public Label _createdLabel;

		public Image _ageTexture;

		public Label _ageLabel;

		public FlowPanel Tags;

		public List<string> _Tags;

		public WindowBase _Parent;

		public void _Create()
		{
			if (assignedCharacter != null && assignedCharacter.characterControl != null)
			{
				ContentService contentService = new ContentService();
				Character c = assignedCharacter;
				_ = assignedCharacter.characterControl;
				int index = 0;
				_nameTexture = new Image
				{
					Texture = Textures.Icons[26],
					Parent = this,
					Location = new Point(0, index * 25),
					Size = new Point(20, 20),
					Visible = false
				};
				_nameLabel = new Label
				{
					Text = c.Name,
					Parent = this,
					Location = new Point(0, index * 25),
					Visible = true,
					Width = 200,
					Font = contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size20, ContentService.FontStyle.Regular),
					HorizontalAlignment = HorizontalAlignment.Center
				};
				new Image
				{
					Texture = Textures.Icons[19],
					Parent = this,
					Location = new Point(0, 25 + index * 25),
					Size = new Point(base.Width, 4)
				};
				_raceTexture = new Image
				{
					Texture = Textures.Races[(uint)c.Race],
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Size = new Point(20, 20),
					Visible = true
				};
				_raceLabel = new Label
				{
					Text = DataManager.getRaceName(c.Race.ToString()),
					Parent = this,
					Location = new Point(30, 40 + index * 25),
					Visible = true,
					AutoSizeWidth = true
				};
				index++;
				_levelTexture = new Image
				{
					Texture = Textures.Icons[32],
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Size = new Point(20, 20),
					Visible = true
				};
				_levelLabel = new Label
				{
					Text = string.Format(common.Level, c.Level),
					Parent = this,
					Location = new Point(30, 40 + index * 25),
					Visible = true,
					AutoSizeWidth = true
				};
				index++;
				_mapTexture = new Image
				{
					Texture = Textures.Icons[29],
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Size = new Point(20, 20),
					Visible = true
				};
				_mapLabel = new Label
				{
					Text = DataManager.getMapName(c.map),
					Parent = this,
					Location = new Point(30, 40 + index * 25),
					Visible = true,
					AutoSizeWidth = true
				};
				DateTime zeroTime = new DateTime(1, 1, 1);
				TimeSpan span = DateTime.UtcNow - c.Created.UtcDateTime;
				index++;
				_createdTexture = new Image
				{
					Texture = Textures.Icons[17],
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Size = new Point(20, 20),
					Visible = true
				};
				_createdLabel = new Label
				{
					Text = c.Created.ToString("G") + " (" + ((zeroTime + span).Year - 1) + " " + common.Years + ")",
					Parent = this,
					Location = new Point(30, 40 + index * 25),
					Visible = true,
					AutoSizeWidth = true
				};
				TimeSpan t = TimeSpan.FromSeconds(c.seconds);
				index++;
				_ageTexture = new Image
				{
					Texture = Textures.Icons[31],
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Size = new Point(20, 20),
					Visible = true
				};
				_ageLabel = new Label
				{
					Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days),
					Parent = this,
					Location = new Point(30, 40 + index * 25),
					Visible = true,
					AutoSizeWidth = true
				};
				index++;
				Tags = new FlowPanel
				{
					Parent = this,
					Location = new Point(0, 40 + index * 25),
					Width = base.Width,
					OuterControlPadding = new Vector2(2f, 2f),
					ControlPadding = new Vector2(5f, 2f),
					HeightSizingMode = SizingMode.AutoSize
				};
				Invalidate();
				_Update();
			}
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
			_ageLabel.Text = string.Format("{3} " + common.Days + " {0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds, t.Days);
			DateTime zeroTime = new DateTime(1, 1, 1);
			TimeSpan span = DateTime.UtcNow - c.Created.UtcDateTime;
			_createdLabel.Text = c.Created.ToString("G") + " (" + ((zeroTime + span).Year - 1) + " " + common.Years + ")";
			_levelLabel.Text = string.Format(common.Level, c.Level);
			_mapLabel.Text = DataManager.getMapName(c.map);
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
