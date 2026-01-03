using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blish_HUD;
using Blish_HUD.Controls;
using FontStashSharp;
using LoreBridge.Models;
using LoreBridge.Modules.Area.Controls;
using LoreBridge.Modules.Area.Forms;
using LoreBridge.Resources;
using LoreBridge.Services;
using LoreBridge.Utils;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Area
{
	public class Area : Module
	{
		private Settings _settings;

		private DynamicSpriteFont _font;

		private OverlayForm _overlay;

		private TranslationWindow _translationWindow;

		private bool _isOverlayActive;

		public override void Load(Settings settings)
		{
			_settings = settings;
			_font = Fonts.FontSystem.GetFont(_settings.AreaFontSize.get_Value());
			_translationWindow = new TranslationWindow(_font);
			_settings.ToggleCapturerHotkey.get_Value().set_Enabled(true);
			_settings.ToggleCapturerHotkey.get_Value().add_Activated((EventHandler<EventArgs>)CaptureScreen);
			GameService.GameIntegration.get_Gw2Instance().add_Gw2LostFocus((EventHandler<EventArgs>)LostFocus);
			_settings.AreaFontSize.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnFontSizeChanged);
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Unload()
		{
			_settings.ToggleCapturerHotkey.get_Value().set_Enabled(false);
			_settings.ToggleCapturerHotkey.get_Value().remove_Activated((EventHandler<EventArgs>)CaptureScreen);
			GameService.GameIntegration.get_Gw2Instance().remove_Gw2LostFocus((EventHandler<EventArgs>)LostFocus);
			_settings.AreaFontSize.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)OnFontSizeChanged);
			((Control)_translationWindow).Dispose();
		}

		private void OnFontSizeChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_font = Fonts.FontSystem.GetFont(e.get_NewValue());
			_translationWindow.UpdateFont(_font);
		}

		private void CaptureScreen(object sender, EventArgs e)
		{
			if (!_isOverlayActive)
			{
				((Control)_translationWindow).Hide();
				ShowOverlay();
			}
		}

		private void LostFocus(object o, EventArgs e)
		{
			if (_isOverlayActive)
			{
				HideOverlay();
			}
		}

		private void OnAreaSelected(object o, Rectangle e)
		{
			ProcessTranslationAsync(e);
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			HideOverlay();
		}

		private void OnHide(object sender, bool e)
		{
			HideOverlay();
		}

		private void ShowOverlay()
		{
			_overlay = new OverlayForm();
			_overlay.AreaSelected += OnAreaSelected;
			_overlay.FormClosed += OnFormClosed;
			_overlay.Hidden += OnHide;
			_overlay.Show();
			_isOverlayActive = true;
		}

		private void HideOverlay()
		{
			_overlay.AreaSelected -= OnAreaSelected;
			_overlay.FormClosed -= OnFormClosed;
			_overlay.Hidden -= OnHide;
			_overlay.Dispose();
			_isOverlayActive = false;
		}

		private async Task ProcessTranslationAsync(Rectangle rectangle)
		{
			string[] result = Array.Empty<string>();
			Bitmap bitmap = global::LoreBridge.Utils.Screen.GetScreen(rectangle);
			try
			{
				result = Service.Ocr.GetTextLines(bitmap);
			}
			catch (Exception)
			{
			}
			if (result.Length != 0)
			{
				string text = "";
				for (int i = 0; i < result.Length; i++)
				{
					string row = result[i];
					text = ((!row.EndsWith(".") || i == result.Length - 1) ? (text + row) : (text + row + "\n"));
				}
				float factor = GameService.Graphics.get_UIScaleMultiplier();
				((Control)_translationWindow).set_Top((int)((float)rectangle.Top / factor));
				((Control)_translationWindow).set_Left((int)((float)rectangle.Left / factor));
				((Control)_translationWindow).set_Size(new Point((int)((float)rectangle.Width / factor), (int)((float)rectangle.Height / factor)));
				_translationWindow.Loading = true;
				((Control)_translationWindow).Show();
				string translation = "";
				try
				{
					translation = await Service.Translation.TranslateAsync(text);
				}
				catch (Exception)
				{
					_translationWindow.Loading = false;
					((Control)_translationWindow).Hide();
				}
				if (string.IsNullOrEmpty(translation))
				{
					_translationWindow.Loading = false;
					((Control)_translationWindow).Hide();
				}
				else
				{
					_translationWindow.Loading = false;
					_translationWindow.Text = translation;
				}
			}
		}
	}
}
