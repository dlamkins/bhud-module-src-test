using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using Neokain.GW2.AllianceManager.Utils;

namespace Neokain.GW2.AllianceManager.Views
{
	internal class AccountCurrencyControl : Panel
	{
		private int _amount;

		private readonly Currency _currency;

		private readonly Module _module;

		private readonly Checkbox _checkboxEnabled;

		private readonly Image _currencyIcon;

		private readonly Label _currencyName;

		private readonly Label _currencyAmount;

		private bool _editMode = true;

		public bool Checked => _checkboxEnabled.get_Checked();

		public bool EditMode
		{
			get
			{
				return _editMode;
			}
			set
			{
				_editMode = value;
				((Control)this).RecalculateLayout();
			}
		}

		public event Action<int, bool> VisibilityChanged;

		public AccountCurrencyControl(int amount, Currency currency, Module module)
			: this()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			_amount = amount;
			_currency = currency;
			_module = module ?? throw new ArgumentNullException("module");
			Checkbox val = new Checkbox();
			((Control)val).set_Parent((Container)(object)this);
			val.set_Checked(true);
			((Control)val).set_Top(7);
			val.set_Text("");
			((Control)val).set_Width(20);
			_checkboxEnabled = val;
			_checkboxEnabled.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnCheckboxChanged);
			try
			{
				Image val2 = new Image(GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(_currency.get_Icon())));
				((Control)val2).set_Parent((Container)(object)this);
				((Control)val2).set_Top(5);
				((Control)val2).set_Width(32);
				((Control)val2).set_Height(32);
				_currencyIcon = val2;
			}
			catch (Exception)
			{
				Image val3 = new Image(AsyncTexture2D.op_Implicit(Textures.get_Error()));
				((Control)val3).set_Parent((Container)(object)this);
				((Control)val3).set_Top(5);
				((Control)val3).set_Width(32);
				((Control)val3).set_Height(32);
				_currencyIcon = val3;
			}
			Label val4 = new Label();
			((Control)val4).set_Parent((Container)(object)this);
			((Control)val4).set_Top(5);
			val4.set_AutoSizeWidth(true);
			((Control)val4).set_Height(24);
			val4.set_TextColor(Color.get_White());
			val4.set_Text(GetFormattedAmount());
			val4.set_Font((BitmapFont)(object)_module.FontService.DejaVuSans18);
			_currencyAmount = val4;
			Label val5 = new Label();
			((Control)val5).set_Parent((Container)(object)this);
			((Control)val5).set_Top(5);
			val5.set_AutoSizeWidth(true);
			((Control)val5).set_Height(24);
			val5.set_TextColor(Color.get_White());
			val5.set_Text(_currency.get_Name());
			val5.set_Font((BitmapFont)(object)_module.FontService.DejaVuSans18);
			_currencyName = val5;
		}

		private void OnCheckboxChanged(object sender, CheckChangedEvent e)
		{
			this.VisibilityChanged?.Invoke(_currency.get_Id(), e.get_Checked());
		}

		public void SetCheckedState(bool isChecked)
		{
			_checkboxEnabled.remove_CheckedChanged((EventHandler<CheckChangedEvent>)OnCheckboxChanged);
			_checkboxEnabled.set_Checked(isChecked);
			_checkboxEnabled.add_CheckedChanged((EventHandler<CheckChangedEvent>)OnCheckboxChanged);
		}

		public void UpdateAmount(int newAmount)
		{
			_amount = newAmount;
			_currencyAmount.set_Text(GetFormattedAmount());
		}

		private string GetFormattedAmount()
		{
			return CurrencyFormatter.FormatCurrency(_currency.get_Id(), _amount);
		}

		protected override void OnShown(EventArgs e)
		{
			((Control)this).RecalculateLayout();
			((Control)this).OnShown(e);
		}

		public override void RecalculateLayout()
		{
			if (_checkboxEnabled != null && _currencyIcon != null && _currencyAmount != null && _currencyName != null)
			{
				int left = 5;
				((Control)_checkboxEnabled).set_Visible(EditMode);
				if (EditMode)
				{
					((Control)_checkboxEnabled).set_Left(left);
					left = ((Control)_checkboxEnabled).get_Right();
				}
				((Control)_currencyIcon).set_Left(left);
				left = ((Control)_currencyIcon).get_Right() + 5;
				((Control)_currencyAmount).set_Left(left);
				left = ((Control)_currencyAmount).get_Right() + 5;
				((Control)_currencyName).set_Left(left);
				left = ((Control)_currencyName).get_Right() + 5;
				((Panel)this).RecalculateLayout();
			}
		}
	}
}
