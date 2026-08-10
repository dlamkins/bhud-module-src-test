using System;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Neokain.GW2.AllianceManager.Views;

namespace Neokain.GW2.AllianceManager.Windows
{
	internal class CurrencyWindow : StandardWindow
	{
		private readonly Module _module;

		private CurrencyView _currencyView;

		private bool _dataLoadAttempted;

		public CurrencyWindow(Module module)
			: this(Textures.get_Pixel(), new Rectangle(0, 0, 313, 691), new Rectangle(10, 10, 293, 671))
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			_module = module ?? throw new ArgumentNullException("module");
			((Control)this).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((Control)this).set_Visible(false);
			((WindowBase2)this).set_Title("Currencies");
			((WindowBase2)this).set_SavesPosition(true);
			((WindowBase2)this).set_SavesSize(true);
			((WindowBase2)this).set_Id("Neokain_GW2_AllianceManager_currenciesWindow");
			((Control)this).set_Top(100);
			((Control)this).set_Left(100);
			((Control)this).set_BackgroundColor(Color.get_Black());
			((WindowBase2)this).set_CanCloseWithEscape(false);
			((WindowBase2)this).set_CanClose(!_module.MakeUnclosable.get_Value());
			((WindowBase2)this).set_CanResize(!_module.MakeNonResizable.get_Value());
			_module.MakeUnclosable.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMakeUnclosableChanged);
			_module.MakeNonResizable.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMakeNonResizableChanged);
		}

		public override void Show()
		{
			if (_currencyView == null)
			{
				_currencyView = new CurrencyView(_module);
			}
			if (((WindowBase2)this).get_CurrentView() == null)
			{
				((StandardWindow)this).Show((IView)(object)_currencyView);
			}
			else
			{
				((WindowBase2)this).Show();
			}
			if (!_dataLoadAttempted && !_module.IsCurrencyDataLoaded)
			{
				_dataLoadAttempted = true;
				LoadCurrencyDataAsync();
			}
		}

		private async Task LoadCurrencyDataAsync()
		{
			_currencyView?.ShowLoading(isLoading: true);
			bool num = await _module.LoadCurrencyDataAsync();
			_currencyView?.ShowLoading(isLoading: false);
			if (num)
			{
				_currencyView?.RebuildControls();
			}
			else
			{
				_currencyView?.ShowError("Failed to load currency data. Please try again later.");
			}
		}

		private void OnMakeUnclosableChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			((WindowBase2)this).set_CanClose(!e.get_NewValue());
		}

		private void OnMakeNonResizableChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			((WindowBase2)this).set_CanResize(!e.get_NewValue());
		}

		protected override void DisposeControl()
		{
			if (_module != null)
			{
				_module.MakeUnclosable.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMakeUnclosableChanged);
				_module.MakeNonResizable.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnMakeNonResizableChanged);
			}
			((WindowBase2)this).DisposeControl();
		}
	}
}
