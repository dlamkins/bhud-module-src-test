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
		private readonly Panel _headerPanel;

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
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			base.BorderWidth = new RectangleDimensions(2);
			base.BorderColor = Color.get_Black();
			base.BackgroundColor = Color.get_Black() * 0.4f;
			base.BackgroundImage = AsyncTexture2D.FromAssetId(156003);
			((Control)this).set_ZIndex(11);
			Panel panel = new Panel();
			((Control)panel).set_Parent((Container)(object)this);
			panel.BackgroundColor = Color.get_Black() * 0.95f;
			((Control)panel).set_Height(25);
			_headerPanel = panel;
			((Control)TabsButtonPanel).set_Location(new Point(0, ((Control)_headerPanel).get_Bottom()));
			CreateHeaderButtons();
			((Control)this).set_Width(250);
			((Container)this).set_HeightSizingMode((SizingMode)1);
			if (base.BackgroundImage != null)
			{
				base.TextureRectangle = new Rectangle(30, 30, base.BackgroundImage.get_Width() - 60, base.BackgroundImage.get_Height() - 60);
			}
		}

		private void CloseButton_Click(object sender, MouseEventArgs e)
		{
			((Control)this).Hide();
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
			ImageButton imageButton = new ImageButton();
			((Control)imageButton).set_Parent((Container)(object)_headerPanel);
			imageButton.Texture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Camera));
			imageButton.HoveredTexture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Camera_Hovered));
			((Control)imageButton).set_Size(new Point(20, 20));
			imageButton.ClickAction = delegate
			{
				_toggleOCR?.Invoke();
			};
			imageButton.SetLocalizedTooltip = () => strings.EditOCR_Tooltip;
			_ocrButton = imageButton;
			_buttons.Add((Control)(object)_ocrButton);
			ImageButton imageButton2 = new ImageButton();
			((Control)imageButton2).set_Parent((Container)(object)_headerPanel);
			imageButton2.Texture = AsyncTexture2D.FromAssetId(358353);
			imageButton2.HoveredTexture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Portrait_Hovered));
			((Control)imageButton2).set_Size(new Point(20, 20));
			imageButton2.ColorHovered = Color.get_White();
			imageButton2.SetLocalizedTooltip = () => strings.TogglePortraitCapture_Tooltip;
			imageButton2.ClickAction = delegate
			{
				_togglePotrait?.Invoke();
			};
			_potraitButton = imageButton2;
			_buttons.Add((Control)(object)_potraitButton);
			ImageButton imageButton3 = new ImageButton();
			((Control)imageButton3).set_Parent((Container)(object)_headerPanel);
			imageButton3.Texture = AsyncTexture2D.FromAssetId(156760);
			imageButton3.HoveredTexture = AsyncTexture2D.FromAssetId(156759);
			((Control)imageButton3).set_Size(new Point(20, 20));
			imageButton3.SetLocalizedTooltip = () => strings.FixCharacters_Tooltip;
			imageButton3.ClickAction = delegate
			{
				if (!GameService.GameIntegration.get_Gw2Instance().get_IsInGame())
				{
					_characterSorting.Start();
				}
			};
			_fixButton = imageButton3;
			_buttons.Add((Control)(object)_fixButton);
			ImageButton imageButton4 = new ImageButton();
			((Control)imageButton4).set_Parent((Container)(object)_headerPanel);
			imageButton4.Texture = AsyncTexture2D.FromAssetId(156749);
			imageButton4.HoveredTexture = AsyncTexture2D.FromAssetId(156750);
			((Control)imageButton4).set_Size(new Point(20, 20));
			imageButton4.ClickAction = delegate
			{
				_refreshAPI?.Invoke();
			};
			imageButton4.SetLocalizedTooltip = () => strings.RefreshAPI;
			_refreshButton = imageButton4;
			_buttons.Add((Control)(object)_refreshButton);
			ImageToggleButton imageToggleButton = new ImageToggleButton(delegate(bool b)
			{
				_settings.PinSideMenus.set_Value(b);
			});
			((Control)imageToggleButton).set_Parent((Container)(object)_headerPanel);
			imageToggleButton.Texture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Pin));
			imageToggleButton.HoveredTexture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Pin_Hovered));
			imageToggleButton.ActiveTexture = AsyncTexture2D.op_Implicit(_textureManager.GetIcon(TextureManager.Icons.Pin_Hovered));
			imageToggleButton.ColorDefault = new Color(175, 175, 175);
			imageToggleButton.ColorActive = Colors.ColonialWhite;
			((Control)imageToggleButton).set_Size(new Point(20, 20));
			imageToggleButton.Active = _settings.PinSideMenus.get_Value();
			imageToggleButton.SetLocalizedTooltip = () => strings.PinSideMenus_Tooltip;
			_pinButton = imageToggleButton;
			_buttons.Add((Control)(object)_pinButton);
			ImageButton imageButton5 = new ImageButton();
			((Control)imageButton5).set_Parent((Container)(object)_headerPanel);
			imageButton5.Texture = AsyncTexture2D.FromAssetId(156012);
			imageButton5.HoveredTexture = AsyncTexture2D.FromAssetId(156011);
			((Control)imageButton5).set_Size(new Point(20, 20));
			imageButton5.TextureRectangle = new Rectangle(7, 7, 20, 20);
			imageButton5.ClickAction = delegate
			{
				((Control)this).Hide();
			};
			imageButton5.SetLocalizedTooltip = () => strings.Close;
			_closeButton = imageButton5;
			_buttons.Add((Control)(object)_closeButton);
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
				((Control)_headerPanel).set_Width(((Container)this).get_ContentRegion().Width);
			}
		}

		public override bool SwitchTab(PanelTab tab = null)
		{
			bool result = base.SwitchTab(tab);
			foreach (PanelTab t in base.Tabs)
			{
				if (t != tab)
				{
					((Control)t).set_Height(5);
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
				base.TextureRectangle = new Rectangle(30, 30, Math.Min(base.BackgroundImage.get_Width() - 100, ((Control)this).get_Width()), Math.Min(base.BackgroundImage.get_Height() - 100, ((Control)this).get_Height()));
			}
			int gap = (((Control)_headerPanel).get_Width() - 7 - _buttons.Count * 20) / (_buttons.Count - 1);
			for (int i = 0; i < _buttons.Count; i++)
			{
				Control b = _buttons[i];
				b.set_Location(new Point(6 + i * gap + i * b.get_Width(), 3));
			}
		}
	}
}
