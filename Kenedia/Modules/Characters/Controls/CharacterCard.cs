using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Gw2Mumble;
using Blish_HUD.Input;
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
		private readonly AsyncTexture2D _iconFrame = AsyncTexture2D.FromAssetId(1414041);

		private readonly AsyncTexture2D _loginTexture = AsyncTexture2D.FromAssetId(60968);

		private readonly AsyncTexture2D _loginTextureHovered = AsyncTexture2D.FromAssetId(60968);

		private readonly AsyncTexture2D _cogTexture = AsyncTexture2D.FromAssetId(157109);

		private readonly AsyncTexture2D _cogTextureHovered = AsyncTexture2D.FromAssetId(157111);

		private readonly AsyncTexture2D _presentTexture = AsyncTexture2D.FromAssetId(593864);

		private readonly AsyncTexture2D _presentTextureOpen = AsyncTexture2D.FromAssetId(593865);

		private readonly BasicTooltip _textTooltip;

		private readonly CharacterTooltip _characterTooltip;

		private readonly FlowPanel _contentPanel;

		private readonly CharacterLabels _infoLabels;

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

		private readonly TextureManager _textureManager;

		private readonly Data _data;

		private readonly MainWindow _mainWindow;

		private readonly Settings _settings;

		private double _lastUniform;

		private bool _updateCharacter;

		public bool IsDraggingTarget { get; set; }

		public List<CharacterCard> AttachedCards { get; set; } = new List<CharacterCard>();


		public BitmapFont NameFont { get; set; } = GameService.Content.get_DefaultFont14();


		public BitmapFont Font { get; set; } = GameService.Content.get_DefaultFont14();


		public int Index
		{
			get
			{
				if (Character == null)
				{
					return 0;
				}
				return Character.Index;
			}
			set
			{
				if (Character != null)
				{
					_infoLabels.UpdateCharacterInfo();
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
				Character_Model temp = _character;
				if (Common.SetProperty(ref _character, value))
				{
					if (temp != null)
					{
						temp.Deleted -= CharacterDeleted;
						temp.Updated -= ApplyCharacter;
					}
					if (_character != null)
					{
						_character.Deleted += CharacterDeleted;
						_character.Updated += ApplyCharacter;
					}
				}
				if (_characterTooltip != null)
				{
					_characterTooltip.Character = value;
				}
				if (_infoLabels != null)
				{
					_infoLabels.Character = value;
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

		public CharacterCard()
		{
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.BackgroundColor = Color.get_Black() * 0.5f;
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
			_infoLabels = new CharacterLabels(_contentPanel);
			BasicTooltip basicTooltip = new BasicTooltip();
			((Control)basicTooltip).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)basicTooltip).set_ZIndex(1000);
			((Control)basicTooltip).set_Size(new Point(300, 50));
			((Control)basicTooltip).set_Visible(false);
			_textTooltip = basicTooltip;
			((Control)_textTooltip).add_Shown((EventHandler<EventArgs>)TextTooltip_Shown);
			_created = true;
			_updateCharacter = true;
		}

		public CharacterCard(CharacterCard card)
			: this()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			_textureManager = card._textureManager;
			_data = card._data;
			_mainWindow = card._mainWindow;
			_settings = card._settings;
			((Control)this).set_Size(((Control)card).get_Size());
			Character = card._character;
			_infoLabels.TextureManager = _textureManager;
			_infoLabels.Data = _data;
			_infoLabels.Settings = _settings;
			_settings.AppearanceSettingChanged += Settings_AppearanceSettingChanged;
			Settings_AppearanceSettingChanged(this, null);
		}

		private void Settings_AppearanceSettingChanged(object sender, EventArgs e)
		{
			Update();
			UniformWithAttached();
		}

		private void Update()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_infoLabels.UpdateDataControlsVisibility();
			_infoLabels.UpdateCharacterInfo();
			CalculateLayout();
			AdaptNewBounds();
		}

		public CharacterCard(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, MainWindow mainWindow, Settings settings)
			: this()
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			_textureManager = textureManager;
			_data = data;
			_mainWindow = mainWindow;
			_settings = settings;
			((Container)this).set_HeightSizingMode((SizingMode)1);
			base.BackgroundColor = new Color(0, 0, 0, 75);
			((Container)this).set_AutoSizePadding(new Point(0, 2));
			_infoLabels.TextureManager = _textureManager;
			_infoLabels.Data = _data;
			_infoLabels.Settings = _settings;
			_infoLabels.CurrentCharacter = currentCharacter;
			_settings.AppearanceSettingChanged += Settings_AppearanceSettingChanged;
			CharacterTooltip characterTooltip = new CharacterTooltip(currentCharacter, textureManager, data, _settings);
			((Control)characterTooltip).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)characterTooltip).set_ZIndex(1001);
			((Control)characterTooltip).set_Size(new Point(300, 50));
			((Control)characterTooltip).set_Visible(false);
			_characterTooltip = characterTooltip;
		}

		private void ApplyCharacter(object sender, EventArgs e)
		{
			_updateCharacter = true;
		}

		public void UniformWithAttached(bool force = false)
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			double now = Common.Now();
			Update();
			if (!(_lastUniform != now || force))
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
				_lastUniform = now;
				ControlContentBounds = CalculateLayout();
				AdaptNewBounds();
			}
		}

		public Rectangle CalculateLayout()
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			if (_created && ((Control)this).get_Visible())
			{
				_infoLabels.RecalculateBounds();
				((Control)_contentPanel).set_Visible(_settings.PanelLayout.get_Value() != Settings.CharacterPanelLayout.OnlyIcons);
				IEnumerable<Control> controls = _infoLabels.DataControls.Where((Control e) => e.get_Visible());
				Control firstControl = ((controls.Count() <= 0) ? null : _infoLabels.DataControls.Where((Control e) => e.get_Visible() && e is IFontControl)?.FirstOrDefault());
				bool anyVisible = ((Control)_contentPanel).get_Visible() && controls.Count() > 0;
				int width = (anyVisible ? (controls.Max((Control e) => e.get_Width()) + (int)(((FlowPanel)_contentPanel).get_OuterControlPadding().X * 2f)) : 0);
				int height = (anyVisible ? controls.Aggregate((int)(((FlowPanel)_contentPanel).get_OuterControlPadding().Y * 2f), (int result, Control e) => result + e.get_Height() + (int)((FlowPanel)_contentPanel).get_ControlPadding().Y) : 0);
				Settings.PanelSizes pSize = _settings.PanelSize.get_Value();
				_iconSize = ((_settings.PanelLayout.get_Value() != Settings.CharacterPanelLayout.OnlyText) ? (pSize switch
				{
					Settings.PanelSizes.Large => 112, 
					Settings.PanelSizes.Normal => 80, 
					Settings.PanelSizes.Small => 64, 
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
			if (_settings.PanelLayout.get_Value() != Settings.CharacterPanelLayout.OnlyText)
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
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_0420: Unknown result type (might be due to invalid IL or missing references)
			//IL_042a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0457: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_046d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_0488: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Unknown result type (might be due to invalid IL or missing references)
			//IL_051e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054a: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0559: Unknown result type (might be due to invalid IL or missing references)
			//IL_055f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0604: Unknown result type (might be due to invalid IL or missing references)
			//IL_060e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0614: Unknown result type (might be due to invalid IL or missing references)
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_063f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_064f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0677: Unknown result type (might be due to invalid IL or missing references)
			//IL_067c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_068c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
			((Container)this).PaintAfterChildren(spriteBatch, bounds);
			if (((Control)this).get_MouseOver())
			{
				((Control)_textTooltip).set_Visible(false);
				bool loginHovered = !IsDraggingTarget && ((Rectangle)(ref _loginRect)).Contains(((Control)this).get_RelativeMousePosition());
				if (_settings.PanelLayout.get_Value() != Settings.CharacterPanelLayout.OnlyText)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), _iconRectangle, (Rectangle?)Rectangle.get_Empty(), IsDraggingTarget ? Color.get_Transparent() : (Color.get_Black() * 0.5f), 0f, default(Vector2), (SpriteEffects)0);
					if (!IsDraggingTarget)
					{
						int num;
						if (((Control)_contentPanel).get_Visible())
						{
							IEnumerable<Control> enumerable = _infoLabels.DataControls.Where((Control e) => e.get_Visible());
							num = ((enumerable != null && enumerable.Count() > 0) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool anyVisible = (byte)num != 0;
						_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Format(strings.LoginWith, Character.Name));
						((Control)_textTooltip).set_Visible(loginHovered && anyVisible);
						SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture)), _loginRect, (Rectangle?)_loginTexture.get_Bounds(), (Color)(loginHovered ? Color.get_White() : new Color(215, 215, 215)), 0f, default(Vector2), (SpriteEffects)0);
					}
				}
				else
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, Textures.get_Pixel(), bounds, (Rectangle?)Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Empty);
					((Control)_textTooltip).set_Visible(!string.IsNullOrEmpty(_textTooltip.Text));
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit((!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture)), _loginRect, (Rectangle?)_loginTexture.get_Bounds(), (Color)(loginHovered ? Color.get_White() : new Color(200, 200, 200)), 0f, default(Vector2), (SpriteEffects)0);
				}
				if (!IsDraggingTarget)
				{
					SpriteBatchExtensions.DrawOnCtrl(spriteBatch, (Control)(object)this, AsyncTexture2D.op_Implicit(((Rectangle)(ref _cogRect)).Contains(((Control)this).get_RelativeMousePosition()) ? _cogTextureHovered : _cogTexture), _cogRect, (Rectangle?)new Rectangle(5, 5, 22, 22), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (((Rectangle)(ref _cogRect)).Contains(((Control)this).get_RelativeMousePosition()))
					{
						_textTooltip.Text = string.Format(strings.AdjustSettings, Character.Name);
						((Control)_textTooltip).set_Visible(true);
					}
				}
			}
			if (!((Control)this).get_MouseOver() && Character != null && Character.HasBirthdayPresent)
			{
				if (_settings.PanelLayout.get_Value() != Settings.CharacterPanelLayout.OnlyText)
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
			if (IsDraggingTarget || (_mainWindow != null && ((Rectangle)(ref bounds)).Contains(((Control)this).get_RelativeMousePosition()) && _mainWindow.IsActive) || ((Control)this).get_MouseOver())
			{
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
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			((Container)this).UpdateContainer(gameTime);
			if (!IsDraggingTarget)
			{
				if (!((Control)this).get_MouseOver() && ((Control)_textTooltip).get_Visible())
				{
					((Control)_textTooltip).set_Visible(((Control)this).get_MouseOver());
				}
				if (!((Control)this).get_MouseOver() && ((Control)_characterTooltip).get_Visible())
				{
					((Control)_characterTooltip).set_Visible(((Control)this).get_MouseOver());
				}
			}
			_infoLabels.Update();
			if (_updateCharacter && _created && ((Control)this).get_Visible())
			{
				Settings_AppearanceSettingChanged(this, null);
				_updateCharacter = false;
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			((Control)this).OnRightMouseButtonPressed(e);
			if (!IsDraggingTarget)
			{
				_mainWindow.ShowAttached((_mainWindow.CharacterEdit.Character != Character || !((Control)_mainWindow.CharacterEdit).get_Visible()) ? _mainWindow.CharacterEdit : null);
				_mainWindow.CharacterEdit.Character = Character;
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			if (IsDraggingTarget)
			{
				return;
			}
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
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnLeftMouseButtonPressed(e);
			if (IsDraggingTarget)
			{
				return;
			}
			KeyboardState state = Keyboard.GetState();
			if (((KeyboardState)(ref state)).IsKeyDown((Keys)162) && _settings.SortType.get_Value() == Settings.SortBy.Custom)
			{
				_mainWindow.DraggingControl.StartDragging(this);
				_dragging = true;
				CharacterTooltip characterTooltip = _characterTooltip;
				if (characterTooltip != null)
				{
					((Control)characterTooltip).Hide();
				}
				BasicTooltip textTooltip = _textTooltip;
				if (textTooltip != null)
				{
					((Control)textTooltip).Hide();
				}
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			((Control)this).OnLeftMouseButtonReleased(e);
			if (!IsDraggingTarget && _dragging)
			{
				_mainWindow.DraggingControl.EndDragging();
				_dragging = false;
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			if (!IsDraggingTarget && !_mainWindow.DraggingControl.IsActive && (_textTooltip == null || (!((Control)_textTooltip).get_Visible() && _settings.ShowDetailedTooltip.get_Value())))
			{
				CharacterTooltip characterTooltip = _characterTooltip;
				if (characterTooltip != null)
				{
					((Control)characterTooltip).Show();
				}
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			if (!IsDraggingTarget && !_mainWindow.DraggingControl.IsActive && (_textTooltip == null || (!((Control)_textTooltip).get_Visible() && _settings.ShowDetailedTooltip.get_Value())))
			{
				CharacterTooltip characterTooltip = _characterTooltip;
				if (characterTooltip != null)
				{
					((Control)characterTooltip).Show();
				}
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
			((Control)_textTooltip).remove_Shown((EventHandler<EventArgs>)TextTooltip_Shown);
			if (_character != null)
			{
				_character.Deleted -= CharacterDeleted;
			}
			_infoLabels?.Dispose();
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
			((IEnumerable<IDisposable>)((Container)this).get_Children()).DisposeAll();
			_mainWindow.CharacterCards.Remove(this);
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

		public void HideTooltips()
		{
			((Control)_textTooltip).Hide();
			((Control)_characterTooltip).Hide();
		}

		private void AdaptNewBounds()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
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
				IEnumerable<Control> enumerable = _infoLabels.DataControls.Where((Control e) => e.get_Visible());
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
	}
}
