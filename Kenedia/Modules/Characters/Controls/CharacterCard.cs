using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Kenedia.Modules.Characters.Models;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Characters.Views;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Characters.Controls
{
	public class CharacterCard : Panel
	{
		private readonly List<Control> _dataControls = new List<Control>();

		private bool _updateCharacter;

		private readonly AsyncTexture2D _iconFrame = AsyncTexture2D.FromAssetId(1414041);

		private readonly AsyncTexture2D _loginTexture = AsyncTexture2D.FromAssetId(157092);

		private readonly AsyncTexture2D _loginTextureHovered = AsyncTexture2D.FromAssetId(157094);

		private readonly AsyncTexture2D _cogTexture = AsyncTexture2D.FromAssetId(157109);

		private readonly AsyncTexture2D _cogTextureHovered = AsyncTexture2D.FromAssetId(157111);

		private readonly AsyncTexture2D _presentTexture = AsyncTexture2D.FromAssetId(593864);

		private readonly AsyncTexture2D _presentTextureOpen = AsyncTexture2D.FromAssetId(593865);

		private readonly IconLabel _nameLabel;

		private readonly IconLabel _levelLabel;

		private readonly IconLabel _professionLabel;

		private readonly IconLabel _raceLabel;

		private readonly IconLabel _genderLabel;

		private readonly IconLabel _mapLabel;

		private readonly IconLabel _lastLoginLabel;

		private readonly TagFlowPanel _tagPanel;

		private readonly CraftingControl _craftingControl;

		private readonly BasicTooltip _textTooltip;

		private readonly CharacterTooltip _characterTooltip;

		private readonly FlowPanel _contentPanel;

		private readonly Dummy _iconDummy;

		private Rectangle _loginRect;

		private Rectangle _cogRect;

		private Rectangle _controlBounds = Rectangle.get_Empty();

		private Rectangle _textBounds;

		private Rectangle _iconRectangle;

		private readonly bool _created;

		private bool _dragging;

		private int _cogSize;

		private int _iconSize;

		private Character_Model _character;

		private readonly List<Tag> _tags = new List<Tag>();

		private readonly Func<Character_Model> _currentCharacter;

		private readonly TextureManager _textureManager;

		private readonly Data _data;

		private readonly MainWindow _mainWindow;

		private readonly SettingsModel _settings;

		private double _lastUniform;

		private Character_Model CurrentCharacter => _currentCharacter?.Invoke();

		public List<CharacterCard> AttachedCards { get; set; } = new List<CharacterCard>();


		public BitmapFont NameFont { get; set; } = GameService.Content.get_DefaultFont14();


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


		public double Index
		{
			get
			{
				return (Character != null) ? Character.Index : 0;
			}
			set
			{
				if (Character != null)
				{
					Character.Index = (int)value;
				}
			}
		}

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
						_character.Deleted -= CharacterDeleted;
					}
					_character = value;
					_characterTooltip.Character = value;
					if (value != null)
					{
						_character.Updated += ApplyCharacter;
						_character.Deleted += CharacterDeleted;
						UpdateCharacterInfo();
					}
				}
			}
		}

		public Rectangle ControlContentBounds
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _controlBounds;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				_controlBounds = value;
				_ = _controlBounds;
				AdaptNewBounds();
			}
		}

		public CharacterCard(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, MainWindow mainWindow, SettingsModel settings)
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			_currentCharacter = currentCharacter;
			_textureManager = textureManager;
			_data = data;
			_mainWindow = mainWindow;
			_settings = settings;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.BackgroundColor = new Color(0, 0, 0, 75);
			((Container)this).set_AutoSizePadding(new Point(0, 2));
			FlowPanel flowPanel = new FlowPanel();
			((Control)flowPanel).set_Parent((Container)(object)this);
			((FlowPanel)flowPanel).set_FlowDirection((ControlFlowDirection)3);
			((FlowPanel)flowPanel).set_OuterControlPadding(new Vector2(5f, 5f));
			_contentPanel = flowPanel;
			Dummy dummy = new Dummy();
			((Control)dummy).set_Parent((Container)(object)this);
			((Control)dummy).set_Size(Point.get_Zero());
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
			CraftingControl craftingControl = new CraftingControl(data, _settings);
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
			BasicTooltip basicTooltip = new BasicTooltip();
			((Control)basicTooltip).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)basicTooltip).set_ZIndex(1000);
			((Control)basicTooltip).set_Size(new Point(300, 50));
			((Control)basicTooltip).set_Visible(false);
			_textTooltip = basicTooltip;
			((Control)_textTooltip).add_Shown((EventHandler<EventArgs>)TextTooltip_Shown);
			CharacterTooltip characterTooltip = new CharacterTooltip(currentCharacter, textureManager, data, _settings);
			((Control)characterTooltip).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)characterTooltip).set_ZIndex(1001);
			((Control)characterTooltip).set_Size(new Point(300, 50));
			((Control)characterTooltip).set_Visible(false);
			_characterTooltip = characterTooltip;
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)ApplyCharacter);
			_settings.AppearanceSettingChanged += Settings_AppearanceSettingChanged;
			_created = true;
		}

		public void UniformWithAttached()
		{
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			double now = Common.Now();
			if (_lastUniform == now)
			{
				return;
			}
			List<CharacterCard> attachedCards = AttachedCards;
			if (attachedCards != null && attachedCards.Count() > 0)
			{
				int maxWidth = AttachedCards.Max((CharacterCard e) => e.CalculateLayout().Width);
				AttachedCards.ForEach(delegate(CharacterCard e)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					Rectangle controlContentBounds2 = e.ControlContentBounds;
					e.ControlContentBounds = new Rectangle(((Rectangle)(ref controlContentBounds2)).get_Location(), new Point(maxWidth, e.ControlContentBounds.Height));
				});
				AttachedCards.ForEach(delegate(CharacterCard e)
				{
					e._lastUniform = now;
				});
				Rectangle controlContentBounds = ControlContentBounds;
				ControlContentBounds = new Rectangle(((Rectangle)(ref controlContentBounds)).get_Location(), new Point(maxWidth, ControlContentBounds.Height));
			}
			else
			{
				ControlContentBounds = CalculateLayout();
			}
		}

		public Rectangle CalculateLayout()
		{
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			if (_created && ((Control)this).get_Visible())
			{
				UpdateDataControlsVisibility();
				((Control)_contentPanel).set_Visible(_settings.PanelLayout.get_Value() != SettingsModel.CharacterPanelLayout.OnlyIcons);
				_tagPanel.FitWidestTag(_dataControls.Max((Control e) => (e.get_Visible() && e != _tagPanel) ? e.get_Width() : 0));
				IEnumerable<Control> controls = _dataControls.Where((Control e) => e.get_Visible());
				Control firstControl = ((controls.Count() <= 0) ? null : _dataControls.Where((Control e) => e.get_Visible() && e is IFontControl)?.FirstOrDefault());
				bool anyVisible = ((Control)_contentPanel).get_Visible() && controls.Count() > 0;
				int width = (anyVisible ? (controls.Max((Control e) => e.get_Width()) + (int)(((FlowPanel)_contentPanel).get_OuterControlPadding().X * 2f)) : 0);
				int height = (anyVisible ? controls.Aggregate((int)(((FlowPanel)_contentPanel).get_OuterControlPadding().Y * 2f), (int result, Control e) => result + e.get_Height() + (int)((FlowPanel)_contentPanel).get_ControlPadding().Y) : 0);
				SettingsModel.PanelSizes pSize = _settings.PanelSize.get_Value();
				_iconSize = ((_settings.PanelLayout.get_Value() != SettingsModel.CharacterPanelLayout.OnlyText) ? (pSize switch
				{
					SettingsModel.PanelSizes.Large => 112, 
					SettingsModel.PanelSizes.Normal => 80, 
					SettingsModel.PanelSizes.Small => 64, 
					_ => _settings.CustomCharacterIconSize.get_Value(), 
				}) : 0);
				if (_settings.CharacterPanelFixedWidth.get_Value())
				{
					width = _settings.CharacterPanelWidth.get_Value() - _iconSize;
				}
				_iconRectangle = new Rectangle(0, 0, _iconSize, _iconSize);
				_cogSize = Math.Max(20, ((firstControl != null) ? ((IFontControl)firstControl).Font.get_LineHeight() : Font.get_LineHeight()) - 4);
				_cogSize = ((!anyVisible) ? (_iconSize / 5) : _cogSize);
				if (firstControl != null && width < firstControl.get_Width() + 5 + _cogSize)
				{
					width += (anyVisible ? (5 + _cogSize) : 0);
				}
				_textBounds = new Rectangle(((Rectangle)(ref _iconRectangle)).get_Right() + ((anyVisible && _iconSize > 0) ? 5 : 0), 0, width, height);
				((Control)_contentPanel).set_Location(((Rectangle)(ref _textBounds)).get_Location());
				((Control)_contentPanel).set_Size(((Rectangle)(ref _textBounds)).get_Size());
				_controlBounds = new Rectangle(((Rectangle)(ref _iconRectangle)).get_Left(), ((Rectangle)(ref _iconRectangle)).get_Top(), ((Rectangle)(ref _textBounds)).get_Right() - ((Rectangle)(ref _iconRectangle)).get_Left(), Math.Max(_textBounds.Height, _iconRectangle.Height));
				_cogRect = new Rectangle(_controlBounds.Width - _cogSize - 4, 4, _cogSize, _cogSize);
				int size = ((_iconSize > 0) ? Math.Min(56, _iconRectangle.Width - 8) : Math.Min(56, Math.Min(_textBounds.Width, _textBounds.Height) - 8));
				int pad = (_iconRectangle.Width - size) / 2;
				_loginRect = ((!anyVisible) ? new Rectangle(pad, pad, size, size) : ((_iconSize > 0) ? new Rectangle((_iconRectangle.Width - size) / 2, (_iconRectangle.Height - size) / 2, size, size) : new Rectangle((_textBounds.Width - size) / 2, (_textBounds.Height - size) / 2, size, size)));
			}
			return _controlBounds;
		}

		public override void RecalculateLayout()
		{
			base.RecalculateLayout();
		}

		public override void PaintBeforeChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			base.PaintBeforeChildren(spriteBatch, bounds);
			SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _iconRectangle, (Rectangle?)Rectangle.get_Empty(), Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
			if (Character == null)
			{
				return;
			}
			if (_settings.PanelLayout.get_Value() != SettingsModel.CharacterPanelLayout.OnlyText)
			{
				if (!Character.HasDefaultIcon && Character.Icon != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(Character.Icon), _iconRectangle, (Rectangle?)Character.Icon.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					return;
				}
				AsyncTexture2D texture = Character.SpecializationIcon;
				if (texture != null)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_iconFrame), new Rectangle(_iconRectangle.X, _iconRectangle.Y, _iconRectangle.Width, _iconRectangle.Height), (Rectangle?)_iconFrame.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_iconFrame), new Rectangle(_iconRectangle.Width, _iconRectangle.Height, _iconRectangle.Width, _iconRectangle.Height), (Rectangle?)_iconFrame.get_Bounds(), Color.get_White(), 3.14f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(texture), new Rectangle(8, 8, _iconRectangle.Width - 16, _iconRectangle.Height - 16), (Rectangle?)texture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
			else if (((Control)this).get_MouseOver())
			{
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _iconRectangle, (Rectangle?)Rectangle.get_Empty(), Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_038f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0399: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0419: Unknown result type (might be due to invalid IL or missing references)
			//IL_0439: Unknown result type (might be due to invalid IL or missing references)
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Unknown result type (might be due to invalid IL or missing references)
			//IL_056d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0594: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0627: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			if (((Control)this).get_MouseOver())
			{
				((Control)_textTooltip).set_Visible(false);
				if (_settings.PanelLayout.get_Value() != SettingsModel.CharacterPanelLayout.OnlyText)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _iconRectangle, (Rectangle?)Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Format(strings.LoginWith, Character.Name));
					((Control)_textTooltip).set_Visible(((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((!Character.HasBirthdayPresent) ? (((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _loginTextureHovered : _loginTexture) : (((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _presentTextureOpen : _presentTexture)), _loginRect, (Rectangle?)_loginTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Format(strings.LoginWith, Character.Name));
					((Control)_textTooltip).set_Visible(((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((!Character.HasBirthdayPresent) ? (((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _loginTextureHovered : _loginTexture) : (((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _presentTextureOpen : _presentTexture)), _loginRect, (Rectangle?)_loginTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Rectangle)(ref _cogRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _cogTextureHovered : _cogTexture), _cogRect, (Rectangle?)new Rectangle(5, 5, 22, 22), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				if (((Rectangle)(ref _cogRect)).Contains(((Control)this).get_RelativeMousePosition()))
				{
					_textTooltip.Text = string.Format(strings.AdjustSettings, Character.Name);
					((Control)_textTooltip).set_Visible(true);
				}
				Color color = Colors.ColonialWhite;
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.5f);
				SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), (Rectangle?)Rectangle.get_Empty(), color * 0.6f);
			}
			if (!((Control)this).get_MouseOver() && Character != null && Character.HasBirthdayPresent)
			{
				if (_settings.PanelLayout.get_Value() != SettingsModel.CharacterPanelLayout.OnlyText)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _iconRectangle, (Rectangle?)Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_presentTexture), _loginRect, (Rectangle?)_presentTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(_presentTexture), _loginRect, (Rectangle?)_presentTexture.get_Bounds(), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (Character != null && ((Control)_lastLoginLabel).get_Visible())
			{
				if (CurrentCharacter != Character)
				{
					TimeSpan ts = DateTimeOffset.UtcNow.Subtract(Character.LastLogin);
					_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, Math.Floor(ts.TotalDays), ts.Hours, ts.Minutes, ts.Seconds);
					if (!Character.HasBirthdayPresent)
					{
					}
				}
				else
				{
					_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, 0, 0, 0, 0);
				}
			}
			if (!((Control)this).get_MouseOver() && ((Control)_textTooltip).get_Visible())
			{
				((Control)_textTooltip).set_Visible(((Control)this).get_MouseOver());
			}
			if (!((Control)this).get_MouseOver() && ((Control)_characterTooltip).get_Visible())
			{
				((Control)_characterTooltip).set_Visible(((Control)this).get_MouseOver());
			}
			if (_updateCharacter && _created && ((Control)this).get_Visible())
			{
				UpdateCharacterInfo();
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			_mainWindow.ShowAttached((_mainWindow.CharacterEdit.Character != Character || !((Control)_mainWindow.CharacterEdit).get_Visible()) ? _mainWindow.CharacterEdit : null);
			_mainWindow.CharacterEdit.Character = Character;
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			_003C_003En__0(e);
			if (e.get_IsDoubleClick() && _settings.DoubleClickToEnter.get_Value())
			{
				Character.Swap();
				return;
			}
			if (((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				PlayerCharacter player = GameService.Gw2Mumble.get_PlayerCharacter();
				if (player != null && player.get_Name() == Character.Name && Character.HasBirthdayPresent)
				{
					await _settings.MailKey.get_Value().PerformPress(50, triggerSystem: false);
					_mainWindow.CharacterEdit.Character = Character;
					_mainWindow.ShowAttached(_mainWindow.CharacterEdit);
				}
				else
				{
					Character.Swap();
					_mainWindow.ShowAttached();
				}
			}
			if (((Rectangle)(ref _cogRect)).Contains(((Control)this).get_RelativeMousePosition()))
			{
				((Control)_mainWindow.CharacterEdit).set_Visible(!((Control)_mainWindow.CharacterEdit).get_Visible() || _mainWindow.CharacterEdit.Character != Character);
				_mainWindow.CharacterEdit.Character = Character;
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			KeyboardState state = Keyboard.GetState();
			if (((KeyboardState)(ref state)).IsKeyDown((Keys)162))
			{
				_mainWindow.DraggingControl.CharacterControl = this;
				_dragging = true;
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			if (_dragging)
			{
				_mainWindow.DraggingControl.CharacterControl = null;
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			if (_textTooltip == null || (!((Control)_textTooltip).get_Visible() && _settings.ShowDetailedTooltip.get_Value()))
			{
				((Control)_characterTooltip).Show();
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			if (_textTooltip == null || (!((Control)_textTooltip).get_Visible() && _settings.ShowDetailedTooltip.get_Value()))
			{
				((Control)_characterTooltip).Show();
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			((Control)this).OnHidden(e);
			BasicTooltip textTooltip = _textTooltip;
			if (textTooltip != null)
			{
				((Control)textTooltip).Hide();
			}
			CharacterTooltip characterTooltip = _characterTooltip;
			if (characterTooltip != null)
			{
				((Control)characterTooltip).Hide();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)ApplyCharacter);
			((Control)_textTooltip).remove_Shown((EventHandler<EventArgs>)TextTooltip_Shown);
			if (_character != null)
			{
				_character.Updated -= ApplyCharacter;
				_character.Deleted -= CharacterDeleted;
			}
			((IEnumerable<IDisposable>)_dataControls)?.DisposeAll();
			FlowPanel contentPanel = _contentPanel;
			if (contentPanel != null)
			{
				((Control)contentPanel).Dispose();
			}
			BasicTooltip textTooltip = _textTooltip;
			if (textTooltip != null)
			{
				((Control)textTooltip).Dispose();
			}
			CharacterTooltip characterTooltip = _characterTooltip;
			if (characterTooltip != null)
			{
				((Control)characterTooltip).Dispose();
			}
			_iconFrame.Dispose();
			_loginTexture.Dispose();
			_loginTextureHovered.Dispose();
			_cogTexture.Dispose();
			_cogTextureHovered.Dispose();
			_presentTexture.Dispose();
			_presentTextureOpen.Dispose();
			((IEnumerable<IDisposable>)((Container)_tagPanel).get_Children()).DisposeAll();
			((IEnumerable<IDisposable>)_tagPanel).DisposeAll();
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
			_mainWindow.CharacterCards.Remove(this);
			_updateCharacter = true;
		}

		private BitmapFont GetFont(bool nameFont = false)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			FontSize fontSize = (FontSize)8;
			switch (_settings.PanelSize.get_Value())
			{
			case SettingsModel.PanelSizes.Small:
				fontSize = (FontSize)(nameFont ? 16 : 12);
				break;
			case SettingsModel.PanelSizes.Normal:
				fontSize = (FontSize)(nameFont ? 18 : 14);
				break;
			case SettingsModel.PanelSizes.Large:
				fontSize = (FontSize)(nameFont ? 22 : 18);
				break;
			case SettingsModel.PanelSizes.Custom:
				fontSize = (FontSize)(nameFont ? _settings.CustomCharacterNameFontSize.get_Value() : _settings.CustomCharacterFontSize.get_Value());
				break;
			}
			return GameService.Content.GetFont((FontFace)0, fontSize, (FontStyle)0);
		}

		private void TextTooltip_Shown(object sender, EventArgs e)
		{
			CharacterTooltip characterTooltip = _characterTooltip;
			if (characterTooltip != null)
			{
				((Control)characterTooltip).Hide();
			}
		}

		private void CharacterDeleted(object sender, EventArgs e)
		{
			((Control)this).Dispose();
		}

		private void ApplyCharacter(object sender, EventArgs e)
		{
			_updateCharacter = true;
		}

		private void UpdateCharacterInfo()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
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
			_lastLoginLabel.Icon = AsyncTexture2D.FromAssetId(155035);
			_lastLoginLabel.TextureRectangle = new Rectangle(10, 10, 44, 44);
			_lastLoginLabel.Text = string.Format("{1} {0} {2:00}:{3:00}:{4:00}", strings.Days, 0, 0, 0, 0);
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
				_tagPanel.FitWidestTag(_dataControls.Max((Control e) => (e.get_Visible() && e != _tagPanel) ? e.get_Width() : 0));
			}
			_craftingControl.Character = Character;
			_updateCharacter = false;
			UniformWithAttached();
		}

		public void HideTooltips()
		{
			((Control)_textTooltip).Hide();
			((Control)_characterTooltip).Hide();
		}

		private void UpdateDataControlsVisibility()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			NameFont = GetFont(nameFont: true);
			Font = GetFont();
			((FlowPanel)_contentPanel).set_ControlPadding(new Vector2((float)(Font.get_LineHeight() / 10), (float)(Font.get_LineHeight() / 10)));
			((Control)_nameLabel).set_Visible(_settings.DisplayToggles.get_Value()["Name"].Show);
			_nameLabel.Font = NameFont;
			((Control)_levelLabel).set_Visible(_settings.DisplayToggles.get_Value()["Level"].Show);
			_levelLabel.Font = Font;
			((Control)_genderLabel).set_Visible(_settings.DisplayToggles.get_Value()["Gender"].Show);
			_genderLabel.Font = Font;
			((Control)_raceLabel).set_Visible(_settings.DisplayToggles.get_Value()["Race"].Show);
			_raceLabel.Font = Font;
			((Control)_professionLabel).set_Visible(_settings.DisplayToggles.get_Value()["Profession"].Show);
			_professionLabel.Font = Font;
			((Control)_lastLoginLabel).set_Visible(_settings.DisplayToggles.get_Value()["LastLogin"].Show);
			_lastLoginLabel.Font = Font;
			((Control)_mapLabel).set_Visible(_settings.DisplayToggles.get_Value()["Map"].Show);
			_mapLabel.Font = Font;
			((Control)_craftingControl).set_Visible(_settings.DisplayToggles.get_Value()["CraftingProfession"].Show);
			_craftingControl.Font = Font;
			((Control)_tagPanel).set_Visible(_settings.DisplayToggles.get_Value()["Tags"].Show && Character.Tags.Count > 0);
			_tagPanel.Font = Font;
			((Control)_craftingControl).set_Height(Font.get_LineHeight() + 2);
		}

		private void AdaptNewBounds()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Width() != _controlBounds.Width + ((Container)this).get_AutoSizePadding().X)
			{
				((Control)this).set_Width(_controlBounds.Width + ((Container)this).get_AutoSizePadding().X);
			}
			if (((Control)this).get_Height() != _controlBounds.Height + ((Container)this).get_AutoSizePadding().Y)
			{
				((Control)_iconDummy).set_Height(_controlBounds.Height);
			}
			int num;
			if (((Control)_contentPanel).get_Visible())
			{
				IEnumerable<Control> enumerable = _dataControls.Where((Control e) => e.get_Visible());
				num = ((enumerable != null && enumerable.Count() > 0) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool anyVisible = (byte)num != 0;
			_cogRect = new Rectangle(_controlBounds.Width - _cogSize - 4, 4, _cogSize, _cogSize);
			int size = ((_iconSize > 0) ? Math.Min(56, _iconRectangle.Width - 8) : Math.Min(56, Math.Min(_textBounds.Width, _textBounds.Height) - 8));
			int pad = (_iconRectangle.Width - size) / 2;
			_loginRect = ((!anyVisible) ? new Rectangle(pad, pad, size, size) : ((_iconSize > 0) ? new Rectangle((_iconRectangle.Width - size) / 2, (_iconRectangle.Height - size) / 2, size, size) : new Rectangle((_textBounds.Width - size) / 2, (_textBounds.Height - size) / 2, size, size)));
		}

		private void Settings_AppearanceSettingChanged(object sender, EventArgs e)
		{
			ApplyCharacter(null, null);
		}
	}
}
