using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Kenedia.Modules.Characters.Res;
using Kenedia.Modules.Characters.Services;
using Kenedia.Modules.Core.Controls;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Structs;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Characters.Controls.SideMenu
{
	public class SideMenu : TabbedPanel, ILocalizable
	{
		private readonly Kenedia.Modules.Core.Controls.Panel _headerPanel;

		private readonly List<Control> _buttons = new List<Control>();

		private ImageButton _closeButton;

		private ImageToggleButton _pinButton;

		private ImageButton _ocrButton;

		private ImageButton _potraitButton;

		private ImageButton _fixButton;

		private ImageButton _refreshButton;

		private readonly Settings _settings;

		private readonly TextureManager _textureManager;

		private readonly CharacterSorting _characterSorting;

		private readonly Action _toggleOCR;

		private readonly Action _togglePotrait;

		private readonly Action _refreshAPI;

		public SideMenuToggles TogglesTab { get; set; }

		public SideMenu(Action toggleOCR, Action togglePotrait, Action refreshAPI, TextureManager textureManager, Settings settings, CharacterSorting characterSorting)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			_textureManager = textureManager;
			_toggleOCR = toggleOCR;
			_togglePotrait = togglePotrait;
			_refreshAPI = refreshAPI;
			_settings = settings;
			_characterSorting = characterSorting;
			base.Parent = GameService.Graphics.SpriteScreen;
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.get_Black();
			base.BackgroundColor = Color.get_Black() * 0.4f;
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			ZIndex = 11;
			_headerPanel = new Kenedia.Modules.Core.Controls.Panel
			{
				Parent = this,
				BackgroundColor = Color.get_Black() * 0.95f,
				Height = 25
			};
			TabsButtonPanel.Location = new Point(0, _headerPanel.Bottom);
			CreateHeaderButtons();
			base.Width = 250;
			HeightSizingMode = SizingMode.AutoSize;
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(30, 30, base.BackgroundImage.Width - 60, base.BackgroundImage.Height - 60);
			}
		}

		private void CloseButton_Click(object sender, MouseEventArgs e)
		{
			Hide();
		}

		private void CreateHeaderButtons()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			_ocrButton = new ImageButton
			{
				Parent = _headerPanel,
				Texture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Camera),
				HoveredTexture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Camera_Hovered),
				Size = new Point(20, 20),
				ClickAction = delegate
				{
					_toggleOCR?.Invoke();
				},
				SetLocalizedTooltip = () => strings.EditOCR_Tooltip
			};
			_buttons.Add(_ocrButton);
			_potraitButton = new ImageButton
			{
				Parent = _headerPanel,
				Texture = AsyncTexture2D.FromAssetId(358353),
				HoveredTexture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Portrait_Hovered),
				Size = new Point(20, 20),
				ColorHovered = Color.get_White(),
				SetLocalizedTooltip = () => strings.TogglePortraitCapture_Tooltip,
				ClickAction = delegate
				{
					_togglePotrait?.Invoke();
				}
			};
			_buttons.Add(_potraitButton);
			_fixButton = new ImageButton
			{
				Parent = _headerPanel,
				Texture = AsyncTexture2D.FromAssetId(156760),
				HoveredTexture = AsyncTexture2D.FromAssetId(156759),
				Size = new Point(20, 20),
				SetLocalizedTooltip = () => strings.FixCharacters_Tooltip,
				ClickAction = delegate
				{
					if (!GameService.GameIntegration.Gw2Instance.IsInGame)
					{
						_characterSorting.Start();
					}
				}
			};
			_buttons.Add(_fixButton);
			_refreshButton = new ImageButton
			{
				Parent = _headerPanel,
				Texture = AsyncTexture2D.FromAssetId(156749),
				HoveredTexture = AsyncTexture2D.FromAssetId(156750),
				Size = new Point(20, 20),
				ClickAction = delegate
				{
					_refreshAPI?.Invoke();
				},
				SetLocalizedTooltip = () => strings.RefreshAPI
			};
			_buttons.Add(_refreshButton);
			_pinButton = new ImageToggleButton(delegate(bool b)
			{
				_settings.PinSideMenus.Value = b;
			})
			{
				Parent = _headerPanel,
				Texture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Pin),
				HoveredTexture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Pin_Hovered),
				ActiveTexture = (AsyncTexture2D)_textureManager.GetIcon(TextureManager.Icons.Pin_Hovered),
				ColorDefault = new Color(175, 175, 175),
				ColorActive = ContentService.Colors.ColonialWhite,
				Size = new Point(20, 20),
				Active = _settings.PinSideMenus.Value,
				SetLocalizedTooltip = () => strings.PinSideMenus_Tooltip
			};
			_buttons.Add(_pinButton);
			_closeButton = new ImageButton
			{
				Parent = _headerPanel,
				Texture = AsyncTexture2D.FromAssetId(156012),
				HoveredTexture = AsyncTexture2D.FromAssetId(156011),
				Size = new Point(20, 20),
				TextureRectangle = new Rectangle(7, 7, 20, 20),
				ClickAction = delegate
				{
					Hide();
				},
				SetLocalizedTooltip = () => strings.Close
			};
			_buttons.Add(_closeButton);
		}

		public void ResetToggles()
		{
			TogglesTab?.ResetToggles();
		}

		public override void UpdateContainer(GameTime gameTime)
		{
			base.UpdateContainer(gameTime);
		}

		public override void RecalculateLayout()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			if (_headerPanel != null)
			{
				_headerPanel.Width = base.ContentRegion.Width;
			}
		}

		public override bool SwitchTab(PanelTab tab = null)
		{
			bool result = base.SwitchTab(tab);
			foreach (PanelTab t in base.Tabs)
			{
				if (t != tab)
				{
					t.Height = 5;
				}
			}
			return result;
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			base.OnResized(e);
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(30, 30, Math.Min(base.BackgroundImage.Width - 100, base.Width), Math.Min(base.BackgroundImage.Height - 100, base.Height));
			}
			int gap = (_headerPanel.Width - 7 - _buttons.Count * 20) / (_buttons.Count - 1);
			for (int i = 0; i < _buttons.Count; i++)
			{
				Control b = _buttons[i];
				b.Location = new Point(6 + i * gap + i * b.Width, 3);
			}
		}
	}
}
