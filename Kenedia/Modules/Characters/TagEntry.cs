using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters
{
	public class TagEntry : Panel
	{
		public Character assignedCharacter;

		public Label textLabel;

		public Image deleteButton;

		private TagPanel panel;

		private bool _showDeleteButton;

		private bool _Highlighted = true;

		private bool Discardable;

		private string _Text;

		public bool Highlighted
		{
			get
			{
				return _Highlighted;
			}
			set
			{
				_Highlighted = value;
				if (value)
				{
					panel.Texture = Textures.Backgrounds[4];
					deleteButton.Texture = Textures.Icons[35];
				}
				else
				{
					panel.Texture = Textures.Backgrounds[5];
					deleteButton.Texture = Textures.Icons[44];
				}
			}
		}

		public bool showDeleteButton
		{
			get
			{
				return _showDeleteButton;
			}
			set
			{
				_showDeleteButton = value;
				if (deleteButton != null)
				{
					deleteButton.Visible = value;
				}
			}
		}

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
				textLabel.Text = " " + value + " ";
			}
		}

		public TagEntry(string txt, Character character, FlowPanel parent, bool showButton = true, BitmapFont font = null)
		{
			ContentService contentService = new ContentService();
			BitmapFont textFont = ((font == null) ? contentService.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Regular) : font);
			assignedCharacter = character;
			base.Parent = parent;
			WidthSizingMode = SizingMode.AutoSize;
			HeightSizingMode = SizingMode.AutoSize;
			panel = new TagPanel
			{
				Parent = this,
				WidthSizingMode = SizingMode.AutoSize,
				HeightSizingMode = SizingMode.AutoSize,
				FlowDirection = ControlFlowDirection.SingleLeftToRight,
				OuterControlPadding = new Vector2(5f, 3f),
				ControlPadding = new Vector2(3f, 0f),
				AutoSizePadding = new Point(5, 2)
			};
			panel.Click += delegate
			{
				if (!Highlighted)
				{
					Highlighted = true;
					assignedCharacter.Tags.Add(Text);
				}
			};
			deleteButton = new Image
			{
				Texture = Textures.Icons[35],
				Parent = panel,
				Size = new Point(21, 23),
				Visible = showDeleteButton
			};
			deleteButton.MouseEntered += delegate
			{
				deleteButton.Texture = (_Highlighted ? Textures.Icons[36] : Textures.Icons[44]);
			};
			deleteButton.MouseLeft += delegate
			{
				deleteButton.Texture = (_Highlighted ? Textures.Icons[35] : Textures.Icons[44]);
			};
			deleteButton.Click += delegate
			{
				Highlighted = false;
				assignedCharacter.Tags.Remove(Text);
				assignedCharacter.Save();
				foreach (string current in assignedCharacter.Tags)
				{
					Module.Logger.Debug(current);
				}
				if (Module.filterTagsPanel != null)
				{
					List<string> list = new List<string>(Module.Tags);
					foreach (Character character2 in Module.Characters)
					{
						foreach (string current2 in character2.Tags)
						{
							if (list.Contains(current2))
							{
								list.Remove(current2);
							}
						}
					}
					List<TagEntry> list2 = new List<TagEntry>();
					foreach (TagEntry tagEntry in Module.filterTagsPanel)
					{
						if (list.Contains(tagEntry.Text))
						{
							Module.Tags.Remove(tagEntry.Text);
							list2.Add(tagEntry);
						}
					}
					foreach (TagEntry item in list2)
					{
						if (item.Text == Text)
						{
							Dispose();
						}
						item.Dispose();
					}
				}
				if (Discardable)
				{
					Dispose();
				}
			};
			textLabel = new Label
			{
				Text = txt,
				Parent = panel,
				AutoSizeWidth = true,
				Height = (showDeleteButton ? deleteButton.Size.Y : (textFont.LineHeight + 4)),
				Font = textFont,
				VerticalAlignment = VerticalAlignment.Middle
			};
			panel.Invalidate();
			_Text = txt;
			showDeleteButton = showButton;
		}

		private void DeleteOG()
		{
			if (!_Highlighted)
			{
				return;
			}
			assignedCharacter.Tags.Remove(Text);
			if (Module.filterTagsPanel != null)
			{
				List<string> tempList = new List<string>(Module.Tags);
				foreach (Character character in Module.Characters)
				{
					foreach (string t in character.Tags)
					{
						if (tempList.Contains(t))
						{
							tempList.Remove(t);
						}
					}
				}
				List<TagEntry> deleteList = new List<TagEntry>();
				foreach (TagEntry tag in Module.filterTagsPanel)
				{
					if (tempList.Contains(tag.Text))
					{
						Module.Tags.Remove(tag.Text);
						deleteList.Add(tag);
					}
				}
				foreach (TagEntry item in deleteList)
				{
					item.Dispose();
				}
			}
			Dispose();
		}
	}
}
