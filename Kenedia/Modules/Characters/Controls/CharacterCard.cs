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
	public class CharacterCard : Kenedia.Modules.Core.Controls.Panel
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

		private readonly Kenedia.Modules.Core.Controls.FlowPanel _contentPanel;

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


		public BitmapFont NameFont { get; set; } = GameService.Content.DefaultFont14;


		public BitmapFont Font { get; set; } = GameService.Content.DefaultFont14;


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
						temp.Deleted -= new EventHandler(CharacterDeleted);
						temp.Updated -= new EventHandler(ApplyCharacter);
					}
					if (_character != null)
					{
						_character.Deleted += new EventHandler(CharacterDeleted);
						_character.Updated += new EventHandler(ApplyCharacter);
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
			HeightSizingMode = SizingMode.AutoSize;
			base.BackgroundColor = Color.get_Black() * 0.5f;
			base.AutoSizePadding = new Point(0, 2);
			_contentPanel = new Kenedia.Modules.Core.Controls.FlowPanel
			{
				Parent = this,
				FlowDirection = ControlFlowDirection.SingleTopToBottom,
				OuterControlPadding = new Vector2(5f, 5f)
			};
			_iconDummy = new Dummy
			{
				Parent = this,
				Size = Point.get_Zero()
			};
			_infoLabels = new CharacterLabels(_contentPanel);
			_textTooltip = new BasicTooltip
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = 1000,
				Size = new Point(300, 50),
				Visible = false
			};
			_textTooltip.Shown += TextTooltip_Shown;
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
			base.Size = card.Size;
			Character = card._character;
			_infoLabels.TextureManager = _textureManager;
			_infoLabels.Data = _data;
			_infoLabels.Settings = _settings;
			_settings.AppearanceSettingChanged += new EventHandler(Settings_AppearanceSettingChanged);
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
			HeightSizingMode = SizingMode.AutoSize;
			base.BackgroundColor = new Color(0, 0, 0, 75);
			base.AutoSizePadding = new Point(0, 2);
			_infoLabels.TextureManager = _textureManager;
			_infoLabels.Data = _data;
			_infoLabels.Settings = _settings;
			_infoLabels.CurrentCharacter = currentCharacter;
			_settings.AppearanceSettingChanged += new EventHandler(Settings_AppearanceSettingChanged);
			_characterTooltip = new CharacterTooltip(currentCharacter, textureManager, data, _settings)
			{
				Parent = GameService.Graphics.SpriteScreen,
				ZIndex = 1001,
				Size = new Point(300, 50),
				Visible = false
			};
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
			double now = Common.Now;
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
			if (_created && base.Visible)
			{
				_infoLabels.RecalculateBounds();
				_contentPanel.Visible = _settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyIcons;
				IEnumerable<Control> controls = _infoLabels.DataControls.Where((Control e) => e.Visible);
				Control firstControl = ((controls.Count() <= 0) ? null : _infoLabels.DataControls.Where((Control e) => e.Visible && e is IFontControl)?.FirstOrDefault());
				bool anyVisible = _contentPanel.Visible && controls.Count() > 0;
				int width = (anyVisible ? (controls.Max((Control e) => e.Width) + (int)(_contentPanel.OuterControlPadding.X * 2f)) : 0);
				int height = (anyVisible ? controls.Aggregate((int)(_contentPanel.OuterControlPadding.Y * 2f), (int result, Control e) => result + e.Height + (int)_contentPanel.ControlPadding.Y) : 0);
				Settings.PanelSizes pSize = _settings.PanelSize.Value;
				_iconSize = ((_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText) ? (pSize switch
				{
					Settings.PanelSizes.Large => 112, 
					Settings.PanelSizes.Normal => 80, 
					Settings.PanelSizes.Small => 64, 
					_ => _settings.CustomCharacterIconSize.Value, 
				}) : 0);
				if (_settings.CharacterPanelFixedWidth.Value)
				{
					width = _settings.CharacterPanelWidth.Value - _iconSize;
				}
				_iconRectangle = new Rectangle(0, 0, _iconSize, _iconSize);
				_cogSize = Math.Max(20, ((firstControl != null) ? ((IFontControl)firstControl).Font.get_LineHeight() : Font.get_LineHeight()) - 4);
				_cogSize = ((!anyVisible) ? (_iconSize / 5) : _cogSize);
				if (firstControl != null && width < firstControl.Width + 5 + _cogSize)
				{
					width += (anyVisible ? (5 + _cogSize) : 0);
				}
				_textBounds = new Rectangle(((Rectangle)(ref _iconRectangle)).get_Right() + ((anyVisible && _iconSize > 0) ? 5 : 0), 0, width, height);
				_contentPanel.Location = ((Rectangle)(ref _textBounds)).get_Location();
				_contentPanel.Size = ((Rectangle)(ref _textBounds)).get_Size();
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
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.get_Empty(), Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
			if (Character == null)
			{
				return;
			}
			if (_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText)
			{
				if (!Character.HasDefaultIcon && Character.Icon != null)
				{
					spriteBatch.DrawOnCtrl(this, Character.Icon, _iconRectangle, Character.Icon.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					return;
				}
				AsyncTexture2D texture = Character.SpecializationIcon;
				if (texture != null)
				{
					spriteBatch.DrawOnCtrl(this, _iconFrame, new Rectangle(_iconRectangle.X, _iconRectangle.Y, _iconRectangle.Width, _iconRectangle.Height), _iconFrame.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					spriteBatch.DrawOnCtrl(this, _iconFrame, new Rectangle(_iconRectangle.Width, _iconRectangle.Height, _iconRectangle.Width, _iconRectangle.Height), _iconFrame.Bounds, Color.get_White(), 3.14f, default(Vector2), (SpriteEffects)0);
					spriteBatch.DrawOnCtrl(this, texture, new Rectangle(8, 8, _iconRectangle.Width - 16, _iconRectangle.Height - 16), texture.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
			else if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.get_Empty(), Color.get_Transparent(), 0f, default(Vector2), (SpriteEffects)0);
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
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_035e: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_048c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_049b: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0526: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			//IL_0531: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0547: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_0609: Unknown result type (might be due to invalid IL or missing references)
			//IL_060e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			//IL_063d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Unknown result type (might be due to invalid IL or missing references)
			//IL_0649: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_067a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_0686: Unknown result type (might be due to invalid IL or missing references)
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0708: Unknown result type (might be due to invalid IL or missing references)
			//IL_070e: Unknown result type (might be due to invalid IL or missing references)
			//IL_072e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0734: Unknown result type (might be due to invalid IL or missing references)
			//IL_0739: Unknown result type (might be due to invalid IL or missing references)
			//IL_0743: Unknown result type (might be due to invalid IL or missing references)
			//IL_0749: Unknown result type (might be due to invalid IL or missing references)
			//IL_076b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0771: Unknown result type (might be due to invalid IL or missing references)
			//IL_0776: Unknown result type (might be due to invalid IL or missing references)
			//IL_0780: Unknown result type (might be due to invalid IL or missing references)
			//IL_0786: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
			base.PaintAfterChildren(spriteBatch, bounds);
			if (base.MouseOver)
			{
				_textTooltip.Visible = false;
				bool loginHovered = !IsDraggingTarget && ((Rectangle)(ref _loginRect)).Contains(base.RelativeMousePosition);
				if (_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.get_Empty(), IsDraggingTarget ? Color.get_Transparent() : (Color.get_Black() * 0.5f), 0f, default(Vector2), (SpriteEffects)0);
					if (!IsDraggingTarget)
					{
						int num;
						if (_contentPanel.Visible)
						{
							IEnumerable<Control> enumerable = _infoLabels.DataControls.Where((Control e) => e.Visible);
							num = ((enumerable != null && enumerable.Count() > 0) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool anyVisible = (byte)num != 0;
						_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Format(strings.LoginWith, Character.Name));
						_textTooltip.Visible = loginHovered && anyVisible;
						spriteBatch.DrawOnCtrl(this, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture), Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered.Bounds : _loginTexture.Bounds) : (loginHovered ? _presentTextureOpen.Bounds : _presentTexture.Bounds), (Color)(loginHovered ? Color.get_White() : new Color(215, 215, 215)), 0f, default(Vector2), (SpriteEffects)0);
					}
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Empty);
					_textTooltip.Visible = !string.IsNullOrEmpty(_textTooltip.Text);
					spriteBatch.DrawOnCtrl(this, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture), Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered.Bounds : _loginTexture.Bounds) : (loginHovered ? _presentTextureOpen.Bounds : _presentTexture.Bounds), (Color)(loginHovered ? Color.get_White() : new Color(200, 200, 200)), 0f, default(Vector2), (SpriteEffects)0);
				}
				if (!IsDraggingTarget)
				{
					spriteBatch.DrawOnCtrl((Control)this, (Texture2D)(((Rectangle)(ref _cogRect)).Contains(base.RelativeMousePosition) ? _cogTextureHovered : _cogTexture), _cogRect, (Rectangle?)new Rectangle(5, 5, 22, 22), Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
					if (((Rectangle)(ref _cogRect)).Contains(base.RelativeMousePosition))
					{
						_textTooltip.Text = string.Format(strings.AdjustSettings, Character.Name);
						_textTooltip.Visible = true;
					}
				}
			}
			if (!base.MouseOver && Character != null && Character.HasBirthdayPresent)
			{
				if (_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					spriteBatch.DrawOnCtrl(this, _presentTexture, Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, _presentTexture.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.get_Empty(), Color.get_Black() * 0.5f, 0f, default(Vector2), (SpriteEffects)0);
					spriteBatch.DrawOnCtrl(this, _presentTexture, Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, _presentTexture.Bounds, Color.get_White(), 0f, default(Vector2), (SpriteEffects)0);
				}
			}
			if (IsDraggingTarget || (_mainWindow != null && ((Rectangle)(ref bounds)).Contains(base.RelativeMousePosition) && _mainWindow.IsActive) || base.MouseOver)
			{
				Color color = ContentService.Colors.ColonialWhite;
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 2, bounds.Width, 2), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Bottom() - 1, bounds.Width, 1), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 2, ((Rectangle)(ref bounds)).get_Top(), 2, bounds.Height), Rectangle.get_Empty(), color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(((Rectangle)(ref bounds)).get_Right() - 1, ((Rectangle)(ref bounds)).get_Top(), 1, bounds.Height), Rectangle.get_Empty(), color * 0.6f);
			}
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
			if (!IsDraggingTarget)
			{
				if (!base.MouseOver && _textTooltip.Visible)
				{
					_textTooltip.Visible = base.MouseOver;
				}
				if (!base.MouseOver && _characterTooltip.Visible)
				{
					_characterTooltip.Visible = base.MouseOver;
				}
			}
			_infoLabels.Update();
			if (_updateCharacter && _created && base.Visible)
			{
				Settings_AppearanceSettingChanged(this, null);
				_updateCharacter = false;
			}
		}

		protected override void OnRightMouseButtonPressed(MouseEventArgs e)
		{
			base.OnRightMouseButtonPressed(e);
			if (!IsDraggingTarget)
			{
				_mainWindow.ShowAttached((_mainWindow.CharacterEdit.Character != Character || !_mainWindow.CharacterEdit.Visible) ? _mainWindow.CharacterEdit : null);
				_mainWindow.CharacterEdit.Character = Character;
			}
		}

		protected override async void OnClick(MouseEventArgs e)
		{
			if (IsDraggingTarget)
			{
				return;
			}
			base.OnClick(e);
			if (e.IsDoubleClick && _settings.DoubleClickToEnter.Value)
			{
				Character.Swap();
				return;
			}
			if (((Rectangle)(ref _loginRect)).Contains(base.RelativeMousePosition))
			{
				PlayerCharacter player = GameService.Gw2Mumble.PlayerCharacter;
				if (player != null && player.Name == Character.Name && Character.HasBirthdayPresent)
				{
					await _settings.MailKey.Value.PerformPress(50, triggerSystem: false);
					_mainWindow.CharacterEdit.Character = Character;
					_mainWindow.ShowAttached(_mainWindow.CharacterEdit);
				}
				else
				{
					Character.Swap();
					_mainWindow.ShowAttached();
				}
			}
			if (((Rectangle)(ref _cogRect)).Contains(base.RelativeMousePosition))
			{
				_mainWindow.CharacterEdit.Visible = !_mainWindow.CharacterEdit.Visible || _mainWindow.CharacterEdit.Character != Character;
				_mainWindow.CharacterEdit.Character = Character;
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			base.OnLeftMouseButtonPressed(e);
			if (!IsDraggingTarget)
			{
				KeyboardState state = Keyboard.GetState();
				if (((KeyboardState)(ref state)).IsKeyDown((Keys)162) && _settings.SortType.Value == Settings.SortBy.Custom)
				{
					_mainWindow.DraggingControl.StartDragging(this);
					_dragging = true;
					_characterTooltip?.Hide();
					_textTooltip?.Hide();
				}
			}
		}

		protected override void OnLeftMouseButtonReleased(MouseEventArgs e)
		{
			base.OnLeftMouseButtonReleased(e);
			if (!IsDraggingTarget && _dragging)
			{
				_mainWindow.DraggingControl.EndDragging();
				_dragging = false;
			}
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			base.OnMouseMoved(e);
			if (!IsDraggingTarget && !_mainWindow.DraggingControl.IsActive && (_textTooltip == null || (!_textTooltip.Visible && _settings.ShowDetailedTooltip.Value)))
			{
				_characterTooltip?.Show();
			}
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			base.OnMouseEntered(e);
			if (!IsDraggingTarget && !_mainWindow.DraggingControl.IsActive && (_textTooltip == null || (!_textTooltip.Visible && _settings.ShowDetailedTooltip.Value)))
			{
				_characterTooltip?.Show();
			}
		}

		protected override void OnHidden(EventArgs e)
		{
			base.OnHidden(e);
			_textTooltip?.Hide();
			_characterTooltip?.Hide();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			_textTooltip.Shown -= TextTooltip_Shown;
			if (_character != null)
			{
				_character.Deleted -= new EventHandler(CharacterDeleted);
			}
			_infoLabels?.Dispose();
			_contentPanel?.Dispose();
			_textTooltip?.Dispose();
			_characterTooltip?.Dispose();
			base.Children.DisposeAll();
			_mainWindow.CharacterCards.Remove(this);
		}

		private void TextTooltip_Shown(object sender, EventArgs e)
		{
			_characterTooltip?.Hide();
		}

		private void CharacterDeleted(object sender, EventArgs e)
		{
			Dispose();
		}

		public void HideTooltips()
		{
			_textTooltip.Hide();
			_characterTooltip.Hide();
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
			if (base.Width != _controlBounds.Width + base.AutoSizePadding.X)
			{
				base.Width = _controlBounds.Width + base.AutoSizePadding.X;
			}
			if (base.Height != _controlBounds.Height + base.AutoSizePadding.Y)
			{
				_iconDummy.Height = _controlBounds.Height;
			}
			int num;
			if (_contentPanel.Visible)
			{
				IEnumerable<Control> enumerable = _infoLabels.DataControls.Where((Control e) => e.Visible);
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
