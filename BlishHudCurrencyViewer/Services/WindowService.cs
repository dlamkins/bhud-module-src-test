using System;
using System.Collections.Generic;
using System.Linq;
using BlishHudCurrencyViewer.Models;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;

namespace BlishHudCurrencyViewer.Services
{
	public class WindowService : IDisposable
	{
		private StandardWindow _window;

		private ContentsManager _contentsManager;

		private SettingsManager _settingsManager;

		private bool _isVisible;

		private List<UserCurrencyDisplayData> _displayData;

		private Label _descriptionText;

		public WindowService(ContentsManager contentsManager, SettingsManager settingsManager)
		{
			_contentsManager = contentsManager;
			_settingsManager = settingsManager;
		}

		public void Toggle()
		{
			_isVisible = !_isVisible;
		}

		private bool ShouldHideWindow()
		{
			if (GameService.GameIntegration.get_Gw2Instance().get_Gw2IsRunning() && GameService.GameIntegration.get_Gw2Instance().get_IsInGame() && GameService.Gw2Mumble.get_IsAvailable())
			{
				return GameService.Gw2Mumble.get_UI().get_IsMapOpen();
			}
			return true;
		}

		public void Update(GameTime gameTime)
		{
			InitializeIfNotExists();
			if (ShouldHideWindow())
			{
				((Control)_window).Hide();
			}
			else if (_isVisible)
			{
				((Control)_window).Show();
			}
			else
			{
				((Control)_window).Hide();
			}
		}

		public void InitializeIfNotExists()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			if (_window != null)
			{
				return;
			}
			StandardWindow val = new StandardWindow(_contentsManager.GetTexture("empty.png"), new Rectangle(0, 0, 300, 400), new Rectangle(10, 20, 280, 360));
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)val).set_BackgroundColor(new Color(0f, 0f, 0f, 0.6f));
			((WindowBase2)val).set_Title("");
			((WindowBase2)val).set_Emblem(_contentsManager.GetTexture("coins.png"));
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_CanCloseWithEscape(false);
			((WindowBase2)val).set_Id("CurrencyViewerModule_38d37290-b5f9-447d-97ea-45b0b50e5f56");
			((Control)val).set_Opacity(0.6f);
			StandardWindow currencyViewerWindow = (_window = val);
			((Control)_window).add_Hidden((EventHandler<EventArgs>)delegate
			{
				if (!ShouldHideWindow())
				{
					_isVisible = false;
				}
			});
		}

		public void RedrawWindowContent(List<UserCurrency> userCurrencies)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Expected O, but got Unknown
			ResetDisplayData();
			((Container)_window).set_HeightSizingMode((SizingMode)1);
			((Container)_window).set_WidthSizingMode((SizingMode)1);
			List<SettingEntry> selectedCurrencySettings = ((IEnumerable<SettingEntry>)_settingsManager.get_ModuleSettings()).Where((SettingEntry s) => s.get_EntryKey().StartsWith("currency-setting-") && ((s as SettingEntry<bool>)?.get_Value() ?? false)).ToList();
			if (userCurrencies == null || userCurrencies.Count() == 0 || selectedCurrencySettings.Count() == 0)
			{
				Label val = new Label();
				val.set_Text("You have not yet selected any currencies to track! Go to BlishHud's CurrencyViewer module settings to select some.");
				((Control)val).set_Parent((Container)(object)_window);
				((Control)val).set_Width(200);
				((Control)val).set_Height(120);
				val.set_WrapText(true);
				val.set_VerticalAlignment((VerticalAlignment)0);
				_descriptionText = val;
				return;
			}
			for (int i = 0; i < selectedCurrencySettings.Count(); i++)
			{
				SettingEntry currency = selectedCurrencySettings[i];
				UserCurrency userCurrency = userCurrencies.Find((UserCurrency c) => "currency-setting-" + c.CurrencyId == currency.get_EntryKey());
				if (userCurrency == null)
				{
					userCurrency = new UserCurrency
					{
						CurrencyName = currency.get_DisplayName(),
						CurrencyQuantity = 0
					};
				}
				string truncatedName = ((currency.get_DisplayName().Count() > 15) ? (currency.get_DisplayName().Substring(0, 15) + "...") : currency.get_DisplayName());
				Label val2 = new Label();
				val2.set_Text(truncatedName);
				((Control)val2).set_Parent((Container)(object)_window);
				((Control)val2).set_Top(i * 20);
				((Control)val2).set_Left(0);
				((Control)val2).set_Width(120);
				((Control)val2).set_BasicTooltipText(currency.get_DisplayName());
				Label nameLabel = val2;
				Label val3 = new Label();
				val3.set_Text(userCurrency.CurrencyQuantity.ToString("N0"));
				((Control)val3).set_Parent((Container)(object)_window);
				((Control)val3).set_Top(i * 20);
				((Control)val3).set_Left(130);
				val3.set_AutoSizeWidth(true);
				Label quantityLabel = val3;
				_displayData.Add(new UserCurrencyDisplayData
				{
					CurrencyDisplayName = userCurrency.CurrencyName,
					Name = nameLabel,
					Quantity = quantityLabel
				});
			}
		}

		private void ResetDisplayData()
		{
			if (_descriptionText != null)
			{
				((Control)_descriptionText).Dispose();
				_descriptionText = null;
			}
			if (_displayData == null)
			{
				_displayData = new List<UserCurrencyDisplayData>();
			}
			_displayData.ForEach(delegate(UserCurrencyDisplayData d)
			{
				((Control)d.Name).Dispose();
				((Control)d.Quantity).Dispose();
			});
			_displayData.Clear();
		}

		public void Dispose()
		{
			ResetDisplayData();
			((Control)_window).Dispose();
		}
	}
}
