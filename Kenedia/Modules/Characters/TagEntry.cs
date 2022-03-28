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

		private bool showDeleteButton;

		private string _Text;

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
			showDeleteButton = showButton;
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
			if (showDeleteButton)
			{
				deleteButton = new Image
				{
					Texture = Textures.Icons[35],
					Parent = panel,
					Size = new Point(21, 23)
				};
				deleteButton.MouseEntered += delegate
				{
					deleteButton.Texture = Textures.Icons[36];
				};
				deleteButton.MouseLeft += delegate
				{
					deleteButton.Texture = Textures.Icons[35];
				};
				deleteButton.Click += delegate
				{
					assignedCharacter.Tags.Remove(Text);
					Dispose();
				};
			}
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
		}
	}
}
