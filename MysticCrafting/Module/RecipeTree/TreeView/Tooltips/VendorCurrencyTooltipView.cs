using System.Collections.Generic;
using System.Linq;
using Atzie.MysticCrafting.Models.Currencies;
using Blish_HUD;
using Blish_HUD.Common.UI.Views;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using MysticCrafting.Module.Extensions;
using MysticCrafting.Module.Services;
using MysticCrafting.Module.Strings;

namespace MysticCrafting.Module.RecipeTree.TreeView.Tooltips
{
	public class VendorCurrencyTooltipView : View, ICountTooltipView, ITooltipView, IView
	{
		private CurrencyQuantity Quantity { get; set; }

		private IList<Control> _controls { get; set; } = new List<Control>();


		public Point IconSize { get; set; } = new Point(30, 30);


		public int RequiredQuantity { get; set; }

		public VendorCurrencyTooltipView(CurrencyQuantity quantity)
			: this()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Quantity = quantity;
			BuildControls();
		}

		private void BuildControls()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			if (Quantity.Currency?.Icon != null)
			{
				AsyncTexture2D texture = ServiceContainer.TextureRepository.GetTexture(Quantity.Currency.Icon);
				if (texture != null)
				{
					IList<Control> controls = _controls;
					Image val = new Image(texture);
					((Control)val).set_Size(IconSize);
					((Control)val).set_Location(new Point(0, 0));
					controls.Add((Control)val);
				}
			}
			IList<Control> controls2 = _controls;
			Label val2 = new Label();
			val2.set_Text(Quantity.Currency?.Name);
			val2.set_Font(GameService.Content.get_DefaultFont18());
			((Control)val2).set_Location(new Point(40, 5));
			val2.set_TextColor(ColorHelper.CurrencyName);
			val2.set_StrokeText(true);
			val2.set_AutoSizeWidth(true);
			controls2.Add((Control)val2);
			Label val3 = new Label();
			val3.set_Text(Quantity.Currency?.LocalizedDescription());
			val3.set_Font(GameService.Content.get_DefaultFont14());
			((Control)val3).set_Location(new Point(0, 35));
			val3.set_TextColor(Color.get_White());
			((Control)val3).set_Width(400);
			val3.set_AutoSizeHeight(true);
			val3.set_WrapText(true);
			val3.set_StrokeText(true);
			Label descriptionLabel = val3;
			_controls.Add((Control)(object)descriptionLabel);
			IList<Control> controls3 = _controls;
			Label val4 = new Label();
			val4.set_Text(Recipe.WalletLabel);
			val4.set_Font(GameService.Content.get_DefaultFont16());
			((Control)val4).set_Location(new Point(0, ((Control)descriptionLabel).get_Bottom() + 5));
			val4.set_TextColor(Color.get_LightGray());
			val4.set_ShowShadow(true);
			val4.set_AutoSizeWidth(true);
			controls3.Add((Control)val4);
		}

		protected override void Build(Container buildPanel)
		{
			foreach (Control control in _controls.ToList())
			{
				if (control != null)
				{
					control.set_Parent(buildPanel);
				}
			}
		}

		protected override void Unload()
		{
			foreach (Control control in _controls)
			{
				control.Dispose();
			}
			_controls?.Clear();
			Quantity = null;
			((View<IPresenter>)this).Unload();
		}

		public void UpdateLinkedNodes()
		{
		}
	}
}
