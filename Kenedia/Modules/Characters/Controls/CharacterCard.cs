using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		private Rectangle _controlBounds = Rectangle.Empty;

		private Rectangle _textBounds;

		private Rectangle _iconRectangle;

		private readonly bool _created;

		private bool _dragging;

		private int _cogSize;

		private int _iconSize;

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
			[CompilerGenerated]
			get
			{
				return _003CCharacter_003Ek__BackingField;
			}
			set
			{
				Character_Model temp = _003CCharacter_003Ek__BackingField;
				if (Common.SetProperty(ref _003CCharacter_003Ek__BackingField, value))
				{
					if (temp != null)
					{
						temp.Deleted -= new EventHandler(CharacterDeleted);
						temp.Updated -= new EventHandler(ApplyCharacter);
					}
					if (_003CCharacter_003Ek__BackingField != null)
					{
						_003CCharacter_003Ek__BackingField.Deleted += new EventHandler(CharacterDeleted);
						_003CCharacter_003Ek__BackingField.Updated += new EventHandler(ApplyCharacter);
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
				return _controlBounds;
			}
			set
			{
				_controlBounds = value;
				_ = _controlBounds;
				AdaptNewBounds();
			}
		}

		public CharacterCard()
		{
			HeightSizingMode = SizingMode.AutoSize;
			base.BackgroundColor = Color.Black * 0.5f;
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
				Size = Point.Zero
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
			_textureManager = card._textureManager;
			_data = card._data;
			_mainWindow = card._mainWindow;
			_settings = card._settings;
			base.Size = card.Size;
			Character = card.Character;
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
			_infoLabels.UpdateDataControlsVisibility();
			_infoLabels.UpdateCharacterInfo();
			CalculateLayout();
			AdaptNewBounds();
		}

		public CharacterCard(Func<Character_Model> currentCharacter, TextureManager textureManager, Data data, MainWindow mainWindow, Settings settings)
			: this()
		{
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
					e.ControlContentBounds = new Rectangle(e.ControlContentBounds.Location, new Point(maxWidth, e.ControlContentBounds.Height));
				});
				AttachedCards.ForEach(delegate(CharacterCard e)
				{
					e._lastUniform = now;
				});
				ControlContentBounds = new Rectangle(ControlContentBounds.Location, new Point(maxWidth, ControlContentBounds.Height));
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
				_cogSize = Math.Max(20, ((firstControl != null) ? ((IFontControl)firstControl).Font.LineHeight : Font.LineHeight) - 4);
				_cogSize = ((!anyVisible) ? (_iconSize / 5) : _cogSize);
				if (firstControl != null && width < firstControl.Width + 5 + _cogSize)
				{
					width += (anyVisible ? (5 + _cogSize) : 0);
				}
				_textBounds = new Rectangle(_iconRectangle.Right + ((anyVisible && _iconSize > 0) ? 5 : 0), 0, width, height);
				_contentPanel.Location = _textBounds.Location;
				_contentPanel.Size = _textBounds.Size;
				_controlBounds = new Rectangle(_iconRectangle.Left, _iconRectangle.Top, _textBounds.Right - _iconRectangle.Left, Math.Max(_textBounds.Height, _iconRectangle.Height));
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
			base.PaintBeforeChildren(spriteBatch, bounds);
			spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.Empty, Color.Transparent, 0f, default(Vector2));
			if (Character == null)
			{
				return;
			}
			if (_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText)
			{
				if (!Character.HasDefaultIcon && Character.Icon != null)
				{
					spriteBatch.DrawOnCtrl(this, Character.Icon, _iconRectangle, Character.Icon.Bounds, Color.White, 0f, default(Vector2));
					return;
				}
				AsyncTexture2D texture = Character.SpecializationIcon;
				if (texture != null)
				{
					spriteBatch.DrawOnCtrl(this, _iconFrame, new Rectangle(_iconRectangle.X, _iconRectangle.Y, _iconRectangle.Width, _iconRectangle.Height), _iconFrame.Bounds, Color.White, 0f, default(Vector2));
					spriteBatch.DrawOnCtrl(this, _iconFrame, new Rectangle(_iconRectangle.Width, _iconRectangle.Height, _iconRectangle.Width, _iconRectangle.Height), _iconFrame.Bounds, Color.White, 3.14f, default(Vector2));
					spriteBatch.DrawOnCtrl(this, texture, new Rectangle(8, 8, _iconRectangle.Width - 16, _iconRectangle.Height - 16), texture.Bounds, Color.White, 0f, default(Vector2));
				}
			}
			else if (base.MouseOver)
			{
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.Empty, Color.Transparent, 0f, default(Vector2));
			}
		}

		public override void PaintAfterChildren(SpriteBatch spriteBatch, Rectangle bounds)
		{
			base.PaintAfterChildren(spriteBatch, bounds);
			if (base.MouseOver)
			{
				_textTooltip.Visible = false;
				bool loginHovered = !IsDraggingTarget && _loginRect.Contains(base.RelativeMousePosition);
				if (_settings.PanelLayout.Value != Settings.CharacterPanelLayout.OnlyText)
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.Empty, IsDraggingTarget ? Color.Transparent : (Color.Black * 0.5f), 0f, default(Vector2));
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
						spriteBatch.DrawOnCtrl(this, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture), Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered.Bounds : _loginTexture.Bounds) : (loginHovered ? _presentTextureOpen.Bounds : _presentTexture.Bounds), loginHovered ? Color.White : new Color(215, 215, 215), 0f, default(Vector2));
					}
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.Empty, Color.Black * 0.5f, 0f, default(Vector2));
					_textTooltip.Text = (Character.HasBirthdayPresent ? string.Format(strings.Birthday_Text, Character.Name, Character.Age) : string.Empty);
					_textTooltip.Visible = !string.IsNullOrEmpty(_textTooltip.Text);
					spriteBatch.DrawOnCtrl(this, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered : _loginTexture) : (loginHovered ? _presentTextureOpen : _presentTexture), Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, (!Character.HasBirthdayPresent) ? (loginHovered ? _loginTextureHovered.Bounds : _loginTexture.Bounds) : (loginHovered ? _presentTextureOpen.Bounds : _presentTexture.Bounds), loginHovered ? Color.White : new Color(200, 200, 200), 0f, default(Vector2));
				}
				if (!IsDraggingTarget)
				{
					spriteBatch.DrawOnCtrl(this, _cogRect.Contains(base.RelativeMousePosition) ? _cogTextureHovered : _cogTexture, _cogRect, new Rectangle(5, 5, 22, 22), Color.White, 0f, default(Vector2));
					if (_cogRect.Contains(base.RelativeMousePosition))
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
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, _iconRectangle, Rectangle.Empty, Color.Black * 0.5f, 0f, default(Vector2));
					spriteBatch.DrawOnCtrl(this, _presentTexture, Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, _presentTexture.Bounds, Color.White, 0f, default(Vector2));
				}
				else
				{
					spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, bounds, Rectangle.Empty, Color.Black * 0.5f, 0f, default(Vector2));
					spriteBatch.DrawOnCtrl(this, _presentTexture, Character.HasBirthdayPresent ? _loginRect.Add(8, 8, -16, -16) : _loginRect, _presentTexture.Bounds, Color.White, 0f, default(Vector2));
				}
			}
			if (IsDraggingTarget || (_mainWindow != null && bounds.Contains(base.RelativeMousePosition) && _mainWindow.IsActive) || base.MouseOver)
			{
				Color color = ContentService.Colors.ColonialWhite;
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 2), Rectangle.Empty, color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, bounds.Width, 1), Rectangle.Empty, color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 2, bounds.Width, 2), Rectangle.Empty, color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Bottom - 1, bounds.Width, 1), Rectangle.Empty, color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, 2, bounds.Height), Rectangle.Empty, color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Left, bounds.Top, 1, bounds.Height), Rectangle.Empty, color * 0.6f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 2, bounds.Top, 2, bounds.Height), Rectangle.Empty, color * 0.5f);
				spriteBatch.DrawOnCtrl(this, ContentService.Textures.Pixel, new Rectangle(bounds.Right - 1, bounds.Top, 1, bounds.Height), Rectangle.Empty, color * 0.6f);
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
			if (_loginRect.Contains(base.RelativeMousePosition))
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
			if (_cogRect.Contains(base.RelativeMousePosition))
			{
				_mainWindow.CharacterEdit.Visible = !_mainWindow.CharacterEdit.Visible || _mainWindow.CharacterEdit.Character != Character;
				_mainWindow.CharacterEdit.Character = Character;
			}
		}

		protected override void OnLeftMouseButtonPressed(MouseEventArgs e)
		{
			base.OnLeftMouseButtonPressed(e);
			if (!IsDraggingTarget && Keyboard.GetState().IsKeyDown(Keys.LeftControl) && _settings.SortType.Value == Settings.SortBy.Custom)
			{
				_mainWindow.DraggingControl.StartDragging(this);
				_dragging = true;
				_characterTooltip?.Hide();
				_textTooltip?.Hide();
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
			if (Character != null)
			{
				Character.Deleted -= new EventHandler(CharacterDeleted);
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
