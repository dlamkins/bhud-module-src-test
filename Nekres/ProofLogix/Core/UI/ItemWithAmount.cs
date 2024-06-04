using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Nekres.ProofLogix.Core.Services.KpWebApi.V2.Models;

namespace Nekres.ProofLogix.Core.UI
{
	internal class ItemWithAmount : Image
	{
		private int _amount;

		private Color _borderColor;

		private BitmapFont _font = GameService.Content.get_DefaultFont16();

		private readonly Color _amountColor = new Color(255, 247, 169);

		private SpriteBatchParameters _grayscale;

		private SpriteBatchParameters _defaultSpriteBatchParameters;

		public int Amount
		{
			get
			{
				return _amount;
			}
			set
			{
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				if (((Control)this).SetProperty<int>(ref _amount, value, false, "Amount"))
				{
					if (_amount > 0)
					{
						((Image)this).set_Tint(Color.get_White());
						((Control)this).set_Opacity(1f);
					}
					else
					{
						((Image)this).set_Tint(Color.get_White() * 0.5f);
						((Control)this).set_Opacity(0.5f);
					}
				}
			}
		}

		public Color BorderColor
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _borderColor;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).SetProperty<Color>(ref _borderColor, value, false, "BorderColor");
			}
		}

		public BitmapFont Font
		{
			get
			{
				return _font;
			}
			set
			{
				((Control)this).SetProperty<BitmapFont>(ref _font, value, false, "Font");
			}
		}

		private ItemWithAmount(AsyncTexture2D icon)
			: this(icon)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			_defaultSpriteBatchParameters = ((Control)this).get_SpriteBatchParameters();
			SpriteBatchParameters val = new SpriteBatchParameters((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, (Matrix?)null);
			val.set_Effect(ProofLogix.Instance.ContentsManager.GetEffect<Effect>("effects\\grayscale.mgfx"));
			_grayscale = val;
			((Control)this).set_SpriteBatchParameters(_grayscale);
		}

		public static ItemWithAmount Create(int id, int amount)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			Resource resource = ProofLogix.Instance.Resources.GetItem(id);
			Tooltip tooltip = new Tooltip();
			string labelText = " " + AssetUtil.GetItemDisplayName(resource.Name, amount, brackets: false);
			Point labelSize = LabelUtil.GetLabelSize((FontSize)20, labelText, hasPrefix: true);
			((Control)new FormattedLabelBuilder().SetWidth(labelSize.X).SetHeight(labelSize.Y + 10).SetVerticalAlignment((VerticalAlignment)0)
				.CreatePart(labelText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					o.SetPrefixImage(resource.Icon);
					o.SetPrefixImageSize(new Point(32, 32));
					o.SetFontSize((FontSize)20);
					o.SetTextColor(resource.Rarity.AsColor());
				})
				.Build()).set_Parent((Container)(object)tooltip);
			ItemWithAmount itemWithAmount = new ItemWithAmount(resource.Icon);
			((Control)itemWithAmount).set_Width(64);
			((Control)itemWithAmount).set_Height(64);
			itemWithAmount.Amount = amount;
			((Control)itemWithAmount).set_Tooltip(tooltip);
			itemWithAmount.BorderColor = resource.Rarity.AsColor();
			return itemWithAmount;
		}

		protected override void DisposeControl()
		{
			Effect effect = _grayscale.get_Effect();
			if (effect != null)
			{
				((GraphicsResource)effect).Dispose();
			}
			((Control)this).DisposeControl();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			if (_grayscale.get_Effect() != null)
			{
				_grayscale.get_Effect().get_Parameters().get_Item("Intensity")
					.SetValue(Convert.ToSingle(Amount <= 0));
				_grayscale.get_Effect().get_Parameters().get_Item("Opacity")
					.SetValue(((Control)this).get_Opacity());
			}
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _grayscale);
			((Image)this).Paint(spriteBatch, bounds);
			spriteBatch.End();
			SpriteBatchExtensions.Begin(spriteBatch, _defaultSpriteBatchParameters);
			if (Amount > 0)
			{
				spriteBatch.DrawRectangleOnCtrl((Control)(object)this, bounds, 2, BorderColor);
			}
			if (Amount > 1)
			{
				string text = Amount.ToString();
				Rectangle dest = default(Rectangle);
				((Rectangle)(ref dest))._002Ector(-6, 2, bounds.Width, bounds.Height);
				SpriteBatchExtensions.DrawStringOnCtrl(spriteBatch, (Control)(object)this, text, Font, dest, _amountColor, false, true, 2, (HorizontalAlignment)2, (VerticalAlignment)0);
			}
		}
	}
}
