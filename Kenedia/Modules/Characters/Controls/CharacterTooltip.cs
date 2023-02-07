using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterTooltip : FramedContainer
	{
		private readonly AsyncTexture2D _iconFrame = AsyncTexture2D.FromAssetId(1414041);

		private readonly FlowPanel _contentPanel;

		private readonly Dummy _iconDummy;

		private readonly IconLabel _nameLabel;

		private readonly IconLabel _levelLabel;

		private readonly IconLabel _professionLabel;

		private readonly IconLabel _genderLabel;

		private readonly IconLabel _raceLabel;

		private readonly IconLabel _mapLabel;

		private readonly IconLabel _lastLoginLabel;

		private readonly TagFlowPanel _tagPanel;

		private readonly CraftingControl _craftingControl;

		private readonly List<Control> _dataControls;

		private readonly Func<Character_Model> _currentCharacter;

		private readonly TextureManager _textureManager;

		private readonly Data _data;

		private Rectangle _iconRectangle;

		private Rectangle _contentRectangle;

		private Point _textureOffset = new Point(25, 25);

		private Character_Model _character;

		private readonly List<Tag> _tags = new List<Tag>();

		private bool _updateCharacter;

		public Color BackgroundTint { get; set; } = Color.get_Honeydew() * 0.95f;


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


		public BitmapFont NameFont { get; set; } = GameService.Content.get_DefaultFont18();


		public Character_Model Character
		{
			get
			{
				return _character;
			}
			set
			{
				if (_character != value)
				{
					if (_character != null)
					{
						_character.Updated -= ApplyCharacter;
					}
					_character = value;
					if (value != null)
					{
						_character.Updated += ApplyCharacter;
						UpdateCharacterInfo();
					}
				}
			}
		}

		public CharacterTooltip(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, SettingsModel settings)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			_currentCharacter = currentCharacter;
			_textureManager = textureManager;
			_data = data;
			base.TextureRectangle = new Rectangle(60, 25, 250, 250);
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			base.BorderColor = Color.get_Black();
			base.BorderWidth = new RectangleDimensions(2);
			base.BackgroundColor = Color.get_Black() * 0.6f;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			((Container)this).set_AutoSizePadding(new Point(5, 5));
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_ControlPadding(new Vector2(5f, 2f));
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(5f, 0f));
			((Container)flowPanel).set_AutoSizePadding(new Point(5, 5));
			_contentPanel = flowPanel;
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)this);
			_iconDummy = dummy;
			IconLabel iconLabel = new IconLabel();
			((Control)iconLabel).set_Parent((Container)(object)_contentPanel);
			iconLabel.AutoSizeWidth = true;
			iconLabel.AutoSizeHeight = true;
			_nameLabel = iconLabel;
			IconLabel iconLabel2 = new IconLabel();
			((Control)iconLabel2).set_Parent((Container)(object)_contentPanel);
			iconLabel2.AutoSizeWidth = true;
			iconLabel2.AutoSizeHeight = true;
			_levelLabel = iconLabel2;
			IconLabel iconLabel3 = new IconLabel();
			((Control)iconLabel3).set_Parent((Container)(object)_contentPanel);
			iconLabel3.AutoSizeWidth = true;
			iconLabel3.AutoSizeHeight = true;
			_genderLabel = iconLabel3;
			IconLabel iconLabel4 = new IconLabel();
			((Control)iconLabel4).set_Parent((Container)(object)_contentPanel);
			iconLabel4.AutoSizeWidth = true;
			iconLabel4.AutoSizeHeight = true;
			_raceLabel = iconLabel4;
			IconLabel iconLabel5 = new IconLabel();
			((Control)iconLabel5).set_Parent((Container)(object)_contentPanel);
			iconLabel5.AutoSizeWidth = true;
			iconLabel5.AutoSizeHeight = true;
			_professionLabel = iconLabel5;
			IconLabel iconLabel6 = new IconLabel();
			((Control)iconLabel6).set_Parent((Container)(object)_contentPanel);
			iconLabel6.AutoSizeWidth = true;
			iconLabel6.AutoSizeHeight = true;
			_mapLabel = iconLabel6;
			CraftingControl craftingControl = new CraftingControl(data, settings);
			((Control)craftingControl).set_Parent((Container)(object)_contentPanel);
			((Control)craftingControl).set_Width(((Control)_contentPanel).get_Width());
			((Control)craftingControl).set_Height(20);
			craftingControl.Character = Character;
			_craftingControl = craftingControl;
			IconLabel iconLabel7 = new IconLabel();
			((Control)iconLabel7).set_Parent((Container)(object)_contentPanel);
			iconLabel7.AutoSizeWidth = true;
			iconLabel7.AutoSizeHeight = true;
			_lastLoginLabel = iconLabel7;
			TagFlowPanel tagFlowPanel = new TagFlowPanel();
			((Control)tagFlowPanel).set_Parent((Container)(object)_contentPanel);
			tagFlowPanel.Font = _lastLoginLabel.Font;
			((FlowPanel)tagFlowPanel).set_FlowDirection((ControlFlowDirection)0);
			((FlowPanel)tagFlowPanel).set_ControlPadding(new Vector2(3f, 2f));
			((Container)tagFlowPanel).set_HeightSizingMode((SizingMode)1);
			((Control)tagFlowPanel).set_Visible(false);
			_tagPanel = tagFlowPanel;
			_dataControls = new List<Control>
			{
				(Control)(object)_nameLabel,
				(Control)(object)_levelLabel,
				(Control)(object)_genderLabel,
				(Control)(object)_raceLabel,
				(Control)(object)_professionLabel,
				(Control)(object)_mapLabel,
				(Control)(object)_lastLoginLabel,
				(Control)(object)_craftingControl,
				(Control)(object)_tagPanel
			};
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateContainer(gameTime);
			((Control)this).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
				.X, Control.get_Input().get_Mouse().get_Position()
				.Y + 35));
			if (Character != null && ((Control)_lastLoginLabel).get_Visible() && _currentCharacter?.Invoke() != Character)
			{
				TimeSpan ts = DateTimeOffset.UtcNow.Subtract(Character.LastLogin);
				_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, Math.Floor(ts.TotalDays), ts.Hours, ts.Minutes, ts.Seconds);
			}
			UpdateLayout();
			if (_updateCharacter && ((Control)this).get_Visible())
			{
				UpdateCharacterInfo();
			}
		}

		public void UpdateLayout()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			UpdateLabelLayout();
			UpdateSize();
			_contentRectangle = new Rectangle(Point.get_Zero(), ((Control)_contentPanel).get_Size());
			((Control)_contentPanel).set_Location(((Rectangle)(ref _contentRectangle)).get_Location());
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
		}

		public void UpdateLabelLayout()
		{
			((Control)_nameLabel).set_Visible(true);
			_nameLabel.Font = NameFont;
			((Control)_levelLabel).set_Visible(true);
			_levelLabel.Font = Font;
			((Control)_professionLabel).set_Visible(true);
			_professionLabel.Font = Font;
			((Control)_genderLabel).set_Visible(true);
			_genderLabel.Font = Font;
			((Control)_raceLabel).set_Visible(true);
			_raceLabel.Font = Font;
			((Control)_mapLabel).set_Visible(true);
			_mapLabel.Font = Font;
			((Control)_lastLoginLabel).set_Visible(true);
			_lastLoginLabel.Font = Font;
			((Control)_craftingControl).set_Visible(true);
			_craftingControl.Font = Font;
			((Control)_tagPanel).set_Visible(Character.Tags.Count > 0);
			_tagPanel.Font = Font;
			((Control)_craftingControl).set_Height(Font.get_LineHeight() + 2);
		}

		public void UpdateSize()
		{
			IEnumerable<Control> visibleControls = _dataControls.Where((Control e) => e.get_Visible());
			visibleControls.Count();
			int height = ((visibleControls.Count() > 0) ? visibleControls.Aggregate(0, (int result, Control ctrl) => result + ctrl.get_Height() + (int)((FlowPanel)_contentPanel).get_ControlPadding().Y) : 0);
			if (visibleControls.Count() > 0)
			{
				visibleControls.Max((Control ctrl) => (ctrl != _tagPanel) ? ctrl.get_Width() : 0);
			}
			((Control)_contentPanel).set_Height(height);
			((Control)_contentPanel).set_Width(((Control)this).get_Width());
			_tagPanel.FitWidestTag(((Control)this).get_Width() - 10);
		}

		protected override void OnShown(EventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			base.OnShown(e);
			((Control)this).set_Location(new Point(Control.get_Input().get_Mouse().get_Position()
				.X, Control.get_Input().get_Mouse().get_Position()
				.Y + 35));
		}

		private void ApplyCharacter(object sender, EventArgs e)
		{
			_updateCharacter = true;
		}

		private void UpdateCharacterInfo()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			_updateCharacter = false;
			_nameLabel.Text = Character.Name;
			_nameLabel.TextColor = new Color(208, 188, 142, 255);
			_levelLabel.Text = string.Format(strings.LevelAmount, Character.Level);
			_levelLabel.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_levelLabel.Icon = AsyncTexture2D.FromAssetId(157085);
			_professionLabel.Icon = Character.SpecializationIcon;
			_professionLabel.Text = Character.SpecializationName;
			if (_professionLabel.Icon != null)
			{
				_professionLabel.TextureRectangle = ((_professionLabel.Icon.get_Width() == 32) ? new Rectangle(2, 2, 28, 28) : new Rectangle(4, 4, 56, 56));
			}
			IconLabel genderLabel = _genderLabel;
			Gender gender = Character.Gender;
			genderLabel.Text = ((object)(Gender)(ref gender)).ToString();
			_genderLabel.Icon = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Gender));
			_raceLabel.Text = _data.Races[Character.Race].Name;
			_raceLabel.Icon = _data.Races[Character.Race].Icon;
			_mapLabel.Text = _data.GetMapById(Character.Map).Name;
			_mapLabel.TextureRectangle = new Rectangle(2, 2, 28, 28);
			_mapLabel.Icon = AsyncTexture2D.FromAssetId(358406);
			_lastLoginLabel.Icon = AsyncTexture2D.FromAssetId(841721);
			_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, 0, 0, 0, 0);
			_lastLoginLabel.TextureRectangle = Rectangle.get_Empty();
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
					((Control)tag3).set_Parent((Container)(object)_tagPanel);
					tag3.Text = tag;
					tag3.Active = true;
					tag3.ShowDelete = false;
					tag3.CanInteract = false;
					tags.Add(tag3);
				}
			}
			_craftingControl.Character = Character;
			UpdateLabelLayout();
			UpdateSize();
		}
	}
}
