using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using FontStashSharp;
using LoreBridge.Models;
using LoreBridge.Modules.Chat.Models;
using LoreBridge.Services;
using LoreBridge.Services.GameState;
using Microsoft.Xna.Framework;

namespace LoreBridge.Modules.Chat.Controls
{
	public sealed class TranslationWindow : ChatWindow
	{
		private readonly StandardButton _clearButton;

		private readonly TranslationScrollPanel _panel;

		private readonly Settings _settings;

		private bool _preventSaveVisible;

		public TranslationWindow(Settings settings, Messages messages, SpriteFontBase font)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Location(new Point(settings.WindowLocationX.get_Value(), settings.WindowLocationY.get_Value()));
			((Control)this).set_Height(settings.WindowHeight.get_Value());
			((Control)this).set_Width(settings.WindowWidth.get_Value());
			base.CanClose = true;
			base.CanCloseWithEscape = false;
			base.CanResize = !settings.WindowFixed.get_Value();
			base.Title = "Chat";
			base.Transparent = settings.WindowTransparent.get_Value();
			_settings = settings;
			TranslationScrollPanel translationScrollPanel = new TranslationScrollPanel(messages, font);
			((Control)translationScrollPanel).set_Parent((Container)(object)this);
			_panel = translationScrollPanel;
			_settings.ToggleTranslationWindowHotKey.get_Value().set_Enabled(true);
			_settings.ToggleTranslationWindowHotKey.get_Value().add_Activated((EventHandler<EventArgs>)OnToggleHotKey);
			_settings.WindowFixed.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnFixedChanged);
			_settings.WindowTransparent.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnTransparentChange);
			GameService.Gw2Mumble.get_UI().add_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			Service.GameState.GameStateChanged += OnGameStateChanged;
			StandardButton val = new StandardButton();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Text("Clear");
			((Control)val).set_Width(42);
			((Control)val).set_Height(20);
			((Control)val).set_Top(0);
			((Control)val).set_Right(settings.WindowWidth.get_Value());
			((Control)val).set_Visible(false);
			_clearButton = val;
			((Control)_clearButton).add_Click((EventHandler<MouseEventArgs>)delegate
			{
				messages.Clear();
			});
			if (settings.WindowVisible.get_Value())
			{
				((Control)this).Show();
			}
		}

		private void OnFixedChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			base.CanResize = !e.get_NewValue();
		}

		private void OnTransparentChange(object sender, ValueChangedEventArgs<bool> e)
		{
			base.Transparent = e.get_NewValue();
		}

		public void UpdateFont(SpriteFontBase font)
		{
			_panel.UpdateFont(font);
		}

		protected override void OnShown(EventArgs e)
		{
			if (_settings != null)
			{
				_settings.WindowVisible.set_Value(true);
			}
			((Control)this).OnShown(e);
		}

		protected override void OnHidden(EventArgs e)
		{
			if (_settings != null && !_preventSaveVisible)
			{
				_settings.WindowVisible.set_Value(false);
			}
			((Control)this).OnHidden(e);
		}

		protected override void OnMoved(MovedEventArgs e)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			if (_settings != null)
			{
				if (_settings.WindowFixed.get_Value())
				{
					((Control)this).set_Location(new Point(_settings.WindowLocationX.get_Value(), _settings.WindowLocationY.get_Value()));
					return;
				}
				_settings.WindowLocationX.set_Value(e.get_CurrentLocation().X);
				_settings.WindowLocationY.set_Value(e.get_CurrentLocation().Y);
			}
			((Control)this).OnMoved(e);
		}

		protected override void OnResized(ResizedEventArgs e)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (_clearButton != null)
			{
				((Control)_clearButton).set_Right(e.get_CurrentSize().X);
			}
			if (_settings != null)
			{
				_settings.WindowWidth.set_Value(e.get_CurrentSize().X);
				_settings.WindowHeight.set_Value(e.get_CurrentSize().Y);
			}
			base.OnResized(e);
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			if (_clearButton != null)
			{
				((Control)_clearButton).set_Visible(true);
			}
			if (_settings != null && _settings.WindowTransparent.get_Value())
			{
				base.Transparent = false;
			}
			((Control)this).OnMouseEntered(e);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			if (_clearButton != null)
			{
				((Control)_clearButton).set_Visible(false);
			}
			if (_settings != null && _settings.WindowTransparent.get_Value())
			{
				base.Transparent = true;
			}
			base.OnMouseLeft(e);
		}

		private void OnIsMapOpenChanged(object o, ValueEventArgs<bool> e)
		{
			if (e.get_Value())
			{
				_preventSaveVisible = true;
				((Control)this).Hide();
			}
			else if (_settings != null && _settings.WindowVisible.get_Value())
			{
				((Control)this).Show();
				_preventSaveVisible = false;
			}
		}

		private void OnGameStateChanged(object o, GameStateType e)
		{
			if (e == GameStateType.LoadingOrCharacterSelection)
			{
				_preventSaveVisible = true;
				((Control)this).Hide();
			}
			else if (_settings != null && _settings.WindowVisible.get_Value())
			{
				((Control)this).Show();
				_preventSaveVisible = false;
			}
		}

		private void OnToggleHotKey(object o, EventArgs e)
		{
			ToggleWindow();
		}

		protected override void DisposeControl()
		{
			_settings.ToggleTranslationWindowHotKey.get_Value().set_Enabled(false);
			_settings.ToggleTranslationWindowHotKey.get_Value().remove_Activated((EventHandler<EventArgs>)OnToggleHotKey);
			_settings.WindowFixed.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnFixedChanged);
			GameService.Gw2Mumble.get_UI().remove_IsMapOpenChanged((EventHandler<ValueEventArgs<bool>>)OnIsMapOpenChanged);
			base.DisposeControl();
		}
	}
}
