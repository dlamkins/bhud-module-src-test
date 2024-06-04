using System;
using System.Linq;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Extended;
using Blish_HUD.Graphics.UI;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ProofLogix.Core.UI
{
	internal class ItemTooltipView : View
	{
		private int _amount;

		private int _itemId;

		private Texture2D _icon;

		private Item _item;

		public ItemTooltipView(int itemId, int amount)
			: this()
		{
			_itemId = itemId;
			_amount = amount;
		}

		protected override async Task<bool> Load(IProgress<string> progress)
		{
			progress.Report("Loading…");
			_item = (await ProofLogix.Instance.Gw2WebApi.GetItems(_itemId)).FirstOrDefault();
			if (_item != null)
			{
				_icon = AsyncTexture2D.op_Implicit(GameService.Content.GetRenderServiceTexture(RenderUrl.op_Implicit(_item.get_Icon())));
			}
			progress.Report(null);
			return _item != null;
		}

		protected override void Build(Container buildPanel)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			string labelText = " " + AssetUtil.GetItemDisplayName(_item.get_Name(), _amount, brackets: false);
			Point labelSize = LabelUtil.GetLabelSize((FontSize)20, labelText, hasPrefix: true);
			((Control)new FormattedLabelBuilder().SetWidth(labelSize.X + 10).SetHeight(labelSize.Y + 10).SetVerticalAlignment((VerticalAlignment)0)
				.CreatePart(labelText, (Action<FormattedLabelPartBuilder>)delegate(FormattedLabelPartBuilder o)
				{
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					o.SetPrefixImage(AsyncTexture2D.op_Implicit(_icon));
					o.SetPrefixImageSize(new Point(32, 32));
					o.SetFontSize((FontSize)20);
					o.SetTextColor(_item.get_Rarity().get_Value().AsColor());
					o.MakeBold();
				})
				.Build()).set_Parent(buildPanel);
			((View<IPresenter>)this).Build(buildPanel);
		}
	}
}
