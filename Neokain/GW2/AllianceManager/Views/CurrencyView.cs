using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;

namespace Neokain.GW2.AllianceManager.Views
{
	internal class CurrencyView : View
	{
		private static readonly List<int> DefaultVisibleCurrencyIds = new List<int> { 1, 4, 2, 3, 18, 23, 32 };

		private readonly Module _module;

		private readonly FlowPanel _panel;

		private readonly Checkbox _checkboxEditMode;

		private readonly SettingEntry<List<int>> _visibleCurrenciesSetting;

		private readonly Dictionary<int, AccountCurrencyControl> _allCurrencyControls = new Dictionary<int, AccountCurrencyControl>();

		private bool _controlsBuilt;

		private Label _loadingLabel;

		private Label _errorLabel;

		private Container _buildPanel;

		public CurrencyView(Module module)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			_module = module;
			FlowPanel val = new FlowPanel();
			((Panel)val).set_CanScroll(true);
			_panel = val;
			Checkbox val2 = new Checkbox();
			val2.set_Checked(false);
			_checkboxEditMode = val2;
			_visibleCurrenciesSetting = _module.SettingsManager.get_ModuleSettings().DefineSetting<List<int>>("visibleCurrencies", (List<int>)null, (Func<string>)null, (Func<string>)null);
			if (_visibleCurrenciesSetting.get_Value() == null)
			{
				InitializeDefaultVisibleCurrencies();
			}
			_module.WalletUpdated += OnWalletUpdated;
			_visibleCurrenciesSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)OnVisibleCurrenciesSettingChanged);
		}

		private void InitializeDefaultVisibleCurrencies()
		{
			_visibleCurrenciesSetting.set_Value(new List<int>(DefaultVisibleCurrencyIds));
		}

		private void OnVisibleCurrenciesSettingChanged(object sender, ValueChangedEventArgs<List<int>> e)
		{
			UpdateControlsVisibility();
		}

		private void OnWalletUpdated(object sender, EventArgs e)
		{
			UpdateWalletControlAmounts();
		}

		private void UpdateWalletControlAmounts()
		{
			if (_module.Wallet == null || _module.Currencies == null)
			{
				return;
			}
			foreach (Currency currency in _module.Currencies)
			{
				AccountCurrency obj = _module.Wallet.FirstOrDefault((AccountCurrency c) => c.get_Id() == currency.get_Id());
				int amount = ((obj != null) ? obj.get_Value() : 0);
				if (_allCurrencyControls.TryGetValue(currency.get_Id(), out var control))
				{
					control.UpdateAmount(amount);
				}
			}
		}

		private void BuildAllCurrencyControls()
		{
			if (_controlsBuilt || _module.Currencies == null)
			{
				return;
			}
			foreach (Currency currency in _module.Currencies)
			{
				IReadOnlyList<AccountCurrency> wallet = _module.Wallet;
				int? obj;
				if (wallet == null)
				{
					obj = null;
				}
				else
				{
					AccountCurrency obj2 = wallet.FirstOrDefault((AccountCurrency c) => c.get_Id() == currency.get_Id());
					obj = ((obj2 != null) ? new int?(obj2.get_Value()) : null);
				}
				int? num = obj;
				AccountCurrencyControl accountCurrencyControl = new AccountCurrencyControl(num.GetValueOrDefault(), currency, _module);
				((Control)accountCurrencyControl).set_Parent((Container)(object)_panel);
				((Control)accountCurrencyControl).set_Height(32);
				((Container)accountCurrencyControl).set_WidthSizingMode((SizingMode)2);
				accountCurrencyControl.EditMode = _checkboxEditMode.get_Checked();
				((Control)accountCurrencyControl).set_Visible(false);
				AccountCurrencyControl currencyControl = accountCurrencyControl;
				currencyControl.VisibilityChanged += OnCurrencyControlVisibilityChanged;
				_allCurrencyControls.Add(currency.get_Id(), currencyControl);
			}
			_controlsBuilt = true;
			UpdateControlsVisibility();
		}

		private void UpdateControlsVisibility()
		{
			if (!_controlsBuilt || _visibleCurrenciesSetting.get_Value() == null)
			{
				return;
			}
			HashSet<int> visibleCurrencyIds = new HashSet<int>(_visibleCurrenciesSetting.get_Value());
			foreach (KeyValuePair<int, AccountCurrencyControl> kvp in _allCurrencyControls)
			{
				int currencyId = kvp.Key;
				AccountCurrencyControl control = kvp.Value;
				bool shouldBeVisible = visibleCurrencyIds.Contains(currencyId);
				if (_checkboxEditMode.get_Checked())
				{
					((Control)control).set_Visible(true);
					control.SetCheckedState(shouldBeVisible);
				}
				else
				{
					((Control)control).set_Visible(shouldBeVisible);
				}
			}
			((Control)_panel).Invalidate();
		}

		private void OnCurrencyControlVisibilityChanged(int currencyId, bool isVisible)
		{
			List<int> visibleCurrencies = _visibleCurrenciesSetting.get_Value() ?? new List<int>();
			if (isVisible && !visibleCurrencies.Contains(currencyId))
			{
				visibleCurrencies.Add(currencyId);
				_visibleCurrenciesSetting.set_Value(new List<int>(visibleCurrencies));
			}
			else if (!isVisible && visibleCurrencies.Contains(currencyId))
			{
				visibleCurrencies.Remove(currencyId);
				_visibleCurrenciesSetting.set_Value(new List<int>(visibleCurrencies));
			}
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			return await ((View<IPresenter>)this).Load(progress);
		}

		protected override void Unload()
		{
			_module.WalletUpdated -= OnWalletUpdated;
			_visibleCurrenciesSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<List<int>>>)OnVisibleCurrenciesSettingChanged);
			foreach (AccountCurrencyControl value in _allCurrencyControls.Values)
			{
				value.VisibilityChanged -= OnCurrencyControlVisibilityChanged;
			}
			((View<IPresenter>)this).Unload();
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			_buildPanel = buildPanel;
			Label val = new Label();
			((Control)val).set_Parent(buildPanel);
			val.set_Text("Loading currencies...");
			val.set_Font(GameService.Content.get_DefaultFont18());
			val.set_TextColor(Color.get_White());
			val.set_HorizontalAlignment((HorizontalAlignment)1);
			val.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val).set_Width(((Control)buildPanel).get_Width());
			((Control)val).set_Height(50);
			((Control)val).set_Top(((Control)buildPanel).get_Height() / 2 - 25);
			((Control)val).set_Visible(false);
			_loadingLabel = val;
			Label val2 = new Label();
			((Control)val2).set_Parent(buildPanel);
			val2.set_Text("");
			val2.set_Font(GameService.Content.get_DefaultFont16());
			val2.set_TextColor(Color.get_Red());
			val2.set_HorizontalAlignment((HorizontalAlignment)1);
			val2.set_VerticalAlignment((VerticalAlignment)1);
			((Control)val2).set_Width(((Control)buildPanel).get_Width());
			((Control)val2).set_Height(50);
			((Control)val2).set_Top(((Control)buildPanel).get_Height() / 2 - 25);
			((Control)val2).set_Visible(false);
			_errorLabel = val2;
			((Control)_panel).set_Parent(buildPanel);
			((Container)_panel).set_WidthSizingMode((SizingMode)2);
			((Container)_panel).set_HeightSizingMode((SizingMode)2);
			((Control)_checkboxEditMode).set_Parent((Container)(object)_panel);
			((Control)_checkboxEditMode).set_Width(50);
			((Control)_checkboxEditMode).set_Height(24);
			_checkboxEditMode.set_Text("Edit Mode");
			_checkboxEditMode.add_CheckedChanged((EventHandler<CheckChangedEvent>)EditModeCheckBox_CheckedChanged);
			BuildAllCurrencyControls();
			((View<IPresenter>)this).Build(buildPanel);
		}

		public void ShowLoading(bool isLoading)
		{
			if (_loadingLabel != null)
			{
				((Control)_loadingLabel).set_Visible(isLoading);
			}
			if (_errorLabel != null)
			{
				((Control)_errorLabel).set_Visible(false);
			}
			if (_panel != null)
			{
				((Control)_panel).set_Visible(!isLoading);
			}
		}

		public void RebuildControls()
		{
			foreach (AccountCurrencyControl value in _allCurrencyControls.Values)
			{
				value.VisibilityChanged -= OnCurrencyControlVisibilityChanged;
				((Control)value).Dispose();
			}
			_allCurrencyControls.Clear();
			_controlsBuilt = false;
			BuildAllCurrencyControls();
		}

		public void ShowError(string message)
		{
			if (_errorLabel != null)
			{
				_errorLabel.set_Text(message);
				((Control)_errorLabel).set_Visible(true);
			}
			if (_loadingLabel != null)
			{
				((Control)_loadingLabel).set_Visible(false);
			}
			if (_panel != null)
			{
				((Control)_panel).set_Visible(false);
			}
		}

		private void EditModeCheckBox_CheckedChanged(object sender, CheckChangedEvent e)
		{
			foreach (AccountCurrencyControl value in _allCurrencyControls.Values)
			{
				value.EditMode = e.get_Checked();
			}
			UpdateControlsVisibility();
		}
	}
}
