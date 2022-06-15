using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
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
					deleteButton.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[35]));
				}
				else
				{
					panel.Texture = Textures.Backgrounds[5];
					deleteButton.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[44]));
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
					((Control)deleteButton).set_Visible(value);
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
				textLabel.set_Text(" " + value + " ");
			}
		}

		public TagEntry(string txt, Character character, FlowPanel parent, bool showButton = true, BitmapFont font = null)
			: this()
		{
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Expected O, but got Unknown
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Expected O, but got Unknown
			BitmapFont textFont = ((font == null) ? GameService.Content.get_DefaultFont14() : font);
			assignedCharacter = character;
			((Control)this).set_Parent((Container)(object)parent);
			((Container)this).set_WidthSizingMode((SizingMode)1);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			TagPanel tagPanel = new TagPanel();
			((Control)tagPanel).set_Parent((Container)(object)this);
			((Container)tagPanel).set_WidthSizingMode((SizingMode)1);
			((Container)tagPanel).set_HeightSizingMode((SizingMode)1);
			((FlowPanel)tagPanel).set_FlowDirection((ControlFlowDirection)2);
			((FlowPanel)tagPanel).set_OuterControlPadding(new Vector2(5f, 3f));
			((FlowPanel)tagPanel).set_ControlPadding(new Vector2(3f, 0f));
			((Container)tagPanel).set_AutoSizePadding(new Point(5, 2));
			panel = tagPanel;
			((Control)panel).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				if (!Highlighted)
				{
					Highlighted = true;
					assignedCharacter.Tags.Add(Text);
				}
			});
			Image val = new Image();
			val.set_Texture(AsyncTexture2D.op_Implicit(Textures.Icons[35]));
			((Control)val).set_Parent((Container)(object)panel);
			((Control)val).set_Size(new Point(21, 23));
			((Control)val).set_Visible(showDeleteButton);
			deleteButton = val;
			((Control)deleteButton).add_MouseEntered((EventHandler<MouseEventArgs>)delegate
			{
				deleteButton.set_Texture(AsyncTexture2D.op_Implicit(_Highlighted ? Textures.Icons[36] : Textures.Icons[44]));
			});
			((Control)deleteButton).add_MouseLeft((EventHandler<MouseEventArgs>)delegate
			{
				deleteButton.set_Texture(AsyncTexture2D.op_Implicit(_Highlighted ? Textures.Icons[35] : Textures.Icons[44]));
			});
			((Control)deleteButton).add_Click((EventHandler<MouseEventArgs>)delegate
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
					foreach (TagEntry tagEntry in (Container)Module.filterTagsPanel)
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
							((Control)this).Dispose();
						}
						((Control)item).Dispose();
					}
				}
				if (Discardable)
				{
					((Control)this).Dispose();
				}
			});
			Label val2 = new Label();
			val2.set_Text(txt);
			((Control)val2).set_Parent((Container)(object)panel);
			val2.set_AutoSizeWidth(true);
			((Control)val2).set_Height(showDeleteButton ? ((Control)deleteButton).get_Size().Y : (textFont.LineHeight + 4));
			val2.set_Font(textFont);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			textLabel = val2;
			((Control)panel).Invalidate();
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
				foreach (TagEntry tag in (Container)Module.filterTagsPanel)
				{
					if (tempList.Contains(tag.Text))
					{
						Module.Tags.Remove(tag.Text);
						deleteList.Add(tag);
					}
				}
				foreach (TagEntry item in deleteList)
				{
					((Control)item).Dispose();
				}
			}
			((Control)this).Dispose();
		}
	}
}
